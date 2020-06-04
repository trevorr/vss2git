using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;
using Hpdi.VssLogicalLib;

namespace Hpdi.Vss2Git
{
	public partial class MainForm
	{
		private class RunInfo : IDisposable
		{
			private readonly MainForm form;
			private readonly string outputDir;
			private readonly string processDir;
			private readonly string logFile;
			private readonly Logger commonLogger;
			private readonly VssDatabase db;
			private readonly Encoding selectedEncoding;
			private readonly Logger errorLogger;
			private readonly WorkQueue workQueue;
			private readonly Queue<string> toProcess;
			private RepoInfo repoInfo;

			public RevisionAnalyzer RevisionAnalyzer => this.repoInfo?.RevisionAnalyzer;
			public ChangesetBuilder ChangesetBuilder => this.repoInfo?.ChangesetBuilder;

			private const string PROCESS_DIR_NAME = "_p";

			public RunInfo(
				MainForm form,
				string outputDir,
				Logger commonLogger,
				VssDatabase db,
				Encoding selectedEncoding,
				Logger errorLogger,
				WorkQueue workQueue,
				Queue<string> toProcess)
			{
				this.form = form;
				this.outputDir = outputDir;
				this.processDir = Path.Combine(outputDir, PROCESS_DIR_NAME);
				this.logFile = Path.Combine(this.outputDir, PROCESS_DIR_NAME + ".log");
				this.commonLogger = commonLogger;
				this.db = db;
				this.selectedEncoding = selectedEncoding;
				this.errorLogger = errorLogger;
				this.workQueue = workQueue;
				this.toProcess = toProcess;
			}

			public bool StartNext()
			{
				if (this.toProcess.Count <= 0) return false;

				string vssPath = this.toProcess.Dequeue();

				this.Process(vssPath);
				return true;
			}

			private void Process(string vssPath)
			{
				// clear processing folder and delete processing log file
				this.DeleteProcessingFolderAndLog();

				Logger logger = new Logger(this.logFile, this.commonLogger);

				try
				{
					this.ProcessPath(vssPath, logger);
				}
				catch (Exception ex)
				{
					string msg = $"ERROR: {vssPath}\r\n{ex}";

					this.errorLogger.WriteLine(msg);
					logger.WriteLine(msg);
				}
			}

			private void ProcessPath(string vssPath, Logger logger)
			{
				VssItem item;
				try
				{
					item = this.db.GetItem(vssPath);
				}
				catch (VssPathException ex)
				{
					MessageBox.Show(ex.Message, "Invalid project path", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				VssProject project = (VssProject)item;
				if (project == null)
				{
					MessageBox.Show(vssPath + " is not a project", "Invalid project path", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				this.repoInfo = new RepoInfo();
				this.repoInfo.VssPath = vssPath;
				this.repoInfo.Logger = logger;

				this.repoInfo.RevisionAnalyzer = new RevisionAnalyzer(this.workQueue, logger, this.db);
				if (!string.IsNullOrEmpty(this.form.excludeTextBox.Text))
				{
					this.repoInfo.RevisionAnalyzer.ExcludeFiles = this.form.excludeTextBox.Text;
				}
				this.repoInfo.RevisionAnalyzer.AddItem(project);

				this.repoInfo.ChangesetBuilder = new ChangesetBuilder(this.workQueue, logger, this.repoInfo.RevisionAnalyzer);
				this.repoInfo.ChangesetBuilder.AnyCommentThreshold = TimeSpan.FromSeconds((double)this.form.anyCommentUpDown.Value);
				this.repoInfo.ChangesetBuilder.SameCommentThreshold = TimeSpan.FromSeconds((double)this.form.sameCommentUpDown.Value);
				this.repoInfo.ChangesetBuilder.BuildChangesets();

				GitExporter gitExporter = new GitExporter(this.workQueue, logger, this.repoInfo.RevisionAnalyzer, this.repoInfo.ChangesetBuilder);
				if (!string.IsNullOrEmpty(this.form.domainTextBox.Text))
				{
					gitExporter.EmailDomain = this.form.domainTextBox.Text;
				}
				if (!string.IsNullOrEmpty(this.form.commentTextBox.Text))
				{
					gitExporter.DefaultComment = this.form.commentTextBox.Text;
				}
				if (!this.form.transcodeCheckBox.Checked)
				{
					gitExporter.CommitEncoding = this.selectedEncoding;
				}
				gitExporter.IgnoreErrors = this.form.ignoreErrorsCheckBox.Checked;
				gitExporter.ExportToGit(processDir);
			}

			private void DeleteProcessingFolderAndLog()
			{
				// clear processing folder
				try
				{
					if (Directory.Exists(this.processDir))
					{
						ClearDir(new DirectoryInfo(this.processDir), true);
					}
				}
				catch (Exception ex)
				{
					throw new Exception($"Ошибка при удалении папки '{this.processDir}'.", ex);
				}

				// delete processing log file
				try
				{
					if (File.Exists(this.logFile))
					{
						File.Delete(this.logFile);
					}
				}
				catch (Exception ex)
				{
					throw new Exception($"Ошибка при удалении файла '{this.logFile}'.", ex);
				}
			}

			private static void ClearDir(DirectoryInfo info, bool deleteSelf, Func<string, bool> needDeleteFunc = null)
			{
				foreach (FileInfo file in info.GetFiles())
				{
					if (needDeleteFunc == null || needDeleteFunc(file.Name))
					{
						File.SetAttributes(file.FullName, FileAttributes.Normal);
						file.SetAccessControl(new FileSecurity());
						file.Delete();
					}
				}
				foreach (DirectoryInfo dir in info.GetDirectories())
				{
					if (needDeleteFunc == null || needDeleteFunc(dir.Name))
					{
						ClearDir(dir, true);
					}
				}

				if (deleteSelf)
				{
					info.Delete();
				}
			}

			public void PostProcess()
			{
				if (this.repoInfo == null) return;

				try
				{
					if (this.repoInfo.Canceled) return;

					ClearDir(new DirectoryInfo(this.processDir), false, path => !path.StartsWith(".git"));

					ICollection<Exception> exceptions = this.workQueue.FetchExceptions();
					bool isSuccess = exceptions == null || exceptions.Count == 0;
					string projPath = GetProjectPath(this.outputDir, this.repoInfo.VssPath, isSuccess);

					string moveDir = Path.GetDirectoryName(projPath);
					if (moveDir != null && !Directory.Exists(moveDir))
					{
						Directory.CreateDirectory(moveDir);
					}

					try
					{
						Directory.Move(this.processDir, projPath);
					}
					catch (Exception ex)
					{
						throw new Exception($"Ошибка при перемещении папки '{this.processDir}' в '{projPath}'.", ex);
					}
				
					// move log file
					this.repoInfo.Logger.Dispose();
					this.repoInfo.Logger = null;

					if (File.Exists(this.logFile))
					{
						string newLogPath = Path.Combine(projPath, "_vss2git.log");
						File.Move(this.logFile, newLogPath);
					}
				}
				catch (Exception ex)
				{
					string msg = $"POSTPROCESS ERROR: {this.repoInfo.VssPath}\r\n{ex}";
					if (this.repoInfo.Logger != null)
					{
						this.repoInfo.Logger.WriteLine(msg);
					}
					this.errorLogger.WriteLine(msg);
					throw;
				}
				finally
				{
					if (this.repoInfo != null)
					{
						this.repoInfo.Dispose();
						this.repoInfo = null;
					}
				}
			}

			public string GetProject()
			{
				return this.repoInfo?.VssPath;
			}

			public void SetCancelled()
			{
				if (this.repoInfo != null)
				{
					this.repoInfo.Canceled = true;
				}
			}

			public void Dispose()
			{
				this.errorLogger.Dispose();
				FileInfo fileInfo = new FileInfo(this.errorLogger.Filename);
				if (fileInfo.Length == 0)
				{
					fileInfo.Delete();
				}

				this.commonLogger.Dispose();

				if (this.repoInfo != null)
				{
					this.repoInfo.Dispose();
					this.repoInfo = null;
				}
			}

			private class RepoInfo : IDisposable
			{
				public RevisionAnalyzer RevisionAnalyzer;
				public ChangesetBuilder ChangesetBuilder;
				public Logger Logger;
				public string VssPath;
				public bool Canceled;

				public void Dispose()
				{
					if (this.Logger != null)
					{
						this.Logger.Dispose();
						this.Logger = null;
					}
				}
			}
		}
	}
}

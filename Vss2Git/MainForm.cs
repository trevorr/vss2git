/* Copyright 2009 HPDI, LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;
using Hpdi.VssLogicalLib;

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// Main form for the application.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public partial class MainForm : Form
    {
        private readonly Dictionary<int, EncodingInfo> codePages = new Dictionary<int, EncodingInfo>();
        private readonly WorkQueue workQueue = new WorkQueue(1);
	    private RunInfo runInfo;
		private Encoding selectedEncoding = Encoding.Default;

		public MainForm()
        {
	        this.InitializeComponent();
        }

	    public static string GetProjectPath(string outputDir, string vssPath, bool isSuccess)
	    {
			string moveDir = isSuccess ? "_success" : "_fail";
		    moveDir = Path.Combine(outputDir, moveDir);
		    string projFolder = GetSafeDirName(vssPath);
		    string projPath = Path.Combine(moveDir, projFolder);
		    return projPath;
	    }

		private Logger OpenLog(string filename)
        {
            return string.IsNullOrEmpty(filename) ? Logger.Null : new Logger(filename, null);
        }

		private void goButton_Click(object sender, EventArgs e)
		{
			this.goButton.Enabled = false;
			this.projectsTreeControl.CanCheck = false;

			this.WriteSettings();

			string outputDir;
	        if (this.outDirTextBox.TextLength == 0)
	        {
		        outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
	        }
	        else
	        {
		        outputDir = this.outDirTextBox.Text;
	        }

	        if (!Directory.Exists(outputDir))
	        {
		        Directory.CreateDirectory(outputDir);
	        }

	        string timeStr = DateTime.Now.ToString("yyyyMMddHHmmssfff");

			string commonLogFile = Path.Combine(outputDir, $"_{timeStr}.log");
	        Logger commonLogger = this.OpenLog(commonLogFile);

	        string errorLogFile = Path.Combine(outputDir, $"_{timeStr}_errors.log");
	        Logger errorLogger = this.OpenLog(errorLogFile);

			try
            {
                commonLogger.WriteLine("VSS2Git version {0}", Assembly.GetExecutingAssembly().GetName().Version);

	            Encoding encoding = this.selectedEncoding;
              
                commonLogger.WriteLine("VSS encoding: {0} (CP: {1}, IANA: {2})", encoding.EncodingName, encoding.CodePage, encoding.WebName);
                commonLogger.WriteLine("Comment transcoding: {0}", this.transcodeCheckBox.Checked ? "enabled" : "disabled");
                commonLogger.WriteLine("Ignore errors: {0}", this.ignoreErrorsCheckBox.Checked ? "enabled" : "disabled");

                VssDatabaseFactory df = new VssDatabaseFactory(this.vssDirTextBox.Text);
                df.Encoding = this.selectedEncoding;
                VssDatabase db = df.Open();

	            Queue<string> toProcess = new Queue<string>();
	            foreach (string vssPath in this.projectsTreeControl.SelectedPaths)
	            {
		            string projPath = GetProjectPath(outputDir, vssPath, true);
		            if (!Directory.Exists(projPath))
		            {
			            toProcess.Enqueue(vssPath);
		            }
	            }

				this.runInfo = new RunInfo(this, outputDir, commonLogger, db, encoding, errorLogger, this.workQueue, toProcess);
	          
			    this.statusTimer.Enabled = true;
			}
			catch (Exception ex)
            {
			    string msg = $"GLOBAL ERROR: {ex}";

				errorLogger.WriteLine(msg);
	            commonLogger.WriteLine(msg);

				this.ShowException(ex);
            }
        }

	    private void cancelButton_Click(object sender, EventArgs e)
        {
	        if (this.runInfo != null)
	        {
		        this.runInfo.SetCancelled();
	        }
	        this.workQueue.Abort();
        }

		private void statusTimer_Tick(object sender, EventArgs e)
		{
			if (this.runInfo == null)
			{
				this.projectLabel.Text = "";
			}
			else
			{
				this.projectLabel.Text = this.runInfo.GetProject();
			}
			this.statusLabel.Text = this.workQueue.LastStatus ?? "Idle";
	        this.timeLabel.Text = string.Format("Elapsed: {0:HH:mm:ss}", new DateTime(this.workQueue.ActiveTime.Ticks));

	        if (this.runInfo != null)
	        {
		        if (this.runInfo.RevisionAnalyzer != null)
		        {
			        this.fileLabel.Text = "Files: " + this.runInfo.RevisionAnalyzer.FileCount;
			        this.revisionLabel.Text = "Revisions: " + this.runInfo.RevisionAnalyzer.RevisionCount;
		        }

		        if (this.runInfo.ChangesetBuilder != null)
		        {
			        this.changeLabel.Text = "Changesets: " + this.runInfo.ChangesetBuilder.Changesets.Count;
		        }

		        if (this.workQueue.IsIdle)
		        {
			        this.runInfo.PostProcess();
			        this.projectsTreeControl.UpdateNodes();

					bool isEnd;
			        try
			        {
				        isEnd = !this.runInfo.StartNext();
					}
					catch (Exception ex)
					{
						this.ShowException(ex);
						isEnd = true;
			        }

			        if (isEnd && this.runInfo != null)
			        {
				        this.runInfo.Dispose();
				        this.runInfo = null;

						this.workQueue.Abort();
				        this.statusTimer.Enabled = false;
				        this.goButton.Enabled = true;
						this.projectsTreeControl.CanCheck = true;
					}
				}
	        }
        }

        private void ShowException(Exception exception)
        {
	        bool isKnown;
	        string message = ExceptionFormatter.Format(exception, out isKnown);
            //logger.WriteLine("ERROR: {0}", message);
            //logger.WriteLine(exception);

	        if (!isKnown)
	        {
		        message = exception.ToString();
	        }
	        MessageBox.Show(message, "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text += " " + Assembly.GetExecutingAssembly().GetName().Version;

            int defaultCodePage = Encoding.Default.CodePage;
            string description = string.Format("System default - {0}", Encoding.Default.EncodingName);
            int defaultIndex = this.encodingComboBox.Items.Add(description);
	        this.encodingComboBox.SelectedIndex = defaultIndex;

            EncodingInfo[] encodings = Encoding.GetEncodings();
            foreach (var encoding in encodings)
            {
                int codePage = encoding.CodePage;
                description = string.Format("CP{0} - {1}", codePage, encoding.DisplayName);
                int index = this.encodingComboBox.Items.Add(description);
	            this.codePages[index] = encoding;
                if (codePage == defaultCodePage)
                {
	                this.codePages[defaultIndex] = encoding;
                }
            }

	        this.ReadSettings();

	        if (this.vssDirTextBox.TextLength > 0)
	        {
		        this.projectsTreeControl.RefreshProjects();
	        }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
	        this.WriteSettings();

	        this.workQueue.Abort();
	        this.workQueue.WaitIdle();
        }

        private void ReadSettings()
        {
            var settings = Properties.Settings.Default;
	        this.vssDirTextBox.Text = settings.VssDirectory;
	        this.excludeTextBox.Text = settings.VssExcludePaths;
	        this.outDirTextBox.Text = settings.GitDirectory;
	        this.domainTextBox.Text = settings.DefaultEmailDomain;
	        this.commentTextBox.Text = settings.DefaultComment;
	        this.transcodeCheckBox.Checked = settings.TranscodeComments;
	        this.forceAnnotatedCheckBox.Checked = settings.ForceAnnotatedTags;
	        this.anyCommentUpDown.Value = settings.AnyCommentSeconds;
	        this.sameCommentUpDown.Value = settings.SameCommentSeconds;
	        this.projectsTreeControl.SelectedPaths = settings.Projects;
        }

        private void WriteSettings()
        {
            var settings = Properties.Settings.Default;
            settings.VssDirectory = this.vssDirTextBox.Text;
            settings.VssExcludePaths = this.excludeTextBox.Text;
            settings.GitDirectory = this.outDirTextBox.Text;
            settings.DefaultEmailDomain = this.domainTextBox.Text;
            settings.TranscodeComments = this.transcodeCheckBox.Checked;
            settings.ForceAnnotatedTags = this.forceAnnotatedCheckBox.Checked;
            settings.AnyCommentSeconds = (int)this.anyCommentUpDown.Value;
            settings.SameCommentSeconds = (int)this.sameCommentUpDown.Value;
	        settings.Projects = this.projectsTreeControl.SelectedPaths;
            settings.Save();
        }

	    private static string GetSafeDirName(string name)
	    {
		    name = name.Replace("$/", "");
		    name = name.Replace("/", "_");
		    return name;
	    }

		private void vssDirTextBox_TextChanged(object sender, EventArgs e)
		{
			this.projectsTreeControl.VSSDirectory = this.vssDirTextBox.Text.Trim();
		}

	    private void outDirTextBox_TextChanged(object sender, EventArgs e)
	    {
		    this.projectsTreeControl.OutputDirectory = this.outDirTextBox.Text.Trim();
		}

		private void encodingComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			Encoding encoding = Encoding.Default;
			EncodingInfo encodingInfo;
			if (this.codePages.TryGetValue(this.encodingComboBox.SelectedIndex, out encodingInfo))
			{
				encoding = encodingInfo.GetEncoding();
			}
			this.selectedEncoding = encoding;
			this.projectsTreeControl.Encoding = encoding;
		}

		private void projectsTreeControl_CheckedChanged(object sender, EventArgs e)
		{
			this.WriteSettings();
		}
	}
}

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
using System.Reflection;
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
        private Logger logger = Logger.Null;
        private RevisionAnalyzer revisionAnalyzer;
        private ChangesetBuilder changesetBuilder;

        public MainForm()
        {
            InitializeComponent();
        }

        private void OpenLog(string filename)
        {
            logger = string.IsNullOrEmpty(filename) ? Logger.Null : new Logger(filename);
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenLog(logTextBox.Text);

                logger.WriteLine("VSS2Git version {0}", Assembly.GetExecutingAssembly().GetName().Version);

                WriteSettings();

                Encoding encoding = Encoding.Default;
                EncodingInfo encodingInfo;
                if (codePages.TryGetValue(encodingComboBox.SelectedIndex, out encodingInfo))
                {
                    encoding = encodingInfo.GetEncoding();
                }

                logger.WriteLine("VSS encoding: {0} (CP: {1}, IANA: {2})",
                    encoding.EncodingName, encoding.CodePage, encoding.WebName);
                logger.WriteLine("Comment transcoding: {0}",
                    transcodeCheckBox.Checked ? "enabled" : "disabled");
                logger.WriteLine("Ignore errors: {0}",
                    ignoreErrorsCheckBox.Checked ? "enabled" : "disabled");

                var df = new VssDatabaseFactory(vssDirTextBox.Text);
                df.Encoding = encoding;
                var db = df.Open();

                var path = vssProjectTextBox.Text;
                VssItem item;
                try
                {
                    item = db.GetItem(path);
                }
                catch (VssPathException ex)
                {
                    MessageBox.Show(ex.Message, "Invalid project path",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var project = item as VssProject;
                if (project == null)
                {
                    MessageBox.Show(path + " is not a project", "Invalid project path",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                revisionAnalyzer = new RevisionAnalyzer(workQueue, logger, db);
                if (!string.IsNullOrEmpty(excludeTextBox.Text))
                {
                    revisionAnalyzer.ExcludeFiles = excludeTextBox.Text;
                }
                revisionAnalyzer.AddItem(project);

                changesetBuilder = new ChangesetBuilder(workQueue, logger, revisionAnalyzer);
                changesetBuilder.AnyCommentThreshold = TimeSpan.FromSeconds((double)anyCommentUpDown.Value);
                changesetBuilder.SameCommentThreshold = TimeSpan.FromSeconds((double)sameCommentUpDown.Value);
                changesetBuilder.BuildChangesets();

                if (!string.IsNullOrEmpty(outDirTextBox.Text))
                {
                    var gitExporter = new GitExporter(workQueue, logger,
                        revisionAnalyzer, changesetBuilder);
                    if (!string.IsNullOrEmpty(domainTextBox.Text))
                    {
                        gitExporter.EmailDomain = domainTextBox.Text;
                    }
                    if (!string.IsNullOrEmpty(emailMapFileTextBox.Text))
                    {
                        gitExporter.EmailMapFile = emailMapFileTextBox.Text;
                    }
                    if (!string.IsNullOrEmpty(commentTextBox.Text))
                    {
                        gitExporter.DefaultComment = commentTextBox.Text;
                    }
                    if (!transcodeCheckBox.Checked)
                    {
                        gitExporter.CommitEncoding = encoding;
                    }
                    gitExporter.IgnoreErrors = ignoreErrorsCheckBox.Checked;
                    gitExporter.ExportToGit(outDirTextBox.Text);
                }

                workQueue.Idle += delegate
                {
                    logger.Dispose();
                    logger = Logger.Null;
                };

                statusTimer.Enabled = true;
                goButton.Enabled = false;
            }
            catch (Exception ex)
            {
                logger.Dispose();
                logger = Logger.Null;
                ShowException(ex);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            workQueue.Abort();
        }

        private void statusTimer_Tick(object sender, EventArgs e)
        {
            statusLabel.Text = workQueue.LastStatus ?? "Idle";
            timeLabel.Text = string.Format("Elapsed: {0:HH:mm:ss}",
                new DateTime(workQueue.ActiveTime.Ticks));

            if (revisionAnalyzer != null)
            {
                fileLabel.Text = "Files: " + revisionAnalyzer.FileCount;
                revisionLabel.Text = "Revisions: " + revisionAnalyzer.RevisionCount;
            }

            if (changesetBuilder != null)
            {
                changeLabel.Text = "Changesets: " + changesetBuilder.Changesets.Count;
            }

            if (workQueue.IsIdle)
            {
                revisionAnalyzer = null;
                changesetBuilder = null;

                statusTimer.Enabled = false;
                goButton.Enabled = true;
            }

            var exceptions = workQueue.FetchExceptions();
            if (exceptions != null)
            {
                foreach (var exception in exceptions)
                {
                    ShowException(exception);
                }
            }
        }

        private void ShowException(Exception exception)
        {
            var message = ExceptionFormatter.Format(exception);
            logger.WriteLine("ERROR: {0}", message);
            logger.WriteLine(exception);

            MessageBox.Show(message, "Unhandled Exception",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text += " " + Assembly.GetExecutingAssembly().GetName().Version;

            var defaultCodePage = Encoding.Default.CodePage;
            var description = string.Format("System default - {0}", Encoding.Default.EncodingName);
            var defaultIndex = encodingComboBox.Items.Add(description);
            encodingComboBox.SelectedIndex = defaultIndex;

            var encodings = Encoding.GetEncodings();
            foreach (var encoding in encodings)
            {
                var codePage = encoding.CodePage;
                description = string.Format("CP{0} - {1}", codePage, encoding.DisplayName);
                var index = encodingComboBox.Items.Add(description);
                codePages[index] = encoding;
                if (codePage == defaultCodePage)
                {
                    codePages[defaultIndex] = encoding;
                }
            }

            ReadSettings();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteSettings();

            workQueue.Abort();
            workQueue.WaitIdle();
        }

        private void ReadSettings()
        {
            var settings = Properties.Settings.Default;
            vssDirTextBox.Text = settings.VssDirectory;
            vssProjectTextBox.Text = settings.VssProject;
            excludeTextBox.Text = settings.VssExcludePaths;
            outDirTextBox.Text = settings.GitDirectory;
            domainTextBox.Text = settings.DefaultEmailDomain;
            emailMapFileTextBox.Text = settings.EmailMapFile;
            commentTextBox.Text = settings.DefaultComment;
            logTextBox.Text = settings.LogFile;
            transcodeCheckBox.Checked = settings.TranscodeComments;
            forceAnnotatedCheckBox.Checked = settings.ForceAnnotatedTags;
            anyCommentUpDown.Value = settings.AnyCommentSeconds;
            sameCommentUpDown.Value = settings.SameCommentSeconds;
        }

        private void WriteSettings()
        {
            var settings = Properties.Settings.Default;
            settings.VssDirectory = vssDirTextBox.Text;
            settings.VssProject = vssProjectTextBox.Text;
            settings.VssExcludePaths = excludeTextBox.Text;
            settings.GitDirectory = outDirTextBox.Text;
            settings.DefaultEmailDomain = domainTextBox.Text;
            settings.EmailMapFile = emailMapFileTextBox.Text;
            settings.LogFile = logTextBox.Text;
            settings.TranscodeComments = transcodeCheckBox.Checked;
            settings.ForceAnnotatedTags = forceAnnotatedCheckBox.Checked;
            settings.AnyCommentSeconds = (int)anyCommentUpDown.Value;
            settings.SameCommentSeconds = (int)sameCommentUpDown.Value;
            settings.Save();
        }
    }
}

namespace Hpdi.Vss2Git
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.vssGroupBox = new System.Windows.Forms.GroupBox();
            this.vssDirButton = new System.Windows.Forms.Button();
            this.encodingLabel = new System.Windows.Forms.Label();
            this.encodingComboBox = new System.Windows.Forms.ComboBox();
            this.excludeTextBox = new System.Windows.Forms.TextBox();
            this.excludeLabel = new System.Windows.Forms.Label();
            this.vssProjectTextBox = new System.Windows.Forms.TextBox();
            this.vssDirTextBox = new System.Windows.Forms.TextBox();
            this.vssProjectLabel = new System.Windows.Forms.Label();
            this.vssDirLabel = new System.Windows.Forms.Label();
            this.goButton = new System.Windows.Forms.Button();
            this.statusTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.fileLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.revisionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.changeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.outputGroupBox = new System.Windows.Forms.GroupBox();
            this.outDirButton = new System.Windows.Forms.Button();
            this.resetRepoCheckBox = new System.Windows.Forms.CheckBox();
            this.vcsSetttingsTabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.forceAnnotatedCheckBox = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.svnRepoButton = new System.Windows.Forms.Button();
            this.svnProjectPathTextBox = new System.Windows.Forms.TextBox();
            this.svnProjectPathLabel = new System.Windows.Forms.Label();
            this.svnBranchesTextBox = new System.Windows.Forms.TextBox();
            this.svnTagsTextBox = new System.Windows.Forms.TextBox();
            this.svnTrunkTextBox = new System.Windows.Forms.TextBox();
            this.svnBranchesLabel = new System.Windows.Forms.Label();
            this.svnTagsLabel = new System.Windows.Forms.Label();
            this.svnTrunkLabel = new System.Windows.Forms.Label();
            this.svnStandardLayoutCheckBox = new System.Windows.Forms.CheckBox();
            this.svnRepoTextBox = new System.Windows.Forms.TextBox();
            this.svnRepoLabel = new System.Windows.Forms.Label();
            this.svnUserLabel = new System.Windows.Forms.Label();
            this.svnPasswordLabel = new System.Windows.Forms.Label();
            this.svnPasswordTextBox = new System.Windows.Forms.TextBox();
            this.svnUserTextBox = new System.Windows.Forms.TextBox();
            this.outKindLabel = new System.Windows.Forms.Label();
            this.transcodeCheckBox = new System.Windows.Forms.CheckBox();
            this.domainTextBox = new System.Windows.Forms.TextBox();
            this.domainLabel = new System.Windows.Forms.Label();
            this.outDirTextBox = new System.Windows.Forms.TextBox();
            this.outDirLabel = new System.Windows.Forms.Label();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.logLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.changesetGroupBox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.sameCommentUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.anyCommentUpDown = new System.Windows.Forms.NumericUpDown();
            this.vssDirBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.outDirBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.svnRepoBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.settingsOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.settingsSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveSettingsButton = new System.Windows.Forms.Button();
            this.loadSettingsButton = new System.Windows.Forms.Button();
            this.vssGroupBox.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.outputGroupBox.SuspendLayout();
            this.vcsSetttingsTabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.changesetGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sameCommentUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.anyCommentUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // vssGroupBox
            // 
            this.vssGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vssGroupBox.Controls.Add(this.vssDirButton);
            this.vssGroupBox.Controls.Add(this.encodingLabel);
            this.vssGroupBox.Controls.Add(this.encodingComboBox);
            this.vssGroupBox.Controls.Add(this.excludeTextBox);
            this.vssGroupBox.Controls.Add(this.excludeLabel);
            this.vssGroupBox.Controls.Add(this.vssProjectTextBox);
            this.vssGroupBox.Controls.Add(this.vssDirTextBox);
            this.vssGroupBox.Controls.Add(this.vssProjectLabel);
            this.vssGroupBox.Controls.Add(this.vssDirLabel);
            this.vssGroupBox.Location = new System.Drawing.Point(12, 12);
            this.vssGroupBox.Name = "vssGroupBox";
            this.vssGroupBox.Size = new System.Drawing.Size(560, 126);
            this.vssGroupBox.TabIndex = 0;
            this.vssGroupBox.TabStop = false;
            this.vssGroupBox.Text = "VSS Settings";
            // 
            // vssDirButton
            // 
            this.vssDirButton.Image = ((System.Drawing.Image)(resources.GetObject("vssDirButton.Image")));
            this.vssDirButton.Location = new System.Drawing.Point(531, 17);
            this.vssDirButton.Name = "vssDirButton";
            this.vssDirButton.Size = new System.Drawing.Size(23, 23);
            this.vssDirButton.TabIndex = 2;
            this.vssDirButton.UseVisualStyleBackColor = true;
            this.vssDirButton.Click += new System.EventHandler(this.vssDirButton_Click);
            // 
            // encodingLabel
            // 
            this.encodingLabel.AutoSize = true;
            this.encodingLabel.Location = new System.Drawing.Point(6, 100);
            this.encodingLabel.Name = "encodingLabel";
            this.encodingLabel.Size = new System.Drawing.Size(52, 13);
            this.encodingLabel.TabIndex = 7;
            this.encodingLabel.Text = "Encoding";
            // 
            // encodingComboBox
            // 
            this.encodingComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.encodingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encodingComboBox.FormattingEnabled = true;
            this.encodingComboBox.Location = new System.Drawing.Point(90, 97);
            this.encodingComboBox.Name = "encodingComboBox";
            this.encodingComboBox.Size = new System.Drawing.Size(464, 21);
            this.encodingComboBox.TabIndex = 8;
            // 
            // excludeTextBox
            // 
            this.excludeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.excludeTextBox.Location = new System.Drawing.Point(90, 71);
            this.excludeTextBox.Name = "excludeTextBox";
            this.excludeTextBox.Size = new System.Drawing.Size(464, 20);
            this.excludeTextBox.TabIndex = 6;
            // 
            // excludeLabel
            // 
            this.excludeLabel.AutoSize = true;
            this.excludeLabel.Location = new System.Drawing.Point(6, 74);
            this.excludeLabel.Name = "excludeLabel";
            this.excludeLabel.Size = new System.Drawing.Size(66, 13);
            this.excludeLabel.TabIndex = 5;
            this.excludeLabel.Text = "Exclude files";
            // 
            // vssProjectTextBox
            // 
            this.vssProjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vssProjectTextBox.Location = new System.Drawing.Point(90, 45);
            this.vssProjectTextBox.Name = "vssProjectTextBox";
            this.vssProjectTextBox.Size = new System.Drawing.Size(464, 20);
            this.vssProjectTextBox.TabIndex = 4;
            // 
            // vssDirTextBox
            // 
            this.vssDirTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vssDirTextBox.Location = new System.Drawing.Point(90, 19);
            this.vssDirTextBox.Name = "vssDirTextBox";
            this.vssDirTextBox.Size = new System.Drawing.Size(435, 20);
            this.vssDirTextBox.TabIndex = 1;
            // 
            // vssProjectLabel
            // 
            this.vssProjectLabel.AutoSize = true;
            this.vssProjectLabel.Location = new System.Drawing.Point(6, 48);
            this.vssProjectLabel.Name = "vssProjectLabel";
            this.vssProjectLabel.Size = new System.Drawing.Size(40, 13);
            this.vssProjectLabel.TabIndex = 3;
            this.vssProjectLabel.Text = "Project";
            // 
            // vssDirLabel
            // 
            this.vssDirLabel.AutoSize = true;
            this.vssDirLabel.Location = new System.Drawing.Point(6, 22);
            this.vssDirLabel.Name = "vssDirLabel";
            this.vssDirLabel.Size = new System.Drawing.Size(49, 13);
            this.vssDirLabel.TabIndex = 0;
            this.vssDirLabel.Text = "Directory";
            // 
            // goButton
            // 
            this.goButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.goButton.Location = new System.Drawing.Point(416, 545);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(75, 23);
            this.goButton.TabIndex = 3;
            this.goButton.Text = "Go!";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // statusTimer
            // 
            this.statusTimer.Tick += new System.EventHandler(this.statusTimer_Tick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.fileLabel,
            this.revisionLabel,
            this.changeLabel,
            this.timeLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 576);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(584, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(284, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "Idle";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fileLabel
            // 
            this.fileLabel.Name = "fileLabel";
            this.fileLabel.Size = new System.Drawing.Size(42, 17);
            this.fileLabel.Text = "Files: 0";
            // 
            // revisionLabel
            // 
            this.revisionLabel.Name = "revisionLabel";
            this.revisionLabel.Size = new System.Drawing.Size(68, 17);
            this.revisionLabel.Text = "Revisions: 0";
            // 
            // changeLabel
            // 
            this.changeLabel.Name = "changeLabel";
            this.changeLabel.Size = new System.Drawing.Size(80, 17);
            this.changeLabel.Text = "Changesets: 0";
            // 
            // timeLabel
            // 
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(95, 17);
            this.timeLabel.Text = "Elapsed: 00:00:00";
            // 
            // outputGroupBox
            // 
            this.outputGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputGroupBox.Controls.Add(this.outDirButton);
            this.outputGroupBox.Controls.Add(this.resetRepoCheckBox);
            this.outputGroupBox.Controls.Add(this.vcsSetttingsTabs);
            this.outputGroupBox.Controls.Add(this.outKindLabel);
            this.outputGroupBox.Controls.Add(this.transcodeCheckBox);
            this.outputGroupBox.Controls.Add(this.domainTextBox);
            this.outputGroupBox.Controls.Add(this.domainLabel);
            this.outputGroupBox.Controls.Add(this.outDirTextBox);
            this.outputGroupBox.Controls.Add(this.outDirLabel);
            this.outputGroupBox.Controls.Add(this.logTextBox);
            this.outputGroupBox.Controls.Add(this.logLabel);
            this.outputGroupBox.Location = new System.Drawing.Point(12, 138);
            this.outputGroupBox.Name = "outputGroupBox";
            this.outputGroupBox.Size = new System.Drawing.Size(560, 320);
            this.outputGroupBox.TabIndex = 1;
            this.outputGroupBox.TabStop = false;
            this.outputGroupBox.Text = "Output Settings";
            // 
            // outDirButton
            // 
            this.outDirButton.Image = ((System.Drawing.Image)(resources.GetObject("outDirButton.Image")));
            this.outDirButton.Location = new System.Drawing.Point(531, 17);
            this.outDirButton.Name = "outDirButton";
            this.outDirButton.Size = new System.Drawing.Size(23, 23);
            this.outDirButton.TabIndex = 2;
            this.outDirButton.UseVisualStyleBackColor = true;
            this.outDirButton.Click += new System.EventHandler(this.outDirButton_Click);
            // 
            // resetRepoCheckBox
            // 
            this.resetRepoCheckBox.AutoSize = true;
            this.resetRepoCheckBox.Checked = true;
            this.resetRepoCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resetRepoCheckBox.Location = new System.Drawing.Point(306, 98);
            this.resetRepoCheckBox.Name = "resetRepoCheckBox";
            this.resetRepoCheckBox.Size = new System.Drawing.Size(173, 17);
            this.resetRepoCheckBox.TabIndex = 8;
            this.resetRepoCheckBox.Text = "Reset Repository before Export";
            this.resetRepoCheckBox.UseVisualStyleBackColor = true;
            // 
            // vcsSetttingsTabs
            // 
            this.vcsSetttingsTabs.Controls.Add(this.tabPage1);
            this.vcsSetttingsTabs.Controls.Add(this.tabPage2);
            this.vcsSetttingsTabs.Location = new System.Drawing.Point(9, 154);
            this.vcsSetttingsTabs.Name = "vcsSetttingsTabs";
            this.vcsSetttingsTabs.SelectedIndex = 0;
            this.vcsSetttingsTabs.Size = new System.Drawing.Size(545, 159);
            this.vcsSetttingsTabs.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.forceAnnotatedCheckBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(537, 133);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "git";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // forceAnnotatedCheckBox
            // 
            this.forceAnnotatedCheckBox.AutoSize = true;
            this.forceAnnotatedCheckBox.Checked = true;
            this.forceAnnotatedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.forceAnnotatedCheckBox.Location = new System.Drawing.Point(6, 6);
            this.forceAnnotatedCheckBox.Name = "forceAnnotatedCheckBox";
            this.forceAnnotatedCheckBox.Size = new System.Drawing.Size(191, 17);
            this.forceAnnotatedCheckBox.TabIndex = 7;
            this.forceAnnotatedCheckBox.Text = "Force use of annotated tag objects";
            this.forceAnnotatedCheckBox.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.svnRepoButton);
            this.tabPage2.Controls.Add(this.svnProjectPathTextBox);
            this.tabPage2.Controls.Add(this.svnProjectPathLabel);
            this.tabPage2.Controls.Add(this.svnBranchesTextBox);
            this.tabPage2.Controls.Add(this.svnTagsTextBox);
            this.tabPage2.Controls.Add(this.svnTrunkTextBox);
            this.tabPage2.Controls.Add(this.svnBranchesLabel);
            this.tabPage2.Controls.Add(this.svnTagsLabel);
            this.tabPage2.Controls.Add(this.svnTrunkLabel);
            this.tabPage2.Controls.Add(this.svnStandardLayoutCheckBox);
            this.tabPage2.Controls.Add(this.svnRepoTextBox);
            this.tabPage2.Controls.Add(this.svnRepoLabel);
            this.tabPage2.Controls.Add(this.svnUserLabel);
            this.tabPage2.Controls.Add(this.svnPasswordLabel);
            this.tabPage2.Controls.Add(this.svnPasswordTextBox);
            this.tabPage2.Controls.Add(this.svnUserTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(537, 133);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "svn";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // svnRepoButton
            // 
            this.svnRepoButton.Image = ((System.Drawing.Image)(resources.GetObject("svnRepoButton.Image")));
            this.svnRepoButton.Location = new System.Drawing.Point(508, 4);
            this.svnRepoButton.Name = "svnRepoButton";
            this.svnRepoButton.Size = new System.Drawing.Size(23, 23);
            this.svnRepoButton.TabIndex = 2;
            this.svnRepoButton.UseVisualStyleBackColor = true;
            this.svnRepoButton.Click += new System.EventHandler(this.svnRepoButton_Click);
            // 
            // svnProjectPathTextBox
            // 
            this.svnProjectPathTextBox.Location = new System.Drawing.Point(77, 32);
            this.svnProjectPathTextBox.Name = "svnProjectPathTextBox";
            this.svnProjectPathTextBox.Size = new System.Drawing.Size(454, 20);
            this.svnProjectPathTextBox.TabIndex = 4;
            // 
            // svnProjectPathLabel
            // 
            this.svnProjectPathLabel.AutoSize = true;
            this.svnProjectPathLabel.Location = new System.Drawing.Point(6, 35);
            this.svnProjectPathLabel.Name = "svnProjectPathLabel";
            this.svnProjectPathLabel.Size = new System.Drawing.Size(65, 13);
            this.svnProjectPathLabel.TabIndex = 3;
            this.svnProjectPathLabel.Text = "Project Path";
            // 
            // svnBranchesTextBox
            // 
            this.svnBranchesTextBox.Location = new System.Drawing.Point(426, 107);
            this.svnBranchesTextBox.Name = "svnBranchesTextBox";
            this.svnBranchesTextBox.Size = new System.Drawing.Size(100, 20);
            this.svnBranchesTextBox.TabIndex = 15;
            // 
            // svnTagsTextBox
            // 
            this.svnTagsTextBox.Location = new System.Drawing.Point(233, 107);
            this.svnTagsTextBox.Name = "svnTagsTextBox";
            this.svnTagsTextBox.Size = new System.Drawing.Size(100, 20);
            this.svnTagsTextBox.TabIndex = 13;
            // 
            // svnTrunkTextBox
            // 
            this.svnTrunkTextBox.Location = new System.Drawing.Point(77, 107);
            this.svnTrunkTextBox.Name = "svnTrunkTextBox";
            this.svnTrunkTextBox.Size = new System.Drawing.Size(87, 20);
            this.svnTrunkTextBox.TabIndex = 11;
            // 
            // svnBranchesLabel
            // 
            this.svnBranchesLabel.AutoSize = true;
            this.svnBranchesLabel.Location = new System.Drawing.Point(354, 110);
            this.svnBranchesLabel.Name = "svnBranchesLabel";
            this.svnBranchesLabel.Size = new System.Drawing.Size(51, 13);
            this.svnBranchesLabel.TabIndex = 14;
            this.svnBranchesLabel.Text = "branches";
            // 
            // svnTagsLabel
            // 
            this.svnTagsLabel.AutoSize = true;
            this.svnTagsLabel.Location = new System.Drawing.Point(185, 110);
            this.svnTagsLabel.Name = "svnTagsLabel";
            this.svnTagsLabel.Size = new System.Drawing.Size(27, 13);
            this.svnTagsLabel.TabIndex = 12;
            this.svnTagsLabel.Text = "tags";
            // 
            // svnTrunkLabel
            // 
            this.svnTrunkLabel.AutoSize = true;
            this.svnTrunkLabel.Location = new System.Drawing.Point(6, 110);
            this.svnTrunkLabel.Name = "svnTrunkLabel";
            this.svnTrunkLabel.Size = new System.Drawing.Size(31, 13);
            this.svnTrunkLabel.TabIndex = 10;
            this.svnTrunkLabel.Text = "trunk";
            // 
            // svnStandardLayoutCheckBox
            // 
            this.svnStandardLayoutCheckBox.AutoSize = true;
            this.svnStandardLayoutCheckBox.Location = new System.Drawing.Point(77, 84);
            this.svnStandardLayoutCheckBox.Name = "svnStandardLayoutCheckBox";
            this.svnStandardLayoutCheckBox.Size = new System.Drawing.Size(120, 17);
            this.svnStandardLayoutCheckBox.TabIndex = 9;
            this.svnStandardLayoutCheckBox.Text = "Use standard layout";
            this.svnStandardLayoutCheckBox.UseVisualStyleBackColor = true;
            this.svnStandardLayoutCheckBox.CheckedChanged += new System.EventHandler(this.svnStandardLayoutCheckBox_CheckedChanged);
            // 
            // svnRepoTextBox
            // 
            this.svnRepoTextBox.Location = new System.Drawing.Point(77, 6);
            this.svnRepoTextBox.Name = "svnRepoTextBox";
            this.svnRepoTextBox.Size = new System.Drawing.Size(425, 20);
            this.svnRepoTextBox.TabIndex = 1;
            // 
            // svnRepoLabel
            // 
            this.svnRepoLabel.AutoSize = true;
            this.svnRepoLabel.Location = new System.Drawing.Point(6, 9);
            this.svnRepoLabel.Name = "svnRepoLabel";
            this.svnRepoLabel.Size = new System.Drawing.Size(57, 13);
            this.svnRepoLabel.TabIndex = 0;
            this.svnRepoLabel.Text = "Repository";
            // 
            // svnUserLabel
            // 
            this.svnUserLabel.AutoSize = true;
            this.svnUserLabel.Location = new System.Drawing.Point(6, 61);
            this.svnUserLabel.Name = "svnUserLabel";
            this.svnUserLabel.Size = new System.Drawing.Size(43, 13);
            this.svnUserLabel.TabIndex = 5;
            this.svnUserLabel.Text = "User ID";
            // 
            // svnPasswordLabel
            // 
            this.svnPasswordLabel.AutoSize = true;
            this.svnPasswordLabel.Location = new System.Drawing.Point(273, 61);
            this.svnPasswordLabel.Name = "svnPasswordLabel";
            this.svnPasswordLabel.Size = new System.Drawing.Size(53, 13);
            this.svnPasswordLabel.TabIndex = 7;
            this.svnPasswordLabel.Text = "Password";
            // 
            // svnPasswordTextBox
            // 
            this.svnPasswordTextBox.Location = new System.Drawing.Point(331, 58);
            this.svnPasswordTextBox.Name = "svnPasswordTextBox";
            this.svnPasswordTextBox.Size = new System.Drawing.Size(200, 20);
            this.svnPasswordTextBox.TabIndex = 8;
            // 
            // svnUserTextBox
            // 
            this.svnUserTextBox.Location = new System.Drawing.Point(77, 58);
            this.svnUserTextBox.Name = "svnUserTextBox";
            this.svnUserTextBox.Size = new System.Drawing.Size(191, 20);
            this.svnUserTextBox.TabIndex = 6;
            // 
            // outKindLabel
            // 
            this.outKindLabel.AutoSize = true;
            this.outKindLabel.Location = new System.Drawing.Point(6, 132);
            this.outKindLabel.Name = "outKindLabel";
            this.outKindLabel.Size = new System.Drawing.Size(137, 13);
            this.outKindLabel.TabIndex = 9;
            this.outKindLabel.Text = "Output to the following VCS";
            // 
            // transcodeCheckBox
            // 
            this.transcodeCheckBox.AutoSize = true;
            this.transcodeCheckBox.Checked = true;
            this.transcodeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.transcodeCheckBox.Location = new System.Drawing.Point(9, 98);
            this.transcodeCheckBox.Name = "transcodeCheckBox";
            this.transcodeCheckBox.Size = new System.Drawing.Size(209, 17);
            this.transcodeCheckBox.TabIndex = 7;
            this.transcodeCheckBox.Text = "Transcode commit comments to UTF-8";
            this.transcodeCheckBox.UseVisualStyleBackColor = true;
            // 
            // domainTextBox
            // 
            this.domainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.domainTextBox.Location = new System.Drawing.Point(90, 45);
            this.domainTextBox.Name = "domainTextBox";
            this.domainTextBox.Size = new System.Drawing.Size(464, 20);
            this.domainTextBox.TabIndex = 4;
            // 
            // domainLabel
            // 
            this.domainLabel.AutoSize = true;
            this.domainLabel.Location = new System.Drawing.Point(6, 49);
            this.domainLabel.Name = "domainLabel";
            this.domainLabel.Size = new System.Drawing.Size(69, 13);
            this.domainLabel.TabIndex = 3;
            this.domainLabel.Text = "Email domain";
            // 
            // outDirTextBox
            // 
            this.outDirTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outDirTextBox.Location = new System.Drawing.Point(90, 19);
            this.outDirTextBox.Name = "outDirTextBox";
            this.outDirTextBox.Size = new System.Drawing.Size(435, 20);
            this.outDirTextBox.TabIndex = 1;
            // 
            // outDirLabel
            // 
            this.outDirLabel.AutoSize = true;
            this.outDirLabel.Location = new System.Drawing.Point(6, 22);
            this.outDirLabel.Name = "outDirLabel";
            this.outDirLabel.Size = new System.Drawing.Size(49, 13);
            this.outDirLabel.TabIndex = 0;
            this.outDirLabel.Text = "Directory";
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Location = new System.Drawing.Point(90, 72);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(464, 20);
            this.logTextBox.TabIndex = 6;
            // 
            // logLabel
            // 
            this.logLabel.AutoSize = true;
            this.logLabel.Location = new System.Drawing.Point(6, 75);
            this.logLabel.Name = "logLabel";
            this.logLabel.Size = new System.Drawing.Size(41, 13);
            this.logLabel.TabIndex = 5;
            this.logLabel.Text = "Log file";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(497, 545);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Close";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // changesetGroupBox
            // 
            this.changesetGroupBox.Controls.Add(this.label4);
            this.changesetGroupBox.Controls.Add(this.label3);
            this.changesetGroupBox.Controls.Add(this.sameCommentUpDown);
            this.changesetGroupBox.Controls.Add(this.label2);
            this.changesetGroupBox.Controls.Add(this.label1);
            this.changesetGroupBox.Controls.Add(this.anyCommentUpDown);
            this.changesetGroupBox.Location = new System.Drawing.Point(12, 464);
            this.changesetGroupBox.Name = "changesetGroupBox";
            this.changesetGroupBox.Size = new System.Drawing.Size(560, 75);
            this.changesetGroupBox.TabIndex = 2;
            this.changesetGroupBox.TabStop = false;
            this.changesetGroupBox.Text = "Changeset Building";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(194, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(191, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "seconds, if the comments are the same";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "or within";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sameCommentUpDown
            // 
            this.sameCommentUpDown.Location = new System.Drawing.Point(134, 45);
            this.sameCommentUpDown.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.sameCommentUpDown.Name = "sameCommentUpDown";
            this.sameCommentUpDown.Size = new System.Drawing.Size(54, 20);
            this.sameCommentUpDown.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "seconds, regardless of the comment,";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Combine revisions within";
            // 
            // anyCommentUpDown
            // 
            this.anyCommentUpDown.Location = new System.Drawing.Point(134, 19);
            this.anyCommentUpDown.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.anyCommentUpDown.Name = "anyCommentUpDown";
            this.anyCommentUpDown.Size = new System.Drawing.Size(54, 20);
            this.anyCommentUpDown.TabIndex = 1;
            // 
            // vssDirBrowserDialog
            // 
            this.vssDirBrowserDialog.Description = "Directory of the VSS archive";
            this.vssDirBrowserDialog.ShowNewFolderButton = false;
            // 
            // outDirBrowserDialog
            // 
            this.outDirBrowserDialog.Description = "Directory of the export target";
            // 
            // settingsOpenFileDialog
            // 
            this.settingsOpenFileDialog.DefaultExt = "properties";
            this.settingsOpenFileDialog.FileName = "settings.properties";
            this.settingsOpenFileDialog.Filter = "Properties files|*.properties|All files|*.*";
            this.settingsOpenFileDialog.RestoreDirectory = true;
            // 
            // settingsSaveFileDialog
            // 
            this.settingsSaveFileDialog.DefaultExt = "properties";
            this.settingsSaveFileDialog.Filter = "Properties files|*.properties|All files|*.*";
            this.settingsSaveFileDialog.RestoreDirectory = true;
            // 
            // saveSettingsButton
            // 
            this.saveSettingsButton.Location = new System.Drawing.Point(12, 545);
            this.saveSettingsButton.Name = "saveSettingsButton";
            this.saveSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.saveSettingsButton.TabIndex = 6;
            this.saveSettingsButton.Text = "Save...";
            this.saveSettingsButton.UseVisualStyleBackColor = true;
            this.saveSettingsButton.Click += new System.EventHandler(this.saveSettingsButton_Click);
            // 
            // loadSettingsButton
            // 
            this.loadSettingsButton.Location = new System.Drawing.Point(93, 545);
            this.loadSettingsButton.Name = "loadSettingsButton";
            this.loadSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.loadSettingsButton.TabIndex = 7;
            this.loadSettingsButton.Text = "Load...";
            this.loadSettingsButton.UseVisualStyleBackColor = true;
            this.loadSettingsButton.Click += new System.EventHandler(this.loadSettingsButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.goButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(584, 598);
            this.Controls.Add(this.loadSettingsButton);
            this.Controls.Add(this.saveSettingsButton);
            this.Controls.Add(this.changesetGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.outputGroupBox);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.vssGroupBox);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(458, 419);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VSS2Git";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.vssGroupBox.ResumeLayout(false);
            this.vssGroupBox.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.outputGroupBox.ResumeLayout(false);
            this.outputGroupBox.PerformLayout();
            this.vcsSetttingsTabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.changesetGroupBox.ResumeLayout(false);
            this.changesetGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sameCommentUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.anyCommentUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox vssGroupBox;
        private System.Windows.Forms.TextBox vssProjectTextBox;
        private System.Windows.Forms.TextBox vssDirTextBox;
        private System.Windows.Forms.Label vssProjectLabel;
        private System.Windows.Forms.Label vssDirLabel;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.Timer statusTimer;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel fileLabel;
        private System.Windows.Forms.ToolStripStatusLabel timeLabel;
        private System.Windows.Forms.ToolStripStatusLabel revisionLabel;
        private System.Windows.Forms.ToolStripStatusLabel changeLabel;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.GroupBox outputGroupBox;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Label logLabel;
        private System.Windows.Forms.TextBox outDirTextBox;
        private System.Windows.Forms.Label outDirLabel;
        private System.Windows.Forms.TextBox domainTextBox;
        private System.Windows.Forms.Label domainLabel;
        private System.Windows.Forms.TextBox excludeTextBox;
        private System.Windows.Forms.Label excludeLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox changesetGroupBox;
        private System.Windows.Forms.NumericUpDown anyCommentUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown sameCommentUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label encodingLabel;
        private System.Windows.Forms.ComboBox encodingComboBox;
        private System.Windows.Forms.CheckBox transcodeCheckBox;
        private System.Windows.Forms.CheckBox forceAnnotatedCheckBox;
        private System.Windows.Forms.Label outKindLabel;
        private System.Windows.Forms.TextBox svnPasswordTextBox;
        private System.Windows.Forms.TextBox svnUserTextBox;
        private System.Windows.Forms.Label svnPasswordLabel;
        private System.Windows.Forms.Label svnUserLabel;
        private System.Windows.Forms.TabControl vcsSetttingsTabs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox svnRepoTextBox;
        private System.Windows.Forms.Label svnRepoLabel;
        private System.Windows.Forms.CheckBox svnStandardLayoutCheckBox;
        private System.Windows.Forms.TextBox svnBranchesTextBox;
        private System.Windows.Forms.TextBox svnTagsTextBox;
        private System.Windows.Forms.TextBox svnTrunkTextBox;
        private System.Windows.Forms.Label svnBranchesLabel;
        private System.Windows.Forms.Label svnTagsLabel;
        private System.Windows.Forms.Label svnTrunkLabel;
        private System.Windows.Forms.CheckBox resetRepoCheckBox;
        private System.Windows.Forms.TextBox svnProjectPathTextBox;
        private System.Windows.Forms.Label svnProjectPathLabel;
        private System.Windows.Forms.FolderBrowserDialog vssDirBrowserDialog;
        private System.Windows.Forms.Button vssDirButton;
        private System.Windows.Forms.Button outDirButton;
        private System.Windows.Forms.FolderBrowserDialog outDirBrowserDialog;
        private System.Windows.Forms.Button svnRepoButton;
        private System.Windows.Forms.FolderBrowserDialog svnRepoBrowserDialog;
        private System.Windows.Forms.OpenFileDialog settingsOpenFileDialog;
        private System.Windows.Forms.SaveFileDialog settingsSaveFileDialog;
        private System.Windows.Forms.Button saveSettingsButton;
        private System.Windows.Forms.Button loadSettingsButton;

    }
}


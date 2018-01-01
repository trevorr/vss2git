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
            this.ignoreErrorsCheckBox = new System.Windows.Forms.CheckBox();
            this.commentTextBox = new System.Windows.Forms.TextBox();
            this.commentLabel = new System.Windows.Forms.Label();
            this.forceAnnotatedCheckBox = new System.Windows.Forms.CheckBox();
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
            this.cleanVssRemnantsCheckBox = new System.Windows.Forms.CheckBox();
            this.vssGroupBox.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.outputGroupBox.SuspendLayout();
            this.changesetGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sameCommentUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.anyCommentUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // vssGroupBox
            // 
            this.vssGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vssGroupBox.Controls.Add(this.encodingLabel);
            this.vssGroupBox.Controls.Add(this.encodingComboBox);
            this.vssGroupBox.Controls.Add(this.excludeTextBox);
            this.vssGroupBox.Controls.Add(this.excludeLabel);
            this.vssGroupBox.Controls.Add(this.vssProjectTextBox);
            this.vssGroupBox.Controls.Add(this.vssDirTextBox);
            this.vssGroupBox.Controls.Add(this.vssProjectLabel);
            this.vssGroupBox.Controls.Add(this.vssDirLabel);
            this.vssGroupBox.Location = new System.Drawing.Point(16, 15);
            this.vssGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.vssGroupBox.Name = "vssGroupBox";
            this.vssGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.vssGroupBox.Size = new System.Drawing.Size(747, 155);
            this.vssGroupBox.TabIndex = 0;
            this.vssGroupBox.TabStop = false;
            this.vssGroupBox.Text = "VSS Settings";
            // 
            // encodingLabel
            // 
            this.encodingLabel.AutoSize = true;
            this.encodingLabel.Location = new System.Drawing.Point(8, 123);
            this.encodingLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.encodingLabel.Name = "encodingLabel";
            this.encodingLabel.Size = new System.Drawing.Size(67, 17);
            this.encodingLabel.TabIndex = 6;
            this.encodingLabel.Text = "Encoding";
            // 
            // encodingComboBox
            // 
            this.encodingComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.encodingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encodingComboBox.FormattingEnabled = true;
            this.encodingComboBox.Location = new System.Drawing.Point(125, 119);
            this.encodingComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.encodingComboBox.Name = "encodingComboBox";
            this.encodingComboBox.Size = new System.Drawing.Size(612, 24);
            this.encodingComboBox.TabIndex = 7;
            // 
            // excludeTextBox
            // 
            this.excludeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.excludeTextBox.Location = new System.Drawing.Point(125, 87);
            this.excludeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.excludeTextBox.Name = "excludeTextBox";
            this.excludeTextBox.Size = new System.Drawing.Size(612, 22);
            this.excludeTextBox.TabIndex = 5;
            // 
            // excludeLabel
            // 
            this.excludeLabel.AutoSize = true;
            this.excludeLabel.Location = new System.Drawing.Point(8, 91);
            this.excludeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.excludeLabel.Name = "excludeLabel";
            this.excludeLabel.Size = new System.Drawing.Size(86, 17);
            this.excludeLabel.TabIndex = 4;
            this.excludeLabel.Text = "Exclude files";
            // 
            // vssProjectTextBox
            // 
            this.vssProjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vssProjectTextBox.Location = new System.Drawing.Point(125, 55);
            this.vssProjectTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.vssProjectTextBox.Name = "vssProjectTextBox";
            this.vssProjectTextBox.Size = new System.Drawing.Size(612, 22);
            this.vssProjectTextBox.TabIndex = 3;
            // 
            // vssDirTextBox
            // 
            this.vssDirTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vssDirTextBox.Location = new System.Drawing.Point(125, 23);
            this.vssDirTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.vssDirTextBox.Name = "vssDirTextBox";
            this.vssDirTextBox.Size = new System.Drawing.Size(612, 22);
            this.vssDirTextBox.TabIndex = 1;
            // 
            // vssProjectLabel
            // 
            this.vssProjectLabel.AutoSize = true;
            this.vssProjectLabel.Location = new System.Drawing.Point(8, 59);
            this.vssProjectLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.vssProjectLabel.Name = "vssProjectLabel";
            this.vssProjectLabel.Size = new System.Drawing.Size(52, 17);
            this.vssProjectLabel.TabIndex = 2;
            this.vssProjectLabel.Text = "Project";
            // 
            // vssDirLabel
            // 
            this.vssDirLabel.AutoSize = true;
            this.vssDirLabel.Location = new System.Drawing.Point(8, 27);
            this.vssDirLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.vssDirLabel.Name = "vssDirLabel";
            this.vssDirLabel.Size = new System.Drawing.Size(65, 17);
            this.vssDirLabel.TabIndex = 0;
            this.vssDirLabel.Text = "Directory";
            // 
            // goButton
            // 
            this.goButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.goButton.Location = new System.Drawing.Point(555, 501);
            this.goButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(100, 28);
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
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.fileLabel,
            this.revisionLabel,
            this.changeLabel,
            this.timeLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 539);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.Size = new System.Drawing.Size(779, 25);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(400, 20);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "Idle";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fileLabel
            // 
            this.fileLabel.Name = "fileLabel";
            this.fileLabel.Size = new System.Drawing.Size(53, 20);
            this.fileLabel.Text = "Files: 0";
            // 
            // revisionLabel
            // 
            this.revisionLabel.Name = "revisionLabel";
            this.revisionLabel.Size = new System.Drawing.Size(85, 20);
            this.revisionLabel.Text = "Revisions: 0";
            // 
            // changeLabel
            // 
            this.changeLabel.Name = "changeLabel";
            this.changeLabel.Size = new System.Drawing.Size(99, 20);
            this.changeLabel.Text = "Changesets: 0";
            // 
            // timeLabel
            // 
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(122, 20);
            this.timeLabel.Text = "Elapsed: 00:00:00";
            // 
            // outputGroupBox
            // 
            this.outputGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputGroupBox.Controls.Add(this.cleanVssRemnantsCheckBox);
            this.outputGroupBox.Controls.Add(this.ignoreErrorsCheckBox);
            this.outputGroupBox.Controls.Add(this.commentTextBox);
            this.outputGroupBox.Controls.Add(this.commentLabel);
            this.outputGroupBox.Controls.Add(this.forceAnnotatedCheckBox);
            this.outputGroupBox.Controls.Add(this.transcodeCheckBox);
            this.outputGroupBox.Controls.Add(this.domainTextBox);
            this.outputGroupBox.Controls.Add(this.domainLabel);
            this.outputGroupBox.Controls.Add(this.outDirTextBox);
            this.outputGroupBox.Controls.Add(this.outDirLabel);
            this.outputGroupBox.Controls.Add(this.logTextBox);
            this.outputGroupBox.Controls.Add(this.logLabel);
            this.outputGroupBox.Location = new System.Drawing.Point(16, 177);
            this.outputGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.outputGroupBox.Name = "outputGroupBox";
            this.outputGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.outputGroupBox.Size = new System.Drawing.Size(747, 216);
            this.outputGroupBox.TabIndex = 1;
            this.outputGroupBox.TabStop = false;
            this.outputGroupBox.Text = "Output Settings";
            // 
            // ignoreErrorsCheckBox
            // 
            this.ignoreErrorsCheckBox.AutoSize = true;
            this.ignoreErrorsCheckBox.Location = new System.Drawing.Point(557, 149);
            this.ignoreErrorsCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ignoreErrorsCheckBox.Name = "ignoreErrorsCheckBox";
            this.ignoreErrorsCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ignoreErrorsCheckBox.Size = new System.Drawing.Size(134, 21);
            this.ignoreErrorsCheckBox.TabIndex = 8;
            this.ignoreErrorsCheckBox.Text = "Ignore Git errors";
            this.ignoreErrorsCheckBox.UseVisualStyleBackColor = true;
            // 
            // commentTextBox
            // 
            this.commentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commentTextBox.Location = new System.Drawing.Point(125, 119);
            this.commentTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.Size = new System.Drawing.Size(612, 22);
            this.commentTextBox.TabIndex = 6;
            // 
            // commentLabel
            // 
            this.commentLabel.AutoSize = true;
            this.commentLabel.Location = new System.Drawing.Point(8, 123);
            this.commentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.commentLabel.Name = "commentLabel";
            this.commentLabel.Size = new System.Drawing.Size(114, 17);
            this.commentLabel.TabIndex = 8;
            this.commentLabel.Text = "Default comment";
            // 
            // forceAnnotatedCheckBox
            // 
            this.forceAnnotatedCheckBox.AutoSize = true;
            this.forceAnnotatedCheckBox.Checked = true;
            this.forceAnnotatedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.forceAnnotatedCheckBox.Location = new System.Drawing.Point(299, 151);
            this.forceAnnotatedCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.forceAnnotatedCheckBox.Name = "forceAnnotatedCheckBox";
            this.forceAnnotatedCheckBox.Size = new System.Drawing.Size(250, 21);
            this.forceAnnotatedCheckBox.TabIndex = 7;
            this.forceAnnotatedCheckBox.Text = "Force use of annotated tag objects";
            this.forceAnnotatedCheckBox.UseVisualStyleBackColor = true;
            // 
            // transcodeCheckBox
            // 
            this.transcodeCheckBox.AutoSize = true;
            this.transcodeCheckBox.Checked = true;
            this.transcodeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.transcodeCheckBox.Location = new System.Drawing.Point(12, 151);
            this.transcodeCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.transcodeCheckBox.Name = "transcodeCheckBox";
            this.transcodeCheckBox.Size = new System.Drawing.Size(274, 21);
            this.transcodeCheckBox.TabIndex = 6;
            this.transcodeCheckBox.Text = "Transcode commit comments to UTF-8";
            this.transcodeCheckBox.UseVisualStyleBackColor = true;
            // 
            // domainTextBox
            // 
            this.domainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.domainTextBox.Location = new System.Drawing.Point(125, 55);
            this.domainTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.domainTextBox.Name = "domainTextBox";
            this.domainTextBox.Size = new System.Drawing.Size(612, 22);
            this.domainTextBox.TabIndex = 3;
            // 
            // domainLabel
            // 
            this.domainLabel.AutoSize = true;
            this.domainLabel.Location = new System.Drawing.Point(8, 59);
            this.domainLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.domainLabel.Name = "domainLabel";
            this.domainLabel.Size = new System.Drawing.Size(92, 17);
            this.domainLabel.TabIndex = 2;
            this.domainLabel.Text = "Email domain";
            // 
            // outDirTextBox
            // 
            this.outDirTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outDirTextBox.Location = new System.Drawing.Point(125, 23);
            this.outDirTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.outDirTextBox.Name = "outDirTextBox";
            this.outDirTextBox.Size = new System.Drawing.Size(612, 22);
            this.outDirTextBox.TabIndex = 1;
            // 
            // outDirLabel
            // 
            this.outDirLabel.AutoSize = true;
            this.outDirLabel.Location = new System.Drawing.Point(8, 27);
            this.outDirLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.outDirLabel.Name = "outDirLabel";
            this.outDirLabel.Size = new System.Drawing.Size(65, 17);
            this.outDirLabel.TabIndex = 0;
            this.outDirLabel.Text = "Directory";
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Location = new System.Drawing.Point(125, 87);
            this.logTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(612, 22);
            this.logTextBox.TabIndex = 5;
            // 
            // logLabel
            // 
            this.logLabel.AutoSize = true;
            this.logLabel.Location = new System.Drawing.Point(8, 91);
            this.logLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.logLabel.Name = "logLabel";
            this.logLabel.Size = new System.Drawing.Size(54, 17);
            this.logLabel.TabIndex = 4;
            this.logLabel.Text = "Log file";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(663, 501);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 28);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
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
            this.changesetGroupBox.Location = new System.Drawing.Point(16, 401);
            this.changesetGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.changesetGroupBox.Name = "changesetGroupBox";
            this.changesetGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.changesetGroupBox.Size = new System.Drawing.Size(747, 92);
            this.changesetGroupBox.TabIndex = 2;
            this.changesetGroupBox.TabStop = false;
            this.changesetGroupBox.Text = "Changeset Building";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(259, 58);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(255, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "seconds, if the comments are the same";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 58);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "or within";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sameCommentUpDown
            // 
            this.sameCommentUpDown.Location = new System.Drawing.Point(179, 55);
            this.sameCommentUpDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sameCommentUpDown.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.sameCommentUpDown.Name = "sameCommentUpDown";
            this.sameCommentUpDown.Size = new System.Drawing.Size(72, 22);
            this.sameCommentUpDown.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(259, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(241, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "seconds, regardless of the comment,";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Combine revisions within";
            // 
            // anyCommentUpDown
            // 
            this.anyCommentUpDown.Location = new System.Drawing.Point(179, 23);
            this.anyCommentUpDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.anyCommentUpDown.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.anyCommentUpDown.Name = "anyCommentUpDown";
            this.anyCommentUpDown.Size = new System.Drawing.Size(72, 22);
            this.anyCommentUpDown.TabIndex = 1;
            // 
            // cleanVssRemnantsCheckBox
            // 
            this.cleanVssRemnantsCheckBox.AutoSize = true;
            this.cleanVssRemnantsCheckBox.Checked = true;
            this.cleanVssRemnantsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cleanVssRemnantsCheckBox.Location = new System.Drawing.Point(11, 180);
            this.cleanVssRemnantsCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.cleanVssRemnantsCheckBox.Name = "cleanVssRemnantsCheckBox";
            this.cleanVssRemnantsCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cleanVssRemnantsCheckBox.Size = new System.Drawing.Size(374, 21);
            this.cleanVssRemnantsCheckBox.TabIndex = 9;
            this.cleanVssRemnantsCheckBox.Text = "Clean VSS setting and related files in output repository";
            this.cleanVssRemnantsCheckBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AcceptButton = this.goButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(779, 564);
            this.Controls.Add(this.changesetGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.outputGroupBox);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.vssGroupBox);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(605, 505);
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
        private System.Windows.Forms.CheckBox ignoreErrorsCheckBox;
        private System.Windows.Forms.TextBox commentTextBox;
        private System.Windows.Forms.Label commentLabel;
        private System.Windows.Forms.CheckBox cleanVssRemnantsCheckBox;
    }
}


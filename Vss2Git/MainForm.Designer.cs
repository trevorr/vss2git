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
			this.vssDirTextBox = new System.Windows.Forms.TextBox();
			this.vssDirLabel = new System.Windows.Forms.Label();
			this.goButton = new System.Windows.Forms.Button();
			this.statusTimer = new System.Windows.Forms.Timer(this.components);
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.fileLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.revisionLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.changeLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.timeLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.cancelButton = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.changesetGroupBox = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.sameCommentUpDown = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.anyCommentUpDown = new System.Windows.Forms.NumericUpDown();
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
			this.projectLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.projectsTreeControl = new Hpdi.Vss2Git.ProjectsTreeControl();
			this.vssGroupBox.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.changesetGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sameCommentUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.anyCommentUpDown)).BeginInit();
			this.outputGroupBox.SuspendLayout();
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
			this.vssGroupBox.Controls.Add(this.vssDirTextBox);
			this.vssGroupBox.Controls.Add(this.vssDirLabel);
			this.vssGroupBox.Location = new System.Drawing.Point(3, 6);
			this.vssGroupBox.Name = "vssGroupBox";
			this.vssGroupBox.Size = new System.Drawing.Size(560, 104);
			this.vssGroupBox.TabIndex = 0;
			this.vssGroupBox.TabStop = false;
			this.vssGroupBox.Text = "VSS Settings";
			// 
			// encodingLabel
			// 
			this.encodingLabel.AutoSize = true;
			this.encodingLabel.Location = new System.Drawing.Point(6, 74);
			this.encodingLabel.Name = "encodingLabel";
			this.encodingLabel.Size = new System.Drawing.Size(52, 13);
			this.encodingLabel.TabIndex = 6;
			this.encodingLabel.Text = "Encoding";
			// 
			// encodingComboBox
			// 
			this.encodingComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.encodingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.encodingComboBox.FormattingEnabled = true;
			this.encodingComboBox.Location = new System.Drawing.Point(94, 71);
			this.encodingComboBox.Name = "encodingComboBox";
			this.encodingComboBox.Size = new System.Drawing.Size(460, 21);
			this.encodingComboBox.TabIndex = 7;
			this.encodingComboBox.SelectedIndexChanged += new System.EventHandler(this.encodingComboBox_SelectedIndexChanged);
			// 
			// excludeTextBox
			// 
			this.excludeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.excludeTextBox.Location = new System.Drawing.Point(94, 45);
			this.excludeTextBox.Name = "excludeTextBox";
			this.excludeTextBox.Size = new System.Drawing.Size(460, 20);
			this.excludeTextBox.TabIndex = 5;
			// 
			// excludeLabel
			// 
			this.excludeLabel.AutoSize = true;
			this.excludeLabel.Location = new System.Drawing.Point(6, 48);
			this.excludeLabel.Name = "excludeLabel";
			this.excludeLabel.Size = new System.Drawing.Size(66, 13);
			this.excludeLabel.TabIndex = 4;
			this.excludeLabel.Text = "Exclude files";
			// 
			// vssDirTextBox
			// 
			this.vssDirTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.vssDirTextBox.Location = new System.Drawing.Point(94, 19);
			this.vssDirTextBox.Name = "vssDirTextBox";
			this.vssDirTextBox.Size = new System.Drawing.Size(460, 20);
			this.vssDirTextBox.TabIndex = 1;
			this.vssDirTextBox.TextChanged += new System.EventHandler(this.vssDirTextBox_TextChanged);
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
			this.goButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.goButton.Location = new System.Drawing.Point(423, 636);
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
            this.projectLabel,
            this.fileLabel,
            this.revisionLabel,
            this.changeLabel,
            this.timeLabel});
			this.statusStrip.Location = new System.Drawing.Point(0, 668);
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
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(504, 636);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(5, 5);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(576, 625);
			this.tabControl1.TabIndex = 6;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.vssGroupBox);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(568, 599);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "VSS Settings";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.projectsTreeControl);
			this.groupBox1.Location = new System.Drawing.Point(3, 115);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(560, 483);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Projects";
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage2.Controls.Add(this.changesetGroupBox);
			this.tabPage2.Controls.Add(this.outputGroupBox);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(568, 599);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Output Settings";
			// 
			// changesetGroupBox
			// 
			this.changesetGroupBox.Controls.Add(this.label4);
			this.changesetGroupBox.Controls.Add(this.label3);
			this.changesetGroupBox.Controls.Add(this.sameCommentUpDown);
			this.changesetGroupBox.Controls.Add(this.label2);
			this.changesetGroupBox.Controls.Add(this.label1);
			this.changesetGroupBox.Controls.Add(this.anyCommentUpDown);
			this.changesetGroupBox.Location = new System.Drawing.Point(5, 135);
			this.changesetGroupBox.Name = "changesetGroupBox";
			this.changesetGroupBox.Size = new System.Drawing.Size(560, 75);
			this.changesetGroupBox.TabIndex = 4;
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
			// outputGroupBox
			// 
			this.outputGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.outputGroupBox.Controls.Add(this.ignoreErrorsCheckBox);
			this.outputGroupBox.Controls.Add(this.commentTextBox);
			this.outputGroupBox.Controls.Add(this.commentLabel);
			this.outputGroupBox.Controls.Add(this.forceAnnotatedCheckBox);
			this.outputGroupBox.Controls.Add(this.transcodeCheckBox);
			this.outputGroupBox.Controls.Add(this.domainTextBox);
			this.outputGroupBox.Controls.Add(this.domainLabel);
			this.outputGroupBox.Controls.Add(this.outDirTextBox);
			this.outputGroupBox.Controls.Add(this.outDirLabel);
			this.outputGroupBox.Location = new System.Drawing.Point(5, 4);
			this.outputGroupBox.Name = "outputGroupBox";
			this.outputGroupBox.Size = new System.Drawing.Size(560, 128);
			this.outputGroupBox.TabIndex = 3;
			this.outputGroupBox.TabStop = false;
			this.outputGroupBox.Text = "Output Settings";
			// 
			// ignoreErrorsCheckBox
			// 
			this.ignoreErrorsCheckBox.AutoSize = true;
			this.ignoreErrorsCheckBox.Location = new System.Drawing.Point(431, 100);
			this.ignoreErrorsCheckBox.Name = "ignoreErrorsCheckBox";
			this.ignoreErrorsCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ignoreErrorsCheckBox.Size = new System.Drawing.Size(101, 17);
			this.ignoreErrorsCheckBox.TabIndex = 8;
			this.ignoreErrorsCheckBox.Text = "Ignore Git errors";
			this.ignoreErrorsCheckBox.UseVisualStyleBackColor = true;
			// 
			// commentTextBox
			// 
			this.commentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.commentTextBox.Location = new System.Drawing.Point(94, 72);
			this.commentTextBox.Name = "commentTextBox";
			this.commentTextBox.Size = new System.Drawing.Size(460, 20);
			this.commentTextBox.TabIndex = 6;
			// 
			// commentLabel
			// 
			this.commentLabel.AutoSize = true;
			this.commentLabel.Location = new System.Drawing.Point(6, 75);
			this.commentLabel.Name = "commentLabel";
			this.commentLabel.Size = new System.Drawing.Size(87, 13);
			this.commentLabel.TabIndex = 8;
			this.commentLabel.Text = "Default comment";
			// 
			// forceAnnotatedCheckBox
			// 
			this.forceAnnotatedCheckBox.AutoSize = true;
			this.forceAnnotatedCheckBox.Checked = true;
			this.forceAnnotatedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.forceAnnotatedCheckBox.Location = new System.Drawing.Point(224, 100);
			this.forceAnnotatedCheckBox.Name = "forceAnnotatedCheckBox";
			this.forceAnnotatedCheckBox.Size = new System.Drawing.Size(191, 17);
			this.forceAnnotatedCheckBox.TabIndex = 7;
			this.forceAnnotatedCheckBox.Text = "Force use of annotated tag objects";
			this.forceAnnotatedCheckBox.UseVisualStyleBackColor = true;
			// 
			// transcodeCheckBox
			// 
			this.transcodeCheckBox.AutoSize = true;
			this.transcodeCheckBox.Checked = true;
			this.transcodeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.transcodeCheckBox.Location = new System.Drawing.Point(9, 100);
			this.transcodeCheckBox.Name = "transcodeCheckBox";
			this.transcodeCheckBox.Size = new System.Drawing.Size(209, 17);
			this.transcodeCheckBox.TabIndex = 6;
			this.transcodeCheckBox.Text = "Transcode commit comments to UTF-8";
			this.transcodeCheckBox.UseVisualStyleBackColor = true;
			// 
			// domainTextBox
			// 
			this.domainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.domainTextBox.Location = new System.Drawing.Point(94, 45);
			this.domainTextBox.Name = "domainTextBox";
			this.domainTextBox.Size = new System.Drawing.Size(460, 20);
			this.domainTextBox.TabIndex = 3;
			// 
			// domainLabel
			// 
			this.domainLabel.AutoSize = true;
			this.domainLabel.Location = new System.Drawing.Point(6, 48);
			this.domainLabel.Name = "domainLabel";
			this.domainLabel.Size = new System.Drawing.Size(69, 13);
			this.domainLabel.TabIndex = 2;
			this.domainLabel.Text = "Email domain";
			// 
			// outDirTextBox
			// 
			this.outDirTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.outDirTextBox.Location = new System.Drawing.Point(94, 19);
			this.outDirTextBox.Name = "outDirTextBox";
			this.outDirTextBox.Size = new System.Drawing.Size(460, 20);
			this.outDirTextBox.TabIndex = 1;
			this.outDirTextBox.TextChanged += new System.EventHandler(this.outDirTextBox_TextChanged);
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
			// projectLabel
			// 
			this.projectLabel.Name = "projectLabel";
			this.projectLabel.Size = new System.Drawing.Size(0, 17);
			// 
			// projectsTreeControl
			// 
			this.projectsTreeControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.projectsTreeControl.Location = new System.Drawing.Point(3, 16);
			this.projectsTreeControl.Name = "projectsTreeControl";
			this.projectsTreeControl.Size = new System.Drawing.Size(554, 464);
			this.projectsTreeControl.TabIndex = 1;
			this.projectsTreeControl.CheckedChanged += new System.EventHandler(this.projectsTreeControl_CheckedChanged);
			// 
			// MainForm
			// 
			this.AcceptButton = this.goButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(584, 690);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.goButton);
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
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.changesetGroupBox.ResumeLayout(false);
			this.changesetGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.sameCommentUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.anyCommentUpDown)).EndInit();
			this.outputGroupBox.ResumeLayout(false);
			this.outputGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox vssGroupBox;
        private System.Windows.Forms.TextBox vssDirTextBox;
        private System.Windows.Forms.Label vssDirLabel;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.Timer statusTimer;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel fileLabel;
        private System.Windows.Forms.ToolStripStatusLabel timeLabel;
        private System.Windows.Forms.ToolStripStatusLabel revisionLabel;
        private System.Windows.Forms.ToolStripStatusLabel changeLabel;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.TextBox excludeTextBox;
        private System.Windows.Forms.Label excludeLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label encodingLabel;
        private System.Windows.Forms.ComboBox encodingComboBox;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.GroupBox changesetGroupBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown sameCommentUpDown;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown anyCommentUpDown;
		private System.Windows.Forms.GroupBox outputGroupBox;
		private System.Windows.Forms.CheckBox ignoreErrorsCheckBox;
		private System.Windows.Forms.TextBox commentTextBox;
		private System.Windows.Forms.Label commentLabel;
		private System.Windows.Forms.CheckBox forceAnnotatedCheckBox;
		private System.Windows.Forms.CheckBox transcodeCheckBox;
		private System.Windows.Forms.TextBox domainTextBox;
		private System.Windows.Forms.Label domainLabel;
		private System.Windows.Forms.TextBox outDirTextBox;
		private System.Windows.Forms.Label outDirLabel;
		private System.Windows.Forms.GroupBox groupBox1;
		private ProjectsTreeControl projectsTreeControl;
		private System.Windows.Forms.ToolStripStatusLabel projectLabel;
	}
}


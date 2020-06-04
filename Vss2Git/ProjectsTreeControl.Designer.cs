namespace Hpdi.Vss2Git
{
	partial class ProjectsTreeControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tvProjects = new System.Windows.Forms.TreeView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnRefreshProjects = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tvProjects
			// 
			this.tvProjects.CheckBoxes = true;
			this.tvProjects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvProjects.Enabled = false;
			this.tvProjects.Location = new System.Drawing.Point(0, 28);
			this.tvProjects.Name = "tvProjects";
			this.tvProjects.Size = new System.Drawing.Size(560, 455);
			this.tvProjects.TabIndex = 0;
			this.tvProjects.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvProjects_BeforeCheck);
			this.tvProjects.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvProjects_AfterCheck);
			this.tvProjects.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvProjects_BeforeExpand);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnRefreshProjects);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(560, 28);
			this.panel1.TabIndex = 1;
			// 
			// btnRefreshProjects
			// 
			this.btnRefreshProjects.Location = new System.Drawing.Point(3, 3);
			this.btnRefreshProjects.Name = "btnRefreshProjects";
			this.btnRefreshProjects.Size = new System.Drawing.Size(98, 23);
			this.btnRefreshProjects.TabIndex = 0;
			this.btnRefreshProjects.Text = "Refresh";
			this.btnRefreshProjects.UseVisualStyleBackColor = true;
			this.btnRefreshProjects.Click += new System.EventHandler(this.btnRefreshProjects_Click);
			// 
			// ProjectsTreeControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tvProjects);
			this.Controls.Add(this.panel1);
			this.Name = "ProjectsTreeControl";
			this.Size = new System.Drawing.Size(560, 483);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView tvProjects;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnRefreshProjects;
	}
}

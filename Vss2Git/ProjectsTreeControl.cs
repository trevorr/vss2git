using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hpdi.VssLogicalLib;

namespace Hpdi.Vss2Git
{
	public partial class ProjectsTreeControl : UserControl
	{
		public ProjectsTreeControl()
		{
			this.InitializeComponent();
		}

		public event EventHandler CheckedChanged;
		private bool internalUpdate;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string VSSDirectory
		{
			get;
			set;
		}

		private string outputDirectory;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string OutputDirectory
		{
			get
			{
				return this.outputDirectory;
			}
			set
			{
				this.outputDirectory = value;
				this.UpdateNodes();
			}
		}

		private StringCollection selectedPaths;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StringCollection SelectedPaths
		{
			get
			{
				StringCollection collection = new StringCollection();
				CollectCheckedNodeNames(collection, this.tvProjects.Nodes);
				this.selectedPaths = collection;
				return collection;
			}
			set
			{
				this.selectedPaths = value;
				this.UpdateNodes();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Encoding Encoding
		{
			get;
			set;
		}

		private bool canCheck = true;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CanCheck
		{
			get => this.canCheck;
			set => this.canCheck = value;
		}

		public void RefreshProjects()
		{
			VssDatabaseFactory df = new VssDatabaseFactory(this.VSSDirectory);
			df.Encoding = this.Encoding;
			VssDatabase db = df.Open();

			this.tvProjects.Nodes.Clear();
			if (db.RootProject != null)
			{
				TreeNode node = this.AddNode(this.tvProjects.Nodes, db.RootProject, db.RootProject.Path);
				node.Expand();
			}

			this.tvProjects.Enabled = true;
		}

		public void UpdateNodes()
		{
			this.internalUpdate = true;
			this.UpdateNodes(this.selectedPaths, this.tvProjects.Nodes);
			this.internalUpdate = false;
		}

		private static void CollectCheckedNodeNames(StringCollection collection, TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{
				if (node.Checked)
				{
					NodeInfo info = (NodeInfo)node.Tag;
					if (info.Project != null)
					{
						collection.Add(info.Project.Path);
					}
				}

				CollectCheckedNodeNames(collection, node.Nodes);
			}
		}

		private void UpdateNodes(StringCollection collection, TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{
				NodeInfo info = (NodeInfo)node.Tag;
				if (info?.Project != null)
				{
					string path = MainForm.GetProjectPath(this.outputDirectory, info.Project.Path, true);
					info.AlreadyExists = Directory.Exists(path);
					if (info.AlreadyExists)
					{
						node.Text = $"{info.Name} [done]";
						node.ForeColor = Color.DarkGray;
					}
					else
					{
						node.Text = info.Name;
						node.ForeColor = Color.Black;
					}

					foreach (string projPath in collection)
					{
						if (projPath == info.Project.Path)
						{
							node.Checked = true;
						}
						else if (projPath.Contains(info.Project.Path + "/"))
						{
							node.Expand();
						}
					}
				}
				this.UpdateNodes(collection, node.Nodes);
			}
		}

		private void btnRefreshProjects_Click(object sender, EventArgs e)
		{
			this.RefreshProjects();
		}

		private TreeNode AddNode(TreeNodeCollection nodes, VssProject project, string name)
		{
			TreeNode node = nodes.Add(name);
			NodeInfo info = new NodeInfo(project, name);
			if (project == null)
			{
				node.ForeColor = Color.DarkGray;
			}
			else
			{
				bool hasSome = false;
				foreach (VssProject p in project.Projects)
				{
					if (p.IsProject)
					{
						hasSome = true;
						break;
					}
				}
				if (project.Files.Any())
				{
					hasSome = true;
				}

				if (hasSome)
				{
					node.Nodes.Add("");
				}
			}

			node.Tag = info;
			return node;
		}

		private void tvProjects_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			NodeInfo info = (NodeInfo)e.Node.Tag;
			if (!info.IsFilled)
			{
				info.IsFilled = true;

				bool needUpdate = false;
				e.Node.Nodes.Clear();
				foreach (VssProject p in info.Project.Projects)
				{
					if (p.IsProject)
					{
						this.AddNode(e.Node.Nodes, p, p.Name);
						needUpdate = true;
					}
				}

				foreach (VssFile file in info.Project.Files)
				{
					this.AddNode(e.Node.Nodes, null, file.Name);
				}

				if (needUpdate)
				{
					this.UpdateNodes();
				}
			}
		}

		private void tvProjects_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (this.internalUpdate) return;
			this.CheckedChanged?.Invoke(this, EventArgs.Empty);
		}

		private void tvProjects_BeforeCheck(object sender, TreeViewCancelEventArgs e)
		{
			if (this.internalUpdate) return;
			if (!this.CanCheck) return;

			NodeInfo info = (NodeInfo)e.Node.Tag;
			if (info.Project == null)
			{
				e.Cancel = true;
			}
		}

		private class NodeInfo
		{
			public readonly VssProject Project;
			public readonly string Name;
			public bool IsFilled;
			public bool AlreadyExists;
			public NodeInfo(VssProject project, string name)
			{
				this.Project = project;
				this.Name = name;
			}
		}
	}
}

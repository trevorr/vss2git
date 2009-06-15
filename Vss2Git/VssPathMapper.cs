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
using System.Diagnostics;
using System.IO;
using System.Text;
using Hpdi.VssLogicalLib;

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// Base class for representing VSS items.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class VssItemInfo
    {
        private readonly string physicalName;
        public string PhysicalName
        {
            get { return physicalName; }
        }

        public VssItemInfo(string physicalName)
        {
            this.physicalName = physicalName;
        }
    }

    /// <summary>
    /// Represents the current state of a VSS project.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class VssProjectInfo : VssItemInfo
    {
        private VssProjectInfo parentInfo;
        public VssProjectInfo Parent
        {
            get { return parentInfo; }
            set
            {
                if (parentInfo != null)
                {
                    parentInfo.RemoveItem(this);
                }
                parentInfo = value;
                if (parentInfo != null)
                {
                    parentInfo.AddItem(this);
                }
            }
        }

        private bool isRoot;
        public bool IsRoot
        {
            get { return isRoot; }
            set { isRoot = value; }
        }

        public bool IsRooted
        {
            get
            {
                var project = this;
                while (project.parentInfo != null)
                {
                    project = project.parentInfo;
                }
                return project.isRoot;
            }
        }

        private string subpath;
        public string Subpath
        {
            get { return subpath; }
            set { subpath = value; }
        }

        private readonly LinkedList<VssItemInfo> items = new LinkedList<VssItemInfo>();
        public IEnumerable<VssItemInfo> Items
        {
            get { return items; }
        }

        public VssProjectInfo(string physicalName, VssProjectInfo parentInfo, string subpath)
            : base(physicalName)
        {
            this.parentInfo = parentInfo;
            this.subpath = subpath;
        }

        public string GetPath()
        {
            if (IsRooted)
            {
                if (parentInfo != null)
                {
                    return Path.Combine(parentInfo.GetPath(), subpath);
                }
                else
                {
                    return subpath;
                }
            }
            return null;
        }

        public bool IsSameOrSubproject(VssProjectInfo parentInfo)
        {
            var project = this;
            while (project != null)
            {
                if (project == parentInfo)
                {
                    return true;
                }
                project = project.parentInfo;
            }
            return false;
        }

        public void AddItem(VssItemInfo item)
        {
            items.AddLast(item);
        }

        public void RemoveItem(VssItemInfo item)
        {
            items.Remove(item);
        }

        public bool ContainsFiles()
        {
            var subprojects = new LinkedList<VssProjectInfo>();
            var project = this;
            while (project != null)
            {
                foreach (var item in project.items)
                {
                    var subproject = item as VssProjectInfo;
                    if (subproject != null)
                    {
                        subprojects.AddLast(subproject);
                    }
                    else
                    {
                        return true;
                    }
                }
                if (subprojects.First != null)
                {
                    project = subprojects.First.Value;
                    subprojects.RemoveFirst();
                }
                else
                {
                    project = null;
                }
            }
            return false;
        }

        public IEnumerable<VssFileInfo> GetAllFiles()
        {
            var subprojects = new LinkedList<VssProjectInfo>();
            var project = this;
            while (project != null)
            {
                foreach (var item in project.items)
                {
                    var subproject = item as VssProjectInfo;
                    if (subproject != null)
                    {
                        subprojects.AddLast(subproject);
                    }
                    else
                    {
                        yield return (VssFileInfo)item;
                    }
                }
                if (subprojects.First != null)
                {
                    project = subprojects.First.Value;
                    subprojects.RemoveFirst();
                }
                else
                {
                    project = null;
                }
            }
        }
    }

    /// <summary>
    /// Represents the current state of a VSS file.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class VssFileInfo : VssItemInfo
    {
        private readonly List<VssProjectInfo> projects = new List<VssProjectInfo>();
        public IEnumerable<VssProjectInfo> Projects
        {
            get { return projects; }
        }

        private string logicalName;
        public string LogicalName
        {
            get { return logicalName; }
            set { logicalName = value; }
        }

        private int version = 1;
        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        public VssFileInfo(string physicalName, string logicalName)
            : base(physicalName)
        {
            this.logicalName = logicalName;
        }

        public void AddProject(VssProjectInfo project)
        {
            projects.Add(project);
        }

        public void RemoveProject(VssProjectInfo project)
        {
            projects.Remove(project);
        }
    }

    /// <summary>
    /// Tracks the names and locations of VSS projects and files as revisions are replayed.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class VssPathMapper
    {
        private readonly Dictionary<string, VssProjectInfo> projectInfos = new Dictionary<string, VssProjectInfo>();
        private readonly Dictionary<string, VssFileInfo> fileInfos = new Dictionary<string, VssFileInfo>();

        public bool IsProjectRooted(string project)
        {
            VssProjectInfo projectInfo;
            if (projectInfos.TryGetValue(project, out projectInfo))
            {
                return projectInfo.IsRooted;
            }
            return false;
        }

        public string GetProjectPath(string project)
        {
            VssProjectInfo projectInfo;
            if (projectInfos.TryGetValue(project, out projectInfo))
            {
                return projectInfo.GetPath();
            }
            return null;
        }

        public void SetProjectPath(string project, string path)
        {
            var projectInfo = new VssProjectInfo(project, null, path);
            projectInfo.IsRoot = true;
            projectInfos[project] = projectInfo;
        }

        public IEnumerable<VssFileInfo> GetAllFiles(string project)
        {
            VssProjectInfo projectInfo;
            if (projectInfos.TryGetValue(project, out projectInfo))
            {
                return projectInfo.GetAllFiles();
            }
            return null;
        }

        public IEnumerable<string> GetFilePaths(string file, string underProject)
        {
            var result = new LinkedList<string>();
            VssFileInfo fileInfo;
            if (fileInfos.TryGetValue(file, out fileInfo))
            {
                VssProjectInfo underProjectInfo = null;
                if (underProject != null)
                {
                    if (!projectInfos.TryGetValue(underProject, out underProjectInfo))
                    {
                        return result;
                    }
                }
                foreach (var project in fileInfo.Projects)
                {
                    if (underProjectInfo == null || project.IsSameOrSubproject(underProjectInfo))
                    {
                        // ignore projects that are not rooted
                        var projectPath = project.GetPath();
                        if (projectPath != null)
                        {
                            var path = Path.Combine(projectPath, fileInfo.LogicalName);
                            result.AddLast(path);
                        }
                    }
                }
            }
            return result;
        }

        public int GetFileVersion(string file)
        {
            VssFileInfo fileInfo;
            return fileInfos.TryGetValue(file, out fileInfo) ? fileInfo.Version : 1;
        }

        public void SetFileVersion(VssItemName name, int version)
        {
            VssFileInfo fileInfo = GetOrCreateFile(name);
            fileInfo.Version = version;
        }

        public VssItemInfo AddItem(VssItemName project, VssItemName name)
        {
            var parentInfo = GetOrCreateProject(project, null);
            VssItemInfo itemInfo;
            if (name.IsProject)
            {
                itemInfo = GetOrCreateProject(name, parentInfo);
            }
            else
            {
                var fileInfo = GetOrCreateFile(name);
                fileInfo.AddProject(parentInfo);
                itemInfo = fileInfo;
            }
            parentInfo.AddItem(itemInfo);
            return itemInfo;
        }

        public VssItemInfo RenameItem(VssItemName name)
        {
            VssItemInfo itemInfo;
            if (name.IsProject)
            {
                var projectInfo = GetOrCreateProject(name, null);
                projectInfo.Subpath = name.LogicalName;
                itemInfo = projectInfo;
            }
            else
            {
                var fileInfo = GetOrCreateFile(name);
                fileInfo.LogicalName = name.LogicalName;
                itemInfo = fileInfo;
            }
            return itemInfo;
        }

        public VssItemInfo DeleteItem(VssItemName project, VssItemName name)
        {
            var parentInfo = GetOrCreateProject(project, null);
            VssItemInfo itemInfo;
            if (name.IsProject)
            {
                itemInfo = GetOrCreateProject(name, null);
            }
            else
            {
                var fileInfo = GetOrCreateFile(name);
                fileInfo.RemoveProject(parentInfo);
                itemInfo = fileInfo;
            }
            parentInfo.RemoveItem(itemInfo);
            return itemInfo;
        }

        public VssItemInfo RecoverItem(VssItemName project, VssItemName name)
        {
            var parentInfo = GetOrCreateProject(project, null);
            VssItemInfo itemInfo;
            if (name.IsProject)
            {
                itemInfo = GetOrCreateProject(name, null);
            }
            else
            {
                var fileInfo = GetOrCreateFile(name);
                fileInfo.AddProject(parentInfo);
                itemInfo = fileInfo;
            }
            parentInfo.AddItem(itemInfo);
            return itemInfo;
        }

        public VssItemInfo PinItem(VssItemName project, VssItemName name)
        {
            // pinning removes the project from the list of
            // sharing projects, so it no longer receives edits
            return DeleteItem(project, name);
        }

        public VssItemInfo UnpinItem(VssItemName project, VssItemName name)
        {
            // unpinning restores the project to the list of
            // sharing projects, so it receives edits
            return RecoverItem(project, name);
        }

        public VssItemInfo BranchFile(VssItemName project, VssItemName newName, VssItemName oldName)
        {
            Debug.Assert(!newName.IsProject);
            Debug.Assert(!oldName.IsProject);

            var parentInfo = GetOrCreateProject(project, null);
            var oldFile = GetOrCreateFile(oldName);
            oldFile.RemoveProject(parentInfo);
            var newFile = GetOrCreateFile(newName);
            newFile.AddProject(parentInfo);
            newFile.Version = oldFile.Version;
            return newFile;
        }

        public VssProjectInfo MoveProjectFrom(VssItemName project, VssItemName subproject, string oldProjectSpec)
        {
            Debug.Assert(subproject.IsProject);

            var parentInfo = GetOrCreateProject(project, null);
            var subprojectInfo = GetOrCreateProject(subproject, null);
            subprojectInfo.Parent = parentInfo;
            return subprojectInfo;
        }

        public void MoveProjectTo(string project, VssItemName subproject, string newProjectSpec)
        {
            // currently ignored; rely on MoveProjectFrom
        }

        private VssProjectInfo GetOrCreateProject(VssItemName name, VssProjectInfo parentInfo)
        {
            VssProjectInfo projectInfo;
            if (!projectInfos.TryGetValue(name.PhysicalName, out projectInfo))
            {
                projectInfo = new VssProjectInfo(name.PhysicalName, parentInfo, name.LogicalName);
                projectInfos[name.PhysicalName] = projectInfo;
            }
            return projectInfo;
        }

        private VssFileInfo GetOrCreateFile(VssItemName name)
        {
            VssFileInfo fileInfo;
            if (!fileInfos.TryGetValue(name.PhysicalName, out fileInfo))
            {
                fileInfo = new VssFileInfo(name.PhysicalName, name.LogicalName);
                fileInfos[name.PhysicalName] = fileInfo;
            }
            return fileInfo;
        }

        private VssProjectInfo ResolveProjectSpec(string projectSpec)
        {
            if (!projectSpec.StartsWith("$/"))
            {
                throw new ArgumentException("Project spec must start with $/", "projectSpec");
            }

            // TODO
            throw new NotImplementedException();
        }

        public static string GetWorkingPath(string workingRoot, string vssPath)
        {
            if (vssPath == "$")
            {
                return workingRoot;
            }
            if (vssPath.StartsWith("$/"))
            {
                vssPath = vssPath.Substring(2);
            }
            var relPath = vssPath.Replace(VssDatabase.ProjectSeparatorChar, Path.DirectorySeparatorChar);
            return Path.Combine(workingRoot, relPath);
        }
    }
}

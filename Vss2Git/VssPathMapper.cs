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

        private string logicalName;
        public string LogicalName
        {
            get { return logicalName; }
            set { logicalName = value; }
        }

        private bool destroyed;
        public bool Destroyed
        {
            get { return destroyed; }
            set { destroyed = value; }
        }

        public VssItemInfo(string physicalName, string logicalName)
        {
            this.physicalName = physicalName;
            this.logicalName = logicalName;
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
                if (parentInfo != value)
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
        }

        private bool isRoot;
        public bool IsRoot
        {
            get { return isRoot; }
            set { isRoot = value; }
        }

        // valid only for root paths; used to resolve project specifiers
        private string originalVssPath;
        public string OriginalVssPath
        {
            get { return originalVssPath; }
            set { originalVssPath = value; }
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

        private readonly LinkedList<VssItemInfo> items = new LinkedList<VssItemInfo>();
        public IEnumerable<VssItemInfo> Items
        {
            get { return items; }
        }

        public VssProjectInfo(string physicalName, string logicalName)
            : base(physicalName, logicalName)
        {
        }

        public string GetPath()
        {
            if (IsRooted)
            {
                if (parentInfo != null)
                {
                    return Path.Combine(parentInfo.GetPath(), LogicalName);
                }
                else
                {
                    return LogicalName;
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

        public bool ContainsLogicalName(string logicalName)
        {
            foreach (var item in items)
            {
                if (item.LogicalName.Equals(logicalName))
                {
                    return true;
                }
            }
            return false;
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

        public IEnumerable<VssProjectInfo> GetAllProjects()
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
                        yield return subproject;
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

        private int version = 1;
        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        public VssFileInfo(string physicalName, string logicalName)
            : base(physicalName, logicalName)
        {
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
        // keyed by physical name
        private readonly Dictionary<string, VssProjectInfo> projectInfos = new Dictionary<string, VssProjectInfo>();
        private readonly Dictionary<string, VssProjectInfo> rootInfos = new Dictionary<string, VssProjectInfo>();
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

        public void SetProjectPath(string project, string path, string originalVssPath)
        {
            var projectInfo = new VssProjectInfo(project, path);
            projectInfo.IsRoot = true;
            projectInfo.OriginalVssPath = originalVssPath;
            projectInfos[project] = projectInfo;
            rootInfos[project] = projectInfo;
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

        public IEnumerable<VssProjectInfo> GetAllProjects(string project)
        {
            VssProjectInfo projectInfo;
            if (projectInfos.TryGetValue(project, out projectInfo))
            {
                return projectInfo.GetAllProjects();
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
            var parentInfo = GetOrCreateProject(project);
            VssItemInfo itemInfo;
            if (name.IsProject)
            {
                var projectInfo = GetOrCreateProject(name);
                projectInfo.Parent = parentInfo;
                itemInfo = projectInfo;
            }
            else
            {
                var fileInfo = GetOrCreateFile(name);
                fileInfo.AddProject(parentInfo);
                parentInfo.AddItem(fileInfo);
                itemInfo = fileInfo;
            }

            // update name of item in case it was created on demand by
            // an earlier unmapped item that was subsequently renamed
            itemInfo.LogicalName = name.LogicalName;

            return itemInfo;
        }

        public VssItemInfo RenameItem(VssItemName name)
        {
            VssItemInfo itemInfo;
            if (name.IsProject)
            {
                itemInfo = GetOrCreateProject(name);
            }
            else
            {
                itemInfo = GetOrCreateFile(name);
            }
            itemInfo.LogicalName = name.LogicalName;
            return itemInfo;
        }

        public VssItemInfo DeleteItem(VssItemName project, VssItemName name)
        {
            var parentInfo = GetOrCreateProject(project);
            VssItemInfo itemInfo;
            if (name.IsProject)
            {
                var projectInfo = GetOrCreateProject(name);
                projectInfo.Parent = null;
                itemInfo = projectInfo;
            }
            else
            {
                var fileInfo = GetOrCreateFile(name);
                fileInfo.RemoveProject(parentInfo);
                parentInfo.RemoveItem(fileInfo);
                itemInfo = fileInfo;
            }
            return itemInfo;
        }

        public VssItemInfo RecoverItem(VssItemName project, VssItemName name)
        {
            var parentInfo = GetOrCreateProject(project);
            VssItemInfo itemInfo;
            if (name.IsProject)
            {
                var projectInfo = GetOrCreateProject(name);
                projectInfo.Parent = parentInfo;
                itemInfo = projectInfo;
            }
            else
            {
                var fileInfo = GetOrCreateFile(name);
                fileInfo.AddProject(parentInfo);
                parentInfo.AddItem(fileInfo);
                itemInfo = fileInfo;
            }
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

            // "branching a file" (in VSS parlance) essentially moves it from
            // one project to another (and could potentially change its name)
            var parentInfo = GetOrCreateProject(project);

            // remove filename from old project
            var oldFile = GetOrCreateFile(oldName);
            oldFile.RemoveProject(parentInfo);
            parentInfo.RemoveItem(oldFile);

            // add filename to new project
            var newFile = GetOrCreateFile(newName);
            newFile.AddProject(parentInfo);
            parentInfo.AddItem(newFile);

            // retain version number from old file
            newFile.Version = oldFile.Version;

            return newFile;
        }

        public VssProjectInfo MoveProjectFrom(VssItemName project, VssItemName subproject, string oldProjectSpec)
        {
            Debug.Assert(subproject.IsProject);

            var parentInfo = GetOrCreateProject(project);
            var subprojectInfo = GetOrCreateProject(subproject);
            subprojectInfo.Parent = parentInfo;
            return subprojectInfo;
        }

        public VssProjectInfo MoveProjectTo(VssItemName project, VssItemName subproject, string newProjectSpec)
        {
            var subprojectInfo = GetOrCreateProject(subproject);
            var lastSlash = newProjectSpec.LastIndexOf('/');
            if (lastSlash > 0)
            {
                var newParentSpec = newProjectSpec.Substring(0, lastSlash);
                var parentInfo = ResolveProjectSpec(newParentSpec);
                if (parentInfo != null)
                {
                    // propagate the destroyed flag from the new parent
                    subprojectInfo.Parent = parentInfo;
                    subprojectInfo.Destroyed |= parentInfo.Destroyed;
                }
                else
                {
                    // if resolution fails, the target project has been destroyed
                    // or is outside the set of projects being mapped
                    subprojectInfo.Destroyed = true;
                }
            }
            return subprojectInfo;
        }

        public bool ProjectContainsLogicalName(VssItemName project, VssItemName name)
        {
            var parentInfo = GetOrCreateProject(project);
            return parentInfo.ContainsLogicalName(name.LogicalName);
        }

        private VssProjectInfo GetOrCreateProject(VssItemName name)
        {
            VssProjectInfo projectInfo;
            if (!projectInfos.TryGetValue(name.PhysicalName, out projectInfo))
            {
                projectInfo = new VssProjectInfo(name.PhysicalName, name.LogicalName);
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
                throw new ArgumentException("Project spec must start with $/ but was \"" + projectSpec + "\"", "projectSpec");
            }

            foreach (var rootInfo in rootInfos.Values)
            {
                if (projectSpec.StartsWith(rootInfo.OriginalVssPath))
                {
                    var rootLength = rootInfo.OriginalVssPath.Length;
                    if (!rootInfo.OriginalVssPath.EndsWith("/"))
                    {
                        ++rootLength;
                    }
                    var subpath = projectSpec.Substring(rootLength);
                    var subprojectNames = subpath.Split('/');
                    var projectInfo = rootInfo;
                    foreach (var subprojectName in subprojectNames)
                    {
                        var found = false;
                        foreach (var item in projectInfo.Items)
                        {
                            var subprojectInfo = item as VssProjectInfo;
                            if (subprojectInfo != null && subprojectInfo.LogicalName == subprojectName)
                            {
                                projectInfo = subprojectInfo;
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            goto NotFound;
                        }
                    }
                    return projectInfo;
                }
            }

        NotFound:
            return null;
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

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
using System.IO;
using System.Text;
using Hpdi.VssPhysicalLib;

namespace Hpdi.VssLogicalLib
{
    /// <summary>
    /// Represents a VSS database and provides access to the items it contains.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssDatabase
    {
        public const string RootProjectName = "$";
        public const string RootProjectFile = "AAAAAAAA";
        public const char ProjectSeparatorChar = '/';
        public const string ProjectSeparator = "/";

        private readonly string basePath;
        private readonly string iniPath;
        private readonly string dataPath;
        private readonly NameFile nameFile;
        private readonly VssProject rootProject;
        private readonly Encoding encoding;

        public string BasePath
        {
            get { return basePath; }
        }

        public string IniPath
        {
            get { return iniPath; }
        }

        public string DataPath
        {
            get { return dataPath; }
        }

        public VssProject RootProject
        {
            get { return rootProject; }
        }

        public Encoding Encoding
        {
            get { return encoding; }
        }

        public VssItem GetItem(string logicalPath)
        {
            var segments = logicalPath.Split(new char[] { ProjectSeparatorChar },
                StringSplitOptions.RemoveEmptyEntries);
            var index = segments[0] == RootProjectName ? 1 : 0;
            VssProject project = rootProject;
            while (index < segments.Length)
            {
                var name = segments[index++];

                var subproject = project.FindProject(name);
                if (subproject != null)
                {
                    project = subproject;
                    continue;
                }

                var file = project.FindFile(name);
                if (file != null)
                {
                    if (index == segments.Length)
                    {
                        return file;
                    }
                    else
                    {
                        var currentPath = string.Join(ProjectSeparator, segments, 0, index);
                        throw new VssPathException(string.Format("{0} is not a project", currentPath));
                    }
                }

                throw new VssPathException(string.Format("{0} not found in {1}", name, project.Path));
            }
            return project;
        }

        public VssItem GetItemPhysical(string physicalName)
        {
            physicalName = physicalName.ToUpper();

            if (physicalName == RootProjectFile)
            {
                return rootProject;
            }

            var physicalPath = GetDataPath(physicalName);
            var itemFile = new ItemFile(physicalPath, encoding);
            var isProject = (itemFile.Header.ItemType == ItemType.Project);
            var logicalName = GetFullName(itemFile.Header.Name);
            var itemName = new VssItemName(logicalName, physicalName, isProject);
            VssItem item;
            if (isProject)
            {
                var parentFile = ((ProjectHeaderRecord)itemFile.Header).ParentFile;
                var parent = (VssProject)GetItemPhysical(parentFile);
                var logicalPath = BuildPath(parent, logicalName);
                item = new VssProject(this, itemName, physicalPath, logicalPath);
            }
            else
            {
                item = new VssFile(this, itemName, physicalPath);
            }
            item.ItemFile = itemFile;
            return item;
        }

        public bool ItemExists(string physicalName)
        {
            var physicalPath = GetDataPath(physicalName);
            return File.Exists(physicalPath);
        }

        internal VssDatabase(string path, Encoding encoding)
        {
            this.basePath = path;
            this.encoding = encoding;

            iniPath = Path.Combine(path, "srcsafe.ini");
            var iniReader = new SimpleIniReader(iniPath);
            iniReader.Parse();

            dataPath = Path.Combine(path, iniReader.GetValue("Data_Path", "data"));

            var namesPath = Path.Combine(dataPath, "names.dat");
            nameFile = new NameFile(namesPath, encoding);

            rootProject = OpenProject(null, RootProjectFile, RootProjectName);
        }

        internal VssProject OpenProject(VssProject parent, string physicalName, string logicalName)
        {
            var itemName = new VssItemName(logicalName, physicalName, true);
            var logicalPath = BuildPath(parent, logicalName);
            var physicalPath = GetDataPath(physicalName);
            return new VssProject(this, itemName, physicalPath, logicalPath);
        }

        internal VssFile OpenFile(string physicalName, string logicalName)
        {
            var itemName = new VssItemName(logicalName, physicalName, false);
            var physicalPath = GetDataPath(physicalName);
            return new VssFile(this, itemName, physicalPath);
        }

        private static string BuildPath(VssProject parent, string logicalName)
        {
            return (parent != null) ? parent.Path + ProjectSeparator + logicalName : logicalName;
        }

        internal string GetDataPath(string physicalName)
        {
            return Path.Combine(Path.Combine(dataPath, physicalName.Substring(0, 1)), physicalName);
        }

        internal string GetFullName(VssName name)
        {
            if (name.NameFileOffset != 0)
            {
                var nameRecord = nameFile.GetName(name.NameFileOffset);
                var nameIndex = nameRecord.IndexOf(name.IsProject ? NameKind.Project : NameKind.Long);
                if (nameIndex >= 0)
                {
                    return nameRecord.GetName(nameIndex);
                }
            }
            return name.ShortName;
        }

        internal VssItemName GetItemName(VssName name, string physicalName)
        {
            return new VssItemName(GetFullName(name), physicalName, name.IsProject);
        }
    }
}

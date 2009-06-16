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
using System.Collections;
using System.Collections.Generic;
using Hpdi.VssPhysicalLib;

namespace Hpdi.VssLogicalLib
{
    /// <summary>
    /// Represents a VSS file.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssFile : VssItem
    {
        public bool IsLocked
        {
            get { return (Header.Flags & FileFlags.Locked) != 0; }
        }

        public bool IsBinary
        {
            get { return (Header.Flags & FileFlags.Binary) != 0; }
        }

        public bool IsLatestOnly
        {
            get { return (Header.Flags & FileFlags.LatestOnly) != 0; }
        }

        public bool IsShared
        {
            get { return (Header.Flags & FileFlags.Shared) != 0; }
        }

        public bool IsCheckedOut
        {
            get { return (Header.Flags & FileFlags.CheckedOut) != 0; }
        }

        public uint Crc
        {
            get { return Header.DataCrc; }
        }

        public DateTime LastRevised
        {
            get { return Header.LastRevDateTime; }
        }

        public DateTime LastModified
        {
            get { return Header.ModificationDateTime; }
        }

        public DateTime Created
        {
            get { return Header.CreationDateTime; }
        }

        public new IEnumerable<VssFileRevision> Revisions
        {
            get { return new VssRevisions<VssFile, VssFileRevision>(this); }
        }

        public new VssFileRevision GetRevision(int version)
        {
            return (VssFileRevision)base.GetRevision(version);
        }

        internal FileHeaderRecord Header
        {
            get { return (FileHeaderRecord)ItemFile.Header; }
        }

        internal VssFile(VssDatabase database, VssItemName itemName, string physicalPath)
            : base(database, itemName, physicalPath)
        {
        }

        public string GetPath(VssProject project)
        {
            return project.Path + VssDatabase.ProjectSeparator + Name;
        }

        protected override VssRevision CreateRevision(RevisionRecord revision, CommentRecord comment)
        {
            return new VssFileRevision(this, revision, comment);
        }
    }
}

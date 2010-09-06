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
    /// Represents an abstract VSS item, which is a project or file.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public abstract class VssItem
    {
        protected readonly VssDatabase database;
        protected readonly VssItemName itemName;
        protected readonly string physicalPath;
        private ItemFile itemFile;

        public VssDatabase Database
        {
            get { return database; }
        }

        public VssItemName ItemName
        {
            get { return itemName; }
        }

        public bool IsProject
        {
            get { return itemName.IsProject; }
        }

        public string Name
        {
            get { return itemName.LogicalName; }
        }

        public string PhysicalName
        {
            get { return itemName.PhysicalName; }
        }

        public string PhysicalPath
        {
            get { return physicalPath; }
        }

        public string DataPath
        {
            get { return physicalPath + ItemFile.Header.DataExt; }
        }

        public int RevisionCount
        {
            get { return ItemFile.Header.Revisions; }
        }

        public IEnumerable<VssRevision> Revisions
        {
            get { return new VssRevisions<VssItem, VssRevision>(this); }
        }

        public VssRevision GetRevision(int version)
        {
            var itemFile = ItemFile;
            if (version < 1 || version > itemFile.Header.Revisions)
            {
                throw new ArgumentOutOfRangeException("version", version, "Invalid version number");
            }

            // check whether version was before branch
            if (version < itemFile.Header.FirstRevision)
            {
                if (!IsProject)
                {
                    var fileHeader = (FileHeaderRecord)itemFile.Header;
                    return database.GetItemPhysical(fileHeader.BranchFile).GetRevision(version);
                }
                else
                {
                    // should never happen; projects cannot branch
                    throw new ArgumentOutOfRangeException("version", version, "Undefined version");
                }
            }

            var revisionRecord = itemFile.GetFirstRevision();
            while (revisionRecord != null && revisionRecord.Revision < version)
            {
                revisionRecord = itemFile.GetNextRevision(revisionRecord);
            }
            if (revisionRecord == null)
            {
                throw new ArgumentException("Version not found", "version");
            }
            return CreateRevision(revisionRecord);
        }

        internal ItemFile ItemFile
        {
            get
            {
                if (itemFile == null)
                {
                    itemFile = new ItemFile(physicalPath, database.Encoding);
                }
                return itemFile;
            }
            set
            {
                itemFile = value;
            }
        }

        internal VssItem(VssDatabase database, VssItemName itemName, string physicalPath)
        {
            this.database = database;
            this.itemName = itemName;
            this.physicalPath = physicalPath;
        }

        protected VssRevision CreateRevision(RevisionRecord revision)
        {
            CommentRecord comment = null;
            if (revision.CommentLength > 0 && revision.CommentOffset > 0)
            {
                comment = new CommentRecord();
                ItemFile.ReadRecord(comment, revision.CommentOffset);
            }
            else if (revision.Action == VssPhysicalLib.Action.Label &&
                revision.LabelCommentLength > 0 && revision.LabelCommentOffset > 0)
            {
                comment = new CommentRecord();
                ItemFile.ReadRecord(comment, revision.LabelCommentOffset);
            }
            return CreateRevision(revision, comment);
        }

        protected abstract VssRevision CreateRevision(RevisionRecord revision, CommentRecord comment);

        protected class VssRevisions<ItemT, RevisionT> : IEnumerable<RevisionT>
            where ItemT : VssItem
            where RevisionT : VssRevision
        {
            private readonly ItemT item;

            internal VssRevisions(ItemT item)
            {
                this.item = item;
            }

            public IEnumerator<RevisionT> GetEnumerator()
            {
                return new VssRevisionEnumerator<ItemT, RevisionT>(item);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private class VssRevisionEnumerator<ItemT, RevisionT> : IEnumerator<RevisionT>
            where ItemT : VssItem
            where RevisionT : VssRevision
        {
            private readonly ItemT item;
            private RevisionRecord revisionRecord;
            private RevisionT revision;
            private bool beforeFirst = true;

            internal VssRevisionEnumerator(ItemT item)
            {
                this.item = item;
            }

            public void Dispose()
            {
            }

            public void Reset()
            {
                beforeFirst = true;
            }

            public bool MoveNext()
            {
                revision = null;
                if (beforeFirst)
                {
                    revisionRecord = item.ItemFile.GetFirstRevision();
                    beforeFirst = false;
                }
                else if (revisionRecord != null)
                {
                    revisionRecord = item.ItemFile.GetNextRevision(revisionRecord);
                }
                return revisionRecord != null;
            }

            public RevisionT Current
            {
                get
                {
                    if (revisionRecord == null)
                    {
                        throw new InvalidOperationException();
                    }

                    if (revision == null)
                    {
                        revision = (RevisionT)item.CreateRevision(revisionRecord);
                    }

                    return revision;
                }
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }
        }
    }
}

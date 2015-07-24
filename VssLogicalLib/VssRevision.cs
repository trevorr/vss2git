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
using Hpdi.VssPhysicalLib;

namespace Hpdi.VssLogicalLib
{
    /// <summary>
    /// Base class for revisions to a VSS item.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public abstract class VssRevision
    {
        protected readonly VssItem item;
        protected readonly VssAction action;
        protected readonly RevisionRecord revision;
        protected readonly CommentRecord comment;

        public VssItem Item
        {
            get { return item; }
        }

        public VssAction Action
        {
            get { return action; }
        }

        public int Version
        {
            get { return revision.Revision; }
        }

        public DateTime DateTime
        {
            get { return revision.DateTime; }
        }

        public string User
        {
            get { return revision.User; }
        }

        public string Label
        {
            get { return revision.Label; }
        }

        public string Comment
        {
            get { return comment != null ? comment.Comment : null; }
        }

        // use only to fix wrong values, please
        public void FixDateTime(DateTime fixedTime)
        {
            revision.FixDateTime(fixedTime);
        }

        internal VssRevision(VssItem item, RevisionRecord revision, CommentRecord comment)
        {
            this.item = item;
            this.action = CreateAction(revision, item);
            this.revision = revision;
            this.comment = comment;
        }

        private static VssAction CreateAction(RevisionRecord revision, VssItem item)
        {
            var db = item.Database;
            switch (revision.Action)
            {
                case Hpdi.VssPhysicalLib.Action.Label:
                    {
                        return new VssLabelAction(revision.Label);
                    }
                case Hpdi.VssPhysicalLib.Action.DestroyProject:
                case Hpdi.VssPhysicalLib.Action.DestroyFile:
                    {
                        var destroy = (DestroyRevisionRecord)revision;
                        return new VssDestroyAction(db.GetItemName(destroy.Name, destroy.Physical));
                    }
                case Hpdi.VssPhysicalLib.Action.RenameProject:
                case Hpdi.VssPhysicalLib.Action.RenameFile:
                    {
                        var rename = (RenameRevisionRecord)revision;
                        return new VssRenameAction(db.GetItemName(rename.Name, rename.Physical),
                            db.GetFullName(rename.OldName));
                    }
                case Hpdi.VssPhysicalLib.Action.MoveFrom:
                    {
                        var moveFrom = (MoveRevisionRecord)revision;
                        return new VssMoveFromAction(db.GetItemName(moveFrom.Name, moveFrom.Physical),
                            moveFrom.ProjectPath);
                    }
                case Hpdi.VssPhysicalLib.Action.MoveTo:
                    {
                        var moveTo = (MoveRevisionRecord)revision;
                        return new VssMoveToAction(db.GetItemName(moveTo.Name, moveTo.Physical),
                            moveTo.ProjectPath);
                    }
                case Hpdi.VssPhysicalLib.Action.ShareFile:
                    {
                        var share = (ShareRevisionRecord)revision;
                        return new VssShareAction(db.GetItemName(share.Name, share.Physical),
                            share.ProjectPath);
                    }
                case Hpdi.VssPhysicalLib.Action.BranchFile:
                case Hpdi.VssPhysicalLib.Action.CreateBranch:
                    {
                        var branch = (BranchRevisionRecord)revision;
                        var name = db.GetFullName(branch.Name);
                        return new VssBranchAction(
                            new VssItemName(name, branch.Physical, branch.Name.IsProject),
                            new VssItemName(name, branch.BranchFile, branch.Name.IsProject));
                    }
                case Hpdi.VssPhysicalLib.Action.EditFile:
                    {
                        return new VssEditAction(item.PhysicalName);
                    }
                case Hpdi.VssPhysicalLib.Action.CreateProject:
                case Hpdi.VssPhysicalLib.Action.CreateFile:
                    {
                        var create = (CommonRevisionRecord)revision;
                        return new VssCreateAction(db.GetItemName(create.Name, create.Physical));
                    }
                case Hpdi.VssPhysicalLib.Action.AddProject:
                case Hpdi.VssPhysicalLib.Action.AddFile:
                    {
                        var add = (CommonRevisionRecord)revision;
                        return new VssAddAction(db.GetItemName(add.Name, add.Physical));
                    }
                case Hpdi.VssPhysicalLib.Action.DeleteProject:
                case Hpdi.VssPhysicalLib.Action.DeleteFile:
                    {
                        var delete = (CommonRevisionRecord)revision;
                        return new VssDeleteAction(db.GetItemName(delete.Name, delete.Physical));
                    }
                case Hpdi.VssPhysicalLib.Action.RecoverProject:
                case Hpdi.VssPhysicalLib.Action.RecoverFile:
                    {
                        var recover = (CommonRevisionRecord)revision;
                        return new VssRecoverAction(db.GetItemName(recover.Name, recover.Physical));
                    }
                case Hpdi.VssPhysicalLib.Action.ArchiveProject:
                    {
                        var archive = (ArchiveRevisionRecord)revision;
                        return new VssArchiveAction(db.GetItemName(archive.Name, archive.Physical),
                            archive.ArchivePath);
                    }
                case Hpdi.VssPhysicalLib.Action.RestoreProject:
                    {
                        var archive = (ArchiveRevisionRecord)revision;
                        return new VssRestoreAction(db.GetItemName(archive.Name, archive.Physical),
                            archive.ArchivePath);
                    }
                default:
                    throw new ArgumentException("Unknown revision action: " + revision.Action);
            }
        }
    }
}

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

namespace Hpdi.VssLogicalLib
{
    /// <summary>
    /// Enumeration of logical VSS revision actions.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public enum VssActionType
    {
        Label,
        Create,
        Destroy,
        Add,
        Delete,
        Recover,
        Rename,
        MoveFrom,
        MoveTo,
        Share,
        Pin,
        Branch,
        Edit,
        Archive,
        Restore
    }

    /// <summary>
    /// Base class for VSS revision action descriptions.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public abstract class VssAction
    {
        public abstract VssActionType Type { get; }
    }

    /// <summary>
    /// Represents a VSS label action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssLabelAction : VssAction
    {
        public override VssActionType Type { get { return VssActionType.Label; } }

        private readonly string label;
        public string Label
        {
            get { return label; }
        }

        public VssLabelAction(string label)
        {
            this.label = label;
        }

        public override string ToString()
        {
            return "Label " + label;
        }
    }

    /// <summary>
    /// Base class for VSS project actions that target a particular item.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public abstract class VssNamedAction : VssAction
    {
        protected readonly VssItemName name;
        public VssItemName Name
        {
            get { return name; }
        }

        public VssNamedAction(VssItemName name)
        {
            this.name = name;
        }
    }

    /// <summary>
    /// Represents a VSS project/file destroy action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssDestroyAction : VssNamedAction
    {
        public override VssActionType Type { get { return VssActionType.Destroy; } }

        public VssDestroyAction(VssItemName name)
            : base(name)
        {
        }

        public override string ToString()
        {
            return string.Format("Destroy {0}", name);
        }
    }

    /// <summary>
    /// Represents a VSS project/file create action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssCreateAction : VssNamedAction
    {
        public override VssActionType Type { get { return VssActionType.Create; } }

        public VssCreateAction(VssItemName name)
            : base(name)
        {
        }

        public override string ToString()
        {
            return string.Format("Create {0}", name);
        }
    }

    /// <summary>
    /// Represents a VSS project/file add action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssAddAction : VssNamedAction
    {
        public override VssActionType Type { get { return VssActionType.Add; } }

        public VssAddAction(VssItemName name)
            : base(name)
        {
        }

        public override string ToString()
        {
            return string.Format("Add {0}", name);
        }
    }

    /// <summary>
    /// Represents a VSS project/file delete action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssDeleteAction : VssNamedAction
    {
        public override VssActionType Type { get { return VssActionType.Delete; } }

        public VssDeleteAction(VssItemName name)
            : base(name)
        {
        }

        public override string ToString()
        {
            return string.Format("Delete {0}", name);
        }
    }

    /// <summary>
    /// Represents a VSS project/file recover action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssRecoverAction : VssNamedAction
    {
        public override VssActionType Type { get { return VssActionType.Recover; } }

        public VssRecoverAction(VssItemName name)
            : base(name)
        {
        }

        public override string ToString()
        {
            return string.Format("Recover {0}", name);
        }
    }

    /// <summary>
    /// Represents a VSS project/file rename action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssRenameAction : VssNamedAction
    {
        public override VssActionType Type { get { return VssActionType.Rename; } }

        private readonly string originalName;
        public string OriginalName
        {
            get { return originalName; }
        }

        public VssRenameAction(VssItemName name, string originalName)
            : base(name)
        {
            this.originalName = originalName;
        }

        public override string ToString()
        {
            return string.Format("Rename {0} to {1}", originalName, Name);
        }
    }

    /// <summary>
    /// Represents a VSS project move-from action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssMoveFromAction : VssNamedAction
    {
        public override VssActionType Type { get { return VssActionType.MoveFrom; } }

        private readonly string originalProject;
        public string OriginalProject
        {
            get { return originalProject; }
        }

        public VssMoveFromAction(VssItemName name, string originalProject)
            : base(name)
        {
            this.originalProject = originalProject;
        }

        public override string ToString()
        {
            return string.Format("Move {0} from {1}", Name, originalProject);
        }
    }

    /// <summary>
    /// Represents a VSS project move-to action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssMoveToAction : VssNamedAction
    {
        public override VssActionType Type { get { return VssActionType.MoveTo; } }

        private readonly string newProject;
        public string NewProject
        {
            get { return newProject; }
        }

        public VssMoveToAction(VssItemName name, string newProject)
            : base(name)
        {
            this.newProject = newProject;
        }

        public override string ToString()
        {
            return string.Format("Move {0} to {1}", Name, newProject);
        }
    }

    /// <summary>
    /// Represents a VSS file share action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssShareAction : VssNamedAction
    {
        public override VssActionType Type { get { return VssActionType.Share; } }

        private readonly string originalProject;
        public string OriginalProject
        {
            get { return originalProject; }
        }

        public VssShareAction(VssItemName name, string originalProject)
            : base(name)
        {
            this.originalProject = originalProject;
        }

        public override string ToString()
        {
            return string.Format("Share {0} from {1}", Name, originalProject);
        }
    }

    /// <summary>
    /// Represents a VSS file pin/unpin action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssPinAction : VssNamedAction
    {
        public override VssActionType Type { get { return VssActionType.Pin; } }

        private readonly bool pinned;
        public bool Pinned
        {
            get { return pinned; }
        }

        private readonly int revision;
        public int Revision
        {
            get { return revision; }
        }

        public VssPinAction(VssItemName name, bool pinned, int revision)
            : base(name)
        {
            this.pinned = pinned;
            this.revision = revision;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} at revision {2}", pinned ? "Pin " : "Unpin ", Name, revision);
        }
    }

    /// <summary>
    /// Represents a VSS file branch action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssBranchAction : VssNamedAction
    {
        public override VssActionType Type { get { return VssActionType.Branch; } }

        private readonly VssItemName source;
        public VssItemName Source
        {
            get { return source; }
        }

        public VssBranchAction(VssItemName name, VssItemName source)
            : base(name)
        {
            this.source = source;
        }

        public override string ToString()
        {
            return string.Format("Branch {0} from {1}", Name, source.PhysicalName);
        }
    }

    /// <summary>
    /// Represents a VSS file edit action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssEditAction : VssAction
    {
        public override VssActionType Type { get { return VssActionType.Edit; } }

        private readonly string physicalName;
        public string PhysicalName
        {
            get { return physicalName; }
        }

        public VssEditAction(string physicalName)
        {
            this.physicalName = physicalName;
        }

        public override string ToString()
        {
            return "Edit " + physicalName;
        }
    }

    /// <summary>
    /// Represents a VSS archive action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssArchiveAction : VssNamedAction
    {
        public override VssActionType Type { get { return VssActionType.Archive; } }

        private readonly string archivePath;
        public string ArchivePath
        {
            get { return archivePath; }
        }

        public VssArchiveAction(VssItemName name, string archivePath)
            : base(name)
        {
            this.archivePath = archivePath;
        }

        public override string ToString()
        {
            return string.Format("Archive {0} to {1}", Name, archivePath);
        }
    }

    /// <summary>
    /// Represents a VSS restore from archive action.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssRestoreAction : VssNamedAction
    {
        public override VssActionType Type { get { return VssActionType.Restore; } }

        private readonly string archivePath;
        public string ArchivePath
        {
            get { return archivePath; }
        }

        public VssRestoreAction(VssItemName name, string archivePath)
            : base(name)
        {
            this.archivePath = archivePath;
        }

        public override string ToString()
        {
            return string.Format("Restore {0} from archive {1}", Name, archivePath);
        }
    }
}

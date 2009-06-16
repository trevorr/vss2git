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

namespace Hpdi.VssPhysicalLib
{
    /// <summary>
    /// Flags enumeration for items in project.
    /// </summary>
    /// <author>Trevor Robinson</author>
    [Flags]
    public enum ProjectEntryFlags
    {
        None,
        Deleted = 0x01,
        Binary = 0x02,
        LatestOnly = 0x04,
        Shared = 0x08,
    }

    /// <summary>
    /// VSS record for representing an item stored in particular project.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class ProjectEntryRecord : VssRecord
    {
        public const string SIGNATURE = "JP";

        protected ItemType itemType;
        protected ProjectEntryFlags flags;
        protected VssName name;
        protected int pinnedVersion;
        protected string physical;

        public override string Signature { get { return SIGNATURE; } }
        public ItemType ItemType { get { return itemType; } }
        public ProjectEntryFlags Flags { get { return flags; } }
        public VssName Name { get { return name; } }
        public int PinnedVersion { get { return pinnedVersion; } }
        public string Physical { get { return physical; } }

        public override void Read(BufferReader reader, RecordHeader header)
        {
            base.Read(reader, header);

            itemType = (ItemType)reader.ReadInt16();
            flags = (ProjectEntryFlags)reader.ReadInt16();
            name = reader.ReadName();
            pinnedVersion = reader.ReadInt16();
            physical = reader.ReadString(10);
        }

        public override void Dump(TextWriter writer)
        {
            writer.WriteLine("  Item Type: {0} - Name: {1} ({2})",
                itemType, name.ShortName, physical);
            writer.WriteLine("  Flags: {0}", flags);
            writer.WriteLine("  Pinned version: {0}", pinnedVersion);
        }
    }
}

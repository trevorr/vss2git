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
    /// Flags enumeration for a VSS file.
    /// </summary>
    /// <author>Trevor Robinson</author>
    [Flags]
    public enum FileFlags
    {
        None,
        Locked = 0x01,
        Binary = 0x02,
        LatestOnly = 0x04,
        Shared = 0x20,
        CheckedOut = 0x40,
    }

    /// <summary>
    /// VSS header record for a file.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class FileHeaderRecord : ItemHeaderRecord
    {
        FileFlags flags;
        string branchFile;
        int branchOffset;
        int projectOffset;
        int branchCount;
        int projectCount;
        int firstCheckoutOffset;
        int lastCheckoutOffset;
        uint dataCrc;
        DateTime lastRevDateTime;
        DateTime modificationDateTime;
        DateTime creationDateTime;

        public FileFlags Flags { get { return flags; } }
        public string BranchFile { get { return branchFile; } }
        public int BranchOffset { get { return branchOffset; } }
        public int ProjectOffset { get { return projectOffset; } }
        public int BranchCount { get { return branchCount; } }
        public int ProjectCount { get { return projectCount; } }
        public int FirstCheckoutOffset { get { return firstCheckoutOffset; } }
        public int LastCheckoutOffset { get { return lastCheckoutOffset; } }
        public uint DataCrc { get { return dataCrc; } }
        public DateTime LastRevDateTime { get { return lastRevDateTime; } }
        public DateTime ModificationDateTime { get { return modificationDateTime; } }
        public DateTime CreationDateTime { get { return creationDateTime; } }

        public FileHeaderRecord()
            : base(ItemType.File)
        {
        }

        public override void Read(BufferReader reader, RecordHeader header)
        {
            base.Read(reader, header);

            flags = (FileFlags)reader.ReadInt16();
            branchFile = reader.ReadString(8);
            reader.Skip(2); // reserved; always 0
            branchOffset = reader.ReadInt32();
            projectOffset = reader.ReadInt32();
            branchCount = reader.ReadInt16();
            projectCount = reader.ReadInt16();
            firstCheckoutOffset = reader.ReadInt32();
            lastCheckoutOffset = reader.ReadInt32();
            dataCrc = (uint)reader.ReadInt32();
            reader.Skip(8); // reserved; always 0
            lastRevDateTime = reader.ReadDateTime();
            modificationDateTime = reader.ReadDateTime();
            creationDateTime = reader.ReadDateTime();
            // remaining appears to be trash
        }

        public override void Dump(TextWriter writer)
        {
            base.Dump(writer);

            writer.WriteLine("  Flags: {0}", flags);
            writer.WriteLine("  Branched from file: {0}", branchFile);
            writer.WriteLine("  Branch offset: {0:X6}", branchOffset);
            writer.WriteLine("  Branch count: {0}", branchCount);
            writer.WriteLine("  Project offset: {0:X6}", projectOffset);
            writer.WriteLine("  Project count: {0}", projectCount);
            writer.WriteLine("  First/last checkout offset: {0:X6}/{1:X6}",
                firstCheckoutOffset, lastCheckoutOffset);
            writer.WriteLine("  Data CRC: {0:X8}", dataCrc);
            writer.WriteLine("  Last revision time: {0}", lastRevDateTime);
            writer.WriteLine("  Modification time: {0}", modificationDateTime);
            writer.WriteLine("  Creation time: {0}", creationDateTime);
        }
    }
}

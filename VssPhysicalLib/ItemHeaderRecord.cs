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

using System.IO;

namespace Hpdi.VssPhysicalLib
{
    /// <summary>
    /// Enumeration indicating whether an item is a project or a file.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public enum ItemType
    {
        Project = 1,
        File = 2,
    }

    /// <summary>
    /// Base class for item VSS header records.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class ItemHeaderRecord : VssRecord
    {
        public const string SIGNATURE = "DH";

        protected ItemType itemType;
        protected int revisions;
        protected VssName name;
        protected int firstRevision;
        protected string dataExt;
        protected int firstRevOffset;
        protected int lastRevOffset;
        protected int eofOffset;
        protected int rightsOffset;


        public override string Signature { get { return SIGNATURE; } }
        public ItemType ItemType { get { return itemType; } }
        public int Revisions { get { return revisions; } }
        public VssName Name { get { return name; } }
        public int FirstRevision { get { return firstRevision; } }
        public string DataExt { get { return dataExt; } }
        public int FirstRevOffset { get { return firstRevOffset; } }
        public int LastRevOffset { get { return lastRevOffset; } }
        public int EofOffset { get { return eofOffset; } }
        public int RightsOffset { get { return rightsOffset; } }

        public ItemHeaderRecord(ItemType itemType)
        {
            this.itemType = itemType;
        }

        public override void Read(BufferReader reader, RecordHeader header)
        {
            base.Read(reader, header);

            itemType = (ItemType)reader.ReadInt16();
            revisions = reader.ReadInt16();
            name = reader.ReadName();
            firstRevision = reader.ReadInt16();
            dataExt = reader.ReadString(2);
            firstRevOffset = reader.ReadInt32();
            lastRevOffset = reader.ReadInt32();
            eofOffset = reader.ReadInt32();
            rightsOffset = reader.ReadInt32();
            reader.Skip(16); // reserved; always 0
        }

        public override void Dump(TextWriter writer)
        {
            writer.WriteLine("  Item Type: {0} - Revisions: {1} - Name: {2}",
                itemType, revisions, name.ShortName);
            writer.WriteLine("  Name offset: {0:X6}", name.NameFileOffset);
            writer.WriteLine("  First revision: #{0:D3}", firstRevision);
            writer.WriteLine("  Data extension: {0}", dataExt);
            writer.WriteLine("  First/last rev offset: {0:X6}/{1:X6}",
                firstRevOffset, lastRevOffset);
            writer.WriteLine("  EOF offset: {0:X6}", eofOffset);
            writer.WriteLine("  Rights offset: {0:X8}", rightsOffset);
        }
    }
}

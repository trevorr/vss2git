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
    /// Enumeration of the kinds of VSS logical item names.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public enum NameKind
    {
        Dos = 1, // DOS 8.3 filename
        Long = 2, // Win95/NT long filename
        MacOS = 3, // Mac OS 9 and earlier 31-character filename
        Project = 10 // VSS project name
    }

    /// <summary>
    /// VSS record containing the logical names of an object in particular contexts.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class NameRecord : VssRecord
    {
        public const string SIGNATURE = "SN";

        int kindCount;
        NameKind[] kinds;
        string[] names;

        public override string Signature { get { return SIGNATURE; } }
        public int KindCount { get { return kindCount; } }

        public int IndexOf(NameKind kind)
        {
            for (int i = 0; i < kindCount; ++i)
            {
                if (kinds[i] == kind)
                {
                    return i;
                }
            }
            return -1;
        }

        public NameKind GetKind(int index)
        {
            return kinds[index];
        }

        public string GetName(int index)
        {
            return names[index];
        }

        public override void Read(BufferReader reader, RecordHeader header)
        {
            base.Read(reader, header);
            
            kindCount = reader.ReadInt16();
            reader.Skip(2); // unknown
            kinds = new NameKind[kindCount];
            names = new string[kindCount];
            var baseOffset = reader.Offset + (kindCount * 4);
            for (int i = 0; i < kindCount; ++i)
            {
                kinds[i] = (NameKind)reader.ReadInt16();
                var nameOffset = reader.ReadInt16();
                var saveOffset = reader.Offset;
                try
                {
                    reader.Offset = baseOffset + nameOffset;
                    names[i] = reader.ReadString(reader.Remaining);
                }
                finally
                {
                    reader.Offset = saveOffset;
                }
            }
        }

        public override void Dump(TextWriter writer)
        {
            for (int i = 0; i < kindCount; ++i)
            {
                writer.WriteLine("  {0} name: {1}", kinds[i], names[i]);
            }
        }
    }
}

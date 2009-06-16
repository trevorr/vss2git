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
    /// VSS record representing a file checkout.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class CheckoutRecord : VssRecord
    {
        public const string SIGNATURE = "CF";

        string user;
        DateTime dateTime;
        string workingDir;
        string machine;
        string project;
        string comment;
        int revision;
        int flags;
        bool exclusive;
        int prevCheckoutOffset;
        int thisCheckoutOffset;
        int checkouts;

        public override string Signature { get { return SIGNATURE; } }
        public string User { get { return user; } }
        public DateTime DateTime { get { return dateTime; } }
        public string WorkingDir { get { return workingDir; } }
        public string Machine { get { return machine; } }
        public string Project { get { return project; } }
        public string Comment { get { return comment; } }
        public int Revision { get { return revision; } }
        public int Flags { get { return flags; } }
        public bool Exclusive { get { return exclusive; } }
        public int PrevCheckoutOffset { get { return prevCheckoutOffset; } }
        public int ThisCheckoutOffset { get { return thisCheckoutOffset; } }
        public int Checkouts { get { return checkouts; } }

        public override void Read(BufferReader reader, RecordHeader header)
        {
            base.Read(reader, header);

            user = reader.ReadString(32);
            dateTime = reader.ReadDateTime();
            workingDir = reader.ReadString(260);
            machine = reader.ReadString(32);
            project = reader.ReadString(260);
            comment = reader.ReadString(64);
            revision = reader.ReadInt16();
            flags = reader.ReadInt16();
            exclusive = (flags & 0x40) != 0;
            prevCheckoutOffset = reader.ReadInt32();
            thisCheckoutOffset = reader.ReadInt32();
            checkouts = reader.ReadInt32();
        }

        public override void Dump(TextWriter writer)
        {
            writer.WriteLine("  User: {0} @ {1}", user, dateTime);
            writer.WriteLine("  Working: {0}", workingDir);
            writer.WriteLine("  Machine: {0}", machine);
            writer.WriteLine("  Project: {0}", project);
            writer.WriteLine("  Comment: {0}", comment);
            writer.WriteLine("  Revision: #{0:D3}", revision);
            writer.WriteLine("  Flags: {0:X4}{1}", flags,
                exclusive ? " (exclusive)" : "");
            writer.WriteLine("  Prev checkout offset: {0:X6}", prevCheckoutOffset);
            writer.WriteLine("  This checkout offset: {0:X6}", thisCheckoutOffset);
            writer.WriteLine("  Checkouts: {0}", checkouts);
        }
    }
}

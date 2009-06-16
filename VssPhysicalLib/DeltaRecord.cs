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

using System.Collections.Generic;
using System.IO;

namespace Hpdi.VssPhysicalLib
{
    /// <summary>
    /// VSS record representing a reverse-delta for a file revision.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class DeltaRecord : VssRecord
    {
        public const string SIGNATURE = "FD";

        private readonly LinkedList<DeltaOperation> operations =
            new LinkedList<DeltaOperation>();

        public override string Signature { get { return SIGNATURE; } }
        public IEnumerable<DeltaOperation> Operations { get { return operations; } }

        public override void Read(BufferReader reader, RecordHeader header)
        {
            base.Read(reader, header);

            for (; ; )
            {
                DeltaOperation operation = new DeltaOperation();
                operation.Read(reader);
                if (operation.Command == DeltaCommand.Stop) break;
                operations.AddLast(operation);
            }
        }

        public override void Dump(TextWriter writer)
        {
            foreach (DeltaOperation operation in operations)
            {
                operation.Dump(writer);
            }
        }
    }
}

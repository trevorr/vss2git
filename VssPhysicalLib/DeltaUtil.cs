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
    /// Utility methods for merging and applying reverse-delta operations.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public static class DeltaUtil
    {
        public static ICollection<DeltaOperation> Merge(
            IEnumerable<DeltaOperation> lastRevision,
            IEnumerable<DeltaOperation> priorRevision)
        {
            var result = new LinkedList<DeltaOperation>();
            using (var merger = new DeltaSimulator(lastRevision))
            {
                foreach (DeltaOperation operation in priorRevision)
                {
                    switch (operation.Command)
                    {
                        case DeltaCommand.WriteLog:
                            result.AddLast(operation);
                            break;
                        case DeltaCommand.WriteSuccessor:
                            merger.Seek(operation.Offset);
                            merger.Read(operation.Length,
                                delegate(byte[] data, int offset, int count)
                                {
                                    result.AddLast(DeltaOperation.WriteLog(data, offset, count));
                                    return count;
                                },
                                delegate(int offset, int count)
                                {
                                    result.AddLast(DeltaOperation.WriteSuccessor(offset, count));
                                    return count;
                                });
                            break;
                    }
                }
            }
            return result;
        }

        public static void Apply(
            IEnumerable<DeltaOperation> operations,
            Stream input,
            Stream output)
        {
            const int COPY_BUFFER_SIZE = 4096;
            byte[] copyBuffer = null;
            foreach (DeltaOperation operation in operations)
            {
                switch (operation.Command)
                {
                    case DeltaCommand.WriteLog:
                        output.Write(operation.Data.Array,
                            operation.Data.Offset, operation.Data.Count);
                        break;
                    case DeltaCommand.WriteSuccessor:
                        input.Seek(operation.Offset, SeekOrigin.Begin);
                        if (copyBuffer == null)
                        {
                            copyBuffer = new byte[COPY_BUFFER_SIZE];
                        }
                        int remaining = operation.Length;
                        int offset = 0;
                        while (remaining > 0)
                        {
                            int count = input.Read(copyBuffer, offset, remaining);
                            if (count <= 0)
                            {
                                throw new IOException("Unexpected end of current revision file");
                            }
                            offset += count;
                            remaining -= count;
                        }
                        output.Write(copyBuffer, 0, offset);
                        break;
                }
            }
            output.Flush();
        }
    }
}

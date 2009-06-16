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
using System.Text;

namespace Hpdi.VssPhysicalLib
{
    /// <summary>
    /// Enumeration of file revision delta commands.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public enum DeltaCommand
    {
        WriteLog = 0, // write data from the log file
        WriteSuccessor = 1, // write data from the subsequent revision
        Stop = 2 // indicates the last operation
    }

    /// <summary>
    /// Represents a single delta operation for a file revision.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class DeltaOperation
    {
        DeltaCommand command;
        int offset; // meaningful for WriteSuccessor only
        int length;
        ArraySegment<byte> data; // WriteLog only

        public DeltaCommand Command { get { return command; } }
        public int Offset { get { return offset; } }
        public int Length { get { return length; } }
        public ArraySegment<byte> Data { get { return data; } }

        public static DeltaOperation WriteLog(byte[] data, int offset, int length)
        {
            var result = new DeltaOperation();
            result.command = DeltaCommand.WriteLog;
            result.length = length;
            result.data = new ArraySegment<byte>(data, offset, length);
            return result;
        }

        public static DeltaOperation WriteSuccessor(int offset, int length)
        {
            var result = new DeltaOperation();
            result.command = DeltaCommand.WriteSuccessor;
            result.offset = offset;
            result.length = length;
            return result;
        }

        public void Read(BufferReader reader)
        {
            command = (DeltaCommand)reader.ReadInt16();
            reader.Skip(2); // unknown
            offset = reader.ReadInt32();
            length = reader.ReadInt32();
            if (command == DeltaCommand.WriteLog)
            {
                data = reader.GetBytes(length);
            }
        }

        public void Dump(TextWriter writer)
        {
            const int MAX_DATA_DUMP = 40;
            writer.Write("  {0}: Offset={1}, Length={2}",
                command, offset, length);
            if (data.Array != null)
            {
                var dumpLength = data.Count;
                var truncated = false;
                if (dumpLength > MAX_DATA_DUMP)
                {
                    dumpLength = MAX_DATA_DUMP;
                    truncated = true;
                }

                StringBuilder buf = new StringBuilder(dumpLength);
                for (int i = 0; i < dumpLength; ++i)
                {
                    byte b = data.Array[data.Offset + i];
                    buf.Append(b >= 0x20 && b <= 0x7E ? (char)b : '.');
                }
                writer.Write(", Data: {0}{1}", buf.ToString(), truncated ? "..." : "");
            }
            writer.WriteLine();
        }
    }
}

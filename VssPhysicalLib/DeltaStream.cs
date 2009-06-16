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
using System.IO;

namespace Hpdi.VssPhysicalLib
{
    /// <summary>
    /// Provides a seekable input stream over a file revision based on the
    /// latest revision content and a set of reverse-delta operations.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class DeltaStream : Stream
    {
        private Stream baseStream;
        private DeltaSimulator simulator;
        private int length = -1;

        public DeltaStream(Stream stream, IEnumerable<DeltaOperation> operations)
        {
            baseStream = stream;
            simulator = new DeltaSimulator(operations);
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get
            {
                if (length < 0)
                {
                    length = 0;
                    foreach (DeltaOperation operation in simulator.Operations)
                    {
                        length += operation.Length;
                    }
                }
                return length;
            }
        }

        public override long Position
        {
            get
            {
                return simulator.Offset;
            }
            set
            {
                simulator.Seek((int)value);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = 0;
            simulator.Read(count,
                delegate(byte[] opData, int opOffset, int opCount)
                {
                    Buffer.BlockCopy(opData, opOffset, buffer, offset, opCount);
                    offset += opCount;
                    count -= opCount;
                    bytesRead += opCount;
                    return opCount;
                },
                delegate(int opOffset, int opCount)
                {
                    baseStream.Seek(opOffset, SeekOrigin.Begin);
                    var opBytesRead = baseStream.Read(buffer, offset, opCount);
                    offset += opBytesRead;
                    count -= opBytesRead;
                    bytesRead += opBytesRead;
                    return opBytesRead;
                });
            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    simulator.Seek((int)offset);
                    break;
                case SeekOrigin.Current:
                    simulator.Seek((int)(Position + offset));
                    break;
                case SeekOrigin.End:
                    simulator.Seek((int)(Length + offset));
                    break;
                default:
                    throw new ArgumentException("Invalid origin", "origin");
            }
            return Position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void Flush()
        {
            // does nothing
        }

        public override void Close()
        {
            base.Close();
            baseStream.Close();
        }
    }
}

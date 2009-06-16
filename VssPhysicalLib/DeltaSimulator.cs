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

namespace Hpdi.VssPhysicalLib
{
    delegate int FromLogCallback(byte[] data, int offset, int count);
    delegate int FromSuccessorCallback(int offset, int count);

    /// <summary>
    /// Simulates stream-like traversal over a set of revision delta operations.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class DeltaSimulator : IDisposable
    {
        private readonly IEnumerable<DeltaOperation> operations;
        private IEnumerator<DeltaOperation> enumerator;
        private int operationOffset;
        private int fileOffset;
        private bool eof;

        public IEnumerable<DeltaOperation> Operations
        {
            get { return operations; }
        } 

        public int Offset
        {
            get { return fileOffset; }
        }

        public DeltaSimulator(IEnumerable<DeltaOperation> operations)
        {
            this.operations = operations;
            Reset();
        }

        public void Dispose()
        {
            if (enumerator != null)
            {
                enumerator.Dispose();
                enumerator = null;
            }
        }

        public void Seek(int offset)
        {
            if (offset != fileOffset)
            {
                if (offset < fileOffset)
                {
                    Reset();
                }
                while (fileOffset < offset && !eof)
                {
                    var seekRemaining = offset - fileOffset;
                    var operationRemaining = enumerator.Current.Length - operationOffset;
                    if (seekRemaining < operationRemaining)
                    {
                        operationOffset += seekRemaining;
                        fileOffset += seekRemaining;
                    }
                    else
                    {
                        fileOffset += operationRemaining;
                        eof = !enumerator.MoveNext();
                        operationOffset = 0;
                    }
                }
            }
        }

        public void Read(int length, FromLogCallback fromLog, FromSuccessorCallback fromSuccessor)
        {
            while (length > 0 && !eof)
            {
                var operation = enumerator.Current;
                var operationRemaining = operation.Length - operationOffset;
                var count = Math.Min(length, operationRemaining);
                int bytesRead;
                if (operation.Command == DeltaCommand.WriteLog)
                {
                    bytesRead = fromLog(operation.Data.Array, operation.Data.Offset + operationOffset, count);
                }
                else
                {
                    bytesRead = fromSuccessor(operation.Offset + operationOffset, count);
                }
                if (bytesRead == 0)
                {
                    break;
                }
                operationOffset += bytesRead;
                fileOffset += bytesRead;
                if (length >= operationRemaining)
                {
                    eof = !enumerator.MoveNext();
                    operationOffset = 0;
                }
                length -= bytesRead;
            }
        }

        private void Reset()
        {
            if (enumerator != null)
            {
                enumerator.Dispose();
            }
            enumerator = operations.GetEnumerator();
            eof = !enumerator.MoveNext();
            operationOffset = 0;
            fileOffset = 0;
        }
    }
}

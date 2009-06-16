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

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// Copies the contents of one stream to another.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class StreamCopier
    {
        private const int DEFAULT_BUFFER_SIZE = 4096;

        private byte[] buffer;

        private int bufferSize;
        public int BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        }

        public StreamCopier()
            : this(DEFAULT_BUFFER_SIZE)
        {
        }

        public StreamCopier(int bufferSize)
        {
            this.bufferSize = bufferSize;
        }

        public long Copy(Stream inputStream, Stream outputStream)
        {
            if (buffer == null)
            {
                buffer = new byte[bufferSize];
            }
            long copied = 0;
            while (true)
            {
                int count = inputStream.Read(buffer, 0, buffer.Length);
                if (count <= 0)
                {
                    break;
                }
                outputStream.Write(buffer, 0, count);
                copied += count;
            }
            return copied;
        }
    }
}

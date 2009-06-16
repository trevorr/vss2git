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

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// Asynchronously reads complete lines of text from a stream as they become available.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class AsyncLineReader
    {
        private const int DEFAULT_BUFFER_SIZE = 4096;
        private const int DEFAULT_MAX_LINE = 4096;

        private readonly Stream stream;
        private readonly Decoder decoder;

        private readonly byte[] readBuffer; // circular
        private int readOffset;
        private int decodeOffset;
        private int undecodedBytes;

        private readonly char[] decodeBuffer; // linear
        private int copyOffset;
        private int copyLimit;

        private readonly StringBuilder lineBuilder = new StringBuilder();
        private int maxLineLength;

        private bool readPending;
        private bool endOfFile;

        public bool EndOfFile
        {
            get { return endOfFile; }
        }

        public event EventHandler DataReceived;

        public AsyncLineReader(Stream stream)
            : this(stream, Encoding.Default, DEFAULT_BUFFER_SIZE, DEFAULT_MAX_LINE)
        {
        }

        public AsyncLineReader(Stream stream, Encoding encoding, int bufferSize, int maxLineLength)
        {
            this.stream = stream;
            this.decoder = encoding.GetDecoder();
            this.readBuffer = new byte[bufferSize];
            this.decodeBuffer = new char[bufferSize];
            this.maxLineLength = maxLineLength;
            StartRead();
        }

        public string ReadLine()
        {
            bool found = false;
            lock (readBuffer)
            {
                do
                {
                    while (copyOffset < copyLimit)
                    {
                        char c = decodeBuffer[copyOffset++];
                        lineBuilder.Append(c);
                        if (c == '\n' || lineBuilder.Length == maxLineLength)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found && undecodedBytes > 0)
                    {
                        // undecoded bytes may wrap around buffer, in which case two decodes are necessary
                        int readTailCount = readBuffer.Length - decodeOffset;
                        int decodeCount = Math.Min(undecodedBytes, readTailCount);

                        // need to leave room for an extra char in case one is flushed out from the last decode
                        decodeCount = Math.Min(decodeCount, decodeBuffer.Length - 1);

                        copyOffset = 0;
                        copyLimit = decoder.GetChars(
                            readBuffer, decodeOffset, decodeCount, decodeBuffer, copyOffset, endOfFile);

                        undecodedBytes -= decodeCount;
                        decodeOffset += decodeCount;
                        if (decodeOffset == readBuffer.Length)
                        {
                            decodeOffset = 0;
                        }
                    }
                }
                while (!found && copyOffset < copyLimit);

                if (!readPending && !endOfFile)
                {
                    StartRead();
                }

                if (endOfFile && lineBuilder.Length > 0)
                {
                    lineBuilder.Append(Environment.NewLine);
                    found = true;
                }
            }

            string result = null;
            if (found)
            {
                result = lineBuilder.ToString();
                lineBuilder.Length = 0;
            }
            return result;
        }

        protected virtual void OnDataReceived()
        {
            EventHandler handler = DataReceived;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        // Assumes buffer lock is held or called from constructor.
        private void StartRead()
        {
            int readCount = 0;
            if (decodeOffset > readOffset)
            {
                readCount = decodeOffset - readOffset;
            }
            else if (readOffset > decodeOffset || undecodedBytes == 0)
            {
                readCount = readBuffer.Length - readOffset;
            }
            if (readCount > 0)
            {
                stream.BeginRead(readBuffer, readOffset, readCount, ReadCallback, null);
                readPending = true;
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            lock (readBuffer)
            {
                try
                {
                    readPending = false;

                    int count = stream.EndRead(ar);
                    if (count > 0)
                    {
                        undecodedBytes += count;
                        readOffset += count;
                        if (readOffset == readBuffer.Length)
                        {
                            readOffset = 0;
                        }

                        StartRead();
                    }
                    else
                    {
                        // zero-length read indicates end of file
                        endOfFile = true;
                    }
                }
                catch
                {
                    // simulate end of file on read error
                    endOfFile = true;
                }
            }

            OnDataReceived();
        }
    }
}

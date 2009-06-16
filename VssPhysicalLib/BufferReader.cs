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
using System.Text;
using Hpdi.HashLib;

namespace Hpdi.VssPhysicalLib
{
    /// <summary>
    /// Reads VSS data types from a byte buffer.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class BufferReader
    {
        private readonly Encoding encoding;
        private readonly byte[] data;
        private int offset;
        private int limit;

        public BufferReader(Encoding encoding, byte[] data)
            : this(encoding, data, 0, data.Length)
        {
        }

        public BufferReader(Encoding encoding, byte[] data, int offset, int limit)
        {
            this.encoding = encoding;
            this.data = data;
            this.offset = offset;
            this.limit = limit;
        }

        public int Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public int Remaining
        {
            get { return limit - offset; }
        }

        public ushort Checksum16()
        {
            ushort sum = 0;
            for (int i = offset; i < limit; ++i)
            {
                sum += data[i];
            }
            return sum;
        }

        private static Hash16 crc16 = new XorHash32To16(new Crc32(Crc32.IEEE));

        public ushort Crc16()
        {
            return crc16.Compute(data, offset, limit);
        }

        public ushort Crc16(int bytes)
        {
            CheckRead(bytes);
            return crc16.Compute(data, offset, offset + bytes);
        }

        public void Skip(int bytes)
        {
            CheckRead(bytes);
            offset += bytes;
        }

        public short ReadInt16()
        {
            CheckRead(2);
            return (short)(data[offset++] | (data[offset++] << 8));
        }

        public int ReadInt32()
        {
            CheckRead(4);
            return data[offset++] | (data[offset++] << 8) |
                (data[offset++] << 16) | (data[offset++] << 24);
        }

        private static readonly DateTime EPOCH =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

        public DateTime ReadDateTime()
        {
            return EPOCH + TimeSpan.FromSeconds(ReadInt32());
        }

        public string ReadSignature(int length)
        {
            CheckRead(length);
            StringBuilder buf = new StringBuilder(length);
            for (int i = 0; i < length; ++i)
            {
                buf.Append((char)data[offset++]);
            }
            return buf.ToString();
        }

        public VssName ReadName()
        {
            CheckRead(2 + 34 + 4);
            return new VssName(ReadInt16(), ReadString(34), ReadInt32());
        }

        public string ReadString(int fieldSize)
        {
            CheckRead(fieldSize);

            int count = 0;
            for (int i = 0; i < fieldSize; ++i)
            {
                if (data[offset + i] == 0) break;
                ++count;
            }

            var str = encoding.GetString(data, offset, count);

            offset += fieldSize;

            return str;
        }

        public string ReadByteString(int bytes)
        {
            CheckRead(bytes);
            var result = FormatBytes(bytes);
            offset += bytes;
            return result;
        }

        public BufferReader Extract(int bytes)
        {
            CheckRead(bytes);
            return new BufferReader(encoding, data, offset, offset += bytes);
        }

        public ArraySegment<byte> GetBytes(int bytes)
        {
            CheckRead(bytes);
            var result = new ArraySegment<byte>(data, offset, bytes);
            offset += bytes;
            return result;
        }

        public string FormatBytes(int bytes)
        {
            var formatLimit = Math.Min(limit, offset + bytes);
            StringBuilder buf = new StringBuilder((formatLimit - offset) * 3);
            for (int i = offset; i < formatLimit; ++i)
            {
                buf.AppendFormat("{0:X2} ", data[i]);
            }
            return buf.ToString();
        }

        public string FormatRemaining()
        {
            return FormatBytes(Remaining);
        }

        private void CheckRead(int bytes)
        {
            if (offset + bytes > limit)
            {
                throw new EndOfBufferException(string.Format(
                    "Attempted read of {0} bytes with only {1} bytes remaining in buffer",
                    bytes, Remaining));
            }
        }
    }
}

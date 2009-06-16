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

namespace Hpdi.HashLib
{
    /// <summary>
    /// 16-bit CRC hash function.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class Crc16 : Hash16
    {
        // Commonly used polynomials.
        public const ushort IBM = 0xA001; // reversed
        public const ushort DNP = 0xA6BC; // reversed
        public const ushort CCITT = 0x1021;

        private readonly ushort[] table;
        private readonly ushort initial;
        private readonly ushort final;
        private readonly bool reverse;

        public Crc16(ushort poly, bool reverse)
            : this(poly, reverse, 0, 0)
        {
        }

        public Crc16(ushort poly, bool reverse, ushort initial, ushort final)
        {
            this.table = GenerateTable(poly, reverse);
            this.initial = initial;
            this.final = final;
            this.reverse = reverse;
        }

        public ushort Compute(byte[] bytes)
        {
            return Compute(bytes, 0, bytes.Length);
        }

        public ushort Compute(byte[] bytes, int offset, int limit)
        {
            var crc = initial;
            while (offset < limit)
            {
                if (reverse)
                {
                    crc = (ushort)((crc >> 8) ^ table[(byte)(crc ^ bytes[offset++])]);
                }
                else
                {
                    crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ bytes[offset++])]);
                }
            }
            return (ushort)(crc ^ final);
        }

        protected static ushort[] GenerateTable(ushort poly, bool reverse)
        {
            var table = new ushort[256];
            var mask = (ushort)(reverse ? 1 : 0x8000);
            for (int i = 0; i < table.Length; ++i)
            {
                var value = (ushort)(reverse ? i : i << 8);
                for (int j = 0; j < 8; ++j)
                {
                    var xor = (value & mask) != 0;
                    value = reverse ? (ushort)(value >> 1) : (ushort)(value << 1);
                    if (xor) value ^= poly;
                }
                table[i] = value;
            }
            return table;
        }
    }
}

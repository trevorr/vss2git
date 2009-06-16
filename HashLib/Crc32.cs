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
    /// 32-bit CRC hash function.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class Crc32 : Hash32
    {
        // Commonly used polynomials.
        public const uint IEEE = 0xEDB88320; // reversed

        private readonly uint[] table;
        private readonly uint initial;
        private readonly uint final;

        public Crc32(uint poly)
            : this(poly, 0, 0)
        {
        }

        public Crc32(uint poly, uint initial, uint final)
        {
            this.table = GenerateTable(poly);
            this.initial = initial;
            this.final = final;
        }

        public uint Compute(byte[] bytes)
        {
            return Compute(bytes, 0, bytes.Length);
        }

        public uint Compute(byte[] bytes, int offset, int limit)
        {
            var crc = initial;
            while (offset < limit)
            {
                crc = (uint)((crc >> 8) ^ table[(byte)(crc ^ bytes[offset++])]);
            }
            return (uint)(crc ^ final);
        }

        protected static uint[] GenerateTable(uint poly)
        {
            var table = new uint[256];
            for (int i = 0; i < table.Length; ++i)
            {
                var value = (uint)i;
                for (int j = 0; j < 8; ++j)
                {
                    var xor = (value & 1) != 0;
                    value >>= 1;
                    if (xor) value ^= poly;
                }
                table[i] = value;
            }
            return table;
        }
    }
}

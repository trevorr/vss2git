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
    /// Interface for 16-bit hash functions.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public interface Hash16
    {
        ushort Compute(byte[] bytes);
        ushort Compute(byte[] bytes, int offset, int limit);
    }

    /// <summary>
    /// Interface for 32-bit hash functions.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public interface Hash32
    {
        uint Compute(byte[] bytes);
        uint Compute(byte[] bytes, int offset, int limit);
    }

    /// <summary>
    /// 16-bit hash function based on XORing the upper and lower words of a 32-bit hash.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class XorHash32To16 : Hash16
    {
        private readonly Hash32 hash32;

        public XorHash32To16(Hash32 hash32)
        {
            this.hash32 = hash32;
        }

        public ushort Compute(byte[] bytes)
        {
            return Compute(bytes, 0, bytes.Length);
        }

        public ushort Compute(byte[] bytes, int offset, int limit)
        {
            uint value32 = hash32.Compute(bytes, offset, limit);
            return (ushort)(value32 ^ (value32 >> 16));
        }
    }
}

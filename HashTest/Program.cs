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

namespace Hpdi.HashTest
{
    /// <summary>
    /// Simple test program for CRC generation.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class Program
    {
        static void Main(string[] args)
        {
            byte[] data = Encoding.ASCII.GetBytes("123456789");

            var crc16 = new Crc16(Crc16.IBM, true);
            Console.WriteLine("CRC-16 = {0:X4}", crc16.Compute(data));

            var crc16ccitt = new Crc16(Crc16.CCITT, false, 0xFFFF, 0);
            Console.WriteLine("CRC-16-CCITT = {0:X4}", crc16ccitt.Compute(data));

            var crc32 = new Crc32(Crc32.IEEE, 0xFFFFFFFF, 0xFFFFFFFF);
            Console.WriteLine("CRC-32 = {0:X8}", crc32.Compute(data));
        }
    }
}

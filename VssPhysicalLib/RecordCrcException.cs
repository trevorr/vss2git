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

namespace Hpdi.VssPhysicalLib
{
    /// <summary>
    /// Exception thrown when the CRC stored in a record does not match the expected value.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class RecordCrcException : RecordException
    {
        private RecordHeader header;
        public RecordHeader Header
        {
            get { return header; }
        }

        public RecordCrcException(RecordHeader header)
        {
            this.header = header;
        }

        public RecordCrcException(RecordHeader header, string message)
            : base(message)
        {
            this.header = header;
        }
    }
}

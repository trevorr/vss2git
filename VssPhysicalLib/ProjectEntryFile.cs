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

using System.Text;

namespace Hpdi.VssPhysicalLib
{
    /// <summary>
    /// Represents a file containing VSS project entry records.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class ProjectEntryFile : VssRecordFile
    {
        public ProjectEntryFile(string filename, Encoding encoding)
            : base(filename, encoding)
        {
        }

        public ProjectEntryRecord GetFirstEntry()
        {
            reader.Offset = 0;
            return GetNextEntry();
        }

        public ProjectEntryRecord GetNextEntry()
        {
            ProjectEntryRecord record = new ProjectEntryRecord();
            return ReadNextRecord(record) ? record : null;
        }
    }
}

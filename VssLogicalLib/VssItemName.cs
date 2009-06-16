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

namespace Hpdi.VssLogicalLib
{
    /// <summary>
    /// Represents the name of a VSS item.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssItemName
    {
        private readonly string logicalName;
        private readonly string physicalName;
        private readonly bool isProject;

        /// <summary>
        /// The current logical name of the item.
        /// Note that the logical name can change over the history of the item.
        /// </summary>
        public string LogicalName
        {
            get { return logicalName; }
        }

        /// <summary>
        /// The physical name of the item (e.g. AAAAAAAA). This name never changes.
        /// </summary>
        public string PhysicalName
        {
            get { return physicalName; }
        }

        /// <summary>
        /// Indicates whether this item is a project or a file.
        /// </summary>
        public bool IsProject
        {
            get { return isProject; }
        }

        internal VssItemName(string logicalName, string physicalName, bool isProject)
        {
            this.logicalName = logicalName;
            this.physicalName = physicalName;
            this.isProject = isProject;
        }

        public override string ToString()
        {
            return (isProject ? "$" : "") + logicalName + "(" + physicalName + ")";
        }
    }
}

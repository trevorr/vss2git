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

namespace Hpdi.VssLogicalLib
{
    /// <summary>
    /// Factory for obtaining VssDatabase instances.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssDatabaseFactory
    {
        private readonly string path;

        private Encoding encoding = Encoding.Default;
        public Encoding Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        public VssDatabaseFactory(string path)
        {
            this.path = path;
        }

        public VssDatabase Open()
        {
            return new VssDatabase(path, encoding);
        }
    }
}

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

using System.Collections.Generic;
using System.IO;

namespace Hpdi.VssLogicalLib
{
    /// <summary>
    /// A very simple .INI file reader that does not require or support sections.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class SimpleIniReader
    {
        private readonly string filename;
        private readonly Dictionary<string, string> entries = new Dictionary<string, string>();

        public SimpleIniReader(string filename)
        {
            this.filename = filename;
        }

        public string Filename
        {
            get { return filename; }
        }

        public void Parse()
        {
            entries.Clear();
            using (var reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.Length > 0 && !line.StartsWith(";"))
                    {
                        var separator = line.IndexOf('=');
                        if (separator > 0)
                        {
                            var key = line.Substring(0, separator).Trim();
                            var value = line.Substring(separator + 1).Trim();
                            entries[key] = value;
                        }
                    }
                }
            }
        }

        public IEnumerable<string> Keys
        {
            get { return entries.Keys; }
        }

        public string GetValue(string key)
        {
            return entries[key];
        }

        public string GetValue(string key, string defaultValue)
        {
            string result;
            return entries.TryGetValue(key, out result) ? result : defaultValue;
        }
    }
}

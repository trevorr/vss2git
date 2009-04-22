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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hpdi.VssLogicalLib;
using Hpdi.VssPhysicalLib;

namespace Hpdi.VssDump
{
    /// <summary>
    /// Dumps pretty much everything in the VSS database to the console.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class Program
    {
        private const string Separator = "------------------------------------------------------------";

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Syntax: VssDump <vss-base-path>");
                return;
            }

            var repoPath = args[0];
            var df = new VssDatabaseFactory(repoPath);
            var db = df.Open();

            Console.WriteLine("File hierarchy:");
            Console.WriteLine(Separator);
            var tree = new TreeDumper(Console.Out) { IncludeRevisions = false };
            tree.DumpProject(db.RootProject);
            Console.WriteLine();

            Console.WriteLine("Log file contents:");
            for (char c = 'a'; c <= 'z'; ++c)
            {
                string[] dataPaths = Directory.GetFiles(
                    Path.Combine(db.DataPath, c.ToString()), "*.");
                foreach (string dataPath in dataPaths)
                {
                    var dataFile = Path.GetFileName(dataPath).ToUpper();
                    var orphaned = !tree.PhysicalNames.Contains(dataFile);
                    Console.WriteLine(Separator);
                    Console.WriteLine("{0}{1}", dataPath, orphaned ? " (orphaned)" : "");
                    DumpLogFile(dataPath);
                }
            }
            Console.WriteLine();

            Console.WriteLine("Name file contents:");
            Console.WriteLine(Separator);
            var namePath = Path.Combine(db.DataPath, "names.dat");
            DumpNameFile(namePath);
            Console.WriteLine();

            Console.WriteLine(Separator);
            Console.WriteLine("Project actions: {0}", FormatCollection(projectActions));
            Console.WriteLine("File actions: {0}", FormatCollection(fileActions));
        }

        private static HashSet<Hpdi.VssPhysicalLib.Action> projectActions = new HashSet<Hpdi.VssPhysicalLib.Action>();
        private static HashSet<Hpdi.VssPhysicalLib.Action> fileActions = new HashSet<Hpdi.VssPhysicalLib.Action>();

        private static string FormatCollection(IEnumerable collection)
        {
            StringBuilder buf = new StringBuilder();
            foreach (var item in collection)
            {
                if (buf.Length > 0)
                {
                    buf.Append(", ");
                }
                buf.Append(item);
            }
            return buf.ToString();
        }

        private static void DumpLogFile(string filename)
        {
            try
            {
                var itemFile = new ItemFile(filename);
                itemFile.Header.Header.Dump(Console.Out);
                itemFile.Header.Dump(Console.Out);
                var record = itemFile.GetNextRecord(true);
                while (record != null)
                {
                    record.Header.Dump(Console.Out);
                    record.Dump(Console.Out);
                    var revision = record as RevisionRecord;
                    if (revision != null)
                    {
                        if (itemFile.Header.ItemType == ItemType.Project)
                        {
                            projectActions.Add(revision.Action);
                        }
                        else
                        {
                            fileActions.Add(revision.Action);
                        }
                    }
                    record = itemFile.GetNextRecord(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
        }

        private static void DumpNameFile(string filename)
        {
            try
            {
                var nameFile = new NameFile(filename);
                nameFile.Header.Header.Dump(Console.Out);
                nameFile.Header.Dump(Console.Out);
                var name = nameFile.GetNextName();
                while (name != null)
                {
                    name.Header.Dump(Console.Out);
                    name.Dump(Console.Out);
                    name = nameFile.GetNextName();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
        }
    }
}

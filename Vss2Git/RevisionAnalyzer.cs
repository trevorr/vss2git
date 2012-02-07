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
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Hpdi.VssLogicalLib;
using Hpdi.VssPhysicalLib;
using System.Collections;
using System.IO;

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// Value holder for time sequence fixes.
    /// </summary>
    /// <author>Remigius Stalder</author>
    class TimeFix
    {
        public TimeFix(DateTime start, DateTime end, DateTime newTime)
        {
            this.start = start;
            this.end = end;
            this.newTime = newTime;
        }
        public DateTime start;
        public DateTime end;
        public DateTime newTime;
    }

    /// <summary>
    /// Enumerates revisions in a VSS database.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class RevisionAnalyzer : Worker
    {
        private LinkedList<TimeFix> timeFixList;
        private string excludeFiles;
        public string ExcludeFiles
        {
            get { return excludeFiles; }
            set { excludeFiles = value; }
        }

        private readonly VssDatabase database;
        public VssDatabase Database
        {
            get { return database; }
        }

        private readonly LinkedList<VssProject> rootProjects = new LinkedList<VssProject>();
        public IEnumerable<VssProject> RootProjects
        {
            get { return rootProjects; }
        }

        private readonly SortedDictionary<DateTime, ICollection<Revision>> sortedRevisions =
            new SortedDictionary<DateTime, ICollection<Revision>>();
        public SortedDictionary<DateTime, ICollection<Revision>> SortedRevisions
        {
            get { return sortedRevisions; }
        }

        private readonly HashSet<string> processedFiles = new HashSet<string>();
        public HashSet<string> ProcessedFiles
        {
            get { return processedFiles; }
        }

        private readonly HashSet<string> destroyedFiles = new HashSet<string>();
        public HashSet<string> DestroyedFiles
        {
            get { return destroyedFiles; }
        }

        private int projectCount;
        public int ProjectCount
        {
            get { return Thread.VolatileRead(ref projectCount); }
        }

        private int fileCount;
        public int FileCount
        {
            get { return Thread.VolatileRead(ref fileCount); }
        }

        private int revisionCount;
        public int RevisionCount
        {
            get { return Thread.VolatileRead(ref revisionCount); }
        }

        public RevisionAnalyzer(WorkQueue workQueue, Logger logger, VssDatabase database)
            : base(workQueue, logger)
        {
            this.database = database;
        }

        public bool IsDestroyed(string physicalName)
        {
            return destroyedFiles.Contains(physicalName);
        }

        public void AddItem(VssProject project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }
            else if (project.Database != database)
            {
                throw new ArgumentException("Project database mismatch", "project");
            }

            if (timeFixList == null)
            {
                // load the time fix list only once
                timeFixList = ReadTimeFixList(database.BasePath, "time-fix-list.txt");
            }

            rootProjects.AddLast(project);

            PathMatcher exclusionMatcher = null;
            if (!string.IsNullOrEmpty(excludeFiles))
            {
                var excludeFileArray = excludeFiles.Split(
                    new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                exclusionMatcher = new PathMatcher(excludeFileArray);
            }

            workQueue.AddLast(delegate(object work)
            {
                logger.WriteSectionSeparator();
                LogStatus(work, "Building revision list");

                logger.WriteLine("Root project: {0}", project.Path);
                logger.WriteLine("Excluded files: {0}", excludeFiles);

                int excludedProjects = 0;
                int excludedFiles = 0;
                var stopwatch = Stopwatch.StartNew();
                VssUtil.RecurseItems(project,
                    delegate(VssProject subproject)
                    {
                        if (workQueue.IsAborting)
                        {
                            return RecursionStatus.Abort;
                        }

                        var path = subproject.Path;
                        if (exclusionMatcher != null && exclusionMatcher.Matches(path))
                        {
                            logger.WriteLine("Excluding project {0}", path);
                            ++excludedProjects;
                            return RecursionStatus.Skip;
                        }

                        ProcessItem(subproject, path, exclusionMatcher);
                        ++projectCount;
                        return RecursionStatus.Continue;
                    },
                    delegate(VssProject subproject, VssFile file)
                    {
                        if (workQueue.IsAborting)
                        {
                            return RecursionStatus.Abort;
                        }

                        var path = file.GetPath(subproject);
                        if (exclusionMatcher != null && exclusionMatcher.Matches(path))
                        {
                            logger.WriteLine("Excluding file {0}", path);
                            ++excludedFiles;
                            return RecursionStatus.Skip;
                        }

                        // only process shared files once (projects are never shared)
                        if (!processedFiles.Contains(file.PhysicalName))
                        {
                            processedFiles.Add(file.PhysicalName);
                            ProcessItem(file, path, exclusionMatcher);
                            ++fileCount;
                        }
                        return RecursionStatus.Continue;
                    });
                stopwatch.Stop();

                logger.WriteSectionSeparator();
                logger.WriteLine("Analysis complete in {0:HH:mm:ss}", new DateTime(stopwatch.ElapsedTicks));
                logger.WriteLine("Projects: {0} ({1} excluded)", projectCount, excludedProjects);
                logger.WriteLine("Files: {0} ({1} excluded)", fileCount, excludedFiles);
                logger.WriteLine("Revisions: {0}", revisionCount);
            });
        }

        private void ProcessItem(VssItem item, string path, PathMatcher exclusionMatcher)
        {
            try
            {
                VssRevision previousRevision = null;
                LinkedList<VssRevision> revisions = new LinkedList<VssRevision>();
                IEnumerable<VssRevision> originalRevisions = item.Revisions; // this is recreated from the file each time it is queried!!!
                foreach (VssRevision vssRevision in originalRevisions)
                {
                    if (previousRevision != null)
                    {
                        checkRevisionTime(item, previousRevision, vssRevision);
                    }
                    previousRevision = vssRevision;
                    revisions.AddLast(vssRevision);
                }
                foreach (VssRevision vssRevision in revisions)
                {
                    var actionType = vssRevision.Action.Type;
                    var namedAction = vssRevision.Action as VssNamedAction;
                    if (namedAction != null)
                    {
                        if (actionType == VssActionType.Destroy)
                        {
                            // track destroyed files so missing history can be anticipated
                            // (note that Destroy actions on shared files simply delete
                            // that copy, so destroyed files can't be completely ignored)
                            destroyedFiles.Add(namedAction.Name.PhysicalName);
                        }

                        var targetPath = path + VssDatabase.ProjectSeparator + namedAction.Name.LogicalName;
                        if (exclusionMatcher != null && exclusionMatcher.Matches(targetPath))
                        {
                            // project action targets an excluded file
                            continue;
                        }
                    }

                    Revision revision = new Revision(vssRevision.DateTime,
                        vssRevision.User, item.ItemName, vssRevision.Version,
                        vssRevision.Comment, vssRevision.Action);

                    ICollection<Revision> revisionSet;
                    if (!sortedRevisions.TryGetValue(vssRevision.DateTime, out revisionSet))
                    {
                        revisionSet = new LinkedList<Revision>();
                        sortedRevisions[vssRevision.DateTime] = revisionSet;
                    }
                    revisionSet.Add(revision);
                    ++revisionCount;
                }
            }
            catch (RecordException e)
            {
                var message = string.Format("Failed to read revisions for {0} ({1}): {2}",
                    path, item.PhysicalName, ExceptionFormatter.Format(e));
                LogException(e, message);
                ReportError(message);
            }
        }

        private void checkRevisionTime(VssItem item, VssRevision revision1, VssRevision revision2)
        {
            int version1 = revision1.Version;
            int version2 = revision2.Version;
            DateTime time1 = revision1.DateTime;
            DateTime time2 = revision2.DateTime;

            if (version1 > version2)
            {
                string msg = item.Name + "(" + item.PhysicalName + ")";
                logger.WriteLine("***** warning: revision number out of sequence: " + version1
                    + " before " + version2 + " for item " + msg);
            }
            if (time1.CompareTo(time2) > 0)
            {
                string msg = item.Name + "(" + item.PhysicalName + ")";
                logger.WriteLine("***** warning: revision time out of sequence: " + time1 + "@" + version1
                    + " later than " + time2 + "@" + version2 + " for item " + msg);

                // as we are only looking at items that are out of sequence, we try to fix
                // each of them as it may be the first or the second
                fixSequence(item, revision1);
                fixSequence(item, revision2);
            }
        }

        private void fixSequence(VssItem item, VssRevision revision)
        {
            if (timeFixList != null)
            {
                foreach (TimeFix timeFix in timeFixList)
                {
                    DateTime time = revision.DateTime;
                    if (time.CompareTo(timeFix.start) > 0 && time.CompareTo(timeFix.end) < 0)
                    {
                        int version = revision.Version;
                        revision.FixDateTime(timeFix.newTime);
                        string msg = item.Name + "(" + item.PhysicalName + ")";
                        logger.WriteLine("changed time " + time + " to " + timeFix.newTime + " for item " + msg + "@" + version);
                    }
                }
            }
        }

        public LinkedList<TimeFix> ReadTimeFixList(string repoPath, string fileName)
        {
            LinkedList<TimeFix> timeFixList = new LinkedList<TimeFix>();
            string finalPath = Path.Combine(repoPath, fileName);
            // read the time fix file either from the repository path or from the working directory
            if (!File.Exists(finalPath))
            {
                finalPath = fileName;
            }
            if (!File.Exists(finalPath))
            {
                // if the time fix file don't exist, return an empty dictionary
                logger.WriteLine("time fix file not found: " + finalPath);
                return timeFixList;
            }
            try
            {
                foreach (string line in File.ReadAllLines(finalPath))
                {
                    // read lines that contain a '=' sign and skip comment lines starting with a '#'
                    if ((!string.IsNullOrEmpty(line)) &&
                        (!line.StartsWith("#")) &&
                        (line.Contains("=")))
                    {
                        int index1 = line.IndexOf("to");
                        int index2 = line.IndexOf('=');
                        if (index1 < index2)
                        {
                            string startStr = line.Substring(0, index1).Trim();
                            string endStr = line.Substring(index1 + 2, index2 - index1 - 2).Trim();
                            string newTimeStr = line.Substring(index2 + 1).Trim();
                            DateTime start = DateTime.Parse(startStr);
                            DateTime end = DateTime.Parse(endStr);
                            DateTime newTime = DateTime.Parse(newTimeStr);
                            timeFixList.AddLast(new TimeFix(start, end, newTime));
                        }
                    }
                }
            }
            catch (Exception x)
            {
                logger.WriteLine("error reading time fix file " + finalPath + ": " + x.Message);
            }

            logger.WriteLine(timeFixList.Count + " time fix entries read from " + finalPath);
            return timeFixList;
        }
    }
}

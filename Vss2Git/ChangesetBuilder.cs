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
using System.IO;
using System.Threading;
using Hpdi.VssLogicalLib;

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// Reconstructs changesets from independent revisions.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class ChangesetBuilder : Worker
    {
        private readonly RevisionAnalyzer revisionAnalyzer;

        private readonly LinkedList<Changeset> changesets = new LinkedList<Changeset>();
        public LinkedList<Changeset> Changesets
        {
            get { return changesets; }
        }

        private TimeSpan anyCommentThreshold = TimeSpan.FromSeconds(30);
        public TimeSpan AnyCommentThreshold
        {
            get { return anyCommentThreshold; }
            set { anyCommentThreshold = value; }
        }


        private TimeSpan sameCommentThreshold = TimeSpan.FromMinutes(10);
        public TimeSpan SameCommentThreshold
        {
            get { return sameCommentThreshold; }
            set { sameCommentThreshold = value; }
        }

        public ChangesetBuilder(WorkQueue workQueue, Logger logger, RevisionAnalyzer revisionAnalyzer)
            : base(workQueue, logger)
        {
            this.revisionAnalyzer = revisionAnalyzer;
        }

        public void BuildChangesets()
        {
            workQueue.AddLast(delegate(object work)
            {
                logger.WriteSectionSeparator();
                LogStatus(work, "Building changesets");

                var stopwatch = Stopwatch.StartNew();
                var pendingChangesByUser = new Dictionary<string, Changeset>();
                var hasDelete = false;
                foreach (var dateEntry in revisionAnalyzer.SortedRevisions)
                {
                    var dateTime = dateEntry.Key;
                    foreach (Revision revision in dateEntry.Value)
                    {
                        // determine target of project revisions
                        var actionType = revision.Action.Type;
                        var namedAction = revision.Action as VssNamedAction;
                        var targetFile = revision.Item.PhysicalName;
                        if (namedAction != null)
                        {
                            targetFile = namedAction.Name.PhysicalName;
                        }

                        // Create actions are only used to obtain initial item comments;
                        // items are actually created when added to a project
                        var creating = (actionType == VssActionType.Create ||
                            (actionType == VssActionType.Branch && !revision.Item.IsProject));

                        // Share actions are never conflict (which is important,
                        // since Share always precedes Branch)
                        var nonconflicting = creating || (actionType == VssActionType.Share);

                        // look up the pending change for user of this revision
                        // and flush changes past time threshold
                        var pendingUser = revision.User;
                        Changeset pendingChange = null;
                        LinkedList<string> flushedUsers = null;
                        foreach (var userEntry in pendingChangesByUser)
                        {
                            var user = userEntry.Key;
                            var change = userEntry.Value;

                            // flush change if file conflict or past time threshold
                            var flush = false;
                            var timeDiff = revision.DateTime - change.DateTime;
                            if (timeDiff > anyCommentThreshold)
                            {
                                if (HasSameComment(revision, change.Revisions.Last.Value))
                                {
                                    string message;
                                    if (timeDiff < sameCommentThreshold)
                                    {
                                        message = "Using same-comment threshold";
                                    }
                                    else
                                    {
                                        message = "Same comment but exceeded threshold";
                                        flush = true;
                                    }
                                    logger.WriteLine("NOTE: {0} ({1} second gap):",
                                        message, timeDiff.TotalSeconds);
                                }
                                else
                                {
                                    flush = true;
                                }
                            }
                            else if (!nonconflicting && change.TargetFiles.Contains(targetFile))
                            {
                                logger.WriteLine("NOTE: Splitting changeset due to file conflict on {0}:",
                                    targetFile);
                                flush = true;
                            }
                            else if (hasDelete && actionType == VssActionType.Rename)
                            {
                                var renameAction = revision.Action as VssRenameAction;
                                if (renameAction != null && renameAction.Name.IsProject)
                                {
                                    // split the change set if a rename of a directory follows a delete
                                    // otherwise a git error occurs
                                    logger.WriteLine("NOTE: Splitting changeset due to rename after delete in {0}:",
                                        targetFile);
                                    flush = true;
                                }
                            }

                            if (flush)
                            {
                                AddChangeset(change);
                                if (flushedUsers == null)
                                {
                                    flushedUsers = new LinkedList<string>();
                                }
                                flushedUsers.AddLast(user);
                                hasDelete = false;
                            }
                            else if (user == pendingUser)
                            {
                                pendingChange = change;
                            }
                        }
                        if (flushedUsers != null)
                        {
                            foreach (string user in flushedUsers)
                            {
                                pendingChangesByUser.Remove(user);
                            }
                        }

                        // if no pending change for user, create a new one
                        if (pendingChange == null)
                        {
                            pendingChange = new Changeset();
                            pendingChange.User = pendingUser;
                            pendingChangesByUser[pendingUser] = pendingChange;
                        }

                        // update the time of the change based on the last revision
                        pendingChange.DateTime = revision.DateTime;

                        // add the revision to the change
                        pendingChange.Revisions.AddLast(revision);
                        hasDelete |= actionType == VssActionType.Delete || actionType == VssActionType.Destroy;

                        // track target files in changeset to detect conflicting actions
                        if (!nonconflicting)
                        {
                            pendingChange.TargetFiles.Add(targetFile);
                        }

                        // build up a concatenation of unique revision comments
                        var revComment = revision.Comment;
                        if (revComment != null)
                        {
                            revComment = revComment.Trim();
                            if (revComment.Length > 0)
                            {
                                if (string.IsNullOrEmpty(pendingChange.Comment))
                                {
                                    pendingChange.Comment = revComment;
                                }
                                else if (!pendingChange.Comment.Contains(revComment))
                                {
                                    pendingChange.Comment += "\n" + revComment;
                                }
                            }
                        }
                    }
                }

                // flush all remaining changes
                foreach (var change in pendingChangesByUser.Values)
                {
                    AddChangeset(change);
                }
                stopwatch.Stop();

                logger.WriteSectionSeparator();
                logger.WriteLine("Found {0} changesets in {1:HH:mm:ss}",
                    changesets.Count, new DateTime(stopwatch.ElapsedTicks));
            });
        }

        private bool HasSameComment(Revision rev1, Revision rev2)
        {
            return !string.IsNullOrEmpty(rev1.Comment) && rev1.Comment == rev2.Comment;
        }

        private void AddChangeset(Changeset change)
        {
            changesets.AddLast(change);
            int changesetId = changesets.Count;
            DumpChangeset(change, changesetId);
        }

        private void DumpChangeset(Changeset changeset, int changesetId)
        {
            var firstRevTime = changeset.Revisions.First.Value.DateTime;
            var changeDuration = changeset.DateTime - firstRevTime;
            logger.WriteSectionSeparator();
            logger.WriteLine("Changeset {0} - {1} ({2} secs) {3} {4} files",
                changesetId, changeset.DateTime, changeDuration.TotalSeconds, changeset.User,
                changeset.Revisions.Count);
            if (!string.IsNullOrEmpty(changeset.Comment))
            {
                logger.WriteLine(changeset.Comment);
            }
            logger.WriteLine();
            foreach (var revision in changeset.Revisions)
            {
                logger.WriteLine("  {0} {1}@{2} {3}",
                    revision.DateTime, revision.Item, revision.Version, revision.Action);
            }
        }
    }
}

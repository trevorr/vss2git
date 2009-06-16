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
using Hpdi.VssPhysicalLib;

namespace Hpdi.VssLogicalLib
{
    /// <summary>
    /// Represents a revision of a VSS file.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssFileRevision : VssRevision
    {
        public VssFile File
        {
            get { return (VssFile)Item; }
        }

        public Stream GetContents()
        {
            Stream dataFile = new FileStream(item.DataPath,
                FileMode.Open, FileAccess.Read, FileShare.Read);

            var itemFile = item.ItemFile;
            var lastRev = itemFile.GetLastRevision();
            if (lastRev != null)
            {
                IEnumerable<DeltaOperation> deltaOps = null;
                while (lastRev != null && lastRev.Revision > this.Version)
                {
                    var branchRev = lastRev as BranchRevisionRecord;
                    if (branchRev != null)
                    {
                        var branchRevId = branchRev.Revision;
                        var itemPath = item.Database.GetDataPath(branchRev.BranchFile);
                        itemFile = new ItemFile(itemPath, item.Database.Encoding);
                        lastRev = itemFile.GetLastRevision();
                        while (lastRev != null && lastRev.Revision >= branchRevId)
                        {
                            lastRev = itemFile.GetPreviousRevision(lastRev);
                        }
                    }
                    else
                    {
                        var editRev = lastRev as EditRevisionRecord;
                        if (editRev != null)
                        {
                            var delta = itemFile.GetPreviousDelta(editRev);
                            if (delta != null)
                            {
                                var curDeltaOps = delta.Operations;
                                deltaOps = (deltaOps == null) ? curDeltaOps :
                                    DeltaUtil.Merge(deltaOps, curDeltaOps);
                            }
                        }
                        lastRev = itemFile.GetPreviousRevision(lastRev);
                    }
                }

                if (deltaOps != null)
                {
                    dataFile = new DeltaStream(dataFile, deltaOps);
                }
            }

            return dataFile;
        }

        internal VssFileRevision(VssItem item, RevisionRecord revision, CommentRecord comment)
            : base(item, revision, comment)
        {
        }
    }
}

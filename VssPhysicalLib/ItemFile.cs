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
using System.Text;

namespace Hpdi.VssPhysicalLib
{
    /// <summary>
    /// Represents a file containing VSS project/file records.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class ItemFile : VssRecordFile
    {
        private readonly ItemHeaderRecord header;

        public ItemHeaderRecord Header
        {
            get { return header; }
        }

        public ItemFile(string filename, Encoding encoding)
            : base(filename, encoding)
        {
            try
            {
                var fileSig = reader.ReadString(0x20);
                if (fileSig != "SourceSafe@Microsoft")
                {
                    throw new BadHeaderException("Incorrect file signature");
                }

                var fileType = (ItemType)reader.ReadInt16();
                var fileVersion = reader.ReadInt16();
                if (fileVersion != 6)
                {
                    throw new BadHeaderException("Incorrect file version");
                }

                reader.Skip(16); // reserved; always 0

                if (fileType == ItemType.Project)
                {
                    header = new ProjectHeaderRecord();
                }
                else
                {
                    header = new FileHeaderRecord();
                }

                ReadRecord(header);
                if (header.ItemType != fileType)
                {
                    throw new BadHeaderException("Header record type mismatch");
                }
            }
            catch (EndOfBufferException e)
            {
                throw new BadHeaderException("Truncated header", e);
            }
        }

        public VssRecord GetRecord(int offset)
        {
            return GetRecord<VssRecord>(CreateRecord, false, offset);
        }

        public VssRecord GetNextRecord(bool skipUnknown)
        {
            if (reader.Offset == this.Header.EofOffset)
            {
                return null;
            }
            return GetNextRecord<VssRecord>(CreateRecord, skipUnknown);
        }

        public RevisionRecord GetFirstRevision()
        {
            if (header.FirstRevOffset > 0)
            {
                return GetRecord<RevisionRecord>(CreateRevisionRecord, false, header.FirstRevOffset);
            }
            return null;
        }

        public RevisionRecord GetNextRevision(RevisionRecord revision)
        {
            if (reader.Offset == this.Header.EofOffset)
            {
                return null;
            }
            reader.Offset = revision.Header.Offset + revision.Header.Length + RecordHeader.LENGTH;
            return GetNextRecord<RevisionRecord>(CreateRevisionRecord, true);
        }

        public RevisionRecord GetLastRevision()
        {
            if (header.LastRevOffset > 0)
            {
                return GetRecord<RevisionRecord>(CreateRevisionRecord, false, header.LastRevOffset);
            }
            return null;
        }

        public RevisionRecord GetPreviousRevision(RevisionRecord revision)
        {
            if (revision.PrevRevOffset > 0)
            {
                return GetRecord<RevisionRecord>(CreateRevisionRecord, false, revision.PrevRevOffset);
            }
            return null;
        }

        public DeltaRecord GetPreviousDelta(EditRevisionRecord revision)
        {
            if (revision.PrevDeltaOffset > 0)
            {
                var record = new DeltaRecord();
                ReadRecord(record, revision.PrevDeltaOffset);
                return record;
            }
            return null;
        }

        public ICollection<string> GetProjects()
        {
            var result = new LinkedList<string>();
            var fileHeader = header as FileHeaderRecord;
            if (fileHeader != null)
            {
                var record = new ProjectRecord();
                var offset = fileHeader.ProjectOffset;
                while (offset > 0)
                {
                    ReadRecord(record, offset);
                    if (!string.IsNullOrEmpty(record.ProjectFile))
                    {
                        result.AddFirst(record.ProjectFile);
                    }
                    offset = record.PrevProjectOffset;
                }
            }
            return result;
        }

        private static VssRecord CreateRecord(
            RecordHeader recordHeader, BufferReader recordReader)
        {
            VssRecord record = null;
            switch (recordHeader.Signature)
            {
                case RevisionRecord.SIGNATURE:
                    record = CreateRevisionRecord(recordHeader, recordReader);
                    break;
                case CommentRecord.SIGNATURE:
                    record = new CommentRecord();
                    break;
                case CheckoutRecord.SIGNATURE:
                    record = new CheckoutRecord();
                    break;
                case ProjectRecord.SIGNATURE:
                    record = new ProjectRecord();
                    break;
                case BranchRecord.SIGNATURE:
                    record = new BranchRecord();
                    break;
                case DeltaRecord.SIGNATURE:
                    record = new DeltaRecord();
                    break;
            }
            return record;
        }

        private static RevisionRecord CreateRevisionRecord(
            RecordHeader recordHeader, BufferReader recordReader)
        {
            if (recordHeader.Signature != RevisionRecord.SIGNATURE)
            {
                return null;
            }

            RevisionRecord record;
            var action = RevisionRecord.PeekAction(recordReader);
            switch (action)
            {
                case Action.Label:
                    record = new RevisionRecord();
                    break;
                case Action.DestroyProject:
                case Action.DestroyFile:
                    record = new DestroyRevisionRecord();
                    break;
                case Action.RenameProject:
                case Action.RenameFile:
                    record = new RenameRevisionRecord();
                    break;
                case Action.MoveFrom:
                case Action.MoveTo:
                    record = new MoveRevisionRecord();
                    break;
                case Action.ShareFile:
                    record = new ShareRevisionRecord();
                    break;
                case Action.BranchFile:
                case Action.CreateBranch:
                    record = new BranchRevisionRecord();
                    break;
                case Action.EditFile:
                    record = new EditRevisionRecord();
                    break;
                case Action.ArchiveProject:
                case Action.RestoreProject:
                case Action.RestoreFile:
                    record = new ArchiveRevisionRecord();
                    break;
                case Action.CreateProject:
                case Action.AddProject:
                case Action.AddFile:
                case Action.DeleteProject:
                case Action.DeleteFile:
                case Action.RecoverProject:
                case Action.RecoverFile:
                case Action.CreateFile:
                default:
                    record = new CommonRevisionRecord();
                    break;
            }
            return record;
        }
    }
}

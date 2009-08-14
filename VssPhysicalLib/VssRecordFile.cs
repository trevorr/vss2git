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

using System.IO;
using System.Text;

namespace Hpdi.VssPhysicalLib
{
    /// <summary>
    /// Represents a file containing VSS records.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class VssRecordFile
    {
        private readonly string filename;
        protected readonly BufferReader reader;

        public string Filename
        {
            get { return filename; }
        }

        public VssRecordFile(string filename, Encoding encoding)
        {
            this.filename = filename;
            reader = new BufferReader(encoding, ReadFile(filename));
        }

        public void ReadRecord(VssRecord record)
        {
            try
            {
                RecordHeader recordHeader = new RecordHeader();
                recordHeader.Read(reader);

                BufferReader recordReader = reader.Extract(recordHeader.Length);

                // comment records always seem to have a zero CRC
                if (recordHeader.Signature != CommentRecord.SIGNATURE)
                {
                    recordHeader.CheckCrc();
                }

                recordHeader.CheckSignature(record.Signature);

                record.Read(recordReader, recordHeader);
            }
            catch (EndOfBufferException e)
            {
                throw new RecordTruncatedException(e.Message);
            }
        }

        public void ReadRecord(VssRecord record, int offset)
        {
            reader.Offset = offset;
            ReadRecord(record);
        }

        public bool ReadNextRecord(VssRecord record)
        {
            while (reader.Remaining > RecordHeader.LENGTH)
            {
                try
                {
                    RecordHeader recordHeader = new RecordHeader();
                    recordHeader.Read(reader);

                    BufferReader recordReader = reader.Extract(recordHeader.Length);

                    // comment records always seem to have a zero CRC
                    if (recordHeader.Signature != CommentRecord.SIGNATURE)
                    {
                        recordHeader.CheckCrc();
                    }

                    if (recordHeader.Signature == record.Signature)
                    {
                        record.Read(recordReader, recordHeader);
                        return true;
                    }
                }
                catch (EndOfBufferException e)
                {
                    throw new RecordTruncatedException(e.Message);
                }
            }
            return false;
        }

        protected delegate T CreateRecordCallback<T>(
            RecordHeader recordHeader, BufferReader recordReader);

        protected T GetRecord<T>(
            CreateRecordCallback<T> creationCallback,
            bool ignoreUnknown)
            where T : VssRecord
        {
            RecordHeader recordHeader = new RecordHeader();
            recordHeader.Read(reader);

            BufferReader recordReader = reader.Extract(recordHeader.Length);

            // comment records always seem to have a zero CRC
            if (recordHeader.Signature != CommentRecord.SIGNATURE)
            {
                recordHeader.CheckCrc();
            }

            T record = creationCallback(recordHeader, recordReader);
            if (record != null)
            {
                // double-check that the object signature matches the file
                recordHeader.CheckSignature(record.Signature);
                record.Read(recordReader, recordHeader);
            }
            else if (!ignoreUnknown)
            {
                throw new UnrecognizedRecordException(recordHeader,
                    string.Format("Unrecognized record signature {0} in item file",
                    recordHeader.Signature));
            }
            return record;
        }

        protected T GetRecord<T>(
            CreateRecordCallback<T> creationCallback,
            bool ignoreUnknown,
            int offset)
            where T : VssRecord
        {
            reader.Offset = offset;
            return GetRecord<T>(creationCallback, ignoreUnknown);
        }

        protected T GetNextRecord<T>(
            CreateRecordCallback<T> creationCallback,
            bool skipUnknown)
            where T : VssRecord
        {
            while (reader.Remaining > RecordHeader.LENGTH)
            {
                T record = GetRecord<T>(creationCallback, skipUnknown);
                if (record != null)
                {
                    return record;
                }
            }
            return null;
        }

        private static byte[] ReadFile(string filename)
        {
            byte[] data;
            using (var stream = new FileStream(filename,
                FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
            }
            return data;
        }
    }
}

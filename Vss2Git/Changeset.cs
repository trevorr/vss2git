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
using Hpdi.VssLogicalLib;

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// Represents a set of revisions made by a particular person at a particular time.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class Changeset
    {
        private DateTime dateTime;
        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        private string user;
        public string User
        {
            get { return user; }
            set { user = value; }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        private readonly LinkedList<Revision> revisions = new LinkedList<Revision>();
        public LinkedList<Revision> Revisions
        {
            get { return revisions; }
        }

        private readonly HashSet<string> targetFiles = new HashSet<string>();
        public HashSet<string> TargetFiles
        {
            get { return targetFiles; }
        }
    }
}

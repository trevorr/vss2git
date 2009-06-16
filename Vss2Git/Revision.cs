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
    /// Represents a single revision to a file or directory.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class Revision
    {
        private readonly DateTime dateTime;
        public DateTime DateTime
        {
            get { return dateTime; }
        }

        private readonly string user;
        public string User
        {
            get { return user; }
        }

        private readonly VssItemName item;
        public VssItemName Item
        {
            get { return item; }
        } 

        private readonly int version;
        public int Version
        {
            get { return version; }
        }

        private readonly string comment;
        public string Comment
        {
            get { return comment; }
        }

        private readonly VssAction action;
        public VssAction Action
        {
            get { return action; }
        }

        public Revision(DateTime dateTime, string user, VssItemName item,
            int version, string comment, VssAction action)
        {
            this.dateTime = dateTime;
            this.user = user;
            this.item = item;
            this.version = version;
            this.comment = comment;
            this.action = action;
        }
    }
}

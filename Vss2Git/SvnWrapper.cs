/* Copyright 2012 Remigius stalder, Descom Consulting Ltd.
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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Hpdi.Vss2Git
{
    class SvnWrapper : AbstractVcsWrapper
    {
        public static readonly string svnExecutable = "svn";

        private string username;
        private string password;
        private string trunk;
        private string tags;
        private string trunkUrl;
        private string tagsUrl;

        public SvnWrapper(string repoPath, string trunk, string tags, Logger logger)
            : base(repoPath, logger, svnExecutable)
        {
            this.trunk = trunk;
            this.tags = tags;
        }

        public void SetCredentials(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public override void Init()
        {
            var startInfo = GetStartInfo("info");
            string stdout;
            string stderr;
            int exitCode = Execute(startInfo, out stdout, out stderr);
            var result = Regex.Split(stdout, "\r\n|\r|\n");
            foreach (string line in result)
            {
                if (line.StartsWith("URL: "))
                {
                    trunkUrl = line.Substring(5);
                    if (!trunkUrl.EndsWith("/" + trunk))
                    {
                        Logger.WriteLine("WARNING: svn trunk URL " + trunkUrl + " does not end with trunk suffix /" + trunk);
                    }
                    tagsUrl = trunkUrl.Substring(0, trunkUrl.Length - trunk.Length) + tags;
                    Logger.WriteLine("svn trunk URL is " + trunkUrl);
                    Logger.WriteLine("svn tags  URL is " + tagsUrl);

                }
            }
        }

        public override void Configure()
        {
            SetCredentials("remigius", "qwe");
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                AddInitialArguments("--username " + username + " --password " + password);
            }
        }

        public override bool Add(string path)
        {
            var startInfo = GetStartInfo("add " + QuoteRelativePath(path));

            // add fails if there are no files (directories don't count)
            bool result = ExecuteUnless(startInfo, "is already under version control");
            SetNeedsCommit();
            return result;
        }

        public override bool AddDir(string path)
        {
            VcsExec("add " + QuoteRelativePath(path));
            SetNeedsCommit();
            return true;
        }

        public override bool AddAll()
        {
            return true;
        }

        public override void RemoveFile(string path)
        {
            VcsExec("rm " + QuoteRelativePath(path));
            SetNeedsCommit();
        }

        public override void RemoveDir(string path, bool recursive)
        {
            VcsExec("rm " + QuoteRelativePath(path));
            SetNeedsCommit();
        }

        public override void RemoveEmptyDir(string path)
        {
            VcsExec("rm " + QuoteRelativePath(path));
            SetNeedsCommit();
        }

        public override void Move(string sourcePath, string destPath)
        {
            VcsExec("mv " + QuoteRelativePath(sourcePath) + " " + QuoteRelativePath(destPath));
            SetNeedsCommit();
        }

        public override void MoveEmptyDir(string sourcePath, string destPath)
        {
            VcsExec("mv " + QuoteRelativePath(sourcePath) + " " + QuoteRelativePath(destPath));
            SetNeedsCommit();
        }

        public override bool DoCommit(string authorName, string authorEmail, string comment, DateTime localTime)
        {
            TempFile tempFile;
            var suffix = GetCommentOption(comment, out tempFile);
            string command = "commit " + suffix;

            var startInfo = GetStartInfo(command);
            if (!ExecuteUnless(startInfo, "is out of date"))
            {
                Logger.WriteLine("-> update before re-commit");
                VcsExec("update");
                VcsExec(command);
            }

            SetRevisionProperties(authorEmail, localTime);
            return true;
        }

        public override void Tag(string name, string taggerName, string taggerEmail, string comment, DateTime localTime)
        {
            TempFile tempFile;
            var suffix = GetCommentOption(comment, out tempFile);
            string command = "cp " + QuoteRelativePath(trunkUrl) + " " + QuoteRelativePath(tagsUrl + "/" + name) + " " + suffix;
            VcsExec(command);
            SetRevisionProperties(taggerEmail, localTime);
        }

        private string GetCommentOption(string comment, out TempFile tempFile)
        {
            if (comment.StartsWith("-"))
            {
                tempFile = new TempFile();
                tempFile.Write(comment, Encoding.UTF8);
                return "-F " + Quote(tempFile.Name);
            }
            else
            {
                tempFile = null;
                return "-m " + Quote(comment);
            }
        }

        public void SetRevisionProperties(string authorEmail, DateTime localTime)
        {
            // set committer:            svn propset svn:author --revprop -r revision_number your_username URL
            // set commit date:          svn propset svn:date --revprop -r 12345 2009-02-12T00:44:04.921324Z URL
            // set commit message:       svn propset svn:log --revprop -r N "new log message" URL
            SetRevisionProperty("HEAD", "svn:author", authorEmail);
            SetRevisionProperty("HEAD", "svn:date", GetUtcTimeString(localTime));
        }

        private void SetRevisionProperty(string revision, string name, string value)
        {
            // set svn revision property: svn propset PROPNAME --revprop -r REV [PROPVAL | -F VALFILE] [TARGET]
            // revision_number can also be HEAD
            VcsExec("propset " + name + " --revprop -r " + revision + " " + value);
        }

        private void SetRevisionPropertyFile(string revision, string name, TempFile tempFile)
        {
            // set svn revision property: svn propset PROPNAME --revprop -r REV [PROPVAL | -F VALFILE] [TARGET]
            // revision_number can also be HEAD
            VcsExec("propset " + name + " --revprop -r " + revision + " -F " + Quote(tempFile.Name));
        }

        private static string GetUtcTimeString(DateTime localTime)
        {
            // convert local time to UTC based on whether DST was in effect at the time
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(localTime);

            // format time according to ISO 8601 (avoiding locale-dependent month/day names) - however slightly different than for git
            return utcTime.ToString("yyyy'-'MM'-'ddTHH':'mm':'ss" + ".000000Z");
        }
    }
}

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
using System.Threading;

namespace Hpdi.Vss2Git
{
    class SvnWrapper : AbstractVcsWrapper
    {
        public static readonly string svnMetaDir = ".svn";
        public static readonly string stdTrunk = "trunk";
        public static readonly string stdTags = "tags";
        public static readonly string stdBranches = "branches";
        public static readonly string svnExecutable = "svn";
        public static readonly string svnAdmin = "svnadmin";
        public static readonly string protocolFile = "file:";
        public static readonly string protocolSvn = "svn:";
        public static readonly string protocolHttp = "http:";
        public static readonly string protocolHttps = "https:";

        private string repoUrl;
        private string projectPath;
        private string trunk;
        private string tags;
        private string branches;
        private string username;
        private string password;
        private string localRepoPath;
        private string projectRootUrl;
        private string trunkUrl;
        private string tagsUrl;
        private string branchesUrl;

        public SvnWrapper(string outputDirectory, string repoUrl, string projectPath,
            string trunk, string tags, string branches, Logger logger)
            : base(outputDirectory, logger, svnExecutable, svnMetaDir)
        {
            this.repoUrl = repoUrl;
            this.projectPath = projectPath;
            this.trunk = trunk;
            this.tags = tags;
            this.branches = branches;
        }

        public void SetCredentials(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public override void Init(bool resetRepo)
        {
            SetRepoUrls();
            if (resetRepo)
            {
                DeleteDirectory(GetOutputDirectory());
                DeleteDirectory(localRepoPath);
                Thread.Sleep(0);
                Directory.CreateDirectory(GetOutputDirectory());
                Directory.CreateDirectory(localRepoPath);
            }
            if (!Directory.Exists(Path.Combine(GetOutputDirectory(), svnMetaDir)))
            {
                CreateLocalRepo();
                CheckoutWorkingCopy();
            }
        }

        public override void Configure()
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                AddInitialArguments("--username " + username + " --password " + password);
            }
            CheckOutputDirectory();
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

        private void SetRepoUrls()
        {
            if (repoUrl.EndsWith(trunk))
            {
                repoUrl = repoUrl.Substring(0, repoUrl.Length - trunk.Length - 1);
            }
            if (repoUrl.StartsWith(protocolSvn) || repoUrl.StartsWith(protocolHttp)
                || repoUrl.StartsWith(protocolHttps))
            {
                Logger.WriteLine("using remote svn repo at " + repoUrl);
            }
            else
            {
                if (repoUrl.StartsWith(protocolFile))
                {
                    localRepoPath = repoUrl.Substring(protocolFile.Length);
                    string[] legalPrefixes = new string[] { "///", "//localhost/" };
                    for (int i = 0; i < legalPrefixes.Length; i++)
                    {
                        if (localRepoPath.StartsWith(legalPrefixes[i]))
                        {
                            localRepoPath = localRepoPath.Substring(legalPrefixes[i].Length);
                            break;
                        }
                    }
                }
                else
                {
                    localRepoPath = repoUrl;
                }
                localRepoPath = Path.GetFullPath(localRepoPath);
                repoUrl = protocolFile + "///" + localRepoPath.Replace('\\', '/');
                Logger.WriteLine("using local svn repo at " + repoUrl);
            }
            projectRootUrl = repoUrl;
            if (!string.IsNullOrEmpty(projectPath))
            {
                projectRootUrl += "/" + projectPath;
            }
            trunkUrl = projectRootUrl + "/" + trunk;
            tagsUrl = projectRootUrl + "/" + tags;
            branchesUrl = projectRootUrl + "/" + branches;

            Logger.WriteLine("svn trunk    URL is " + trunkUrl);
            Logger.WriteLine("svn tags     URL is " + tagsUrl);
            Logger.WriteLine("svn branches URL is " + branchesUrl);
        }

        protected override void CheckOutputDirectory()
        {
            // check if the directory is initial, i.e. it is empty except for the meta directory .svn
            base.CheckOutputDirectory();
            // check if the checkout is to trunk
            var startInfo = GetStartInfo("info");
            string stdout;
            string stderr;
            int exitCode = Execute(startInfo, out stdout, out stderr);
            var result = Regex.Split(stdout, "\r\n|\r|\n");
            foreach (string line in result)
            {
                if (line.StartsWith("URL: "))
                {
                    string workingUrl = line.Substring(5);
                    if (!workingUrl.Equals(trunkUrl))
                    {
                        throw new ApplicationException("Checked out URL is not trunk: " + workingUrl);
                    }
                }
            }
        }

        private void CreateLocalRepo()
        {
            if (string.IsNullOrEmpty(localRepoPath))
            {
                return;
            }
            Logger.WriteLine("creating local repo at " + localRepoPath);
            Exec(svnAdmin, "create " + Quote(localRepoPath));
            // write pre-revprop-change.bat to repo\hooks\pre-revprop-change.bat
            string hookContent = "REM PRE-REVPROP-CHANGE HOOK\nREM exit 0 to allow changing all revision properties\n\nexit 0";
            WriteStringToFile(localRepoPath + "\\hooks\\pre-revprop-change.bat", hookContent);
            /*
            string confDir = localRepoPath + "\\conf";
            string hooksDir = localRepoPath + "\\hooks";
            // write svnserve.conf to repo\conf\svnserve.conf
            // write password file to repo\conf\passwd
             */
            // create base path if necessary
            string parentPath = repoUrl;
            if (!string.IsNullOrEmpty(projectPath))
            {
                string[] fragments = projectPath.Split('/');
                foreach (string fragment in fragments)
                {
                    CreateSvnDir(parentPath, fragment);
                    parentPath += "/" + fragment;
                }
            }
            // create trunk, tags, branches
            CreateSvnDir(parentPath, trunk);
            CreateSvnDir(parentPath, tags);
            CreateSvnDir(parentPath, branches);
        }

        private void CreateSvnDir(string parent, string dir)
        {
            // svn mkdir -m "create test" svn://localhost/test
            VcsExec("mkdir -m " + Quote("create " + dir) + " " + Quote(parent + "/" + dir));
        }

        private void CheckoutWorkingCopy()
        {
            Logger.WriteLine("checking out " + trunkUrl + " to " + GetOutputDirectory());
            VcsExec("checkout " + Quote(trunkUrl) + " " + Quote(GetOutputDirectory()));
            if (!Directory.Exists(Path.Combine(GetOutputDirectory(), svnMetaDir)))
            {
                throw new ApplicationException(svnMetaDir + " directory not present after checkout");
            }
        }
    }
}

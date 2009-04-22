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
using System.Text;
using System.Threading;

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// Wraps execution of Git and implements the common Git commands.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class GitWrapper
    {
        private readonly string repoPath;
        private readonly Logger logger;
        private readonly Stopwatch stopwatch = new Stopwatch();

        public TimeSpan ElapsedTime
        {
            get { return stopwatch.Elapsed; }
        }

        public GitWrapper(string repoPath, Logger logger)
        {
            this.repoPath = repoPath;
            this.logger = logger;
        }

        public void Init()
        {
            GitExec("init");
        }

        public bool Add(string path)
        {
            var startInfo = GetStartInfo("add " + Quote(path));

            // add fails if there are no files (directories don't count)
            return ExecuteUnless(startInfo, "did not match any files");
        }

        public bool Add(IEnumerable<string> paths)
        {
            if (CollectionUtil.IsEmpty(paths))
            {
                return false;
            }

            var args = new StringBuilder("add ");
            CollectionUtil.Join(args, " ", CollectionUtil.Transform<string, string>(paths, Quote));
            var startInfo = GetStartInfo(args.ToString());

            // add fails if there are no files (directories don't count)
            return ExecuteUnless(startInfo, "did not match any files");
        }

        public bool AddAll()
        {
            var startInfo = GetStartInfo("add -A");

            // add fails if there are no files (directories don't count)
            return ExecuteUnless(startInfo, "did not match any files");
        }

        public void Remove(string path, bool recursive)
        {
            GitExec("rm " + (recursive ? "-r " : "") + Quote(path));
        }

        public void Move(string sourcePath, string destPath)
        {
            if (sourcePath.Equals(destPath, StringComparison.OrdinalIgnoreCase))
            {
                // workaround for git not supporting case preservation on case-
                // insensitive file systems; note that this is only intended to
                // support a case change in the last segment of the path
                var tempPath = sourcePath + ".gitmvtmp";
                Move(sourcePath, tempPath);
                Move(tempPath, destPath);
            }
            else
            {
                GitExec("mv " + Quote(sourcePath) + " " + Quote(destPath));
            }
        }

        public bool Commit(string authorName, string authorEmail, string comment, DateTime localTime)
        {
            var args = "commit";
            if (!string.IsNullOrEmpty(comment))
            {
                args += " -m " + Quote(comment);
            }

            // convert local time to UTC based on whether DST was in effect at the time
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(localTime);

            // format time according to RFC 2822
            var utcTimeStr = utcTime.ToString("ddd MMM dd HH':'mm':'ss yyyy +0000");

            var startInfo = GetStartInfo(args);
            startInfo.EnvironmentVariables["GIT_AUTHOR_NAME"] = authorName;
            startInfo.EnvironmentVariables["GIT_AUTHOR_EMAIL"] = authorEmail;
            startInfo.EnvironmentVariables["GIT_AUTHOR_DATE"] = utcTimeStr;

            // ignore empty commits, since they are non-trivial to detect
            // (e.g. when renaming a directory)
            return ExecuteUnless(startInfo, "nothing to commit");
        }

        public void Tag(string name, string comment)
        {
            var args = "tag " + Quote(name);
            if (!string.IsNullOrEmpty(comment))
            {
                args += " -m " + Quote(comment);
            }
            GitExec(args);
        }

        private void GitExec(string args)
        {
            var startInfo = GetStartInfo(args);
            ExecuteUnless(startInfo, null);
        }

        private ProcessStartInfo GetStartInfo(string args)
        {
            var startInfo = new ProcessStartInfo("git", args);
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.WorkingDirectory = repoPath;
            startInfo.CreateNoWindow = true;
            return startInfo;
        }

        private bool ExecuteUnless(ProcessStartInfo startInfo, string unless)
        {
            string stdout, stderr;
            int exitCode = Execute(startInfo, out stdout, out stderr);
            if (exitCode != 0)
            {
                if (string.IsNullOrEmpty(unless) ||
                    ((string.IsNullOrEmpty(stdout) || !stdout.Contains(unless)) &&
                     (string.IsNullOrEmpty(stderr) || !stderr.Contains(unless))))
                {
                    FailExitCode(startInfo.Arguments, stdout, stderr, exitCode);
                }
            }
            return exitCode == 0;
        }

        private static void FailExitCode(string args, string stdout, string stderr, int exitCode)
        {
            throw new ProcessExitException(
                string.Format("git returned exit code {0}", exitCode),
                args, stdout, stderr);
        }

        private int Execute(ProcessStartInfo startInfo, out string stdout, out string stderr)
        {
            logger.WriteLine("Executing: {0} {1}", startInfo.FileName, startInfo.Arguments);
            stopwatch.Start();
            try
            {
                using (var process = Process.Start(startInfo))
                {
                    process.StandardInput.Close();
                    var stdoutReader = new AsyncLineReader(process.StandardOutput.BaseStream);
                    var stderrReader = new AsyncLineReader(process.StandardError.BaseStream);

                    var activityEvent = new ManualResetEvent(false);
                    EventHandler activityHandler = delegate { activityEvent.Set(); };
                    process.Exited += activityHandler;
                    stdoutReader.DataReceived += activityHandler;
                    stderrReader.DataReceived += activityHandler;

                    stdout = null;
                    stderr = null;
                    while (true)
                    {
                        activityEvent.Reset();

                        while (true)
                        {
                            string line = stdoutReader.ReadLine();
                            if (line != null)
                            {
                                stdout = line;
                                logger.Write('>');
                            }
                            else
                            {
                                line = stderrReader.ReadLine();
                                if (line != null)
                                {
                                    stderr = line;
                                    logger.Write('!');
                                }
                                else
                                {
                                    break;
                                }
                            }
                            logger.WriteLine(line.TrimEnd());
                        }

                        if (process.HasExited)
                        {
                            break;
                        }

                        activityEvent.WaitOne(1000);
                    }

                    return process.ExitCode;
                }
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private const char QuoteChar = '"';
        private const char EscapeChar = '\\';

        private static string Quote(string arg)
        {
            if (string.IsNullOrEmpty(arg))
            {
                return "\"\"";
            }

            StringBuilder buf = null;
            for (int i = 0; i < arg.Length; ++i)
            {
                char c = arg[i];
                if (buf == null && (char.IsWhiteSpace(c) || c == QuoteChar))
                {
                    buf = new StringBuilder(arg.Length + 2);
                    buf.Append(QuoteChar);
                    buf.Append(arg, 0, i);
                }
                if (buf != null)
                {
                    if (c == QuoteChar)
                    {
                        buf.Append(EscapeChar);
                    }
                    buf.Append(c);
                }
            }
            if (buf != null)
            {
                buf.Append(QuoteChar);
                return buf.ToString();
            }
            return arg;
        }
    }
}

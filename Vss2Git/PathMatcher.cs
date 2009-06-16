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

using System.Text;
using System.Text.RegularExpressions;

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// Determines whether a path matches a set of glob patterns.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class PathMatcher
    {
        public const string AnyPathPattern = "**";
        public const string AnyNamePattern = "*";
        public const string AnyNameCharPattern = "?";

        private static readonly char[] directorySeparators = { '/', '\\' };

        private readonly Regex regex;

        public PathMatcher(string pattern)
        {
            regex = new Regex(ConvertPattern(pattern),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public PathMatcher(string[] patterns)
        {
            regex = new Regex(ConvertPatterns(patterns),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public bool Matches(string path)
        {
            return regex.IsMatch(path);
        }

        private static string ConvertPattern(string glob)
        {
            var buf = new StringBuilder(glob.Length * 2);
            ConvertPatternInto(glob, buf);
            return buf.ToString();
        }

        private static string ConvertPatterns(string[] globs)
        {
            var buf = new StringBuilder();
            foreach (var glob in globs)
            {
                if (buf.Length > 0)
                {
                    buf.Append('|');
                }
                ConvertPatternInto(glob, buf);
            }
            return buf.ToString();
        }

        private static void ConvertPatternInto(string glob, StringBuilder buf)
        {
            for (int i = 0; i < glob.Length; ++i)
            {
                char c = glob[i];
                switch (c)
                {
                    case '.':
                    case '$':
                    case '^':
                    case '{':
                    case '[':
                    case '(':
                    case '|':
                    case ')':
                    case '+':
                        // escape regex operators
                        buf.Append('\\');
                        buf.Append(c);
                        break;
                    case '/':
                    case '\\':
                        // accept either directory separator
                        buf.Append(@"[/\\]");
                        break;
                    case '*':
                        if (i + 1 < glob.Length && glob[i + 1] == '*')
                        {
                            // match any path
                            buf.Append(".*");
                            ++i;
                        }
                        else
                        {
                            // match any name
                            buf.Append(@"[^/\\]*");
                        }
                        break;
                    case '?':
                        // match any name char
                        buf.Append(@"[^/\\]");
                        break;
                    default:
                        // passthrough char
                        buf.Append(c);
                        break;
                }
            }
            buf.Append('$');
        }
    }
}

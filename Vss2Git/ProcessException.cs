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

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// Exception thrown while executing an external process.
    /// </summary>
    /// <author>Trevor Robinson</author>
    class ProcessException : Exception
    {
        private readonly string executable;
        public string Executable
        {
            get { return executable; }
        }

        private readonly string arguments;
        public string Arguments
        {
            get { return arguments; }
        }

        public ProcessException(string message, string executable, string arguments)
            : base(message)
        {
            this.executable = executable;
            this.arguments = arguments;
        }

        public ProcessException(string message, Exception innerException, string executable, string arguments)
            : base(message, innerException)
        {
            this.executable = executable;
            this.arguments = arguments;
        }
    }
}

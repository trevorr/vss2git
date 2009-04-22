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
    /// Formats exceptions expected in this application with type-specific details.
    /// </summary>
    /// <author>Trevor Robinson</author>
    static class ExceptionFormatter
    {
        public static string Format(Exception e)
        {
            var message = e.Message;

            var processExit = e as ProcessExitException;
            if (processExit != null)
            {
                message = string.Format("{0}\nArguments: {1}\nStdout: {2}\nStderr: {3}",
                    message, processExit.Arguments, processExit.Stdout, processExit.Stderr);
            }

            return message;
        }
    }
}

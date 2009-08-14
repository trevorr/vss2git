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
using System.Windows.Forms;

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// Base class for queued workers in the application.
    /// </summary>
    /// <author>Trevor Robinson</author>
    abstract class Worker
    {
        protected readonly WorkQueue workQueue;
        protected readonly Logger logger;

        public Worker(WorkQueue workQueue, Logger logger)
        {
            this.workQueue = workQueue;
            this.logger = logger;
        }

        protected void LogStatus(object work, string status)
        {
            workQueue.SetStatus(work, status);
            logger.WriteLine(status);
        }

        protected string LogException(Exception exception)
        {
            var message = ExceptionFormatter.Format(exception);
            LogException(exception, message);
            return message;
        }

        protected void LogException(Exception exception, string message)
        {
            logger.WriteLine("ERROR: {0}", message);
            logger.WriteLine(exception);
        }

        protected void ReportError(string message)
        {
            var button = MessageBox.Show(message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            if (button == DialogResult.Cancel)
            {
                workQueue.Abort();
            }
        }
    }
}

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
using System.Threading;

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// Simple work queue over a bounded number of thread-pool threads.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class SimpleWorkQueue
    {
        private readonly LinkedList<WaitCallback> workQueue = new LinkedList<WaitCallback>();
        private readonly int maxThreads;
        private int activeThreads = 0;
        private bool suspended = false;
        private volatile bool aborting = false;

        public SimpleWorkQueue()
        {
            this.maxThreads = Environment.ProcessorCount;
        }

        public SimpleWorkQueue(int maxThreads)
        {
            this.maxThreads = maxThreads;
        }

        public bool IsIdle
        {
            get { return activeThreads == 0; }
        }

        public bool IsFullyActive
        {
            get { return activeThreads == maxThreads; }
        }

        public bool IsSuspended
        {
            get { return suspended; }
        }

        public bool IsAborting
        {
            get { return aborting; }
        }

        // Adds work to the head of the work queue. Useful for workers that
        // want to reschedule themselves on suspend.
        public void AddFirst(WaitCallback work)
        {
            lock (workQueue)
            {
                workQueue.AddFirst(work);
                StartWorker();
            }
        }

        // Adds work to the tail of the work queue.
        public void AddLast(WaitCallback work)
        {
            lock (workQueue)
            {
                workQueue.AddLast(work);
                StartWorker();
            }
        }

        // Clears pending work without affecting active work.
        public void ClearPending()
        {
            lock (workQueue)
            {
                workQueue.Clear();
            }
        }

        // Stops processing of pending work.
        public void Suspend()
        {
            lock (workQueue)
            {
                suspended = true;
            }
        }

        // Resumes processing of pending work after being suspended.
        public void Resume()
        {
            lock (workQueue)
            {
                suspended = false;
                while (activeThreads < workQueue.Count)
                {
                    StartWorker();
                }
            }
        }

        // Signals active workers to abort and clears pending work.
        public void Abort()
        {
            lock (workQueue)
            {
                if (activeThreads > 0)
                {
                    // flag active workers to stop; last will reset the flag
                    aborting = true;
                }

                // to avoid non-determinism, always clear the queue
                workQueue.Clear();
            }
        }

        protected virtual void OnActive()
        {
        }

        protected virtual void OnIdle()
        {
            // auto-reset abort flag
            aborting = false;
        }

        protected virtual void OnStart(WaitCallback work)
        {
        }

        protected virtual void OnStop(WaitCallback work)
        {
        }

        protected virtual void OnException(WaitCallback work, Exception e)
        {
        }

        // Assumes work queue lock is held.
        private void StartWorker()
        {
            if (activeThreads < maxThreads && !suspended)
            {
                if (++activeThreads == 1)
                {
                    // hook for transition from Idle to Active
                    OnActive();
                }
                ThreadPool.QueueUserWorkItem(Worker);
            }
        }

        private void Worker(object state)
        {
            while (true)
            {
                WaitCallback work;
                lock (workQueue)
                {
                    var head = workQueue.First;
                    if (head == null || suspended)
                    {
                        if (--activeThreads == 0)
                        {
                            // hook for transition from Active to Idle
                            OnIdle();
                        }
                        return;
                    }
                    work = head.Value;
                    workQueue.RemoveFirst();
                }

                // hook for worker initialization
                OnStart(work);
                try
                {
                    work(work);
                }
                catch (Exception e)
                {
                    // hook for worker exceptions
                    OnException(work, e);
                }
                finally
                {
                    // hook for worker cleanup
                    OnStop(work);
                }
            }
        }
    }
}

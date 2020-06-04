﻿/* Copyright 2009 HPDI, LLC
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
using System.Threading;
using System.Windows.Forms;

namespace Hpdi.Vss2Git
{
    /// <summary>
    /// Entrypoint to the application.
    /// </summary>
    /// <author>Trevor Robinson</author>
    static class Program
    {
        [STAThread]
        static void Main()
        {
	        Application.ThreadException += ApplicationOnThreadException;

			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

	    private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs args)
	    {
		    MessageBox.Show(args.Exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
	    }
    }
}

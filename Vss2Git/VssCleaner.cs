using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Hpdi.Vss2Git
{
    internal class VssCleaner : Worker
    {
        private string gitRepoDir = "";
        private string emailDomain = "";
        private List<string> sccFilePaths;
        private List<string> sccBakFilePaths = new List<string>();
        private List<string> slnFilePaths;
        private List<string> slnBakFilePaths = new List<string>();
        private List<string> elemBaseProjFilePath;
        private List<string> elemBaseProjBakFilePaths = new List<string>();
        private List<string> attrBaseProjFilePath;
        private List<string> attrBaseProjBakFilePaths = new List<string>();

        public VssCleaner(WorkQueue workQueue, Logger logger, string outDir, string domain)
            : base(workQueue, logger)
        {
            gitRepoDir = outDir;
            emailDomain = domain;
        }

        public void Commit(string comment)
        {
            GitWrapper gitWrapper = new GitWrapper(gitRepoDir, logger);
            gitWrapper.CommitEncoding = Encoding.UTF8;

            while (!gitWrapper.FindExecutable())
            {
                var button = MessageBox.Show(@"Git not found in PATH. 
                    If you need to modify your PATH variable, please 
                    restart the program for the changes to take effect.",
                    "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (button == DialogResult.Cancel)
                {
                    workQueue.Abort();
                    return;
                }
            }

            gitWrapper.AddAll();
            gitWrapper.Commit(Environment.UserName, $"{Environment.UserName}@{emailDomain}", comment, DateTime.Now);
        }

        public void CleanVssRemnants()
        {
            workQueue.AddLast((object work) =>
            {
                try
                {
                    CleanVssSetting();

                    try
                    {
                        CleanSccFiles();
                    }
                    catch (Exception)
                    {
                        GitWrapper gitWrapper = new GitWrapper(gitRepoDir, logger);
                        gitWrapper.UndoLastCommit();
                        throw;
                    }
                }
                catch (Exception)
                {
                    logger.WriteLine("Failed to clean VSS settings & related files in the output repository.");
                }
            });
        }

        public void CleanSccFiles()
        {
            try
            {
                sccFilePaths = Directory.EnumerateFiles(gitRepoDir, "*.*", SearchOption.AllDirectories)
                .Where(file => file.EndsWith(".vssscc") || file.EndsWith(".vspscc") || file.EndsWith(".scc")).ToList();

                foreach (var path in sccFilePaths)
                {
                    CleanSccFile(path);
                }

                DeleteSccFileBackups();
            }
            catch (Exception)
            {
                RollBackSccFileBackup();
                throw;
            }
            Commit("Delete VSS related file in output repository.");
        }

        public void CleanVssSetting()
        {
            try
            {
                slnFilePaths = Directory.EnumerateFiles(gitRepoDir, "*.sln", SearchOption.AllDirectories).ToList();
                elemBaseProjFilePath = Directory.EnumerateFiles(gitRepoDir, "*.*", SearchOption.AllDirectories)
                    .Where(file => file.EndsWith(".csproj") || file.EndsWith(".vbproj") || file.EndsWith(".vcxproj")).ToList();
                attrBaseProjFilePath = Directory.EnumerateFiles(gitRepoDir, "*.*", SearchOption.AllDirectories)
                    .Where(file => file.EndsWith(".vcproj") || file.EndsWith(".vdproj")).ToList();

                foreach (var path in slnFilePaths)
                {
                    CleanSolutionSetting(path);
                }

                foreach (var path in elemBaseProjFilePath)
                {
                    CleanElemBaseProjSetting(path);
                }

                foreach (var path in attrBaseProjFilePath)
                {
                    var ext = Path.GetExtension(path);
                    if (ext == ".vcproj")
                    {
                        CleanAttrBaseProjSetting(path, "Scc\\w*\\s*=\\s*\\\".*\\\"");
                    }
                    else if (ext == ".vdproj")
                    {
                        CleanAttrBaseProjSetting(path, "\\\"Scc\\w*\\\"\\s*=\\s*\\\".*\\\"");
                    }
                }

                DeleteSettingFileBackups();
            }
            catch (Exception)
            {
                RollbackProjectAndSolutionFileBackup();
                throw;
            }
            Commit("Clean VSS setting in .proj & .sln file.");
        }

        private void CleanSccFile(string sccFilePath)
        {
            var fileName = Path.GetFileName(sccFilePath);
            var dir = Path.GetDirectoryName(sccFilePath);
            var bakFilePath = $@"{dir}\bak-{fileName}";

            try
            {
                File.Copy(sccFilePath, bakFilePath);
                sccBakFilePaths.Add(bakFilePath);

                File.Delete(sccFilePath);
            }
            catch (Exception)
            {
                logger.WriteLine("Failed to clean SCC section in scc file.");
                throw;
            }

        }

        private void CleanSolutionSetting(string soluFilePath)
        {
            var fileName = Path.GetFileName(soluFilePath);
            var dir = Path.GetDirectoryName(soluFilePath);
            var tmpFilePath = $@"{dir}\temp-{fileName}";
            var bakFilePath = $@"{dir}\bak-{fileName}";

            try
            {
                using (StreamReader rdr = new StreamReader(soluFilePath, Encoding.Default))
                {
                    bool inSccSection = false;
                    string line = rdr.ReadLine();

                    using (StreamWriter wri = new StreamWriter(tmpFilePath, false, Encoding.Default))
                    {
                        while (line != null)
                        {
                            if (line.Trim() == "GlobalSection(SourceCodeControl) = preSolution")
                            {
                                inSccSection = true;
                            }
                            else
                            {
                                if (!inSccSection)
                                {
                                    wri.WriteLine(line);
                                }
                                else
                                {
                                    if (line.Trim() == "EndGlobalSection")
                                    {
                                        inSccSection = false;
                                    }
                                }
                            }

                            line = rdr.ReadLine();
                        }
                    }
                }

                File.Move(soluFilePath, bakFilePath);
                slnBakFilePaths.Add(bakFilePath);

                File.Move(tmpFilePath, soluFilePath);
            }
            catch (Exception)
            {
                logger.WriteLine("Failed to clean SCC section in solution file.");
                File.Delete(tmpFilePath);
                throw;
            }
        }

        private void CleanElemBaseProjSetting(string projFilePath)
        {
            var fileName = Path.GetFileName(projFilePath);
            var dir = Path.GetDirectoryName(projFilePath);
            var bakFilePath = $@"{dir}\bak-{fileName}";

            try
            {
                File.Copy(projFilePath, bakFilePath);
                elemBaseProjBakFilePaths.Add(bakFilePath);

                XDocument xml = XDocument.Load(projFilePath);
                xml.Root.Descendants().Where(e =>
                    e.Name.LocalName == "SccProjectName" ||
                    e.Name.LocalName == "SccLocalPath" ||
                    e.Name.LocalName == "SccAuxPath" ||
                    e.Name.LocalName == "SccProvider").Remove();
                xml.Save(projFilePath);
            }
            catch (Exception)
            {
                logger.WriteLine("Failed to clean SCC section in project file.");
                throw;
            }
        }

        private void CleanAttrBaseProjSetting(string projFilePath, string regexPattern)
        {
            var fileName = Path.GetFileName(projFilePath);
            var dir = Path.GetDirectoryName(projFilePath);
            var bakFilePath = $@"{dir}\bak-{fileName}";

            try
            {
                File.Copy(projFilePath, bakFilePath);
                attrBaseProjBakFilePaths.Add(bakFilePath);

                string txtInVcprojFile = "";
                using (StreamReader rdr = new StreamReader(projFilePath, Encoding.Default))
                {
                    txtInVcprojFile = rdr.ReadToEnd();
                }

                Regex r = new Regex(regexPattern);
                using (StreamWriter wri = new StreamWriter(projFilePath, false, Encoding.Default))
                {
                    wri.Write(r.Replace(txtInVcprojFile, ""));
                }
            }
            catch (Exception)
            {
                logger.WriteLine("Failed to clean SCC section in vcproj file.");
                throw;
            }
        }

        private void RollBackSccFileBackup()
        {
            foreach (var path in sccBakFilePaths)
            {
                RecoverBackupFiles(path);
            }
        }

        private void RollbackProjectAndSolutionFileBackup()
        {
            foreach (var path in slnBakFilePaths)
            {
                RecoverBackupFiles(path);
            }

            foreach (var path in elemBaseProjBakFilePaths)
            {
                RecoverBackupFiles(path);
            }

            foreach (var path in attrBaseProjFilePath)
            {
                RecoverBackupFiles(path);
            }
        }

        private void RecoverBackupFiles(string bakPath)
        {
            var oriPath = bakPath.Replace("bak-", "");
            File.Delete(oriPath);
            File.Move(bakPath, oriPath);
        }

        private void DeleteSccFileBackups()
        {
            foreach (var path in sccBakFilePaths)
            {
                File.Delete(path);
            }
        }

        private void DeleteSettingFileBackups()
        {
            foreach (var path in slnBakFilePaths)
            {
                File.Delete(path);
            }

            foreach (var path in elemBaseProjBakFilePaths)
            {
                File.Delete(path);
            }

            foreach (var path in attrBaseProjBakFilePaths)
            {
                File.Delete(path);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

namespace DllCopier
{
    class DllManager
    {
        fsdfsdfsdfsdf
        sdfsdfsdfsdfsdf
        sdfsdfsdfsdfsd
        sdfsdfsdfsdfsdf
        sdfsdfsdfsdfsdf
        public Dictionary<int, List<string>> SelectDll(string[] IDEconst)
        {
            Dictionary<int, List<string>> SeletedDll = new Dictionary<int, List<string>>();
            List<string> tmpList = new List<string>();
            string[] tmpIDEconst = new string[IDEconst.Length - 2];
            for (int i = 2; i < IDEconst.Length; i++) 
            {
                tmpIDEconst[i - 2] = IDEconst[i];
            }
            tmpList.AddRange(tmpIDEconst);
            SeletedDll.Add(Convert.ToInt32(IDEconst[1]), tmpList);             
            
            return SeletedDll;
        }


        private List<string> GetPathesOfSubFolders(string DirPath)
        {
            List<string> pathes = new List<string>();
            FileSystemInfo[] filesInfo;
            DirectoryInfo dirInfo = new DirectoryInfo(DirPath);
            filesInfo = dirInfo.GetFileSystemInfos();
            foreach (FileSystemInfo fsi in filesInfo)
            {
                if (fsi.Attributes == FileAttributes.Directory)
                {
                    pathes.Add(DirPath + "\\" + fsi.Name);
                }
            }
            return pathes;
        }

        public List<string> SelectPathes(List<string> PathesCollection, List<string> SelCrit)
        {            
            List<string> SelectedPathes = new List<string>();
            foreach (string path in PathesCollection)
            {
                int iEqual = 0;
                for (int i = 0; i < SelCrit.Count; i++)
                {
                    if (path.IndexOf(SelCrit[i]) != -1)
                    {
                        iEqual++;
                    }                    
                }
                if (iEqual == SelCrit.Count)
                {
                    SelectedPathes.Add(path);
                }
            }
            return SelectedPathes;
        }

        public List<string> RecBuildPathes(string RootDir)
        {
            List<string> AllWorkedPathes = new List<string>();
            List<string> SubPathes = new List<string>();
            SubPathes = GetPathesOfSubFolders(RootDir);
            if (SubPathes.Count > 0)
            {
                foreach (string path in SubPathes)
                {
                    AllWorkedPathes.AddRange(RecBuildPathes(path));
                }
            }
            else
            {
                AllWorkedPathes.Add(RootDir);
            }
           
            return AllWorkedPathes;
        }

        public string BuildDllPath(string DllName, string DllDirPath, int Dllbit)
        {
            string dllPath = "";
            if (Dllbit == 64)
            {
                DllDirPath = DllDirPath + "\\x64";
            }
            else
            {
                DllDirPath = DllDirPath + "\\x32";
            }
            DirectoryInfo dirInfo = new DirectoryInfo(DllDirPath);
            FileSystemInfo[] fsi = dirInfo.GetFileSystemInfos(DllName);
            if (fsi.Length > 0)
            {
                dllPath = fsi[0].FullName;
            }
            return dllPath;
        }

        public int DllCopy(FileCopier fc, List<string> DestPathes, Dictionary<int, List<string>> DllNames, string DllDirPath)
        {
            foreach (int key in DllNames.Keys)
            {
                foreach (string dllName in DllNames[key])
                {
                    string dllPath = BuildDllPath(dllName, DllDirPath, key);
                    if (dllPath != "")
                    {
                        foreach (string destPath in DestPathes)
                        {
                            string[] lparams = new string[2];
                            lparams[0] = dllPath;
                            lparams[1] = destPath + "\\" + dllName;
                            fc.Copy(lparams);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to find " + dllName);
                        return -1;
                    }
                }                                
            }
            return 0;
        }

        public int DllCopyThread(DataMover DMover)
        {            
            FileCopier fc = DMover.fileCopier;           

            List<string> DestPathes = DMover.DestPathes;
            Dictionary<int, List<string>> DllNames = DMover.DllNames;
            string DllDirPath = DMover.DllFolder;

            foreach (int key in DllNames.Keys)
            {
                foreach (string dllName in DllNames[key])
                {
                    string dllPath = BuildDllPath(dllName, DllDirPath, key);
                    if (dllPath != "")
                    {
                        foreach (string destPath in DestPathes)
                        {
                            string[] lparams = new string[2];
                            lparams[0] = dllPath;
                            lparams[1] = destPath + dllName;
                            fc.Copy(lparams);
                        }
                    }
                    else
                    {
                        return -1;
                    }
                }
            }               
            return 0;
        }

        public List<string> GetIDEPathes(string RootDir, List<string> Criteria)
        {
            List<string> AllPathes = new List<string>();
            List<string> IDEPathes = new List<string>();
            AllPathes = RecBuildPathes(RootDir);
            IDEPathes = SelectPathes(AllPathes, Criteria);
            return IDEPathes;
        }

        public List<string> CreateCriterias(string Profiler, string NameIDE, string Bitn)
        {
            List<string> Criteria = new List<string>();
            Criteria.Add("x" + Bitn);
            Criteria.Add(Profiler);
            Criteria.Add(NameIDE);
            return Criteria;
        }
        //cdcsdclksdjskldfklsdjflsdkjflsdkjf
        //cdcsdclksdjskldfklsdjflsdkjflsdkjf
        //cdcsdclksdjskldfklsdjflsdkjflsdkjf
        //cdcsdclksdjskldfklsdjflsdkjflsdkjf
        //cdcsdclksdjskldfklsdjflsdkjflsdkjf
        //cdcsdclksdjskldfklsdjflsdkjflsdkjf
        //cdcsdclksdjskldfklsdjflsdkjflsdkjf
    }
}

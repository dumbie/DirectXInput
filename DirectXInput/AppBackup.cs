﻿using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace DirectXInput
{
    public partial class AppBackup
    {
        //Backup Json profiles
        public static void BackupJsonProfiles()
        {
            try
            {
                Debug.WriteLine("Creating Json profiles backup.");

                //Create backup directory
                AVFiles.Directory_Create("Backups", false);

                //Cleanup backups
                FileInfo[] fileInfo = new DirectoryInfo("Backups").GetFiles("*.zip");
                foreach (FileInfo backupFile in fileInfo)
                {
                    try
                    {
                        TimeSpan backupSpan = DateTime.Now - backupFile.CreationTime;
                        if (backupSpan.TotalDays > 5)
                        {
                            backupFile.Delete();
                        }
                    }
                    catch { }
                }

                //Create backup
                string backupTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                ZipFile.CreateFromDirectory("Profiles\\User", "Backups\\" + backupTime + "-Profiles.zip");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed making backup: " + ex.Message);
            }
        }
    }
}
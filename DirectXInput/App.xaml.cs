using System;
using System.Windows;
using static ArnoldVinkCode.AVInteropDll;
using static ArnoldVinkCode.AVStartup;
using static DirectXInput.AppBackup;
using static DirectXInput.AppVariables;

namespace DirectXInput
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                //Setup application defaults
                SetupDefaults(ProcessPriorityClasses.HIGH_PRIORITY_CLASS, true);

                //Backup Json profiles
                BackupJsonProfiles();

                //Run application startup code
                await vWindowMain.Application_Startup();
            }
            catch { }
        }
    }
}
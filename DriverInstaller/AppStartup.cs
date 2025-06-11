using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVInteropDll;
using static DriverInstaller.AppVariables;
using static ArnoldVinkCode.AVUpdate;

namespace DriverInstaller
{
    class AppStartup
    {
        public async static Task Startup()
        {
            try
            {
                Debug.WriteLine("Welcome to application.");

                //Setup application defaults
                AVStartup.SetupDefaults(ProcessPriority.Normal, true);

                //Application update cleanup
                await UpdateCleanup();

                //Open the application window
                vWindowMain.Show();
            }
            catch { }
        }
    }
}
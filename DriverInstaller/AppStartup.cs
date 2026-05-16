using ArnoldVinkCode;
using System.Diagnostics;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVProcess;
using static ArnoldVinkCode.AVUpdate;
using static DriverInstaller.AppVariables;

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
                AVStartup.SetupDefaults(ProcessPriorityClasses.Normal, true, false);

                //Application update cleanup
                await UpdateCleanup();

                //Open the application window
                vWindowMain.Show();
            }
            catch { }
        }
    }
}
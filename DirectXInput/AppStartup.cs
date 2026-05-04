using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVUpdate;
using static DirectXInput.AppVariables;

namespace DirectXInput
{
    public partial class WindowMain
    {
        public async Task Application_Startup()
        {
            try
            {
                Debug.WriteLine("Welcome to application.");

                //Clean application update files
                await UpdateCleanup();

                //Check application user folders
                Folders_Check();

                //Application initialize settings
                Settings_Check();
                await Settings_Load();
                Settings_Save();

                //Application initialize shortcuts
                Shortcuts_Check();
                Shortcuts_Load();
                Shortcuts_Save();

                //Create the tray menu
                Application_CreateTrayMenu();

                //Check if application has launched as admin
                if (vAdministratorPermission)
                {
                    this.Title += " (Admin)";
                }

                //Check settings if window needs to be shown
                if (vSettings.Load("AppFirstLaunch", typeof(bool)))
                {
                    Debug.WriteLine("First launch showing the window.");
                    vWindowMain.Show();
                }

                //Check if drivers are installed
                if (!CheckInstalledDrivers())
                {
                    await Message_InstallDrivers();
                    return;
                }

                //Check installed driver double
                if (!CheckDriversDouble())
                {
                    await Message_DoubleDrivers();
                    return;
                }

                //Check installed driver versions
                if (!CheckDriversVersion())
                {
                    await Message_UpdateDrivers();
                    return;
                }

                //Open the virtual bus driver
                if (!await OpenVirtualBusDriver())
                {
                    await Message_InstallDrivers();
                    return;
                }

                //Open the hid hide device
                if (!OpenHidHideDevice())
                {
                    await Message_InstallDrivers();
                    return;
                }

                //Open the FakerInput device
                if (!OpenFakerInputDevice())
                {
                    await Message_InstallDrivers();
                    return;
                }

                //Check settings if CtrlUI launches on start
                if (vSettings.Load("LaunchCtrlUI", typeof(bool)))
                {
                    ProcessLaunch.LaunchCtrlUI(true);
                }

                //Check settings if FpsOverlayer launches on start
                if (vSettings.Load("LaunchFpsOverlayer", typeof(bool)))
                {
                    ProcessLaunch.LaunchFpsOverlayer(true);
                }

                //Check settings if ScreenCapy launches on start
                if (vSettings.Load("LaunchScreenCapy", typeof(bool)))
                {
                    ProcessLaunch.LaunchScreenCapy(true);
                }

                //Load the help text
                LoadHelp();

                //Register Interface Handlers
                RegisterInterfaceHandlers();

                //Load combo box values
                ComboBox_MapKeypad_Load();

                //Close running controller tools
                CloseControllerTools();

                //Update keypad interface
                UpdateKeypadInterface();

                //Bind all the lists to ListBox
                ListBoxBindLists();

                //Reset HidHide to defaults
                vHidHideDevice.ListDeviceReset();
                vHidHideDevice.ListApplicationReset();

                //Allow DirectXInput in HidHide
                vHidHideDevice.ListApplicationAdd(AVFunctions.ApplicationPathExecutable());

                //Enable HidHide device
                vHidHideDevice.DeviceHideToggle(true);

                //Start the background tasks
                TasksBackgroundStart();

                //Register keyboard hotkeys
                AVInputOutputHotkeyHook.Start();
                AVInputOutputHotkeyHook.EventHotkeyPressedList += EventHotkeyPressed;

                //Set application first launch to false
                vSettings.Set("AppFirstLaunch", "False");

                //Enable the socket server
                await EnableSocketServer();
            }
            catch { }
        }
    }
}
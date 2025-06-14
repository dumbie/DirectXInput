﻿using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVInputOutputClass;
using static ArnoldVinkCode.AVSettings;
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

                //Check for available application update
                await UpdateCheck("dumbie", "DirectXInput", true);

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
                if (SettingLoad(vConfigurationDirectXInput, "AppFirstLaunch", typeof(bool)))
                {
                    Debug.WriteLine("First launch showing the window.");
                    Application_ShowHideWindow();
                }

                //Check if drivers are installed
                if (!CheckInstalledDrivers())
                {
                    if (!ShowInTaskbar) { Application_ShowHideWindow(); }
                    await Message_InstallDrivers();
                    return;
                }

                //Check installed driver double
                if (!CheckDriversDouble())
                {
                    if (!ShowInTaskbar) { Application_ShowHideWindow(); }
                    await Message_DoubleDrivers();
                    return;
                }

                //Check installed driver versions
                if (!CheckDriversVersion())
                {
                    if (!ShowInTaskbar) { Application_ShowHideWindow(); }
                    await Message_UpdateDrivers();
                    return;
                }

                //Open the virtual bus driver
                if (!await OpenVirtualBusDriver())
                {
                    if (!ShowInTaskbar) { Application_ShowHideWindow(); }
                    await Message_InstallDrivers();
                    return;
                }

                //Open the hid hide device
                if (!OpenHidHideDevice())
                {
                    if (!ShowInTaskbar) { Application_ShowHideWindow(); }
                    await Message_InstallDrivers();
                    return;
                }

                //Open the FakerInput device
                if (!OpenFakerInputDevice())
                {
                    if (!ShowInTaskbar) { Application_ShowHideWindow(); }
                    await Message_InstallDrivers();
                    return;
                }

                //Check settings if Screen Capture Tool launches on start
                bool shortcutCaptureImage = vShortcutsController.Any(x => x.Name == "CaptureImage" && !x.Trigger.All(x => x == ControllerButtons.None));
                bool shortcutCaptureVideo = vShortcutsController.Any(x => x.Name == "CaptureVideo" && !x.Trigger.All(x => x == ControllerButtons.None));
                if (shortcutCaptureImage || shortcutCaptureVideo)
                {
                    ProcessLaunch.LaunchScreenCaptureTool(true, true);
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
                SettingSave(vConfigurationDirectXInput, "AppFirstLaunch", "False");

                //Enable the socket server
                await EnableSocketServer();
            }
            catch { }
        }
    }
}
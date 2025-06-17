using System;
using System.Diagnostics;
using static ArnoldVinkCode.AVSettings;
using static DirectXInput.AppVariables;

namespace DirectXInput
{
    partial class WindowMain
    {
        //Check - Application Settings
        void Settings_Check()
        {
            try
            {
                if (!SettingCheck(vConfigurationDirectXInput, "AppFirstLaunch")) { SettingSave(vConfigurationDirectXInput, "AppFirstLaunch", "True"); }
                if (!SettingCheck(vConfigurationDirectXInput, "ExclusiveGuide")) { SettingSave(vConfigurationDirectXInput, "ExclusiveGuide", "True"); }

                //Server settings
                if (!SettingCheck(vConfigurationDirectXInput, "ServerPort")) { SettingSave(vConfigurationDirectXInput, "ServerPort", "26760"); }

                //Display settings
                if (!SettingCheck(vConfigurationDirectXInput, "DisplayMonitor")) { SettingSave(vConfigurationDirectXInput, "DisplayMonitor", "1"); }

                //Launch settings
                if (!SettingCheck(vConfigurationDirectXInput, "LaunchCtrlUI")) { SettingSave(vConfigurationDirectXInput, "LaunchCtrlUI", "False"); }
                if (!SettingCheck(vConfigurationDirectXInput, "LaunchFpsOverlayer")) { SettingSave(vConfigurationDirectXInput, "LaunchFpsOverlayer", "False"); }
                if (!SettingCheck(vConfigurationDirectXInput, "LaunchScreenCaptureTool")) { SettingSave(vConfigurationDirectXInput, "LaunchScreenCaptureTool", "True"); }

                //Sound settings
                if (!SettingCheck(vConfigurationDirectXInput, "InterfaceSound")) { SettingSave(vConfigurationDirectXInput, "InterfaceSound", "True"); }
                if (!SettingCheck(vConfigurationDirectXInput, "InterfaceSoundVolume")) { SettingSave(vConfigurationDirectXInput, "InterfaceSoundVolume", "75"); }
                if (!SettingCheck(vConfigurationDirectXInput, "InterfaceSoundPackName")) { SettingSave(vConfigurationDirectXInput, "InterfaceSoundPackName", "Default"); }

                //Battery settings
                if (!SettingCheck(vConfigurationDirectXInput, "BatteryLowLevel")) { SettingSave(vConfigurationDirectXInput, "BatteryLowLevel", "20"); }
                if (!SettingCheck(vConfigurationDirectXInput, "BatteryLowBlinkLed")) { SettingSave(vConfigurationDirectXInput, "BatteryLowBlinkLed", "True"); }
                if (!SettingCheck(vConfigurationDirectXInput, "BatteryLowShowNotification")) { SettingSave(vConfigurationDirectXInput, "BatteryLowShowNotification", "True"); }
                if (!SettingCheck(vConfigurationDirectXInput, "BatteryLowPlaySound")) { SettingSave(vConfigurationDirectXInput, "BatteryLowPlaySound", "True"); }

                //Controller settings
                if (!SettingCheck(vConfigurationDirectXInput, "ControllerIdleDisconnectMin")) { SettingSave(vConfigurationDirectXInput, "ControllerIdleDisconnectMin", "10"); }
                if (!SettingCheck(vConfigurationDirectXInput, "ControllerLedCondition")) { SettingSave(vConfigurationDirectXInput, "ControllerLedCondition", "0"); }
                if (!SettingCheck(vConfigurationDirectXInput, "ControllerColor0")) { SettingSave(vConfigurationDirectXInput, "ControllerColor0", "#00C7FF"); }
                if (!SettingCheck(vConfigurationDirectXInput, "ControllerColor1")) { SettingSave(vConfigurationDirectXInput, "ControllerColor1", "#F0140A"); }
                if (!SettingCheck(vConfigurationDirectXInput, "ControllerColor2")) { SettingSave(vConfigurationDirectXInput, "ControllerColor2", "#14F00A"); }
                if (!SettingCheck(vConfigurationDirectXInput, "ControllerColor3")) { SettingSave(vConfigurationDirectXInput, "ControllerColor3", "#F0DC0A"); }

                //Keyboard settings
                if (!SettingCheck(vConfigurationDirectXInput, "KeyboardLayout")) { SettingSave(vConfigurationDirectXInput, "KeyboardLayout", "0"); }
                if (!SettingCheck(vConfigurationDirectXInput, "KeyboardMode")) { SettingSave(vConfigurationDirectXInput, "KeyboardMode", "1"); }
                if (!SettingCheck(vConfigurationDirectXInput, "KeyboardResetPosition")) { SettingSave(vConfigurationDirectXInput, "KeyboardResetPosition", "False"); }
                if (!SettingCheck(vConfigurationDirectXInput, "KeyboardCloseNoController")) { SettingSave(vConfigurationDirectXInput, "KeyboardCloseNoController", "True"); }
                if (!SettingCheck(vConfigurationDirectXInput, "KeyboardMouseMoveSensitivity")) { SettingSave(vConfigurationDirectXInput, "KeyboardMouseMoveSensitivity", "7,50"); }
                if (!SettingCheck(vConfigurationDirectXInput, "KeyboardMouseScrollSensitivity2")) { SettingSave(vConfigurationDirectXInput, "KeyboardMouseScrollSensitivity2", "2"); }

                //Media settings
                if (!SettingCheck(vConfigurationDirectXInput, "MediaVolumeStep")) { SettingSave(vConfigurationDirectXInput, "MediaVolumeStep", "2"); }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check the application settings: " + ex.Message);
            }
        }
    }
}
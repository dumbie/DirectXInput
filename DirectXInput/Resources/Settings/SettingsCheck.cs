using System;
using System.Diagnostics;
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
                if (!vSettings.Check("AppFirstLaunch")) { vSettings.Set("AppFirstLaunch", "True"); }
                if (!vSettings.Check("ExclusiveGuide")) { vSettings.Set("ExclusiveGuide", "True"); }

                //Server settings
                if (!vSettings.Check("ServerPort")) { vSettings.Set("ServerPort", "26760"); }

                //Display settings
                if (!vSettings.Check("DisplayMonitor")) { vSettings.Set("DisplayMonitor", "1"); }

                //Launch settings
                if (!vSettings.Check("LaunchCtrlUI")) { vSettings.Set("LaunchCtrlUI", "False"); }
                if (!vSettings.Check("LaunchFpsOverlayer")) { vSettings.Set("LaunchFpsOverlayer", "False"); }
                if (!vSettings.Check("LaunchScreenCapy")) { vSettings.Set("LaunchScreenCapy", "True"); }

                //Sound settings
                if (!vSettings.Check("InterfaceSound")) { vSettings.Set("InterfaceSound", "True"); }
                if (!vSettings.Check("InterfaceSoundVolume")) { vSettings.Set("InterfaceSoundVolume", "75"); }
                if (!vSettings.Check("InterfaceSoundPackName")) { vSettings.Set("InterfaceSoundPackName", "Default"); }

                //Battery settings
                if (!vSettings.Check("BatteryLowLevel")) { vSettings.Set("BatteryLowLevel", "20"); }
                if (!vSettings.Check("BatteryLowBlinkLed")) { vSettings.Set("BatteryLowBlinkLed", "True"); }
                if (!vSettings.Check("BatteryLowShowNotification")) { vSettings.Set("BatteryLowShowNotification", "True"); }
                if (!vSettings.Check("BatteryLowPlaySound")) { vSettings.Set("BatteryLowPlaySound", "True"); }

                //Controller settings
                if (!vSettings.Check("ControllerIdleDisconnectMin")) { vSettings.Set("ControllerIdleDisconnectMin", "10"); }
                if (!vSettings.Check("ControllerLedCondition")) { vSettings.Set("ControllerLedCondition", "0"); }
                if (!vSettings.Check("ControllerColor0")) { vSettings.Set("ControllerColor0", "#00C7FF"); }
                if (!vSettings.Check("ControllerColor1")) { vSettings.Set("ControllerColor1", "#F0140A"); }
                if (!vSettings.Check("ControllerColor2")) { vSettings.Set("ControllerColor2", "#14F00A"); }
                if (!vSettings.Check("ControllerColor3")) { vSettings.Set("ControllerColor3", "#F0DC0A"); }

                //Keyboard settings
                if (!vSettings.Check("KeyboardLayout")) { vSettings.Set("KeyboardLayout", "0"); }
                if (!vSettings.Check("KeyboardMode")) { vSettings.Set("KeyboardMode", "1"); }
                if (!vSettings.Check("KeyboardResetPosition")) { vSettings.Set("KeyboardResetPosition", "False"); }
                if (!vSettings.Check("KeyboardCloseNoController")) { vSettings.Set("KeyboardCloseNoController", "True"); }
                if (!vSettings.Check("KeyboardMouseMoveSensitivity")) { vSettings.Set("KeyboardMouseMoveSensitivity", "7,50"); }
                if (!vSettings.Check("KeyboardMouseScrollSensitivity2")) { vSettings.Set("KeyboardMouseScrollSensitivity2", "2"); }

                //Media settings
                if (!vSettings.Check("MediaVolumeStep")) { vSettings.Set("MediaVolumeStep", "2"); }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check the application settings: " + ex.Message);
            }
        }
    }
}
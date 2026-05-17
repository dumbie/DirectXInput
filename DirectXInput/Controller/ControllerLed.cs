using System;
using System.Diagnostics;
using System.Windows.Media;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.Enums;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Controller get led color
        public static Color ControllerLedColorGet(int controllerId)
        {
            try
            {
                if (controllerId == 0)
                {
                    return vControllerColor0;
                }
                else if (controllerId == 1)
                {
                    return vControllerColor1;
                }
                else if (controllerId == 2)
                {
                    return vControllerColor2;
                }
                else if (controllerId == 3)
                {
                    return vControllerColor3;
                }
                else
                {
                    return Colors.White;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get controller led color: " + ex.Message);
                return Colors.White;
            }
        }

        //Controller update led color
        void ControllerLedColorUpdate(ControllerStatus Controller)
        {
            try
            {
                //Check if controller is connected
                if (!Controller.Connected())
                {
                    //Debug.WriteLine("Led update controller is not connected: " + Controller.NumberId);
                    return;
                }

                //Load battery settings
                bool batteryBlinkLedSetting = vSettings.Load("BatteryLowBlinkLed", typeof(bool));
                int batteryLowLevelSetting = vSettings.Load("BatteryLowLevel", typeof(int));

                //Check led battery blink and if battery is low
                if (batteryBlinkLedSetting)
                {
                    if (!Controller.ColorLedBlink && Controller.BatteryCurrent.BatteryPercentage <= batteryLowLevelSetting && Controller.BatteryCurrent.BatteryStatus == BatteryStatus.Normal)
                    {
                        Controller.ColorLedBlink = true;
                    }
                    else
                    {
                        Controller.ColorLedBlink = false;
                    }
                }
                else
                {
                    Controller.ColorLedBlink = false;
                }

                //Set controller led color
                if (Controller.ColorLedBlink)
                {
                    Controller.ColorLedCurrentBrightness = 0;
                    Controller.ColorLedCurrentR = 0;
                    Controller.ColorLedCurrentG = 0;
                    Controller.ColorLedCurrentB = 0;
                }
                else
                {
                    Color controllerColor = ControllerLedColorGet(Controller.NumberId);
                    double controllerLedBrightness = Convert.ToDouble(Controller.Details.Profile.LedBrightness) / 100;
                    Controller.ColorLedCurrentBrightness = Convert.ToByte(controllerLedBrightness * 255);
                    Controller.ColorLedCurrentR = Convert.ToByte(controllerColor.R * controllerLedBrightness);
                    Controller.ColorLedCurrentG = Convert.ToByte(controllerColor.G * controllerLedBrightness);
                    Controller.ColorLedCurrentB = Convert.ToByte(controllerColor.B * controllerLedBrightness);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update controller led color: " + ex.Message);
            }
        }
    }
}
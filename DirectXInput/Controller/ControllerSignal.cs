using System;
using System.Diagnostics;
using static ArnoldVinkCode.AVActions;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Send signal to controller
        void ControllerSignal(ControllerStatus Controller)
        {
            try
            {
                //Check if controller is connected
                if (!Controller.Connected())
                {
                    //Debug.WriteLine("Controller is not connected skipping signal: " + Controller.NumberId);
                    return;
                }

                //Check which controller is connected
                if (Controller.SupportedCurrent.CodeName == "SteamController2026")
                {
                    //Get current system ticks in milliseconds
                    long ticksSystem = GetSystemTicksMs();

                    //Steam Controller 2026 disable Lizard (Mouse + Keyboard) mode
                    //Note: Lizard mode automatically renables every 10 seconds so loop is needed to keep it disabled
                    if (ticksSystem - Controller.TicksSignalOne > 8000)
                    {
                        byte HEAD_FEATURE_REPORT = 0x01;
                        byte ID_SET_SETTINGS_VALUES = 0x87;
                        byte SETTING_LIZARD_MODE = 0x09;
                        byte LIZARD_MODE_OFF = 0x00;

                        byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                        outputReport[0] = HEAD_FEATURE_REPORT;
                        outputReport[1] = ID_SET_SETTINGS_VALUES;
                        outputReport[2] = 0x03;
                        outputReport[3] = SETTING_LIZARD_MODE;
                        outputReport[4] = LIZARD_MODE_OFF;
                        outputReport[5] = (byte)(LIZARD_MODE_OFF >> 8);

                        //Send data to the controller
                        bool bytesWritten = Controller.HidDevice.SetFeature(outputReport);
                        //Debug.WriteLine("Disabled Lizard mode controller: " + Controller.SupportedCurrent.CodeName + " / " + bytesWritten);

                        //Update signal ticks
                        Controller.TicksSignalOne = ticksSystem;
                    }

                    //Steam Controller 2026 led brightness
                    //Note: It takes a second for the controller to change led brightness, limit updates to 3 seconds to prevent overlap
                    if (ticksSystem - Controller.TicksSignalTwo > 3000)
                    {
                        byte HEAD_FEATURE_REPORT = 0x01;
                        byte ID_SET_SETTINGS_VALUES = 0x87;
                        //byte SETTING_LED_BASELINE_BRIGHTNESS = 0x2C;
                        byte SETTING_LED_USER_BRIGHTNESS = 0x2D;
                        byte LED_BRIGHTNESS = (byte)(Controller.ColorLedCurrentBrightness / 2);

                        byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                        outputReport[0] = HEAD_FEATURE_REPORT;
                        outputReport[1] = ID_SET_SETTINGS_VALUES;
                        outputReport[2] = 0x03;
                        outputReport[3] = SETTING_LED_USER_BRIGHTNESS;
                        outputReport[4] = LED_BRIGHTNESS;
                        outputReport[5] = (byte)(LED_BRIGHTNESS >> 8);

                        //Send data to the controller
                        bool bytesWritten = Controller.HidDevice.SetFeature(outputReport);
                        //Debug.WriteLine("Set led brightness controller: " + Controller.SupportedCurrent.CodeName + " / " + LED_BRIGHTNESS + " / " + bytesWritten);

                        //Update signal ticks
                        Controller.TicksSignalTwo = ticksSystem;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed sending controller signal: " + ex.Message);
            }
        }
    }
}
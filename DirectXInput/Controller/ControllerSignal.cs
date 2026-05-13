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
                //Check which controller is connected
                if (Controller.SupportedCurrent.CodeName == "SteamController2026")
                {
                    if ((GetSystemTicksMs() - Controller.TicksSignalLast) > 8000)
                    {
                        //Steam Controller 2026 disable Lizard (Mouse + Keyboard) mode
                        //Note: Lizard mode automatically renables every x seconds so loop is needed to keep it disabled
                        byte ID_SET_SETTINGS_VALUES = 0x87;
                        byte SETTING_LIZARD_MODE = 0x09;
                        byte LIZARD_MODE_OFF = 0x00;

                        byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                        outputReport[0] = 0x01;
                        outputReport[1] = ID_SET_SETTINGS_VALUES;
                        outputReport[2] = 0x03;
                        outputReport[3] = SETTING_LIZARD_MODE;
                        outputReport[4] = LIZARD_MODE_OFF;

                        //Send data to the controller
                        bool bytesWritten = Controller.HidDevice.SetFeature(outputReport);
                        //Debug.WriteLine("Disabled Lizard mode controller: " + Controller.SupportedCurrent.CodeName + " / " + bytesWritten);

                        //Update signal ticks
                        Controller.TicksSignalLast = GetSystemTicksMs();
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
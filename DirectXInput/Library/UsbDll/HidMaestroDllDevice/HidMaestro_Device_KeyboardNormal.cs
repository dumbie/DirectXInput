using HIDMaestro;
using System;
using System.Diagnostics;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVInputOutputClass;

namespace LibraryUsb
{
    public partial class HidMaestroDllDevice
    {
        public HMController KeyboardNormalCreate()
        {
            try
            {
                //Get device profile
                HMProfile hmProfile = hidMaestroContext.GetProfile("keyboard-normal");
                if (hmProfile == null)
                {
                    Debug.WriteLine("Keyboard device profile not found.");
                    return null;
                }

                //Create device controller
                HMController hmController = hidMaestroContext.CreateController(hmProfile);
                if (hmController == null)
                {
                    Debug.WriteLine("Failed to create Keyboard device.");
                    return null;
                }

                //Return result
                Debug.WriteLine("Created Keyboard device");
                return hmController;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to create Keyboard device: " + ex.Message);
                return null;
            }
        }

        public bool KeyboardNormalPressRelease(HMController hmController, KeysHidAction keyboardAction)
        {
            try
            {
                KeyboardNormalSetInput(hmController, keyboardAction);
                AVHighResDelay.Delay(50);
                KeyboardNormalResetInput(hmController);
                return true;
            }
            catch
            {
                Debug.WriteLine("Failed to press and release Keyboard keys.");
                return false;
            }
        }

        public bool KeyboardNormalSetInput(HMController hmController, KeysHidAction keyboardAction)
        {
            try
            {
                //Keyboard input report
                byte[] inputReport = new byte[8];
                inputReport[0] = (byte)keyboardAction.Modifiers;
                inputReport[2] = (byte)keyboardAction.Key0;
                inputReport[3] = (byte)keyboardAction.Key1;
                inputReport[4] = (byte)keyboardAction.Key2;
                inputReport[5] = (byte)keyboardAction.Key3;
                inputReport[6] = (byte)keyboardAction.Key4;
                inputReport[7] = (byte)keyboardAction.Key5;

                //Submit raw report
                hmController.SubmitRawReport(inputReport);

                //Return result
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to set Keyboard input: " + ex.Message);
                return false;
            }
        }

        public bool KeyboardNormalResetInput(HMController hmController)
        {
            try
            {
                //Keyboard input report
                byte[] inputReport = new byte[8];

                //Submit raw report
                hmController.SubmitRawReport(inputReport);

                //Return result
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to reset Keyboard input: " + ex.Message);
                return false;
            }
        }
    }
}
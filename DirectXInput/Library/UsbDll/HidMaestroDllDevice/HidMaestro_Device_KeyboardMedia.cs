using HIDMaestro;
using System;
using System.Diagnostics;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVInputOutputClass;

namespace LibraryUsb
{
    public partial class HidMaestroDllDevice
    {
        public HMController KeyboardMediaCreate()
        {
            try
            {
                //Get device profile
                HMProfile hmProfile = hidMaestroContext.GetProfile("keyboard-media");
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

        public bool KeyboardMediaPressRelease(HMController hmController, KeysMediaHidOne keyboardActionOne, KeysMediaHidTwo keyboardActionTwo, KeysMediaHidThree keyboardActionThree)
        {
            try
            {
                KeyboardMediaSetInput(hmController, keyboardActionOne, keyboardActionTwo, keyboardActionThree);
                AVHighResDelay.Delay(50);
                KeyboardMediaResetInput(hmController);
                return true;
            }
            catch
            {
                Debug.WriteLine("Failed to press and release Keyboard keys.");
                return false;
            }
        }

        public bool KeyboardMediaSetInput(HMController hmController, KeysMediaHidOne keyboardActionOne, KeysMediaHidTwo keyboardActionTwo, KeysMediaHidThree keyboardActionThree)
        {
            try
            {
                //Keyboard input report
                byte[] inputReport = new byte[8];
                inputReport[0] = (byte)keyboardActionOne;
                inputReport[1] = (byte)keyboardActionTwo;
                inputReport[2] = (byte)keyboardActionThree;

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

        public bool KeyboardMediaResetInput(HMController hmController)
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
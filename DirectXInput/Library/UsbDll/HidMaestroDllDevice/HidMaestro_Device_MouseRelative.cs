using HIDMaestro;
using System;
using System.Diagnostics;
using static ArnoldVinkCode.AVInputOutputClass;

namespace LibraryUsb
{
    public partial class HidMaestroDllDevice
    {
        public HMController MouseRelativeCreate()
        {
            try
            {
                //Get device profile
                HMProfile hmProfile = hidMaestroContext.GetProfile("mouse-relative");
                if (hmProfile == null)
                {
                    Debug.WriteLine("Mouse device profile not found.");
                    return null;
                }

                //Create device controller
                HMController hmController = hidMaestroContext.CreateController(hmProfile);
                if (hmController == null)
                {
                    Debug.WriteLine("Failed to create Mouse device.");
                    return null;
                }

                //Return result
                Debug.WriteLine("Created Mouse device");
                return hmController;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to create Mouse device: " + ex.Message);
                return null;
            }
        }

        public bool MouseRelativeSetInput(HMController hmController, MouseHidAction mouseAction)
        {
            try
            {
                //Mouse input report
                byte[] inputReport = new byte[8];
                inputReport[0] = (byte)mouseAction.Button; //Buttons
                inputReport[1] = (byte)(mouseAction.MoveHorizontal & 0xFF); //Horizontal movement
                inputReport[2] = (byte)((mouseAction.MoveHorizontal >> 8) & 0xFF); ; //Horizontal movement
                inputReport[3] = (byte)(mouseAction.MoveVertical & 0xFF); ; //Vertical movement
                inputReport[4] = (byte)((mouseAction.MoveVertical >> 8) & 0xFF); ; //Vertical movement
                inputReport[5] = (byte)mouseAction.ScrollVertical; //Scroll Vertical
                inputReport[6] = (byte)mouseAction.ScrollHorizontal; //Scroll Horizontal

                //Submit raw report
                hmController.SubmitRawReport(inputReport);

                //Return result
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to set Mouse input: " + ex.Message);
                return false;
            }
        }

        public bool MouseRelativeResetInput(HMController hmController)
        {
            try
            {
                //Mouse input report
                byte[] inputReport = new byte[8];

                //Submit raw report
                hmController.SubmitRawReport(inputReport);

                //Return result
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to reset Mouse input: " + ex.Message);
                return false;
            }
        }
    }
}
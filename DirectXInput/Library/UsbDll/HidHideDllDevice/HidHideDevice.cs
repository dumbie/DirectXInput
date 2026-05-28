using Nefarius.Drivers.HidHide;
using System;
using System.Diagnostics;

namespace LibraryUsb
{
    public partial class HidHideDllDevice
    {
        public bool Connected;
        public HidHideControlService Control = null;

        public HidHideDllDevice()
        {
            try
            {
                //Create HidHide Control Service
                Control = new HidHideControlService();

                //Update variables
                Connected = true;
                Debug.WriteLine("HidHide device created.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed creating HidHide device: " + ex.Message);
            }
        }

        public void CloseDevice()
        {
            try
            {
                if (Control != null)
                {
                    //Update variables
                    Control = null;
                    Connected = false;
                    Debug.WriteLine("Closed HidHide device");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed closing HidHide device: " + ex.Message);
            }
        }
    }
}
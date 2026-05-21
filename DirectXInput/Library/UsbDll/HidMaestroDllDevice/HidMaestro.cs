using HIDMaestro;
using System;
using System.Diagnostics;

namespace LibraryUsb
{
    public partial class HidMaestroDllDevice
    {
        public bool Connected;
        public HMContext hidMaestroContext = null;

        public HidMaestroDllDevice()
        {
            try
            {
                //Create HID Maestro context
                hidMaestroContext = new HMContext();

                //Install HID Maestro driver
                hidMaestroContext.InstallDriver();

                //Load device profiles
                int loadedDefaultProfiles = hidMaestroContext.LoadDefaultProfiles();
                int loadedCustomProfiles = hidMaestroContext.LoadProfilesFromDirectory("Profiles\\HidMaestro");

                Debug.WriteLine("HidMaestro device created: " + loadedDefaultProfiles + " / " + loadedCustomProfiles);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed creating HidMaestro device: " + ex.Message);
            }
        }

        public void CloseDevice()
        {
            try
            {
                if (hidMaestroContext != null)
                {
                    hidMaestroContext.Dispose();
                }

                Debug.WriteLine("Closed HidMaestro device.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed closing HidMaestro device: " + ex.Message);
            }
        }
    }
}
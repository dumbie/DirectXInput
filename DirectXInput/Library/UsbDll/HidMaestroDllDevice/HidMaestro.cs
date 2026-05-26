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
                //Create HIDMaestro context
                hidMaestroContext = new HMContext();

                //Install drivers, certificate and remove ghost devices
                hidMaestroContext.InstallDriver();

                //Load device profiles
                int loadedCustomProfiles = hidMaestroContext.LoadProfilesFromDirectory("Profiles\\HidMaestro");

                Connected = true;
                Debug.WriteLine("HidMaestro device created: " + loadedCustomProfiles);
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

                Connected = false;
                Debug.WriteLine("Closed HidMaestro device.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed closing HidMaestro device: " + ex.Message);
            }
        }
    }
}
using System;
using System.Diagnostics;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Returns if a controller is connected
        public static bool ControllerAnyConnected()
        {
            //Debug.WriteLine("Controller connected: 0" + vController0.Connected() + "/1" + vController1.Connected() + "/2" + vController2.Connected() + "/3" + vController3.Connected());
            return vController0.Connected() || vController1.Connected() || vController2.Connected() || vController3.Connected();
        }

        //Returns if a controller is disconnecting
        public static bool ControllerAnyDisconnecting()
        {
            //Debug.WriteLine("Controller disconnecting: 0" + vController0.Disconnecting + "/1" + vController1.Disconnecting + "/2" + vController2.Disconnecting + "/3" + vController3.Disconnecting);
            return vController0.Disconnecting || vController1.Disconnecting || vController2.Disconnecting || vController3.Disconnecting;
        }

        //Returns active controller status
        public static ControllerStatus ControllerGetActive()
        {
            try
            {
                if (vController0.Activated) { return vController0; }
                else if (vController1.Activated) { return vController1; }
                else if (vController2.Activated) { return vController2; }
                else if (vController3.Activated) { return vController3; }
            }
            catch { }
            return null;
        }

        //Reset controller status to defaults
        public static void ControllerResetStatus(int controllerId)
        {
            try
            {
                if (controllerId == 0)
                {
                    vController0 = new ControllerStatus(0);
                }
                else if (controllerId == 1)
                {
                    vController1 = new ControllerStatus(1);
                }
                else if (controllerId == 2)
                {
                    vController2 = new ControllerStatus(2);
                }
                else if (controllerId == 3)
                {
                    vController3 = new ControllerStatus(3);
                }

                //Debug.WriteLine("Reset controller status: " + controllerId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed resetting controller status: " + controllerId + " / " + ex.Message);
            }
        }
    }
}
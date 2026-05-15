using System;
using System.Diagnostics;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Controller close handle
        void ControllerHandleClose(ControllerStatus controller)
        {
            try
            {
                //Close Hid or WinUsb device
                if (controller.WinUsbDevice != null)
                {
                    controller.WinUsbDevice.CloseDevice();
                }
                else if (controller.HidDevice != null)
                {
                    controller.HidDevice.CloseDevice();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to close controller handle: " + ex.Message);
            }
        }
    }
}
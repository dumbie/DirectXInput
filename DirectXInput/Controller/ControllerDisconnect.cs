using System;
using System.Diagnostics;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Controller close handle
        void ControllerDisconnectClose(ControllerStatus controller)
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

        //Controller disconnect wireless
        void ControllerDisconnectWireless(ControllerStatus controller)
        {
            try
            {
                if (controller.SupportedCurrent.CodeName == "SteamController2026")
                {
                    byte ID_TURN_OFF_CONTROLLER = 0x9F;
                    byte[] featureData = new byte[controller.ControllerDataOutput.Length];
                    featureData[0] = 0x01;
                    featureData[1] = ID_TURN_OFF_CONTROLLER;
                    controller.HidDevice.SetFeature(featureData);
                }
                else if (controller.Details.IsBluetooth)
                {
                    //Disconnect controller from bluetooth
                    controller.HidDevice.BluetoothDisconnect();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to disconnect wireless controller: " + ex.Message);
            }
        }
    }
}
using System;
using System.Diagnostics;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Controller disconnect wireless
        void ControllerDisconnectWireless(ControllerStatus controller)
        {
            try
            {
                if (controller.SupportedCurrent.CodeName == "SteamController2026")
                {
                    byte HEAD_FEATURE_REPORT = 0x01;
                    byte ID_TURN_OFF_CONTROLLER = 0x9F;
                    byte[] featureData = new byte[controller.ControllerDataOutput.Length];
                    featureData[0] = HEAD_FEATURE_REPORT;
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
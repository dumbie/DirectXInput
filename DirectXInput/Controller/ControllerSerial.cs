using System;
using System.Diagnostics;
using static LibraryShared.Classes;
using static LibraryShared.Enums;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Check and set controller serial number
        void ControllerReadSerialNumber(ControllerStatus controller)
        {
            try
            {
                if (controller.Details.Type == ControllerType.HidDevice && string.IsNullOrWhiteSpace(controller.HidDevice.Attributes.SerialNumber))
                {
                    Debug.WriteLine("Serial number is missing for controller: " + controller.SupportedCurrent.CodeName + " / " + controller.Details.DisplayName);

                    //Get serial number feature
                    byte[] dataFeature = null;
                    if (controller.SupportedCurrent.CodeName == "SonyPS5DualSense")
                    {
                        dataFeature = controller.HidDevice.GetFeature(0x09);
                    }
                    else if (controller.SupportedCurrent.CodeName == "SonyPS4DualShock")
                    {
                        dataFeature = controller.HidDevice.GetFeature(0x12);
                    }
                    else if (controller.SupportedCurrent.CodeName == "SteamController2026")
                    {
                        //Note: Steam Controller uses a custom serial number format with 13 characters not usable to disconnect bluetooth
                        //byte HEAD_FEATURE_REPORT = 0x01;
                        //byte ID_GET_STRING_ATTRIBUTE = 0xAE;
                        //byte STEAM_SERIAL_LENGTH = 0x01; //0x01, 0x14, 0x15
                        //byte ATTRIB_STR_BOARD_SERIAL = 0x00;
                        //byte ATTRIB_STR_UNIT_SERIAL = 0x01;
                        //int STEAM_SERIAL_ARRAY_LENGTH = 25;

                        //byte[] sendData = new byte[STEAM_SERIAL_ARRAY_LENGTH];
                        //sendData[0] = HEAD_FEATURE_REPORT;
                        //sendData[1] = ID_GET_STRING_ATTRIBUTE;
                        //sendData[2] = STEAM_SERIAL_LENGTH;
                        //sendData[3] = ATTRIB_STR_UNIT_SERIAL;
                        //dataFeature = controller.HidDevice.GetFeature(ref sendData);
                    }

                    //Check data feature
                    if (dataFeature != null)
                    {
                        string serialNumberFeature = dataFeature[6].ToString("X2") + dataFeature[5].ToString("X2") + dataFeature[4].ToString("X2") + dataFeature[3].ToString("X2") + dataFeature[2].ToString("X2") + dataFeature[1].ToString("X2");
                        if (!string.IsNullOrWhiteSpace(serialNumberFeature))
                        {
                            controller.HidDevice.Attributes.SerialNumber = serialNumberFeature.ToUpper();
                            Debug.WriteLine("Got serial number from feature: " + controller.HidDevice.Attributes.SerialNumber);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to read serial number: " + ex.Message);
            }
        }
    }
}
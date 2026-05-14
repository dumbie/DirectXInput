using System;
using System.Diagnostics;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        private static bool InputUpdateTouchpad(ControllerStatus controller)
        {
            try
            {
                //X horizontal range is 0 to 1920 used by DSU
                //Y vertical range is 0 to 942 used by DSU

                //Touchpad One
                if (controller.SupportedCurrent.OffsetHeader.TouchpadOne != null)
                {
                    //Set controller header offset
                    int headerOffset = controller.Details.Wireless ? controller.SupportedCurrent.OffsetWireless : controller.SupportedCurrent.OffsetWired;

                    byte touchByte0 = controller.ControllerDataInput[headerOffset + (int)controller.SupportedCurrent.OffsetHeader.TouchpadOne];
                    byte touchByte1 = controller.ControllerDataInput[headerOffset + (int)controller.SupportedCurrent.OffsetHeader.TouchpadOne + 1];
                    byte touchByte2 = controller.ControllerDataInput[headerOffset + (int)controller.SupportedCurrent.OffsetHeader.TouchpadOne + 2];
                    byte touchByte3 = controller.ControllerDataInput[headerOffset + (int)controller.SupportedCurrent.OffsetHeader.TouchpadOne + 3];

                    if (controller.SupportedCurrent.CodeName == "SonyPS4DualShock" || controller.SupportedCurrent.CodeName == "SonyPS5DualSense")
                    {
                        if ((touchByte0 & 0x80) == 0)
                        {
                            controller.InputCurrent.TouchpadOneActive = 1;
                        }
                        else
                        {
                            controller.InputCurrent.TouchpadOneActive = 0;
                        }
                        controller.InputCurrent.TouchpadOneIdentifier = (byte)(touchByte0 & 0x7F);
                        controller.InputCurrent.TouchpadOneX = ((ushort)(touchByte2 & 0x0F) << 8) | touchByte1;
                        controller.InputCurrent.TouchpadOneY = (touchByte3 << 4) | ((ushort)(touchByte2 & 0xF0) >> 4);
                    }
                    else if (controller.SupportedCurrent.CodeName == "SteamController2026")
                    {
                        //Get and convert X to DSU range
                        if (touchByte1 > 0)
                        {
                            int touchinput = (touchByte1 << 8) | touchByte0;
                            int converted = (int)((((touchinput + 32768) % 65536) / 65535F) * 1920);
                            controller.InputCurrent.TouchpadOneX = converted;
                        }

                        //Get and convert Y to DSU range
                        if (touchByte3 > 0)
                        {
                            int touchinput = (touchByte3 << 8) | touchByte2;
                            int converted = (int)((1F - (((touchinput + 32768) % 65536) / 65535F)) * 942);
                            controller.InputCurrent.TouchpadOneY = converted;
                        }

                        controller.InputCurrent.TouchpadOneActive = (controller.InputCurrent.TouchpadOneX != 0 && controller.InputCurrent.TouchpadOneY != 0) ? (byte)1 : (byte)0;
                        controller.InputCurrent.TouchpadOneIdentifier = (byte)vRandom.Next(0, 255);
                    }
                }

                //Touchpad Two
                if (controller.SupportedCurrent.OffsetHeader.TouchpadTwo != null)
                {
                    //Set controller header offset
                    int headerOffset = controller.Details.Wireless ? controller.SupportedCurrent.OffsetWireless : controller.SupportedCurrent.OffsetWired;

                    byte touchByte0 = controller.ControllerDataInput[headerOffset + (int)controller.SupportedCurrent.OffsetHeader.TouchpadTwo];
                    byte touchByte1 = controller.ControllerDataInput[headerOffset + (int)controller.SupportedCurrent.OffsetHeader.TouchpadTwo + 1];
                    byte touchByte2 = controller.ControllerDataInput[headerOffset + (int)controller.SupportedCurrent.OffsetHeader.TouchpadTwo + 2];
                    byte touchByte3 = controller.ControllerDataInput[headerOffset + (int)controller.SupportedCurrent.OffsetHeader.TouchpadTwo + 3];

                    if (controller.SupportedCurrent.CodeName == "SonyPS4DualShock" || controller.SupportedCurrent.CodeName == "SonyPS5DualSense")
                    {
                        if ((touchByte0 & 0x80) == 0)
                        {
                            controller.InputCurrent.TouchpadTwoActive = 1;
                        }
                        else
                        {
                            controller.InputCurrent.TouchpadTwoActive = 0;
                        }
                        controller.InputCurrent.TouchpadTwoIdentifier = (byte)(touchByte0 & 0x7F);
                        controller.InputCurrent.TouchpadTwoX = ((ushort)(touchByte2 & 0x0F) << 8) | touchByte1;
                        controller.InputCurrent.TouchpadTwoY = (touchByte3 << 4) | ((ushort)(touchByte2 & 0xF0) >> 4);
                    }
                    else if (controller.SupportedCurrent.CodeName == "SteamController2026")
                    {
                        //Get and convert X to DSU range
                        if (touchByte1 > 0)
                        {
                            int touchinput = (touchByte1 << 8) | touchByte0;
                            int converted = (int)((((touchinput + 32768) % 65536) / 65535F) * 1920);
                            controller.InputCurrent.TouchpadTwoX = converted;
                        }

                        //Get and convert Y to DSU range
                        if (touchByte3 > 0)
                        {
                            int touchinput = (touchByte3 << 8) | touchByte2;
                            int converted = (int)((1F - (((touchinput + 32768) % 65536) / 65535F)) * 942);
                            controller.InputCurrent.TouchpadTwoY = converted;
                        }

                        controller.InputCurrent.TouchpadTwoActive = (controller.InputCurrent.TouchpadTwoX != 0 && controller.InputCurrent.TouchpadTwoY != 0) ? (byte)1 : (byte)0;
                        controller.InputCurrent.TouchpadTwoIdentifier = (byte)vRandom.Next(0, 255);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update touchpad input: " + ex.Message);
                return false;
            }
        }
    }
}
﻿using System;
using System.Diagnostics;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Send controller output
        public void ControllerOutputSend(ControllerStatus Controller)
        {
            try
            {
                //Check if controller is connected
                if (!Controller.Connected())
                {
                    //Debug.WriteLine("Rumble controller is not connected: " + Controller.NumberId);
                    return;
                }

                //Read the rumble strength
                byte controllerRumbleMode = 0;
                byte controllerRumbleHeavy = Controller.RumbleCurrentHeavy;
                byte controllerRumbleLight = Controller.RumbleCurrentLight;

                //Adjust the trigger rumble strength
                byte triggerRumbleMinimum = 5;
                byte triggerRumbleLeft = 0;
                byte triggerRumbleRight = 0;
                if (Controller.Details.Profile.TriggerRumbleEnabled)
                {
                    double triggerRumbleStrengthLeft = Convert.ToDouble(Controller.Details.Profile.TriggerRumbleStrengthLeft) / 100;
                    double triggerRumbleStrengthRight = Convert.ToDouble(Controller.Details.Profile.TriggerRumbleStrengthRight) / 100;
                    byte triggerRumbleLimit = Convert.ToByte(Convert.ToDouble(Controller.Details.Profile.TriggerRumbleLimit) / 100 * 255);

                    triggerRumbleLeft = Convert.ToByte(Math.Max(controllerRumbleLight, controllerRumbleHeavy) * triggerRumbleStrengthLeft);
                    if (triggerRumbleLeft > triggerRumbleLimit) { triggerRumbleLeft = triggerRumbleLimit; }
                    Debug.WriteLine("Trigger rumble Left: " + triggerRumbleLeft + " / Limit: " + triggerRumbleLimit + " / Minimum: " + triggerRumbleMinimum);

                    triggerRumbleRight = Convert.ToByte(Math.Max(controllerRumbleLight, controllerRumbleHeavy) * triggerRumbleStrengthRight);
                    if (triggerRumbleRight > triggerRumbleLimit) { triggerRumbleRight = triggerRumbleLimit; }
                    Debug.WriteLine("Trigger rumble Right: " + triggerRumbleRight + " / Limit: " + triggerRumbleLimit + " / Minimum: " + triggerRumbleMinimum);
                }

                //Adjust the controller rumble strength
                if (Controller.Details.Profile.ControllerRumbleEnabled)
                {
                    double controllerRumbleStrength = Convert.ToDouble(Controller.Details.Profile.ControllerRumbleStrength) / 100;
                    byte controllerRumbleLimit = Convert.ToByte(Convert.ToDouble(Controller.Details.Profile.ControllerRumbleLimit) / 100 * 255);

                    controllerRumbleHeavy = Convert.ToByte(controllerRumbleHeavy * controllerRumbleStrength);
                    if (controllerRumbleHeavy > controllerRumbleLimit) { controllerRumbleHeavy = controllerRumbleLimit; }

                    controllerRumbleLight = Convert.ToByte(controllerRumbleLight * controllerRumbleStrength);
                    if (controllerRumbleLight > controllerRumbleLimit) { controllerRumbleLight = controllerRumbleLimit; }

                    if (Controller.Details.Profile.ControllerRumbleMode == 1)
                    {
                        controllerRumbleMode = 0x01; //90%
                    }
                    else if (Controller.Details.Profile.ControllerRumbleMode == 2)
                    {
                        controllerRumbleMode = 0x02; //80%
                    }
                    else if (Controller.Details.Profile.ControllerRumbleMode == 3)
                    {
                        controllerRumbleMode = 0x03; //70%
                    }
                    else if (Controller.Details.Profile.ControllerRumbleMode == 4)
                    {
                        controllerRumbleMode = 0x04; //60%
                    }

                    Debug.WriteLine("Controller rumble Light: " + controllerRumbleLight + " / Heavy: " + controllerRumbleHeavy + " / Limit: " + controllerRumbleLimit + " / Mode: " + controllerRumbleMode);
                }
                else
                {
                    controllerRumbleHeavy = 0;
                    controllerRumbleLight = 0;
                }

                //Check which controller is connected
                if (Controller.SupportedCurrent.CodeName == "MicrosoftXboxOneS" && Controller.Details.Wireless)
                {
                    //Bluetooth Output - MicrosoftXboxOneS
                    byte[] outputReport = new byte[9];
                    outputReport[0] = 0x03; //Report identifier
                    outputReport[1] = 0x0F; //Rumble mode
                    outputReport[2] = 0x00; //Left trigger
                    outputReport[3] = 0x00; //Right trigger
                    outputReport[4] = controllerRumbleHeavy; //Heavy rumble
                    outputReport[5] = controllerRumbleLight; //Light rumble
                    outputReport[6] = 0xFF;
                    outputReport[7] = 0x00;
                    outputReport[8] = 0x01;

                    //Send data to the controller
                    bool bytesWritten = Controller.HidDevice.WriteBytesFile(outputReport);
                    Debug.WriteLine("BlueRumb MicrosoftXboxOneS: " + bytesWritten);
                }
                else if (Controller.SupportedCurrent.CodeName == "SonyPS5DualSense" && Controller.Details.Wireless)
                {
                    //Bluetooth Output - SonyPS5DualSense
                    byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                    outputReport[0] = 0xA2;
                    outputReport[1] = 0x31;
                    outputReport[2] = 0x02;
                    outputReport[3] = 0xFF;
                    outputReport[4] = 0xF7;

                    //Controller rumble strength
                    outputReport[5] = controllerRumbleLight;
                    outputReport[6] = controllerRumbleHeavy;

                    //Controller rumble mode
                    outputReport[39] = controllerRumbleMode;

                    //Trigger rumble
                    if (triggerRumbleRight >= triggerRumbleMinimum)
                    {
                        outputReport[13] = 0x01; //Right trigger
                        outputReport[14] = 0x00; //Begin;
                        outputReport[15] = triggerRumbleRight; //Force
                    }
                    else
                    {
                        outputReport[13] = 0x01; //Right trigger
                        outputReport[14] = 0xFF; //Begin;
                        outputReport[15] = 0x00; //Force
                    }
                    if (triggerRumbleLeft >= triggerRumbleMinimum)
                    {
                        outputReport[24] = 0x01; //Left trigger
                        outputReport[25] = 0x00; //Begin;
                        outputReport[26] = triggerRumbleLeft; //Force
                    }
                    else
                    {
                        outputReport[24] = 0x01; //Left trigger
                        outputReport[25] = 0xFF; //Begin;
                        outputReport[26] = 0x00; //Force
                    }

                    //If volume is muted turn on mute led
                    if (vControllerMuteLedCurrent)
                    {
                        outputReport[11] = 0x01;
                    }

                    //Set controller player led
                    if (Controller.Details.Profile.PlayerLedEnabled)
                    {
                        switch (Controller.NumberId)
                        {
                            case 0: { outputReport[46] = 0x04; break; }
                            case 1: { outputReport[46] = 0x02 | 0x08; break; }
                            case 2: { outputReport[46] = 0x01 | 0x04 | 0x10; break; }
                            case 3: { outputReport[46] = 0x01 | 0x02 | 0x08 | 0x10; break; }
                        }
                    }

                    //Set controller led color
                    outputReport[47] = Controller.ColorLedCurrentR;
                    outputReport[48] = Controller.ColorLedCurrentG;
                    outputReport[49] = Controller.ColorLedCurrentB;

                    //Replace CRC32 in bytes array
                    ByteArrayCRC32Replace(ref outputReport, 0, 1, 74);

                    //Send data to the controller
                    bool bytesWritten = Controller.HidDevice.WriteBytesFile(outputReport);
                    Debug.WriteLine("BlueRumb SonyPS5DualSense: " + bytesWritten);
                }
                else if (Controller.SupportedCurrent.CodeName == "SonyPS5DualSense" && !Controller.Details.Wireless)
                {
                    //Wired Output - SonyPS5DualSense
                    byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                    outputReport[0] = 0x02;
                    outputReport[1] = 0xFF;
                    outputReport[2] = 0xF7;

                    //Controller rumble strength
                    outputReport[3] = controllerRumbleLight;
                    outputReport[4] = controllerRumbleHeavy;

                    //Controller rumble mode
                    outputReport[37] = controllerRumbleMode;

                    //Trigger rumble
                    if (triggerRumbleRight >= triggerRumbleMinimum)
                    {
                        outputReport[11] = 0x01; //Right trigger
                        outputReport[12] = 0x00; //Begin;
                        outputReport[13] = triggerRumbleRight; //Force
                    }
                    else
                    {
                        outputReport[11] = 0x01; //Right trigger
                        outputReport[12] = 0xFF; //Begin;
                        outputReport[13] = 0x00; //Force
                    }
                    if (triggerRumbleLeft >= triggerRumbleMinimum)
                    {
                        outputReport[22] = 0x01; //Left trigger
                        outputReport[23] = 0x00; //Begin;
                        outputReport[24] = triggerRumbleLeft; //Force
                    }
                    else
                    {
                        outputReport[22] = 0x01; //Left trigger
                        outputReport[23] = 0xFF; //Begin;
                        outputReport[24] = 0x00; //Force
                    }

                    //If volume is muted turn on mute led
                    if (vControllerMuteLedCurrent)
                    {
                        outputReport[9] = 0x01;
                    }

                    //Set controller player led
                    if (Controller.Details.Profile.PlayerLedEnabled)
                    {
                        switch (Controller.NumberId)
                        {
                            case 0: { outputReport[44] = 0x04; break; }
                            case 1: { outputReport[44] = 0x02 | 0x08; break; }
                            case 2: { outputReport[44] = 0x01 | 0x04 | 0x10; break; }
                            case 3: { outputReport[44] = 0x01 | 0x02 | 0x08 | 0x10; break; }
                        }
                    }

                    //Set controller led color
                    outputReport[45] = Controller.ColorLedCurrentR;
                    outputReport[46] = Controller.ColorLedCurrentG;
                    outputReport[47] = Controller.ColorLedCurrentB;

                    //Send data to the controller
                    bool bytesWritten = Controller.HidDevice.WriteBytesFile(outputReport);
                    Debug.WriteLine("UsbRumb SonyPS5DualSense: " + bytesWritten);
                }
                else if (Controller.SupportedCurrent.CodeName == "SonyPS4DualShock" && Controller.Details.Wireless)
                {
                    //Bluetooth Output - SonyPS4DualShock
                    byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                    outputReport[0] = 0xA2;
                    outputReport[1] = 0x11;
                    outputReport[2] = 0xC0;
                    outputReport[4] = 0xFF;
                    outputReport[7] = controllerRumbleLight;
                    outputReport[8] = controllerRumbleHeavy;

                    //Set the controller led color
                    outputReport[9] = Controller.ColorLedCurrentR;
                    outputReport[10] = Controller.ColorLedCurrentG;
                    outputReport[11] = Controller.ColorLedCurrentB;

                    //Replace CRC32 in bytes array
                    ByteArrayCRC32Replace(ref outputReport, 0, 1, 74);

                    //Send data to the controller
                    bool bytesWritten = Controller.HidDevice.WriteBytesFile(outputReport);
                    Debug.WriteLine("BlueRumb SonyPS4DualShock: " + bytesWritten);
                }
                else if (Controller.SupportedCurrent.CodeName == "SonyPS4DualShock" && !Controller.Details.Wireless)
                {
                    //Wired Output - SonyPS4DualShock
                    byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                    outputReport[0] = 0x05;
                    outputReport[1] = 0xFF;
                    outputReport[4] = controllerRumbleLight;
                    outputReport[5] = controllerRumbleHeavy;

                    //Set the controller led color
                    outputReport[6] = Controller.ColorLedCurrentR;
                    outputReport[7] = Controller.ColorLedCurrentG;
                    outputReport[8] = Controller.ColorLedCurrentB;

                    //Send data to the controller
                    bool bytesWritten = Controller.HidDevice.WriteBytesFile(outputReport);
                    Debug.WriteLine("UsbRumb SonyPS4DualShock: " + bytesWritten);
                }
                else if (Controller.SupportedCurrent.CodeName == "SonyPS3DualShock")
                {
                    //Wired Output - SonyPS3DualShock
                    byte[] outputReport = new byte[30];
                    outputReport[1] = 0xFF;
                    outputReport[2] = (byte)(controllerRumbleLight > 0 ? 0x01 : 0x00); //On or Off
                    outputReport[3] = 0xFF;
                    outputReport[4] = controllerRumbleHeavy;
                    outputReport[10] = 0xFF;
                    outputReport[11] = 0x27;
                    outputReport[12] = 0x10;
                    outputReport[14] = 0x32;
                    outputReport[15] = 0xFF;
                    outputReport[16] = 0x27;
                    outputReport[17] = 0x10;
                    outputReport[19] = 0x32;
                    outputReport[20] = 0xFF;
                    outputReport[21] = 0x27;
                    outputReport[22] = 0x10;
                    outputReport[24] = 0x32;
                    outputReport[25] = 0xFF;
                    outputReport[26] = 0x27;
                    outputReport[27] = 0x10;
                    outputReport[29] = 0x32;

                    //Set controller player led
                    if (Controller.Details.Profile.PlayerLedEnabled)
                    {
                        switch (Controller.NumberId)
                        {
                            case 0: { outputReport[9] = 0x02; break; }
                            case 1: { outputReport[9] = 0x04; break; }
                            case 2: { outputReport[9] = 0x08; break; }
                            case 3: { outputReport[9] = 0x10; break; }
                        }
                    }

                    //Send data to the controller
                    bool bytesWritten = Controller.WinUsbDevice.WriteBytesTransfer(0x21, 0x09, 0x0201, outputReport);
                    Debug.WriteLine("UsbRumb SonyPS3DualShock: " + bytesWritten);
                }
                else if (Controller.SupportedCurrent.CodeName == "SonyPS12DualShock")
                {
                    //Wired Output - SonyPS12DualShock
                    byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                    outputReport[0] = (byte)Controller.NumberOutput;
                    outputReport[3] = (byte)(controllerRumbleHeavy / 2); //Between 0 and 127.5
                    outputReport[4] = (byte)(controllerRumbleLight > 0 ? 0x01 : 0x00); //On or Off

                    //Send data to the controller
                    bool bytesWritten = Controller.HidDevice.WriteBytesFile(outputReport);
                    Debug.WriteLine("UsbRumb SonyPS12DualShock: " + bytesWritten);
                }
                else if (Controller.SupportedCurrent.CodeName == "NintendoSwitchPro")
                {
                    //Fix test real controller 8BitDo only supports rumble on or off in switch mode

                    //Enable vibration
                    byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                    outputReport[0] = 0x10;
                    outputReport[1] = 0xFF;

                    //Heavy rumble
                    outputReport[2] = 0x74;
                    outputReport[3] = Math.Min(controllerRumbleHeavy, (byte)200);
                    outputReport[4] = 0x40;
                    outputReport[5] = 0x40;

                    //Light rumble
                    outputReport[6] = 0x74;
                    outputReport[7] = Math.Min(controllerRumbleLight, (byte)200);
                    outputReport[8] = 0x40;
                    outputReport[9] = 0x40;

                    //Send data to the controller
                    bool bytesWritten = Controller.HidDevice.WriteBytesFile(outputReport);
                    Debug.WriteLine("Rumble NintendoSwitchPro: " + bytesWritten);
                }
                else if (Controller.SupportedCurrent.CodeName == "8BitDoPro2" && !Controller.Details.Wireless)
                {
                    //Wired Output - 8BitDoPro2
                    byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                    outputReport[0] = 0x05;
                    outputReport[1] = (byte)(controllerRumbleHeavy / 2);
                    outputReport[2] = (byte)(controllerRumbleLight / 2);

                    ////Request debug information
                    //outputReport[0] = 0x81;
                    //outputReport[1] = 0x11;
                    //outputReport[2] = 0x04;
                    //outputReport[3] = 0x03;
                    //outputReport[5] = 0x15;

                    //Send data to the controller
                    bool bytesWritten = Controller.HidDevice.WriteBytesFile(outputReport);
                    Debug.WriteLine("UsbRumb 8BitDoPro2: " + bytesWritten);
                }
                else if (Controller.SupportedCurrent.CodeName == "8BitDoPro2" && Controller.Details.Wireless)
                {
                    //Bluetooth Output - 8BitDoPro2
                    byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                    outputReport[0] = 0x05;
                    outputReport[1] = (byte)(controllerRumbleHeavy / 2);
                    outputReport[2] = (byte)(controllerRumbleLight / 2);

                    //Send data to the controller
                    bool bytesWritten = Controller.HidDevice.WriteBytesFile(outputReport);
                    Debug.WriteLine("BlueRumb 8BitDoPro2: " + bytesWritten);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to output rumble: " + ex.Message);
            }
        }
    }
}
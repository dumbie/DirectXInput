using System;
using System.Diagnostics;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.Enums;

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

                //Adjust trigger rumble strength
                byte triggerRumbleLeft = 0x00;
                byte triggerRumbleRight = 0x00;
                if (Controller.Details.Profile.TriggerRumbleEnabled)
                {
                    double triggerRumbleStrengthLeft = Convert.ToDouble(Controller.Details.Profile.TriggerRumbleStrengthLeft) / 100;
                    double triggerRumbleStrengthRight = Convert.ToDouble(Controller.Details.Profile.TriggerRumbleStrengthRight) / 100;
                    byte triggerRumbleLimit = Convert.ToByte(Convert.ToDouble(Controller.Details.Profile.TriggerRumbleLimit) / 100 * 255);

                    //Generate custom trigger rumble
                    if (Controller.Details.Profile.TriggerRumbleGenerate)
                    {
                        byte controllerRumbleMax = Math.Max(Controller.RumbleCurrentControllerLight, Controller.RumbleCurrentControllerHeavy);
                        triggerRumbleLeft = Convert.ToByte(controllerRumbleMax * triggerRumbleStrengthLeft);
                        triggerRumbleRight = Convert.ToByte(controllerRumbleMax * triggerRumbleStrengthRight);
                    }
                    else
                    {
                        triggerRumbleLeft = Convert.ToByte(Controller.RumbleCurrentTriggerLeft * triggerRumbleStrengthLeft);
                        triggerRumbleRight = Convert.ToByte(Controller.RumbleCurrentTriggerRight * triggerRumbleStrengthRight);
                    }

                    //Check rumble limits
                    if (triggerRumbleLeft > triggerRumbleLimit) { triggerRumbleLeft = triggerRumbleLimit; }
                    if (triggerRumbleRight > triggerRumbleLimit) { triggerRumbleRight = triggerRumbleLimit; }

                    Debug.WriteLine("Trigger rumble Left: " + triggerRumbleLeft + " / Right: " + triggerRumbleRight + " / Limit: " + triggerRumbleLimit);
                }

                //Adjust controller rumble strength
                byte controllerRumbleHeavy = 0x00;
                byte controllerRumbleLight = 0x00;
                if (Controller.Details.Profile.ControllerRumbleEnabled)
                {
                    double controllerRumbleStrength = Convert.ToDouble(Controller.Details.Profile.ControllerRumbleStrength) / 100;
                    byte controllerRumbleLimit = Convert.ToByte(Convert.ToDouble(Controller.Details.Profile.ControllerRumbleLimit) / 100 * 255);

                    controllerRumbleHeavy = Convert.ToByte(Controller.RumbleCurrentControllerHeavy * controllerRumbleStrength);
                    controllerRumbleLight = Convert.ToByte(Controller.RumbleCurrentControllerLight * controllerRumbleStrength);

                    //Check rumble limits
                    if (controllerRumbleHeavy > controllerRumbleLimit) { controllerRumbleHeavy = controllerRumbleLimit; }
                    if (controllerRumbleLight > controllerRumbleLimit) { controllerRumbleLight = controllerRumbleLimit; }

                    Debug.WriteLine("Controller rumble Light: " + controllerRumbleLight + " / Heavy: " + controllerRumbleHeavy + " / Limit: " + controllerRumbleLimit);
                }

                //Check which controller is connected
                if (Controller.SupportedCurrent.CodeName == "MicrosoftXboxOneS" && Controller.Details.ConnectionType == ConnectionType.Bluetooth)
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
                else if (Controller.SupportedCurrent.CodeName == "SonyPS5DualSense" && Controller.Details.ConnectionType == ConnectionType.Bluetooth)
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

                    //Controller rumble power mode
                    outputReport[39] = ControllerBytesRumblePowerMode(Controller);

                    //Trigger Rumble Right
                    ControllerBytesRumbleTrigger(Controller, ref Controller.RumbleTicksTriggerRight, triggerRumbleRight).CopyTo(outputReport, 13);

                    //Trigger Rumble Left
                    ControllerBytesRumbleTrigger(Controller, ref Controller.RumbleTicksTriggerLeft, triggerRumbleLeft).CopyTo(outputReport, 24);

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
                else if (Controller.SupportedCurrent.CodeName == "SonyPS5DualSense" && Controller.Details.ConnectionType == ConnectionType.Wired)
                {
                    //Wired Output - SonyPS5DualSense
                    byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                    outputReport[0] = 0x02;
                    outputReport[1] = 0xFF;
                    outputReport[2] = 0xF7;

                    //Controller rumble strength
                    outputReport[3] = controllerRumbleLight;
                    outputReport[4] = controllerRumbleHeavy;

                    //Controller rumble power mode
                    outputReport[37] = ControllerBytesRumblePowerMode(Controller);

                    //Trigger Rumble Right
                    ControllerBytesRumbleTrigger(Controller, ref Controller.RumbleTicksTriggerRight, triggerRumbleRight).CopyTo(outputReport, 11);

                    //Trigger Rumble Left
                    ControllerBytesRumbleTrigger(Controller, ref Controller.RumbleTicksTriggerLeft, triggerRumbleLeft).CopyTo(outputReport, 22);

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
                else if (Controller.SupportedCurrent.CodeName == "SonyPS4DualShock" && Controller.Details.ConnectionType == ConnectionType.Bluetooth)
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
                else if (Controller.SupportedCurrent.CodeName == "SonyPS4DualShock" && Controller.Details.ConnectionType == ConnectionType.Wired)
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
                else if (Controller.SupportedCurrent.CodeName == "8BitDoPro2" && Controller.Details.ConnectionType == ConnectionType.Wired)
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
                else if (Controller.SupportedCurrent.CodeName == "8BitDoPro2" && Controller.Details.ConnectionType == ConnectionType.Bluetooth)
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
                else if (Controller.SupportedCurrent.CodeName == "SteamController2026")
                {
                    //Get controller rumble power mode
                    byte controllerRumblePower = ControllerBytesRumblePowerMode(Controller);

                    //Convert rumble to little endian
                    int rumbleLightEndian = (int)(controllerRumbleLight / 256F * 65535F);
                    byte rumbleLightLow = (byte)(rumbleLightEndian & 0xFF);
                    byte rumbleLightHigh = (byte)((rumbleLightEndian >> 8) & 0xFF);
                    byte rumbleLightGain = (byte)((float)(controllerRumbleLight / 255F) * controllerRumblePower);
                    int rumbleHeavyEndian = (int)(controllerRumbleHeavy / 256F * 65535F);
                    byte rumbleHeavyLow = (byte)(rumbleHeavyEndian & 0xFF);
                    byte rumbleHeavyHigh = (byte)((rumbleHeavyEndian >> 8) & 0xFF);
                    byte rumbleHeavyGain = (byte)((float)(controllerRumbleHeavy / 255F) * controllerRumblePower);

                    //Output Rumble - SteamController2026
                    byte ID_OUT_REPORT_HAPTIC_RUMBLE = 0x80;
                    byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                    outputReport[0] = ID_OUT_REPORT_HAPTIC_RUMBLE;
                    outputReport[1] = 0x01; //Type
                    outputReport[2] = 0x40; //Intensity dB
                    outputReport[3] = 0x1F; //Intensity dB
                    outputReport[4] = rumbleHeavyLow; //Left Speed
                    outputReport[5] = rumbleHeavyHigh; //Left Speed
                    outputReport[6] = rumbleHeavyGain; //Left Gain
                    outputReport[7] = rumbleLightLow; //Right Speed
                    outputReport[8] = rumbleLightHigh; //Right Speed
                    outputReport[9] = rumbleLightGain; //Right Gain

                    //Output Pulse - SteamController2026
                    //byte ID_OUT_REPORT_HAPTIC_PULSE = 0x81;
                    //byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                    //outputReport[0] = ID_OUT_REPORT_HAPTIC_PULSE;
                    //outputReport[2] = 0x08; //Intensity
                    //outputReport[3] = 0x20; //Intensity
                    //outputReport[6] = 0x01;
                    //81 00 90 01 00 00 01 00
                    //81 00 08 20 00 00 01 00

                    //Output Command - SteamController2026
                    //byte ID_OUT_REPORT_HAPTIC_COMMAND = 0x82;
                    //byte[] outputReport = new byte[Controller.ControllerDataOutput.Length];
                    //outputReport[0] = ID_OUT_REPORT_HAPTIC_COMMAND;
                    //outputReport[2] = 0x01;
                    //outputReport[3] = 0x01;
                    //outputReport[6] = 0x02;
                    //82 00 02 07 (Left test 12dB)
                    //82 01 02 07 (Right test 12dB)

                    //Send data to the controller
                    bool bytesWritten = Controller.HidDevice.WriteBytesFile(outputReport);
                    Debug.WriteLine("Rumble: " + Controller.SupportedCurrent.CodeName + " / " + bytesWritten);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to output rumble: " + ex.Message);
            }
        }
    }
}
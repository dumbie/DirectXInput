using System;
using System.Diagnostics;
using System.Linq;
using static LibraryShared.Classes;
using static LibraryShared.CRC32;
using static LibraryShared.Enums;

namespace DirectXInput
{
    partial class WindowMain
    {
        //Check controller input data type
        ControllerInputType InputDataCheckType(ControllerStatus controllerStatus)
        {
            try
            {
                //Debug.WriteLine(GenerateControllerDebugString(true, true));
                if (controllerStatus.SupportedCurrent.CodeName == "SteamController2026")
                {
                    //Steam Controller report types
                    byte ID_TRITON_CONTROLLER_STATE = 0x42;
                    byte ID_TRITON_BATTERY_STATUS = 0x43;
                    byte ID_TRITON_CONTROLLER_STATE_BLE = 0x45;
                    //byte ID_TRITON_WIRELESS_STATUS_X = 0x46;
                    //byte ID_TRITON_WIRELESS_STATUS = 0x79;
                    //byte ID_TRITON_WIRELESS_UNKNOWN = 0x7B;

                    //Check controller report mode
                    byte check0 = controllerStatus.ControllerDataInput[0];
                    if (check0 == ID_TRITON_CONTROLLER_STATE || check0 == ID_TRITON_CONTROLLER_STATE_BLE)
                    {
                        return ControllerInputType.Input;
                    }
                    else if (check0 == ID_TRITON_BATTERY_STATUS)
                    {
                        return ControllerInputType.Status;
                    }
                    else
                    {
                        return ControllerInputType.Unknown;
                    }
                }
                else if (controllerStatus.SupportedCurrent.CodeName == "NintendoSwitchPro")
                {
                    //Check controller report mode
                    byte check0 = controllerStatus.ControllerDataInput[0];
                    if (check0 != 0x30) { return ControllerInputType.Invalid; }
                }
                else if (controllerStatus.SupportedCurrent.CodeName == "SonyPS4DualShock")
                {
                    if (controllerStatus.Details.ConnectionType == ConnectionType.Bluetooth)
                    {
                        //Compute CRC32
                        int checksumOffset = controllerStatus.SupportedCurrent.OffsetBluetooth + (int)controllerStatus.SupportedCurrent.OffsetHeader.Checksum;
                        byte[] checksumInput = controllerStatus.ControllerDataInput.Take(checksumOffset).ToArray();

                        //Read CRC32
                        byte check0 = controllerStatus.ControllerDataInput[checksumOffset];
                        byte check1 = controllerStatus.ControllerDataInput[checksumOffset + 1];
                        byte check2 = controllerStatus.ControllerDataInput[checksumOffset + 2];
                        byte check3 = controllerStatus.ControllerDataInput[checksumOffset + 3];

                        //Compare 8BitDo static hash
                        if (check0 == 169 && check1 == 47 && check2 == 73 && check3 == 54) { return ControllerInputType.Input; }

                        //Compare computed CRC32 hash
                        byte[] checksumCompute = ComputeHashCRC32(0x8C2C830C, checksumInput, false);
                        if (checksumCompute[0] != check0) { return ControllerInputType.Invalid; }
                        if (checksumCompute[1] != check1) { return ControllerInputType.Invalid; }
                        if (checksumCompute[2] != check2) { return ControllerInputType.Invalid; }
                        if (checksumCompute[3] != check3) { return ControllerInputType.Invalid; }
                    }
                }
                else if (controllerStatus.SupportedCurrent.CodeName == "SonyPS5DualSense")
                {
                    if (controllerStatus.Details.ConnectionType == ConnectionType.Bluetooth)
                    {
                        //Compute CRC32
                        int checksumOffset = controllerStatus.SupportedCurrent.OffsetBluetooth + (int)controllerStatus.SupportedCurrent.OffsetHeader.Checksum;
                        byte[] checksumInput = controllerStatus.ControllerDataInput.Take(checksumOffset).ToArray();
                        byte[] checksumCompute = ComputeHashCRC32(0x8C2C830C, checksumInput, false);

                        //Compare computed CRC32 hash
                        byte check0 = controllerStatus.ControllerDataInput[checksumOffset];
                        if (checksumCompute[0] != check0) { return ControllerInputType.Invalid; }
                        byte check1 = controllerStatus.ControllerDataInput[checksumOffset + 1];
                        if (checksumCompute[1] != check1) { return ControllerInputType.Invalid; }
                        byte check2 = controllerStatus.ControllerDataInput[checksumOffset + 2];
                        if (checksumCompute[2] != check2) { return ControllerInputType.Invalid; }
                        byte check3 = controllerStatus.ControllerDataInput[checksumOffset + 3];
                        if (checksumCompute[3] != check3) { return ControllerInputType.Invalid; }
                    }
                }

                //Return result
                return ControllerInputType.Input;
            }
            catch (Exception ex)
            {
                //Return result
                Debug.WriteLine("Failed to check input data type: " + ex.Message);
                return ControllerInputType.Invalid;
            }
        }
    }
}
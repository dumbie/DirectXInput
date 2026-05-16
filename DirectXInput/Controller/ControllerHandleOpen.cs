using LibraryUsb;
using System;
using System.Diagnostics;
using static LibraryShared.Classes;
using static LibraryShared.Enums;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Controller open handle
        bool ControllerHandleOpen(ControllerStatus controllerStatus)
        {
            try
            {
                //Find and connect to win controller
                if (controllerStatus.Details.Type == ControllerType.WinUsbDevice)
                {
                    controllerStatus.WinUsbDevice = new WinUsbDevice(controllerStatus.Details.DevicePath, controllerStatus.Details.DeviceInstanceId, true, false);
                    if (!controllerStatus.WinUsbDevice.Connected)
                    {
                        Debug.WriteLine("Invalid winusb open device: " + controllerStatus.Details.DisplayName);
                        return false;
                    }
                    else
                    {
                        //Set default controller variables
                        controllerStatus.ControllerDataInput = new byte[controllerStatus.WinUsbDevice.IntIn];
                        controllerStatus.ControllerDataOutput = new byte[controllerStatus.WinUsbDevice.IntOut];

                        Debug.WriteLine("Opened winusb controller: " + controllerStatus.Details.DisplayName + " / Path " + controllerStatus.Details.DevicePath);
                        return true;
                    }
                }
                //Find and connect to hid controller
                else
                {
                    controllerStatus.HidDevice = new HidDevice(controllerStatus.Details.DevicePath, controllerStatus.Details.DeviceInstanceId, false);
                    if (!controllerStatus.HidDevice.Connected)
                    {
                        Debug.WriteLine("Invalid hid open device: " + controllerStatus.Details.DisplayName);
                        return false;
                    }
                    else
                    {
                        //Set default controller variables
                        controllerStatus.ControllerDataInput = new byte[controllerStatus.HidDevice.Capabilities.InputReportByteLength];
                        controllerStatus.ControllerDataOutput = new byte[controllerStatus.HidDevice.Capabilities.OutputReportByteLength];

                        Debug.WriteLine("Opened hid controller: " + controllerStatus.Details.DisplayName + " / Exclusive " + controllerStatus.HidDevice.Exclusive + " / Path " + controllerStatus.Details.DevicePath);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed opening controller handle: " + ex.Message);
                return false;
            }
        }
    }
}
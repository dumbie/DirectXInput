using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static ArnoldVinkCode.AVDevices.Enumerate;
using static ArnoldVinkCode.AVDevices.Interop;
using static LibraryUsb.NativeMethods_File;
using static LibraryUsb.NativeMethods_Guid;

namespace LibraryUsb
{
    public partial class TetherScriptDevice
    {
        public bool Connected;
        public bool Exclusive;
        private AVFin FileHandle;
        public string DevicePath;
        public string HardwareTarget;

        public TetherScriptDevice(DriverProductIds driverProduct)
        {
            try
            {
                OpenDevice(driverProduct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to create TetherScript device: " + ex.Message);
            }
        }

        private bool OpenDevice(DriverProductIds driverProduct)
        {
            try
            {
                //Check driver product type
                if (driverProduct == DriverProductIds.TTC_PRODUCTID_KEYBOARD)
                {
                    HardwareTarget = "hid\\ttcvcontrkb";
                }
                else if (driverProduct == DriverProductIds.TTC_PRODUCTID_MOUSEREL)
                {
                    HardwareTarget = "hid\\ttcvcontrmsrel";
                }
                else if (driverProduct == DriverProductIds.TTC_PRODUCTID_MOUSEABS)
                {
                    HardwareTarget = "hid\\ttcvcontrmsabs";
                }

                //Find the virtual device path
                IEnumerable<EnumerateInfo> SelectedHidDevice = EnumerateDevicesSetupApi(GuidClassHidDevice, true);
                foreach (EnumerateInfo EnumDevice in SelectedHidDevice)
                {
                    if (EnumDevice.HardwareIds.FirstOrDefault().ToLower() == HardwareTarget)
                    {
                        DevicePath = EnumDevice.DevicePath;
                        break;
                    }
                }

                FileShareMode shareModeExclusive = FileShareMode.FILE_SHARE_NONE;
                FileShareMode shareModeNormal = FileShareMode.FILE_SHARE_READ | FileShareMode.FILE_SHARE_WRITE;
                FileDesiredAccess desiredAccess = FileDesiredAccess.GENERIC_WRITE;
                FileCreationDisposition creationDisposition = FileCreationDisposition.OPEN_EXISTING;
                FileFlagsAndAttributes flagsAttributes = FileFlagsAndAttributes.FILE_FLAG_NORMAL | FileFlagsAndAttributes.FILE_FLAG_NO_BUFFERING | FileFlagsAndAttributes.FILE_FLAG_WRITE_THROUGH;

                //Try to open the device exclusively
                FileHandle = new AVFin(AVFinMethod.CloseHandle, CreateFile(DevicePath, desiredAccess, shareModeExclusive, IntPtr.Zero, creationDisposition, flagsAttributes, IntPtr.Zero));
                Exclusive = true;

                //Try to open the device normally
                if (FileHandle.Get() == IntPtr.Zero || IntPtr.IsNegative(FileHandle.Get()))
                {
                    //Debug.WriteLine("Failed to open TetherScript device exclusively, opening normally.");
                    FileHandle.Set(CreateFile(DevicePath, desiredAccess, shareModeNormal, IntPtr.Zero, creationDisposition, flagsAttributes, IntPtr.Zero));
                    Exclusive = false;
                }

                //Check if the device is opened
                if (FileHandle.Get() == IntPtr.Zero || IntPtr.IsNegative(FileHandle.Get()))
                {
                    //Debug.WriteLine("Failed to open TetherScript device: " + DevicePath);
                    Connected = false;
                    return false;
                }
                else
                {
                    //Debug.WriteLine("Opened TetherScript device: " + DevicePath + ", exclusively: " + Exclusive);
                    Connected = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to open TetherScript device: " + ex.Message);
                Connected = false;
                return false;
            }
        }

        public bool CloseDevice()
        {
            try
            {
                if (FileHandle != null)
                {
                    FileHandle.Dispose();
                    FileHandle = null;
                }
                Connected = false;
                Exclusive = false;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to close TetherScript device: " + ex.Message);
                return false;
            }
        }
    }
}
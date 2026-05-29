using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Linq;
using static ArnoldVinkCode.AVDevices.Enumerate;
using static ArnoldVinkCode.AVDevices.Interop;
using static LibraryUsb.NativeMethods_File;
using static LibraryUsb.NativeMethods_WinUsb;

namespace LibraryUsb
{
    public partial class WinUsbDevice
    {
        public bool Connected;
        public bool Initialized;
        public string DevicePath;
        public string DeviceInstanceId;
        private AVFin FileHandle;
        private AVFin WinUsbHandle;
        public byte PipeIdInterruptIn = 0xFF;
        public byte PipeIdInterruptOut = 0xFF;
        public byte PipeIdBulkIn = 0xFF;
        public byte PipeIdBulkOut = 0xFF;

        public WinUsbDevice(Guid deviceGuid, bool initialize, bool closeDevice)
        {
            try
            {
                EnumerateInfo enumerateInfo = EnumerateDevicesSetupApi(deviceGuid, true, true).FirstOrDefault();
                if (enumerateInfo != null)
                {
                    DevicePath = enumerateInfo.DevicePath.ToLower();
                    DeviceInstanceId = enumerateInfo.DeviceInstanceId.ToLower();
                }
                else
                {
                    Debug.WriteLine("Create winusb device not found.");
                    return;
                }

                if (OpenDevice())
                {
                    if (initialize)
                    {
                        InitializeDevice();
                    }
                    if (closeDevice)
                    {
                        CloseDevice();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to create winusb device: " + ex.Message);
            }
        }

        public WinUsbDevice(string devicePath, string deviceInstanceId, bool initialize, bool closeDevice)
        {
            try
            {
                DevicePath = devicePath.ToLower();
                DeviceInstanceId = deviceInstanceId.ToLower();

                if (OpenDevice())
                {
                    if (initialize)
                    {
                        InitializeDevice();
                    }
                    if (closeDevice)
                    {
                        CloseDevice();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to create winusb device: " + ex.Message);
            }
        }

        private bool OpenDevice()
        {
            try
            {
                FileShareMode shareModeNormal = FileShareMode.FILE_SHARE_READ | FileShareMode.FILE_SHARE_WRITE;
                FileDesiredAccess desiredAccess = FileDesiredAccess.GENERIC_READ | FileDesiredAccess.GENERIC_WRITE;
                FileCreationDisposition creationDisposition = FileCreationDisposition.OPEN_EXISTING;
                FileFlagsAndAttributes flagsAttributes = FileFlagsAndAttributes.FILE_FLAG_NORMAL | FileFlagsAndAttributes.FILE_FLAG_OVERLAPPED | FileFlagsAndAttributes.FILE_FLAG_NO_BUFFERING | FileFlagsAndAttributes.FILE_FLAG_WRITE_THROUGH;

                //Try to open the device normally
                FileHandle = new AVFin(AVFinMethod.CloseHandle, CreateFile(DevicePath, desiredAccess, shareModeNormal, IntPtr.Zero, creationDisposition, flagsAttributes, IntPtr.Zero));

                //Check if the device is opened
                if (FileHandle.Get() == IntPtr.Zero || IntPtr.IsNegative(FileHandle.Get()))
                {
                    //Debug.WriteLine("Failed to open winusb device: " + DevicePath);
                    Connected = false;
                    return false;
                }
                else
                {
                    //Debug.WriteLine("Opened winusb device: " + DevicePath);
                    Connected = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to open winusb device: " + ex.Message);
                Connected = false;
                return false;
            }
        }

        private bool InitializeDevice()
        {
            try
            {
                //Try to initialize the device
                WinUsbHandle = new AVFin(AVFinMethod.CloseHandle);
                if (!WinUsb_Initialize(FileHandle.Get(), out WinUsbHandle.Get()))
                {
                    Debug.WriteLine("Failed to initialize winusb device: " + DevicePath);
                    Initialized = false;
                    return false;
                }

                WINUSB_PIPE_INFORMATION pipeInformation = new WINUSB_PIPE_INFORMATION();
                WINUSB_INTERFACE_DESCRIPTOR interfaceDescriptor = new WINUSB_INTERFACE_DESCRIPTOR();

                if (WinUsb_QueryInterfaceSettings(WinUsbHandle.Get(), 0, ref interfaceDescriptor))
                {
                    for (byte i = 0; i < interfaceDescriptor.bNumEndpoints; i++)
                    {
                        WinUsb_QueryPipe(WinUsbHandle.Get(), 0, i, ref pipeInformation);
                        if (pipeInformation.PipeType == WINUSB_PIPE_TYPE.Bulk && WinUsbEndpointDirectionIn(pipeInformation.PipeId))
                        {
                            PipeIdBulkIn = pipeInformation.PipeId;
                            WinUsb_FlushPipe(WinUsbHandle.Get(), PipeIdBulkIn);
                        }
                        else if (pipeInformation.PipeType == WINUSB_PIPE_TYPE.Bulk && WinUsbEndpointDirectionOut(pipeInformation.PipeId))
                        {
                            PipeIdBulkOut = pipeInformation.PipeId;
                            WinUsb_FlushPipe(WinUsbHandle.Get(), PipeIdBulkOut);
                        }
                        else if (pipeInformation.PipeType == WINUSB_PIPE_TYPE.Interrupt && WinUsbEndpointDirectionIn(pipeInformation.PipeId))
                        {
                            PipeIdInterruptIn = pipeInformation.PipeId;
                            WinUsb_FlushPipe(WinUsbHandle.Get(), PipeIdInterruptIn);
                        }
                        else if (pipeInformation.PipeType == WINUSB_PIPE_TYPE.Interrupt && WinUsbEndpointDirectionOut(pipeInformation.PipeId))
                        {
                            PipeIdInterruptOut = pipeInformation.PipeId;
                            WinUsb_FlushPipe(WinUsbHandle.Get(), PipeIdInterruptOut);
                        }
                    }
                    //Debug.WriteLine("Initialized winusb device: " + DevicePath);
                    Initialized = true;
                    return true;
                }
                //Debug.WriteLine("Failed to initialize winusb device: " + DevicePath);
                Initialized = false;
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to initialize winusb device: " + ex.Message);
                Initialized = false;
                return false;
            }
        }

        public bool CloseDevice()
        {
            try
            {
                if (WinUsbHandle != null)
                {
                    WinUsb_AbortPipe(WinUsbHandle.Get(), PipeIdInterruptIn);
                    WinUsb_AbortPipe(WinUsbHandle.Get(), PipeIdInterruptOut);
                    WinUsb_AbortPipe(WinUsbHandle.Get(), PipeIdBulkIn);
                    WinUsb_AbortPipe(WinUsbHandle.Get(), PipeIdBulkOut);
                    WinUsb_Free(WinUsbHandle.Get());
                    WinUsbHandle.Dispose();
                    WinUsbHandle = null;
                }
                if (FileHandle != null)
                {
                    FileHandle.Dispose();
                    FileHandle = null;
                }
                Connected = false;
                Initialized = false;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to close winusb device: " + ex.Message);
                return false;
            }
        }
    }
}
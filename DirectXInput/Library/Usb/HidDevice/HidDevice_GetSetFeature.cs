using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using static ArnoldVinkCode.AVInteropDll;
using static LibraryUsb.NativeMethods_IoControl;

namespace LibraryUsb
{
    public partial class HidDevice
    {
        public byte[] GetFeature(byte[] inputFeatureBytes, uint timeOutMs)
        {
            try
            {
                //Check if device is connected
                if (!Connected) { return null; }

                //Create event
                using AVFin createEvent = new AVFin(AVFinMethod.CloseHandle, CreateEvent(IntPtr.Zero, true, false, null));

                //Create native overlapped
                NativeOverlapped nativeOverlapped = new NativeOverlapped();
                nativeOverlapped.EventHandle = createEvent.Get();

                //Send device control code
                bool overlapResult = DeviceIoControl(FileHandle.Get(), (uint)IoControlCodes.IOCTL_HID_GET_FEATURE, null, 0, inputFeatureBytes, inputFeatureBytes.Length, out int bytesReturned, ref nativeOverlapped);

                //Check overlap result
                if (!overlapResult)
                {
                    int lastWin32Error = Marshal.GetLastWin32Error();
                    if (lastWin32Error == (int)IoErrorCodes.ERROR_SUCCESS || lastWin32Error == (int)IoErrorCodes.ERROR_IO_PENDING)
                    {
                        if (WaitForSingleObject(nativeOverlapped.EventHandle, timeOutMs) == WaitForSingleObjectResult.WAIT_OBJECT)
                        {
                            if (GetOverlappedResult(FileHandle.Get(), ref nativeOverlapped, out int bytesTransferred, false))
                            {
                                return inputFeatureBytes;
                            }
                        }
                    }
                }
                else
                {
                    return inputFeatureBytes;
                }

                //Return result
                //Debug.WriteLine("Failed to get feature: " + overlapResult + " / " + bytesReturned);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get feature: " + ex.Message);
                return null;
            }
        }

        public bool SetFeature(byte[] featureByte, uint timeOutMs)
        {
            try
            {
                //Check if device is connected
                if (!Connected) { return false; }

                //Create event
                using AVFin createEvent = new AVFin(AVFinMethod.CloseHandle, CreateEvent(IntPtr.Zero, true, false, null));

                //Create native overlapped
                NativeOverlapped nativeOverlapped = new NativeOverlapped();
                nativeOverlapped.EventHandle = createEvent.Get();

                //Send device control code
                bool overlapResult = DeviceIoControl(FileHandle.Get(), (uint)IoControlCodes.IOCTL_HID_SET_FEATURE, featureByte, featureByte.Length, null, 0, out int bytesReturned, ref nativeOverlapped);

                //Check overlap result
                if (!overlapResult)
                {
                    int lastWin32Error = Marshal.GetLastWin32Error();
                    if (lastWin32Error == (int)IoErrorCodes.ERROR_SUCCESS || lastWin32Error == (int)IoErrorCodes.ERROR_IO_PENDING)
                    {
                        if (WaitForSingleObject(nativeOverlapped.EventHandle, timeOutMs) == WaitForSingleObjectResult.WAIT_OBJECT)
                        {
                            if (GetOverlappedResult(FileHandle.Get(), ref nativeOverlapped, out int bytesTransferred, false))
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    return true;
                }

                //Return result
                //Debug.WriteLine("Failed to set feature: " + overlapResult + " / " + bytesReturned);
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to set feature: " + ex.Message);
                return false;
            }
        }
    }
}
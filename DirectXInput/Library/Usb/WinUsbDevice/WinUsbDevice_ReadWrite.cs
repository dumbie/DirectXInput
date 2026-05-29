using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using static ArnoldVinkCode.AVInteropDll;
using static LibraryUsb.NativeMethods_IoControl;
using static LibraryUsb.NativeMethods_WinUsb;

namespace LibraryUsb
{
    public partial class WinUsbDevice
    {
        public bool WriteBytesTransfer(WINUSB_REQUEST_TYPE requestType, WINUSB_REQUEST request, ushort value, byte[] outputBuffer, uint timeOutMs)
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

                //Write bytes
                WINUSB_SETUP_PACKET setupPacket = new WINUSB_SETUP_PACKET();
                setupPacket.RequestType = requestType;
                setupPacket.Request = request;
                setupPacket.Value = value;
                setupPacket.Index = 0;
                setupPacket.Length = (ushort)outputBuffer.Length;
                bool overlapResult = WinUsb_ControlTransfer(WinUsbHandle.Get(), setupPacket, outputBuffer, outputBuffer.Length, out int bytesReturned, ref nativeOverlapped);

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
                //Debug.WriteLine("Failed to write transfer bytes: " + overlapResult + " / " + bytesReturned);
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to write transfer bytes: " + ex.Message);
                return false;
            }
        }

        public bool WriteBytesInterruptPipe(byte[] outputBuffer, uint timeOutMs)
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

                //Write bytes
                bool overlapResult = WinUsb_WritePipe(WinUsbHandle.Get(), PipeIdInterruptOut, outputBuffer, outputBuffer.Length, out int bytesReturned, ref nativeOverlapped);

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
                //Debug.WriteLine("Failed to write intpipe bytes: " + overlapResult + " / " + bytesReturned);
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to write intpipe bytes: " + ex.Message);
                return false;
            }
        }

        public bool WriteBytesBulkPipe(byte[] outputBuffer, uint timeOutMs)
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

                //Write bytes
                bool overlapResult = WinUsb_WritePipe(WinUsbHandle.Get(), PipeIdBulkOut, outputBuffer, outputBuffer.Length, out int bytesReturned, ref nativeOverlapped);

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
                //Debug.WriteLine("Failed to write bulkpipe bytes: " + overlapResult + " / " + bytesReturned);
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to write bulkpipe bytes: " + ex.Message);
                return false;
            }
        }

        public byte[] ReadBytesInterruptPipe(int inputBufferLength, uint timeOutMs)
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

                //Read bytes
                byte[] inputBuffer = new byte[inputBufferLength];
                bool overlapResult = WinUsb_ReadPipe(WinUsbHandle.Get(), PipeIdInterruptIn, inputBuffer, inputBuffer.Length, out int bytesReturned, ref nativeOverlapped);

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
                                return inputBuffer;
                            }
                        }
                    }
                }
                else
                {
                    return inputBuffer;
                }

                //Return result
                //Debug.WriteLine("Failed to read intpipe bytes: " + overlapResult + " / " + bytesReturned);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to read intpipe bytes: " + ex.Message);
                return null;
            }
        }

        public byte[] ReadBytesBulkPipe(int inputBufferLength, uint timeOutMs)
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

                //Read bytes
                byte[] inputBuffer = new byte[inputBufferLength];
                bool overlapResult = WinUsb_ReadPipe(WinUsbHandle.Get(), PipeIdBulkIn, inputBuffer, inputBuffer.Length, out int bytesReturned, ref nativeOverlapped);

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
                                return inputBuffer;
                            }
                        }
                    }
                }
                else
                {
                    return inputBuffer;
                }

                //Return result
                //Debug.WriteLine("Failed to read bulkpipe bytes: " + overlapResult + " / " + bytesReturned);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to read bulkpipe bytes: " + ex.Message);
                return null;
            }
        }
    }
}
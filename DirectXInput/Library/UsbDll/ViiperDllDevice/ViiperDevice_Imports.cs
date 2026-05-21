using System;
using System.Runtime.InteropServices;

namespace LibraryUsb
{
    public partial class ViiperDllDevice
    {
        const string dllPathLibViiper = "libVIIPER";

        [DllImport(dllPathLibViiper, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool NewUSBServer([In] ref USBServerConfig config, out UIntPtr outHandle, VIIPERLogCallbackDelegate logCallback);

        [DllImport(dllPathLibViiper, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool CloseUSBServer(UIntPtr handle);

        [DllImport(dllPathLibViiper, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool CreateUSBBus(UIntPtr handle, ref uint busID);

        [DllImport(dllPathLibViiper, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool CreateXbox360Device(UIntPtr serverHandle, out UIntPtr outDeviceHandle, uint busID, [MarshalAs(UnmanagedType.I1)] bool autoAttachLocalhost, ushort idVendor, ushort idProduct, byte xinputSubType);

        [DllImport(dllPathLibViiper, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool CreateMouseDevice(UIntPtr serverHandle, out UIntPtr outDeviceHandle, uint busID, [MarshalAs(UnmanagedType.I1)] bool autoAttachLocalhost, ushort idVendor, ushort idProduct);

        [DllImport(dllPathLibViiper, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool CreateKeyboardDevice(UIntPtr serverHandle, out UIntPtr outDeviceHandle, uint busID, [MarshalAs(UnmanagedType.I1)] bool autoAttachLocalhost, ushort idVendor, ushort idProduct);

        [DllImport(dllPathLibViiper, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SetXbox360DeviceState(UIntPtr deviceHandle, Xbox360DeviceState state);

        [DllImport(dllPathLibViiper, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SetMouseDeviceState(UIntPtr deviceHandle, MouseDeviceState state);

        [DllImport(dllPathLibViiper, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SetKeyboardDeviceState(UIntPtr deviceHandle, KeyboardDeviceState state);

        [DllImport(dllPathLibViiper, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SetXbox360RumbleCallback(UIntPtr deviceHandle, Xbox360RumbleCallbackDelegate callback);
    }
}
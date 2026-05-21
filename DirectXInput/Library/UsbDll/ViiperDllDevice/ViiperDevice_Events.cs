using System;
using System.Runtime.InteropServices;

namespace LibraryUsb
{
    public partial class ViiperDllDevice
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Xbox360RumbleCallbackDelegate(UIntPtr handle, byte leftMotor, byte rightMotor);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void VIIPERLogCallbackDelegate(VIIPERLogLevel level, [MarshalAs(UnmanagedType.LPStr)] string message);
    }
}
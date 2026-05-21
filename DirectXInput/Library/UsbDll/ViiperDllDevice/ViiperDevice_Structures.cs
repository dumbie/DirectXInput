using System.Runtime.InteropServices;

namespace LibraryUsb
{
    public partial class ViiperDllDevice
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct USBServerConfig
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string addr;
            public ulong connection_timeout_ms;
            public ulong device_handler_connect_timeout_ms;
            public uint write_batch_flush_interval_ms;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Xbox360DeviceState
        {
            public uint Buttons;
            public byte LT;
            public byte RT;
            public short LX;
            public short LY;
            public short RX;
            public short RY;
            public byte Reserved0, Reserved1, Reserved2, Reserved3, Reserved4, Reserved5;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseDeviceState
        {
            public byte Buttons;
            public short DX;
            public short DY;
            public short Wheel;
            public short Pan;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardDeviceState
        {
            public byte Modifiers;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] KeyBitmap;
        }
    }
}
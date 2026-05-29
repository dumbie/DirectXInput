using System;
using System.Runtime.InteropServices;
using System.Security;
using static LibraryUsb.HidDeviceAttributes;
using static LibraryUsb.HidDeviceCapabilities;

namespace LibraryUsb
{
    [SuppressUnmanagedCodeSecurity]
    public class NativeMethods_Hid
    {
        public enum HID_USAGE_GENERIC_DESKTOP_PAGE : byte
        {
            HID_USAGE_GENERIC_UNDEFINED = 0x00,
            HID_USAGE_GENERIC_POINTER = 0x01,
            HID_USAGE_GENERIC_MOUSE = 0x02,
            HID_USAGE_GENERIC_JOYSTICK = 0x04,
            HID_USAGE_GENERIC_GAMEPAD = 0x05,
            HID_USAGE_GENERIC_KEYBOARD = 0x06,
            HID_USAGE_GENERIC_KEYPAD = 0x07,
            HID_USAGE_GENERIC_MULTI_AXIS_CONTROLLER = 0x08
        }

        [DllImport("hid.dll")]
        public static extern bool HidD_GetAttributes(IntPtr hidDeviceObject, ref HIDD_ATTRIBUTES attributes);

        [DllImport("hid.dll")]
        public static extern bool HidP_GetCaps(IntPtr preparsedData, ref HIDP_CAPS capabilities);

        [DllImport("hid.dll", CharSet = CharSet.Unicode)]
        public static extern bool HidD_GetProductString(IntPtr hidDeviceObject, ref byte lpReportBuffer, int ReportBufferLength);

        [DllImport("hid.dll", CharSet = CharSet.Unicode)]
        public static extern bool HidD_GetManufacturerString(IntPtr hidDeviceObject, ref byte lpReportBuffer, int ReportBufferLength);

        [DllImport("hid.dll", CharSet = CharSet.Unicode)]
        public static extern bool HidD_GetSerialNumberString(IntPtr hidDeviceObject, ref byte lpReportBuffer, int reportBufferLength);

        [DllImport("hid.dll")]
        public static extern bool HidD_GetPreparsedData(IntPtr hidDeviceObject, ref IntPtr preparsedData);

        [DllImport("hid.dll")]
        public static extern bool HidD_FreePreparsedData(IntPtr preparsedData);
    }
}
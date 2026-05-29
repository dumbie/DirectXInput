using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace LibraryUsb
{
    [SuppressUnmanagedCodeSecurity]
    public class NativeMethods_WinUsb
    {
        public enum WINUSB_REQUEST_TYPE : byte
        {
            //Host to Device
            HostToDevice_Standard_Device = 0x00,
            HostToDevice_Standard_Interface = 0x01,
            HostToDevice_Standard_Endpoint = 0x02,
            HostToDevice_Standard_Other = 0x03,
            HostToDevice_Class_Device = 0x20,
            HostToDevice_Class_Interface = 0x21,
            HostToDevice_Class_Endpoint = 0x22,
            HostToDevice_Class_Other = 0x23,
            HostToDevice_Vendor_Device = 0x40,
            HostToDevice_Vendor_Interface = 0x41,
            HostToDevice_Vendor_Endpoint = 0x42,
            HostToDevice_Vendor_Other = 0x43,
            HostToDevice_Reserved_Device = 0x60,
            HostToDevice_Reserved_Interface = 0x61,
            HostToDevice_Reserved_Endpoint = 0x62,
            HostToDevice_Reserved_Other = 0x63,
            //Device to Host
            DeviceToHost_Standard_Device = 0x80,
            DeviceToHost_Standard_Interface = 0x81,
            DeviceToHost_Standard_Endpoint = 0x82,
            DeviceToHost_Standard_Other = 0x83,
            DeviceToHost_Class_Device = 0xA0,
            DeviceToHost_Class_Interface = 0xA1,
            DeviceToHost_Class_Endpoint = 0xA2,
            DeviceToHost_Class_Other = 0xA3,
            DeviceToHost_Vendor_Device = 0xC0,
            DeviceToHost_Vendor_Interface = 0xC1,
            DeviceToHost_Vendor_Endpoint = 0xC2,
            DeviceToHost_Vendor_Other = 0xC3,
            DeviceToHost_Reserved_Device = 0xE0,
            DeviceToHost_Reserved_Interface = 0xE1,
            DeviceToHost_Reserved_Endpoint = 0xE2,
            DeviceToHost_Reserved_Other = 0xE3
        }

        public enum WINUSB_REQUEST : byte
        {
            GetStatus = 0x00,
            ClearFeature = 0x01,
            SetFeature = 0x03,
            SetAddress = 0x05,
            GetDescriptor = 0x06,
            SetDescriptor = 0x07,
            GetConfiguration = 0x08,
            SetConfiguration = 0x09,
            GetInterface = 0x0A,
            SetInterface = 0x0B,
            SynchFrame = 0x0C
        }

        public enum WINUSB_PIPE_TYPE : int
        {
            Control = 0,
            Isochronous = 1,
            Bulk = 2,
            Interrupt = 3
        }

        public enum WINUSB_DESCRIPTOR_TYPE : byte
        {
            USB_DEVICE_DESCRIPTOR_TYPE = 0x01,
            USB_CONFIGURATION_DESCRIPTOR_TYPE = 0x02,
            USB_STRING_DESCRIPTOR_TYPE = 0x03
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINUSB_DEVICE_DESCRIPTOR
        {
            public byte bLength;
            public byte bDescriptorType;
            public ushort bcdUSB;
            public byte bDeviceClass;
            public byte bDeviceSubClass;
            public byte bDeviceProtocol;
            public byte bMaxPacketSize0;
            public ushort idVendor;
            public ushort idProduct;
            public ushort bcdDevice;
            public byte iManufacturer;
            public byte iProduct;
            public byte iSerialNumber;
            public byte bNumConfigurations;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINUSB_INTERFACE_DESCRIPTOR
        {
            public byte bLength;
            public byte bDescriptorType;
            public byte bInterfaceNumber;
            public byte bAlternateSetting;
            public byte bNumEndpoints;
            public byte bInterfaceClass;
            public byte bInterfaceSubClass;
            public byte bInterfaceProtocol;
            public byte iInterface;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINUSB_PIPE_INFORMATION
        {
            public WINUSB_PIPE_TYPE PipeType;
            public byte PipeId;
            public ushort MaximumPacketSize;
            public byte Interval;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINUSB_SETUP_PACKET
        {
            public WINUSB_REQUEST_TYPE RequestType;
            public WINUSB_REQUEST Request;
            public ushort Value;
            public ushort Index;
            public ushort Length;
        }

        [DllImport("winusb.dll")]
        public static extern bool WinUsb_Initialize(IntPtr deviceHandle, out IntPtr interfaceHandle);

        [DllImport("winusb.dll")]
        public static extern bool WinUsb_GetDescriptor(IntPtr interfaceHandle, WINUSB_DESCRIPTOR_TYPE DescriptorType, byte Index, ushort LanguageID, ref WINUSB_DEVICE_DESCRIPTOR deviceDesc, int BufferLength, out int LengthTransfered);

        [DllImport("winusb.dll")]
        public static extern bool WinUsb_QueryInterfaceSettings(IntPtr interfaceHandle, byte alternateInterfaceNumber, ref WINUSB_INTERFACE_DESCRIPTOR usbAltInterfaceDescriptor);

        [DllImport("winusb.dll")]
        public static extern bool WinUsb_QueryPipe(IntPtr interfaceHandle, byte alternateInterfaceNumber, byte PipeIndex, ref WINUSB_PIPE_INFORMATION pipeInformation);

        [DllImport("winusb.dll")]
        public static extern bool WinUsb_AbortPipe(IntPtr interfaceHandle, byte pipeID);

        [DllImport("winusb.dll")]
        public static extern bool WinUsb_FlushPipe(IntPtr interfaceHandle, byte pipeID);

        [DllImport("winusb.dll")]
        public static extern bool WinUsb_ControlTransfer(IntPtr interfaceHandle, WINUSB_SETUP_PACKET setupPacket, byte[] buffer, int bufferLength, out int lengthTransferred, ref NativeOverlapped overlapped);

        [DllImport("winusb.dll")]
        public static extern bool WinUsb_WritePipe(IntPtr interfaceHandle, byte pipeID, byte[] buffer, int bufferLength, out int lengthTransferred, ref NativeOverlapped overlapped);

        [DllImport("winusb.dll")]
        public static extern bool WinUsb_ReadPipe(IntPtr interfaceHandle, byte pipeID, byte[] buffer, int bufferLength, out int lengthTransferred, ref NativeOverlapped overlapped);

        [DllImport("winusb.dll")]
        public static extern bool WinUsb_Free(IntPtr interfaceHandle);
    }
}
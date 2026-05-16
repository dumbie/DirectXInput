using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static LibraryUsb.NativeMethods_Bth;
using static LibraryUsb.NativeMethods_IoControl;

namespace LibraryUsb
{
    public partial class BthDevice
    {
        public static bool BluetoothDisconnect(string serialNumber)
        {
            try
            {
                //Check bluetooth device serial number
                if (string.IsNullOrWhiteSpace(serialNumber))
                {
                    Debug.WriteLine("Empty bluetooth device serial number.");
                    return false;
                }

                Debug.WriteLine("Attempting to disconnect bluetooth device: " + serialNumber);

                //Get and parse the mac address
                byte[] macAddressBytes = new byte[8];
                string[] macAddressSplit = { $"{serialNumber[0]}{serialNumber[1]}", $"{serialNumber[2]}{serialNumber[3]}", $"{serialNumber[4]}{serialNumber[5]}", $"{serialNumber[6]}{serialNumber[7]}", $"{serialNumber[8]}{serialNumber[9]}", $"{serialNumber[10]}{serialNumber[11]}" };
                for (int i = 0; i < 6; i++)
                {
                    macAddressBytes[5 - i] = Convert.ToByte(macAddressSplit[i], 16);
                }

                Debug.WriteLine("Disconnecting bluetooth device: " + serialNumber);

                //Disconnect the device from bluetooth
                BLUETOOTH_FIND_RADIO_PARAMS radioFindParams = new BLUETOOTH_FIND_RADIO_PARAMS();
                radioFindParams.dwSize = Marshal.SizeOf(radioFindParams);

                //Find first radio handle
                using AVFin radioHandleFound = new AVFin(AVFinMethod.CloseHandle);
                using AVFin radioHandle = new AVFin(AVFinMethod.Custom, BluetoothFindFirstRadio(ref radioFindParams, out radioHandleFound.Get()));
                radioHandle.SetReleaser(delegate (IntPtr releaseObject) { BluetoothFindRadioClose(releaseObject); });

                if (radioHandle.Get() == IntPtr.Zero)
                {
                    Debug.WriteLine("No bluetooth radio found to disconnect.");
                    return false;
                }

                bool bluetoothDisconnected = false;
                while (!bluetoothDisconnected)
                {
                    bluetoothDisconnected = DeviceIoControl(radioHandleFound.Get(), (uint)IoControlCodes.IOCTL_BTH_DISCONNECT_DEVICE, macAddressBytes, macAddressBytes.Length, null, 0, out int bytesWritten, IntPtr.Zero) && bytesWritten > 0;
                    if (!bluetoothDisconnected)
                    {
                        radioHandleFound.Dispose();
                        if (!BluetoothFindNextRadio(radioHandle.Get(), out radioHandleFound.Get()))
                        {
                            bluetoothDisconnected = true;
                        }
                    }
                }

                Debug.WriteLine("Succesfully disconnected bluetooth: " + bluetoothDisconnected);
                return bluetoothDisconnected;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed disconnecting bluetooth: " + ex.Message);
                return false;
            }
        }
    }
}
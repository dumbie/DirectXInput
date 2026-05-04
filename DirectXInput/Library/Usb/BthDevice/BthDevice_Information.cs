using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static LibraryUsb.NativeMethods_Bth;

namespace LibraryUsb
{
    public partial class BthDevice
    {
        public static BLUETOOTH_ADDRESS? GetLocalBluetoothMacAddress()
        {
            try
            {
                BLUETOOTH_FIND_RADIO_PARAMS radioFindParams = new BLUETOOTH_FIND_RADIO_PARAMS();
                radioFindParams.dwSize = Marshal.SizeOf(radioFindParams);

                //Find first radio handle
                using AVFin radioHandleFound = new AVFin(AVFinMethod.CloseHandle);
                using AVFin radioHandle = new AVFin(AVFinMethod.Custom, BluetoothFindFirstRadio(ref radioFindParams, out radioHandleFound.Get()));
                radioHandle.SetReleaser(delegate (IntPtr releaseObject) { BluetoothFindRadioClose(releaseObject); });

                if (radioHandle.Get() == IntPtr.Zero)
                {
                    Debug.WriteLine("No bluetooth radio found to get mac address for.");
                    return null;
                }

                BLUETOOTH_RADIO_INFO radioInfo = new BLUETOOTH_RADIO_INFO();
                radioInfo.dwSize = Marshal.SizeOf(radioInfo);
                if (BluetoothGetRadioInfo(radioHandle.Get(), ref radioInfo))
                {
                    Debug.WriteLine("Bluetooth local mac address: " + radioInfo.address.byte1 + ":" + radioInfo.address.byte2 + ":" + radioInfo.address.byte3 + ":" + radioInfo.address.byte4 + ":" + radioInfo.address.byte5 + ":" + radioInfo.address.byte6);
                    return radioInfo.address;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get local bluetooth mac address: " + ex.Message);
                return null;
            }
        }
    }
}
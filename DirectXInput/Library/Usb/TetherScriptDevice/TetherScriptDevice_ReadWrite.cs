using System;
using System.Diagnostics;
using static LibraryUsb.NativeMethods_Hid;

namespace LibraryUsb
{
    public partial class TetherScriptDevice
    {
        public bool SetFeature(IntPtr FileHandle, byte[] featureByte)
        {
            try
            {
                return HidD_SetFeature(FileHandle, featureByte, featureByte.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to set feature: " + ex.Message);
                return false;
            }
        }
    }
}
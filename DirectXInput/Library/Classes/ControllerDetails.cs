using static LibraryShared.Enums;

namespace LibraryShared
{
    public partial class Classes
    {
        public class ControllerDetails
        {
            public string DisplayName { get; set; }
            public string DevicePath { get; set; }
            public string DeviceInstanceId { get; set; }
            public ConnectionType ConnectionType { get; set; }
            public ControllerType Type { get; set; }
            public ControllerProfile Profile { get; set; }

            public string ConnectionTypeString()
            {
                try
                {
                    if (ConnectionType == ConnectionType.Wifi)
                    {
                        return "Wifi";
                    }
                    else if (ConnectionType == ConnectionType.Bluetooth)
                    {
                        return "Bluetooth";
                    }
                    else
                    {
                        return "Wired";
                    }
                }
                catch
                {
                    return "Wired";
                }
            }

            public int ConnectionTypeOffset(ControllerSupported supportedCurrent)
            {
                try
                {
                    if (ConnectionType == ConnectionType.Wifi)
                    {
                        return supportedCurrent.OffsetWifi;
                    }
                    else if (ConnectionType == ConnectionType.Bluetooth)
                    {
                        return supportedCurrent.OffsetBluetooth;
                    }
                    else
                    {
                        return supportedCurrent.OffsetWired;
                    }
                }
                catch
                {
                    return supportedCurrent.OffsetWired;
                }
            }
        }
    }
}
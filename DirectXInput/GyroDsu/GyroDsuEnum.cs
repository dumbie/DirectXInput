namespace DirectXInput
{
    partial class WindowMain
    {
        public enum DsuMessageType : uint
        {
            VersionInfo = 0x100000,
            ControllerInfo = 0x100001,
            ControllerData = 0x100002,
            RumbleInfo = 0x110001,
            RumbleData = 0x110002
        }

        public enum DsuState : byte
        {
            Disconnected = 0x00,
            Reserved = 0x01,
            Connected = 0x02
        }

        public enum DsuConnectionType : byte
        {
            None = 0x00,
            Usb = 0x01,
            Bluetooth = 0x02
        }

        public enum DsuModel : byte
        {
            None = 0,
            DualShock3 = 1,
            DualShock4 = 2,
            Generic = 3
        }

        public enum DsuBattery : byte
        {
            None = 0x00,
            Dying = 0x01,
            Low = 0x02,
            Medium = 0x03,
            High = 0x04,
            Full = 0x05,
            Charging = 0xEE,
            Charged = 0xEF
        }
    }
}
namespace LibraryShared
{
    public partial class Enums
    {
        public enum ControllerType : int
        {
            WinUsbDevice = 0,
            HidDevice = 1
        }

        public enum ControllerInputType : int
        {
            Unknown = 0,
            Invalid = 1,
            Input = 2,
            Status = 3
        }
    }
}
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
            Invalid = 0,
            Input = 1,
            Status = 2
        }
    }
}
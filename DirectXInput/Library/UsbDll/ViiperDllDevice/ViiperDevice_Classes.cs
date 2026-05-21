namespace LibraryUsb
{
    public partial class ViiperDllDevice
    {
        static class Xbox360Buttons
        {
            public const uint DPadUp = 0x0001;
            public const uint DPadDown = 0x0002;
            public const uint DPadLeft = 0x0004;
            public const uint DPadRight = 0x0008;
            public const uint Start = 0x0010;
            public const uint Back = 0x0020;
            public const uint LThumb = 0x0040;
            public const uint RThumb = 0x0080;
            public const uint LShoulder = 0x0100;
            public const uint RShoulder = 0x0200;
            public const uint Guide = 0x0400;
            public const uint A = 0x1000;
            public const uint B = 0x2000;
            public const uint X = 0x4000;
            public const uint Y = 0x8000;
        }
    }
}
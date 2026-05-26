using HIDMaestro;
using LibraryUsb;
using System.Windows.Media;
using static ArnoldVinkCode.ArnoldVinkSockets;
using static ArnoldVinkCode.AVActions;

namespace LibraryShared
{
    public partial class Classes
    {
        public class ControllerStatus
        {
            //Controller Basics
            public int NumberId = -1;
            public int NumberOutput = -1;
            public int NumberDisplay() { return NumberId + 1; }
            public bool Activated = false;

            //Controller Details
            public ControllerDetails Details = null;

            //Color Status
            public Color? Color = null;
            public bool ColorLedBlink = false;
            public byte ColorLedCurrentBrightness = 0;
            public byte ColorLedCurrentR = 0;
            public byte ColorLedCurrentG = 0;
            public byte ColorLedCurrentB = 0;
            public byte ColorLedPreviousBrightness = 0;
            public byte ColorLedPreviousR = 0;
            public byte ColorLedPreviousG = 0;
            public byte ColorLedPreviousB = 0;

            //Battery Status
            public ControllerBattery BatteryCurrent = new ControllerBattery();
            public ControllerBattery BatteryPrevious = new ControllerBattery();

            //Timeout Variables
            public bool TimeoutIgnore = false;
            public long TicksInputLast = 0;
            public long TicksInputPrev = 0;
            public long TicksActiveLast = 0;
            public int TicksTimeoutTarget = 3000;
            public int ReadFailureCount = 0;
            public int ReadFailureCountTarget = 200;

            //Signal Variables
            public long TicksSignalOne = 0;
            public long TicksSignalTwo = 0;
            public long TicksSignalThree = 0;
            public long TicksSignalFour = 0;
            public long TicksSignalFive = 0;
            public long TicksSignalSix = 0;

            //Controller Status
            public bool Disconnecting = false;
            public bool Connected()
            {
                try
                {
                    if (HidDevice != null && !HidDevice.Connected) { return false; }
                    else if (WinUsbDevice != null && !WinUsbDevice.Connected) { return false; }
                    else if (Details == null) { return false; }
                    else if (Disconnecting) { return false; }
                }
                catch { }
                return true;
            }

            //Controller Tasks
            public AVTaskDetails InputControllerTask = new AVTaskDetails("InputControllerTask");
            public AVTaskDetails OutputControllerTask = new AVTaskDetails("OutputControllerTask");
            public AVTaskDetails OutputGyroscopeTask = new AVTaskDetails("OutputGyroscopeTask");

            //WinUsb Device Variables
            public WinUsbDevice WinUsbDevice = null;

            //Hid Device Variables
            public HidDevice HidDevice = null;

            //Virtual Device Variables
            public HMController VirtualDevice = null;

            //Gyro Dsu Client Variables
            public uint GyroDsuClientPacketNumber = 0;
            public UdpEndPointDetails GyroDsuClientEndPoint = null;

            //Input and Output data
            public bool ControllerDataRead = false;
            public byte[] ControllerDataInput = null;
            public byte[] ControllerDataOutput = null;
            public byte[] VirtualDataInput = null;
            public byte[] VirtualDataOutput = null;

            //Controller Rumble
            public byte RumbleCurrentControllerHeavy = 0;
            public byte RumbleCurrentControllerLight = 0;
            public byte RumblePreviousControllerHeavy = 0;
            public byte RumblePreviousControllerLight = 0;

            //Trigger Rumble
            public long RumbleTicksTriggerLeft = 0;
            public long RumbleTicksTriggerRight = 0;
            public byte RumbleCurrentTriggerLeft = 0;
            public byte RumbleCurrentTriggerRight = 0;
            public byte RumblePreviousTriggerLeft = 0;
            public byte RumblePreviousTriggerRight = 0;

            //Controller Input
            public long Delay_CtrlUIOutput = 0;
            public long Delay_ControllerShortcut = 0;
            public ControllerInput InputCurrent = new ControllerInput();
            public ControllerSupported SupportedCurrent = new ControllerSupported();

            //Set used controller number
            public ControllerStatus(int numberId)
            {
                NumberId = numberId;
            }
        }
    }
}
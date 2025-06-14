﻿using LibraryUsb;
using System.Diagnostics;
using System.Windows.Media;
using static ArnoldVinkCode.ArnoldVinkSockets;
using static ArnoldVinkCode.AVActions;
using static LibraryUsb.VigemBusDevice;

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
            public int NumberVirtual() { return NumberId + VirtualIdOffset; }
            public bool Activated = false;

            //Controller Details
            public ControllerDetails Details = null;

            //Color Status
            public Color? Color = null;
            public bool ColorLedBlink = false;
            public byte ColorLedCurrentR = 0;
            public byte ColorLedCurrentG = 0;
            public byte ColorLedCurrentB = 0;
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
            public AVTaskDetails OutputVirtualTask = new AVTaskDetails("OutputVirtualTask");
            public AVTaskDetails OutputGyroscopeTask = new AVTaskDetails("OutputGyroscopeTask");

            //WinUsb Device Variables
            public WinUsbDevice WinUsbDevice = null;

            //Hid Device Variables
            public HidDevice HidDevice = null;

            //Gyro Dsu Client Variables
            public uint GyroDsuClientPacketNumber = 0;
            public UdpEndPointDetails GyroDsuClientEndPoint = null;

            //Input and Output data
            public bool ControllerDataRead = false;
            public byte[] ControllerDataInput = null;
            public byte[] ControllerDataOutput = null;
            public byte[] VirtualDataInput = new byte[(int)VigemBusDevice.ByteArraySizes.Input];
            public byte[] VirtualDataOutput = new byte[(int)VigemBusDevice.ByteArraySizes.Output];
            public byte RumbleCurrentHeavy = 0;
            public byte RumbleCurrentLight = 0;
            public byte RumblePreviousHeavy = 0;
            public byte RumblePreviousLight = 0;

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

            //Reset controller status to defaults
            public void ResetControllerStatus()
            {
                try
                {
                    Debug.WriteLine("Reset the controller status for controller: " + NumberId);

                    //Controller Basics
                    NumberOutput = -1;
                    Activated = false;

                    //Controller Details
                    Details = null;

                    //Color Status
                    ColorLedBlink = false;
                    ColorLedCurrentR = 0;
                    ColorLedCurrentG = 0;
                    ColorLedCurrentB = 0;
                    ColorLedPreviousR = 0;
                    ColorLedPreviousG = 0;
                    ColorLedPreviousB = 0;

                    //Battery Status
                    BatteryCurrent = new ControllerBattery();
                    BatteryPrevious = new ControllerBattery();

                    //Timeout Variables
                    TimeoutIgnore = false;
                    TicksInputPrev = 0;
                    TicksInputLast = 0;
                    TicksActiveLast = 0;
                    ReadFailureCount = 0;

                    //Controller Status
                    Disconnecting = false;

                    //Controller Tasks
                    InputControllerTask = new AVTaskDetails("InputControllerTask");
                    OutputControllerTask = new AVTaskDetails("OutputControllerTask");
                    OutputVirtualTask = new AVTaskDetails("OutputVirtualTask");
                    OutputGyroscopeTask = new AVTaskDetails("OutputGyroscopeTask");

                    //WinUsb Device Variables
                    WinUsbDevice = null;

                    //Hid Device Variables
                    HidDevice = null;

                    //Gyro Dsu Client Variables
                    GyroDsuClientPacketNumber = 0;
                    GyroDsuClientEndPoint = null;

                    //Device In and Output
                    ControllerDataRead = false;
                    ControllerDataInput = null;
                    ControllerDataOutput = null;
                    VirtualDataInput = new byte[(int)VigemBusDevice.ByteArraySizes.Input];
                    VirtualDataOutput = new byte[(int)VigemBusDevice.ByteArraySizes.Output];
                    RumbleCurrentHeavy = 0;
                    RumbleCurrentLight = 0;
                    RumblePreviousHeavy = 0;
                    RumblePreviousLight = 0;

                    //Controller Input
                    Delay_CtrlUIOutput = 0;
                    Delay_ControllerShortcut = 0;
                    InputCurrent = new ControllerInput();
                    SupportedCurrent = new ControllerSupported();
                }
                catch { }
            }
        }
    }
}
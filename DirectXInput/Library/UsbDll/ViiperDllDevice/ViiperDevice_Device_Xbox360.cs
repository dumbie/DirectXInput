using System;
using System.Diagnostics;
using static ArnoldVinkCode.AVInputOutputClass;
using static LibraryShared.Classes;

namespace LibraryUsb
{
    public partial class ViiperDllDevice
    {
        public UIntPtr Xbox360Create()
        {
            UIntPtr deviceHandle = 0;
            try
            {
                bool success = CreateXbox360Device(ServerHandle, out deviceHandle, BusIdentifier, true, 0, 0, 0);
                if (!success)
                {
                    Console.WriteLine("Failed to create Xbox 360 device.");
                    return deviceHandle;
                }
                else
                {
                    Xbox360RumbleCallbackDelegate rumbleCallback = Xbox360OutputCallback;
                    SetXbox360RumbleCallback(deviceHandle, rumbleCallback);

                    Debug.WriteLine("Created Xbox 360 device with handle: " + deviceHandle);
                    return deviceHandle;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to create Xbox 360 device: " + ex.Message);
                return deviceHandle;
            }
        }

        public bool Xbox360SetInput(UIntPtr deviceHandle, ref ControllerStatus controller)
        {
            try
            {
                //Create device state
                Xbox360DeviceState deviceState = new Xbox360DeviceState();

                //Thumb Left
                deviceState.LX = (short)controller.InputCurrent.ThumbLeftX;
                deviceState.LY = (short)controller.InputCurrent.ThumbLeftY;

                //Thumb Right
                deviceState.RX = (short)controller.InputCurrent.ThumbRightX;
                deviceState.RY = (short)controller.InputCurrent.ThumbRightY;

                //Triggers
                deviceState.LT = controller.InputCurrent.TriggerLeft;
                deviceState.RT = controller.InputCurrent.TriggerRight;

                //DPad
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.DPadLeft].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.DPadLeft; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.DPadUp].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.DPadUp; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.DPadRight].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.DPadRight; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.DPadDown].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.DPadDown; }

                //Buttons
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.A].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.A; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.B].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.B; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.X].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.X; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.Y].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.Y; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.Back].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.Back; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.Start].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.Start; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.Guide].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.Guide; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.ShoulderLeft].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.LShoulder; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.ShoulderRight].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.RShoulder; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.ThumbLeft].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.LThumb; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.ThumbRight].PressedRaw) { deviceState.Buttons |= (uint)Xbox360Buttons.RThumb; }

                //Set device state
                bool success = SetXbox360DeviceState(deviceHandle, deviceState);
                if (!success)
                {
                    Debug.WriteLine("Failed to set Xbox 360 input.");
                }

                //Return result
                return success;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to set Xbox 360 input: " + ex.Message);
                return false;
            }
        }

        public bool Xbox360ResetInput(UIntPtr deviceHandle, ref ControllerStatus controller)
        {
            try
            {
                //Create device state
                Xbox360DeviceState deviceState = new Xbox360DeviceState();

                //Set device state
                bool success = SetXbox360DeviceState(deviceHandle, deviceState);
                if (!success)
                {
                    Debug.WriteLine("Failed to reset Xbox 360 input.");
                }

                //Return result
                return success;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to reset Xbox 360 input: " + ex.Message);
                return false;
            }
        }

        private static void Xbox360OutputCallback(UIntPtr deviceHandle, byte leftMotor, byte rightMotor)
        {
            Debug.WriteLine("Rumble Left: " + leftMotor + " / Right: " + rightMotor);
        }
    }
}
using HIDMaestro;
using System;
using System.Diagnostics;
using static ArnoldVinkCode.AVInputOutputClass;
using static LibraryShared.Classes;

namespace LibraryUsb
{
    public partial class HidMaestroDllDevice
    {
        public HMController Xbox360Create()
        {
            try
            {
                //Get device profile
                HMProfile hmProfile = hidMaestroContext.GetProfile("xbox-360-wired");
                if (hmProfile == null)
                {
                    Debug.WriteLine("Xbox 360 device profile not found.");
                    return null;
                }

                //Create device controller
                HMController hmController = hidMaestroContext.CreateController(hmProfile);
                if (hmController == null)
                {
                    Debug.WriteLine("Failed to create Xbox 360 device.");
                    return null;
                }

                //Set output received event
                hmController.OutputReceived += (controller, packet) =>
                {
                    Debug.WriteLine($"[output] ctrl1 source={packet.Source} " + $"reportId=0x{packet.ReportId:X2} len={packet.Data.Length}");
                };

                //Return result
                Debug.WriteLine("Created Xbox 360 device");
                return hmController;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to create Xbox 360 device: " + ex.Message);
                return null;
            }
        }

        public bool Xbox360SetInput(HMController hmController, ControllerStatus controller)
        {
            try
            {
                //Create device state
                HMGamepadState hmDeviceState = new HMGamepadState();

                //Thumb Left
                float thumbLeftX = (float)(controller.InputCurrent.ThumbLeftX + 32767F) / 65534F;
                float thumbLeftY = 1F - (float)(controller.InputCurrent.ThumbLeftY + 32767F) / 65534F;

                //Thumb Right
                float thumbRightX = (float)(controller.InputCurrent.ThumbRightX + 32767F) / 65534F;
                float thumbRightY = 1F - (float)(controller.InputCurrent.ThumbRightY + 32767F) / 65534F;

                //Triggers
                float triggerLeft = (float)(controller.InputCurrent.TriggerLeft / 255F);
                float triggerRight = (float)(controller.InputCurrent.TriggerRight / 255F);

                //Set device axes
                hmDeviceState.Axes = HMGamepadStateHelpers.StandardAxes(hmController.Profile, thumbLeftX, thumbLeftY, thumbRightX, thumbRightY, triggerLeft, triggerRight);

                //DPad
                bool dpadLeft = controller.InputCurrent.Buttons[(byte)ControllerButtons.DPadLeft].PressedRaw;
                bool dpadUp = controller.InputCurrent.Buttons[(byte)ControllerButtons.DPadUp].PressedRaw;
                bool dpadRight = controller.InputCurrent.Buttons[(byte)ControllerButtons.DPadRight].PressedRaw;
                bool dpadDown = controller.InputCurrent.Buttons[(byte)ControllerButtons.DPadDown].PressedRaw;
                if (dpadUp && dpadRight) { hmDeviceState.Hat = HMHat.NorthEast; }
                else if (dpadUp && dpadLeft) { hmDeviceState.Hat = HMHat.NorthWest; }
                else if (dpadDown && dpadRight) { hmDeviceState.Hat = HMHat.SouthEast; }
                else if (dpadDown && dpadLeft) { hmDeviceState.Hat = HMHat.SouthWest; }
                else if (dpadUp) { hmDeviceState.Hat = HMHat.North; }
                else if (dpadDown) { hmDeviceState.Hat = HMHat.South; }
                else if (dpadLeft) { hmDeviceState.Hat = HMHat.West; }
                else if (dpadRight) { hmDeviceState.Hat = HMHat.East; }

                //Buttons
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.A].PressedRaw) { hmDeviceState.Buttons |= HMButton.A; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.B].PressedRaw) { hmDeviceState.Buttons |= HMButton.B; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.X].PressedRaw) { hmDeviceState.Buttons |= HMButton.X; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.Y].PressedRaw) { hmDeviceState.Buttons |= HMButton.Y; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.Back].PressedRaw) { hmDeviceState.Buttons |= HMButton.Back; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.Start].PressedRaw) { hmDeviceState.Buttons |= HMButton.Start; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.Guide].PressedRaw) { hmDeviceState.Buttons |= HMButton.Guide; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.ShoulderLeft].PressedRaw) { hmDeviceState.Buttons |= HMButton.LeftBumper; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.ShoulderRight].PressedRaw) { hmDeviceState.Buttons |= HMButton.RightBumper; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.ThumbLeft].PressedRaw) { hmDeviceState.Buttons |= HMButton.LeftStick; }
                if (controller.InputCurrent.Buttons[(byte)ControllerButtons.ThumbRight].PressedRaw) { hmDeviceState.Buttons |= HMButton.RightStick; }

                //Set device state
                hmController.SubmitState(hmDeviceState);

                //Return result
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to set Xbox 360 input: " + ex.Message);
                return false;
            }
        }

        public bool Xbox360ResetInput(HMController hmController)
        {
            try
            {
                //Create device state
                HMGamepadState hmDeviceState = new HMGamepadState();

                //Set device state
                hmController.SubmitState(hmDeviceState);

                //Return result
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to reset Xbox 360 input: " + ex.Message);
                return false;
            }
        }
    }
}
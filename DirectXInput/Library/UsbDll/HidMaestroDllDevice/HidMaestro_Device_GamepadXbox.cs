using HIDMaestro;
using System;
using System.Diagnostics;
using static ArnoldVinkCode.AVInputOutputClass;
using static LibraryShared.Classes;

namespace LibraryUsb
{
    public partial class HidMaestroDllDevice
    {
        public HMController GamepadXboxCreate()
        {
            try
            {
                //Get device profile
                //Note: Newer Xbox controllers might not be supported by older games like BF4
                //HMProfile hmProfile = hidMaestroContext.GetProfile("microsoft-xbox-360");
                HMProfile hmProfile = hidMaestroContext.GetProfile("microsoft-xbox-one-1537");
                if (hmProfile == null)
                {
                    Debug.WriteLine("Xbox gamepad device profile not found.");
                    return null;
                }

                //Create device controller
                HMController hmController = hidMaestroContext.CreateController(hmProfile);
                if (hmController == null)
                {
                    Debug.WriteLine("Failed to create Xbox gamepad device.");
                    return null;
                }

                //Return result
                Debug.WriteLine("Created Xbox gamepad device");
                return hmController;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to create Xbox gamepad device: " + ex.Message);
                return null;
            }
        }

        public bool GamepadXboxSetInput(HMController hmController, ControllerStatus controller)
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
                Debug.WriteLine("Failed to set Xbox gamepad input: " + ex.Message);
                return false;
            }
        }

        public bool GamepadXboxResetInput(HMController hmController)
        {
            try
            {
                //Create device state
                HMGamepadState hmDeviceState = new HMGamepadState();

                //Set device axes
                hmDeviceState.Axes = HMGamepadStateHelpers.StandardAxes(hmController.Profile, 0.5F, 0.5F, 0.5F, 0.5F, 0, 0);

                //Set device state
                hmController.SubmitState(hmDeviceState);

                //Return result
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to reset Xbox gamepad input: " + ex.Message);
                return false;
            }
        }
    }
}
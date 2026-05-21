using System;
using System.Diagnostics;
using static ArnoldVinkCode.AVInputOutputClass;

namespace LibraryUsb
{
    public partial class ViiperDllDevice
    {
        public UIntPtr MouseCreate()
        {
            UIntPtr deviceHandle = 0;
            try
            {
                bool success = CreateMouseDevice(ServerHandle, out deviceHandle, BusIdentifier, true, 0, 0);
                if (!success)
                {
                    Console.WriteLine("Failed to create mouse device.");
                    return deviceHandle;
                }
                else
                {
                    Debug.WriteLine("Created mouse device with handle: " + deviceHandle);
                    return deviceHandle;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to create mouse device: " + ex.Message);
                return deviceHandle;
            }
        }

        public bool MouseSetInputRelative(UIntPtr deviceHandle, MouseHidAction mouseAction)
        {
            try
            {
                //Create device state
                MouseDeviceState deviceState = new MouseDeviceState();
                deviceState.DX = (short)mouseAction.MoveHorizontal;
                deviceState.DY = (short)mouseAction.MoveVertical;
                deviceState.Wheel = (short)mouseAction.ScrollVertical;
                deviceState.Pan = (short)mouseAction.ScrollHorizontal;
                deviceState.Buttons = (byte)mouseAction.Button;

                //Set device state
                bool success = SetMouseDeviceState(deviceHandle, deviceState);
                if (!success)
                {
                    Debug.WriteLine("Failed to set mouse input.");
                }

                //Return result
                return success;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to set mouse input: " + ex.Message);
                return false;
            }
        }

        public bool MouseResetInput(UIntPtr deviceHandle, MouseHidAction mouseAction)
        {
            try
            {
                //Create device state
                MouseDeviceState deviceState = new MouseDeviceState();

                //Set device state
                bool success = SetMouseDeviceState(deviceHandle, deviceState);
                if (!success)
                {
                    Debug.WriteLine("Failed to reset mouse input.");
                }

                //Return result
                return success;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to reset mouse input: " + ex.Message);
                return false;
            }
        }
    }
}
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVActions;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.Enums;

namespace DirectXInput
{
    public partial class WindowMain
    {
        private async Task ControllerInputSend(ControllerStatus controller)
        {
            try
            {
                //Check if controller is connected
                if (!controller.Connected())
                {
                    controller.ReadFailureCount++;
                    Debug.WriteLine("Read input controller is not connected: " + controller.NumberId);
                    AVHighResDelay.Delay(0.1F);
                    return;
                }

                //Read data from the controller
                if (controller.Details.Type == ControllerType.HidDevice)
                {
                    if (!controller.HidDevice.ReadBytesFile(controller.ControllerDataInput))
                    {
                        controller.ReadFailureCount++;
                        Debug.WriteLine("Failed to read input data from hid controller: " + controller.NumberId);
                        AVHighResDelay.Delay(0.1F);
                        return;
                    }
                }
                else
                {
                    if (!controller.WinUsbDevice.ReadBytesIntPipe(controller.ControllerDataInput))
                    {
                        controller.ReadFailureCount++;
                        Debug.WriteLine("Failed to read input data from win controller: " + controller.NumberId);
                        AVHighResDelay.Delay(0.1F);
                        return;
                    }
                }

                //Check controller input data type
                ControllerInputType inputDataType = InputDataCheckType(controller);
                if (inputDataType == ControllerInputType.Invalid)
                {
                    controller.ReadFailureCount++;
                    Debug.WriteLine("Received invalid data from controller: " + controller.NumberId);
                    AVHighResDelay.Delay(0.1F);
                    return;
                }
                else if (inputDataType == ControllerInputType.Status)
                {
                    //Debug.WriteLine("Received status data from controller: " + controller.NumberId);

                    //Read controller battery level from input data
                    ControllerReadBatteryLevelInput(controller);

                    return;
                }
                else if (inputDataType == ControllerInputType.Unknown)
                {
                    //Debug.WriteLine("Received unknown data from controller: " + controller.NumberId);
                    return;
                }

                //Update read status
                controller.ControllerDataRead = true;

                //Update read failure count
                controller.ReadFailureCount = 0;

                //Update Identifiers
                InputUpdateIdentifiers(controller);

                //Update Thumbsticks
                InputUpdateThumbsticks(controller);

                //Update DPad
                InputUpdateDirectionalPad(controller);

                //Update Buttons
                InputUpdateButtons(controller);

                //Update Triggers
                InputUpdateTriggers(controller);

                //Update Touchpad
                InputUpdateTouchpad(controller);

                //Update Gyroscope
                InputUpdateGyroscope(controller);

                //Update Accelerometer
                InputUpdateAccelerometer(controller);

                //Update HandSensor
                InputUpdateHandSensor(controller);

                //Save controller button mapping
                if (ControllerMappingSave(controller))
                {
                    AVHighResDelay.Delay(0.1F);
                    return;
                }

                //Check controller button press times
                CheckControllerButtonPressTimes(controller);

                //Check if controller output needs to be blocked
                bool blockOutput = await CheckControllerBlockInteraction(controller);

                //Update controller input time
                long ticksSystem = GetSystemTicksMs();
                controller.TicksInputPrev = controller.TicksInputLast;
                controller.TicksInputLast = ticksSystem;

                //Check if controller is idle and update active time
                if (controller.BatteryCurrent.BatteryStatus == BatteryStatus.Charging || !CheckControllerIdlePress(controller))
                {
                    controller.TicksActiveLast = ticksSystem;
                }

                if (blockOutput)
                {
                    //Send empty input to virtual device
                    vVirtualBusDevice.Xbox360ResetInput(controller.VirtualDevice);
                }
                else
                {
                    //Check and overwrite controller button presses
                    CheckControllerButtonOverwrite(controller);

                    //Send current input to virtual device
                    vVirtualBusDevice.Xbox360SetInput(controller.VirtualDevice, controller);
                }
            }
            catch
            {
                Debug.WriteLine("DirectInput " + controller.Details.Type + " data report is out of range or empty, skipping.");
            }
        }
    }
}
using System.Diagnostics;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVActions;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Loop controller input
        async Task LoopInputController(ControllerStatus controller)
        {
            try
            {
                Debug.WriteLine("Handle controller input data " + controller.Details.Type + " for: " + controller.Details.DisplayName);

                //Receive input from the selected controller
                while (await TaskCheckLoop(controller.InputControllerTask, 0))
                {
                    try
                    {
                        await ControllerInputSend(controller);
                    }
                    catch { }
                }
            }
            catch { }
        }

        //Loop controller output
        async Task LoopOutputController(ControllerStatus controller)
        {
            try
            {
                Debug.WriteLine("Handle controller output data for: " + controller.Details.DisplayName);

                while (await TaskCheckLoop(controller.OutputControllerTask, 0.1F))
                {
                    try
                    {
                        //Check if output values have changed
                        bool ledBrightnessChanged = controller.ColorLedCurrentBrightness == controller.ColorLedPreviousBrightness;
                        bool ledRChanged = controller.ColorLedCurrentR == controller.ColorLedPreviousR;
                        bool ledGChanged = controller.ColorLedCurrentG == controller.ColorLedPreviousG;
                        bool ledBChanged = controller.ColorLedCurrentB == controller.ColorLedPreviousB;
                        bool ledMuteChanged = vControllerMuteLedCurrent == vControllerMuteLedPrevious;
                        bool heavyRumbleChanged = controller.RumbleCurrentControllerHeavy == controller.RumblePreviousControllerHeavy;
                        bool lightRumbleChanged = controller.RumbleCurrentControllerLight == controller.RumblePreviousControllerLight;
                        bool triggerLeftRumbleChanged = controller.RumbleCurrentTriggerLeft == controller.RumblePreviousTriggerLeft;
                        bool triggerRightRumbleChanged = controller.RumbleCurrentTriggerRight == controller.RumblePreviousTriggerRight;
                        if ((ledBrightnessChanged && ledRChanged && ledGChanged && ledBChanged && ledMuteChanged && heavyRumbleChanged && lightRumbleChanged && triggerLeftRumbleChanged && triggerRightRumbleChanged) == false)
                        {
                            //Update the previous output values
                            controller.ColorLedPreviousBrightness = controller.ColorLedCurrentBrightness;
                            controller.ColorLedPreviousR = controller.ColorLedCurrentR;
                            controller.ColorLedPreviousG = controller.ColorLedCurrentG;
                            controller.ColorLedPreviousB = controller.ColorLedCurrentB;
                            vControllerMuteLedPrevious = vControllerMuteLedCurrent;
                            controller.RumblePreviousControllerHeavy = controller.RumbleCurrentControllerHeavy;
                            controller.RumblePreviousControllerLight = controller.RumbleCurrentControllerLight;
                            controller.RumblePreviousTriggerLeft = controller.RumbleCurrentTriggerLeft;
                            controller.RumblePreviousTriggerRight = controller.RumbleCurrentTriggerRight;

                            //Send received output to controller
                            ControllerOutputSend(controller);
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }

        //Loop gyroscope output
        async Task LoopOutputGyro(ControllerStatus controller)
        {
            try
            {
                Debug.WriteLine("Handle controller gyroscope data for: " + controller.Details.DisplayName);

                //Send gyro motion to dsu client
                while (await TaskCheckLoop(controller.OutputGyroscopeTask, 0.1F))
                {
                    try
                    {
                        await SendGyroMotionController(controller);
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}
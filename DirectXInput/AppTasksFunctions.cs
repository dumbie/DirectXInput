using System.Threading.Tasks;
using static ArnoldVinkCode.AVActions;
using static DirectXInput.AppVariables;

namespace DirectXInput
{
    public partial class WindowMain
    {
        async Task vTaskLoop_UpdateWindowStatus()
        {
            try
            {
                while (await TaskCheckLoop(vTask_UpdateWindowStatus, 1000))
                {
                    UpdateWindowStatus();
                }
            }
            catch { }
        }

        async Task vTaskLoop_ControllerMonitor()
        {
            try
            {
                while (await TaskCheckLoop(vTask_ControllerMonitor, 2000))
                {
                    await MonitorController();
                    MonitorVolumeMute();
                }
            }
            catch { }
        }

        async Task vTaskLoop_ControllerDisconnect()
        {
            try
            {
                while (await TaskCheckLoop(vTask_ControllerDisconnect, 1000))
                {
                    await CheckAllControllersTimeout();
                    await CheckAllControllersIdle();
                }
            }
            catch { }
        }

        async Task vTaskLoop_ControllerLedColor()
        {
            try
            {
                while (await TaskCheckLoop(vTask_ControllerLedColor, 1000))
                {
                    //Controller update led color
                    ControllerLedColorUpdate(vController0);
                    ControllerLedColorUpdate(vController1);
                    ControllerLedColorUpdate(vController2);
                    ControllerLedColorUpdate(vController3);
                }
            }
            catch { }
        }

        async Task vTaskLoop_ControllerBattery()
        {
            try
            {
                while (await TaskCheckLoop(vTask_ControllerBattery, 2000))
                {
                    //Read controller battery level by polling
                    ControllerReadBatteryLevelPoll(vController0);
                    ControllerReadBatteryLevelPoll(vController1);
                    ControllerReadBatteryLevelPoll(vController2);
                    ControllerReadBatteryLevelPoll(vController3);

                    //Check controller low battery level
                    CheckAllControllersLowBattery(false);
                }
            }
            catch { }
        }

        async Task vTaskLoop_ControllerSignal()
        {
            try
            {
                while (await TaskCheckLoop(vTask_ControllerSignal, 1000))
                {
                    //Send signals to controller
                    ControllerSignal(vController0);
                    ControllerSignal(vController1);
                    ControllerSignal(vController2);
                    ControllerSignal(vController3);
                }
            }
            catch { }
        }

        async Task vTaskLoop_ControllerInformation()
        {
            try
            {
                while (await TaskCheckLoop(vTask_ControllerInformation, 100))
                {
                    UpdateControllerInformation();
                }
            }
            catch { }
        }
    }
}
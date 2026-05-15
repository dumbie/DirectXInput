using ArnoldVinkCode;
using ArnoldVinkStyles;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVActions;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.Enums;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Start and open desired controller
        async Task<bool> ControllerStartOpen(ControllerStatus controllerStatus, ControllerDetails controllerDetails)
        {
            try
            {
                //Set controller details
                controllerStatus.Details = controllerDetails;

                //Open selected controller
                if (!ControllerHandleOpen(controllerStatus))
                {
                    Debug.WriteLine("Failed starting and opening controller: " + controllerStatus.Details.DisplayName);

                    //Close controller handle
                    ControllerHandleClose(controllerStatus);

                    //Reset controller status to defaults
                    controllerStatus.ResetControllerStatus();

                    //Return result
                    return false;
                }

                //Set controller supported profile
                controllerStatus.SupportedCurrent = vDirectControllersSupported.FirstOrDefault(x => x.ProductIDs.Any(z => z.ToLower() == controllerStatus.Details.Profile.ProductID.ToLower() && x.VendorID.ToLower() == controllerStatus.Details.Profile.VendorID.ToLower()));
                if (controllerStatus.SupportedCurrent == null)
                {
                    Debug.WriteLine("Controller is missing supported profile: " + controllerStatus.Details.DisplayName + " / " + controllerStatus.Details.Profile.VendorID + " / " + controllerStatus.Details.Profile.ProductID);

                    //Close controller handle
                    ControllerHandleClose(controllerStatus);

                    //Reset controller status to defaults
                    controllerStatus.ResetControllerStatus();

                    //Return result
                    return false;
                }

                //Validate controller by status
                if (!ControllerValidateStatus(controllerStatus, controllerDetails))
                {
                    Debug.WriteLine("Controller open status invalid: " + controllerStatus.Details.DisplayName);

                    //Close controller handle
                    ControllerHandleClose(controllerStatus);

                    //Reset controller status to defaults
                    controllerStatus.ResetControllerStatus();

                    //Return result
                    return false;
                }

                //Check and set controller serial number
                ControllerReadSerialNumber(controllerStatus);

                //Allow controller in HidHide
                if (controllerStatus.Details.Type == ControllerType.HidDevice)
                {
                    await vHidHideDevice.ListDeviceAdd(controllerStatus.Details.DeviceInstanceId);
                }

                //Unplug and plugin virtual device
                bool virtualUnplug = await vVirtualBusDevice.VirtualUnplug(controllerStatus.NumberVirtual());
                bool virtualPlugin = await vVirtualBusDevice.VirtualPlugin(controllerStatus.NumberVirtual());
                Debug.WriteLine("Virtual device plugin result: " + virtualUnplug + " / " + virtualPlugin);

                //Set controller interface information
                string controllerNumberDisplay = controllerStatus.NumberDisplay().ToString();

                //Show controller connected notification
                NotificationDetails notificationDetailsConnected = new NotificationDetails();
                notificationDetailsConnected.Icon = "Controller";
                notificationDetailsConnected.Text = "Connected (" + controllerNumberDisplay + ")";
                notificationDetailsConnected.Color = controllerStatus.Color;
                vWindowOverlay.Notification_Show_Status(notificationDetailsConnected);
                AVDispatcherInvoke.DispatcherInvoke(delegate
                {
                    txt_Controller_Information.Text = "Connected controller " + controllerNumberDisplay + ": " + controllerStatus.Details.DisplayName;
                });

                //Update the controller interface settings
                ControllerUpdateSettingsInterface(controllerStatus);

                //Initialize controller
                ControllerInitialize(controllerStatus);

                //Controller update led color
                ControllerLedColor(controllerStatus);

                //Update controller last input time
                long ticksSystem = GetSystemTicksMs();
                controllerStatus.TicksInputPrev = ticksSystem;
                controllerStatus.TicksInputLast = ticksSystem;

                //Update controller last active time
                controllerStatus.TicksActiveLast = ticksSystem;

                //Start input controller task loop
                async Task TaskActionInputController()
                {
                    try
                    {
                        await LoopInputController(controllerStatus);
                    }
                    catch { }
                }
                AVActions.TaskStartLoop(TaskActionInputController, controllerStatus.InputControllerTask);

                //Start output controller task loop
                async Task TaskActionOutputController()
                {
                    try
                    {
                        await LoopOutputController(controllerStatus);
                    }
                    catch { }
                }
                AVActions.TaskStartLoop(TaskActionOutputController, controllerStatus.OutputControllerTask);

                //Start output virtual task loop
                async Task TaskActionOutputVirtual()
                {
                    try
                    {
                        await LoopOutputVirtual(controllerStatus);
                    }
                    catch { }
                }
                AVActions.TaskStartLoop(TaskActionOutputVirtual, controllerStatus.OutputVirtualTask);

                //Start output gyroscope task loop
                async Task TaskActionOutputGyro()
                {
                    try
                    {
                        await LoopOutputGyro(controllerStatus);
                    }
                    catch { }
                }
                AVActions.TaskStartLoop(TaskActionOutputGyro, controllerStatus.OutputGyroscopeTask);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed starting and opening controller: " + controllerDetails.DisplayName + " / " + ex.Message);
                return false;
            }
        }
    }
}
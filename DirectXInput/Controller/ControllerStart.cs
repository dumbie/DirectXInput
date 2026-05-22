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
        async Task<ControllerStatus> ControllerStartOpen(int controllerId, ControllerDetails controllerDetails)
        {
            try
            {
                //Create new controller status
                ControllerStatus controllerStatus = new ControllerStatus(controllerId);

                //Set controller details
                controllerStatus.Details = controllerDetails;

                //Set controller supported profile
                controllerStatus.SupportedCurrent = vDirectControllersSupported.FirstOrDefault(x => x.ProductIDs.Any(z => z.ToLower() == controllerStatus.Details.Profile.ProductID.ToLower() && x.VendorID.ToLower() == controllerStatus.Details.Profile.VendorID.ToLower()));
                if (controllerStatus.SupportedCurrent == null)
                {
                    Debug.WriteLine("Controller is missing supported profile: " + controllerStatus.Details.DisplayName + " / " + controllerStatus.Details.Profile.VendorID + " / " + controllerStatus.Details.Profile.ProductID);

                    //Return result
                    return null;
                }

                //Open selected controller
                if (!ControllerHandleOpen(controllerStatus))
                {
                    Debug.WriteLine("Failed starting and opening controller: " + controllerStatus.Details.DisplayName);

                    //Close controller handle
                    ControllerHandleClose(controllerStatus);

                    //Return result
                    return null;
                }

                //Validate controller by status
                if (!ControllerValidateStatus(controllerStatus, controllerDetails))
                {
                    Debug.WriteLine("Controller open status invalid: " + controllerStatus.Details.DisplayName);

                    //Close controller handle
                    ControllerHandleClose(controllerStatus);

                    //Return result
                    return null;
                }

                //Check and set controller serial number
                ControllerReadSerialNumber(controllerStatus);

                //Allow controller in HidHide
                if (controllerStatus.Details.Type == ControllerType.HidDevice)
                {
                    await vHidHideDevice.ListDeviceAdd(controllerStatus.Details.DeviceInstanceId);
                }

                //Disable and enable controller to make sure no other app is using it
                controllerStatus.HidDevice.DisableDevice();
                controllerStatus.HidDevice.EnableDevice();

                //Plugin virtual device
                controllerStatus.VirtualDevice = vVirtualBusDevice.Xbox360Create();
                Debug.WriteLine("Virtual device plugin result: " + (controllerStatus.VirtualDevice == null));

                //Virtual device output event
                controllerStatus.VirtualDevice.OutputReceived += (hmController, hmOutput) => InputUpdateVirtualRumble(hmController, hmOutput, controllerStatus);

                //Set controller interface information
                string controllerNumberDisplay = controllerStatus.NumberDisplay().ToString();

                //Show controller connected notification
                NotificationDetails notificationDetailsConnected = new NotificationDetails();
                notificationDetailsConnected.Icon = "Controller";
                notificationDetailsConnected.Text = "Connected (" + controllerNumberDisplay + ")";
                notificationDetailsConnected.Color = ControllerLedColorGet(controllerStatus.NumberId);
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
                ControllerLedColorUpdate(controllerStatus);

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

                //Return result
                return controllerStatus;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed starting and opening controller: " + controllerDetails.DisplayName + " / " + ex.Message);
                return null;
            }
        }
    }
}
using ArnoldVinkStyles;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVActions;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Stop and close desired controller
        private async Task<bool> ControllerStopClose(ControllerStatus controller, string disconnectInfo, string controllerInfo)
        {
            try
            {
                //Check if controller is connected
                if (!controller.Connected())
                {
                    Debug.WriteLine("Controller " + controller.NumberId + " is already disconnected.");
                    return false;
                }

                //Check if the controller is disconnecting
                if (controller.Disconnecting)
                {
                    Debug.WriteLine("Controller " + controller.NumberId + " is currently disconnecting.");
                    return false;
                }

                //Update controller disconnecting status
                controller.Disconnecting = true;

                //Get controller display number
                Debug.WriteLine("Disconnecting controller " + controller.NumberId + ": " + controller.Details.DisplayName);
                string controllerNumberDisplay = controller.NumberDisplay().ToString();

                //Show controller disconnect notification
                NotificationDetails notificationDetails = new NotificationDetails();
                notificationDetails.Icon = "Controller";
                if (string.IsNullOrWhiteSpace(disconnectInfo))
                {
                    notificationDetails.Text = "Disconnected (" + controllerNumberDisplay + ")";
                }
                else
                {
                    notificationDetails.Text = "Disconnected " + disconnectInfo + " (" + controllerNumberDisplay + ")";
                }
                notificationDetails.Color = ControllerLedColorGet(controller.NumberId);
                vWindowOverlay.Notification_Show_Status(notificationDetails);

                //Update user interface controller status
                AVDispatcherInvoke.DispatcherInvoke(delegate
                {
                    if (string.IsNullOrWhiteSpace(controllerInfo))
                    {
                        txt_Controller_Information.Text = "Disconnected controller " + controllerNumberDisplay + ": " + controller.Details.DisplayName;
                    }
                    else
                    {
                        txt_Controller_Information.Text = controllerInfo;
                    }

                    if (controller.NumberId == 0)
                    {
                        image_Controller0.Source = vImagePreloadIconControllerDark;
                        textblock_Controller0.Text = "No controller connected";
                        textblock_Controller0CodeName.Text = string.Empty;
                        ResetControllerDebugInformation();
                    }
                    else if (controller.NumberId == 1)
                    {
                        image_Controller1.Source = vImagePreloadIconControllerDark;
                        textblock_Controller1.Text = "No controller connected";
                        textblock_Controller1CodeName.Text = string.Empty;
                        ResetControllerDebugInformation();
                    }
                    else if (controller.NumberId == 2)
                    {
                        image_Controller2.Source = vImagePreloadIconControllerDark;
                        textblock_Controller2.Text = "No controller connected";
                        textblock_Controller2CodeName.Text = string.Empty;
                        ResetControllerDebugInformation();
                    }
                    else if (controller.NumberId == 3)
                    {
                        image_Controller3.Source = vImagePreloadIconControllerDark;
                        textblock_Controller3.Text = "No controller connected";
                        textblock_Controller3CodeName.Text = string.Empty;
                        ResetControllerDebugInformation();
                    }
                });

                //Stop controller loop tasks
                await TaskStopLoop(controller.InputControllerTask, 1000);
                await TaskStopLoop(controller.OutputControllerTask, 1000);
                await TaskStopLoop(controller.OutputGyroscopeTask, 1000);

                //Disconnect controller virtual
                if (vVirtualBusDevice != null && controller.VirtualDevice != null)
                {
                    controller.VirtualDevice.Dispose();
                }

                //Disconnect controller wireless
                ControllerDisconnectWireless(controller);

                //Close controller handle
                ControllerHandleClose(controller);

                //Reset controller status to defaults
                ControllerResetStatus(controller.NumberId);

                //Check if any controller is connected
                if (!ControllerAnyConnected())
                {
                    //Close open popups
                    if (vSettings.Load("KeyboardCloseNoController", typeof(bool)))
                    {
                        Debug.WriteLine("No controller connected closing open popups.");
                        await HideOpenPopups();
                    }
                }

                //Return result
                Debug.WriteLine("Successfully stopped DirectInput controller: " + controller.NumberId);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed stopping DirectInput controller: " + controller.NumberId + ": " + ex.Message);
                return false;
            }
        }

        //Stop and close all controllers
        async Task StopAllControllers()
        {
            try
            {
                await ControllerStopClose(vController0, "all", "Disconnected all controllers.");
                await ControllerStopClose(vController1, "all", "Disconnected all controllers.");
                await ControllerStopClose(vController2, "all", "Disconnected all controllers.");
                await ControllerStopClose(vController3, "all", "Disconnected all controllers.");
                Debug.WriteLine("Stopped all DirectInput controllers.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed stopping all DirectInput controllers: " + ex.Message);
            }
        }
    }
}
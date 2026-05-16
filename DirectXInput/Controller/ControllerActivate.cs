using ArnoldVinkStyles;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Check if there is an actived controller
        void ControllerCheckActivated()
        {
            try
            {
                //Debug.WriteLine("There is currently no actived controller.");
                ControllerStatus activeController = vActiveController();
                if (vController0.Connected() && activeController == null) { ControllerActivate(vController0); }
                else if (vController1.Connected() && activeController == null) { ControllerActivate(vController1); }
                else if (vController2.Connected() && activeController == null) { ControllerActivate(vController2); }
                else if (vController3.Connected() && activeController == null) { ControllerActivate(vController3); }
                else if (activeController == null)
                {
                    AVDispatcherInvoke.DispatcherInvoke(delegate
                    {
                        //Clear controller information
                        txt_ActiveControllerType.Text = "Type";
                        txt_ActiveControllerLatency.Text = "Latency";
                        txt_ActiveControllerBattery.Text = "Battery";
                        txt_ActiveControllerName.Text = "No controller";
                        txt_ActiveControllerName.Foreground = (SolidColorBrush)Application.Current.Resources["ApplicationAccentLightBrush"];

                        //Disable controller tab
                        grid_Controller.IsEnabled = false;
                    });
                }
            }
            catch { }
        }

        //Activate controller
        bool ControllerActivate(ControllerStatus Controller)
        {
            try
            {
                if (Controller.Connected() && !Controller.Activated)
                {
                    Debug.WriteLine("Activating controller: " + Controller.NumberId);

                    ControllerStatus activeController = vActiveController();
                    if (activeController != null)
                    {
                        //Deactivate previous controller
                        activeController.Activated = false;

                        //Show controller activated notification
                        string controllerNumberDisplay = Controller.NumberDisplay().ToString();
                        NotificationDetails notificationDetails = new NotificationDetails();
                        notificationDetails.Icon = "Controller";
                        notificationDetails.Text = "Activated (" + controllerNumberDisplay + ")";
                        notificationDetails.Color = Controller.Color;
                        vWindowOverlay.Notification_Show_Status(notificationDetails);
                    }

                    //Activate current controller
                    Controller.Activated = true;

                    //Update settings interface
                    ControllerUpdateSettingsInterface(Controller);
                    return true;
                }
            }
            catch { }
            return false;
        }

        //Change the active controller to 0
        void Button_Controller0_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ControllerActivate(vController0);
            }
            catch { }
        }

        //Change the active controller to 1
        void Button_Controller1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ControllerActivate(vController1);
            }
            catch { }
        }

        //Change the active controller to 2
        void Button_Controller2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ControllerActivate(vController2);
            }
            catch { }
        }

        //Change the active controller to 3
        void Button_Controller3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ControllerActivate(vController3);
            }
            catch { }
        }
    }
}
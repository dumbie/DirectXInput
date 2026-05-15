using ArnoldVinkStyles;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static ArnoldVinkCode.AVAudioDevice;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Monitor connected controllers
        async Task MonitorController()
        {
            try
            {
                //Check if a controller is disconnecting
                if (vControllerAnyDisconnecting())
                {
                    Debug.WriteLine("A controller is disconnecting, delaying monitor.");
                    return;
                }

                //Load all the connected controllers
                await ControllerReceiveAllConnected();

                //Check if there is an active controller
                ControllerCheckActivated();
            }
            catch { }
        }

        //Monitor volume mute status
        void MonitorVolumeMute()
        {
            try
            {
                int muteFunction = vSettings.Load("ControllerLedCondition", typeof(int));
                if (muteFunction == 0)
                {
                    vControllerMuteLedCurrent = AudioMuteGetStatus(true);
                }
                else
                {
                    vControllerMuteLedCurrent = AudioMuteGetStatus(false);
                }
            }
            catch { }
        }

        //Connect with the controller
        async Task ControllerConnect(ControllerDetails controllerDetails)
        {
            try
            {
                //Check if the controller is already in use
                bool ControllerInuse = false;
                if (vController0.Connected() && vController0.Details.DevicePath == controllerDetails.DevicePath) { ControllerInuse = true; }
                if (vController1.Connected() && vController1.Details.DevicePath == controllerDetails.DevicePath) { ControllerInuse = true; }
                if (vController2.Connected() && vController2.Details.DevicePath == controllerDetails.DevicePath) { ControllerInuse = true; }
                if (vController3.Connected() && vController3.Details.DevicePath == controllerDetails.DevicePath) { ControllerInuse = true; }
                if (ControllerInuse) { return; }

                Debug.WriteLine("Found a connected " + controllerDetails.Type + " controller to use: " + controllerDetails.DisplayName);

                //Connect the controller to available slot
                if (!vController0.Connected())
                {
                    bool controllerStarted = await ControllerStartOpen(vController0, controllerDetails);
                    if (controllerStarted)
                    {
                        AVDispatcherInvoke.DispatcherInvoke(delegate
                        {
                            image_Controller0.Source = vImagePreloadIconControllerAccent;
                            textblock_Controller0.Text = vController0.Details.DisplayName;
                            textblock_Controller0CodeName.Text = vController0.SupportedCurrent.CodeName;
                        });
                    }
                }
                else if (!vController1.Connected())
                {
                    bool controllerStarted = await ControllerStartOpen(vController1, controllerDetails);
                    if (controllerStarted)
                    {
                        AVDispatcherInvoke.DispatcherInvoke(delegate
                        {
                            image_Controller1.Source = vImagePreloadIconControllerAccent;
                            textblock_Controller1.Text = vController1.Details.DisplayName;
                            textblock_Controller1CodeName.Text = vController1.SupportedCurrent.CodeName;
                        });
                    }
                }
                else if (!vController2.Connected())
                {
                    bool controllerStarted = await ControllerStartOpen(vController2, controllerDetails);
                    if (controllerStarted)
                    {
                        AVDispatcherInvoke.DispatcherInvoke(delegate
                        {
                            image_Controller2.Source = vImagePreloadIconControllerAccent;
                            textblock_Controller2.Text = vController2.Details.DisplayName;
                            textblock_Controller2CodeName.Text = vController2.SupportedCurrent.CodeName;
                        });
                    }
                }
                else if (!vController3.Connected())
                {
                    bool controllerStarted = await ControllerStartOpen(vController3, controllerDetails);
                    if (controllerStarted)
                    {
                        AVDispatcherInvoke.DispatcherInvoke(delegate
                        {
                            image_Controller3.Source = vImagePreloadIconControllerAccent;
                            textblock_Controller3.Text = vController3.Details.DisplayName;
                            textblock_Controller3CodeName.Text = vController3.SupportedCurrent.CodeName;
                        });
                    }
                }
            }
            catch { }
        }

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

        //Save the previous combobox selected item
        void Cb_Controller_Mouse_Down(object sender, EventArgs args)
        {
            try
            {
                ComboBox SelectedComboBox = (ComboBox)sender;
                vComboboxIndexPrev = SelectedComboBox.SelectedIndex;
            }
            catch { }
        }
    }
}
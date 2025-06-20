﻿using ArnoldVinkCode;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using static ArnoldVinkCode.AVClasses;
using static ArnoldVinkCode.AVSettings;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain : Window
    {
        //Window Initialize
        public WindowMain() { InitializeComponent(); }

        //Window Variables
        private IntPtr vInteropWindowHandle = IntPtr.Zero;

        //Window Initialized
        protected override void OnSourceInitialized(EventArgs e)
        {
            try
            {
                //Get interop window handle
                vInteropWindowHandle = new WindowInteropHelper(this).EnsureHandle();
            }
            catch { }
        }

        //Bind the lists to the listbox elements
        void ListBoxBindLists()
        {
            try
            {
                combobox_KeyboardTextString.ItemsSource = vDirectKeyboardTextList;
                combobox_KeyboardTextString.DisplayMemberPath = "String1";
                combobox_KeyboardTextString.SelectedIndex = 0;

                combobox_KeypadProcessProfile.ItemsSource = vDirectKeypadMapping;
                combobox_KeypadProcessProfile.DisplayMemberPath = "Name";
                combobox_KeypadProcessProfile.SelectedIndex = 0;

                listbox_LiveDebugInput.ItemsSource = vControllerDebugInput;
                ResetControllerDebugInformation();

                ListboxLoadIgnoredController();

                Debug.WriteLine("Lists bound to interface.");
            }
            catch { }
        }

        //Enable the socket server
        private async Task EnableSocketServer()
        {
            try
            {
                int socketServerPort = SettingLoad(vConfigurationDirectXInput, "ServerPort", typeof(int));
                vArnoldVinkSockets = new ArnoldVinkSockets("127.0.0.1", socketServerPort, false, true);
                vArnoldVinkSockets.vSocketTimeout = 250;
                vArnoldVinkSockets.EventBytesReceived += ReceivedSocketHandler;
                await vArnoldVinkSockets.SocketServerEnable();
            }
            catch { }
        }

        //Test the rumble button
        async void Btn_TestRumble_Click(object sender, RoutedEventArgs args)
        {
            try
            {
                ControllerStatus activeController = vActiveController();
                if (activeController != null)
                {
                    if (!vControllerRumbleTest)
                    {
                        vControllerRumbleTest = true;
                        Button SendButton = sender as Button;

                        //Enable rumble
                        if (SendButton.Name == "btn_RumbleTestLight")
                        {
                            activeController.RumbleCurrentHeavy = 0;
                            activeController.RumbleCurrentLight = 255;
                        }
                        else
                        {
                            activeController.RumbleCurrentHeavy = 255;
                            activeController.RumbleCurrentLight = 0;
                        }

                        //Wait rumble
                        await Task.Delay(1000);

                        //Disable rumble
                        activeController.RumbleCurrentHeavy = 0;
                        activeController.RumbleCurrentLight = 0;

                        vControllerRumbleTest = false;
                    }
                }
                else
                {
                    NotificationDetails notificationDetails = new NotificationDetails();
                    notificationDetails.Icon = "Controller";
                    notificationDetails.Text = "No controller connected";
                    vWindowOverlay.Notification_Show_Status(notificationDetails);
                }
            }
            catch { }
        }

        //Close other running controller tools
        void CloseControllerTools()
        {
            try
            {
                Debug.WriteLine("Closing other running controller tools.");
                foreach (ProfileShared closeTool in vDirectCloseTools)
                {
                    try
                    {
                        AVProcess.Close_ProcessesByName(closeTool.String1, true);
                    }
                    catch { }
                }
            }
            catch { }
        }

        //Application Close Handler
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                this.Hide();
            }
            catch { }
        }
    }
}
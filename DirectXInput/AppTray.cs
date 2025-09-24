using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static ArnoldVinkCode.AVFunctions;
using static DirectXInput.AppVariables;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Tray Menu Variables
        public static NotifyIcon TrayNotifyIcon = new NotifyIcon();
        public static ContextMenuStrip TrayContextMenu = new ContextMenuStrip();

        //Create the application tray menu
        void Application_CreateTrayMenu()
        {
            try
            {
                Debug.WriteLine("Creating application tray menu.");

                //Create a context menu for systray.
                TrayContextMenu.Items.Add("Show Keyboard", null, NotifyIcon_Keyboard);
                TrayContextMenu.Items.Add("-");
                TrayContextMenu.Items.Add("Launch CtrlUI", null, NotifyIcon_CtrlUI);
                TrayContextMenu.Items.Add("Launch FpsOverlayer", null, NotifyIcon_FpsOverlayer);
                TrayContextMenu.Items.Add("Launch ScreenCapy", null, NotifyIcon_ScreenCapy);
                TrayContextMenu.Items.Add("-");
                TrayContextMenu.Items.Add("Re/disconnect all controllers", null, NotifyIcon_DisconnectAll);
                TrayContextMenu.Items.Add("-");
                TrayContextMenu.Items.Add("Settings", null, NotifyIcon_Settings);
                TrayContextMenu.Items.Add("Website", null, NotifyIcon_Website);
                TrayContextMenu.Items.Add("Exit", null, NotifyIcon_Exit);

                //Initialize the tray notify icon. 
                TrayNotifyIcon.Text = "DirectXInput";
                TrayNotifyIcon.Icon = new Icon(AVEmbedded.EmbeddedResourceToStream(null, "DirectXInput.Assets.AppIcon.ico"));

                //Handle Middle Click event
                TrayNotifyIcon.MouseUp += NotifyIcon_MouseUp;

                //Handle Double Click event
                TrayNotifyIcon.DoubleClick += NotifyIcon_DoubleClick;

                //Add menu to tray icon and show it.  
                TrayNotifyIcon.ContextMenuStrip = TrayContextMenu;
                TrayNotifyIcon.Visible = true;
            }
            catch { }
        }

        void NotifyIcon_DoubleClick(object sender, EventArgs args)
        {
            try
            {
                vWindowMain.Show();
            }
            catch { }
        }

        void NotifyIcon_Settings(object sender, EventArgs args)
        {
            try
            {
                vWindowMain.Show();
            }
            catch { }
        }

        async void NotifyIcon_Keyboard(object sender, EventArgs args)
        {
            try
            {
                await KeyboardPopupHideShow(true, false);
            }
            catch { }
        }

        async void NotifyIcon_CtrlUI(object sender, EventArgs args)
        {
            try
            {
                await ToolFunctions.CtrlUI_LaunchShow();
            }
            catch { }
        }

        void NotifyIcon_FpsOverlayer(object sender, EventArgs args)
        {
            try
            {
                ProcessLaunch.LaunchFpsOverlayer(false);
            }
            catch { }
        }

        void NotifyIcon_ScreenCapy(object sender, EventArgs args)
        {
            try
            {
                ProcessLaunch.LaunchScreenCapy(false);
            }
            catch { }
        }

        void NotifyIcon_Website(object sender, EventArgs args)
        {
            try
            {
                OpenWebsiteBrowser("https://projects.arnoldvink.com");
            }
            catch { }
        }

        async void NotifyIcon_DisconnectAll(object sender, EventArgs args)
        {
            try
            {
                await StopAllControllers();
            }
            catch { }
        }

        async void NotifyIcon_MouseUp(object sender, MouseEventArgs args)
        {
            try
            {
                if (args.Button == MouseButtons.Middle)
                {
                    await StopAllControllers();
                }
            }
            catch { }
        }

        async void NotifyIcon_Exit(object sender, EventArgs args)
        {
            try
            {
                await Application_Exit_Prompt();
            }
            catch { }
        }
    }
}
using ArnoldVinkCode;
using System.Diagnostics;
using System.Threading.Tasks;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.SoundPlayer;

namespace DirectXInput
{
    partial class ProcessLaunch
    {
        //Launch CtrlUI
        public static async Task LaunchCtrlUI(bool silentLaunch)
        {
            try
            {
                Debug.WriteLine("Launching CtrlUI.");

                //Show notification
                if (!silentLaunch)
                {
                    NotificationDetails notificationDetails = new NotificationDetails();
                    notificationDetails.Icon = "Controller";
                    notificationDetails.Text = "Launching CtrlUI";
                    vWindowOverlay.Notification_Show_Status(notificationDetails);
                }

                //Launch application
                await AVTaskScheduler.TaskRun("ArnoldVink_CtrlUI", "CtrlUI", silentLaunch);
            }
            catch { }
        }

        //Launch Fps Overlayer
        public static async Task LaunchFpsOverlayer(bool silentLaunch)
        {
            try
            {
                Debug.WriteLine("Launching Fps Overlayer");

                //Show notification
                if (!silentLaunch)
                {
                    NotificationDetails notificationDetails = new NotificationDetails();
                    notificationDetails.Icon = "Fps";
                    notificationDetails.Text = "Launching Fps Overlayer";
                    vWindowOverlay.Notification_Show_Status(notificationDetails);
                }

                //Launch application
                await AVTaskScheduler.TaskRun("ArnoldVink_FpsOverlayer", "Fps Overlayer", silentLaunch);
            }
            catch { }
        }

        //Launch Screen Capture Tool
        public static async Task LaunchScreenCaptureTool(bool silentLaunch)
        {
            try
            {
                Debug.WriteLine("Launching Screen Capture Tool");

                //Show notification
                if (!silentLaunch)
                {
                    NotificationDetails notificationDetails = new NotificationDetails();
                    notificationDetails.Icon = "Screenshot";
                    notificationDetails.Text = "Launching Screen Capture Tool";
                    vWindowOverlay.Notification_Show_Status(notificationDetails);
                }

                //Launch application
                await AVTaskScheduler.TaskRun("ArnoldVink_ScreenCaptureTool", "Screen Capture Tool", silentLaunch);
            }
            catch { }
        }

        //Launch Xbox Game Bar
        public static void LaunchXboxGameBar()
        {
            try
            {
                //Play interface sound
                PlayInterfaceSound(vConfigurationDirectXInput, "PopupOpen", false, true);

                //Launch application
                AVProcess.Launch_UwpApplication("Microsoft.XboxGamingOverlay_8wekyb3d8bbwe!App", string.Empty);
            }
            catch { }
        }
    }
}
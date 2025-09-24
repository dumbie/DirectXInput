using ArnoldVinkCode;
using System.Diagnostics;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.SoundPlayer;

namespace DirectXInput
{
    partial class ProcessLaunch
    {
        //Launch CtrlUI
        public static void LaunchCtrlUI(bool silentLaunch)
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
                AVTaskScheduler.TaskRun("ArnoldVink_CtrlUI", "CtrlUI", silentLaunch);
            }
            catch { }
        }

        //Launch FpsOverlayer
        public static void LaunchFpsOverlayer(bool silentLaunch)
        {
            try
            {
                Debug.WriteLine("Launching FpsOverlayer");

                //Show notification
                if (!silentLaunch)
                {
                    NotificationDetails notificationDetails = new NotificationDetails();
                    notificationDetails.Icon = "Fps";
                    notificationDetails.Text = "Launching FpsOverlayer";
                    vWindowOverlay.Notification_Show_Status(notificationDetails);
                }

                //Launch application
                AVTaskScheduler.TaskRun("ArnoldVink_FpsOverlayer", "FpsOverlayer", silentLaunch);
            }
            catch { }
        }

        //Launch ScreenCapy
        public static void LaunchScreenCapy(bool silentLaunch)
        {
            try
            {
                Debug.WriteLine("Launching ScreenCapy");

                //Show notification
                if (!silentLaunch)
                {
                    NotificationDetails notificationDetails = new NotificationDetails();
                    notificationDetails.Icon = "Screenshot";
                    notificationDetails.Text = "Launching ScreenCapy";
                    vWindowOverlay.Notification_Show_Status(notificationDetails);
                }

                //Launch application
                AVTaskScheduler.TaskRun("ArnoldVink_ScreenCapy", "ScreenCapy", silentLaunch);
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
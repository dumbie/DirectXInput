using ArnoldVinkStyles;
using System;
using System.Windows;
using System.Windows.Media;
using static ArnoldVinkStyles.AVImage;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;

namespace DirectXInput.OverlayCode
{
    public partial class WindowOverlay : Window
    {
        //Show notification
        public void Notification_Show_Status(string icon, string text)
        {
            try
            {
                NotificationDetails notificationDetails = new NotificationDetails();
                notificationDetails.Icon = icon;
                notificationDetails.Text = text;
                Notification_Show_Status(notificationDetails);
            }
            catch { }
        }

        //Show notification
        public void Notification_Show_Status(NotificationDetails notificationDetails)
        {
            try
            {
                //Update notification
                AVDispatcherInvoke.DispatcherInvoke(delegate
                {
                    try
                    {
                        //Set notification text
                        grid_Message_Status_Image.Source = FileToBitmapImage(new string[] { notificationDetails.Icon, "Assets/Default/Icons/" + notificationDetails.Icon + ".png" }, null, vImageBackupSource, -1, -1, IntPtr.Zero, 0);
                        grid_Message_Status_Text.Text = notificationDetails.Text;
                        if (notificationDetails.Color != null)
                        {
                            grid_Message_Status_Border.Background = new SolidColorBrush((Color)notificationDetails.Color);
                        }
                        else
                        {
                            grid_Message_Status_Border.Background = (SolidColorBrush)Application.Current.Resources["ApplicationAccentLightBrush"];
                        }

                        //Show notification
                        Show();
                    }
                    catch { }
                });

                //Start notification timer
                vTimerOverlay.Interval = 3000;
                vTimerOverlay.TickSet = delegate
                {
                    try
                    {
                        AVDispatcherInvoke.DispatcherInvoke(delegate
                        {
                            //Hide notification
                            Hide();
                        });
                    }
                    catch { }
                };
                vTimerOverlay.Start();
            }
            catch { }
        }
    }
}
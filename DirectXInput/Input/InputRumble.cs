using HIDMaestro;
using System;
using System.Diagnostics;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        private void InputUpdateVirtualRumble(HMController hmController, HMOutputPacket hmOutputPacket, ControllerStatus controllerStatus)
        {
            try
            {
                //Note: Games with native Xbox Impulse triggers
                //UWP Forza Apex, Forza Horizon 3, ReCore, Gears of War 4
                //Win F1 2018, Shadow of the Tomb Raider

                //Convert data to array
                byte[] hmOutputPacketData = hmOutputPacket.Data.ToArray();

                //Microsoft Xbox 360
                if (hmController.Profile.Id == "microsoft-xbox-360")
                {
                    //Light rumble
                    controllerStatus.RumbleCurrentControllerLight = hmOutputPacketData[3];

                    //Heavy Rumble
                    controllerStatus.RumbleCurrentControllerHeavy = hmOutputPacketData[2];
                }
                //Microsoft Xbox One
                else if (hmController.Profile.Id == "microsoft-xbox-one-1537")
                {
                    //Rumble Trigger Left
                    controllerStatus.RumbleCurrentTriggerLeft = (byte)(hmOutputPacketData[0] * 2.55);

                    //Rumble Trigger Right
                    controllerStatus.RumbleCurrentTriggerRight = (byte)(hmOutputPacketData[1] * 2.55);

                    //Rumble Controller Light
                    controllerStatus.RumbleCurrentControllerLight = (byte)(hmOutputPacketData[3] * 2.55);

                    //Rumble Controller Heavy
                    controllerStatus.RumbleCurrentControllerHeavy = (byte)(hmOutputPacketData[2] * 2.55);
                }

                //string rumbleData = string.Join(",", hmOutputPacketData);
                //Debug.WriteLine("Updated rumble virtual input: " + hmController.Profile.Id + " / " + rumbleData);
                //Debug.WriteLine("Updated rumble virtual input: TL" + controllerStatus.RumbleCurrentTriggerLeft + " / TR" + controllerStatus.RumbleCurrentTriggerRight + " / L" + controllerStatus.RumbleCurrentControllerLight + " / H" + controllerStatus.RumbleCurrentControllerHeavy);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update rumble virtual input: " + ex.Message);
            }
        }
    }
}
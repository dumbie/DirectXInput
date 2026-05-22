using HIDMaestro;
using System;
using System.Diagnostics;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        private static void InputUpdateVirtualRumble(HMController hmController, HMOutputPacket hmOutputPacket, ControllerStatus controllerStatus)
        {
            try
            {
                //Convert data to array
                byte[] hmOutputPacketData = hmOutputPacket.Data.ToArray();

                //Microsoft Xbox 360
                if (hmController.Profile.Id == "xbox-360-wired")
                {
                    //Light rumble      
                    controllerStatus.RumbleCurrentLight = hmOutputPacketData[2];

                    //Heavy Rumble
                    controllerStatus.RumbleCurrentHeavy = hmOutputPacketData[3];
                }
                //Microsoft Xbox Series X|S
                else if (hmController.Profile.Id == "xbox-series-xs")
                {
                    //Rumble Trigger Left
                    //[0]

                    //Rumble Trigger Right
                    //[1]

                    //Rumble Controller Light
                    controllerStatus.RumbleCurrentLight = (byte)(hmOutputPacketData[2] * 2.55);

                    //Rumble Controller Heavy
                    controllerStatus.RumbleCurrentHeavy = (byte)(hmOutputPacketData[3] * 2.55);
                }

                string rumbleData = string.Join(",", hmOutputPacketData);
                Debug.WriteLine("Updated rumble virtual input: " + hmController.Profile.Id + " / " + rumbleData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update rumble virtual input: " + ex.Message);
            }
        }
    }
}
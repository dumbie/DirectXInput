using System;
using System.Diagnostics;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        private static bool InputUpdateHandSensor(ControllerStatus controller)
        {
            try
            {
                if (controller.SupportedCurrent.OffsetHeader.HandSensor != null)
                {
                    //Get controller header offset
                    int headerOffset = controller.Details.ConnectionTypeOffset(controller.SupportedCurrent);

                    if (controller.SupportedCurrent.CodeName == "SteamController2026")
                    {
                        byte handSensorByte = controller.ControllerDataInput[headerOffset + (int)controller.SupportedCurrent.OffsetHeader.HandSensor];
                        bool handSensorRight = (handSensorByte & (1 << 4)) != 0;
                        bool handSensorLeft = (handSensorByte & (1 << 5)) != 0;
                        controller.InputCurrent.HandSensorLeft = handSensorLeft;
                        controller.InputCurrent.HandSensorRight = handSensorRight;
                    }

                    //Debug.WriteLine("HandSensor: Left " + controller.InputCurrent.HandSensorLeft + " / Right " + controller.InputCurrent.HandSensorRight);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update HandSensor input: " + ex.Message);
                return false;
            }
        }
    }
}
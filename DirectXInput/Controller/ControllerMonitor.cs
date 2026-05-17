using System.Diagnostics;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVAudioDevice;
using static DirectXInput.AppVariables;

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
                if (ControllerAnyDisconnecting())
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
    }
}
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using static ArnoldVinkCode.ArnoldVinkSockets;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVClassConverters;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.ControllerTimings;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Send controller output to CtrlUI
        async Task OutputAppCtrlUI(ControllerStatus controller)
        {
            try
            {
                if (GetSystemTicksMilli() >= controller.Delay_CtrlUIOutput)
                {
                    //Check if socket server is running
                    if (vArnoldVinkSockets == null)
                    {
                        Debug.WriteLine("The socket server is not running.");
                        return;
                    }

                    //Prepare socket data
                    SocketSendContainer socketSend = new SocketSendContainer();
                    socketSend.SourceIp = vArnoldVinkSockets.vSocketServerIp;
                    socketSend.SourcePort = vArnoldVinkSockets.vSocketServerPort;
                    socketSend.SetObject(controller.InputCurrent);
                    byte[] SerializedData = SerializeObjectToBytes(socketSend);

                    //Send socket data
                    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(vArnoldVinkSockets.vSocketServerIp), 26759);
                    await vArnoldVinkSockets.UdpClientSendBytesServer(ipEndPoint, SerializedData, vArnoldVinkSockets.vSocketTimeout);

                    //Update delay time
                    controller.Delay_CtrlUIOutput = GetSystemTicksMilli() + vControllerDelayTicks10;
                }
            }
            catch { }
        }
    }
}
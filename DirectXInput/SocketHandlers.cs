using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using static ArnoldVinkCode.ArnoldVinkSockets;
using static ArnoldVinkCode.AVClassConverters;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;

namespace DirectXInput
{
    partial class WindowMain
    {
        //Handle received socket data
        public void ReceivedSocketHandler(TcpClient tcpClient, UdpEndPointDetails endPoint, byte[] receivedBytes)
        {
            try
            {
                async Task TaskAction()
                {
                    try
                    {
                        if (tcpClient != null)
                        {
                            //await ReceivedTcpSocketHandlerThread(tcpClient, receivedBytes);
                        }
                        else
                        {
                            await ReceivedUdpSocketHandlerThread(endPoint, receivedBytes);
                        }
                    }
                    catch { }
                }
                AVActions.TaskStartBackground(TaskAction);
            }
            catch { }
        }

        async Task ReceivedUdpSocketHandlerThread(UdpEndPointDetails endPoint, byte[] receivedBytes)
        {
            try
            {
                //Get the source server ip and port
                //Debug.WriteLine("Received udp socket from: " + endPoint.IPEndPoint.Address.ToString() + ":" + endPoint.IPEndPoint.Port + "/" + receivedBytes.Length + "bytes");

                //Check incoming gyro dsu bytes
                if (await GyroDsuClientHandler(endPoint, receivedBytes))
                {
                    return;
                }

                //Deserialize the received bytes
                if (DeserializeBytesToObject(receivedBytes, out SocketSendContainer deserializedBytes))
                {
                    Type objectType = Type.GetType(deserializedBytes.SendType);
                    if (objectType == typeof(NotificationDetails))
                    {
                        NotificationDetails receivedNotificationDetails = deserializedBytes.GetObjectAsType<NotificationDetails>();
                        vWindowOverlay.Notification_Show_Status(receivedNotificationDetails);
                    }
                    else if (objectType == typeof(string))
                    {
                        string receivedString = (string)deserializedBytes.SendObject;
                        Debug.WriteLine("Received socket string: " + receivedString);
                        if (receivedString == "ControllerStatusSummaryList")
                        {
                            await SendControllerStatusDetailsList(deserializedBytes.SourceIp, deserializedBytes.SourcePort);
                        }
                        else if (receivedString == "KeyboardHideShow")
                        {
                            await KeyboardPopupHideShow(false, false);
                        }
                        else if (receivedString == "KeyboardShow")
                        {
                            await KeyboardPopupHideShow(true, true);
                        }
                    }
                }
            }
            catch { }
        }

        async Task SendControllerStatusDetailsList(string targetIp, int targetPort)
        {
            try
            {
                //Check if socket server is running
                if (vArnoldVinkSockets == null)
                {
                    Debug.WriteLine("The socket server is not running.");
                    return;
                }

                //List controller status
                List<ControllerStatusDetails> controllerStatusDetailsList = new List<ControllerStatusDetails>();

                //Gather controller status
                ControllerStatusDetails controllerStatus0 = new ControllerStatusDetails(vController0.NumberId);
                controllerStatus0.Activated = vController0.Activated;
                controllerStatus0.Connected = vController0.Connected();
                controllerStatus0.Color = vController0.Color.ToString();
                controllerStatus0.BatteryCurrent = vController0.BatteryCurrent;
                controllerStatusDetailsList.Add(controllerStatus0);

                ControllerStatusDetails controllerStatus1 = new ControllerStatusDetails(vController1.NumberId);
                controllerStatus1.Activated = vController1.Activated;
                controllerStatus1.Connected = vController1.Connected();
                controllerStatus1.Color = vController1.Color.ToString();
                controllerStatus1.BatteryCurrent = vController1.BatteryCurrent;
                controllerStatusDetailsList.Add(controllerStatus1);

                ControllerStatusDetails controllerStatus2 = new ControllerStatusDetails(vController2.NumberId);
                controllerStatus2.Activated = vController2.Activated;
                controllerStatus2.Connected = vController2.Connected();
                controllerStatus2.Color = vController2.Color.ToString();
                controllerStatus2.BatteryCurrent = vController2.BatteryCurrent;
                controllerStatusDetailsList.Add(controllerStatus2);

                ControllerStatusDetails controllerStatus3 = new ControllerStatusDetails(vController3.NumberId);
                controllerStatus3.Activated = vController3.Activated;
                controllerStatus3.Connected = vController3.Connected();
                controllerStatus3.Color = vController3.Color.ToString();
                controllerStatus3.BatteryCurrent = vController3.BatteryCurrent;
                controllerStatusDetailsList.Add(controllerStatus3);

                //Prepare socket data
                SocketSendContainer socketSend = new SocketSendContainer();
                socketSend.SourceIp = vArnoldVinkSockets.vSocketServerIp;
                socketSend.SourcePort = vArnoldVinkSockets.vSocketServerPort;
                socketSend.SetObject(controllerStatusDetailsList);
                byte[] SerializedData = SerializeObjectToBytes(socketSend);

                //Send socket data
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(targetIp), targetPort);
                await vArnoldVinkSockets.UdpClientSendBytesServer(ipEndPoint, SerializedData, vArnoldVinkSockets.vSocketTimeout);
            }
            catch { }
        }
    }
}
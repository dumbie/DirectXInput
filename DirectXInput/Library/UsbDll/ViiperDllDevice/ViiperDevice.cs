using System;
using System.Diagnostics;

namespace LibraryUsb
{
    public partial class ViiperDllDevice
    {
        public bool Connected;
        public UIntPtr ServerHandle = 0;
        public uint BusIdentifier = 0;

        public ViiperDllDevice()
        {
            try
            {
                //Create USB server configuration
                USBServerConfig usbServerConfiguration = new USBServerConfig
                {
                    addr = "localhost:39747"
                };

                //Create USB server
                bool success = NewUSBServer(ref usbServerConfiguration, out ServerHandle, null);
                if (!success)
                {
                    Debug.WriteLine("Failed to create Viiper USB server.");
                    return;
                }
                Debug.WriteLine("Created Viiper USB server with handle: " + ServerHandle);

                //Create USB bus
                success = CreateUSBBus(ServerHandle, ref BusIdentifier);
                if (!success)
                {
                    Debug.WriteLine("Failed to create Viiper USB bus.");
                    return;
                }
                Debug.WriteLine("Created Viiper USB bus with identifier: " + BusIdentifier);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed creating Viiper device: " + ex.Message);
            }
        }

        public void CloseDevice()
        {
            try
            {
                if (ServerHandle != 0)
                {
                    bool closedServer = CloseUSBServer(ServerHandle);
                    Debug.WriteLine("Closed Viiper USB device: " + closedServer);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed closing Viiper device: " + ex.Message);
            }
        }
    }
}
using ArnoldVinkStyles;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.Enums;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Connect with controller
        async Task ControllerConnect(ControllerDetails controllerDetails)
        {
            try
            {
                //Check if the controller is already in use
                bool ControllerInuse = false;
                if (vController0.Connected() && vController0.Details.DevicePath == controllerDetails.DevicePath) { ControllerInuse = true; }
                if (vController1.Connected() && vController1.Details.DevicePath == controllerDetails.DevicePath) { ControllerInuse = true; }
                if (vController2.Connected() && vController2.Details.DevicePath == controllerDetails.DevicePath) { ControllerInuse = true; }
                if (vController3.Connected() && vController3.Details.DevicePath == controllerDetails.DevicePath) { ControllerInuse = true; }
                if (ControllerInuse) { return; }

                Debug.WriteLine("Found a connected " + controllerDetails.Type + " controller to use: " + controllerDetails.DisplayName);

                //Connect the controller to available slot
                if (!vController0.Connected())
                {
                    bool controllerStarted = await ControllerStartOpen(vController0, controllerDetails);
                    if (controllerStarted)
                    {
                        AVDispatcherInvoke.DispatcherInvoke(delegate
                        {
                            image_Controller0.Source = vImagePreloadIconControllerAccent;
                            textblock_Controller0.Text = vController0.Details.DisplayName;
                            textblock_Controller0CodeName.Text = vController0.SupportedCurrent.CodeName;
                        });
                    }
                }
                else if (!vController1.Connected())
                {
                    bool controllerStarted = await ControllerStartOpen(vController1, controllerDetails);
                    if (controllerStarted)
                    {
                        AVDispatcherInvoke.DispatcherInvoke(delegate
                        {
                            image_Controller1.Source = vImagePreloadIconControllerAccent;
                            textblock_Controller1.Text = vController1.Details.DisplayName;
                            textblock_Controller1CodeName.Text = vController1.SupportedCurrent.CodeName;
                        });
                    }
                }
                else if (!vController2.Connected())
                {
                    bool controllerStarted = await ControllerStartOpen(vController2, controllerDetails);
                    if (controllerStarted)
                    {
                        AVDispatcherInvoke.DispatcherInvoke(delegate
                        {
                            image_Controller2.Source = vImagePreloadIconControllerAccent;
                            textblock_Controller2.Text = vController2.Details.DisplayName;
                            textblock_Controller2CodeName.Text = vController2.SupportedCurrent.CodeName;
                        });
                    }
                }
                else if (!vController3.Connected())
                {
                    bool controllerStarted = await ControllerStartOpen(vController3, controllerDetails);
                    if (controllerStarted)
                    {
                        AVDispatcherInvoke.DispatcherInvoke(delegate
                        {
                            image_Controller3.Source = vImagePreloadIconControllerAccent;
                            textblock_Controller3.Text = vController3.Details.DisplayName;
                            textblock_Controller3CodeName.Text = vController3.SupportedCurrent.CodeName;
                        });
                    }
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine("Failed to connect controller: " + ex.Message);
            }
        }

        //Get controller connection type
        ConnectionType ControllerConnectionType(ControllerDetails controllerDetails)
        {
            try 
            {
                //Debug.WriteLine("Getting controller connection type for: " + controllerDetails.DisplayName + " / Vendor: " + controllerDetails.Profile.VendorID + " / Product: " + controllerDetails.Profile.ProductID);

                //Check if controller uses bluetooth
                if (controllerDetails.DevicePath.ToLower().Contains("00805f9b34fb"))
                {
                    return ConnectionType.Bluetooth;
                }

                //Check if controller uses wifi
                if (controllerDetails.Profile.VendorID.ToLower() == "0x28de" && controllerDetails.Profile.ProductID.ToLower() == "0x1304")
                {
                    return ConnectionType.Wifi;
                }

                //Return default connection type
                return ConnectionType.Wired;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get controller connection type: " + ex.Message);
                return ConnectionType.Wired;
            }
        }
    }
}
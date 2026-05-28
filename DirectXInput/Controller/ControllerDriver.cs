using LibraryUsb;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVDevices.Enumerate;
using static DirectXInput.AppVariables;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Check if drivers are installed
        bool CheckInstalledDrivers()
        {
            try
            {
                bool virtualBusDriver = EnumerateDevicesDriverStore("hidmaestro.inf", false).Any();
                bool hidHideDriver = EnumerateDevicesDriverStore("HidHide.inf", false).Any();
                bool ds3ControllerDriver = EnumerateDevicesDriverStore("Ds3Controller.inf", false).Any();
                return virtualBusDriver && hidHideDriver && ds3ControllerDriver;
            }
            catch
            {
                Debug.WriteLine("Failed to check installed drivers.");
                return false;
            }
        }

        //Check drivers double
        bool CheckDriversDouble()
        {
            try
            {
                if (EnumerateDevicesDriverStore("hidmaestro.inf", false).Count() > 1)
                {
                    return false;
                }

                if (EnumerateDevicesDriverStore("HidHide.inf", false).Count() > 1)
                {
                    return false;
                }

                if (EnumerateDevicesDriverStore("Ds3Controller.inf", false).Count() > 1)
                {
                    return false;
                }

                Debug.WriteLine("No double drivers found.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check double drivers: " + ex.Message);
                return true;
            }
        }

        //Check drivers version and doubles
        bool CheckDriversVersion()
        {
            try
            {
                foreach (FileInfo infNames in EnumerateDevicesDriverStore("hidmaestro.inf", false))
                {
                    string availableVersion = File.ReadAllLines(@"Drivers\HIDMaestro\hidmaestro\x64\hidmaestro.inf").FirstOrDefault(x => x.StartsWith("DriverVer"));
                    string installedVersion = File.ReadAllLines(infNames.FullName).FirstOrDefault(x => x.StartsWith("DriverVer"));
                    //Debug.WriteLine("HIDMaestro: " + installedVersion + " / " + availableVersion);
                    if (availableVersion != installedVersion) { return false; } else { break; }
                }

                foreach (FileInfo infNames in EnumerateDevicesDriverStore("HidHide.inf", false))
                {
                    string availableVersion = File.ReadAllLines(@"Drivers\HidHide\x64\HidHide.inf").FirstOrDefault(x => x.StartsWith("DriverVer"));
                    string installedVersion = File.ReadAllLines(infNames.FullName).FirstOrDefault(x => x.StartsWith("DriverVer"));
                    //Debug.WriteLine("HidHide: " + installedVersion + " / " + availableVersion);
                    if (availableVersion != installedVersion) { return false; } else { break; }
                }

                foreach (FileInfo infNames in EnumerateDevicesDriverStore("Ds3Controller.inf", false))
                {
                    string availableVersion = File.ReadAllLines(@"Drivers\Ds3Controller\Ds3Controller.inf").FirstOrDefault(x => x.StartsWith("DriverVer"));
                    string installedVersion = File.ReadAllLines(infNames.FullName).FirstOrDefault(x => x.StartsWith("DriverVer"));
                    //Debug.WriteLine("Ds3Controller: " + installedVersion + " / " + availableVersion);
                    if (availableVersion != installedVersion) { return false; } else { break; }
                }

                Debug.WriteLine("Drivers seem to be up to date.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check drivers version: " + ex.Message);
                return true;
            }
        }

        //Open the virtual bus driver
        async Task<bool> OpenVirtualBusDriver()
        {
            try
            {
                vVirtualBusDevice = new HidMaestroDllDevice();
                if (vVirtualBusDevice.Connected)
                {
                    vHMMouseRelative = vVirtualBusDevice.MouseRelativeCreate();
                    vHMKeyboardNormal = vVirtualBusDevice.KeyboardNormalCreate();
                    vHMKeyboardMedia = vVirtualBusDevice.KeyboardMediaCreate();

                    Debug.WriteLine("Virtual bus driver is installed.");
                    return true;
                }
                else
                {
                    Debug.WriteLine("Virtual bus driver not installed.");
                    return false;
                }
            }
            catch
            {
                Debug.WriteLine("Failed to open virtual bus driver.");
                return false;
            }
        }

        //Open the hid hide device
        bool OpenHidHideDevice()
        {
            try
            {
                vHidHideDevice = new HidHideDllDevice();
                if (vHidHideDevice.Connected)
                {
                    Debug.WriteLine("HidHide device is installed.");
                    return true;
                }
                else
                {
                    Debug.WriteLine("HidHide device not installed.");
                    return false;
                }
            }
            catch
            {
                Debug.WriteLine("Failed to open HidHide device.");
                return false;
            }
        }
    }
}
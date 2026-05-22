using System.Linq;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.Enums;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Validate controller by identity
        bool ControllerValidateIdentity(string vendorHexId, string productHexId, string controllerPath, string serialNumber)
        {
            try
            {
                string vendorHexIdLower = vendorHexId.ToLower();
                string productHexIdLower = productHexId.ToLower();

                //Check if controller is already connected by serialnumber
                //if (!string.IsNullOrWhiteSpace(serialNumber))
                //{
                //    //Fix add code that reads serial number from devices
                //}

                //Check if the controller is on user ignore list
                foreach (ControllerIgnored ignoreCheck in vDirectControllersIgnored)
                {
                    string filterVendor = ignoreCheck.VendorID.ToLower();
                    string[] filterProducts = ignoreCheck.ProductIDs.Select(x => x.ToLower()).ToArray();
                    if (filterVendor == vendorHexIdLower && filterProducts.Any(productHexIdLower.Contains))
                    {
                        //Debug.WriteLine("Controller is on user ignore list: " + controllerPath);
                        return false;
                    }
                }

                //Check if the controller is on supported list
                foreach (ControllerSupported supportedCheck in vDirectControllersSupported)
                {
                    string filterVendor = supportedCheck.VendorID.ToLower();
                    string[] filterProducts = supportedCheck.ProductIDs.Select(x => x.ToLower()).ToArray();
                    if (filterVendor == vendorHexIdLower && filterProducts.Any(productHexIdLower.Contains))
                    {
                        //Debug.WriteLine("Controller is on supported list: " + controllerPath);
                        return true;
                    }
                }

                //Debug.WriteLine("Unknown controller found: " + vendorHexIdLower + "/" + productHexIdLower);
            }
            catch { }
            return false;
        }

        //Validate controller by status
        bool ControllerValidateStatus(ControllerStatus controller, ControllerDetails controllerDetails)
        {
            bool validStatus = true;
            try
            {
                if (controller.SupportedCurrent.CodeName == "SteamController2026" && controller.Details.ConnectionType == ConnectionType.Wifi)
                {
                    //Note: It takes a few seconds after disconnecting for SetFeature to stop responding use GetFeature and validate data instead.
                    //Fix: if Steam had access to the controller first, GetFeature does not work until you reconnect the controller.

                    //Check if controller returns feature data
                    byte HEAD_FEATURE_REPORT = 0x01;
                    byte ID_GET_DEVICE_INFO = 0xA1;
                    byte[] sendData = new byte[controller.HidDevice.Capabilities.FeatureReportByteLength];
                    sendData[0] = HEAD_FEATURE_REPORT;
                    sendData[1] = ID_GET_DEVICE_INFO;
                    byte[] dataFeature = controller.HidDevice.GetFeature(ref sendData);
                    validStatus = dataFeature != null;

                    //Debug.WriteLine("Controller valid status: " + validStatus + " / " + controller.Details.DevicePath);
                }
            }
            catch { }
            return validStatus;
        }
    }
}
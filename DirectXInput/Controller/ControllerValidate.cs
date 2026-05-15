using System.Linq;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;

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
                if (controller.SupportedCurrent.CodeName == "SteamController2026")
                {
                    //Check if controller responds to features
                    byte ID_GET_DEVICE_INFO = 0xA1;
                    byte HEAD_FEATURE_REPORT = 0x01;
                    byte[] featureData = new byte[controller.ControllerDataOutput.Length];
                    featureData[0] = HEAD_FEATURE_REPORT;
                    featureData[1] = ID_GET_DEVICE_INFO;
                    validStatus = controller.HidDevice.SetFeature(featureData);

                    //Debug.WriteLine("Controller valid status: " + controller.Details.DevicePath + " / " + validStatus);
                }
            }
            catch { }
            return validStatus;
        }
    }
}
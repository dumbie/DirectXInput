using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static ArnoldVinkCode.AVDevices.Extensions;
using static LibraryUsb.HidDeviceAttributes;
using static LibraryUsb.HidDeviceCapabilities;
using static LibraryUsb.NativeMethods_Hid;

namespace LibraryUsb
{
    public partial class HidDevice
    {
        private bool GetDeviceAttributes()
        {
            try
            {
                HIDD_ATTRIBUTES hiddDeviceAttributes = new HIDD_ATTRIBUTES();
                hiddDeviceAttributes.Size = Marshal.SizeOf(hiddDeviceAttributes);
                if (HidD_GetAttributes(FileHandle.Get(), ref hiddDeviceAttributes))
                {
                    Attributes = new HidDeviceAttributes(hiddDeviceAttributes);
                    return true;
                }
                else
                {
                    Debug.WriteLine("Failed to get device attributes.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get device attributes: " + ex.Message);
                return false;
            }
        }

        private bool GetDeviceCapabilities()
        {
            IntPtr preparsedDataPointer = IntPtr.Zero;
            try
            {
                if (HidD_GetPreparsedData(FileHandle.Get(), ref preparsedDataPointer))
                {
                    HIDP_CAPS deviceCapabilities = new HIDP_CAPS();
                    HidP_GetCaps(preparsedDataPointer, ref deviceCapabilities);
                    Capabilities = new HidDeviceCapabilities(deviceCapabilities);
                    return true;
                }
                else
                {
                    Debug.WriteLine("Failed to get device capabilities.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get device capabilities: " + ex.Message);
                return false;
            }
            finally
            {
                if (preparsedDataPointer != IntPtr.Zero)
                {
                    HidD_FreePreparsedData(preparsedDataPointer);
                }
            }
        }

        private bool GetProductName()
        {
            try
            {
                //Check if device has attributes
                if (Attributes == null) { return false; }

                byte[] data = new byte[254];
                HidD_GetProductString(FileHandle.Get(), ref data[0], data.Length);
                string productNameString = data.ToUTF16String().Replace("\0", string.Empty);
                if (!string.IsNullOrWhiteSpace(productNameString))
                {
                    Attributes.ProductName = productNameString;
                    return true;
                }
                else
                {
                    Attributes.ProductName = Attributes.ProductHexId + " Unknown";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Attributes.ProductName = Attributes.ProductHexId + " Unknown";
                Debug.WriteLine("Failed to get product name: " + ex.Message);
                return false;
            }
        }

        public bool GetVendorName()
        {
            try
            {
                //Check if device has attributes
                if (Attributes == null) { return false; }

                byte[] data = new byte[254];
                HidD_GetManufacturerString(FileHandle.Get(), ref data[0], data.Length);
                string vendorNameString = data.ToUTF16String().Replace("\0", string.Empty);
                if (!string.IsNullOrWhiteSpace(vendorNameString))
                {
                    Attributes.VendorName = vendorNameString;
                    return true;
                }
                else
                {
                    Attributes.VendorName = Attributes.VendorHexId + " Unknown";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Attributes.VendorName = Attributes.VendorHexId + " Unknown";
                Debug.WriteLine("Failed to get vendor name: " + ex.Message);
                return false;
            }
        }

        public bool GetSerialNumber()
        {
            try
            {
                //Check if device has attributes
                if (Attributes == null) { return false; }

                //Get serial number string
                byte[] dataString = new byte[254];
                HidD_GetSerialNumberString(FileHandle.Get(), ref dataString[0], dataString.Length);
                if (dataString != null)
                {
                    string serialNumberString = dataString.ToUTF16String().Replace("\0", string.Empty);
                    if (!string.IsNullOrWhiteSpace(serialNumberString))
                    {
                        //Return result
                        Attributes.SerialNumber = serialNumberString.ToUpper();
                        //Debug.WriteLine("Got serial number string: " + Attributes.SerialNumber);
                        return true;
                    }
                }

                //Return result
                Attributes.SerialNumber = string.Empty;
                //Debug.WriteLine("Failed to get serial number, not found.");
                return false;
            }
            catch (Exception ex)
            {
                //Return result
                Debug.WriteLine("Failed to get serial number: " + ex.Message);
                return false;
            }
        }
    }
}
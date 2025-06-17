using ArnoldVinkCode;
using System;
using System.Diagnostics;

namespace DirectXInput
{
    partial class WindowMain
    {
        //Check application user folders
        public void Folders_Check()
        {
            try
            {
                AVFiles.Directory_Create(@"Profiles\User\DirectControllersProfile", false);
                AVFiles.Directory_Create(@"Profiles\User\DirectKeypadMapping", false);

                Debug.WriteLine("Checked application user folders.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed checking application user folders: " + ex.Message);
            }
        }
    }
}
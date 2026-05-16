using System.Threading.Tasks;
using static DirectXInput.AppVariables;
using static LibraryShared.Classes;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Check if controller output needs to be forwarded
        async Task<bool> ControllerOutputForward(ControllerStatus controller)
        {
            try
            {
                if (controller.Activated && !controller.Disconnecting)
                {
                    //Check if a popup is visible
                    if (vWindowKeyboard.vWindowVisible)
                    {
                        vWindowKeyboard.ControllerInteractionMouse(controller.InputCurrent);
                        await vWindowKeyboard.ControllerInteractionKeyboard(controller.InputCurrent);
                        return true;
                    }
                    else if (vWindowKeypad.vWindowVisible)
                    {
                        vWindowKeypad.ControllerInteractionKeypadPreview(controller.InputCurrent);
                        vWindowKeypad.ControllerInteractionMouse(controller.InputCurrent);
                        vWindowKeypad.ControllerInteractionKeyboard(controller.InputCurrent);
                        return true;
                    }
                    else if (vProcessCtrlUI != null && vProcessCtrlUIActivated)
                    {
                        await OutputAppCtrlUI(controller);
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
    }
}
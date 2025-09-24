using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media;
using static ArnoldVinkCode.AVSettings;
using static DirectXInput.AppVariables;

namespace DirectXInput
{
    partial class WindowMain
    {
        //Load - Application Settings
        async Task Settings_Load()
        {
            try
            {
                cb_SettingsExclusiveGuide.IsChecked = SettingLoad(vConfigurationDirectXInput, "ExclusiveGuide", typeof(bool));

                //Load battery settings
                int batteryLevelLowInt = SettingLoad(vConfigurationDirectXInput, "BatteryLowLevel", typeof(int));
                textblock_BatteryLowLevel.Text = textblock_BatteryLowLevel.Tag + ": " + batteryLevelLowInt + "%";
                slider_BatteryLowLevel.Value = batteryLevelLowInt;

                cb_SettingsBatteryLowBlinkLed.IsChecked = SettingLoad(vConfigurationDirectXInput, "BatteryLowBlinkLed", typeof(bool));
                cb_SettingsBatteryLowShowNotification.IsChecked = SettingLoad(vConfigurationDirectXInput, "BatteryLowShowNotification", typeof(bool));
                cb_SettingsBatteryLowPlaySound.IsChecked = SettingLoad(vConfigurationDirectXInput, "BatteryLowPlaySound", typeof(bool));

                //Load controller settings
                int controllerIdleDisconnectMinInt = SettingLoad(vConfigurationDirectXInput, "ControllerIdleDisconnectMin", typeof(int));
                textblock_ControllerIdleDisconnectMin.Text = textblock_ControllerIdleDisconnectMin.Tag + ": " + controllerIdleDisconnectMinInt + " minutes";
                slider_ControllerIdleDisconnectMin.Value = controllerIdleDisconnectMinInt;

                string ControllerColor0 = SettingLoad(vConfigurationDirectXInput, "ControllerColor0", typeof(string));
                SolidColorBrush ControllerColor0Brush = new BrushConverter().ConvertFrom(ControllerColor0) as SolidColorBrush;
                colorpicker_Controller0.Background = ControllerColor0Brush;
                vController0.Color = ControllerColor0Brush.Color;

                string ControllerColor1 = SettingLoad(vConfigurationDirectXInput, "ControllerColor1", typeof(string));
                SolidColorBrush ControllerColor1Brush = new BrushConverter().ConvertFrom(ControllerColor1) as SolidColorBrush;
                colorpicker_Controller1.Background = ControllerColor1Brush;
                vController1.Color = ControllerColor1Brush.Color;

                string ControllerColor2 = SettingLoad(vConfigurationDirectXInput, "ControllerColor2", typeof(string));
                SolidColorBrush ControllerColor2Brush = new BrushConverter().ConvertFrom(ControllerColor2) as SolidColorBrush;
                colorpicker_Controller2.Background = ControllerColor2Brush;
                vController2.Color = ControllerColor2Brush.Color;

                string ControllerColor3 = SettingLoad(vConfigurationDirectXInput, "ControllerColor3", typeof(string));
                SolidColorBrush ControllerColor3Brush = new BrushConverter().ConvertFrom(ControllerColor3) as SolidColorBrush;
                colorpicker_Controller3.Background = ControllerColor3Brush;
                vController3.Color = ControllerColor3Brush.Color;

                //Load launch settings
                cb_SettingsLaunchCtrlUI.IsChecked = SettingLoad(vConfigurationDirectXInput, "LaunchCtrlUI", typeof(bool));
                cb_SettingsLaunchFpsOverlayer.IsChecked = SettingLoad(vConfigurationDirectXInput, "LaunchFpsOverlayer", typeof(bool));
                cb_SettingsLaunchScreenCapy.IsChecked = SettingLoad(vConfigurationDirectXInput, "LaunchScreenCapy", typeof(bool));

                //Load keyboard settings
                cb_SettingsKeyboardCloseNoController.IsChecked = SettingLoad(vConfigurationDirectXInput, "KeyboardCloseNoController", typeof(bool));
                cb_SettingsKeyboardResetPosition.IsChecked = SettingLoad(vConfigurationDirectXInput, "KeyboardResetPosition", typeof(bool));
                combobox_KeyboardLayout.SelectedIndex = SettingLoad(vConfigurationDirectXInput, "KeyboardLayout", typeof(int));

                //Load mouse sensitivity
                textblock_SettingsKeyboardMouseMoveSensitivity.Text = textblock_SettingsKeyboardMouseMoveSensitivity.Tag.ToString() + SettingLoad(vConfigurationDirectXInput, "KeyboardMouseMoveSensitivity", typeof(string));
                slider_SettingsKeyboardMouseMoveSensitivity.Value = SettingLoad(vConfigurationDirectXInput, "KeyboardMouseMoveSensitivity", typeof(double));
                textblock_SettingsKeyboardMouseScrollSensitivity2.Text = textblock_SettingsKeyboardMouseScrollSensitivity2.Tag.ToString() + SettingLoad(vConfigurationDirectXInput, "KeyboardMouseScrollSensitivity2", typeof(string));
                slider_SettingsKeyboardMouseScrollSensitivity2.Value = SettingLoad(vConfigurationDirectXInput, "KeyboardMouseScrollSensitivity2", typeof(double));

                //Load media settings
                combobox_ControllerLedCondition.SelectedIndex = SettingLoad(vConfigurationDirectXInput, "ControllerLedCondition", typeof(int));
                textblock_SettingsMediaVolumeStep.Text = textblock_SettingsMediaVolumeStep.Tag.ToString() + SettingLoad(vConfigurationDirectXInput, "MediaVolumeStep", typeof(string));
                slider_SettingsMediaVolumeStep.Value = SettingLoad(vConfigurationDirectXInput, "MediaVolumeStep", typeof(double));

                //Launch settings
                cb_SettingsWindowsStartup.IsChecked = AVSettings.StartupShortcutCheck();

                //Display settings
                int monitorNumber = SettingLoad(vConfigurationDirectXInput, "DisplayMonitor", typeof(int));
                textblock_SettingsDisplayMonitor.Text = textblock_SettingsDisplayMonitor.Tag + ": " + monitorNumber;
                slider_SettingsDisplayMonitor.Value = monitorNumber;

                //Sound settings
                cb_SettingsInterfaceSound.IsChecked = SettingLoad(vConfigurationDirectXInput, "InterfaceSound", typeof(bool));
                textblock_SettingsSoundVolume.Text = textblock_SettingsSoundVolume.Tag + ": " + SettingLoad(vConfigurationDirectXInput, "InterfaceSoundVolume", typeof(string)) + "%";
                slider_SettingsSoundVolume.Value = SettingLoad(vConfigurationDirectXInput, "InterfaceSoundVolume", typeof(double));

                //Wait for settings to have loaded
                await Task.Delay(1500);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load application settings: " + ex.Message);
            }
        }
    }
}
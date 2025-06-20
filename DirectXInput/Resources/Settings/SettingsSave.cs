﻿using ArnoldVinkCode.Styles;
using System;
using System.Diagnostics;
using System.Windows.Media;
using static ArnoldVinkCode.AVJsonFunctions;
using static ArnoldVinkCode.AVSettings;
using static DirectXInput.AppVariables;
using static DirectXInput.ProfileFunctions;
using static DirectXInput.SettingsNotify;
using static LibraryShared.Classes;

namespace DirectXInput
{
    partial class WindowMain
    {
        //Save - Monitor Application Settings
        void Settings_Save()
        {
            try
            {
                cb_SettingsExclusiveGuide.Click += (sender, e) =>
                {
                    SettingSave(vConfigurationDirectXInput, "ExclusiveGuide", cb_SettingsExclusiveGuide.IsChecked.ToString());
                };

                //Launch settings
                cb_SettingsWindowsStartup.Click += (sender, e) =>
                {
                    StartupShortcutManage("Launcher.exe", false);
                };

                cb_SettingsLaunchCtrlUI.Click += (sender, e) =>
                {
                    SettingSave(vConfigurationDirectXInput, "LaunchCtrlUI", cb_SettingsLaunchCtrlUI.IsChecked.ToString());
                };

                cb_SettingsLaunchFpsOverlayer.Click += (sender, e) =>
                {
                    SettingSave(vConfigurationDirectXInput, "LaunchFpsOverlayer", cb_SettingsLaunchFpsOverlayer.IsChecked.ToString());
                };

                cb_SettingsLaunchScreenCaptureTool.Click += (sender, e) =>
                {
                    SettingSave(vConfigurationDirectXInput, "LaunchScreenCaptureTool", cb_SettingsLaunchScreenCaptureTool.IsChecked.ToString());
                };

                //Battery settings
                slider_BatteryLowLevel.ValueChanged += (sender, e) =>
                {
                    textblock_BatteryLowLevel.Text = textblock_BatteryLowLevel.Tag + ": " + slider_BatteryLowLevel.Value.ToString() + "%";
                    SettingSave(vConfigurationDirectXInput, "BatteryLowLevel", slider_BatteryLowLevel.Value);

                    //Check all controllers for low battery level
                    CheckAllControllersLowBattery(true);
                };

                cb_SettingsBatteryLowBlinkLed.Click += (sender, e) =>
                {
                    SettingSave(vConfigurationDirectXInput, "BatteryLowBlinkLed", cb_SettingsBatteryLowBlinkLed.IsChecked.ToString());

                    //Check all controllers for low battery level
                    CheckAllControllersLowBattery(true);
                };

                cb_SettingsBatteryLowShowNotification.Click += (sender, e) =>
                {
                    SettingSave(vConfigurationDirectXInput, "BatteryLowShowNotification", cb_SettingsBatteryLowShowNotification.IsChecked.ToString());

                    //Check all controllers for low battery level
                    CheckAllControllersLowBattery(true);
                };

                cb_SettingsBatteryLowPlaySound.Click += (sender, e) =>
                {
                    SettingSave(vConfigurationDirectXInput, "BatteryLowPlaySound", cb_SettingsBatteryLowPlaySound.IsChecked.ToString());

                    //Check all controllers for low battery level
                    CheckAllControllersLowBattery(true);
                };

                //Controller settings
                slider_ControllerIdleDisconnectMin.ValueChanged += (sender, e) =>
                {
                    textblock_ControllerIdleDisconnectMin.Text = textblock_ControllerIdleDisconnectMin.Tag + ": " + slider_ControllerIdleDisconnectMin.Value.ToString() + " minutes";
                    SettingSave(vConfigurationDirectXInput, "ControllerIdleDisconnectMin", slider_ControllerIdleDisconnectMin.Value);
                };

                colorpicker_Controller0.Click += async (sender, e) =>
                {
                    Color? newColor = await new ColorPickerPreset().Popup(null);
                    if (newColor != null)
                    {
                        SolidColorBrush newBrush = new SolidColorBrush((Color)newColor);
                        SettingSave(vConfigurationDirectXInput, "ControllerColor0", newBrush.ToString());
                        colorpicker_Controller0.Background = newBrush;
                        vController0.Color = newBrush.Color;
                        if (vController0 == vActiveController())
                        {
                            txt_ActiveControllerName.Foreground = newBrush;
                        }

                        //Controller update led color
                        ControllerLedColor(vController0);

                        //Send controller colors to CtrlUI
                        await SendControllerStatusDetailsList(vArnoldVinkSockets.vSocketServerIp, 26759);
                    }
                };

                colorpicker_Controller1.Click += async (sender, e) =>
                {
                    Color? newColor = await new ColorPickerPreset().Popup(null);
                    if (newColor != null)
                    {
                        SolidColorBrush newBrush = new SolidColorBrush((Color)newColor);
                        SettingSave(vConfigurationDirectXInput, "ControllerColor1", newBrush.ToString());
                        colorpicker_Controller1.Background = newBrush;
                        vController1.Color = newBrush.Color;
                        if (vController1 == vActiveController())
                        {
                            txt_ActiveControllerName.Foreground = newBrush;
                        }

                        //Controller update led color
                        ControllerLedColor(vController1);

                        //Send controller colors to CtrlUI
                        await SendControllerStatusDetailsList(vArnoldVinkSockets.vSocketServerIp, 26759);
                    }
                };

                colorpicker_Controller2.Click += async (sender, e) =>
                {
                    Color? newColor = await new ColorPickerPreset().Popup(null);
                    if (newColor != null)
                    {
                        SolidColorBrush newBrush = new SolidColorBrush((Color)newColor);
                        SettingSave(vConfigurationDirectXInput, "ControllerColor2", newBrush.ToString());
                        colorpicker_Controller2.Background = newBrush;
                        vController2.Color = newBrush.Color;
                        if (vController2 == vActiveController())
                        {
                            txt_ActiveControllerName.Foreground = newBrush;
                        }

                        //Controller update led color
                        ControllerLedColor(vController2);

                        //Send controller colors to CtrlUI
                        await SendControllerStatusDetailsList(vArnoldVinkSockets.vSocketServerIp, 26759);
                    }
                };

                colorpicker_Controller3.Click += async (sender, e) =>
                {
                    Color? newColor = await new ColorPickerPreset().Popup(null);
                    if (newColor != null)
                    {
                        SolidColorBrush newBrush = new SolidColorBrush((Color)newColor);
                        SettingSave(vConfigurationDirectXInput, "ControllerColor3", newBrush.ToString());
                        colorpicker_Controller3.Background = newBrush;
                        vController3.Color = newBrush.Color;
                        if (vController3 == vActiveController())
                        {
                            txt_ActiveControllerName.Foreground = newBrush;
                        }

                        //Controller update led color
                        ControllerLedColor(vController3);

                        //Send controller colors to CtrlUI
                        await SendControllerStatusDetailsList(vArnoldVinkSockets.vSocketServerIp, 26759);
                    }
                };

                //Keyboard settings
                cb_SettingsKeyboardCloseNoController.Click += (sender, e) =>
                {
                    SettingSave(vConfigurationDirectXInput, "KeyboardCloseNoController", cb_SettingsKeyboardCloseNoController.IsChecked.ToString());
                };

                cb_SettingsKeyboardResetPosition.Click += (sender, e) =>
                {
                    SettingSave(vConfigurationDirectXInput, "KeyboardResetPosition", cb_SettingsKeyboardResetPosition.IsChecked.ToString());
                };

                combobox_KeyboardLayout.SelectionChanged += (sender, e) =>
                {
                    SettingSave(vConfigurationDirectXInput, "KeyboardLayout", combobox_KeyboardLayout.SelectedIndex.ToString());
                    vWindowKeyboard.UpdateKeyboardLayout();
                };

                slider_SettingsKeyboardMouseMoveSensitivity.ValueChanged += (sender, e) =>
                {
                    textblock_SettingsKeyboardMouseMoveSensitivity.Text = textblock_SettingsKeyboardMouseMoveSensitivity.Tag.ToString() + slider_SettingsKeyboardMouseMoveSensitivity.Value.ToString("0.00");
                    SettingSave(vConfigurationDirectXInput, "KeyboardMouseMoveSensitivity", slider_SettingsKeyboardMouseMoveSensitivity.Value);
                };

                slider_SettingsKeyboardMouseScrollSensitivity2.ValueChanged += (sender, e) =>
                {
                    textblock_SettingsKeyboardMouseScrollSensitivity2.Text = textblock_SettingsKeyboardMouseScrollSensitivity2.Tag.ToString() + slider_SettingsKeyboardMouseScrollSensitivity2.Value.ToString();
                    SettingSave(vConfigurationDirectXInput, "KeyboardMouseScrollSensitivity2", slider_SettingsKeyboardMouseScrollSensitivity2.Value);
                };

                //Keypad settings
                slider_KeypadOpacity.ValueChanged += (sender, e) =>
                {
                    KeypadMapping selectedProfile = (KeypadMapping)combobox_KeypadProcessProfile.SelectedItem;
                    selectedProfile.KeypadOpacity = slider_KeypadOpacity.Value;

                    //Save changes to Json file
                    JsonSaveObject(selectedProfile, GenerateJsonNameKeypadMapping(selectedProfile));

                    textblock_KeypadOpacity.Text = textblock_KeypadOpacity.Tag + ": " + slider_KeypadOpacity.Value.ToString("0.00") + "%";
                    vWindowKeypad.UpdatePopupOpacity();
                };

                combobox_KeypadDisplayStyle.SelectionChanged += (sender, e) =>
                {
                    KeypadMapping selectedProfile = (KeypadMapping)combobox_KeypadProcessProfile.SelectedItem;
                    selectedProfile.KeypadDisplayStyle = combobox_KeypadDisplayStyle.SelectedIndex;

                    //Save changes to Json file
                    JsonSaveObject(selectedProfile, GenerateJsonNameKeypadMapping(selectedProfile));

                    vWindowKeypad.UpdateKeypadStyle();
                };

                slider_KeypadDisplaySize.ValueChanged += async (sender, e) =>
                {
                    KeypadMapping selectedProfile = (KeypadMapping)combobox_KeypadProcessProfile.SelectedItem;
                    selectedProfile.KeypadDisplaySize = Convert.ToInt32(slider_KeypadDisplaySize.Value);

                    //Save changes to Json file
                    JsonSaveObject(selectedProfile, GenerateJsonNameKeypadMapping(selectedProfile));

                    textblock_KeypadDisplaySize.Text = textblock_KeypadDisplaySize.Tag + ": " + selectedProfile.KeypadDisplaySize + "%";

                    //Update the keypad size
                    double keypadHeight = vWindowKeypad.UpdateKeypadSize();

                    //Notify - Fps Overlayer keypad size changed
                    await NotifyFpsOverlayerKeypadSizeChanged(Convert.ToInt32(keypadHeight));
                };

                cb_SettingsKeypadMouseMoveEnabled.Click += (sender, e) =>
                {
                    KeypadMapping selectedProfile = (KeypadMapping)combobox_KeypadProcessProfile.SelectedItem;
                    selectedProfile.KeypadMouseMoveEnabled = (bool)cb_SettingsKeypadMouseMoveEnabled.IsChecked;

                    //Save changes to Json file
                    JsonSaveObject(selectedProfile, GenerateJsonNameKeypadMapping(selectedProfile));

                    //Update all keypad key names
                    vWindowKeypad.UpdateKeypadNames();
                };

                slider_SettingsKeypadMouseMoveSensitivity.ValueChanged += (sender, e) =>
                {
                    KeypadMapping selectedProfile = (KeypadMapping)combobox_KeypadProcessProfile.SelectedItem;
                    selectedProfile.KeypadMouseMoveSensitivity = slider_SettingsKeypadMouseMoveSensitivity.Value;

                    //Save changes to Json file
                    JsonSaveObject(selectedProfile, GenerateJsonNameKeypadMapping(selectedProfile));

                    textblock_SettingsKeypadMouseMoveSensitivity.Text = textblock_SettingsKeypadMouseMoveSensitivity.Tag + ": " + selectedProfile.KeypadMouseMoveSensitivity.ToString("0.00");
                };

                //Media settings
                combobox_ControllerLedCondition.SelectionChanged += (sender, e) =>
                {
                    SettingSave(vConfigurationDirectXInput, "ControllerLedCondition", combobox_ControllerLedCondition.SelectedIndex.ToString());
                };

                slider_SettingsMediaVolumeStep.ValueChanged += (sender, e) =>
                {
                    textblock_SettingsMediaVolumeStep.Text = textblock_SettingsMediaVolumeStep.Tag.ToString() + slider_SettingsMediaVolumeStep.Value.ToString();
                    SettingSave(vConfigurationDirectXInput, "MediaVolumeStep", slider_SettingsMediaVolumeStep.Value);
                };

                //Display settings
                slider_SettingsDisplayMonitor.ValueChanged += (sender, e) =>
                {
                    textblock_SettingsDisplayMonitor.Text = textblock_SettingsDisplayMonitor.Tag + ": " + Convert.ToInt32(slider_SettingsDisplayMonitor.Value);
                    SettingSave(vConfigurationDirectXInput, "DisplayMonitor", slider_SettingsDisplayMonitor.Value);
                    vWindowOverlay.UpdateWindowPosition();
                    vWindowKeyboard.UpdateWindowPosition();
                    vWindowKeypad.UpdateWindowPosition();
                };

                //Sound settings
                cb_SettingsInterfaceSound.Click += (sender, e) =>
                {
                    SettingSave(vConfigurationDirectXInput, "InterfaceSound", cb_SettingsInterfaceSound.IsChecked.ToString());
                };

                slider_SettingsSoundVolume.ValueChanged += (sender, e) =>
                {
                    textblock_SettingsSoundVolume.Text = textblock_SettingsSoundVolume.Tag + ": " + Convert.ToInt32(slider_SettingsSoundVolume.Value) + "%";
                    SettingSave(vConfigurationDirectXInput, "InterfaceSoundVolume", slider_SettingsSoundVolume.Value);
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save the application settings: " + ex.Message);
            }
        }
    }
}
using System;
using System.Diagnostics;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVInputOutputClass;

namespace LibraryUsb
{
    public partial class ViiperDllDevice
    {
        public UIntPtr KeyboardCreate()
        {
            UIntPtr deviceHandle = 0;
            try
            {
                bool success = CreateKeyboardDevice(ServerHandle, out deviceHandle, BusIdentifier, true, 0, 0);
                if (!success)
                {
                    Debug.WriteLine("Failed to create keyboard device.");
                    return deviceHandle;
                }
                else
                {
                    Debug.WriteLine("Created keyboard device with handle: " + deviceHandle);
                    return deviceHandle;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to create keyboard device: " + ex.Message);
                return deviceHandle;
            }
        }

        public bool KeyboardPressRelease(UIntPtr deviceHandle, KeysHidAction keyboardAction)
        {
            try
            {
                KeyboardSetInput(deviceHandle, keyboardAction);
                AVHighResDelay.Delay(50);
                KeyboardResetInput(deviceHandle);
                return true;
            }
            catch
            {
                Debug.WriteLine("Failed to press and release keyboard keys.");
                return false;
            }
        }

        public bool KeyboardSetInput(UIntPtr deviceHandle, KeysHidAction keyboardAction)
        {
            try
            {
                //Create device state
                KeyboardDeviceState deviceState = new KeyboardDeviceState();

                //Set key modifiers
                deviceState.Modifiers = (byte)keyboardAction.Modifiers;

                //Set key bitmap
                deviceState.KeyBitmap = new byte[32];
                if (keyboardAction.Key0 != KeysHid.None)
                {
                    int byteIndex = (byte)keyboardAction.Key0 / 8;
                    int bitIndex = (byte)keyboardAction.Key0 % 8;
                    deviceState.KeyBitmap[byteIndex] |= (byte)(1 << bitIndex);
                }
                if (keyboardAction.Key1 != KeysHid.None)
                {
                    int byteIndex = (byte)keyboardAction.Key1 / 8;
                    int bitIndex = (byte)keyboardAction.Key1 % 8;
                    deviceState.KeyBitmap[byteIndex] |= (byte)(1 << bitIndex);
                }
                if (keyboardAction.Key2 != KeysHid.None)
                {
                    int byteIndex = (byte)keyboardAction.Key2 / 8;
                    int bitIndex = (byte)keyboardAction.Key2 % 8;
                    deviceState.KeyBitmap[byteIndex] |= (byte)(1 << bitIndex);
                }
                if (keyboardAction.Key3 != KeysHid.None)
                {
                    int byteIndex = (byte)keyboardAction.Key3 / 8;
                    int bitIndex = (byte)keyboardAction.Key3 % 8;
                    deviceState.KeyBitmap[byteIndex] |= (byte)(1 << bitIndex);
                }
                if (keyboardAction.Key4 != KeysHid.None)
                {
                    int byteIndex = (byte)keyboardAction.Key4 / 8;
                    int bitIndex = (byte)keyboardAction.Key4 % 8;
                    deviceState.KeyBitmap[byteIndex] |= (byte)(1 << bitIndex);
                }
                if (keyboardAction.Key5 != KeysHid.None)
                {
                    int byteIndex = (byte)keyboardAction.Key5 / 8;
                    int bitIndex = (byte)keyboardAction.Key5 % 8;
                    deviceState.KeyBitmap[byteIndex] |= (byte)(1 << bitIndex);
                }

                //Set device state
                bool success = SetKeyboardDeviceState(deviceHandle, deviceState);
                if (!success)
                {
                    Debug.WriteLine("Failed to set keyboard input.");
                }

                //Return result
                return success;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to set keyboard input: " + ex.Message);
                return false;
            }
        }

        public bool KeyboardResetInput(UIntPtr deviceHandle)
        {
            try
            {
                //Create device state
                KeyboardDeviceState deviceState = new KeyboardDeviceState();
                deviceState.Modifiers = 0;
                deviceState.KeyBitmap = new byte[32];

                //Set device state
                bool success = SetKeyboardDeviceState(deviceHandle, deviceState);
                if (!success)
                {
                    Debug.WriteLine("Failed to reset keyboard input.");
                }

                //Return result
                return success;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to reset keyboard input: " + ex.Message);
                return false;
            }
        }
    }
}
using System;
using System.Diagnostics;
using static ArnoldVinkCode.AVActions;
using static LibraryShared.Classes;
using static LibraryShared.Enums;

namespace DirectXInput
{
    public partial class WindowMain
    {
        //Get rumble power mode bytes
        public byte ControllerBytesRumblePowerMode(ControllerStatus Controller)
        {
            try
            {
                if (Controller.SupportedCurrent.CodeName == "SonyPS5DualSense")
                {
                    if (Controller.Details.Profile.ControllerRumblePower == ControllerRumblePower.Maximum)
                    {
                        return 0x00; //100% (Default)
                    }
                    else if (Controller.Details.Profile.ControllerRumblePower == ControllerRumblePower.High)
                    {
                        return 0x01; //90%
                    }
                    else if (Controller.Details.Profile.ControllerRumblePower == ControllerRumblePower.Medium)
                    {
                        return 0x02; //80%
                    }
                    else if (Controller.Details.Profile.ControllerRumblePower == ControllerRumblePower.Low)
                    {
                        return 0x03; //70%
                    }
                    else if (Controller.Details.Profile.ControllerRumblePower == ControllerRumblePower.Minimum)
                    {
                        return 0x04; //60%
                    }
                }
                else if (Controller.SupportedCurrent.CodeName == "SteamController2026")
                {
                    //Note: Ranges from 0 to 150 but 15+ is already too much
                    if (Controller.Details.Profile.ControllerRumblePower == ControllerRumblePower.Maximum)
                    {
                        return 0x0C;
                    }
                    else if (Controller.Details.Profile.ControllerRumblePower == ControllerRumblePower.High)
                    {
                        return 0x09;
                    }
                    else if (Controller.Details.Profile.ControllerRumblePower == ControllerRumblePower.Medium)
                    {
                        return 0x06;
                    }
                    else if (Controller.Details.Profile.ControllerRumblePower == ControllerRumblePower.Low)
                    {
                        return 0x03;
                    }
                    else if (Controller.Details.Profile.ControllerRumblePower == ControllerRumblePower.Minimum)
                    {
                        return 0x00; //Default (same as 0xFF Steam uses)
                    }
                }

                return 0x00;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get rumble power mode byte: " + ex.Message);
                return 0x00;
            }
        }

        //Get rumble trigger bytes
        public byte[] ControllerBytesRumbleTrigger(ControllerStatus Controller, ref long triggerTicks, byte triggerStrength)
        {

            try
            {
                if (Controller.SupportedCurrent.CodeName == "SonyPS5DualSense")
                {
                    //Note: Trigger effects 0x01 Feedback / 0x02 Weapon / 0x05 Off / 0x06 Vibrate
                    //Quickly turning trigger rumble On and Off creates a very noisy trigger kickback. *1
                    byte triggerEffectMode = 0x01;
                    byte[] triggerBytes = new byte[11];
                    long currentSytemTicks = GetSystemTicksMilli();

                    //Check trigger rumble
                    if (triggerStrength > 0)
                    {
                        if (triggerEffectMode == 0x01)
                        {
                            //Feedback
                            triggerBytes[0] = 0x01; //Effect
                            triggerBytes[1] = 0x00; //Position
                            triggerBytes[2] = triggerStrength; //Strength
                        }
                        else if (triggerEffectMode == 0x02)
                        {
                            //Weapon
                            triggerBytes[0] = 0x02; //Effect
                            triggerBytes[1] = 0xFF; //Start Position
                            triggerBytes[1] = 0x00; //Stop Position
                            triggerBytes[2] = triggerStrength; //Strength
                        }
                        else if (triggerEffectMode == 0x06)
                        {
                            //Vibrate
                            triggerBytes[0] = 0x06; //Effect
                            triggerBytes[1] = 0x01; //Frequency
                            triggerBytes[2] = triggerStrength; //Strength
                            triggerBytes[3] = 0x00; //Position
                        }

                        //Update last update ticks
                        triggerTicks = currentSytemTicks;
                    }
                    else
                    {
                        //*1 To reduce noisy kickback delay turn off
                        //Fix instead of turning effect off instantly reduce trigger strength and position
                        if ((currentSytemTicks - triggerTicks) > 400)
                        {
                            //Off
                            triggerBytes[0] = 0x05; //Effect
                        }
                    }
                    return triggerBytes;
                }

                return new byte[0];
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get trigger rumble bytes: " + ex.Message);
                return new byte[0];
            }
        }
    }
}
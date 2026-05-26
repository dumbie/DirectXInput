using System;
using static ArnoldVinkCode.AVActions;
using static LibraryShared.ControllerTimings;

namespace LibraryShared
{
    public partial class Classes
    {
        [Serializable]
        public class ControllerButtonDetails
        {
            public bool PressedRaw { get; set; } = false;
            public bool PressTimeDone { get; set; } = false;
            public long PressTimeStart { get; set; } = 0;
            public long PressTimeEnd { get; set; } = 0;

            public void PressTimeUpdate()
            {
                try
                {
                    if (PressedRaw)
                    {
                        if (PressTimeStart == 0)
                        {
                            PressTimeEnd = 0;
                            PressTimeStart = GetSystemTicksMilli();
                            PressTimeDone = false;
                        }
                    }
                    else
                    {
                        PressTimeEnd = PressTimeStart;
                        PressTimeStart = 0;
                        PressTimeDone = true;
                    }
                }
                catch { }
            }

            public long PressTimeCurrent
            {
                get
                {
                    if (PressTimeStart > 0)
                    {
                        return GetSystemTicksMilli() - PressTimeStart;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            public long PressTimeReleased
            {
                get
                {
                    if (PressTimeEnd > 0)
                    {
                        return GetSystemTicksMilli() - PressTimeEnd;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            public bool PressedShort
            {
                get
                {
                    if (PressTimeDone && PressTimeReleased > 0 && PressTimeReleased <= vControllerButtonPressShort)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            public bool PressedLong
            {
                get
                {
                    if (!PressTimeDone && PressTimeCurrent >= vControllerButtonPressLong)
                    {
                        PressTimeDone = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
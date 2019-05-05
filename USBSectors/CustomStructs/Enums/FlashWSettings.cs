using System;

namespace USBSectors.CustomStructs.Enums
{
    [Flags]
    public enum FlashWSettings : uint
    {
        FLASHW_STOP = 0,
        FLASHW_CAPTION = 1,
        FLASHW_TRAY = 2,
        FLASHW_ALL = 3,
        FLASHW_TIMER = 4,
        FLASHW_TIMERNOFG = 12
    }
}

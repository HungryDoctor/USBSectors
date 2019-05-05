using System;
using System.Runtime.InteropServices;
using USBSectors.ConstValues;
using USBSectors.CustomStructs.CppStructs;
using USBSectors.CustomStructs.Enums;
using USBSectors.Utils;

namespace USBSectors
{
    public static class Blink
    {
        private static FLASHWINFO Create_FLASHWINFO(IntPtr handle, FlashWSettings flags, uint count, uint timeout)
        {
            FLASHWINFO fi = new FLASHWINFO();
            fi.cbSize = Convert.ToUInt32(Marshal.SizeOf(fi));
            fi.hwnd = handle;
            fi.dwFlags = (uint)flags;
            fi.uCount = count;
            fi.dwTimeout = timeout;
            return fi;
        }


        public static bool Flash(IntPtr handle, uint count, bool stopOnActivated)
        {
            FlashWSettings flag;
            if (stopOnActivated)
            {
                flag = FlashWSettings.FLASHW_ALL | FlashWSettings.FLASHW_TIMERNOFG;
            }
            else
            {
                flag = FlashWSettings.FLASHW_ALL;
            }

            FLASHWINFO fi = Create_FLASHWINFO(handle, flag, count, Win32Constants.FLASHW_TIMEOUT);

            return NativeMethods.FlashWindowEx(ref fi);
        }

        public static bool Start(IntPtr handle, bool stopOnActivated)
        {
            return Flash(handle, uint.MaxValue, stopOnActivated);
        }

        public static bool Stop(IntPtr handle, bool leaveTrayColored)
        {
            uint count;
            if (leaveTrayColored)
            {
                count = 1;
            }
            else
            {
                count = 0;
            }

            FLASHWINFO fi = Create_FLASHWINFO(handle, FlashWSettings.FLASHW_STOP, count, 0);
            return NativeMethods.FlashWindowEx(ref fi);
        }

    }
}

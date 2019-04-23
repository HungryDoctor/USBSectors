using System;
using USBSectors.CustomStructs;
using USBSectors.Utils;

namespace USBSectors
{
    public static class Keyboard
    {
        public static bool SetLanguage(LanguageId languageId)
        {
            return Win32Utils.SetLanguage(languageId);
        }

        public static IntPtr GetCurrentKeyboardLayout()
        {
            return Win32Utils.GetCurrentKeyboardLayout();
        }
    }
}

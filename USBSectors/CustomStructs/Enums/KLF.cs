using System;

namespace USBSectors.CustomStructs.Enums
{
    [Flags]
    public enum KLF : uint
    {
        KLF_ACTIVATE = 0x00000001,
        KLF_SUBSTITUTE_OK = 0x00000002,
        KLF_REORDER = 0x00000008,
        KLF_REPLACELANG = 0x00000010,
        KLF_NOTELLSHELL = 0x00000080,
        KLF_SETFORPROCESS = 0x00000100,
        KLF_SHIFTLOCK = 0x00010000,
        KLF_RESET = 0x40000000
    }
}

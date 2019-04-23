using System;
using System.Runtime.InteropServices;

namespace USBSectors.CustomStructs.CppStructs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SP_DEVINFO_DATA
    {
        public uint cbSize;
        public Guid classGuid;
        public uint devInst;
        public IntPtr reserved;
    }
}

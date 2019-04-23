using System;
using System.Runtime.InteropServices;

namespace USBSectors.CustomStructs.CppStructs
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class DEV_BROADCAST_DEVICEINTERFACE_1
    {
        public int dbcc_size;
        public int dbcc_devicetype;
        public int dbcc_reserved;
        public Guid dbcc_classguid;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
        public char[] dbcc_name;
    }
}

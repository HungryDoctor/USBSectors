using System.Runtime.InteropServices;
using USBSectors.ConstValues;

namespace USBSectors.CustomStructs.CppStructs
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SP_DEVICE_INTERFACE_DETAIL_DATA
    {
        public uint cbSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Win32Constants.SIZE_OUTPUT)]
        public string DevicePath;
    }
}

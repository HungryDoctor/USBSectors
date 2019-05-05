using System.Runtime.InteropServices;
using USBSectors.CustomStructs.Enums;

namespace USBSectors.CustomStructs.CppStructs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_DEVICE_NUMBER
    {
        public DeviceType DeviceType;
        public uint DeviceNumber;
        public uint PartitionNumber;
    }
}

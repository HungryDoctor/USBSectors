using System.Runtime.InteropServices;

namespace USBSectors.CustomStructs.CppStructs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_DEVICE_NUMBER
    {
        public DEVICE_TYPE DeviceType;
        public uint DeviceNumber;
        public uint PartitionNumber;
    }
}

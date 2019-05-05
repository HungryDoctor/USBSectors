using USBSectors.CustomStructs.Enums;

namespace USBSectors.CustomStructs
{
    public struct VolumeInformation
    {
        public string Name;
        public string FileSystemName;
        public uint SerialNumber;
        public EFileSystemFeature FileSystemFeature;
    }
}

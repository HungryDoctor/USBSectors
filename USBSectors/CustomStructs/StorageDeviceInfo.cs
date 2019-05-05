using USBSectors.CustomStructs.CppStructs;

namespace USBSectors.CustomStructs
{
    public struct StorageDeviceInfo
    {
        public STORAGE_DEVICE_NUMBER StorageDeviceNumber;
        public byte[] StorageDeviceNumberRaw;
        public string InstanceID;
        public string DevicePath;
    }
}

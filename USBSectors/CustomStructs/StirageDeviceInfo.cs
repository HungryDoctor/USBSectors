using USBSectors.CustomStructs.CppStructs;

namespace USBSectors.CustomStructs
{
    public struct StirageDeviceInfo
    {
        public STORAGE_DEVICE_NUMBER StorageDeviceNumber;
        public byte[] StorageDeviceNumberRaw;
        public string InstanceID;
        public string DevicePath;
    }
}

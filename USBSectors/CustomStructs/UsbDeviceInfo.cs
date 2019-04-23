namespace USBSectors.CustomStructs
{
    public class UsbDeviceInfo
    {
        public DiskSpaceLayout? DiskSpaceLayout { get; }
        public DeviceInfo DeviceInfo { get; }
        public BootSectorInfo BootSectorInfo { get; }



        public UsbDeviceInfo(DeviceInfo deviceInfo, BootSectorInfo bootSectorInfo, DiskSpaceLayout? diskSpaceLayout)
        {
            DiskSpaceLayout = diskSpaceLayout;
            DeviceInfo = deviceInfo;
            BootSectorInfo = bootSectorInfo;
        }
    }
}

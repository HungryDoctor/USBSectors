using System;

namespace USBSectors.CustomStructs.UsbDeviceEvents
{
    public class UsbDeviceArrivedEventArgs : EventArgs
    {
        public char DriveLetter { get; }
        public UsbDeviceInfo USBDeviceInfo { get; }



        public UsbDeviceArrivedEventArgs(char driveLetter, UsbDeviceInfo deviceInfo)
        {
            this.DriveLetter = driveLetter;
            this.USBDeviceInfo = deviceInfo;
        }
    }
}

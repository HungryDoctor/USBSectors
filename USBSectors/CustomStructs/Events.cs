using System;

namespace USBSectors.CustomStructs
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

    public class UsbDeviceRemovedEventArgs : EventArgs
    {
        public char DriveLetter { get; }



        public UsbDeviceRemovedEventArgs(char driveLetter)
        {
            this.DriveLetter = driveLetter;
        }
    }

}

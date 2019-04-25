using System;

namespace USBSectors.CustomStructs.UsbDeviceEvents
{
    public class UsbDeviceRemovedEventArgs : EventArgs
    {
        public char DriveLetter { get; }



        public UsbDeviceRemovedEventArgs(char driveLetter)
        {
            this.DriveLetter = driveLetter;
        }
    }
}

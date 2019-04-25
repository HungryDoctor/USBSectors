using System;

namespace USBSectors.CustomStructs.UsbDeviceEvents
{
    public class UsbDeviceExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; }



        public UsbDeviceExceptionEventArgs(Exception exception)
        {
            this.Exception = exception;
        }
    }
}

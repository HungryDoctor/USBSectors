using System;
using USBSectors.CustomStructs.UsbDeviceEvents;

namespace USBSectors.Base
{
    public interface IUsbDeviceEventHelper : IDisposable
    {
        event EventHandler<UsbDeviceArrivedEventArgs> UsbDeviceArrivedEvent;
        event EventHandler<UsbDeviceRemovedEventArgs> UsbDeviceRemovedEvent;
        event EventHandler<UsbDeviceExceptionEventArgs> UsbDeviceErrorEvent;
    }
}

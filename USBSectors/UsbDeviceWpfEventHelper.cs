using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using USBSectors.Base;
using USBSectors.ConstValues;
using USBSectors.CustomStructs.CppStructs;
using USBSectors.CustomStructs.Enums;
using USBSectors.CustomStructs.UsbDeviceEvents;
using USBSectors.Utils;

namespace USBSectors
{
    public class UsbDeviceWpfEventHelper : IUsbDeviceEventHelper
    {
        private volatile bool _disposed;
        private IntPtr m_hNotifyDevNode;
        private readonly Window _window;
        private HwndSource hwndSource;

        public event EventHandler<UsbDeviceArrivedEventArgs> UsbDeviceArrivedEvent;
        public event EventHandler<UsbDeviceRemovedEventArgs> UsbDeviceRemovedEvent;
        public event EventHandler<UsbDeviceExceptionEventArgs> UsbDeviceErrorEvent;



        public UsbDeviceWpfEventHelper(Window window)
        {
            _disposed = false;

            _window = window;

            if (_window.IsLoaded)
            {
                RegisterNotification();
            }
            else
            {
                _window.Loaded += Window_Loaded;
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterNotification();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Win32Utils.UnregisterNotification(m_hNotifyDevNode);
                hwndSource.Dispose();

                _disposed = true;
            }
        }


        private void RegisterNotification()
        {
            var handle = new WindowInteropHelper(_window).Handle;
            m_hNotifyDevNode = Win32Utils.RegisterNotification(Win32Constants.GUID_DEVINTERFACE_DISK, handle);

            hwndSource = HwndSource.FromHwnd(handle);
            hwndSource?.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == Win32Constants.WM_DEVICECHANGE && wParam.ToInt32().TryConvertToEnum<DeviceEvent>(out var nEventType) && (nEventType == DeviceEvent.DEVICEARRIVAL || nEventType == DeviceEvent.DEVICEREMOVECOMPLETE))
            {
                DEV_BROADCAST_HDR hdr = new DEV_BROADCAST_HDR();
                Marshal.PtrToStructure(lParam, hdr);

                if (hdr.dbch_devicetype == Win32Constants.DBT_DEVTYP_VOLUME)
                {
                    DEV_BROADCAST_VOLUME volume = new DEV_BROADCAST_VOLUME();
                    Marshal.PtrToStructure(lParam, volume);

                    if (nEventType == DeviceEvent.DEVICEREMOVECOMPLETE)
                    {
                        try
                        {
                            var driveLetter = UsbDevice.DriveMaskToLetter(volume.dbcv_unitmask);
                            UsbDeviceRemovedEvent?.Invoke(this, new UsbDeviceRemovedEventArgs(driveLetter));
                        }
                        catch (Exception e)
                        {
                            UsbDeviceErrorEvent?.Invoke(this, new UsbDeviceExceptionEventArgs(e));
                        }
                    }
                    else if (nEventType == DeviceEvent.DEVICEARRIVAL)
                    {
                        try
                        {
                            var driveLetter = UsbDevice.DriveMaskToLetter(volume.dbcv_unitmask);
                            var usbDeviceInfo = UsbDevice.GetUSBDeviceInfo(driveLetter);

                            UsbDeviceArrivedEvent?.Invoke(this, new UsbDeviceArrivedEventArgs(driveLetter, usbDeviceInfo));
                        }
                        catch (Exception e)
                        {
                            UsbDeviceErrorEvent?.Invoke(this, new UsbDeviceExceptionEventArgs(e));
                        }
                    }
                }
            }

            return IntPtr.Zero;
        }

    }
}

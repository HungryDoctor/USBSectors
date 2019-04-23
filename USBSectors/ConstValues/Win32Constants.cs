using Microsoft.Win32.SafeHandles;
using System;
using System.Threading;

namespace USBSectors.ConstValues
{
    public static class Win32Constants
    {
        #region Lazy

        public static readonly Lazy<Guid> GUID_DEVINTERFACE_DISK_L;
        public static readonly Lazy<IntPtr> INVALID_HANDLE_VALUE_L;
        public static readonly Lazy<SafeFileHandle> INVALID_HANDLE_VALUE_S_L;

        #endregion

        public const uint FLASHW_TIMEOUT = 500;

        public const int SIZE_OUTPUT = 0x400;

        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0x0;
        public const ushort WM_DEVICECHANGE = 0x0219;
        public const ushort DBT_DEVICEARRIVAL = 0x8000;
        public const ushort DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const ushort DBT_DEVTYP_DEVICEINTERFACE = 0x5;

        public const ushort DBT_DEVTYP_VOLUME = 0x2;

        public const uint IOCTL_STORAGE_QUERY_PROPERTY = 0x2d1400;
        public const uint IOCTL_STORAGE_GET_DEVICE_NUMBER = 0x2d1080;


        public const uint CR_SUCCESS = 0x0;


        public const uint INVALID_HANDLE_VALUE_UI = 0xFFFFFFFF;
        public static Guid GUID_DEVINTERFACE_DISK => GUID_DEVINTERFACE_DISK_L.Value;
        public static IntPtr INVALID_HANDLE_VALUE => INVALID_HANDLE_VALUE_L.Value;
        public static SafeFileHandle INVALID_HANDLE_VALUE_S => INVALID_HANDLE_VALUE_S_L.Value;



        static Win32Constants()
        {
            GUID_DEVINTERFACE_DISK_L = new Lazy<Guid>(()=> new Guid("53F56307-B6BF-11D0-94F2-00A0C91EFB8B"), LazyThreadSafetyMode.PublicationOnly);
            INVALID_HANDLE_VALUE_L = new Lazy<IntPtr>(() => new IntPtr(-1), LazyThreadSafetyMode.PublicationOnly);
            INVALID_HANDLE_VALUE_S_L = new Lazy<SafeFileHandle>(()=> new SafeFileHandle(new IntPtr(-1), true), LazyThreadSafetyMode.PublicationOnly);
        }
    }

}

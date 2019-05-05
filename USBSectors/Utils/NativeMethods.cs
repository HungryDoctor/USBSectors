using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Text;
using USBSectors.CustomStructs.CppStructs;
using USBSectors.CustomStructs.Enums;

namespace USBSectors.Utils
{
    internal static class NativeMethods
    {
        #region User32

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern HKL ActivateKeyboardLayout(
            [In] [MarshalAs(UnmanagedType.U4)] HKL hkl,
            [In] [MarshalAs(UnmanagedType.U4)] KLF flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern HKL ActivateKeyboardLayout(
            [In] IntPtr langIdentifier,
            [In] [MarshalAs(UnmanagedType.U4)] KLF flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetKeyboardLayoutList(
            [In] [MarshalAs(UnmanagedType.I4)] int nBuff,
            [Out] IntPtr[] lpList);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetKeyboardLayout(
            [In] [MarshalAs(UnmanagedType.U4)] uint idThread);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LoadKeyboardLayout(
            [In] [MarshalAs(UnmanagedType.LPWStr)] string pwszKLID,
            [In] [MarshalAs(UnmanagedType.U4)] KLF flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(
            [In] IntPtr hWnd,
            [In] IntPtr processId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetForegroundWindow(
            [In] IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetLastInputInfo(
            [In, Out] ref LASTINPUTINFO plii);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool FlashWindowEx(
            [In, Out] ref FLASHWINFO pwfi);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr RegisterDeviceNotification(
            [In] IntPtr hRecipient,
            [In] IntPtr NotificationFilter,
            [In] [MarshalAs(UnmanagedType.U4)] uint Flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint UnregisterDeviceNotification(
            [In] IntPtr Handle);

        #endregion


        #region Kernel32

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern SafeFileHandle CreateFile(
            [In] [MarshalAs(UnmanagedType.LPWStr)] string filename,
            [In] [MarshalAs(UnmanagedType.U4)] EFileAccess dwDesiredAccess,
            [In] [MarshalAs(UnmanagedType.U4)] EFileShare dwShareMode,
            [In, Optional] IntPtr lpSecurityAttributes,
            [In] [MarshalAs(UnmanagedType.U4)] ECreationDisposition dwCreationDisposition,
            [In] [MarshalAs(UnmanagedType.U4)] EFileAttributes dwFlagsAndAttributes,
            [In, Optional] IntPtr hTemplateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetDiskFreeSpace(
            [In] [MarshalAs(UnmanagedType.LPWStr)] string filename,
            [Out] [MarshalAs(UnmanagedType.U4)] out uint lpSectorsPerCluster,
            [Out] [MarshalAs(UnmanagedType.U4)] out uint lpBytesPerSector,
            [Out] [MarshalAs(UnmanagedType.U4)] out uint lpNumberOfFreeClusters,
            [Out] [MarshalAs(UnmanagedType.U4)] out uint lpTotalNumberOfClusters);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetVolumeInformationByHandleW(
            [In] SafeFileHandle hFile,
            [Out, Optional] [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpVolumeNameBuffer,
            [In] [MarshalAs(UnmanagedType.U4)] uint nVolumeNameSize,
            [Out, Optional] [MarshalAs(UnmanagedType.U4)] out uint lpVolumeSerialNumber,
            [Out, Optional] [MarshalAs(UnmanagedType.U4)] out uint lpMaximumComponentLength,
            [Out, Optional] [MarshalAs(UnmanagedType.U4)] out EFileSystemFeature lpFileSystemFlags,
            [Out, Optional] [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpFileSystemNameBuffer,
            [In] [MarshalAs(UnmanagedType.U4)] uint nFileSystemNameSize);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DeviceIoControl(
            [In] SafeFileHandle hDevice,
            [In] [MarshalAs(UnmanagedType.U4)] DeviceControlCode dwIoControlCode,
            [In, Optional] [MarshalAs(UnmanagedType.LPArray)] byte[] lpInBuffer,
            [In] [MarshalAs(UnmanagedType.U4)] uint nInBufferSize,
            [Out, Optional] [MarshalAs(UnmanagedType.LPArray)] byte[] lpOutBuffer,
            [In] [MarshalAs(UnmanagedType.U4)] uint nOutBufferSize,
            [Out, Optional] [MarshalAs(UnmanagedType.U4)] out uint lpBytesReturned,
            [In, Out, Optional] IntPtr lpOverlapped);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ReadFile(
            [In] SafeFileHandle hFile,
            [Out] [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer,
            [In] [MarshalAs(UnmanagedType.U4)] uint nNumberOfBytesToRead,
            [Out, Optional] [MarshalAs(UnmanagedType.U4)] out int lpNumberOfBytesRead,
            [In, Out, Optional] IntPtr lpOverlapped);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool WriteFile(
            [In] SafeFileHandle hFile,
            [In] [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer,
            [In] [MarshalAs(UnmanagedType.U4)] uint nNumberOfBytesToWrite,
            [Out, Optional] [MarshalAs(UnmanagedType.U4)] out uint lpNumberOfBytesWritten,
            [In, Out, Optional] IntPtr lpOverlapped);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetFilePointerEx(
            [In] SafeFileHandle hFile,
            [In] long liDistanceToMove,
            [Out, Optional] [MarshalAs(UnmanagedType.I8)] out long lpNewFilePointer,
            [In] [MarshalAs(UnmanagedType.U4)] EMoveMethod dwMoveMethod);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern DriveType GetDriveType(
            [In] [MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetVolumeNameForVolumeMountPoint(
            [In] string lpszVolumeMountPoint,
            [Out] [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpszVolumeName,
            [In] uint cchBufferLength);

        #endregion


        #region SetupApi

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(
            [In, Optional] ref Guid ClassGuid,
            [In, Optional] [MarshalAs(UnmanagedType.LPWStr)] string Enumerator,
            [In, Optional] IntPtr hwndParent,
            [In] DiGetClassFlags Flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInterfaces(
            [In] IntPtr hDevInfo,
            [In, Optional] IntPtr devInfo,
            [In] ref Guid interfaceClassGuid,
            [In] [MarshalAs(UnmanagedType.U4)] uint memberIndex,
            [Out] out SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
           [In] IntPtr hDevInfo,
           [In] ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
           [Out, Optional] IntPtr deviceInterfaceDetailData,
           [In] [MarshalAs(UnmanagedType.U4)] uint deviceInterfaceDetailDataSize,
           [Out, Optional] [MarshalAs(UnmanagedType.U4)] out uint requiredSize,
           [Out, Optional] IntPtr deviceInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetupDiDestroyDeviceInfoList(
            [In] IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CM_Get_Parent(
            [Out] out IntPtr pdnDevInst,
            [In] [MarshalAs(UnmanagedType.U4)] uint dnDevInst,
            [In] [MarshalAs(UnmanagedType.I4)] int ulFlags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CM_Get_Device_ID(
            [In] IntPtr dnDevInst,
            [Out] IntPtr buffer,
            [In] [MarshalAs(UnmanagedType.U4)] uint bufferLen,
            [In] [MarshalAs(UnmanagedType.I4)] int flags);

        #endregion

    }
}

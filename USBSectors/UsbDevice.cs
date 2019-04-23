using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using USBSectors.ConstValues;
using USBSectors.CustomStructs;
using USBSectors.CustomStructs.CppStructs;
using USBSectors.Utils;

namespace USBSectors
{
    public static class UsbDevice
    {
        public static UsbDeviceInfo GetUSBDeviceInfo(char driveLetter)
        {
            byte[] bootSectorData;
            string fileSystemName;

            STORAGE_DEVICE_NUMBER deviceNumber;
            DeviceInfo deviceInfo = new DeviceInfo();
            DiskSpaceLayout diskSpaceLayout = GetDiskLayout(driveLetter);

            using (SafeFileHandle handle = CreateDeviceReadHandle(driveLetter))
            {
                bootSectorData = ReadSector(handle, diskSpaceLayout, 0);

                var volumeInfo = GetVolumeInformation(handle);
                fileSystemName = volumeInfo.FileSystemName;

                deviceNumber = Win32Utils.DeviceIoControlAction<STORAGE_DEVICE_NUMBER>(handle, Win32Constants.IOCTL_STORAGE_GET_DEVICE_NUMBER).Item1;
            }

            var bootSectorInfo = ParseBootSector(bootSectorData, fileSystemName);
            bootSectorInfo.FileSystemName_Query = fileSystemName;

            var driveType = NativeMethods.GetDriveType($"{driveLetter}:\\");
            deviceInfo.DeviceSerialNumber = GetRemovableDriveDeviceId(driveType, deviceNumber);

            return new UsbDeviceInfo(deviceInfo, bootSectorInfo, diskSpaceLayout);
        }

        public static char DriveMaskToLetter(uint mask)
        {
            const string drives = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int cnt = 0;
            uint pom = mask / 2;
            while (pom != 0)
            {
                pom /= 2;
                cnt++;
            }

            return cnt < drives.Length ? drives[cnt] : '?';
        }


        public static string GetRemovableDriveDeviceId(DriveType driveType, STORAGE_DEVICE_NUMBER deviceNumber)
        {
            string deviceId = "";

            var foundItem = GetRemovableDriveDeviceIds(driveType, deviceNumber.DeviceType).Find(x => x.StorageDeviceNumber.DeviceNumber == deviceNumber.DeviceNumber);
            if (!default(StirageDeviceInfo).Equals(foundItem))
            {
                deviceId = foundItem.InstanceID;
            }

            return deviceId;
        }

        public static List<StirageDeviceInfo> GetRemovableDriveDeviceIds(DriveType driveType, DEVICE_TYPE deviceType)
        {
            List<StirageDeviceInfo> devices = new List<StirageDeviceInfo>();

            if (driveType == DriveType.Removable && deviceType == DEVICE_TYPE.FILE_DEVICE_DISK)
            {
                var guid = Win32Constants.GUID_DEVINTERFACE_DISK;

                var setupInterfaceResult = NativeMethods.SetupDiGetClassDevs(
                    ref guid,
                    null,
                    IntPtr.Zero,
                    DiGetClassFlags.DIGCF_PRESENT | DiGetClassFlags.DIGCF_DEVICEINTERFACE);
                if (setupInterfaceResult == Win32Constants.INVALID_HANDLE_VALUE)
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }

                bool result = true;
                uint i = 0;
                while (result)
                {
                    SP_DEVICE_INTERFACE_DATA dia = new SP_DEVICE_INTERFACE_DATA();
                    dia.cbSize = (uint)Marshal.SizeOf(dia);

                    result = NativeMethods.SetupDiEnumDeviceInterfaces(setupInterfaceResult, IntPtr.Zero, ref guid, i, out dia);
                    i++;

                    if (result)
                    {
                        SP_DEVINFO_DATA da = new SP_DEVINFO_DATA();
                        da.cbSize = (uint)Marshal.SizeOf(da);
                        var daPtr = Win32Utils.AllocateStructToPointer<SP_DEVINFO_DATA>(da).Item1;

                        SP_DEVICE_INTERFACE_DETAIL_DATA didd = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                        if (IntPtr.Size == 8)
                        {
                            didd.cbSize = 8;
                        }
                        else
                        {
                            didd.cbSize = (uint)(4 + Marshal.SystemDefaultCharSize);
                        }
                        var diddPtr = Win32Utils.AllocateStructToPointer(didd).Item1;

                        if (!NativeMethods.SetupDiGetDeviceInterfaceDetail(
                            setupInterfaceResult,
                            ref dia,
                            diddPtr,
                            Win32Constants.SIZE_OUTPUT,
                            out uint requireSize,
                            daPtr))
                        {
                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                        }

                        da = Marshal.PtrToStructure<SP_DEVINFO_DATA>(daPtr);
                        didd = Marshal.PtrToStructure<SP_DEVICE_INTERFACE_DETAIL_DATA>(diddPtr);

                        string instanceID;
                        var getParentResult = NativeMethods.CM_Get_Parent(out IntPtr ptrPrevious, da.devInst, 0);
                        if (getParentResult != Win32Constants.CR_SUCCESS)
                        {
                            instanceID = getParentResult.ToString();
                        }
                        else
                        {
                            IntPtr ptrInstanceBuf = Marshal.AllocHGlobal(Win32Constants.SIZE_OUTPUT);

                            var getDeviceIdResult = NativeMethods.CM_Get_Device_ID(ptrPrevious, ptrInstanceBuf, Win32Constants.SIZE_OUTPUT, 0);
                            if (getDeviceIdResult != Win32Constants.CR_SUCCESS)
                            {
                                instanceID = getDeviceIdResult.ToString();
                            }
                            else
                            {
                                instanceID = Marshal.PtrToStringAuto(ptrInstanceBuf);

                                using (SafeFileHandle handle = CreateDeviceNoRightsHandle(didd.DevicePath))
                                {
                                    var deviceNumber = Win32Utils.DeviceIoControlAction<STORAGE_DEVICE_NUMBER>(handle, Win32Constants.IOCTL_STORAGE_GET_DEVICE_NUMBER);

                                    StirageDeviceInfo info = new StirageDeviceInfo
                                    {
                                        StorageDeviceNumber = deviceNumber.Item1,
                                        StorageDeviceNumberRaw = deviceNumber.Item2,
                                        InstanceID = instanceID,
                                        DevicePath = didd.DevicePath
                                    };

                                    devices.Add(info);
                                }
                            }

                            Marshal.FreeHGlobal(daPtr);
                            Marshal.FreeHGlobal(diddPtr);
                            Marshal.FreeHGlobal(ptrInstanceBuf);
                        }
                    }
                }

                if (!NativeMethods.SetupDiDestroyDeviceInfoList(setupInterfaceResult))
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
            }

            return devices;
        }

        public static VolumeInformation GetVolumeInformation(SafeFileHandle handle)
        {
            StringBuilder nameBuffer = new StringBuilder(Win32Constants.SIZE_OUTPUT);
            StringBuilder fileSystemBuffer = new StringBuilder(Win32Constants.SIZE_OUTPUT);

            if (!NativeMethods.GetVolumeInformationByHandleW(
                handle,
                nameBuffer,
                (uint)nameBuffer.Capacity,
                out uint serialNumber,
                out uint maxComponentLength,
                out EFileSystemFeature flag,
                fileSystemBuffer,
                (uint)fileSystemBuffer.Capacity))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            VolumeInformation info = new VolumeInformation
            {
                Name = nameBuffer.ToString().Trim(),
                FileSystemName = fileSystemBuffer.ToString().Trim(),
                FileSystemFeature = flag,
                SerialNumber = serialNumber
            };

            return info;
        }


        #region GetDiskLayout

        public static DiskSpaceLayout GetDiskLayout(char driveLetter)
        {
            return GetDiskLayout(string.Format("{0}:\\", driveLetter));
        }

        public static DiskSpaceLayout GetDiskLayout(string drivePath)
        {
            DiskSpaceLayout info = new DiskSpaceLayout();

            uint lpSectorsPerCluster;
            uint lpBytesPerSector;
            uint lpNumberOfFreeClusters;
            uint lpTotalNumberOfClusters;


            if (!NativeMethods.GetDiskFreeSpace(
                drivePath,
                out lpSectorsPerCluster,
                out lpBytesPerSector,
                out lpNumberOfFreeClusters,
                out lpTotalNumberOfClusters))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }


            info.lpSectorsPerCluster = lpSectorsPerCluster;
            info.lpBytesPerSector = lpBytesPerSector;
            info.lpNumberOfFreeClusters = lpNumberOfFreeClusters;
            info.lpTotalNumberOfClusters = lpTotalNumberOfClusters;

            return info;
        }

        #endregion


        #region CreateHandle

        public static SafeFileHandle CreateDeviceReadHandle(char driveLetter)
        {
            return CreateDeviceReadHandle(string.Format(@"\\.\{0}:", driveLetter));
        }

        public static SafeFileHandle CreateDeviceWriteHandle(char driveLetter)
        {
            return CreateDeviceWriteHandle(string.Format(@"\\.\{0}:", driveLetter));
        }


        private static SafeFileHandle CreateDeviceReadHandle(string filePath)
        {
            SafeFileHandle handle = NativeMethods.CreateFile(
                filePath,
                EFileAccess.GenericRead,
                EFileShare.Delete | EFileShare.Read | EFileShare.Write,
                IntPtr.Zero,
                ECreationDisposition.OpenExisting,
                EFileAttributes.Normal,
                IntPtr.Zero);

            if (handle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            return handle;
        }

        private static SafeFileHandle CreateDeviceNoRightsHandle(string filePath)
        {
            SafeFileHandle handle = NativeMethods.CreateFile(
                filePath,
                EFileAccess.None,
                EFileShare.Delete | EFileShare.Read | EFileShare.Write,
                IntPtr.Zero,
                ECreationDisposition.OpenExisting,
                EFileAttributes.Normal,
                IntPtr.Zero);

            if (handle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            return handle;
        }

        private static SafeFileHandle CreateDeviceWriteHandle(string filePath)
        {
            SafeFileHandle handle = NativeMethods.CreateFile(
                filePath,
                EFileAccess.GenericWrite,
                EFileShare.Read,
                IntPtr.Zero,
                ECreationDisposition.OpenExisting,
                EFileAttributes.Normal,
                IntPtr.Zero);

            if (handle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            return handle;
        }

        #endregion


        #region ReadSector

        public static byte[] ReadSector(char driveLetter, DiskSpaceLayout diskSpaceLayout, uint sectorNumber)
        {
            byte[] buffer = null;
            using (SafeFileHandle handle = CreateDeviceReadHandle(driveLetter))
            {
                buffer = ReadSector(handle, diskSpaceLayout, sectorNumber);
            }

            return buffer;
        }

        private static byte[] ReadSector(SafeFileHandle safeHandle, DiskSpaceLayout diskSpaceLayout, uint sectorNumber)
        {
            byte[] buffer = new byte[diskSpaceLayout.lpBytesPerSector];

            if (!NativeMethods.SetFilePointerEx(safeHandle, sectorNumber * diskSpaceLayout.lpBytesPerSector, out long newFilePointer, EMoveMethod.Current))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            if (!NativeMethods.ReadFile(safeHandle, buffer, diskSpaceLayout.lpBytesPerSector, out int bytesRead))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            return buffer;
        }

        #endregion


        #region WriteSector

        public static void WriteSector(char driveLetter, DiskSpaceLayout diskSpaceLayout, uint sectorNumber, byte[] bytesToWrite)
        {
            //using (SafeFileHandle handle = CreateDeviceWriteHandle(driveLetter))
            //{
            //    WriteSector(handle, diskSpaceLayout, sectorNumber, bytesToWrite);
            //}
        }

        public static void WriteSector(SafeFileHandle safeHandle, DiskSpaceLayout diskSpaceLayout, uint sectorNumber, byte[] bytesToWrite)
        {
            //if (!NativeMethods.SetFilePointerEx(safeHandle, sectorNumber * diskSpaceLayout.lpBytesPerSector, out long newFilePointer, EMoveMethod.Current))
            //{
            //    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            //}

            //if (!NativeMethods.WriteFile(safeHandle, bytesToWrite, (uint)bytesToWrite.Length, out var bytesWritten) || bytesWritten != bytesToWrite.Length)
            //{
            //    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            //}
        }

        #endregion


        #region ParseBootSector

        public static BootSectorInfo ParseBootSector(byte[] bootSectorData, string fileSystemName)
        {
            string fileSystemNameLow = fileSystemName.ToLowerInvariant();

            if (fileSystemNameLow == "fat32")
            {
                return ParseBootSector_FATx(bootSectorData, FATxType.Fat32);
            }
            else if (fileSystemNameLow == "exfat")
            {
                return ParseBootSector_ExFat(bootSectorData);
            }
            else if (fileSystemNameLow.Contains("fat"))
            {
                return ParseBootSector_FATx(bootSectorData, FATxType.Fat12_16);
            }
            else if (fileSystemNameLow == "ntfs")
            {
                return ParseBootSector_NTFS(bootSectorData);
            }
            else
            {
                return new BootSectorInfo();
            }
        }


        public static BootSectorInfo ParseBootSector_NTFS(byte[] bootSector)
        {
            BootSectorInfo info = new BootSectorInfo();

            byte[] BytesPerSector = new byte[] { bootSector[11], bootSector[12] };
            byte[] SectorsPerCluster = new byte[] { bootSector[13] };
            byte[] ReservedSectors = new byte[] { bootSector[14], bootSector[15] };
            byte[] FileSystemName = new byte[] { bootSector[3], bootSector[4], bootSector[5], bootSector[6], bootSector[7], bootSector[8], bootSector[9], bootSector[10] };
            byte[] VolumeSerialNumber = new byte[] { bootSector[72], bootSector[73], bootSector[74], bootSector[75], bootSector[76], bootSector[77], bootSector[78], bootSector[79] };

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(BytesPerSector);
                Array.Reverse(ReservedSectors);
            }

            info.FileSystemName_Parsed = Encoding.ASCII.GetString(FileSystemName);
            info.BytesPerSector = BitConverter.ToUInt16(BytesPerSector, 0);
            info.SectorsPerCluster = SectorsPerCluster[0];
            info.ReservedSectors = BitConverter.ToUInt16(ReservedSectors, 0);
            info.VolumeSerialNumber = BitConverter.ToUInt64(VolumeSerialNumber, 0);

            return info;
        }

        public static BootSectorInfo ParseBootSector_ExFat(byte[] bootSector)
        {
            BootSectorInfo info = new BootSectorInfo();

            byte[] BytesPerSector = new byte[] { bootSector[108] };
            byte[] SectorsPerCluster = new byte[] { bootSector[109] };
            byte[] FileSystemName = new byte[] { bootSector[3], bootSector[4], bootSector[5], bootSector[6], bootSector[7], bootSector[8], bootSector[9], bootSector[10] };
            byte[] VolumeSerialNumber = new byte[] { bootSector[100], bootSector[101], bootSector[102], bootSector[103] };

            info.FileSystemName_Parsed = Encoding.ASCII.GetString(FileSystemName);
            info.BytesPerSector = (uint)Math.Pow(2.0, BytesPerSector[0]);
            info.SectorsPerCluster = (byte)Math.Pow(2.0, SectorsPerCluster[0]);
            info.VolumeSerialNumber = BitConverter.ToUInt32(VolumeSerialNumber, 0);

            return info;
        }

        public static BootSectorInfo ParseBootSector_FATx(byte[] bootSector, FATxType fatType)
        {
            BootSectorInfo info = new BootSectorInfo();

            byte[] BytesPerSector = new byte[] { bootSector[11], bootSector[12] };
            byte[] SectorsPerCluster = new byte[] { bootSector[13] };
            byte[] ReservedSectors = new byte[] { bootSector[14], bootSector[15] };
            byte[] FileSystemName = null;
            byte[] VolumeSerialNumber = null;

            if (fatType == FATxType.Fat32)
            {
                //FAT32
                FileSystemName = new byte[] { bootSector[82], bootSector[83], bootSector[84], bootSector[85], bootSector[86], bootSector[87], bootSector[88], bootSector[89] };
                VolumeSerialNumber = new byte[] { bootSector[67], bootSector[68], bootSector[69], bootSector[70] };
            }
            else if (fatType == FATxType.Fat12_16)
            {
                //FAT12/16
                FileSystemName = new byte[] { bootSector[54], bootSector[55], bootSector[56], bootSector[57], bootSector[58], bootSector[59], bootSector[60], bootSector[61] };
                VolumeSerialNumber = new byte[] { bootSector[39], bootSector[40], bootSector[41], bootSector[42] };
            }

            info.FileSystemName_Parsed = Encoding.ASCII.GetString(FileSystemName);
            info.BytesPerSector = BitConverter.ToUInt16(BytesPerSector, 0);
            info.SectorsPerCluster = SectorsPerCluster[0];
            info.ReservedSectors = BitConverter.ToUInt16(ReservedSectors, 0);
            info.VolumeSerialNumber = BitConverter.ToUInt32(VolumeSerialNumber, 0);

            return info;
        }

        #endregion

    }
}

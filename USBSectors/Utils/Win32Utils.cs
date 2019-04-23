using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Text;
using USBSectors.Constants;
using USBSectors.CustomStructs;
using USBSectors.CustomStructs.CppStructs;

namespace USBSectors.Utils
{
    public static class Win32Utils
    {
        #region Public

        public static IntPtr RegisterNotification(Guid guid, IntPtr handle)
        {
            IntPtr result;

            DEV_BROADCAST_DEVICEINTERFACE devIF = new DEV_BROADCAST_DEVICEINTERFACE();
            IntPtr devIFBuffer;

            devIF.dbcc_size = Marshal.SizeOf(devIF);
            devIF.dbcc_devicetype = Win32Constants.DBT_DEVTYP_DEVICEINTERFACE;
            devIF.dbcc_reserved = 0;
            devIF.dbcc_classguid = guid;

            devIFBuffer = Marshal.AllocHGlobal(devIF.dbcc_size);
            Marshal.StructureToPtr(devIF, devIFBuffer, true);

            result = NativeMethods.RegisterDeviceNotification(handle, devIFBuffer, Win32Constants.DEVICE_NOTIFY_WINDOW_HANDLE);

            Marshal.PtrToStructure(devIFBuffer, devIF);
            Marshal.FreeHGlobal(devIFBuffer);

            return result;
        }

        public static uint UnregisterNotification(IntPtr m_hNotifyDevNode)
        {
            return NativeMethods.UnregisterDeviceNotification(m_hNotifyDevNode);
        }

        public static IntPtr GetForegroundWindow()
        {
            return NativeMethods.GetForegroundWindow();
        }

        public static IdleTimeInfo GetSystemIdleTimeInfo()
        {
            int systemUptime = Environment.TickCount;
            int lastInputTicks = 0;
            int idleTicks = 0;

            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            if (NativeMethods.GetLastInputInfo(ref lastInputInfo))
            {
                lastInputTicks = (int)lastInputInfo.dwTime;

                idleTicks = systemUptime - lastInputTicks;
            }

            return new IdleTimeInfo(DateTime.Now.AddMilliseconds(-1 * idleTicks), new TimeSpan(0, 0, 0, 0, idleTicks), systemUptime);
        }

        public static string ReadNullTerminatedString(byte[] array, int offset, Encoding encoding)
        {
            int finalChar = offset;
            for (int x = offset + 1; x < array.Length; x++)
            {
                if (array[x] == 0x00)
                {
                    finalChar = x;
                    break;
                }
                else if (x == array.Length - 1)
                {
                    finalChar = x;
                    break;
                }
            }

            if (finalChar != offset)
            {
                return new string(encoding.GetChars(array, offset, finalChar - offset));
            }
            else
            {
                return null;
            }

        }

        #endregion



        internal static bool SetLanguage(LanguageId languageId)
        {
            bool result = false;
            uint layoutCount = NativeMethods.GetKeyboardLayoutList(0, null);
            IntPtr[] layoutIds = new IntPtr[layoutCount];
            NativeMethods.GetKeyboardLayoutList(layoutIds.Length, layoutIds);

            foreach (var item in layoutIds)
            {
                var itemInt32 = item.ToInt32();
                if ((itemInt32 & 0xFFFF) == (int)languageId)
                {
                    var newLayout = NativeMethods.LoadKeyboardLayout(((int)languageId).ToString("X8"), KLF.KLF_ACTIVATE | KLF.KLF_SETFORPROCESS);
                    NativeMethods.ActivateKeyboardLayout(newLayout, KLF.KLF_SETFORPROCESS);

                    result = true;
                    break;
                }
            }

            return result;
        }

        internal static IntPtr GetCurrentKeyboardLayout()
        {
            return NativeMethods.GetKeyboardLayout(NativeMethods.GetWindowThreadProcessId(NativeMethods.GetForegroundWindow(), IntPtr.Zero));
        }


        internal static Tuple<T, byte[]> DeviceIoControlAction<T>(SafeFileHandle safeHandle, uint controlCode, int outputSize = Win32Constants.SIZE_OUTPUT) where T : struct
        {
            byte[] outputBuffer = new byte[outputSize];

            if (!NativeMethods.DeviceIoControl(
                safeHandle,
                controlCode,
                null,
                0,
                outputBuffer,
                (uint)outputBuffer.Length,
                out uint bytesReturned,
                IntPtr.Zero))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            var outputPtr = Marshal.AllocHGlobal(outputBuffer.Length);
            Marshal.Copy(outputBuffer, 0, outputPtr, outputBuffer.Length);
            T result = Marshal.PtrToStructure<T>(outputPtr);
            Marshal.FreeHGlobal(outputPtr);

            return new Tuple<T, byte[]>(result, outputBuffer);
        }

        internal static Tuple<T, byte[]> DeviceIoControlAction<T, U>(SafeFileHandle safeHandle, uint controlCode, U input, int outputSize = Win32Constants.SIZE_OUTPUT) where T : struct where U : struct
        {
            var inputSize = Marshal.SizeOf(input);
            var inputPtr = Marshal.AllocHGlobal(inputSize);
            Marshal.StructureToPtr(input, inputPtr, false);

            byte[] inputBuffer = new byte[inputSize];
            byte[] outputBuffer = new byte[outputSize];

            Marshal.Copy(inputPtr, inputBuffer, 0, inputSize);
            Marshal.FreeHGlobal(inputPtr);


            if (!NativeMethods.DeviceIoControl(
                safeHandle,
                controlCode,
                inputBuffer,
                (uint)inputBuffer.Length,
                outputBuffer,
                (uint)outputBuffer.Length,
                out uint bytesReturned,
                IntPtr.Zero))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            var outputPtr = Marshal.AllocHGlobal(outputBuffer.Length);
            Marshal.Copy(outputBuffer, 0, outputPtr, outputBuffer.Length);
            T result = Marshal.PtrToStructure<T>(outputPtr);
            Marshal.FreeHGlobal(outputPtr);

            return new Tuple<T, byte[]>(result, outputBuffer);
        }

        internal static Tuple<IntPtr, uint> AllocateStructToPointer<T>(T input) where T : struct
        {
            var inputSize = Marshal.SizeOf(input);
            var inputPtr = Marshal.AllocHGlobal(inputSize);
            Marshal.StructureToPtr(input, inputPtr, false);

            return new Tuple<IntPtr, uint>(inputPtr, (uint)inputSize);
        }

    }
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using USBSectors;
using USBSectors.Constants;
using USBSectors.CustomStructs;
using USBSectors.CustomStructs.CppStructs;
using USBSectors.Utils;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<UsbDeviceArrivedEventArgs> arrivedItems;
        private IntPtr m_hNotifyDevNode;

        public event EventHandler<UsbDeviceArrivedEventArgs> UsbDeviceArrivedEvent;
        public event EventHandler<UsbDeviceRemovedEventArgs> UsbDeviceRemovedEvent;



        public MainWindow()
        {
            InitializeComponent();

            UsbDeviceArrivedEvent += MainWindow_UsbDeviceArrivedEvent;
            UsbDeviceRemovedEvent += MainWindow_UsbDeviceRemovedEvent;

            arrivedItems = new ObservableCollection<UsbDeviceArrivedEventArgs>();
            comboBox_Devices.ItemsSource = arrivedItems;
        }



        private void ComboBox_Devices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button_GenerateKey.IsEnabled = false;
            button_WriteKey.IsEnabled = false;

            if (e.AddedItems.OfType<UsbDeviceArrivedEventArgs>().SingleOrDefault() is UsbDeviceArrivedEventArgs args)
            {
                var diskSpaceLayout = args.USBDeviceInfo.DiskSpaceLayout;
                var bootSectorInfo = args.USBDeviceInfo.BootSectorInfo;


                textBox_Serial.Text = args.USBDeviceInfo.DeviceInfo?.DeviceSerialNumber;

                textBox_VolumeSerial.Text = bootSectorInfo?.VolumeSerialNumber.ToString();

                textBox_FileSystemQ.Text = bootSectorInfo?.FileSystemName_Query;
                textBox_FileSystemM.Text = bootSectorInfo?.FileSystemName_Parsed;

                textBox_BytesPerSectorQ.Text = diskSpaceLayout?.lpBytesPerSector.ToString();
                textBox_BytesPerSectorM.Text = bootSectorInfo?.BytesPerSector.ToString();

                textBox_SectorsPerClusterQ.Text = diskSpaceLayout?.lpSectorsPerCluster.ToString();
                textBox_SectorsPerClusterM.Text = bootSectorInfo?.SectorsPerCluster.ToString();

                textBox_ReservedSectorsM.Text = bootSectorInfo?.ReservedSectors.ToString();


                if (TryGetKey(args, out var key))
                {
                    bool keyFound = false;

                    button_GenerateKey.IsEnabled = true;
                    button_WriteKey.IsEnabled = true;

                    for (int x = 0; x < key.Length; x++)
                    {
                        if (key[x] != 0)
                        {
                            keyFound = true;
                            break;
                        }
                    }

                    if (keyFound)
                    {
                        textBox_KeySector.Text = Encoding.ASCII.GetString(key);
                    }
                    else
                    {
                        textBox_KeySector.Text = "";
                    }
                }
            }
            else
            {
                textBox_Serial.Text = "";

                textBox_VolumeSerial.Text = "";

                textBox_FileSystemQ.Text = "";
                textBox_FileSystemM.Text = "";

                textBox_BytesPerSectorQ.Text = "";
                textBox_BytesPerSectorM.Text = "";

                textBox_SectorsPerClusterQ.Text = "";
                textBox_SectorsPerClusterM.Text = "";

                textBox_ReservedSectorsM.Text = "";
            }
        }

        private void Button_GenerateKey_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = comboBox_Devices.SelectedIndex;
            if (selectedIndex > -1)
            {
                var selectedItem = arrivedItems[selectedIndex];
                if (selectedItem.USBDeviceInfo.DiskSpaceLayout?.lpBytesPerSector >= 64 && TryGetKey(selectedItem, out var key))
                {
                    const byte offset = 33;
                    var bytes = new byte[64];
                    for (byte x = 0; x < 64; x++)
                    {
                        bytes[x] = (byte)(x + offset);
                    }

                    textBox_KeySector.Text = Encoding.ASCII.GetString(bytes);
                }
            }
        }

        private void Button_WriteKey_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = comboBox_Devices.SelectedIndex;
            if (selectedIndex > -1)
            {
                var selectedItem = arrivedItems[selectedIndex];
                var layout = selectedItem.USBDeviceInfo.DiskSpaceLayout;
                var bootSectorInfo = selectedItem.USBDeviceInfo.BootSectorInfo;

                if (layout?.lpBytesPerSector >= 64 && TryGetKey(selectedItem, out var key))
                {
                    var bytes = Encoding.ASCII.GetBytes(textBox_KeySector.Text);

                    UsbDevice.WriteSector(selectedItem.DriveLetter, layout.Value, bootSectorInfo.SectorsPerCluster - 1, bytes);
                }
            }
        }


        private void MainWindow_UsbDeviceArrivedEvent(object sender, UsbDeviceArrivedEventArgs e)
        {
            arrivedItems.Add(e);

            if (arrivedItems.Count == 1)
            {
                comboBox_Devices.SelectedIndex = 0;
            }
        }

        private void MainWindow_UsbDeviceRemovedEvent(object sender, UsbDeviceRemovedEventArgs e)
        {
            var item = arrivedItems.FirstOrDefault(x => x.DriveLetter == e.DriveLetter);
            if (item != null)
            {
                arrivedItems.Remove(item);
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);

            m_hNotifyDevNode = Win32Utils.RegisterNotification(Win32Constants.GUID_DEVINTERFACE_DISK, new WindowInteropHelper(this).Handle);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Win32Utils.UnregisterNotification(m_hNotifyDevNode);
        }

        protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == Win32Constants.WM_DEVICECHANGE)
            {
                int nEventType = wParam.ToInt32();

                if (nEventType == Win32Constants.DBT_DEVICEARRIVAL || nEventType == Win32Constants.DBT_DEVICEREMOVECOMPLETE)
                {
                    DEV_BROADCAST_HDR hdr = new DEV_BROADCAST_HDR();
                    Marshal.PtrToStructure(lParam, hdr);

                    if (hdr.dbch_devicetype == Win32Constants.DBT_DEVTYP_VOLUME)
                    {
                        DEV_BROADCAST_VOLUME volume = new DEV_BROADCAST_VOLUME();
                        Marshal.PtrToStructure(lParam, volume);

                        if (nEventType == Win32Constants.DBT_DEVICEREMOVECOMPLETE)
                        {
                            UsbDeviceRemovedEventArgs eventArgs = null;

                            try
                            {
                                var driveLetter = UsbDevice.DriveMaskToLetter(volume.dbcv_unitmask);

                                eventArgs = new UsbDeviceRemovedEventArgs(driveLetter);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "");
                            }

                            if (eventArgs != null)
                            {
                                UsbDeviceRemovedEvent?.Invoke(this, eventArgs);
                            }
                        }
                        else if (nEventType == Win32Constants.DBT_DEVICEARRIVAL)
                        {
                            UsbDeviceArrivedEventArgs eventArgs = null;

                            try
                            {
                                var driveLetter = UsbDevice.DriveMaskToLetter(volume.dbcv_unitmask);
                                var usbDeviceInfo = UsbDevice.GetUSBDeviceInfo(driveLetter);

                                eventArgs = new UsbDeviceArrivedEventArgs(driveLetter, usbDeviceInfo);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "");
                            }

                            if (eventArgs != null)
                            {
                                UsbDeviceArrivedEvent?.Invoke(this, eventArgs);
                            }
                        }
                    }
                }
            }

            return IntPtr.Zero;
        }


        private bool TryGetKey(UsbDeviceArrivedEventArgs arrivedItem, out byte[] key)
        {
            key = null;

            var diskSpaceLayout = arrivedItem.USBDeviceInfo.DiskSpaceLayout;
            var bootSectorInfo = arrivedItem.USBDeviceInfo.BootSectorInfo;

            if (diskSpaceLayout.HasValue && bootSectorInfo != null
                && diskSpaceLayout.Value.lpBytesPerSector == bootSectorInfo.BytesPerSector
                && diskSpaceLayout.Value.lpSectorsPerCluster == bootSectorInfo.SectorsPerCluster
                && bootSectorInfo.ReservedSectors > 1)
            {
                key = UsbDevice.ReadSector(arrivedItem.DriveLetter, diskSpaceLayout.Value, bootSectorInfo.SectorsPerCluster - 1);
            }

            return key != null;
        }
    }
}

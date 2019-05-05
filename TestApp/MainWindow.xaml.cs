using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using USBSectors;
using USBSectors.Base;
using USBSectors.CustomStructs.UsbDeviceEvents;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private readonly ObservableCollection<UsbDeviceArrivedEventArgs> arrivedItems;
        private readonly IUsbDeviceEventHelper eventHelper;



        public MainWindow()
        {
            InitializeComponent();

            eventHelper = new UsbDeviceWpfEventHelper(this);
            eventHelper.UsbDeviceArrivedEvent += UsbDeviceArrivedEvent;
            eventHelper.UsbDeviceRemovedEvent += UsbDeviceRemovedEvent;
            eventHelper.UsbDeviceErrorEvent += UsbDeviceErrorEvent;

            arrivedItems = new ObservableCollection<UsbDeviceArrivedEventArgs>();
            comboBox_Devices.ItemsSource = arrivedItems;
        }



        public void Dispose()
        {
            eventHelper?.Dispose();
        }

        private void ComboBox_Devices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button_GenerateData.IsEnabled = false;
            button_WriteData.IsEnabled = false;

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


                if (TryGetData(args, out var data))
                {
                    bool dataFound = false;

                    button_GenerateData.IsEnabled = true;
                    button_WriteData.IsEnabled = true;

                    for (int x = 0; x < data.Length; x++)
                    {
                        if (data[x] != 0)
                        {
                            dataFound = true;
                            break;
                        }
                    }

                    if (dataFound)
                    {
                        textBox_DataSector.Text = Encoding.ASCII.GetString(data);
                    }
                    else
                    {
                        textBox_DataSector.Text = "";
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

        private void Button_GenerateData_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = comboBox_Devices.SelectedIndex;
            if (selectedIndex > -1)
            {
                var selectedItem = arrivedItems[selectedIndex];
                if (selectedItem.USBDeviceInfo.DiskSpaceLayout?.lpBytesPerSector >= 64 && TryGetData(selectedItem, out var data))
                {
                    const byte offset = 33;
                    var bytes = new byte[64];
                    for (byte x = 0; x < 64; x++)
                    {
                        bytes[x] = (byte)(x + offset);
                    }

                    textBox_DataSector.Text = Encoding.ASCII.GetString(bytes);
                }
            }
        }


        //Not tested yet
        private void Button_WriteData_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = comboBox_Devices.SelectedIndex;
            if (selectedIndex > -1)
            {
                var selectedItem = arrivedItems[selectedIndex];
                var layout = selectedItem.USBDeviceInfo.DiskSpaceLayout;
                var bootSectorInfo = selectedItem.USBDeviceInfo.BootSectorInfo;

                if (layout?.lpBytesPerSector >= 64 && TryGetData(selectedItem, out var data))
                {
                    var bytes = Encoding.ASCII.GetBytes(textBox_DataSector.Text);

                    UsbDevice.WriteSector(selectedItem.DriveLetter, layout.Value, bootSectorInfo.SectorsPerCluster - 1, bytes);
                }
            }
        }


        private void UsbDeviceArrivedEvent(object sender, UsbDeviceArrivedEventArgs e)
        {
            arrivedItems.Add(e);

            if (arrivedItems.Count == 1)
            {
                comboBox_Devices.SelectedIndex = 0;
            }
        }

        private void UsbDeviceRemovedEvent(object sender, UsbDeviceRemovedEventArgs e)
        {
            var item = arrivedItems.FirstOrDefault(x => x.DriveLetter == e.DriveLetter);
            if (item != null)
            {
                arrivedItems.Remove(item);
            }
        }

        private void UsbDeviceErrorEvent(object sender, UsbDeviceExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }


        private bool TryGetData(UsbDeviceArrivedEventArgs arrivedItem, out byte[] data)
        {
            data = null;

            var diskSpaceLayout = arrivedItem.USBDeviceInfo.DiskSpaceLayout;
            var bootSectorInfo = arrivedItem.USBDeviceInfo.BootSectorInfo;

            if (diskSpaceLayout.HasValue && bootSectorInfo != null
                && diskSpaceLayout.Value.lpBytesPerSector == bootSectorInfo.BytesPerSector
                && diskSpaceLayout.Value.lpSectorsPerCluster == bootSectorInfo.SectorsPerCluster
                && bootSectorInfo.ReservedSectors > 1)
            {
                data = UsbDevice.ReadSector(arrivedItem.DriveLetter, diskSpaceLayout.Value, bootSectorInfo.SectorsPerCluster - 1);
            }

            return data != null;
        }
    }
}

using OTSF_Application.Devices;
using OTSF_Application.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OTSF_Application.Pages {
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page {


        public List<IDevice> devices;

        public Dashboard() {

            InitializeComponent();

            devices = new List<IDevice> {
                new FakeDevice("H.Saint pierre"),
                new FakeDevice("H.ULB",0.15f),
                new FakeDevice("H.UCL",0.2f),
            };

            AdaFruitSettingsManager adaFruitSettingsManager = AdaFruitSettingsManager.GetInstance();

            foreach (string device in adaFruitSettingsManager.Devices) {
                devices.Add(new AdafruitDevice(device, device));
            }


            SetDevicesStackPanel();

            PageManager.GetInstance().SetDashboard(this);

        }

        public void UpdateDevices(List<string> devicesID) {

            List<IDevice> devices_to_remove = new List<IDevice>();

            // remove old devices
            foreach (IDevice device in devices) {
                if (!devicesID.Contains(device.deviceElement.Id)) {
                    devices_to_remove.Add(device);
                }
            }

            foreach (IDevice device in devices_to_remove) {
                devices.Remove(device);
            }

            // add new devices
            foreach (string deviceID in devicesID) {
                devices.Add(new AdafruitDevice(deviceID, deviceID));
            }

            SetDevicesStackPanel();
        }

        private void SetDevicesStackPanel() {
            DeviceStackPanel.Children.Clear();
            foreach (IDevice device in devices) {
                device.deviceElement.Height = 40;
                device.deviceElement.MouseDown += ClickDevice;

                DeviceStackPanel.Children.Add(device.deviceElement);
            }
        }

        public void ClickDevice(object sender, RoutedEventArgs e) {

            PageManager.GetInstance().device.device.DataReceived -= PageManager.GetInstance().device.OnDataReceived;

            PageManager.GetInstance().device.device = (IDevice)((DeviceElement)sender).Tag;
            PageManager.GetInstance().device.device.DataReceived += PageManager.GetInstance().device.OnDataReceived;

            PageManager.GetInstance().device.LbId.Content = ((IDevice)((DeviceElement)sender).Tag).deviceElement.Id;
            PageManager.GetInstance().device.LbName.Content = ((IDevice)((DeviceElement)sender).Tag).deviceElement.DeviceName;
            PageManager.GetInstance().device.LbTemp.Content = ((IDevice)((DeviceElement)sender).Tag).Temperature;

            Storyboard storyboard = PageManager.GetInstance().mainWindow.FindResource("GoToDevice") as Storyboard;
            storyboard.Begin();
        }
    }
}

using OTSF_Application.Devices;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OTSF_Application.Pages {
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page {
        AdaFruitSettingsManager adaFruitSettingsManager;
        public Settings() {
            InitializeComponent();

            adaFruitSettingsManager = AdaFruitSettingsManager.GetInstance();

            AdaUsername.Text = adaFruitSettingsManager.Username;
            AdaAppId.Text = adaFruitSettingsManager.ApplicationID;
            AdaServer.Text = adaFruitSettingsManager.Server;
            AdaPassword.Password = adaFruitSettingsManager.Password;

            LbDevices.ItemsSource = adaFruitSettingsManager.Devices;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            AdaFruitSettingsManager.GetInstance().Username = AdaUsername.Text;
            AdaFruitSettingsManager.GetInstance().Password = AdaPassword.Password;
            AdaFruitSettingsManager.GetInstance().Server = AdaServer.Text;
            AdaFruitSettingsManager.GetInstance().ApplicationID = AdaAppId.Text;

            AdaFruitSettingsManager.GetInstance().Save();
        }

        private void BtRemove_Click(object sender, RoutedEventArgs e) {

            string SelectedDevice = (string)LbDevices.SelectedItem;

            adaFruitSettingsManager.Devices.Remove(SelectedDevice);

            LbDevices.ItemsSource = null;

            LbDevices.ItemsSource = adaFruitSettingsManager.Devices;

            adaFruitSettingsManager.Save();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e) {

            string SelectedDevice = TbDevice.Text;

            TbDevice.Text = "";

            adaFruitSettingsManager.Devices.Add(SelectedDevice);

            LbDevices.ItemsSource = null;

            LbDevices.ItemsSource = adaFruitSettingsManager.Devices;

            adaFruitSettingsManager.Save();

        }
    }
}

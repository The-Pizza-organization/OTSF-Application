using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace OTSF_Application.Elements {
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DeviceElement : UserControl {

        private string _id;
        public string Id {
            get { return _id; } set {
                _id = value;
                tbID.Text = _id;
        } }

        private string _name;
        public string DeviceName { get { return _name; } set {
                _name = value;
                tbName.Text = _name;
        } }

        private DeviceStatus _status;
        public DeviceStatus DeviceStatus { get { return _status; } set { 
                _status = value;
                bStatus.Background = StatusColor;
        } }

        public Brush StatusColor { get {

                switch (DeviceStatus) {
                    case DeviceStatus.Connected:
                        return Brushes.Green;
                    case DeviceStatus.Disconnected:
                        return Brushes.Black;
                    case DeviceStatus.Error:
                        return Brushes.Red;
                    case DeviceStatus.Warning: 
                        return Brushes.Yellow;
                    default:
                        return Brushes.OrangeRed;
                }

            }
        }

        public DeviceElement() {
            InitializeComponent();

            Id = "[ID]";
            DeviceName = "[DEVICE NAME]";

        }
    }
}

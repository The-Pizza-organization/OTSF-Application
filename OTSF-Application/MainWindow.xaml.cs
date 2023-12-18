using Microsoft.Maps.MapControl.WPF;
using OTSF_Application.MQTT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml.Linq;

namespace OTSF_Application {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        PageManager pageManager;

        Thread thread;

        public MainWindow() {

            InitializeComponent();

            pageManager = PageManager.GetInstance(this);

            
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
            thread = new(LateResize);
            thread.IsBackground = true;
            thread.Start();
        }

        private void LateResize() {

            Thread.Sleep(500);
            this.Dispatcher.Invoke((() => {
                Storyboard storyboard = FindResource("GoToDashboard") as Storyboard;
                storyboard.Begin();
            }));
            
        }
    }
}

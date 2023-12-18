using LiveCharts.Wpf;
using LiveCharts;
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
    /// Interaction logic for DevicePage.xaml
    /// </summary>
    public partial class DevicePage : Page {

        public IDevice device;

        private string last_id = "";

        public DevicePage() {
            InitializeComponent();

            device = new FakeDevice("H.Saint pierre");

            SeriesCollection AccelerationCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "X",
                    Values = new ChartValues<float> { 0 },
                    Fill = Brushes.Transparent,
                    PointGeometry = null,
                },
                new LineSeries
                {
                    Title = "Y",
                    Values = new ChartValues<float> { 0 },
                    Fill = Brushes.Transparent,
                    PointGeometry = null,
                },
                new LineSeries
                {
                    Title = "Z",
                    Values = new ChartValues<float> { 0 },
                    Fill = Brushes.Transparent,
                    PointGeometry = null,
                }
            };

            SeriesCollection RotationCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Pitch",
                    Values = new ChartValues<float> { 0 },
                    Fill = Brushes.Transparent,
                    PointGeometry = null,
                },
                new LineSeries
                {
                    Title = "Roll",
                    Values = new ChartValues<float> { 0 },
                    Fill = Brushes.Transparent,
                    PointGeometry = null,
                },
                new LineSeries
                {
                    Title = "Yaw",
                    Values = new ChartValues<float> { 0 },
                    Fill = Brushes.Transparent,
                    PointGeometry = null,
                }
            };

            SeriesCollection TemperatureCollection = new SeriesCollection {

                new LineSeries
                {
                    Title = "Temperatures",
                    Values = new ChartValues<float> { 0 },
                    PointGeometry = null,
                },

            };
            
            AccelerationGraph.Series = AccelerationCollection;
            RotationGraph.Series = RotationCollection;
            TemperatureGraph.Series = TemperatureCollection;

            ResetSeries();

            device.DataReceived += OnDataReceived;

            PageManager.GetInstance().SetDevice(this);
        }

        private void ResetSeries() {

            try {
                foreach (var series in AccelerationGraph.Series) {
                    series.Values.Clear();

                    for (int i = 0; i < 50; i++) {
                        series.Values.Add(0f);
                    }

                }

                foreach (var series in RotationGraph.Series) {
                    series.Values.Clear();

                    for (int i = 0; i < 50; i++) {
                        series.Values.Add(0f);
                    }
                }

                foreach (var series in TemperatureGraph.Series) {
                    series.Values.Clear();

                    for (int i = 0; i < 50; i++) {
                        series.Values.Add(0f);
                    }
                }
            } catch { }

            
        }
        public void OnDataReceived(object? sender, DeviceData e) {

            AccelerationGraph.Series[0].Values.Add(e.a_x);
            AccelerationGraph.Series[1].Values.Add(e.a_y);
            AccelerationGraph.Series[2].Values.Add(e.a_z);

            foreach (var series in AccelerationGraph.Series) {
                if (series.Values.Count > 50) {
                    series.Values.RemoveAt(0);
                }
            }

            RotationGraph.Series[0].Values.Add(e.pitch);
            RotationGraph.Series[1].Values.Add(e.roll);
            RotationGraph.Series[2].Values.Add(e.yaw);

            foreach (var series in RotationGraph.Series) {
                if (series.Values.Count > 50) {
                    series.Values.RemoveAt(0);
                }
            }

            TemperatureGraph.Series[0].Values.Add(e.temperature);

            foreach (var series in TemperatureGraph.Series) {
                if (series.Values.Count > 50) {
                    series.Values.RemoveAt(0);
                }
            }

            if(last_id != device.deviceElement.Id) {
                last_id = device.deviceElement.Id;
                //clear all series
                ResetSeries();
            }

            this.Dispatcher.Invoke((() => {
                LbTemp.Content = $"{e.temperature.ToString("F2")} °C";

                if(device.HasDoorBeenOpened) {
                    LbDoor.Content = "Open";
                    LbDoor.Foreground = Brushes.Red;
                    LbDate.Content = device.OpenedAt.ToString("G");
                } else {
                    LbDoor.Content = "Closed";
                    LbDate.Content = "";
                    LbDoor.Foreground = Brushes.Green;
                }

                LbLastDate.Content = $"Last update:{DateTime.Now.ToString("G")}";

                LbId.Content = device.deviceElement.Id;
                LbName.Content = device.deviceElement.DeviceName;

                Map.Center = new Microsoft.Maps.MapControl.WPF.Location(e.latitude, e.longitude);
            }));
        }

    }
}

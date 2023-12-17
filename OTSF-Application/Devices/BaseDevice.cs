using OTSF_Application.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OTSF_Application.Devices {
    public class BaseDevice : IDevice {

        public EventHandler<DeviceData> DataReceived { get; set; }

        Thread StatusThread;

        public DeviceElement deviceElement {get;set;}
        public float Temperature {get;set;}
        public Vector3 acceleration {get;set;}
        public Vector3 rotation_speed {get;set;}
        public bool HasDoorBeenOpened {get;set;}
        public float latitude {get;set;}
        public float longitude {get;set;}

        private DateTime last_data_update;

        public BaseDevice() {
            deviceElement = new DeviceElement();
            deviceElement.Tag = this;

            last_data_update = DateTime.Now;

            Temperature_history = new float[100];
            acceleration_history = new Vector3[100];
            rotation_speed_history = new Vector3[100];

            DataReceived += OnDataReceived;

            StatusThread = new Thread(StatusThreadFunction);
            StatusThread.IsBackground = true;
            StatusThread.Start();

            HasDoorBeenOpened = false;

        }


        private void StatusThreadFunction() {
            while (true) {
                Thread.Sleep(500);

                TimeSpan time_from_last = DateTime.Now - last_data_update;

                if (HasDoorBeenOpened) {
                    deviceElement.DeviceStatus = DeviceStatus.Warning;
                    continue;
                }

                if (time_from_last < TimeSpan.FromSeconds(2)) {
                    deviceElement.DeviceStatus = DeviceStatus.Connected;
                }else if (time_from_last < TimeSpan.FromSeconds(15)) {
                    deviceElement.DeviceStatus = DeviceStatus.Error;
                }else {
                    deviceElement.DeviceStatus = DeviceStatus.Disconnected;
                }

                
            }
        }

        public float[] Temperature_history { get; set; }
        public Vector3[] acceleration_history { get; set; }
        public Vector3[] rotation_speed_history { get; set; }

        public float[] a_x { get {
                return acceleration_history.Select(x => x.X).ToArray();
        } }

        public float[] a_y {
            get {
                return acceleration_history.Select(x => x.Y).ToArray();
            }
        }

        public float[] a_z {
            get {
                return acceleration_history.Select(x => x.Z).ToArray();
            }
        }

        public float[] g_x {
            get {
                return rotation_speed_history.Select(x => x.X).ToArray();
            }
        }

        public float[] g_y {
            get {
                return rotation_speed_history.Select(x => x.Y).ToArray();
            }
        }

        public float[] g_z {
            get {
                return rotation_speed_history.Select(x => x.Z).ToArray();
            }
        }

        private void SetValueHistory() {

            //shift values
            for (int i = 0; i < Temperature_history.Length - 1; i++) {
                Temperature_history[i] = Temperature_history[i + 1];
                acceleration_history[i] = acceleration_history[i + 1];
                rotation_speed_history[i] = rotation_speed_history[i + 1];
            }

            //add new values
            Temperature_history[Temperature_history.Length - 1] = Temperature;
            acceleration_history[acceleration_history.Length - 1] = acceleration;
            rotation_speed_history[rotation_speed_history.Length - 1] = rotation_speed;
        }

        private void OnDataReceived(object? sender, DeviceData e) {

            //set status to warning if no data has been received for 10 seconds

            if (DateTime.Now - last_data_update > TimeSpan.FromSeconds(10)) {
                deviceElement.DeviceStatus = DeviceStatus.Warning;
            } else {
                deviceElement.DeviceStatus = DeviceStatus.Connected;
            }

            last_data_update = DateTime.Now;

            // Update data
            Temperature = e.temperature;
            acceleration = new Vector3(e.a_x, e.a_y, e.a_z);
            rotation_speed = new Vector3(e.pitch, e.roll, e.yaw);

            //once true -> always true
            if (!HasDoorBeenOpened && e.isDoorOpen) {
                HasDoorBeenOpened = true;
            }

            latitude = e.latitude;
            longitude = e.longitude;

            SetValueHistory();
        }

    }
}

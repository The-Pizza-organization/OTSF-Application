using OTSF_Application.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OTSF_Application.Devices {
    public class FakeDevice : BaseDevice {

        Thread FakeDataGenerator;


        private float error_probability;

        private static int Counter = 1;

        public FakeDevice( string name ,float error_probability = 0 ) {
            deviceElement.Id = Counter.ToString();
            Counter++;

            deviceElement.DeviceName = name;
        
            this.error_probability = error_probability;

            FakeDataGenerator = new Thread(GenerateData);
            FakeDataGenerator.IsBackground = true;

            FakeDataGenerator.Start();
        }

        private void GenerateData() {

            Random random = new Random();
            SpecialRandom specialRandom = new();

            specialRandom.SetPreviousData("latitude", 45.5f);
            specialRandom.SetPreviousData("longitude", 2f);
            specialRandom.SetPreviousData("temp", 20f);

            specialRandom.SetPreviousData("a_x", 0f);
            specialRandom.SetPreviousData("a_y", 0f);
            specialRandom.SetPreviousData("a_z", 1f);

            specialRandom.SetPreviousData("pitch", 0f);
            specialRandom.SetPreviousData("roll", 0f);
            specialRandom.SetPreviousData("yaw", 0f);


            while (true) {

                // Generate random data every 2 seconds
                Thread.Sleep(500);

                if (random.NextDouble() < error_probability) {
                    continue;
                }

                // Generate random data
                DeviceData data = new DeviceData();

                data.temperature = specialRandom.GetNearRandom("temp",0.1f);

                data.pitch = specialRandom.GetRandom("pitch", 0.1f);
                data.roll = specialRandom.GetRandom("roll", 0.1f);
                data.yaw = specialRandom.GetRandom("yaw", 0.1f);

                data.isDoorOpen = random.NextDouble() > 0.5;

                // Generate latitude and longitude, the new value is always near the last
                data.latitude = specialRandom.GetNearRandom("latitude", 0.0001f);
                data.longitude = specialRandom.GetNearRandom("longitude", 0.0001f);

                data.a_x = specialRandom.GetRandom("a_x", 1f);
                data.a_y = specialRandom.GetRandom("a_y", 1f);
                data.a_z = specialRandom.GetNearRandom("a_z", 0.01f);

                // invoke the DataReceived event
                DataReceived?.Invoke(this, data);
            }
        }
    }

    class SpecialRandom {

        Random rnd = new Random();

        Dictionary<string, float> data = new Dictionary<string, float>();

        public void SetPreviousData(string key, float value) {
            if (data.ContainsKey(key)) {
                data[key] = value;
            } else {
                data.Add(key, value);
            }
        }

        public float GetNearRandom(string key, float amplitude = 1f) {

            float previous = ((float)(rnd.NextDouble() - 0.5) * 2);
            if (data.ContainsKey(key)) {
                previous = data[key];
            } else {
                data.Add(key, previous);
            }

            float new_value = previous + (((float)(rnd.NextDouble()-0.5)*2) * amplitude);
            data[key] = new_value;

            return new_value;
        }

        public float GetRandom(string key, float amplitude = 1f) {

            float new_value = ((float)(rnd.NextDouble() - 0.5) * 2) * amplitude;
            data[key] = new_value;

            return new_value;
        }

    }
}

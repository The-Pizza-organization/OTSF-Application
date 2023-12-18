using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mqtt;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OTSF_Application.MQTT {
    internal class MClient {

        public EventHandler<DeviceData> DataReceived;


        public async void ConnectAsync(string server,string appId,string clientId, string userName,string password,string deviceID) {
            var configuration = new MqttConfiguration {
                BufferSize = 128 * 1024,
                Port = 1883,
                KeepAliveSecs = 10,
                WaitTimeoutSecs = 2,
                MaximumQualityOfService = MqttQualityOfService.AtMostOnce,
                AllowWildcardsInTopicFilters = true
            };

            var client = await MqttClient.CreateAsync(server, configuration);

            try {
                var sessionState = await client.ConnectAsync(new MqttClientCredentials(clientId, userName, password));

                await client.SubscribeAsync($"v3/{appId}/devices/{deviceID}/up", MqttQualityOfService.ExactlyOnce);


                client.MessageStream.Subscribe(msg => NewMessage(msg, msg.Payload));
            } catch {
                MessageBox.Show($"Adafruit Device {deviceID} not connected");
            }

        }

        private void NewMessage(MqttApplicationMessage message,byte[] payload) {

            string decodedPayload = Encoding.UTF8.GetString(payload);

            // Convert decodedPayload to JSON
            var json = JsonNode.Parse(decodedPayload);
            // Get the data field
            JsonNode? data = json["uplink_message"]["decoded_payload"];
            JsonNode? location = json["uplink_message"]["locations"]["frm-payload"];

            // Create a new DeviceData object
            DeviceData deviceData = new DeviceData();

            // Set the properties of the DeviceData object
            if (data == null || data["temperature"] == null) {
                return;
            }
            deviceData.temperature = (float)data["temperature"];
            deviceData.pitch = (float)data["g_x"];
            deviceData.roll = (float)data["g_y"];
            deviceData.yaw = (float)data["g_z"];
            deviceData.isDoorOpen = (bool)data["isDoorOpen"];
            deviceData.latitude = (float)data["latitude"];
            deviceData.longitude = (float)data["longitude"];
            deviceData.a_x = (float)data["a_x"];
            deviceData.a_y = (float)data["a_y"];
            deviceData.a_z = (float)data["a_z"];

            if (location != null) {
                deviceData.latitude = (float)location["latitude"];
                deviceData.longitude = (float)location["longitude"];
            }

            // Invoke the DataReceived event
            DataReceived?.Invoke(this, deviceData);
        }
    }
}

using OTSF_Application.MQTT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace OTSF_Application.Devices {
    internal class AdafruitDevice : BaseDevice {


        private MClient client;
        
        public AdafruitDevice(string DeviceID,string Name) {

            AdaFruitSettingsManager AFS= AdaFruitSettingsManager.GetInstance();


            try {
                this.deviceElement.Id = DeviceID.Substring(4, 5);
            } catch {
                this.deviceElement.Id = DeviceID;
            }
            this.deviceElement.DeviceName = Name;

            client = new MClient();

            try {
                client.ConnectAsync(AFS.Server,AFS.ApplicationID ,AFS.Username, AFS.ApplicationID, AFS.Password, DeviceID);
                client.DataReceived += Client_DataReceived;
            } catch {
                MessageBox.Show($"Adafruit Device {DeviceID} not connected");
            }

        }

        private void Client_DataReceived(object sender, DeviceData e) {
            DataReceived?.Invoke(this, e);
        }


    }

    public class AdaFruitSettings {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApplicationID { get; set; }
        public List<string> Devices { get; set; }
    }

    public class AdaFruitSettingsManager {

        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string ApplicationID { get; set; }
        public List<string> Devices { get; set; }

        const string SETTINGS_FILE = "AdaFruitSettings.json";

        private static AdaFruitSettingsManager? instance;

        public static AdaFruitSettingsManager GetInstance() {
            if (instance == null)
                instance = new AdaFruitSettingsManager();
            return instance;
        }

        private AdaFruitSettingsManager() { 

            if(File.Exists(SETTINGS_FILE)) {
                string json = File.ReadAllText(SETTINGS_FILE);

                AdaFruitSettings settings = JsonSerializer.Deserialize<AdaFruitSettings>(json);
                Server = settings.Server;
                Username = settings.Username;
                Password = settings.Password;
                ApplicationID = settings.ApplicationID;
                Devices = settings.Devices;
            }
            else {
                Server = "";
                Username = "";
                Password = "";
                ApplicationID = "";
                Devices = new List<string>();
            }

        }

        public void Save() {
            string json = JsonSerializer.Serialize(new AdaFruitSettings {
                ApplicationID = ApplicationID,
                Devices = Devices,
                Password = Password,
                Server = Server,
                Username = Username
            });

            PageManager.GetInstance().dashboard.UpdateDevices(Devices);

            File.WriteAllText(SETTINGS_FILE, json);
        }
    }
}

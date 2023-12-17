using OTSF_Application.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OTSF_Application
{
    class PageManager{

        public MainWindow mainWindow;
        public Dashboard? dashboard;
        public Settings? settings;
        public DevicePage? device;


        private static PageManager Instance; 

        private PageManager(MainWindow mainWindow) {
            this.mainWindow = mainWindow;
        }

        public static PageManager GetInstance(MainWindow? mainWindow = null) {
            if (Instance == null) {
                if (mainWindow == null)
                    throw new Exception("PageManager not initialized");

                Instance = new PageManager(mainWindow);
            }
            return Instance;
        }

        public void SetDashboard(Dashboard dashboard) {
            this.dashboard = dashboard;
        }

        public void SetSettings(Settings settings) {
            this.settings = settings;
        }
        public void SetDevice(DevicePage device) {
            this.device = device;
        }
    }
}

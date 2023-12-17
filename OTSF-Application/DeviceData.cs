using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSF_Application {
    public struct DeviceData {

        /*
              "a_x": 0,
              "a_y": 0,
              "a_z": 0,
              "g_x": 0.8515990805261412,
              "g_y": 0.9659858889684686,
              "g_z": 0.20805512028142975,
              "isDoorOpen": false,
              "latitude": 48.12062319549441,
              "longitude": 3.7929995204137334,
              "temperature": 0
         */

        public float temperature { get; set; }
        public float pitch { get; set; }
        public float roll { get; set; }
        public float yaw { get; set; }
        public bool isDoorOpen { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public float a_x { get; set; }
        public float a_y { get; set; }
        public float a_z { get; set; }

    }
}

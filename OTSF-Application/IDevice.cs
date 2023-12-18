using OTSF_Application.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OTSF_Application {
    public interface IDevice {

        EventHandler<DeviceData> DataReceived { get; set; }

        DeviceElement deviceElement { get; set; }

        float Temperature { get; set; }

        Vector3 acceleration { get; set; }

        Vector3 rotation_speed { get; set; }

        bool HasDoorBeenOpened { get; set; }
        DateTime OpenedAt { get; set; }

        float latitude { get; set; }
        float longitude { get; set; }

        public float[] Temperature_history { get; set; }
        public Vector3[] acceleration_history { get; set; }
        public Vector3[] rotation_speed_history { get; set; }

        public float[] a_x {
            get;
        }

        public float[] a_y {
            get;
        }

        public float[] a_z {
            get;
        }

        public float[] g_x {
            get;
        }

        public float[] g_y {
            get;
        }

        public float[] g_z {
            get;
        }

    }
}

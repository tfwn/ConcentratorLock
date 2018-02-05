using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model.POCO
{
    public class ps_LockSensorData
    {
        public int LSD_Id { get; set; }
        public string LSD_GateWayNo { get; set; }
        public int LSD_LC_Id { get; set; }
        public string LSD_LockNo { get; set; }
        public int LSD_SensorA { get; set; }
        public int LSD_SensorB { get; set; }
        public int LSD_SensorC { get; set; }
        public DateTime LSD_CreateTime { get; set; }
    }
}

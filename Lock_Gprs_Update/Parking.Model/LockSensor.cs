using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
   public  class LockSensor
    {
        /// <summary>
        /// 地锁id，16位(8字节)
        /// </summary>
        public string LockNo { get; set; }


        /// <summary>
        /// 传感器1检测值
        /// </summary>
        public UInt16 SensorA { get; set; }

        /// <summary>
        /// 传感器2检测值
        /// </summary>
        public UInt16 SensorB { get; set; }

        /// <summary>
        /// 传感器3检测值
        /// </summary>
        public Byte SensorC { get; set; }

        public string GateWayNo { get; set; }

    }
}

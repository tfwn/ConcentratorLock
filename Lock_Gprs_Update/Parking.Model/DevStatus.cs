using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{

    public  class LockStatus
    {
        /// <summary>
        /// 地锁id，16位(8字节)
        /// </summary>
        public string LockNo { get; set; }

        /// <summary>
        /// 数据类型,1个字节：0开锁；1关锁；2报警
        /// </summary>
        public Byte DataType { get; set; }

        /// <summary>
        /// 操作账户11Byte
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 蓝牙id ，12位(6字节)
        /// </summary>
        public string BluetoothId { get; set; }

        /// <summary>
        /// 6位地锁信息上报时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 电池电压
        /// </summary>
        public string Voltage { get; set; }

        /// <summary>
        /// 8位(4字节)状态信息
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 上行信号
        /// </summary>
        public Byte UpSignal { get; set; }

        /// <summary>
        /// 下行信号
        /// </summary>
        public Byte DownSignal { get; set; }

        public string GateWayNo { get; set; }
    }
}

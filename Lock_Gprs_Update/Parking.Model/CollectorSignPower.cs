using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
    /// <summary>
    /// 集中器信号强度
    /// </summary>
    public class ResCollectorSignPower
    {
        /// <summary>
        /// 模组的信号强度
        /// </summary>
        public Byte SIGNPOWER { get; set; }
        /// <summary>
        /// 集中器联网状态,0离线；1在线
        /// </summary>
        public byte ONLINE { get; set; }

        /// <summary>
        /// IMSI的字符个数
        /// </summary>
        public Byte IMSI_LEN { get; set; }
        /// <summary>
        /// IMSI的字符内容
        /// </summary>
        public string IMSI_CONTENT { get; set; }
        /// <summary>
        /// 模组型号的字符个数
        /// </summary>
        public Byte MODULE_LEN { get; set; }

        /// <summary>
        /// 模组型号的字符内容
        /// </summary>
        public string MODULE_CONTENT { get; set; }

    }
}

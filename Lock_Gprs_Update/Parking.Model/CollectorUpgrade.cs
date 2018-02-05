using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
    public class CollectorUpgrade
    {
        /// </summary>
        public string CollectorNo { get; set; }

        public string UserName { get; set; }

        public string Pass { get; set; }

        /// <summary>
        /// 升级代码所有字节的CRC16计算值
        /// </summary>
        public Byte[] FILE_CRC16 { get; set; }

        /// <summary>
        /// 偏移地址（从0开始）
        /// </summary>
        public UInt32 FILE_OFFSET { get; set; }

        /// <summary>
        /// 升级代码总的字节数
        /// </summary>
        public UInt32 FILE_LEN { get; set; }

        /// <summary>
        /// 本包升级代码长度，必须为偶数
        /// </summary>
        public UInt16 CODE_LEN {get;set;}

        /// <summary>
        /// 升级代码
        /// </summary>
        public Byte[] CODE_CONTENT { get; set; }   


        public Byte RESULT { get; set; } 
    }
}

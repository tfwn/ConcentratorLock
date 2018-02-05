using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
    public class ReqDownEncryptKey
    {
        /// <summary>
        /// 16位Hex字符的集中器id
        /// </summary>
        public string CollectorNo { get; set; }

        public string UserName { get; set; }

        public string Pass { get; set; }

        /// <summary>
        /// 若上一条命令未下发或下发失败，当前命令的操作方式（是否覆盖上条命令）
        /// </summary>
        public int RecentCmdFailOp { get; set; }

        /// <summary>
        /// 是否回复上一条命令信息给服务器标志位
        /// </summary>
        public bool ReCmd { get; set; }

        /// <summary>
        /// 16位Hex字符的地锁No
        /// </summary>
        public string LockNo { get; set; }
        
      
        public ushort KeyId { get; set; }

        /// <summary>
        /// 随机数
        /// </summary>
        public UInt16 RoundId { get; set; }

        /// <summary>
        /// 签名,Md5
        /// </summary>
        public string Sign { get; set; }
    }


    public class ResDownEncryptKey
    {
        public string Result { get; set; }

        /// <summary>
        /// 上一条命令的名字字
        /// </summary>
        public string RecentCmd { get; set; }

        /// <summary>
        /// 上一条命令下发状态
        /// </summary>
        public Byte RecentDownStatus { get; set; }

        /// <summary>
        /// 上一条命令的数据域长度
        /// </summary>
        public Byte RecentDataLength { get; set; }

        /// <summary>
        /// 上一条命令的数据域数据
        /// </summary>
        public ReqDownEncryptKey RecentReqData { get; set; }
    }
}

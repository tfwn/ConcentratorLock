using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
    public class ReqCollector
    {
        /// <summary>
        /// 16位Hex字符的集中器id
        /// </summary>
        public string CollectorNo { get; set; }

        public string UserName { get; set; }

        public string Pass { get; set; }

    }
    /// <summary>
    /// 集中器参数
    /// </summary>
    public class CollectorParam
    {
        public string CollectorNo { get; set; }

        public string UserName { get; set; }

        public string Pass { get; set; }

        /// <summary>
        /// 集中器连接服务器主站的主IP地址,4Byte
        /// </summary>
        public string M_DSC_IP { get; set; }

        /// <summary>
        /// 集中器连接服务器主站的主端口
        /// </summary>
        public UInt16 M_DSC_PORT { get; set; }

        /// <summary>
        /// 集中器连接服务器主站的备用IP地址,4个字节
        /// </summary>
        public string B_DSC_IP { get; set; }

        /// <summary>
        /// 集中器连接服务器主站的备用端口
        /// </summary>
        public UInt16 B_DSC_PORT { get; set; }

        /// <summary>
        /// 集中器发送心跳间隔。单位10秒
        /// </summary>
        public Byte HEARTBEAT { get; set; }

        /// <summary>
        /// 网络拨号APN的字符个数
        /// </summary>
        public Byte APN_LEN {get;set;}

        /// <summary>
        /// 网络拨号APN的字符内容
        /// </summary>
        public string APN_CONTENT { get; set; }

        /// <summary>
        /// 网络拨号APN用户的字符个数
        /// </summary>
        public Byte APN_USER_LEN { get; set; }

        /// <summary>
        /// 网络拨号APN用户的字符内容
        /// </summary>
        public string APN_USER_CONTENT { get; set; }

        /// <summary>
        /// 网络拨号APN密码的字符个数
        /// </summary>
        public byte APN_PASS_LEN { get; set; }
        /// <summary>
        /// 网络拨号APN密码的字符内容
        /// </summary>
        public string APN_PASS_CONTENT { get; set; }


    }
}

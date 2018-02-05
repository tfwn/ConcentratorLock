using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
    public class ReqUser
    {
        /// <summary>
        /// 上传方式0、init状态上报，1、change 状态上报
        /// </summary>
        public string Type { get; set; }

        public string AccountNo { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public string LockNo { get; set; }

        /// <summary>
        /// 蓝牙地址
        /// </summary>
        public string BleMac { get; set; }

        public string KeyId { get; set; }

        public string Voltage { get; set; }

        public string Status { get; set; }

        public string OldLockNo { get; set; }

        public string NewLockNo { get; set; }

        public string NewBleMac { get; set; }


    }

    public class ResUser
    {
        public string AccountNo { get; set; }

        public int StatusCode { get; set; }

        public string Token { get; set; }
    }
}

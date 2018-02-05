using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
     public class Header
    {
        public string Key { get; set; }
        /// <summary>
        /// 命令字
        /// </summary>
        public Byte Cmd { get; set; }

        /// <summary>
        /// 关联id1
        /// </summary>
        public Byte RId1 { get; set; }

        /// <summary>
        /// 关联id2
        /// </summary>
        public Byte RId2 { get; set; }

        public string DtuId { get; set; }

      public string UserName { get; set; }

        public DateTime inTime
        {
            get;set;
        }

        /// <summary>
        /// MessageBodyLength 本消息主体的长度
        /// </summary>
        public int MessageBodyLength { get; set; }
    }


}

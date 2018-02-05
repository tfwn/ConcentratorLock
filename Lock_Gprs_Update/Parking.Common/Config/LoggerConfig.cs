using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Parking.Common.Config
{
    public class LoggerConfig
    {
        /// <summary>
        /// 写日志文件最大容量，单位M
        /// </summary>
        [XmlElement("MaxFileSize")]
        public string MaxFileSize { get; set; }

        /// <summary>
        /// 写日志级别
        /// </summary>
        [XmlElement("LogLevel")]
        public string LogLevel { get; set; }

        /// <summary>
        /// 读Log队列间隔时间，单位ms
        /// </summary>
        [XmlElement("ReadLogQueueInterval")]
        public string ReadLogQueueInterval { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Parking.Common.Config
{
    [XmlRoot("Configs")]
    public class AllConfig
    {
        [XmlElement("Logger")]
        public LoggerConfig LoggerConfig { get; set; }

        [XmlElement("ValidateUser")]
        public ValidateUserConfig ValidateUserConfig { get; set; }
    }
}

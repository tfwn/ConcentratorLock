using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Parking.Common.Config
{
    public class ValidateUserConfig
    {
        /// <summary>
        /// 是否验证用户名密码:1验证，0不验证
        /// </summary>
        [XmlElement("IsValidate")]
        public string IsValidate { get; set; }


        [XmlElement("UserName")]
        public string UserName { get; set; }

        [XmlElement("Pass")]
        public string Pass { get; set; }
    }
}

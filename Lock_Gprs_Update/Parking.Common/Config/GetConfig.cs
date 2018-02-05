using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Parking.Common.Config
{
    public class GetConfig
    {
        public static AllConfig GetAllConfig()
        {
            if (HttpContext.Current != null)
            {
                if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/config/main.config")))
                {
                    throw (new System.IO.FileNotFoundException());
                }
                return XmlUtility.XmlDeserailize<AllConfig>(HttpContext.Current.Server.MapPath("~/config/main.config"));
            }
            else
            {
                if (!System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "config\\main.config"))
                {
                    throw (new System.IO.FileNotFoundException());
                }
                return XmlUtility.XmlDeserailize<AllConfig>(System.AppDomain.CurrentDomain.BaseDirectory + "config\\main.config");
            }

        }
    }
}

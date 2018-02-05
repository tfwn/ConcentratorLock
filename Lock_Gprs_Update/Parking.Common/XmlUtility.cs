using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Parking.Common
{
    public class XmlUtility
    {
        /// <summary>  
        /// 将xml配置文件反序列化为类  
        /// </summary>  
        /// <param name="xml"></param>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static T XmlDeserailize<T>(String xmFile)
        {
            using (FileStream rdr = new FileStream(xmFile, FileMode.Open, FileAccess.Read))
            {
                //声明序列化对象实例serializer 
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                //反序列化，并将反序列化结果值赋给变量i
                var obj = (T)serializer.Deserialize(rdr);
                //输出反序列化结果
                return obj;
            }
        }

        /// <summary>
        /// 将xml字符串反序列化为类  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T XmlDeserailizeFromString<T>(string str)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                return (T)serializer.Deserialize(stream);
            }
        }

 

 



        /// <summary>  
        /// 将类序列化为xml文本  
        /// </summary>  
        /// <param name="type">要被序列化的对象的type</param>  
        /// <param name="isDflg">序列化时候是否进行缩进格式化</param>  
        /// <returns></returns>  
        public static string XmlSerialize<T>(T obj)
        {
            string str;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringWriter writer = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true
                };
                using (XmlWriter writer2 = XmlWriter.Create(writer, settings))
                {
                    serializer.Serialize(writer2, obj, new XmlSerializerNamespaces(new XmlQualifiedName[] { XmlQualifiedName.Empty }));
                    str = writer.ToString();
                }
            }
            return str;
        }

 

    }  
}

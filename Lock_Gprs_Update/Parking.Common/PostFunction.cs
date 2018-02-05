using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Parking.Common
{
    public class PostFunction
    {
        public string PostInterface(string url, string json)
        {
            //try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ServicePoint.ConnectionLimit = int.MaxValue;
                request.Method = "post";
                request.Timeout = 60000;
                
                request.Accept = "text/html, application/xhtml+xml, */*";
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json";
                
                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.UseNagleAlgorithm = false;
                request.AllowWriteStreamBuffering = false;

                byte[] buffer = encoding.GetBytes(json);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                string resultStr = string.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
                    {
                        resultStr = reader.ReadToEnd();
                    }
                    if (response != null)
                        response.Close();
                }
                return resultStr;
            }
            //catch (Exception ex)
            {
               // throw ex;
                //return ex.Message;
            }
        }

        public string PostInterface2(string url, string json, int timeout)
        {
            //try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ServicePoint.ConnectionLimit = int.MaxValue;
                request.Method = "post";
                request.Timeout = timeout;
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/x-www-form-urlencoded";
                
                //request.ContentType = "application/json";

                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.UseNagleAlgorithm = false;
                request.AllowWriteStreamBuffering = false;

                byte[] buffer = encoding.GetBytes(json);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                string resultStr = string.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
                    {
                        resultStr = reader.ReadToEnd();
                    }
                    if (response != null)
                        response.Close();
                }
                return resultStr;
            }
            //catch (Exception ex)
            {
                // throw ex;
                //return ex.Message;
            }
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="timeout">毫秒</param>
        /// <returns></returns>
        public string PostInterface(string url, string json,int timeout)
        {
            //try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ServicePoint.ConnectionLimit = int.MaxValue;
                request.Method = "post";
                request.Timeout = timeout;

                request.Accept = "text/html, application/xhtml+xml, */*";
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json";

                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.UseNagleAlgorithm = false;
                request.AllowWriteStreamBuffering = false;

                byte[] buffer = encoding.GetBytes(json);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                string resultStr = string.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
                    {
                        resultStr = reader.ReadToEnd();
                    }
                    if (response != null)
                        response.Close();
                }
                return resultStr;
            }
            //catch (Exception ex)
            {
                // throw ex;
                //return ex.Message;
            }
        }

        public string Get(string url)
        {
            HttpWebRequest objWebRequest = null;

            objWebRequest = (HttpWebRequest)WebRequest.Create(url);

            objWebRequest.Method = "GET";
            objWebRequest.Timeout = 60000;
            objWebRequest.ContentType = "application/x-www-form-urlencoded";
            string textResponse="";
            using(var  response = (HttpWebResponse)objWebRequest.GetResponse())
            {
                using(StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
                {
                    textResponse = sr.ReadToEnd();
                }
                response.Close();
            }
            return textResponse;
        }

        public string Get(string url, System.Text.Encoding encoding)
        {
            HttpWebRequest objWebRequest = null;

            objWebRequest = (HttpWebRequest)WebRequest.Create(url);

            objWebRequest.Method = "GET";
            objWebRequest.Timeout = 60000;
            string textResponse;
            objWebRequest.ContentType = "application/x-www-form-urlencoded";

            using (var response = (HttpWebResponse)objWebRequest.GetResponse())
            {
                using(StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
                {
                    textResponse = sr.ReadToEnd();
                }
                response.Close();
            }
            

            return textResponse;
        }


        public List<T> DeserializeFromStringToList<T>(string value)
        {
           // return new JavaScriptSerializer().Deserialize<List<T>>(value);
            return ServiceStack.Text.JsonSerializer.DeserializeFromString<List<T>>(value);
        }

        public T DeserializeFromString<T>(string value)
        {
            //return new JavaScriptSerializer().Deserialize<T>(value);
            return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
        }

        /// <summary>
        /// 将对象序列化JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SerializeStringByObject(object obj) 
        {
            //return new JavaScriptSerializer().Serialize(obj);
            return ServiceStack.Text.JsonSerializer.SerializeToString(obj);
        }
    }
}

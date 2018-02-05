using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Parking.Common
{
    /// <summary>
    /// 签名算法：所有参数值转成数组加上appid,再按字符串自然排序（值为数字的也按照字符串处理）后连接成字符串+密钥的md5
    /// </summary>
    public class ValidSign
    {
        public static string appId = "iocp";
        public static string appSecret = "4bd024a77d7d454e8b6589f77a979a0d";
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="data"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GenSign(ArrayList data,long time)
        {
            data.Add(time.ToString());
            data.Add(appId);
            data.Sort();
            var encryptText = string.Join("", data.ToArray());
            string sign = md5(encryptText, appSecret);
            return sign;
        }

        /// <summary>
        /// 检查签名
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool CheckSign(Dictionary<string, object> data)
        {
            if (data.ContainsKey("sign") && data.ContainsKey("time"))
            {
                string origSign = data["sign"].ToString();
                ArrayList al = new ArrayList();
                foreach (KeyValuePair<string, object> kv in data)
                {
                    //排除掉key=sign的元素和值为空的元素
                    if (kv.Key != "sign".ToLower() && !string.IsNullOrEmpty(kv.Value.ToString()))
                        al.Add(kv.Value.ToString());
                }
                al.Add(appId);
                al.Sort();

                var encryptText = string.Join("", al.ToArray());
                string newSign = md5(encryptText, appSecret);
                if (string.Compare(origSign, newSign) != 0)
                    throw new Exception("sign wrong");
                else
                {
                    long nowticks = ConventTimeTicks(DateTime.Now);
                    long time = Convert.ToInt32(data["time"]);
                    if (time > (nowticks + 300) || time < (nowticks - 300))
                        throw new Exception("time wrong");
                    else
                        return true;
                }
            }
            else
                return true;

        }

        public static string md5(string encryptText, string encryptKey)
        {
            //return FormsAuthentication.HashPasswordForStoringInConfigFile(encryptText + encryptKey, "md5").ToLower();
            return Md5Hash(encryptText + encryptKey);
        }
        private static string Md5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public static long ConventTimeTicks(DateTime date)
        {
            return (date.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }
 
}

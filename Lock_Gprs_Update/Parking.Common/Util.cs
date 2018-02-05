using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Parking.Common
{
    public class Util
    {
        
        public static string GetIP()
        {
           HttpRequest request = HttpContext.Current.Request;
            string result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                if (request.ServerVariables["HTTP_VIA"] != null)
                {
                    result = request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
                }
            }
            if (String.IsNullOrEmpty(result))
            {
                result = request.UserHostAddress;
            }

            if (String.IsNullOrEmpty(result))
                result = "127.0.0.1";

            return result.Split(',')[0].Trim();
        }
        public static string GetProtocolResultText(string hexStr)
        {
            string rel = "";
            switch (hexStr)
            {
                case "AA":
                    rel = "成功";
                    break;
                case "AB":
                    rel = "失败";
                    break;
                case "BB":
                    rel = "对象已存在";
                    break;
                case "BA":
                    rel = "对象不存在";
                    break;
                default:
                    rel = hexStr+"未定义";
                    break;
            }
            return rel;
        }

        /// 获取时间戳  13位,秒
        /// </summary>  
        /// <returns></returns>  
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 产生随机种子
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        /// <returns></returns>
        public static int GetRandomNum(int startNum, int endNum)
        {
            var random = new Random(GetRandomSeed());
            return random.Next(startNum, endNum);
        }


        public static string GetMD5(string s, string ecode)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(ecode).GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// MD5 
        /// </summary>
        /// <param name="instr"></param>
        /// <returns></returns>
        public static string md5(string instr)
        {
            //return FormsAuthentication.HashPasswordForStoringInConfigFile(instr, "md5").ToLower();
            return Md5Hash(instr);
        }
        private static string Md5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public static string ToUrlEnCode(string value)
        {
            return HttpUtility.UrlEncode(value, Encoding.UTF8);
        }

        public static string ToUrlDeCode(string value)
        {
            return HttpUtility.UrlDecode(value, Encoding.UTF8);
        }

    }
}

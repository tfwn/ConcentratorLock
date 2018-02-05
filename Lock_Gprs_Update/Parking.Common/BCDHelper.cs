using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Common
{
    public class BCDHelper
    {
        /// <summary>  
        /// 16进制转换BCD（解压BCD）  
        /// </summary>  
        /// <param name="AData"></param>  
        /// <returns></returns>  
        public static string ConvertTo(Byte[] AData)
        {
            try
            {
                StringBuilder sb = new StringBuilder(AData.Length * 2);
                foreach (Byte b in AData)
                {
                    sb.Append(b >> 4);
                    sb.Append(b & 0x0f);
                }
                return sb.ToString();
            }
            catch { return null; }
        }

        /// <summary>  
        /// BCD码转换16进制(压缩BCD)  
        /// </summary>  
        /// <param name="strTemp"></param>  
        /// <returns></returns>  
        public static Byte[] ConvertFrom(string strTemp, int IntLen)
        {
            try
            {
                Byte[] Temp = ConvertFrom(strTemp.Trim());
                Byte[] return_Byte = new Byte[IntLen];
                if (IntLen != 0)
                {
                    if (Temp.Length < IntLen)
                    {
                        for (int i = 0; i < IntLen - Temp.Length; i++)
                        {
                            return_Byte[i] = 0x00;
                        }
                    }
                    Array.Copy(Temp, 0, return_Byte, IntLen - Temp.Length, Temp.Length);
                    return return_Byte;
                }
                else
                {
                    return Temp;
                }
            }
            catch
            { return null; }
        }

        private static Byte[] ConvertFrom(string strTemp)
        {
            try
            {
                if (Convert.ToBoolean(strTemp.Length & 1))//数字的二进制码最后1位是1则为奇数  
                {
                    strTemp = "0" + strTemp;//数位为奇数时前面补0  
                }
                Byte[] aryTemp = new Byte[strTemp.Length / 2];
                for (int i = 0; i < (strTemp.Length / 2); i++)
                {
                    aryTemp[i] = (Byte)(((strTemp[i * 2] - '0') << 4) | (strTemp[i * 2 + 1] - '0'));
                }
                return aryTemp;//高位在前  
            }
            catch
            { return null; }
        }
    }
}

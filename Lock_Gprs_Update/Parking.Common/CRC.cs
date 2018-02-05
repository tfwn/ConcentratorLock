using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Common
{
    public class CRC
    {
        
        public static byte CRC8(byte[] tData, int iOffSet, int iLen)
        {
            uint i, chChar;
            uint wCRC = 0x0;

            for (int m = 0; m < iLen; m++)
            {

                chChar = tData[m + iOffSet];
                wCRC ^= chChar;
                for (i = 0; i < 8; i++)
                {
                    if ((wCRC & 0x01) == 0x01)
                        wCRC = (wCRC >> 1) ^ 0x8C;
                    else
                        wCRC >>= 1;
                }
            }
            return (byte)wCRC;
        }

        public static byte[] CRC16(byte[] Buf, int iStart, int iLen)
        {
            byte[] baReturnValue = new byte[2];

            ushort uiCRC = 0xFFFF;

            for (int iLoop = 0; iLoop < iLen; iLoop++)
            {
                uiCRC ^= Buf[iStart + iLoop];
                for (int iLoop1 = 0; iLoop1 < 8; iLoop1++)
                {
                    if ((uiCRC & 1) == 1)
                    {
                        uiCRC >>= 1;
                        uiCRC ^= 0x8408;
                    }
                    else
                    {
                        uiCRC >>= 1;
                    }
                }
            }
            uiCRC ^= 0xFFFF;

            //
            baReturnValue[0] = (byte)((0xff00 & uiCRC) >> 8);
            baReturnValue[1] = (byte)(0xff & uiCRC);
            // baReturnValue[0] = (byte)(0xff & uiCRC);
            // baReturnValue[1] = (byte)((0xff00 & uiCRC) >> 8);

            return baReturnValue;
        }

    }

}

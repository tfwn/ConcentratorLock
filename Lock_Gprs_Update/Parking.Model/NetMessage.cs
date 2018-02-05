
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
    [Serializable]
    public class NetMessage<T>
    {
        public Header Header { get; set; }

        public int ResponseCode { get; set; }

        public int ErrorCodes { get; set; }

        public T ResponseData { get; set; }
        
        /// <summary>
        /// Body 消息主体，可以经过变换
        /// </summary>
        public byte[] Body { get; set; }
        public int BodyOffset
        { get; set; }
        
       

        public NetMessage(Header header)
        {
            ResponseCode = -1;

            ErrorCodes = -1;
            Header = header;
        }

        #region Ctor ,ToStream		
        /// <summary>
        /// NetMessage 本Ctor说明如果body不为null，则BodyOffset为0，且this.Header.MessageBodyLength = this.Body.Length 
        /// </summary>	
        public NetMessage(Header header, byte[] body)
        {
            this.Header = header;
            this.Body = body;

            if (this.Body == null)
            {
                this.Header.MessageBodyLength = 0;
            }
            else
            {
                this.Header.MessageBodyLength = this.Body.Length;
            }

        }

       

        public NetMessage(Header header, byte[] body, int bodyOffset, int bodyLen)
        {
            this.Header = header;
            this.Body = body;
            this.BodyOffset = bodyOffset;
            this.Header.MessageBodyLength = bodyLen;
        }

        public byte[] ToStream()
        {
            //if (this.Body == null)
            //{
            //    return this.Header.ToStream();
            //}

            //int headerLen = this.Header.GetStreamLength();
            //byte[] result = new byte[headerLen + this.Header.MessageBodyLength];
            //this.Header.ToStream(result, 0);
            //for (int i = 0; i < this.Header.MessageBodyLength; i++)
            //{
            //    result[headerLen + i] = this.Body[this.BodyOffset + i];
            //}

            //return result;
            return null;
        }
        #endregion

        #region Length
        public int Length
        {
            get
            {
                var headerLength = 0;
                return headerLength + this.Header.MessageBodyLength;
            }
        }
        #endregion

    }
}

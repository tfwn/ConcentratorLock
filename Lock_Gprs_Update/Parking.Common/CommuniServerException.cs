using Parking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Common
{
    public class CommuniServerException : Exception
    {
        private static readonly IDictionary<int, string> ErrorMessages = new Dictionary<int, string>
        {
              {ErrorCodes.SysErrLoginFail,"登录验证失败！"},
             {ErrorCodes.ResponseTerminalIsBusy,"当前集中器正在通讯中！"},
             {ErrorCodes.TaskErrTerminalNoNotInList,"设备列表中没有找到相应的集中器！"},
             {ErrorCodes.TaskErrResponseTimeOut,"It's timeout for wait task"},
              {ErrorCodes.TaskErrProtocolNotExist,"AsyncSocketInvokeElement1 is null"},
               {ErrorCodes.TaskErrRequestSendFail,"发送请求包失败"},
        };

        public int ErrorCode { get; set; }

        public new Object Data { get; set; }

        public CommuniServerException(int errCode = -10000, string message = "", Object data = null)
            : base(message)
        {
            ErrorCode = errCode;
            Data = data;
        }

        public CommuniServerException(int errCode = -10000, string message = "")
            : base(message)
        {
            ErrorCode = errCode;
        }

        public CommuniServerException(string message)
            : base(message)
        {
        }

        public CommuniServerException(int errCode)
            : this(
                errCode, ErrorMessages[errCode]) { }

        public CommuniServerException() : this(-1) { }
    }
}

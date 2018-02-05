using Parking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Common
{
    public class DataServerException : Exception
    {
        private static readonly IDictionary<int, string> ErrorMessages = new Dictionary<int, string>
        {
            {DataErrorCode.NoLogin,"登录验证失败！"},
            {DataErrorCode.Fail,"失败"},

            {DataErrorCode.InvalidUserNameOrPass,"用户名或密码错误"},
            {DataErrorCode.LockInited,"地锁已经初始化"},
            {DataErrorCode.LockNotFound,"地锁信息不存在"},
            {DataErrorCode.LockChangeAlreadySuccess,"change已经收到响应"},
            {DataErrorCode.InvalidParams,"无效的参数"},

        };

        public int ErrorCode { get; set; }

        public new Object Data { get; set; }

        public DataServerException(int errCode = -10000, string message = "", Object data = null)
            : base(message)
        {
            ErrorCode = errCode;
            Data = data;
        }

        public DataServerException(int errCode = -10000, string message = "")
            : base(message)
        {
            ErrorCode = errCode;
        }

        public DataServerException(string message)
            : base(message)
        {
        }

        public DataServerException(int errCode)
            : this(
                errCode, ErrorMessages[errCode]) { }

        public DataServerException() : this(-1) { }
    }
}

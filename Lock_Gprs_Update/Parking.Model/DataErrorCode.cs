using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
    public  class DataErrorCode
    {
        public const int Success =200;

        /// <summary>
        /// 未登录
        /// </summary>
        public const int NoLogin = 10002;

        public const int Fail = 10001;

        /// <summary>
        /// 用户名或密码错误
        /// </summary>
        public const int InvalidUserNameOrPass = 10003;

        /// <summary>
        /// 地锁已经初始化
        /// </summary>
        public const int LockInited = 10004;

        /// <summary>
        /// 地锁信息未找到
        /// </summary>
        public const int LockNotFound = 10005;

        /// <summary>
        /// change已经收到响应
        /// </summary>
        public const int LockChangeAlreadySuccess = 10006;

        public const int InvalidParams = 10007;

    }
}

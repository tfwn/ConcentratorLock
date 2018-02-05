using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
    public class ResponseCodes
    {
        /// <summary>
        /// 请求任务处理返回成功
        /// </summary>
        public const int ResponseTaskSuccess = 1;
        /// <summary>
        /// 请求任务处理返回失败
        /// </summary>
        public const int ResponseTaskFail = 2;
        /// <summary>
        /// 查询终端状态：在线
        /// </summary>
        public const int ResponseTerminalOnLine = 3;
        /// <summary>
        /// 查询终端状态：离线
        /// </summary>
        public const int ResponseTerminalOffLine = 4;
        /// <summary>
        /// 查询终端状态：未连接
        /// </summary>
        public const int ResponseTerminalNoLink = 5;
        /// <summary>
        /// 查询终端状态：工作中
        /// </summary>
        public const int ResponseTerminalIsBusy = 6;
        /// <summary>
        /// 查询终端状态：空闲
        /// </summary>
        public const int ResponseTerminalIsIdle = 7;

    }
}

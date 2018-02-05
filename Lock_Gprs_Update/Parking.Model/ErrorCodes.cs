using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
    public class ErrorCodes
    {
        /// <summary>
        /// 无错误
        /// </summary>
        public const int NoError = 0;
        /// <summary>
        /// 登录验证失败
        /// </summary>
        public const int SysErrLoginFail = 1;
        /// <summary>
        /// 请求通信协议不存在
        /// </summary>
        public const int TaskErrProtocolNotExist = 2;
        /// <summary>
        /// 请求终端编号不在连接列表
        /// </summary>
        public const int TaskErrTerminalNoNotInList = 3;
        /// <summary>
        /// 请求参数字符串格式或内容非法
        /// </summary>
        public const int TaskErrRequestStrInvalid = 4;
        /// <summary>
        /// 请求任务处理出现对象任务为空错误
        /// </summary>
        public const int SysErrObjectIsNull = 5;
        /// <summary>
        /// 请求任务处理超时
        /// </summary>
        public const int TaskErrResponseTimeOut = 6;
        /// <summary>
        /// 请求任务响应数据确认失败
        /// </summary>
        public const int TaskErrResponseFail = 7;
        /// <summary>
        /// 请求任务响应数据为未知数据
        /// </summary>
        public const int TaskErrResponseUnknowData = 8;
        /// <summary>
        /// 请求任务响应数据为对象不存在
        /// </summary>
        public const int TaskErrResponseObjectNotExist = 9;
        /// <summary>
        /// 请求任务响应数据为对象已存在
        /// </summary>
        public const int TaskErrResponseObjectExists = 10;
        /// <summary>
        /// 请求任务响应数据为存储空间已满
        /// </summary>
        public const int TaskErrResponseStorageSpaceIsFull = 11;
        /// <summary>
        /// 请求任务响应数据为非法数据
        /// </summary>
        public const int TaskErrResponseInvalidData = 12;
        /// <summary>
        /// 请求任务响应数据为不支持此功能
        /// </summary>
        public const int TaskErrResponseFunctionNotSupport = 13;

        /// <summary>
        /// 查询终端状态：工作中
        /// </summary>
        public const int ResponseTerminalIsBusy =14;

        /// <summary>
        /// 发送请求包失败
        /// </summary>
        public const int TaskErrRequestSendFail = 15;
    }

}

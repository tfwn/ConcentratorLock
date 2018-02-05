using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
    /// <summary>
    /// 响应结果对象类
    /// </summary>
    [DataContract]
    public class ResponseObjResult
    {
        /// <summary>
        /// 响应代码
        /// </summary>
        [DataMember]
        public int ResultCode { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        [DataMember]
        public int ErrorCode { get; set; }

        /// <summary>
        /// 终端编号
        /// </summary>
        [DataMember]
        public string TerminalNo { get; set; }

        /// <summary>
        /// 返回数据对象类型
        /// </summary>
        [DataMember]
        public int ReturnDataObjectType { get; set; }

        /// <summary>
        /// 字符串类型结果
        /// </summary>
        [DataMember]
        public string ResultOfString { get; set; }

        /// <summary>
        /// 档案类类型结果数组
        /// </summary>
        [DataMember]
        public ResultArchiveItem[] ResultOfArchiveList { get; set; }

        /// <summary>
        /// 数据类类型结果数组
        /// </summary>
        [DataMember]
        public ResultMeterDataItem[] ResultOfDataList { get; set; }
    }

    /// <summary>
    /// 结果档案类
    /// </summary>
    [DataContract]
    public class ResultArchiveItem
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [DataMember]
        public string DeviceNo { get; set; }

        /// <summary>
        /// 设备序号
        /// </summary>
        [DataMember]
        public byte DeviceIndex { get; set; }

        /// <summary>
        /// 操作结果
        /// 0xAA: 成功；0xAB: 失败；0xBB: 已存在；0xBA: 不存在；
        /// </summary>
        [DataMember]
        public string OperateResult { get; set; }

        /// <summary>
        /// 路由路径
        /// </summary>
        [DataMember]
        public string RoutePaths { get; set; }
    }

    /// <summary>
    /// 结果数据类
    /// </summary>
    [DataContract]
    public class ResultMeterDataItem
    {
        /// <summary>
        /// 表具编号
        /// </summary>
        [DataMember]
        public string MeterNo { get; set; }
        /// <summary>
        /// 表具序号
        /// </summary>
        [DataMember]
        public byte Index { get; set; }
        /// <summary>
        /// 抄表状态(0xAA: 成功；0xAB: 失败)
        /// </summary>
        [DataMember]
        public byte ReadResult { get; set; }
        /// <summary>
        /// 接收数据时间
        /// </summary>
        [DataMember]
        public string ReceiveDataTime { get; set; }
        /// <summary>
        /// 正转用量
        /// </summary>
        [DataMember]
        public double PositiveComsumption { get; set; }
        /// <summary>
        /// 反转用量
        /// </summary>
        [DataMember]
        public double ReverseComsumption { get; set; }
        /// <summary>
        /// 电池电压
        /// </summary>
        [DataMember]
        public double BatteryVoltage { get; set; }
        /// <summary>
        /// 表故障状态
        /// </summary>
        [DataMember]
        public byte FaultStatus { get; set; }
        /// <summary>
        /// 历史报警1
        /// </summary>
        [DataMember]
        public byte HistoryAlarmOne { get; set; }
        /// <summary>
        /// 历史报警2
        /// </summary>
        [DataMember]
        public byte HistoryAlarmTwo { get; set; }
        /// <summary>
        /// 信号强度
        /// </summary>
        [DataMember]
        public byte Rssi { get; set; }
    }

    /// <summary>
    ///  返回数据对象类型0：空1：字符串2：档案类型3：数据类型
    /// </summary>
    public class ResDataType {
        public const int Empty=0;
        public const int Str=1;
        public const int Archive=2;
        public const int MeterDataItem =3;
    }
}

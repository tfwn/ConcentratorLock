using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Model
{
    public class testpost
    {
        public int UserId { get; set; }
    }
    public class ReqWatermeter
    {
        /// <summary>
        /// 集中器id
        /// </summary>
        public string DevId { get; set; }

        /// <summary>
        /// 水具id列表
        /// </summary>
        public List<string> WatermeterList { get; set; }

        public string UserName { get; set; }

        public string Pass { get; set; }
    }


    public class ResWatermeter
    {

        /// <summary>
        /// 包号
        /// </summary>
        public Byte PackageId { get; set; }

        /// <summary>
        /// 末包标志
        /// </summary>
        public Byte EndPacketeFlag { get; set; }

        public List<WatermeterInfo> WatermeterInfoList { get; set; }
    }

    /// <summary>
    /// 表具信息类
    /// </summary>
    public class WatermeterInfo
    {
        

        public string Wid { get; set; }
        public Byte Order { get; set; }
        public string Result { get; set; }

    }

   

    public class UserInfo
    {
        public string UserName { get; set; }

        public string Pass { get; set; }
    }


    /// <summary>
    /// 时钟请求类
    /// </summary>
    public class ReqClock
    {
        /// <summary>
        /// 集中器id
        /// </summary>
        public string DevId { get; set; }

        /// <summary>
        /// 由年月日时分秒组成的时钟：2016-10-02 12:12:43
        /// </summary>
        //public string Clock { get; set; }

        public string UserName { get; set; }

        public string Pass { get; set; }

        public DateTime NowTime { get; set; }
    }
    public class ResClock
    {
        
        /// <summary>
        /// 由年月日时分秒组成的时钟：2016-10-02 12:12:43
        /// </summary>
        public string Clock { get; set; }
        
        public string Result { get; set; }
    }

    /// <summary>
    /// 路由路径请求类
    /// </summary>
    public class ReqRouteDir
    {
        /// <summary>
        /// 集中器id
        /// </summary>
        public string DevId { get; set; }

        /// <summary>
        /// 表具id
        /// </summary>
        public string WatermeterId { get; set; }

    
        public List<List<string>> RouteDirList { get; set; }

        public string UserName { get; set; }

        public string Pass { get; set; }
    }

    public class ResRouteDir
    {
        
        public string Result { get; set; }

        /// <summary>
        /// 设置路由路径返回结果列表
        /// </summary>
        public List<string> SetRouteDirOpList { get; set; }

        /// <summary>
        /// 获取路由路径返回结果列表
        /// </summary>
        public List<List<string>> GetRouteDirOpList { get; set; }
    }

    /// <summary>
    /// 获取表具数据请求类
    /// </summary>
    public class ReqWartermeterData
    {
        /// <summary>
        /// 集中器id
        /// </summary>
        public string DevId { get; set; }

        /// <summary>
        /// 表具id
        /// </summary>
        public string WatermeterId { get; set; }

        public string UserName { get; set; }

        public string Pass { get; set; }
    }

    /// <summary>
    /// 实时，定时数据类
    /// </summary>
    public class ResInfoData
    {
        public string Result { get; set; }

        /// <summary>
        /// 抄取时间
        /// </summary>
        public string ReadTime { get; set; }

        /// <summary>
        /// 正转用量
        /// </summary>
        public decimal PlusNumber { get; set; }

        /// <summary>
        /// 反转用量
        /// </summary>
        public decimal SubNumber { get; set; }

        /// <summary>
        /// 电池电压
        /// </summary>
        public decimal Voltage { get; set; }

        /// <summary>
        /// 信号强度
        /// </summary>
        public int Signal { get; set; }

        /// <summary>
        /// 状态，开阀或者关阀
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 故障列表
        /// </summary>
        public List<string> AlarmList { get; set; }

        /// <summary>
        /// 表故障状态
        /// </summary>
         public byte FaultStatus { get; set; }
        /// <summary>
        /// 历史报警1
        /// </summary>
         public byte HistoryAlarmOne { get; set; }
        /// <summary>
        /// 历史报警2
        /// </summary>
         public byte HistoryAlarmTwo { get; set; }
    }

}

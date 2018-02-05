/************************************************************************************************
*                                   SRWF-6009
*    (c) Copyright 2015, Software Department, Sunray Technology Co.Ltd
*                               All Rights Reserved
*
* FileName     : DataHandle.h
* Description  :
* Version      :
* Function List:
*------------------------------Revision History--------------------------------------------------
* No.   Version     Date            Revised By      Item            Description
* 1     V1.1        08/12/2015      Zhangxp         SRWF-6009       Original Version
************************************************************************************************/

#ifndef  DATAHANDLE_H
#define  DATAHANDLE_H


#ifdef   DATAHANDLE_GLOBALS
#define  DATAHANDLE_EXT
#else
#define  DATAHANDLE_EXT  extern
#endif


/************************************************************************************************
*                               Pubilc Macro Define Section
************************************************************************************************/
// 数据中去掉数据域和传输路径后的数据长度,即包长(2)+报文标识(1)+任务号(1)+命令字(1)+设备类型(1)+生命周期(1)+
//                                          路径信息(1)+下行信号强度(1)+上行信号强度(1)+校验字(2)+结束符(1)
#define DATA_FIXED_AREA_LENGTH                      13

// 数据处理部分事件过滤定义
#define DATAHANDLE_EVENT_FILTER                     (FLAG_USART_DEBUG_RX |          \
                                                     FLAG_USART_IR_RX |             \
                                                     FLAG_USART_RF_RX |             \
                                                     FLAG_UART_RS485_RX |           \
                                                     FLAG_USB_RX |                  \
                                                     FLAG_GPRS_RX |                 \
                                                     FLAG_DELAY_SAVE_TIMER |        \
                                                     FLAG_DATA_DOWNLOAD_TIMER |    \
                                                     FLAG_DATA_UPLOAD_TIMER |       \
                                                     FLAG_RTC_TIMING_TIMER)

// 补抄数据相关参数定义
#define DATAREPLENISH_INTERVAL_TIME                 (5 * 60)        // 每隔这段时间就会检查是否启动补抄功能(单位:秒)
// 数据下发间隔时间定义
#define DATADOWNLOAD_INTERVAL_TIME                  (0xFFFF)        // 每隔这段时间就会检查是否有数据需要上传(单位:秒)

// 数据上传间隔时间定义
#define DATAUPLOAD_INTERVAL_TIME                    (5 * 60)        // 每隔这段时间就会检查是否有数据会上传(单位:秒)
#undef ALWAYS_UPLOAD                                                // 如果服务器抄完表数据后集中器就不再主动上报了,则定义本标识
#define GIVEUP_ABNORMAL_DATA                                        // 如果集中器收到非法数据处理:抛弃或者上传到服务器

// 校时间隔时间定义
#define RTCTIMING_INTERVAL_TIME                     (3 * 3600)      // 每隔这段时间就会和服务器校对时间
// Gprs数据传输等待的时间
#define GPRS_WAIT_ACK_OVERTIME                      TIME_DELAY_MS(5000) //TIME_DELAY_MS(10000)    // Gprs数据发出后等待应答的超时时间

#define MAX_ROUTER_NUM                              7
#define MAX_NODES_ONE_PKG                           10
#define MAX_DATA_HANDLE_TASK_NUM                    5
#define DELAYTIME_ONE_LAYER                         8000

// 主动抄表时默认的应答信道
#define DEFAULT_TX_CHANNEL                          3   // 集中器默认发送信道
#define DEFAULT_RX_CHANNEL                          9   // 集中器默认接收信道
#define CHANNEL_OFFSET                              16  // 通知主机模块接收信道时,需要加上此值

// 报文标识定义
//---------------------------------------------------------------------------------------------------------------
// 报文与运营商编码不异或标志定义(Bit0) 0-异或 1-不异或
#define XOR_OFF                                     0   // 不异或
#define XOR_ON                                      1   // 异或

// 地址长度定义(Bit2) 0-6字节 1-8字节
#define SIX_SIZE                                   	0   // 6字节
#define EIGHT_SIZE                                  1   // 8字节

// 是否需要回执定义(Bit4) 0-不需回执 1-需要回执
#define NONE_ACK                                    0   // 不需要回执
#define NEED_ACK                                    1   // 需要回执

// 数据域加密标志定义(Bit5) 0-非加密 1-加密
#define ENCRYPT_OFF                                 0   // 非加密
#define ENCRYPT_ON                                  1   // 加密

// 数据包类型定义(Bit6) 0-命令帧 1-应答帧
#define CMD_PKG                                     0   // 命令帧
#define ACK_PKG                                     1   // 应答帧

// 数据流方向定义(Bit7) 0-下行 1-上行
#define DOWN_DIR                                    0   // 下行
#define UP_DIR                                      1   // 上行

// 是否翻转原有路径
#define REVERSED                                    1   // 翻转路径
#define UNREVERSED                                  0   // 不翻转路径

/************************************************************************************************
*                                   Enum Define Section
************************************************************************************************/
// 数据通信使用到的命令类型
typedef enum {
    // 和集中器有关的通信命令
    Read_CONC_Version = 0x40,                           // 读集中器版本信息
    Read_CONC_ID = 0x41,                                // 读集中器ID
    Write_CONC_ID = 0x42,                               // 写集中器ID
    Read_CONC_RTC = 0x43,                               // 读集中器时钟
    Write_CONC_RTC = 0x44,                              // 写集中器时钟
    Read_GPRS_Param = 0x45,                             // 读Gprs参数
    Write_GPRS_Param = 0x46,                            // 写Gprs参数
    Read_GPRS_RSSI = 0x47,                              // 读Gprs信号强度
    Initial_CONC_Cmd = 0x48,                            // 集中器初始化
    Restart_CONC_Cmd = 0x4C,                            // 集中器重新启动


    // 表具档案操作命令定义
    Read_Meter_Total_Number = 0x50,                     // 读表具档案的总数量
    Read_Meters_Doc_Info = 0x51,                        // 读取表具档案信息
    Write_Meters_Doc_Info = 0x52,                       // 写入表具档案信息
    Delete_Meters_Doc_Info = 0x53,                      // 删除表具档案信息
    Modify_Meter_Doc_Info = 0x54,                       // 修改表具档案信息

	// 和地锁通讯的相关命令
	Lock_Status_Report_Cmd = 0x72,						// 地锁状态变化
	Lock_Status_Issued_Cmd = 0x73, 						// 服务器下发地锁开关命令
	Lock_Key_Updata_Cmd = 0x74, 						// 服务器下发地锁更新密钥命令
	Lock_Sensor_Status_Cmd = 0x75,						// 地锁传感器状态信息，上传服务器
	Lock_Key_Updata_Status_Cmd = 0x76, 					// 地锁更新密钥命令结果跟新到服务器
	Read_Lock_Data_Cmd = 0x77,							// 获取集中器所保存的最新一条地锁状态信息
	Batch_Read_Lock_Data_Cmd = 0x78,					// 批量获取集中器所保存的最新一条地锁状态信息 0x78
	Lock_Read_CONC_Work_Param = 0x79,					// 读集中器的工作参数
	Lock_Write_CONC_Work_Param = 0x7A,					// 写集中器的工作参数
	Lock_Read_Host_Channel_Param = 0x7B,				// 读主机工作信道
	Lock_Write_Host_Channel_Param = 0x7C,				// 写主机工作信道

	// 和服务器通讯的相关命令
	Gprs_Heartbeat_Cmd = 0x10,							// 心跳指令
	CONC_RTC_Timing = 0x11, 							// 集中器主动校时
	Gprs_LogOff_Cmd = 0x12,								// 注销指令

    // 内部调试指令
    Software_Update_Cmd = 0xF1,                         // 程序升级命令
    Output_Monitior_Msg_Cmd = 0xF2,                     // 输出监控信息命令
    Eeprom_Check_Cmd = 0xF3,                            // Eeprom检查

    // 无操作指令
    Idle_Cmd = 0xFF                                     // 无操作指令
} COMMAND_TYPE;

// 定义使用的USART口和UART口,其中1 2 3为USART,4 5为UART。
typedef enum {
    Event_Start = 0,
    Event_Debug = Event_Start,                          // Debug串口
    Event_Ir,                                           // Ir通信串口
    Event_Rf,                                           // RF模块口
    Event_Rs485,                                        // RS485接口
    Event_Gprs,                                         // GPRS模块接口
    Event_Total                                         // UART总数
} DATA_EVENT;

// 定义接收数据后检验的结果
typedef enum {
    Error_Data = 0,                                     // 错误数据,没有发现关键字
    Error_DataLength,                                   // 错误的数据长度
    Error_DataOverFlow,                                 // 数据溢出
    Error_DataCrcCheck,                                 // 数据的Crc8校验出错
    Error_DstAddress,                                   // 目标地址错误
    Error_GetMem,                                       // 申请内存空间失败
    Ok_Data,                                            // 正确的数据
} EXTRACT_DATA_RESULT;

// 表端上传数据类型定义
typedef enum {
    RealTimeData = 0,                                   // 实时数据
    FwFreezingData,                                     // 正转冻结数据
    RwFreezingData,                                     // 反转冻结数据
    FixedTimeData,                                      // 定时上传数据
    QuantitativeData,                                   // 定量上传数据
    AlarmData,                                          // 报警数据
} METER_DATA_TYPE;

// 监控信息类型
typedef enum {
    Gprs_Connect_Msg = 0,                               // Gprs连接信息
    Total_Msg
} MONITOR_MSG_TYPE;

/************************************************************************************************
*                                  Union Define Section
************************************************************************************************/
// 路由信息定义
typedef union {
    uint8 Content;
    struct {
        uint8 Level: 4;                                 // Bit0-Bit3 路径级数,最小值为2
        uint8 CurPos: 4;                                // Bit4-Bit7 当前发送点的位置,从0开始
    };
} ROUTER_INFO;

// 报文标识定义(参见define部分的对应定义)
typedef union {
    uint8 Content;
    struct {
        uint8 PkgXor: 1;                                // Bit0 报文与运营商编码不异或标志: 0-不异或 1-异或
        uint8 AddrSize: 1;								// bit1 (0：地址为6字节地址，1：8字节地址)
		uint8 RxSleep: 1;								// bit2 接收方休眠	0:不休眠。1:休眠设备，需要唤醒
		uint8 TxSleep: 1;								// bit3 发送方休眠	0:不休眠。1:休眠设备，需要唤醒
        uint8 NeedAck: 1;                               // Bit4 是否需要回执 0-不需回执 1-需要回执
        uint8 Encrypt: 1;                               // Bit5 数据域加密标志 0-非加密 1-加密
        uint8 PkgType: 1;                               // Bit6 帧类型 0-命令帧 1-应答帧
        uint8 Direction: 1;                             // Bit7 上下行标识 0-下行 1-上行
    };
} PKG_PROPERTY;

// 生命周期和应答信道定义
typedef union {
    uint8 Content;
    struct {
        uint8 LifeCycle: 4;                             // 生命周期,Bit0-Bit3 最大为15,过一个路由减一,为0后抛弃
        uint8 AckChannel: 4;                            // 应答信道,Bit4-Bit7 抄表时是告诉户表模块应答的信道,主动上传时是指集中器应答上报的信道
    };
} LIFE_ACK;
/************************************************************************************************
*                                  Struct Define Section
************************************************************************************************/
// 任务运行标志定义
typedef struct {
    uint8 RTCTiming: 1;                                 // 为1时表示RTC校时任务正在进行中
    uint8 DataReplenish: 1;                             // 为1时表示数据补抄任务正在进行中
    uint8 RTCService: 1;                                // 为1时表示授时服务任务正在进行中
    uint8 DataUpload: 1;                                // 为1时表示数据上传任务正在进行中
    uint8 DataForward: 1;                               // 为1时表示数据转发任务正在进行中
    uint8 DataDownload: 1;                               // 为1时表示数据下发任务正在进行中
} TASK_STATUS_STRUCT;

// 数据格式
typedef struct {
    uint8 PortNo;                                       // 从哪个端口进来的数据
    uint16 PkgLength;                                   // 数据包长度,从本身开始(包含本身)到结束符(包含结束符)的长度
    PKG_PROPERTY PkgProp;                               // 报文标识
    uint8 PkgSn;                                        // 报文序号,发送方累计,广播用来源方的任务号,中间不变更
    COMMAND_TYPE Command;                               // 通信命令字
    uint8 DeviceType;                                   // 设备类型
    LIFE_ACK Life_Ack;                                  // 生命周期和应答信道定义
    ROUTER_INFO RouteInfo;                              // 路径信息
    uint8 Route[MAX_ROUTER_NUM][LONG_ADDR_SIZE];        // 传输路径
    uint8 DownRssi;                                     // 下行信号强度
    uint8 UpRssi;                                       // 上行信号强度
    uint16 Crc16;                                       // Crc8校验值
    uint16 DataLen;                                     // 数据域长度
    uint8 DataBuf[1];                                   // 数据域信息
} DATA_FRAME_STRUCT;

// 表具处理任务相关参数定义
typedef struct {
    uint8 Prio;                                         // 任务的优先级
    OS_STK *StkPtr;                                     // 任务的堆栈指针
    uint8 PkgSn;                                        // 数据包的报文序号
    uint8 Command;                                      // 通信命令字
    uint8 RouteLevel;                                   // 通信路径的级数
    uint8 PortNo;                                       // 通信时使用的串口
    uint16 NodeId;                                      // 该包处理的子节点的ID
    OS_EVENT *Mbox;                                     // 通信邮箱
    uint8 *Msg;                                         // 任务开始时收到的信息
} DATA_HANDLE_TASK;

/************************************************************************************************
*                        Global Variable Declare Section
************************************************************************************************/
DATAHANDLE_EXT uint8 SubNodesSaveDelayTimer;
DATAHANDLE_EXT uint16 DataUploadTimer;
DATAHANDLE_EXT uint16 DataDownloadTimer;
DATAHANDLE_EXT uint16 RTCTimingTimer;
DATAHANDLE_EXT OS_STK Task_DataHandle_Stk[TASK_DATAHANDLE_STK_SIZE];

/************************************************************************************************
*                            Function Declare Section
************************************************************************************************/
DATAHANDLE_EXT void DataHandle_Task(void *p_arg);
DATAHANDLE_EXT void DataHandle_OutputMonitorMsg(MONITOR_MSG_TYPE MsgType, uint8 *MsgPtr, uint16 MsgLen);

#endif
/***************************************End of file*********************************************/



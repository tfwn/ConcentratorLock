/************************************************************************************************
*                                   SRWF-6009
*    (c) Copyright 2015, Software Department, Sunray Technology Co.Ltd
*                               All Rights Reserved
*
* FileName     : DataHandle.c
* Description  :
* Version      :
* Function List:
*------------------------------Revision History--------------------------------------------------
* No.   Version     Date            Revised By      Item            Description
* 1     V1.1        08/12/2015      Zhangxp         SRWF-6009       Original Version
************************************************************************************************/

#define DATAHANDLE_GLOBALS

/************************************************************************************************
*                             Include File Section
************************************************************************************************/
#include "Stm32f10x_conf.h"
#include "ucos_ii.h"
#include "Bsp.h"
#include "Main.h"
#include "Rtc.h"
#include "Timer.h"
#include "SerialPort.h"
#include "Gprs.h"
#include "Flash.h"
#include "Eeprom.h"
#include "DataHandle.h"
#include "Database.h"
#include <string.h>
#include <md5.h>
#include <aes.h>
#include <version.h>


#define OPERATOR_NUMBER	(0x53525746) //SWRF

/************************************************************************************************
*                        Global Variable Declare Section
************************************************************************************************/
uint8 PkgNo;
PORT_NO MonitorPort = Usart_Debug;                      // 监控端口
uint8 SubNodesSaveDelayTimer = 0;                       // 档案延时保存时间
uint16 DataUploadTimer = 60;                            // 数据上传定时器
uint16 DataUpHostTimer = 0;                            // 升级主机定时器
uint16 DataDownloadTimer = 0;  // 数据下发定时器
uint16 RTCTimingTimer = 60;                             // RTC校时任务启动定时器
TASK_STATUS_STRUCT TaskRunStatus;                       // 任务运行状态
DATA_HANDLE_TASK DataHandle_TaskArry[MAX_DATA_HANDLE_TASK_NUM];
const uint8 Uart_RfTx_Filter[] = {SYNCWORD1, SYNCWORD2};
const uint8 DayMaskTab[] = {0xF0, 0xE0, 0xC0, 0x80};
uint16 DataDownloadNodeId = 0xABCD;

extern void Gprs_OutputDebugMsg(bool NeedTime, uint8 *StrPtr);

#define PRINT_INFO  0 //  1

uint8 test_print(DATA_FRAME_STRUCT *DataFrmPtr)
{
    uint8 *AsciiBuf = NULL;

    if ((void *)0 == (AsciiBuf = OSMemGetOpt(LargeMemoryPtr, 20, TIME_DELAY_MS(50)))) {
        return -1;
    }
    memset(AsciiBuf, 0, MEM_LARGE_BLOCK_LEN);
    BcdToAscii((uint8 *)DataFrmPtr, AsciiBuf, sizeof(DATA_FRAME_STRUCT), 3);
    Gprs_OutputDebugMsg(TRUE, AsciiBuf);
    OSMemPut(LargeMemoryPtr, AsciiBuf);
    return 0;
}

void DebugOutputLength(uint8 *StrPtr, uint8 SrcLength)
{
    uint16 len;
    uint8 *bufPtr = NULL;

    if ((void *)0 == (bufPtr = OSMemGetOpt(LargeMemoryPtr, 20, TIME_DELAY_MS(50)))) {
        return;
    }
    len = BcdToAscii( StrPtr, (uint8 *)bufPtr, SrcLength, 3);
    DataHandle_OutputMonitorMsg(Gprs_Connect_Msg, bufPtr, len);
    OSMemPut(LargeMemoryPtr, bufPtr);
    return;
}

/************************************************************************************************
*                           Prototype Declare Section
************************************************************************************************/
//void BigDataDebug(uint8 *BufPtr);

/************************************************************************************************
*                           Function Declare Section
************************************************************************************************/

/************************************************************************************************
* Function Name: DataHandle_GetEmptyTaskPtr
* Decription   : 在任务队列中搜索空的任务指针
* Input        : 无
* Output       : 任务的指针
* Others       : 无
************************************************************************************************/
DATA_HANDLE_TASK *DataHandle_GetEmptyTaskPtr(void)
{
    uint8 i;

    // 搜索未被占用的空间,创建数据上传任务
    for (i = 0; i < MAX_DATA_HANDLE_TASK_NUM; i++) {
        if ((void *)0 == DataHandle_TaskArry[i].StkPtr) {
            return (&DataHandle_TaskArry[i]);
        }
    }

    // 任务队列全部使用返回空队列
    return ((void *)0);
}

/************************************************************************************************
* Function Name: DataHandle_SetPkgProperty
* Decription   : 设置包属性值
* Input        : PkgXor-报文与运营商编码不异或标志: 0-不异或 1-异或
*                NeedAck-是否需要回执 0-不需回执 1-需要回执
*                PkgType-帧类型 0-命令帧 1-应答帧
*                Dir-上下行标识 0-下行 1-上行
* Output       : 属性值
* Others       : 无
************************************************************************************************/
PKG_PROPERTY DataHandle_SetPkgProperty(bool PkgXor, bool NeedAck, bool PkgType, bool Dir)
{
    PKG_PROPERTY pkgProp;

    pkgProp.Content = 0;
	pkgProp.AddrSize = 0x1;
    pkgProp.RxSleep = 0;
    pkgProp.TxSleep = 0;
    pkgProp.PkgXor = PkgXor;
    pkgProp.NeedAck = NeedAck;
    pkgProp.Encrypt = Concentrator.Param.DataEncryptCtrl;
    pkgProp.PkgType = PkgType;
    pkgProp.Direction = Dir;
    return pkgProp;
}

/************************************************************************************************
* Function Name: DataHandle_SetPkgPath
* Decription   : 设置数据包的路径
* Input        : DataFrmPtr-数据指针
*                ReversePath-是否需要翻转路径
* Output       : 无
* Others       : 无
************************************************************************************************/
void DataHandle_SetPkgPath(DATA_FRAME_STRUCT *DataFrmPtr, bool ReversePath)
{
    uint8 i, tmpBuf[LONG_ADDR_SIZE];

    if (0 == memcmp(BroadcastAddrIn, DataFrmPtr->Route[DataFrmPtr->RouteInfo.CurPos], LONG_ADDR_SIZE)) {
        memcpy(DataFrmPtr->Route[DataFrmPtr->RouteInfo.CurPos], Concentrator.LongAddr, LONG_ADDR_SIZE);
    }
    // 路径是否翻转处理
    if (REVERSED == ReversePath) {
        DataFrmPtr->RouteInfo.CurPos = DataFrmPtr->RouteInfo.Level - 1 - DataFrmPtr->RouteInfo.CurPos;
        for (i = 0; i < DataFrmPtr->RouteInfo.Level / 2; i++) {
            memcpy(tmpBuf, DataFrmPtr->Route[i], LONG_ADDR_SIZE);
            memcpy(DataFrmPtr->Route[i], DataFrmPtr->Route[DataFrmPtr->RouteInfo.Level - 1 - i], LONG_ADDR_SIZE);
            memcpy(DataFrmPtr->Route[DataFrmPtr->RouteInfo.Level - 1 - i], tmpBuf, LONG_ADDR_SIZE);
        }
    }
}


/************************************************************************************************
* Function Name: DataHandle_ExtractData
* Decription   : 按协议提取出数据并检验数据的正确性
* Input        : BufPtr-原数据指针
* Output       : 成功或错误说明
* Others       : 注意-成功调用此函数后BufPtr指向提取数据后的内存
************************************************************************************************/
EXTRACT_DATA_RESULT DataHandle_ExtractData(uint8 *BufPtr)
{
    uint8 i, *msg = NULL, *AesMsg = NULL;
    uint16 tmp;
    PORT_BUF_FORMAT *portBufPtr = NULL;
    DATA_FRAME_STRUCT *dataFrmPtr = NULL;
    uint16 AesPackLen;

    // 按协议格式提取相应的数据
    portBufPtr = (PORT_BUF_FORMAT *)BufPtr;
    if (FALSE == portBufPtr->Property.FilterDone) {
        return Error_Data;
    }
    // 申请一个内存用于存放提取后的数据
    if ((void *)0 == (msg = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        return Error_GetMem;
    }
    dataFrmPtr = (DATA_FRAME_STRUCT *)msg;
    dataFrmPtr->PortNo = portBufPtr->Property.PortNo;
    dataFrmPtr->PkgLength = ((uint16 *)portBufPtr->Buffer)[0] & 0x03FF;
    dataFrmPtr->PkgProp.Content = portBufPtr->Buffer[2];

    //数据解密
    if( 0x1 == dataFrmPtr->PkgProp.Encrypt){
        // 申请一个内存用于存放 AES 加解密的数据
        if ((void *)0 == (AesMsg = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
            OSMemPut(LargeMemoryPtr, msg);
            return Error_GetMem;
        }

        //此处使用包序号制作 md5 数据包，用于生成密钥
        unsigned char md5Test[10] = {0};
        unsigned char md5Key[16] = {0};
		MD5_CTX md5;
        md5Test[0] = portBufPtr->Buffer[3];

		MD5Init(&md5, OPERATOR_NUMBER);
		MD5Update(&md5,md5Test,strlen((char *)md5Test));//输入要加密的数据和长度
		MD5Final(&md5,md5Key);//md5 加密成 16 字节数据

        AesPackLen = dataFrmPtr->PkgLength - 6;

        //解密区域为'命令字'到'上行信号强度'.
        AES_DecryptData( &portBufPtr->Buffer[4], AesPackLen, AesMsg, md5Key);
        memcpy(portBufPtr->Buffer, AesMsg, AesPackLen);

        //标志位置为已解密
        dataFrmPtr->PkgProp.Encrypt = 0x0;
        portBufPtr->Buffer[2] = dataFrmPtr->PkgProp.Content;

        OSMemPut(LargeMemoryPtr, AesMsg);
    }

    dataFrmPtr->PkgSn = portBufPtr->Buffer[3];
    dataFrmPtr->Command = (COMMAND_TYPE)(portBufPtr->Buffer[4]);

    dataFrmPtr->DeviceType = portBufPtr->Buffer[5];
    dataFrmPtr->Life_Ack.Content = portBufPtr->Buffer[6];
    dataFrmPtr->RouteInfo.Content = portBufPtr->Buffer[7];
    memset(dataFrmPtr->Route[0], 0, (MAX_ROUTER_NUM+1)*LONG_ADDR_SIZE);
    for (i = 0; i < dataFrmPtr->RouteInfo.Level && i < MAX_ROUTER_NUM; i++) {
        memcpy(dataFrmPtr->Route[i], &portBufPtr->Buffer[8 + LONG_ADDR_SIZE * i], LONG_ADDR_SIZE);
    }
    dataFrmPtr->DownRssi = *(portBufPtr->Buffer + dataFrmPtr->PkgLength- 5);
    dataFrmPtr->UpRssi = *(portBufPtr->Buffer + dataFrmPtr->PkgLength - 4);
    //dataFrmPtr->Crc16 = *(portBufPtr->Buffer + dataFrmPtr->PkgLength - 3);
    dataFrmPtr->Crc16 = ((portBufPtr->Buffer[dataFrmPtr->PkgLength - 3] << 8)&0xff00)|(portBufPtr->Buffer[dataFrmPtr->PkgLength - 2]&0xFF);
    tmp = LONG_ADDR_SIZE * dataFrmPtr->RouteInfo.Level + DATA_FIXED_AREA_LENGTH;
    if (dataFrmPtr->PkgLength < tmp || dataFrmPtr->PkgLength > MEM_LARGE_BLOCK_LEN - 1) {
        OSMemPut(LargeMemoryPtr, msg);
        return Error_DataLength;
    }
    dataFrmPtr->DataLen = dataFrmPtr->PkgLength - tmp;
    if (dataFrmPtr->DataLen < MEM_LARGE_BLOCK_LEN - sizeof(DATA_FRAME_STRUCT)) {
        memcpy(dataFrmPtr->DataBuf, portBufPtr->Buffer + 8 + LONG_ADDR_SIZE * dataFrmPtr->RouteInfo.Level, dataFrmPtr->DataLen);
    } else {
        OSMemPut(LargeMemoryPtr, msg);
        return Error_DataOverFlow;
    }

    // 检查Crc8是否正确
    if (dataFrmPtr->Crc16 != CalCrc16(portBufPtr->Buffer, dataFrmPtr->PkgLength - 3) || portBufPtr->Length < dataFrmPtr->PkgLength) {
        OSMemPut(LargeMemoryPtr, msg);
        return Error_DataCrcCheck;
    }

    // 检查结束符是否是 0x16
    if ( 0x16 != *(portBufPtr->Buffer + dataFrmPtr->PkgLength-1)) {
        OSMemPut(LargeMemoryPtr, msg);
        return Error_Data;
    }

	dataFrmPtr->DownRssi = 0;
	dataFrmPtr->UpRssi = *(portBufPtr->Buffer + dataFrmPtr->PkgLength);
	if( (0x1 == dataFrmPtr->RouteInfo.Content) &&
		(0 == memcmp(Concentrator.LongAddr, dataFrmPtr->Route[dataFrmPtr->RouteInfo.CurPos], LONG_ADDR_SIZE))){
        memcpy(BufPtr, msg, MEM_LARGE_BLOCK_LEN);
        OSMemPut(LargeMemoryPtr, msg);
        return Ok_Data;
	}

    // 检查是否为广播地址或本机地址
    dataFrmPtr->RouteInfo.CurPos += 1;
    if ((0 == memcmp(Concentrator.LongAddr, dataFrmPtr->Route[dataFrmPtr->RouteInfo.CurPos], LONG_ADDR_SIZE) ||
        0 == memcmp(BroadcastAddrIn, dataFrmPtr->Route[dataFrmPtr->RouteInfo.CurPos], LONG_ADDR_SIZE)) &&
        dataFrmPtr->RouteInfo.CurPos < dataFrmPtr->RouteInfo.Level) {
        memcpy(BufPtr, msg, MEM_LARGE_BLOCK_LEN);
        OSMemPut(LargeMemoryPtr, msg);
        return Ok_Data;
    }

    // 要进行后续处理,所以此处将提取出的数据返回
    memcpy(BufPtr, msg, MEM_LARGE_BLOCK_LEN);
    OSMemPut(LargeMemoryPtr, msg);
    return Error_DstAddress;
}

/************************************************************************************************
* Function Name: DataHandle_CreateTxData
* Decription   : 创建发送数据包
* Input        : DataFrmPtr-待发送的数据
* Output       : 成功或错误
* Others       : 该函数执行完毕后会释放DataBufPtr指向的存储区,还会将路由区的地址排列翻转
************************************************************************************************/
ErrorStatus DataHandle_CreateTxData(DATA_FRAME_STRUCT *DataFrmPtr)
{
    uint8 err;
    //uint8 *AesMsg = NULL;
    //uint16 AesPackLen;
    uint16 tmp, nodeId;
    PORT_BUF_FORMAT *txPortBufPtr = NULL;
    uint16 crc16;

	//uint8 delay = 0;
    //RTC_TIME rtcTime;
	// 随机延时，防止空中数据碰撞。
    //Rtc_Get(&rtcTime, Format_Bcd);
	//delay = (rtcTime.Second % 3) * 20 + (Concentrator.LongAddr[LONG_ADDR_SIZE-1]&0xf) * 100;
	//if( 0 != delay){
	//	OSTimeDlyHMSM(0, 0, 0, delay);
	//}

    // 先申请一个内存用于中间数据处理
    if ((void *)0 == (txPortBufPtr = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        OSMemPut(LargeMemoryPtr, DataFrmPtr);
        return ERROR;
    }
    txPortBufPtr->Property.PortNo = DataFrmPtr->PortNo;
    txPortBufPtr->Property.FilterDone = 1;
    memcpy(txPortBufPtr->Buffer, Uart_RfTx_Filter, sizeof(Uart_RfTx_Filter));
    txPortBufPtr->Length = sizeof(Uart_RfTx_Filter);

    tmp = txPortBufPtr->Length;
    DataFrmPtr->PkgLength = DataFrmPtr->DataLen + DataFrmPtr->RouteInfo.Level * LONG_ADDR_SIZE + DATA_FIXED_AREA_LENGTH;
    ((uint16 *)(&txPortBufPtr->Buffer[txPortBufPtr->Length]))[0] = DataFrmPtr->PkgLength;
    txPortBufPtr->Length += 2;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = DataFrmPtr->PkgProp.Content;         // 报文标识
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = DataFrmPtr->PkgSn;                   // 任务号
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = DataFrmPtr->Command;                 // 命令字
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = DataFrmPtr->DeviceType;          	// 设备类型
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = DataFrmPtr->Life_Ack.Content;        // 生命周期和应答信道
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = DataFrmPtr->RouteInfo.Content;       // 路径信息
    memcpy(&txPortBufPtr->Buffer[txPortBufPtr->Length], DataFrmPtr->Route[0], DataFrmPtr->RouteInfo.Level * LONG_ADDR_SIZE);
    txPortBufPtr->Length += DataFrmPtr->RouteInfo.Level * LONG_ADDR_SIZE;
    memcpy(txPortBufPtr->Buffer + txPortBufPtr->Length, DataFrmPtr->DataBuf, DataFrmPtr->DataLen);      // 数据域
    txPortBufPtr->Length += DataFrmPtr->DataLen;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x55;                                // 下行信号强度
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x55;                                // 上行信号强度
    crc16 = CalCrc16((uint8 *)(&txPortBufPtr->Buffer[tmp]), txPortBufPtr->Length - tmp);     // Crc8校验
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)((crc16 >> 8)&0xFF);
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)((crc16)&0xFF);
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = TAILBYTE;

    if (CMD_PKG == DataFrmPtr->PkgProp.PkgType) {
        txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x1E;
        nodeId = Data_FindNodeId(0, DataFrmPtr->Route[DataFrmPtr->RouteInfo.Level - 1]);
        if (DATA_CENTER_ID == nodeId || NULL_U16_ID == nodeId) {
            txPortBufPtr->Buffer[txPortBufPtr->Length++] = DEFAULT_TX_CHANNEL;
        } else {
            txPortBufPtr->Buffer[txPortBufPtr->Length++] = SubNodes[nodeId].RxChannel;
        }
        txPortBufPtr->Buffer[txPortBufPtr->Length++] = (DEFAULT_RX_CHANNEL + CHANNEL_OFFSET);
    } else {
        txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x00;
        // 为了和已出货的第一批表兼容
        if (0 == DataFrmPtr->Life_Ack.AckChannel) {
            txPortBufPtr->Buffer[txPortBufPtr->Length++] = (DEFAULT_RX_CHANNEL + CHANNEL_OFFSET);
        } else {
            txPortBufPtr->Buffer[txPortBufPtr->Length++] = DataFrmPtr->Life_Ack.AckChannel;
        }
        txPortBufPtr->Buffer[txPortBufPtr->Length++] = (DEFAULT_RX_CHANNEL + CHANNEL_OFFSET);
    }
#if 0
	//数据加密
	if (0x0 == DataFrmPtr->PkgProp.Encrypt) {

		// 先申请一个内存用于中间数据处理
		if ((void *)0 == (AesMsg = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
			OSMemPut(LargeMemoryPtr, DataFrmPtr);
			return ERROR;
		}

		unsigned char md5Test[10] = {0};
		unsigned char md5Key[16] = {0};

		//此处使用包序号制作 md5 数据包，用于生成密钥
		md5Test[0] = txPortBufPtr->Buffer[3];

		//md5 生成 16 字节密钥
		MD5_EncryptData(md5Test, strlen((char *)md5Test), md5Key, OPERATOR_NUMBER);
		AesPackLen = DataFrmPtr->PkgLength - 6;

		//加密区域为'命令字'到'上行信号强度'.
		AES_EncryptData( &txPortBufPtr->Buffer[4], AesPackLen, AesMsg, md5Key);
		memcpy(txPortBufPtr->Buffer, AesMsg, AesPackLen);

		//标志位置为已加密
		DataFrmPtr->PkgProp.Encrypt = 0x0;
		txPortBufPtr->Buffer[2] = DataFrmPtr->PkgProp.Content;

		OSMemPut(LargeMemoryPtr, AesMsg);

	}
#endif
    OSMemPut(LargeMemoryPtr, DataFrmPtr);
    if (Uart_Gprs == txPortBufPtr->Property.PortNo) {
        if (FALSE == Gprs.Online ||
            OS_ERR_NONE != OSMboxPost(Gprs.MboxTx, txPortBufPtr)) {
            OSMemPut(LargeMemoryPtr, txPortBufPtr);
            return ERROR;
        } else {
            OSFlagPost(GlobalEventFlag, FLAG_GPRS_TX, OS_FLAG_SET, &err);
            return SUCCESS;
        }
    } else {
        if (txPortBufPtr->Property.PortNo < Port_Total &&
            OS_ERR_NONE != OSMboxPost(SerialPort.Port[txPortBufPtr->Property.PortNo].MboxTx, txPortBufPtr)) {
            OSMemPut(LargeMemoryPtr, txPortBufPtr);
            return ERROR;
        } else {
            OSFlagPost(GlobalEventFlag, (OS_FLAGS)(1 << txPortBufPtr->Property.PortNo + SERIALPORT_TX_FLAG_OFFSET), OS_FLAG_SET, &err);
            return SUCCESS;
        }
    }
}

// ****用于大数据调试
/*
void BigDataDebug(uint8 *BufPtr)
{
    PORT_BUF_FORMAT *portBufPtr;
    DATA_FRAME_STRUCT *DataFrmPtr;
    uint16 len;
    uint8 *p;

    portBufPtr = (PORT_BUF_FORMAT *)BufPtr;
    len = portBufPtr->Buffer[0] + portBufPtr->Buffer[1] * 256;
    p = portBufPtr->Buffer + len - 4 - 2;
    if (0x39 == *p) {
        return;
    }
    if ((void *)0 == (DataFrmPtr = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        return;
    }
    DataFrmPtr->PortNo = Uart_Gprs;
    DataFrmPtr->PkgLength = DATA_FIXED_AREA_LENGTH;
    DataFrmPtr->PkgSn = PkgNo++;
    DataFrmPtr->DeviceType = Dev_Concentrator;
    DataFrmPtr->Life_Ack.Content = 0x0F;
    DataFrmPtr->RouteInfo.CurPos = 0;
    DataFrmPtr->RouteInfo.Level = 2;
    memcpy(DataFrmPtr->Route[0], Concentrator.LongAddr, LONG_ADDR_SIZE);
    memcpy(DataFrmPtr->Route[1], BroadcastAddrOut, LONG_ADDR_SIZE);
    DataFrmPtr->DataLen = 2 + len;
    DataFrmPtr->DataBuf[0] = 0xEE;
    DataFrmPtr->DataBuf[1] = 0xEE;
    memcpy(DataFrmPtr->DataBuf + 2, portBufPtr->Buffer, len);
    DataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, CMD_PKG, UP_DIR);
    DataHandle_SetPkgPath(DataFrmPtr, UNREVERSED);
    DataHandle_CreateTxData(DataFrmPtr);
}
*/
// ****用于大数据调试

/************************************************************************************************
* Function Name: DataHandle_DataDelaySaveProc
* Decription   : 数据延时保存处理函数
* Input        : 无
* Output       : 无
* Others       : 当多组数据需要保存时启动一次延时保存,以延长Flash的寿命
************************************************************************************************/
void DataHandle_DataDelaySaveProc(void)
{
    SubNodesSaveDelayTimer = 0;
    Flash_SaveSubNodesInfo();
    Flash_SaveConcentratorInfo();
}

/************************************************************************************************
* Function Name: DataHandle_OutputMonitorMsg
* Decription   : 集中器主动输出监控信息
* Input        : MsgType-信息的类型,MsgPtr-输出信息指针,MsgLen-信息的长度
* Output       : 无
* Others       : 无
************************************************************************************************/
void DataHandle_OutputMonitorMsg(MONITOR_MSG_TYPE MsgType, uint8 *MsgPtr, uint16 MsgLen)
{
    DATA_FRAME_STRUCT *dataFrmPtr = NULL;

    if ((void *)0 == (dataFrmPtr = OSMemGetOpt(LargeMemoryPtr, 20, TIME_DELAY_MS(50)))) {
        return;
    }
    dataFrmPtr->PortNo = MonitorPort;
    dataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, CMD_PKG, UP_DIR);
    dataFrmPtr->PkgSn = PkgNo++;
    dataFrmPtr->Command = Output_Monitior_Msg_Cmd;
    dataFrmPtr->DeviceType = Dev_Concentrator;
    dataFrmPtr->Life_Ack.Content = 0x0F;
    dataFrmPtr->RouteInfo.CurPos = 0;
    dataFrmPtr->RouteInfo.Level = 2;
    memcpy(dataFrmPtr->Route[0], Concentrator.LongAddr, LONG_ADDR_SIZE);
    memcpy(dataFrmPtr->Route[1], BroadcastAddrOut, LONG_ADDR_SIZE);
    dataFrmPtr->DataBuf[0] = MsgType;
    memcpy(&dataFrmPtr->DataBuf[1], MsgPtr, MsgLen);
    dataFrmPtr->DataLen = 1 + MsgLen;
    DataHandle_SetPkgPath(dataFrmPtr, UNREVERSED);
    DataHandle_CreateTxData(dataFrmPtr);
    return;
}


/************************************************************************************************
* Function Name: DataHandle_LockStatusDataSaveProc
* Decription   : 地锁状态数据保存处理函数
* Input        : DataFrmPtr-指向数据帧的指针
* Output       : 成功或失败
* Others       : 用于保存地锁状态数据和上下行信号强度
************************************************************************************************/
uint8 DataHandle_LockStatusDataSaveProc(DATA_FRAME_STRUCT *DataFrmPtr)
{
    uint16 nodeId;
    METER_DATA_SAVE_FORMAT *meterBufPtr = NULL;
    uint8 nodeAddr;

    nodeId = Data_FindNodeId(0, DataFrmPtr->Route[0]);
    if (NULL_U16_ID == nodeId) {
        return OP_Failure;
    }
    // 检查命令字是否正确 + 检查数据域长度是否正确
    if ((DataFrmPtr->Command != Lock_Status_Report_Cmd) ||
        ( DataFrmPtr->DataLen != LOCK_STATUS_DATA_SIZE - UPDOWN_RSSI_SIZE )){
        return OP_Failure;
    }
    // 申请空间用于保存地锁状态数据
    if ((void *)0 == (meterBufPtr = OSMemGetOpt(SmallMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        return OP_Failure;
    }

    // 读取出 Eeprom 中保存的地锁状态数据。
    memset((uint8 *)meterBufPtr, 0 , MEM_SMALL_BLOCK_LEN);
    Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, READ_ATTR);

    // 如果节点地址出错，则初始化该节点
    if (0 != memcmp(SubNodes[nodeId].LongAddr, meterBufPtr->Address, LONG_ADDR_SIZE)) {
        Data_MeterDataInit(meterBufPtr, nodeId, NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT));
    }

    // 将读取出的地锁数据的更新到最后一包
    if( meterBufPtr->MeterData[0] >= LOCK_STATUS_DATA_NUM){
        if(DataFrmPtr->PkgProp.TxSleep == 0x0){
            DataDownloadTimer = 3;
        }
        DataUploadTimer = 2;
        DataDownloadNodeId = nodeId;
        OSMemPut(SmallMemoryPtr, meterBufPtr);
        return ERROR;
    }
    nodeAddr = meterBufPtr->MeterData[0] * LOCK_STATUS_DATA_SIZE + 1;
    memcpy(&meterBufPtr->MeterData[nodeAddr], DataFrmPtr->DataBuf, LOCK_STATUS_DATA_SIZE-UPDOWN_RSSI_SIZE);
	meterBufPtr->MeterData[nodeAddr + LOCK_STATUS_DATA_SIZE-UPDOWN_RSSI_SIZE] = DataFrmPtr->UpRssi;//上行信号强度

	// 数据域第一个字节存放数据包 包数，最大为 10，超过部分不予保存，直接丢弃
    meterBufPtr->MeterData[0] += 1;
    meterBufPtr->RxLastDataNum = meterBufPtr->MeterData[0];// 存放最后一包数据所在位置，用于集中器上位机软件获取数据

    meterBufPtr->Property.CurRouteNo = SubNodes[nodeId].Property.CurRouteNo;
    meterBufPtr->Property.LastResult = SubNodes[nodeId].Property.LastResult = 1;

    // 该节点的收发信道位于数据末尾第4个字节(要考虑到收发信道的问题)
    SubNodes[nodeId].RxChannel = DataFrmPtr->DataBuf[LOCK_STATUS_DATA_SIZE + 5];

    //地锁状态变化，立刻保存并上报服务器
    meterBufPtr->Crc8MeterData = CalCrc8(meterBufPtr->MeterData, NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT));
    meterBufPtr->Property.UploadData = SubNodes[nodeId].Property.UploadData = FALSE;
    meterBufPtr->Property.UploadPrio = SubNodes[nodeId].Property.UploadPrio = HIGH;
    Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, WRITE_ATTR);

    DataUploadTimer = 2;
    if(DataFrmPtr->PkgProp.TxSleep == 0x0){
        DataDownloadTimer = 3;
    }
    if( 0xABCD == DataDownloadNodeId){
        DataDownloadNodeId = nodeId;
    }

    OSMemPut(SmallMemoryPtr, meterBufPtr);
    return OP_Succeed;
}


/************************************************************************************************
* Function Name: DataHandle_LockGprsDataSaveProc
* Decription   : 服务器下发地锁命令保存函数
* Input        : DataFrmPtr-指向数据帧的指针
* Output       : 成功或失败
* Others       : 用于保存地锁状态数据和上下行信号强度
************************************************************************************************/
bool DataHandle_LockGprsDataSaveProc(DATA_FRAME_STRUCT *DataFrmPtr)
{
    uint16 nodeId;
    METER_DATA_SAVE_FORMAT *meterBufPtr = NULL;
    uint8 *msg = NULL;
    uint8 statusSize, flag, OpResult;

    // 检查是否有这个节点
    nodeId = Data_FindNodeId(0, DataFrmPtr->Route[0]);
    if (NULL_U16_ID == nodeId) {
        return ERROR;
    }
    // 检查命令字是否正确 + 检查数据域长度是否正确
    if ( !((DataFrmPtr->Command == Lock_Status_Issued_Cmd)&&(DataFrmPtr->DataLen == LOCK_GPRS_DATA_SIZE)) &&
         !((DataFrmPtr->Command == Lock_Key_Updata_Cmd)&&(DataFrmPtr->DataLen == LOCK_KEY_DATA_SIZE))) {
        return ERROR;
    }


    // 申请空间用于保存地锁状态数据
    if ((void *)0 == (meterBufPtr = OSMemGetOpt(SmallMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        return ERROR;
    }
    // 申请空间用于保存中间数据
    if ((void *)0 == (msg = OSMemGetOpt(SmallMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        OSMemPut(SmallMemoryPtr, meterBufPtr);
        return ERROR;
    }

    // 读取出 Eeprom 中保存的地锁状态数据。
    memset((uint8 *)meterBufPtr, 0 , MEM_SMALL_BLOCK_LEN);
    Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, READ_ATTR);

    // 如果节点地址出错，则初始化该节点
    if (0 != memcmp(SubNodes[nodeId].LongAddr, meterBufPtr->Address, LONG_ADDR_SIZE)) {
        Data_MeterDataInit(meterBufPtr, nodeId, NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT));
    }
    // statusSize 长度地址保存地锁状态数据
    statusSize = LOCK_STATUS_DATA_NUM * LOCK_STATUS_DATA_SIZE + 1;
    flag = DataFrmPtr->DataBuf[0]; // 下发数据的标志，根据标志进行不同处理

    // 如果回复内容中需要带上上一条数据
    if( 0x80 == (flag & 0x80)){
        // 将原数据(命令字1 + 命令状态1 + 数据长度1 + 下发数据n)保存在缓存中，以便接下来的处理
        memcpy(msg, &meterBufPtr->MeterData[statusSize], meterBufPtr->MeterData[statusSize + 2] + 3);
    }

    // 0x0:未下发， 0x1 ：成功 ，0x2 ：失败
	if ( 0x1 == (flag & 0x1)){
        // 覆盖保存下发数据
            // 将读取出的服务器下发地锁命令的数据更新
            meterBufPtr->MeterData[statusSize] = DataFrmPtr->Command; // 存放命令字
            // 存放此包数据的状态（是否已经发送给地锁）
            // 0x0:未下发 0x1 ：成功 0x2 ：失败
            meterBufPtr->MeterData[statusSize + 1] = 0x0; // 存放此包数据的状态（是否已经发送给地锁）
            meterBufPtr->MeterData[statusSize + 2] = DataFrmPtr->DataLen; // 存放数据长度
            //	更新下发数据
            memcpy(&meterBufPtr->MeterData[statusSize + 3], DataFrmPtr->DataBuf, DataFrmPtr->DataLen);
            OpResult = OP_CmdRevokeSucceed;
	} else if( (0x1 == meterBufPtr->MeterData[statusSize + 1]) ||
        (0x0 == meterBufPtr->MeterData[statusSize + 1] && 0x0 == meterBufPtr->MeterData[statusSize + 2])){
        // 保存下发数据

        // 将读取出的服务器下发地锁命令的数据更新
        meterBufPtr->MeterData[statusSize] = DataFrmPtr->Command; // 存放命令字
        // 存放此包数据的状态（是否已经发送给地锁）
        // 0x0:未下发 0x1 ：成功 0x2 ：失败
        meterBufPtr->MeterData[statusSize + 1] = 0x0; // 存放此包数据的状态（是否已经发送给地锁）
        meterBufPtr->MeterData[statusSize + 2] = DataFrmPtr->DataLen; // 存放数据长度
        //	更新下发数据
        memcpy(&meterBufPtr->MeterData[statusSize + 3], DataFrmPtr->DataBuf, DataFrmPtr->DataLen);
        OpResult = OP_Succeed;
    } else {
        // 下发失败， eeprom 数据不变
        OpResult = OP_Failure;
    }

    //bit7： 1 ：回复内容中包含集中器存储的上一条命令，数据域 = （ 操作结果1 + 命令字1 + 命令状态1 + 命令长度1 + 数据域n ）。
    // 	     0 : 只回复操作结果一个字节。
    if( 0x80 == (flag & 0x80)){
        DataFrmPtr->DataLen = msg[2] + 4;
        DataFrmPtr->DataBuf[0] = OpResult;
        memcpy(&DataFrmPtr->DataBuf[1], msg, DataFrmPtr->DataLen-1);
    } else {
        DataFrmPtr->DataLen = 1;
        DataFrmPtr->DataBuf[0] = OpResult;
    }

    //地锁状态变化，立刻保存并上报服务器
    meterBufPtr->Crc8MeterData = CalCrc8(meterBufPtr->MeterData, NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT));

    meterBufPtr->Property.DownloadPrio = SubNodes[nodeId].Property.DownloadPrio = HIGH;
    Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, WRITE_ATTR);

    OSMemPut(SmallMemoryPtr, meterBufPtr);
    OSMemPut(SmallMemoryPtr, msg);
    return SUCCESS;
}


/************************************************************************************************
* Function Name: DataHandle_LockSensorStatusUp
* Decription   : 地锁传感器状态信息上传服务器
* Input        : DataFrmPtr-指向数据帧的指针
* Output       : 成功或失败
************************************************************************************************/
uint8 DataHandle_LockSensorStatusUp(DATA_FRAME_STRUCT *DataFrmPtr)
{
    uint16 nodeId;
    DATA_FRAME_STRUCT *sensorBufPtr = NULL;

    // 检查是否有这个节点
    nodeId = Data_FindNodeId(0, DataFrmPtr->Route[0]);
    if (NULL_U16_ID == nodeId) {
        return OP_Failure;
    }
    // 检查命令字是否正确 + 检查数据域长度是否正确
    if ( (DataFrmPtr->Command != Lock_Sensor_Status_Cmd) ||
        (DataFrmPtr->DataLen != LOCK_SENSOR_DATA_SIZE)) {
        return OP_Failure;
    }
    // 申请空间用于保存地锁状态数据
    if ((void *)0 == (sensorBufPtr = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        return OP_Failure;
    }
	memcpy(sensorBufPtr, DataFrmPtr, MEM_LARGE_BLOCK_LEN);

	sensorBufPtr->PortNo = Uart_Gprs;
	sensorBufPtr->DeviceType = Dev_Concentrator;
	memcpy(&sensorBufPtr->DataBuf[0], SubNodes[nodeId].LongAddr, LONG_ADDR_SIZE);
	memcpy(&sensorBufPtr->DataBuf[LONG_ADDR_SIZE], &DataFrmPtr->DataBuf[0], LOCK_SENSOR_DATA_SIZE);

	sensorBufPtr->DataLen = LONG_ADDR_SIZE + LOCK_SENSOR_DATA_SIZE;
	sensorBufPtr->RouteInfo.CurPos = 0;
	sensorBufPtr->RouteInfo.Level = 1;
	memcpy(sensorBufPtr->Route[0], Concentrator.LongAddr, LONG_ADDR_SIZE);
	sensorBufPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, CMD_PKG, DOWN_DIR);
	DataHandle_SetPkgPath(sensorBufPtr, REVERSED);
	DataHandle_CreateTxData(sensorBufPtr);

    return OP_Succeed;
}


/************************************************************************************************
* Function Name: DataHandle_ReadLockDataProc
* Decription   : 获取集中器所保存的最新一条地锁状态信息
* Input        : DataFrmPtr-指向数据帧的指针
* Output       : 无
* Others       : 下行:长地址(8)
*                地锁数据上行:操作状态(1)+长地址(8)+地锁最新数据(N)
*                操作状态(1)+长地址(8)+0x72data(32) + 0x73data(3+28)
*                操作状态(1)+长地址(8)+0x72data(32) + 0x74data(3+21+7)
************************************************************************************************/
void DataHandle_ReadLockDataProc(DATA_FRAME_STRUCT *DataFrmPtr)
{
    COMMAND_TYPE cmd;
    uint8 i, dataLen, *dataBufPtr = NULL, locknum, locklen, statusSize;
    uint16 nodeId;
    METER_DATA_SAVE_FORMAT *meterBufPtr = NULL;

    cmd = DataFrmPtr->Command;
    dataLen = 0;
    nodeId = Data_FindNodeId(0, DataFrmPtr->DataBuf);
    for (i = LONG_ADDR_SIZE; i > 0; i--) {
        DataFrmPtr->DataBuf[i] = DataFrmPtr->DataBuf[i - 1];
    }
    dataBufPtr = DataFrmPtr->DataBuf + 1 + LONG_ADDR_SIZE;

    // 没有此节点或请求的数据类型与集中器的工作模式不一致或申请内存失败等情况
    if (NULL_U16_ID == nodeId) {
        DataFrmPtr->DataBuf[0] = OP_ObjectNotExist;
    } else if ((void *)0 == (meterBufPtr = OSMemGetOpt(SmallMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        DataFrmPtr->DataBuf[0] = OP_Failure;
    } else if (Read_Lock_Data_Cmd == cmd) {
        dataLen = LOCK_STATUS_DATA_SIZE + 3 + LOCK_GPRS_DATA_SIZE;
        Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, READ_ATTR);
        if (0 != memcmp(SubNodes[nodeId].LongAddr, meterBufPtr->Address, LONG_ADDR_SIZE)) {
            Data_MeterDataInit(meterBufPtr, nodeId, NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT));
        }
        if (meterBufPtr->Crc8MeterData == CalCrc8(meterBufPtr->MeterData, NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT))) {
            DataFrmPtr->DataBuf[0] = OP_Succeed;
            memcpy(&DataFrmPtr->DataBuf[1], SubNodes[nodeId].LongAddr, LONG_ADDR_SIZE);
            locknum = meterBufPtr->RxLastDataNum - 1; // 集中器所保存的最新一条数据所保存位置
            if( 0 == locknum+1 ){
                // 如果没有数据，全部赋值0
                memset(&DataFrmPtr->DataBuf[1+LONG_ADDR_SIZE], 0, LOCK_STATUS_DATA_SIZE);
            }else {
                // 最新一条数据
                memcpy(&DataFrmPtr->DataBuf[1+LONG_ADDR_SIZE], &meterBufPtr->MeterData[1 + locknum * LOCK_STATUS_DATA_SIZE], LOCK_STATUS_DATA_SIZE);
            }
            statusSize = LOCK_STATUS_DATA_NUM * LOCK_STATUS_DATA_SIZE + 1;
            locklen = meterBufPtr->MeterData[statusSize + 2]; // 服务器下发命令的长度
            if( (0 != locklen) &&
				( Lock_Status_Issued_Cmd == meterBufPtr->MeterData[statusSize] ||
				  Lock_Key_Updata_Cmd == meterBufPtr->MeterData[statusSize])){
                memcpy(&DataFrmPtr->DataBuf[1+LONG_ADDR_SIZE + LOCK_STATUS_DATA_SIZE], &meterBufPtr->MeterData[statusSize], 3+LOCK_GPRS_DATA_SIZE);
            } else {// 无数据或数据不正常
            	memset(&DataFrmPtr->DataBuf[1+LONG_ADDR_SIZE + LOCK_STATUS_DATA_SIZE], 0, 3+LOCK_GPRS_DATA_SIZE);
            }
        } else {
            DataFrmPtr->DataBuf[0] = OP_ErrorMeterData;
            memset(dataBufPtr, 0, dataLen);
        }
        OSMemPut(SmallMemoryPtr, meterBufPtr);
    }

    DataFrmPtr->DataLen = 1 + LONG_ADDR_SIZE + dataLen;
}


/************************************************************************************************
* Function Name: DataHandle_ReadLockDataProc
* Decription   : 批量获取集中器所保存的最新一条地锁状态信息
* Input        : DataFrmPtr-指向数据帧的指针
* Output       : 无
* Others       : 下行:起始节点序号(2)+读取的数量(1)
*                地锁数据上行:
*	节点总数量(2)+本次返回的数量N(1)+N*[操作状态(1)+长地址(8)+地锁最新数据(M)]
*
************************************************************************************************/
void DataHandle_BatchReadLockDataProc(DATA_FRAME_STRUCT *DataFrmPtr)
{
    COMMAND_TYPE cmd;
    uint8 readCount, ackCount, dataLen, blockLen;
    uint8 *dataBufPtr = NULL, *opStatusPtr = NULL;
    uint16 nodeId, startId, totalNodes;
    METER_DATA_SAVE_FORMAT *meterBufPtr = NULL;
    uint8 locknum, statusSize, locklen;

    if ((void *)0 == (meterBufPtr = OSMemGetOpt(SmallMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        DataFrmPtr->DataBuf[0] = OP_Failure;
        DataFrmPtr->DataLen = 1;
        return;
    }
    cmd = DataFrmPtr->Command;
    if ((Batch_Read_Lock_Data_Cmd != cmd)) {
        DataFrmPtr->DataBuf[0] = OP_NoFunction;
        DataFrmPtr->DataLen = 1;
        OSMemPut(SmallMemoryPtr, meterBufPtr);
        return;
    }
    // 单条数据的长度
    dataLen = LOCK_STATUS_DATA_SIZE + 3 + LOCK_GPRS_DATA_SIZE;
    startId = ((uint16 *)DataFrmPtr->DataBuf)[0];
    readCount = DataFrmPtr->DataBuf[2];
    ackCount = 0;
    totalNodes = 0;
    dataBufPtr = DataFrmPtr->DataBuf + 3;
    blockLen = dataLen + 1 + LONG_ADDR_SIZE;
    for (nodeId = 0; nodeId < Concentrator.MaxNodeId; nodeId++) {
        if (0 == memcmp(SubNodes[nodeId].LongAddr, NullAddress, LONG_ADDR_SIZE)) {
            continue;
        } else if ((SubNodes[nodeId].DevType & 0xF0) == 0xF0) {
            continue;
        } else {
            totalNodes++;
            if (totalNodes > startId && ackCount < readCount && dataBufPtr - DataFrmPtr->DataBuf + blockLen < GPRS_DATA_MAX_DATA) {
                ackCount++;
                opStatusPtr = dataBufPtr++;
                memcpy(dataBufPtr, SubNodes[nodeId].LongAddr, LONG_ADDR_SIZE);
                dataBufPtr += LONG_ADDR_SIZE;
                Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, READ_ATTR);
                if (0 != memcmp(SubNodes[nodeId].LongAddr, meterBufPtr->Address, LONG_ADDR_SIZE)) {
                    Data_MeterDataInit(meterBufPtr, nodeId, NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT));
                }
                if (meterBufPtr->Crc8MeterData == CalCrc8(meterBufPtr->MeterData, NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT))) {
                    *opStatusPtr = OP_Succeed;

                    //memcpy(&dataBufPtr[1], SubNodes[nodeId].LongAddr, LONG_ADDR_SIZE);
                    locknum = meterBufPtr->RxLastDataNum - 1; // 集中器所保存的最新一条数据所保存位置
                    if( 0 == locknum+1 ){
                        // 如果没有数据，全部赋值0
                        memset(&dataBufPtr[0], 0, LOCK_STATUS_DATA_SIZE);
                    }else {
                        // 最新一条数据
                        memcpy(&dataBufPtr[0], &meterBufPtr->MeterData[1 + locknum * LOCK_STATUS_DATA_SIZE], LOCK_STATUS_DATA_SIZE);
                    }
                    statusSize = LOCK_STATUS_DATA_NUM * LOCK_STATUS_DATA_SIZE + 1;
                    locklen = meterBufPtr->MeterData[statusSize + 2]; // 服务器下发命令的长度
                    if( (0 != locklen) &&
                        ( Lock_Status_Issued_Cmd == meterBufPtr->MeterData[statusSize] ||
                          Lock_Key_Updata_Cmd == meterBufPtr->MeterData[statusSize])){
                        memcpy(&dataBufPtr[LOCK_STATUS_DATA_SIZE], &meterBufPtr->MeterData[statusSize], 3+LOCK_GPRS_DATA_SIZE);
                    } else {// 无数据或数据不正常
                        memset(&dataBufPtr[LOCK_STATUS_DATA_SIZE], 0, 3+LOCK_GPRS_DATA_SIZE);
                    }
                } else {
                    *opStatusPtr = OP_ErrorMeterData;
                    memset(dataBufPtr, 0, dataLen);
                }
                dataBufPtr += dataLen;
            }
        }
    }
    DataFrmPtr->DataBuf[0] = (uint8)totalNodes;
    DataFrmPtr->DataBuf[1] = (uint8)(totalNodes >> 8);
    DataFrmPtr->DataBuf[2] = ackCount;
    DataFrmPtr->DataLen = dataBufPtr - DataFrmPtr->DataBuf;
    OSMemPut(SmallMemoryPtr, meterBufPtr);
    return;
}

/************************************************************************************************
* Function Name: DataHandle_LockStatusReportProc
* Decription   : 地锁上报状态数据处理函数
* Input        : DataFrmPtr-接收到的数据指针
* Output       : 后续是否需要应答处理
* Others       : 用于处理主动地锁的状态改变数据
************************************************************************************************/
bool DataHandle_LockStatusReportProc(DATA_FRAME_STRUCT *DataFrmPtr)
{
	uint8 result;
	result = DataHandle_LockStatusDataSaveProc(DataFrmPtr);

	// 数据测试，对于地锁的命令不予应答，只接收。
	if (CURRENT_VERSION == SRWF_CTP_TEST){
		OSMemPut(LargeMemoryPtr, DataFrmPtr);
		return NONE_ACK;
	}

    // 地锁上报状态数据处理函数，保存 + 回应地锁 + 上报服务器
    // 成功或者数据长度不对则应答。
    // 如果集中器空间已满，则不应答。
    if (OP_Succeed == result) {
        // 创建下行数据
        DataFrmPtr->DeviceType = Dev_Concentrator;
        DataFrmPtr->DataBuf[0] = OP_Succeed;
        DataFrmPtr->DataLen = 1;
        DataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, ACK_PKG, DOWN_DIR);
        DataHandle_SetPkgPath(DataFrmPtr, REVERSED);
        DataHandle_CreateTxData(DataFrmPtr);
    } else if (OP_Failure == result){
        DataFrmPtr->DeviceType = Dev_Concentrator;
        DataFrmPtr->DataBuf[0] = OP_Failure;
        DataFrmPtr->DataLen = 1;
        DataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, ACK_PKG, DOWN_DIR);
        DataHandle_SetPkgPath(DataFrmPtr, REVERSED);
        DataHandle_CreateTxData(DataFrmPtr);
    } else {
		OSMemPut(LargeMemoryPtr, DataFrmPtr);
	}

    return NONE_ACK;
}


/************************************************************************************************
* Function Name: DataHandle_LockStatusIssuedProc
* Decription   : 服务器下发地锁开关命令处理函数
* Input        : DataFrmPtr-接收到的数据指针
* Output       : 后续是否需要应答处理
* Others       : 用于处理主动地锁的状态改变数据
************************************************************************************************/
bool DataHandle_LockStatusIssuedProc(DATA_FRAME_STRUCT *DataFrmPtr)
{
	// 服务器下发地锁开关命令保存并回应
    if (SUCCESS == DataHandle_LockGprsDataSaveProc(DataFrmPtr)) {
        // 创建下行数据
        DataFrmPtr->PortNo = Uart_Gprs;
        DataFrmPtr->DeviceType = Dev_Concentrator;
        DataFrmPtr->RouteInfo.CurPos = 0;
        DataFrmPtr->RouteInfo.Level = 1;
        memcpy(DataFrmPtr->Route[0], Concentrator.LongAddr, LONG_ADDR_SIZE);
        DataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, ACK_PKG, DOWN_DIR);
        DataHandle_SetPkgPath(DataFrmPtr, REVERSED);
        DataHandle_CreateTxData(DataFrmPtr);
    } else {
        // 创建下行数据
        DataFrmPtr->PortNo = Uart_Gprs;
        DataFrmPtr->DeviceType = Dev_Concentrator;
        DataFrmPtr->DataBuf[0] = OP_Failure;
        DataFrmPtr->DataLen = 1;
        DataFrmPtr->RouteInfo.CurPos = 0;
        DataFrmPtr->RouteInfo.Level = 1;
        memcpy(DataFrmPtr->Route[0], Concentrator.LongAddr, LONG_ADDR_SIZE);
        DataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, ACK_PKG, DOWN_DIR);
        DataHandle_SetPkgPath(DataFrmPtr, REVERSED);
        DataHandle_CreateTxData(DataFrmPtr);
    }
    return NONE_ACK;
}

/************************************************************************************************
* Function Name: DataHandle_LockKeyUpdataProc
* Decription   : 服务器下发地锁更新密钥命令处理函数
* Input        : DataFrmPtr-接收到的数据指针
* Output       : 后续是否需要应答处理
* Others       : 用于处理下发地锁更新密钥命令处理函数
************************************************************************************************/
bool DataHandle_LockKeyUpdataProc(DATA_FRAME_STRUCT *DataFrmPtr)
{
	// 服务器下发地锁密钥更新命令保存并回应
    if (SUCCESS == DataHandle_LockGprsDataSaveProc(DataFrmPtr)) {
        // 创建下行数据
        DataFrmPtr->PortNo = Uart_Gprs;
        DataFrmPtr->DeviceType = Dev_Concentrator;
        DataFrmPtr->RouteInfo.CurPos = 0;
        DataFrmPtr->RouteInfo.Level = 1;
        memcpy(DataFrmPtr->Route[0], Concentrator.LongAddr, LONG_ADDR_SIZE);
        DataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, ACK_PKG, DOWN_DIR);
        DataHandle_SetPkgPath(DataFrmPtr, REVERSED);
        DataHandle_CreateTxData(DataFrmPtr);
    } else {
        // 创建下行数据
        DataFrmPtr->PortNo = Uart_Gprs;
        DataFrmPtr->DeviceType = Dev_Concentrator;
        DataFrmPtr->DataBuf[0] = OP_Failure;
        DataFrmPtr->DataLen = 1;
        DataFrmPtr->RouteInfo.CurPos = 0;
        DataFrmPtr->RouteInfo.Level = 1;
        memcpy(DataFrmPtr->Route[0], Concentrator.LongAddr, LONG_ADDR_SIZE);
        DataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, ACK_PKG, DOWN_DIR);
        DataHandle_SetPkgPath(DataFrmPtr, REVERSED);
        DataHandle_CreateTxData(DataFrmPtr);
    }
    return NONE_ACK;
}

/************************************************************************************************
* Function Name: DataHandle_LockSensorStatusProc
* Decription   : 地锁传感器状态信息处理函数
* Input        : DataFrmPtr-接收到的数据指针
************************************************************************************************/
bool DataHandle_LockSensorStatusProc(DATA_FRAME_STRUCT *DataFrmPtr)
{
	uint8 result;

	result = DataHandle_LockSensorStatusUp(DataFrmPtr);

	// 地锁传感器状态信息直接上传服务器
    if (OP_Succeed == result) {
		// 创建下行数据
		DataFrmPtr->DeviceType = Dev_Concentrator;
		DataFrmPtr->DataBuf[0] = OP_Succeed;
		DataFrmPtr->DataLen = 1;
		DataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, ACK_PKG, DOWN_DIR);
		DataHandle_SetPkgPath(DataFrmPtr, REVERSED);
		DataHandle_CreateTxData(DataFrmPtr);
	} else if(OP_Failure == result) {
		DataFrmPtr->DeviceType = Dev_Concentrator;
		DataFrmPtr->DataBuf[0] = OP_Failure;
		DataFrmPtr->DataLen = 1;
		DataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, ACK_PKG, DOWN_DIR);
		DataHandle_SetPkgPath(DataFrmPtr, REVERSED);
		DataHandle_CreateTxData(DataFrmPtr);
	}

    return NONE_ACK;
}


/************************************************************************************************
* Function Name: DataHandle_RTCTimingTask
* Decription   : 实时时钟校时处理任务
* Input        : *p_arg-参数指针
* Output       : 无
* Others       : 无
************************************************************************************************/
void DataHandle_RTCTimingTask(void *p_arg)
{
    uint8 err;
    DATA_HANDLE_TASK *taskPtr = NULL;
    DATA_FRAME_STRUCT *txDataFrmPtr = NULL, *rxDataFrmPtr = NULL;

    // 创建上行校时数据包
    TaskRunStatus.RTCTiming = TRUE;
    if ((void *)0 != (txDataFrmPtr = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        txDataFrmPtr->PortNo = Uart_Gprs;
        txDataFrmPtr->PkgLength = DATA_FIXED_AREA_LENGTH;
        txDataFrmPtr->PkgSn = PkgNo++;
        txDataFrmPtr->Command = CONC_RTC_Timing;
        txDataFrmPtr->DeviceType = Dev_Concentrator;
        txDataFrmPtr->Life_Ack.Content = 0x0F;
        txDataFrmPtr->RouteInfo.CurPos = 0;
        txDataFrmPtr->RouteInfo.Level = 1;
        memcpy(txDataFrmPtr->Route[0], Concentrator.LongAddr, LONG_ADDR_SIZE);
        txDataFrmPtr->DataLen = 0;

        taskPtr = (DATA_HANDLE_TASK *)p_arg;
        taskPtr->Command = txDataFrmPtr->Command;
        taskPtr->NodeId = DATA_CENTER_ID;
        taskPtr->PkgSn = txDataFrmPtr->PkgSn;

        // 创建发送数据包
        txDataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NEED_ACK, CMD_PKG, UP_DIR);
        DataHandle_SetPkgPath(txDataFrmPtr, UNREVERSED);
        DataHandle_CreateTxData(txDataFrmPtr);

        // 等待服务器的应答
        rxDataFrmPtr = OSMboxPend(taskPtr->Mbox, GPRS_WAIT_ACK_OVERTIME, &err);
        if ((void *)0 == rxDataFrmPtr) {
            RTCTimingTimer = 300;               // 如果超时则5分钟后重试
        } else {
            if (SUCCESS == Rtc_Set(*(RTC_TIME *)(rxDataFrmPtr->DataBuf), Format_Bcd)) {
                RTCTimingTimer = RTCTIMING_INTERVAL_TIME;
            } else {
                RTCTimingTimer = 5;             // 如果校时失败则5秒后重试
            }
            OSMemPut(LargeMemoryPtr, rxDataFrmPtr);
        }

    }

    // 销毁本任务,此处必须先禁止任务调度,否则无法释放本任务占用的内存空间
    OSMboxDel(taskPtr->Mbox, OS_DEL_ALWAYS, &err);
    OSSchedLock();
    OSTaskDel(OS_PRIO_SELF);
    OSMemPut(LargeMemoryPtr, taskPtr->StkPtr);
    taskPtr->StkPtr = (void *)0;
    TaskRunStatus.RTCTiming = FALSE;
    OSSchedUnlock();
}

/************************************************************************************************
* Function Name: DataHandle_RTCTimingProc
* Decription   : 集中器实时时钟主动校时处理函数
* Input        : 无
* Output       : 无
* Others       : 每隔一段时间就启动一次校时任务
************************************************************************************************/
void DataHandle_RTCTimingProc(void)
{
    uint8 err;
    DATA_HANDLE_TASK *taskPtr = NULL;

    // 检查Gprs是否在线或者任务是否正在运行中
    if (FALSE == Gprs.Online || TRUE == TaskRunStatus.RTCTiming) {
        RTCTimingTimer = 60;
        return;
    }

    if ((void *)0 == (taskPtr = DataHandle_GetEmptyTaskPtr())) {
        return;
    }
    if ((void *)0 == (taskPtr->StkPtr = (OS_STK *)OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        return;
    }
    taskPtr->Mbox = OSMboxCreate((void *)0);
    taskPtr->Msg = (void *)0;
    if (OS_ERR_NONE != OSTaskCreate(DataHandle_RTCTimingTask, taskPtr,
        taskPtr->StkPtr + MEM_LARGE_BLOCK_LEN / sizeof(OS_STK) - 1, taskPtr->Prio)) {
        OSMemPut(LargeMemoryPtr, taskPtr->StkPtr);
        taskPtr->StkPtr = (void *)0;
        OSMboxDel(taskPtr->Mbox, OS_DEL_ALWAYS, &err);
    }
}


/************************************************************************************************
* Function Name: DataHandle_DataUploadTask
* Decription   : 数据上传处理任务
* Input        : *p_arg-参数指针
* Output       : 无
* Others       : 当需要上传的数据够一包时,或需实时上传时,或一天快结束时,启动数据上传服务器功能
************************************************************************************************/
void DataHandle_DataUploadTask(void *p_arg)
{
    COMMAND_TYPE cmd;
    uint8 i, retry, err, count, dataLen, meterDataLen;
    uint16 nodeId, highPrioDataCount, rxMeterDataCount, uploadMaxCountOnePkg, *record = NULL;
    RTC_TIME rtcTime;
    DATA_HANDLE_TASK *taskPtr = NULL;
    DATA_FRAME_STRUCT *txDataFrmPtr = NULL, *rxDataFrmPtr = NULL;
    METER_DATA_SAVE_FORMAT *meterBufPtr = NULL;
	uint8 lockStatusNum;

    // 计算未上报的节点的数量
    taskPtr = (DATA_HANDLE_TASK *)p_arg;
    TaskRunStatus.DataUpload = TRUE;
    meterBufPtr = (void *)0;
    record = (void *)0;
    Rtc_Get(&rtcTime, Format_Bcd);

    retry = 5;
    while (retry-- && FALSE == TaskRunStatus.DataForward) {
        DataUploadTimer = 60;
        highPrioDataCount = 0;
        rxMeterDataCount = 0;
        for (nodeId = 0; nodeId < Concentrator.MaxNodeId; nodeId++) {
            if (0 == memcmp(SubNodes[nodeId].LongAddr, NullAddress, LONG_ADDR_SIZE)) {
                continue;
            }
            if ((SubNodes[nodeId].DevType & 0xF0) == 0xF0) {
                continue;
            }
            if (TRUE == SubNodes[nodeId].Property.UploadData) {
                continue;
            }
            if (HIGH == SubNodes[nodeId].Property.UploadPrio) {
                highPrioDataCount++;
            }
            rxMeterDataCount++;
        }

        // 如果没有数据上传则跳出
        if (0 == highPrioDataCount && (0 == rxMeterDataCount || 0 == Concentrator.Param.DataUploadCtrl)) {
            break;
        }

        uploadMaxCountOnePkg = (GPRS_DATA_MAX_DATA - DATA_FIXED_AREA_LENGTH) / (LONG_ADDR_SIZE + LOCK_STATUS_DATA_SIZE);
        dataLen = LOCK_STATUS_DATA_SIZE;
        cmd = Lock_Status_Report_Cmd;

        meterDataLen = NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT);

        if ((void *)0 == meterBufPtr) {
            if ((void *)0 == (meterBufPtr = OSMemGetOpt(SmallMemoryPtr, 10, TIME_DELAY_MS(50)))) {
                break;
            }
        }
        if ((void *)0 == record) {
            if ((void *)0 == (record = OSMemGetOpt(SmallMemoryPtr, 10, TIME_DELAY_MS(50)))) {
                break;
            }
        }
        if ((void *)0 == (txDataFrmPtr = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
            break;
        }
        txDataFrmPtr->PortNo = Uart_Gprs;
        txDataFrmPtr->PkgLength = DATA_FIXED_AREA_LENGTH;
        txDataFrmPtr->PkgSn = PkgNo++;
        txDataFrmPtr->Command = cmd;
        txDataFrmPtr->DeviceType = Dev_Concentrator;
        txDataFrmPtr->Life_Ack.LifeCycle = 0x0F;
        txDataFrmPtr->Life_Ack.AckChannel = DEFAULT_RX_CHANNEL;
        txDataFrmPtr->RouteInfo.CurPos = 0;
        txDataFrmPtr->RouteInfo.Level = 1;
        memcpy(txDataFrmPtr->Route[0], Concentrator.LongAddr, LONG_ADDR_SIZE);
        txDataFrmPtr->DataLen = 1;
        count = 0;
        for (nodeId = 0; nodeId < Concentrator.MaxNodeId; nodeId++) {
            if (0 == memcmp(SubNodes[nodeId].LongAddr, NullAddress, LONG_ADDR_SIZE)) {
                continue;
            }
            if ((SubNodes[nodeId].DevType & 0xF0) == 0xF0) {
                continue;
            }
            if (TRUE == SubNodes[nodeId].Property.UploadData) {
                continue;
            }
            if ((highPrioDataCount > 0 && HIGH == SubNodes[nodeId].Property.UploadPrio) || 0 == highPrioDataCount) {
                Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, READ_ATTR);
                uint8 crctest = CalCrc8(meterBufPtr->MeterData, NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT));
                if (0 != memcmp(meterBufPtr->Address, SubNodes[nodeId].LongAddr, LONG_ADDR_SIZE) ||
                    meterBufPtr->Crc8MeterData != crctest) {
                    Data_MeterDataInit(meterBufPtr, nodeId, meterDataLen);
                    Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, meterDataLen, WRITE_ATTR);
                    continue;
                } else {
                	lockStatusNum = meterBufPtr->MeterData[0];
                	while(lockStatusNum-- > 0){
	                    memcpy(txDataFrmPtr->DataBuf + txDataFrmPtr->DataLen, SubNodes[nodeId].LongAddr, LONG_ADDR_SIZE);
	                    txDataFrmPtr->DataLen += LONG_ADDR_SIZE;
	                    memcpy(txDataFrmPtr->DataBuf + txDataFrmPtr->DataLen, &meterBufPtr->MeterData[LOCK_STATUS_DATA_SIZE*lockStatusNum + 1], dataLen);
                            txDataFrmPtr->DataLen += dataLen;
	                    *(record + count++) = nodeId;
	                    if (count >= uploadMaxCountOnePkg) {
	                        break;
	                    }
                        }
                    if (count >= uploadMaxCountOnePkg) {
                        break;
                    }
                }
            }
        }

        if (0 == count) {
            OSMemPut(LargeMemoryPtr, txDataFrmPtr);
            break;
        }
#if PRINT_INFO
        uint8 test[10] = {0 };
        Gprs_OutputDebugMsg(0,"\n--数据上传，cmd+len+count = ");
        test[0] = cmd;
        test[1] = txDataFrmPtr->DataLen;
        test[2] = count;
        DebugOutputLength(&test[0], 3);
#endif
        txDataFrmPtr->DataBuf[0] = count;
        taskPtr->Command = txDataFrmPtr->Command;
        taskPtr->NodeId = DATA_CENTER_ID;
        taskPtr->PkgSn = txDataFrmPtr->PkgSn;

        // 创建发送数据包
        txDataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NEED_ACK, CMD_PKG, UP_DIR);
        DataHandle_SetPkgPath(txDataFrmPtr, UNREVERSED);
        DataHandle_CreateTxData(txDataFrmPtr);

        // 等待服务器的应答
        rxDataFrmPtr = OSMboxPend(taskPtr->Mbox, GPRS_WAIT_ACK_OVERTIME, &err);
        if ((void *)0 != rxDataFrmPtr) {
            // 查找那个节点并更改状态保存起
            if (1 == rxDataFrmPtr->DataLen && OP_Succeed == rxDataFrmPtr->DataBuf[0]) {
                retry = 5;
                for (i = 0; i < count; i++) {
                    nodeId = *(record + i);
                    Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, READ_ATTR);
                    if (0 == memcmp(meterBufPtr->Address, SubNodes[nodeId].LongAddr, LONG_ADDR_SIZE)) {
                        meterBufPtr->MeterData[0] -= 1;
                        if( 0 == meterBufPtr->MeterData[0] ){
                            meterBufPtr->Property.UploadData = SubNodes[nodeId].Property.UploadData = TRUE;
                            meterBufPtr->Property.UploadPrio = SubNodes[nodeId].Property.UploadPrio = LOW;
                        }
                        meterBufPtr->Crc8MeterData = CalCrc8(meterBufPtr->MeterData, NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT));
                        Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, WRITE_ATTR);
                    }
                }
            }
            OSMemPut(LargeMemoryPtr, rxDataFrmPtr);
        }
    }

    // 销毁本任务,此处必须先禁止任务调度,否则无法释放本任务占用的内存空间
    if ((void *)0 != meterBufPtr) {
        OSMemPut(SmallMemoryPtr, meterBufPtr);
    }
    if ((void *)0 != record) {
        OSMemPut(SmallMemoryPtr, record);
    }
    DataUploadTimer = 0;
    OSMboxDel(taskPtr->Mbox, OS_DEL_ALWAYS, &err);
    OSSchedLock();
    OSTaskDel(OS_PRIO_SELF);
    OSMemPut(LargeMemoryPtr, taskPtr->StkPtr);
    taskPtr->StkPtr = (void *)0;
    TaskRunStatus.DataUpload = FALSE;
    OSSchedUnlock();
}


/************************************************************************************************
* Function Name: DataHandle_DataDownloadTask
* Decription   : 数据下发处理任务
* Input        : *p_arg-参数指针
* Output       : 无
* Others       : 当有需要下发的数据时,启动数据下发地锁
************************************************************************************************/
void DataHandle_DataDownloadTask(void *p_arg)
{
    COMMAND_TYPE cmd;
    uint8 retry, err, dataLen, meterDataLen;
    uint16 nodeId;
    RTC_TIME rtcTime;
    DATA_HANDLE_TASK *taskPtr;
    DATA_FRAME_STRUCT *txDataFrmPtr = NULL, *rxDataFrmPtr = NULL, *gprsDataFrmPtr = NULL;
    METER_DATA_SAVE_FORMAT *meterBufPtr = NULL;
    uint8 statusSize, cmdStatus;

    // 计算未上报的节点的数量
    taskPtr = (DATA_HANDLE_TASK *)p_arg;
    TaskRunStatus.DataDownload = TRUE;
    meterBufPtr = (void *)0;
    Rtc_Get(&rtcTime, Format_Bcd);

    retry = 1;
    while (retry-- && FALSE == TaskRunStatus.DataForward) {
        // 获取到可以下发数据的地锁节点号
        nodeId = DataDownloadNodeId;

        if ((0 == memcmp(SubNodes[nodeId].LongAddr, NullAddress, LONG_ADDR_SIZE)) ||
            (LOW == SubNodes[nodeId].Property.DownloadPrio)) {
            break;
        }
        if ( (void *)0 == meterBufPtr){
            if ((void *)0 == (meterBufPtr = OSMemGetOpt(SmallMemoryPtr, 10, TIME_DELAY_MS(50)))) {
                break;
            }
        }
        if ((void *)0 == (txDataFrmPtr = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
            break;
        }
        if ( (void *)0 == gprsDataFrmPtr){
            if ((void *)0 == (gprsDataFrmPtr = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
                break;
            }
        }
        txDataFrmPtr->PortNo = Usart_Rf;
        txDataFrmPtr->PkgLength = DATA_FIXED_AREA_LENGTH;
        txDataFrmPtr->PkgSn = PkgNo++;
        txDataFrmPtr->DeviceType = Dev_Concentrator;
        txDataFrmPtr->Life_Ack.LifeCycle = 0x0F;
        txDataFrmPtr->Life_Ack.AckChannel = DEFAULT_RX_CHANNEL;
        txDataFrmPtr->RouteInfo.CurPos = 0;
        txDataFrmPtr->RouteInfo.Level = 2;
        memcpy(txDataFrmPtr->Route[0], Concentrator.LongAddr, LONG_ADDR_SIZE);
        memcpy(txDataFrmPtr->Route[1], SubNodes[nodeId].LongAddr, LONG_ADDR_SIZE);

        statusSize = LOCK_STATUS_DATA_NUM * LOCK_STATUS_DATA_SIZE + 1;
        meterDataLen = NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT);

        Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, READ_ATTR);
        cmdStatus = meterBufPtr->MeterData[statusSize+1];
        if(0x1 == cmdStatus){
			OSMemPut(SmallMemoryPtr, meterBufPtr);
			OSMemPut(LargeMemoryPtr, gprsDataFrmPtr);
			OSMemPut(LargeMemoryPtr, txDataFrmPtr);
			break;
        }
        cmd = (COMMAND_TYPE)meterBufPtr->MeterData[statusSize];
        dataLen = meterBufPtr->MeterData[statusSize + 2];
		// 防止出现清空档案后第一次上传数据出现下发空命令
		if( (dataLen == 0x0) || (cmd == 0x0) ){
			OSMemPut(SmallMemoryPtr, meterBufPtr);
			OSMemPut(LargeMemoryPtr, gprsDataFrmPtr);
			OSMemPut(LargeMemoryPtr, txDataFrmPtr);
			break;
		}
        if (0 != memcmp(meterBufPtr->Address, SubNodes[nodeId].LongAddr, LONG_ADDR_SIZE) ||
            meterBufPtr->Crc8MeterData != CalCrc8(meterBufPtr->MeterData, NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT))) {
            Data_MeterDataInit(meterBufPtr, nodeId, meterDataLen);
            Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, meterDataLen, WRITE_ATTR);
            break;
        } else {
            memcpy(txDataFrmPtr->DataBuf, &meterBufPtr->MeterData[statusSize + 3], dataLen);
            txDataFrmPtr->DataLen = dataLen;
        }

#if PRINT_INFO
        uint8 test[10] = {0};
        Gprs_OutputDebugMsg(0,"\n--数据下发，cmd+len+addr = ");
        test[0] = cmd;
        test[1] = txDataFrmPtr->DataLen;
		memcpy(&test[2], &SubNodes[nodeId].LongAddr[3], 5);
        DebugOutputLength(&test[0], 7);
#endif
        txDataFrmPtr->Command = cmd;
        taskPtr->Command = txDataFrmPtr->Command;
        taskPtr->NodeId = nodeId;
        taskPtr->PkgSn = txDataFrmPtr->PkgSn;

        memcpy(gprsDataFrmPtr, txDataFrmPtr, MEM_LARGE_BLOCK_LEN);
        // 创建发送数据包
        txDataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NEED_ACK, CMD_PKG, UP_DIR);
        DataHandle_SetPkgPath(txDataFrmPtr, UNREVERSED);
        DataHandle_CreateTxData(txDataFrmPtr);

        // 等待服务器的应答
        rxDataFrmPtr = OSMboxPend(taskPtr->Mbox, GPRS_WAIT_ACK_OVERTIME, &err);
        if ((void *)0 != rxDataFrmPtr) {
            // 查找那个节点并更改状态保存起
            if (1 == rxDataFrmPtr->DataLen && OP_Succeed == rxDataFrmPtr->DataBuf[0]) {
                retry = 0;
                Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, READ_ATTR);
                // 0x0:未下发， 0x1 ：下发成功 ，0x2 ：下发失败
                meterBufPtr->MeterData[statusSize + 1] = 0x1;
                meterBufPtr->Property.DownloadPrio = SubNodes[nodeId].Property.DownloadPrio = LOW;
                meterBufPtr->Crc8MeterData = CalCrc8(meterBufPtr->MeterData, NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT));
                Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, WRITE_ATTR);
				if( Lock_Key_Updata_Cmd == gprsDataFrmPtr->Command){
					gprsDataFrmPtr->Command = Lock_Key_Updata_Status_Cmd;
	                // 数据下发成功，上传服务器。
	                gprsDataFrmPtr->PortNo = Uart_Gprs;
	                gprsDataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, CMD_PKG, UP_DIR);
	                DataHandle_SetPkgPath(gprsDataFrmPtr, UNREVERSED);
	                DataHandle_CreateTxData(gprsDataFrmPtr);
				}else{
					OSMemPut(LargeMemoryPtr, gprsDataFrmPtr);
				}
            } else {
				OSMemPut(LargeMemoryPtr, gprsDataFrmPtr);
			}
            OSMemPut(LargeMemoryPtr, rxDataFrmPtr);
        } else{
            retry = 0;
            Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, READ_ATTR);
            // 0x0:未下发， 0x1 ：下发成功 ，0x2 ：下发失败
            meterBufPtr->MeterData[statusSize + 1] = 0x2;
            meterBufPtr->Crc8MeterData = CalCrc8(meterBufPtr->MeterData, NODE_INFO_SIZE - sizeof(METER_DATA_SAVE_FORMAT));
            Eeprom_ReadWrite((uint8 *)meterBufPtr, nodeId * NODE_INFO_SIZE, NODE_INFO_SIZE, WRITE_ATTR);
			OSMemPut(LargeMemoryPtr, gprsDataFrmPtr);
		}
    }

    // 销毁本任务,此处必须先禁止任务调度,否则无法释放本任务占用的内存空间
    if ((void *)0 != meterBufPtr) {
        OSMemPut(SmallMemoryPtr, meterBufPtr);
    }
    DataDownloadTimer = 0;
    DataDownloadNodeId = 0xABCD;
    OSMboxDel(taskPtr->Mbox, OS_DEL_ALWAYS, &err);
    OSSchedLock();
    OSTaskDel(OS_PRIO_SELF);
    OSMemPut(LargeMemoryPtr, taskPtr->StkPtr);
    taskPtr->StkPtr = (void *)0;
    TaskRunStatus.DataDownload = FALSE;
    OSSchedUnlock();
}


/************************************************************************************************
* Function Name: DataHandle_DataUploadProc
* Decription   : 数据上传服务器处理函数
* Input        : 无
* Output       : 无
* Others       : 判断是否有数据需要上传并启动上传任务
************************************************************************************************/
void DataHandle_DataUploadProc(void)
{
    uint8 err;
    DATA_HANDLE_TASK *taskPtr = NULL;

    DataUploadTimer = 60;

    // Gprs必须在线,并且上传任务没有运行
    if (FALSE == Gprs.Online || TRUE == TaskRunStatus.DataUpload || TRUE == TaskRunStatus.DataForward) {
        return;
    }
    // 搜索未被占用的空间,创建数据上传任务
    if ((void *)0 == (taskPtr = DataHandle_GetEmptyTaskPtr())) {
        return;
    }
    if ((void *)0 == (taskPtr->StkPtr = (OS_STK *)OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        return;
    }
    taskPtr->Mbox = OSMboxCreate((void *)0);
    taskPtr->Msg = (void *)0;
    if (OS_ERR_NONE != OSTaskCreate(DataHandle_DataUploadTask, taskPtr,
        taskPtr->StkPtr + MEM_LARGE_BLOCK_LEN / sizeof(OS_STK) - 1, taskPtr->Prio)) {
        OSMemPut(LargeMemoryPtr, taskPtr->StkPtr);
        taskPtr->StkPtr = (void *)0;
        OSMboxDel(taskPtr->Mbox, OS_DEL_ALWAYS, &err);
    }
}

/************************************************************************************************
* Function Name: DataHandle_DataDownloadProc
* Decription   : 数据下发地锁处理函数
* Input        : 无
* Output       : 无
* Others       : 判断是否有数据需要下发并启动下发任务
************************************************************************************************/
void DataHandle_DataDownloadProc(void)
{
    uint8 err;
    DATA_HANDLE_TASK *taskPtr = NULL;

    // 下发任务没有运行
    if (TRUE == TaskRunStatus.DataDownload || TRUE == TaskRunStatus.DataForward) {
        return;
    }
    // 搜索未被占用的空间,创建数据上传任务
    if ((void *)0 == (taskPtr = DataHandle_GetEmptyTaskPtr())) {
        return;
    }
    if ((void *)0 == (taskPtr->StkPtr = (OS_STK *)OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        return;
    }
    taskPtr->Mbox = OSMboxCreate((void *)0);
    taskPtr->Msg = (void *)0;
    if (OS_ERR_NONE != OSTaskCreate(DataHandle_DataDownloadTask, taskPtr,
        taskPtr->StkPtr + MEM_LARGE_BLOCK_LEN / sizeof(OS_STK) - 1, taskPtr->Prio)) {
        OSMemPut(LargeMemoryPtr, taskPtr->StkPtr);
        taskPtr->StkPtr = (void *)0;
        OSMboxDel(taskPtr->Mbox, OS_DEL_ALWAYS, &err);
    }
}

/************************************************************************************************
* Function Name: DataHandle_ResetHostProc
* Decription   : 复位主机模块
************************************************************************************************/
bool DataHandle_ResetHostProc(void)
{
    uint8 err;
    PORT_BUF_FORMAT *txPortBufPtr = NULL;

    // 先申请一个内存用于中间数据处理
    if ((void *)0 == (txPortBufPtr = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        return ERROR;
    }
    txPortBufPtr->Property.PortNo = Usart_Rf;
    txPortBufPtr->Property.FilterDone = 1;
	txPortBufPtr->Length = 0;

    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x55;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0xAA;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x0A;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x17;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x00;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x01;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x03;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0xBB;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x54;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = TAILBYTE;

	if (txPortBufPtr->Property.PortNo < Port_Total &&
		OS_ERR_NONE != OSMboxPost(SerialPort.Port[txPortBufPtr->Property.PortNo].MboxTx, txPortBufPtr)) {
		OSMemPut(LargeMemoryPtr, txPortBufPtr);
        return ERROR;
	} else {
		OSFlagPost(GlobalEventFlag, (OS_FLAGS)(1 << txPortBufPtr->Property.PortNo + SERIALPORT_TX_FLAG_OFFSET), OS_FLAG_SET, &err);
        return SUCCESS;
	}

}

//仅用于快速切换 RF 串口波特率
static void Uart_RF_Config(PORT_NO UartNo,
                         PARITY_TYPE Parity,
                         DATABITS_TYPE DataBits,
                         STOPBITS_TYPE StopBits,
                         uint32 Baudrate)
{
    USART_InitTypeDef USART_InitStruct;
    GPIO_InitTypeDef GPIO_InitStruct;
    NVIC_InitTypeDef NVIC_InitStruct;

    switch (UartNo) {
        case Usart_Rf:
            RCC_APB1PeriphClockCmd(RCC_APB1Periph_USART3, ENABLE);
            RCC_APB2PeriphClockCmd(RCC_APB2Periph_AFIO | RCC_APB2Periph_GPIOB, ENABLE); // 使能Usart3时钟

            GPIO_InitStruct.GPIO_Pin = GPIO_Pin_10;                                     // USART3_Tx PB10
            GPIO_InitStruct.GPIO_Speed = GPIO_Speed_50MHz;
            GPIO_InitStruct.GPIO_Mode =  GPIO_Mode_AF_PP;                               // GPIO_Mode_AF_PP 复用推挽输出 修改为复用开漏输出
            GPIO_Init(GPIOB, &GPIO_InitStruct);
            GPIO_InitStruct.GPIO_Pin = GPIO_Pin_11;                                     // USART3_Rx PB11
            GPIO_InitStruct.GPIO_Mode = GPIO_Mode_IN_FLOATING;                          // 普通输入模式(浮空)
            GPIO_Init(GPIOB, &GPIO_InitStruct);

            USART_InitStruct.USART_BaudRate = Baudrate;                                 // 波特率
            USART_InitStruct.USART_WordLength = DataBits;                               // 位长
            USART_InitStruct.USART_StopBits = StopBits;                                 // 停止位数
            USART_InitStruct.USART_Parity = Parity;                                     // 奇偶校验
            USART_InitStruct.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;
            USART_InitStruct.USART_HardwareFlowControl = USART_HardwareFlowControl_None;// 流控设为无
            USART_Init(USART3, &USART_InitStruct);                                      // 设置串口参数

            NVIC_InitStruct.NVIC_IRQChannel = USART3_IRQn;                              // Usart3中断设置
            NVIC_InitStruct.NVIC_IRQChannelPreemptionPriority = 1;
            NVIC_InitStruct.NVIC_IRQChannelSubPriority = 1;
            NVIC_InitStruct.NVIC_IRQChannelCmd = ENABLE;
            NVIC_Init(&NVIC_InitStruct);

            USART_Cmd(USART3, ENABLE);
            USART_ITConfig(USART3, USART_IT_RXNE, ENABLE);                              // 先使能接收中断
            USART_ITConfig(USART3, USART_IT_TXE, DISABLE);                              // 先禁止发送中断,若先使能发送中断，则可能会先发00
            break;
        default:
            break;
    }

    // Uart参数配置
    // Uart_ParameterInit(UartNo, Baudrate);
}

/************************************************************************************************
* Function Name: DataHandle_Updata_HostTask
* Decription   : 升级主机模块处理任务
************************************************************************************************/
void DataHandle_Updata_HostTask(void *p_arg)
{
    uint8 err;
    DATA_HANDLE_TASK *taskPtr = NULL;
	uint8 *rxDataFrmPtr = NULL;
    uint16 crc16, pkgCodeLen, dataCrc;
    uint32 writeAddr, codeLength;
    uint8 buf[13] = {0};
	uint8 upHostError = 0;

    // 计算未上报的节点的数量
    taskPtr = (DATA_HANDLE_TASK *)p_arg;
    TaskRunStatus.DataUpHost = TRUE;

	PORT_BUF_FORMAT *txPortBufPtr = NULL;


    DataUpHostTimer = 3600;

    // 升级信息保存格式: Crc16(2)+升级文件保存位置(4)+升级代码总长度(4)+Crc16(2)
    memcpy(buf, (uint8 *)FLASH_MODULE_UPGRADE_INFO_START, 12);
    if (((uint16 *)(&buf[10]))[0] != CalCrc16(buf, 10))
    {
        goto UPHOST_ERROR;
    }

    // 提取数据
    crc16 = ((uint16 *)buf)[0]; // 升级代码的校验码
    writeAddr = 0;
    codeLength = ((uint32 *)(&buf[6]))[0]; // 升级代码总长度
    pkgCodeLen = 1000;
	if( 0 == codeLength ){
        goto UPHOST_ERROR;
	}

	while( upHostError < 10){

		// 申请一个内存用于中间数据处理
		if ((void *)0 == (txPortBufPtr = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
			goto UPHOST_ERROR;
		}

// 0x55AA + 总文件Crc16(2)+升级代码总长度(4)+升级文件保存位置(4)+本包代码长度(2)+本包代码数据(N)+ 此包数据Crc16(2)
		pkgCodeLen = (codeLength - writeAddr > 1000) ? 1000 : (codeLength - writeAddr);// 单包的长度
		txPortBufPtr->Property.PortNo = Usart_Rf;
		txPortBufPtr->Property.FilterDone = 1;
		txPortBufPtr->Length = 0;
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x55;
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0xAA;
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)crc16;
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)(crc16 >> 8);
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)codeLength;
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)(codeLength >> 8);
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)(codeLength >> 16);
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)(codeLength >> 24);
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)writeAddr;
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)(writeAddr >> 8);
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)(writeAddr >> 16);
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)(writeAddr >> 24);
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)pkgCodeLen;
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)(pkgCodeLen >> 8);
		memcpy(&txPortBufPtr->Buffer[txPortBufPtr->Length],
			(uint8 *)(FLASH_MODULE_UPGRADECODE_START_ADDR + writeAddr),
			pkgCodeLen);
		txPortBufPtr->Length += pkgCodeLen;
		dataCrc = CalCrc16((uint8 *)(&txPortBufPtr->Buffer[2]), txPortBufPtr->Length-2);	 // Crc16校验
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)((dataCrc)&0xFF);
		txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)((dataCrc >> 8)&0xFF);

        taskPtr->PkgSn = txPortBufPtr->Buffer[2];
        taskPtr->Command = txPortBufPtr->Buffer[3];
        taskPtr->PortNo = Usart_Rf;

		// 第一包数据需要先将主机复位进入 boot 模式，并将串口波特率切到 115200
		if( 0 == writeAddr ){
			OS_ENTER_CRITICAL();
			Uart_RF_Config(Usart_Rf, Parity_Even, DataBits_9, StopBits_1, 9600); 			// RF串口初始化
			OS_EXIT_CRITICAL();
			DataHandle_ResetHostProc(); // 复位主机，使进入 boot模式。
			OSTimeDlyHMSM(0, 0, 0, 300);
			OS_ENTER_CRITICAL();
			Uart_RF_Config(Usart_Rf, Parity_Even, DataBits_9, StopBits_1, 115200); 			// RF串口初始化
			OS_EXIT_CRITICAL();
		}

		if (txPortBufPtr->Property.PortNo < Port_Total &&
			OS_ERR_NONE != OSMboxPost(SerialPort.Port[txPortBufPtr->Property.PortNo].MboxTx, txPortBufPtr)) {
			OSMemPut(LargeMemoryPtr, txPortBufPtr);
		} else {
			OSFlagPost(GlobalEventFlag, (OS_FLAGS)(1 << txPortBufPtr->Property.PortNo + SERIALPORT_TX_FLAG_OFFSET), OS_FLAG_SET, &err);
		}

		// 等待服务器的应答
		rxDataFrmPtr = OSMboxPend(taskPtr->Mbox, TIME_DELAY_MS(1000), &err);
		if ((void *)0 != rxDataFrmPtr) {
			// 查找那个节点并更改状态保存起
			if ( UpHost_Success == rxDataFrmPtr[14] ) {
				writeAddr += pkgCodeLen;
				upHostError = 0;
			} else if ( rxDataFrmPtr[14] > UpHost_Success) {
				upHostError++;
			}
			OSMemPut(LargeMemoryPtr, rxDataFrmPtr);

			// 后续需要将 flash 清空并上报服务器"主机模块升级成功命令"
			if ( writeAddr >= codeLength ) {
				upHostError = 100;
				//Gprs_OutputDebugMsg(0,"\n-- 升级成功 --\n");
				break;
			}
		}else {
			upHostError++;
		}
	}


UPHOST_ERROR:

	// 将串口切回正常工作模式。
	OS_ENTER_CRITICAL();
	Uart_RF_Config(Usart_Rf, Parity_Even, DataBits_9, StopBits_1, 9600);			// RF串口初始化
	OS_EXIT_CRITICAL();

    DataUpHostTimer = 0;
    OSMboxDel(taskPtr->Mbox, OS_DEL_ALWAYS, &err);
    OSSchedLock();
    OSTaskDel(OS_PRIO_SELF);
    OSMemPut(LargeMemoryPtr, taskPtr->StkPtr);
    taskPtr->StkPtr = (void *)0;
    TaskRunStatus.DataUpHost = FALSE;
    OSSchedUnlock();
}


/************************************************************************************************
* Function Name: DataHandle_Updata_HostProc
* Decription   : 升级主机模块
************************************************************************************************/
void DataHandle_Updata_HostProc(void)
{
    uint8 err;
    DATA_HANDLE_TASK *taskPtr = NULL;

    DataUpHostTimer = 60;

    // 确保没有上传任务没有运行
    if (TRUE == TaskRunStatus.DataUpload || TRUE == TaskRunStatus.DataForward ||
		TRUE == TaskRunStatus.DataDownload || TRUE == TaskRunStatus.DataReplenish ||
		TRUE == TaskRunStatus.DataUpHost ) {
        return;
    }
    // 搜索未被占用的空间,创建升级主机任务
    if ((void *)0 == (taskPtr = DataHandle_GetEmptyTaskPtr())) {
        return;
    }
    if ((void *)0 == (taskPtr->StkPtr = (OS_STK *)OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        return;
    }
    taskPtr->Mbox = OSMboxCreate((void *)0);
    taskPtr->Msg = (void *)0;
    if (OS_ERR_NONE != OSTaskCreate(DataHandle_Updata_HostTask, taskPtr,
        taskPtr->StkPtr + MEM_LARGE_BLOCK_LEN / sizeof(OS_STK) - 1, taskPtr->Prio)) {
        OSMemPut(LargeMemoryPtr, taskPtr->StkPtr);
        taskPtr->StkPtr = (void *)0;
        OSMboxDel(taskPtr->Mbox, OS_DEL_ALWAYS, &err);
    }
}


/************************************************************************************************
* Function Name: Data_Module_SwUpdate
* Decription   : 无线模块升级代码保存并处理函数
* Input        : DataFrmPtr-指向数据帧的指针
* Output       : 无
* Others       : 下行: Crc16(2)+写入地址(4)+升级代码总长度(4)+本包升级代码长度(2)+升级代码(N)
*                上行: Crc16(2)+写入地址(4)+操作结果(1)
************************************************************************************************/
void Data_Module_SwUpdate(DATA_FRAME_STRUCT *DataFrmPtr)
{
    uint16 crc16, pkgCodeLen;
    uint32 writeAddr, codeLength;
    uint8 *codeBufPtr = NULL, *dataBufPtr = NULL;
    uint8 buf[12];

    // 提取数据
    dataBufPtr = DataFrmPtr->DataBuf;
    crc16 = ((uint16 *)dataBufPtr)[0];
    writeAddr = ((uint32 *)(dataBufPtr + 2))[0];
    codeLength = ((uint32 *)(dataBufPtr + 6))[0];
    pkgCodeLen = ((uint16 *)(dataBufPtr + 10))[0];
    codeBufPtr = dataBufPtr + 12;

    // 如果升级代码长度错误
    if (codeLength > FLASH_MODULE_UPGRADECODE_SIZE * FLASH_PAGE_SIZE) {
        *(dataBufPtr + 6) = OP_ParameterError;
        DataFrmPtr->DataLen = 7;
        return;
    }

    // 如果收到的写入地址为0,表示有一个新的升级要进行
    if (0 == writeAddr) {
        Flash_Erase(FLASH_MODULE_UPGRADECODE_START_ADDR, FLASH_MODULE_UPGRADECODE_SIZE);
        Flash_Erase(FLASH_MODULE_UPGRADE_INFO_START, FLASH_MODULE_UPGRADE_INFO_SIZE);
        // 升级信息保存格式: Crc16(2)+升级文件保存位置(4)+升级代码总长度(4)+Crc16(2)
        memcpy(buf, dataBufPtr, sizeof(buf));
        ((uint32 *)(&buf[2]))[0] = 0; //FLASH_MODULE_UPGRADECODE_START_ADDR;
        ((uint16 *)(&buf[10]))[0] = CalCrc16(buf, 10);
        Flash_Write(buf, 16, FLASH_MODULE_UPGRADE_INFO_START);
    }

    // 如果程序的校验字节或升级代码总长度错误则返回错误
    if (crc16 != ((uint16 *)FLASH_MODULE_UPGRADE_INFO_START)[0] ||
        codeLength != ((uint32 *)(FLASH_MODULE_UPGRADE_INFO_START + 6))[0]) {
        *(dataBufPtr + 6) = OP_ParameterError;
        DataFrmPtr->DataLen = 7;
        return;
    }

    // 写入升级代码
    if (codeLength >= writeAddr + pkgCodeLen) {
        if (0 != memcmp(codeBufPtr, (uint8 *)(FLASH_MODULE_UPGRADECODE_START_ADDR + writeAddr), pkgCodeLen)) {
            Flash_Write(codeBufPtr, pkgCodeLen, FLASH_MODULE_UPGRADECODE_START_ADDR + writeAddr);
            if (0 != memcmp(codeBufPtr, (uint8 *)(FLASH_MODULE_UPGRADECODE_START_ADDR + writeAddr), pkgCodeLen)) {
                *(dataBufPtr + 6) = OP_Failure;
                DataFrmPtr->DataLen = 7;
                return;
            }
        }
    } else {
        *(dataBufPtr + 6) = OP_ParameterError;
        DataFrmPtr->DataLen = 7;
        return;
    }

    // 检查是否是最后一包
    if (writeAddr + pkgCodeLen >= codeLength) {
        if (crc16 == CalCrc16((uint8 *)FLASH_MODULE_UPGRADECODE_START_ADDR, codeLength)) {
            *(dataBufPtr + 6) = OP_Succeed;
			DataUpHostTimer = 3; // 升级主机模块
        } else {
            *(dataBufPtr + 6) = OP_Failure;
        }
    } else {
        *(dataBufPtr + 6) = OP_Succeed;
    }
    DataFrmPtr->DataLen = 7;
    return;
}

/************************************************************************************************
* Function Name: DataHandle_ReadHostChannelTask
* Decription   : 读取主机模块信道处理任务
* Input        : p_arg-保存原来数据的指针
* Output       : 无
* Others       : 该函数用处理PC,串口,手持机发送数据后的等待任务
************************************************************************************************/
void DataHandle_ReadHostChannelTask(void *p_arg)
{
    uint8 err;
    DATA_HANDLE_TASK *taskPtr;
    DATA_FRAME_STRUCT *txDataFrmPtr;
	uint8 *rxDataFrmPtr;
	uint8 len = 0, dataLen;
	uint16 crc16;

    taskPtr = (DATA_HANDLE_TASK *)p_arg;
    txDataFrmPtr = (DATA_FRAME_STRUCT *)(taskPtr->Msg);

    TaskRunStatus.DataForward = TRUE;
    rxDataFrmPtr = OSMboxPend(taskPtr->Mbox, TIME_DELAY_MS(DELAYTIME_ONE_LAYER), &err);
    if ((void *)0 == rxDataFrmPtr) {
        txDataFrmPtr->DataBuf[0] = OP_Failure;
        txDataFrmPtr->DataLen = 1;
    } else {
    	len = 3;
		if(0x55 == rxDataFrmPtr[len] && 0xAA == rxDataFrmPtr[len+1]){
			dataLen = rxDataFrmPtr[len + 2];
			crc16 = CalCrc16((uint8 *)(&rxDataFrmPtr[len]), dataLen-3); 	// Crc16校验
			if( (uint8)((crc16)&0xFF) == rxDataFrmPtr[len+dataLen-3] &&
				(uint8)((crc16>>8)&0xFF) == rxDataFrmPtr[len+dataLen-2]){
				txDataFrmPtr->DataBuf[0] = OP_Succeed;
				txDataFrmPtr->DataBuf[1] = rxDataFrmPtr[len + 11];//模块频段，2B (433 MHz)，2F (470MHz)，56 (868MHz)，5B (915MHz)
				txDataFrmPtr->DataBuf[2] = rxDataFrmPtr[len + 13];//信道号，例如0F表示该模块的信道为15
				txDataFrmPtr->DataLen = 3;
			}else{
				txDataFrmPtr->DataBuf[0] = OP_Failure;
				txDataFrmPtr->DataLen = 1;
			}
		}
        OSMemPut(LargeMemoryPtr, rxDataFrmPtr);
    }
	DataHandle_ResetHostProc();//复位主机模块

    // 创建应答数据包
    txDataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, ACK_PKG, UP_DIR);
    DataHandle_SetPkgPath(txDataFrmPtr, REVERSED);
    DataHandle_CreateTxData(txDataFrmPtr);

    // 销毁本任务,此处必须先禁止任务调度,否则无法释放本任务占用的内存空间
    OSMboxDel(taskPtr->Mbox, OS_DEL_ALWAYS, &err);
    OSSchedLock();
    OSTaskDel(OS_PRIO_SELF);
    OSMemPut(LargeMemoryPtr, taskPtr->StkPtr);
    taskPtr->StkPtr = (void *)0;
    TaskRunStatus.DataForward = FALSE;
    OSSchedUnlock();
}

/************************************************************************************************
* Function Name: DataHandle_ReadHostChannelProc
* Decription   : 读取主机模块的信道
* Input        : DataFrmPtr-接收到的数据指针
* Output       : 是否需要后续处理
* Others       : 下行:空
*                上行:
************************************************************************************************/
bool DataHandle_ReadHostChannelProc(DATA_FRAME_STRUCT *DataFrmPtr)
{
    uint8 err;
    DATA_HANDLE_TASK *taskPtr;

    // 节点不存在集中器档案中或有任务在处理中
    if (TRUE == TaskRunStatus.DataForward) {
        if (NEED_ACK == DataFrmPtr->PkgProp.NeedAck) {
            DataFrmPtr->DataBuf[0] = OP_Failure;
            DataFrmPtr->DataLen = 1;
            return NEED_ACK;
        }
        OSMemPut(LargeMemoryPtr, DataFrmPtr);
        return NONE_ACK;
    }

    PORT_BUF_FORMAT *txPortBufPtr = NULL;

    // 先申请一个内存用于中间数据处理
    if ((void *)0 == (txPortBufPtr = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        OSMemPut(LargeMemoryPtr, DataFrmPtr);
        return ERROR;
    }
    txPortBufPtr->Property.PortNo = Usart_Rf;
    txPortBufPtr->Property.FilterDone = 1;
	txPortBufPtr->Length = 0;

    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x55;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0xAA;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x0C;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x05;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x00;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x01;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x00;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x10;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x0E;
    uint16 crc16 = CalCrc16((uint8 *)(&txPortBufPtr->Buffer[0]), txPortBufPtr->Length);     // Crc16校验
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)((crc16)&0xFF);
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)((crc16 >> 8)&0xFF);
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = TAILBYTE;

    // 如果需要应答,则创建应答任务
    if (NEED_ACK == DataFrmPtr->PkgProp.NeedAck) {
        if ((void *)0 == (taskPtr = DataHandle_GetEmptyTaskPtr())) {
            goto DATA_FORWARD_FAILED;
        }
        if ((void *)0 == (taskPtr->StkPtr = (OS_STK *)OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
            goto DATA_FORWARD_FAILED;
        }
        taskPtr->PkgSn = txPortBufPtr->Buffer[2];
        taskPtr->Command = txPortBufPtr->Buffer[3];
        taskPtr->PortNo = Usart_Rf;
        taskPtr->Mbox = OSMboxCreate((void *)0);
        taskPtr->Msg = (uint8 *)DataFrmPtr;
        if (OS_ERR_NONE != OSTaskCreate(DataHandle_ReadHostChannelTask, taskPtr, taskPtr->StkPtr + MEM_LARGE_BLOCK_LEN / sizeof(OS_STK) - 1, taskPtr->Prio)) {
            OSMemPut(LargeMemoryPtr, taskPtr->StkPtr);
            taskPtr->StkPtr = (void *)0;
            OSMboxDel(taskPtr->Mbox, OS_DEL_ALWAYS, &err);
            goto DATA_FORWARD_FAILED;
        }
    }

	if (txPortBufPtr->Property.PortNo < Port_Total &&
		OS_ERR_NONE != OSMboxPost(SerialPort.Port[txPortBufPtr->Property.PortNo].MboxTx, txPortBufPtr)) {
		OSMemPut(LargeMemoryPtr, txPortBufPtr);
		goto DATA_FORWARD_FAILED;
	} else {
		OSFlagPost(GlobalEventFlag, (OS_FLAGS)(1 << txPortBufPtr->Property.PortNo + SERIALPORT_TX_FLAG_OFFSET), OS_FLAG_SET, &err);
	}

    return NONE_ACK;

DATA_FORWARD_FAILED:
    if (NEED_ACK == DataFrmPtr->PkgProp.NeedAck) {
        DataFrmPtr->DataBuf[0] = OP_Failure;
        DataFrmPtr->DataLen = 1;
        return NEED_ACK;
    }
    return NONE_ACK;
}

/************************************************************************************************
* Function Name: DataHandle_WriteHostChannelTask
* Decription   : 写主机模块信道处理任务
* Input        : p_arg-保存原来数据的指针
* Output       : 无
* Others       : 该函数用处理PC,串口,手持机发送数据后的等待任务
************************************************************************************************/
void DataHandle_WriteHostChannelTask(void *p_arg)
{
    uint8 err;
    DATA_HANDLE_TASK *taskPtr;
    DATA_FRAME_STRUCT *txDataFrmPtr;
	uint8 *rxDataFrmPtr;
	uint8 len = 0, dataLen;
	uint16 crc16;

    taskPtr = (DATA_HANDLE_TASK *)p_arg;
    txDataFrmPtr = (DATA_FRAME_STRUCT *)(taskPtr->Msg);

    TaskRunStatus.DataForward = TRUE;
    rxDataFrmPtr = OSMboxPend(taskPtr->Mbox, TIME_DELAY_MS(DELAYTIME_ONE_LAYER), &err);
    if ((void *)0 == rxDataFrmPtr) {
        txDataFrmPtr->DataBuf[0] = OP_Failure;
        txDataFrmPtr->DataLen = 1;
    } else {
    	len = 3;
		if(0x55 == rxDataFrmPtr[len] && 0xAA == rxDataFrmPtr[len+1]){
			dataLen = rxDataFrmPtr[len + 2];
			crc16 = CalCrc16((uint8 *)(&rxDataFrmPtr[len]), dataLen-3); 	// Crc16校验
			if( (uint8)((crc16)&0xFF) == rxDataFrmPtr[len+dataLen-3] &&
				(uint8)((crc16>>8)&0xFF) == rxDataFrmPtr[len+dataLen-2]){
				txDataFrmPtr->DataBuf[0] = OP_Succeed;
				txDataFrmPtr->DataLen = 1;
			}else{
				txDataFrmPtr->DataBuf[0] = OP_Failure;
				txDataFrmPtr->DataLen = 1;
			}
		}
        OSMemPut(LargeMemoryPtr, rxDataFrmPtr);
    }
	DataHandle_ResetHostProc();//复位主机模块

    // 创建应答数据包
    txDataFrmPtr->PkgProp = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, ACK_PKG, UP_DIR);
    DataHandle_SetPkgPath(txDataFrmPtr, REVERSED);
    DataHandle_CreateTxData(txDataFrmPtr);

    // 销毁本任务,此处必须先禁止任务调度,否则无法释放本任务占用的内存空间
    OSMboxDel(taskPtr->Mbox, OS_DEL_ALWAYS, &err);
    OSSchedLock();
    OSTaskDel(OS_PRIO_SELF);
    OSMemPut(LargeMemoryPtr, taskPtr->StkPtr);
    taskPtr->StkPtr = (void *)0;
    TaskRunStatus.DataForward = FALSE;
    OSSchedUnlock();
}

/************************************************************************************************
* Function Name: DataHandle_WriteHostChannelProc
* Decription   : 写主机模块的信道
* Input        : DataFrmPtr-接收到的数据指针
* Output       : 是否需要后续处理
* Others       : 下行:空
*                上行:
************************************************************************************************/
bool DataHandle_WriteHostChannelProc(DATA_FRAME_STRUCT *DataFrmPtr)
{
    uint8 err;
    DATA_HANDLE_TASK *taskPtr;

    // 节点不存在集中器档案中或有任务在处理中
    if (TRUE == TaskRunStatus.DataForward) {
        if (NEED_ACK == DataFrmPtr->PkgProp.NeedAck) {
            DataFrmPtr->DataBuf[0] = OP_Failure;
            DataFrmPtr->DataLen = 1;
            return NEED_ACK;
        }
        OSMemPut(LargeMemoryPtr, DataFrmPtr);
        return NONE_ACK;
    }

    PORT_BUF_FORMAT *txPortBufPtr = NULL;

    // 先申请一个内存用于中间数据处理
    if ((void *)0 == (txPortBufPtr = OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
        OSMemPut(LargeMemoryPtr, DataFrmPtr);
        return ERROR;
    }
    txPortBufPtr->Property.PortNo = Usart_Rf;
    txPortBufPtr->Property.FilterDone = 1;
	txPortBufPtr->Length = 0;

    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x55;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0xAA;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x0E;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x05;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x01;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x01;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x06;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x10;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x02;
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = 0x00;
	txPortBufPtr->Buffer[txPortBufPtr->Length++] = DataFrmPtr->DataBuf[0];
    uint16 crc16 = CalCrc16((uint8 *)(&txPortBufPtr->Buffer[0]), txPortBufPtr->Length);     // Crc16校验
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)((crc16)&0xFF);
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = (uint8)((crc16 >> 8)&0xFF);
    txPortBufPtr->Buffer[txPortBufPtr->Length++] = TAILBYTE;

    // 如果需要应答,则创建应答任务
    if (NEED_ACK == DataFrmPtr->PkgProp.NeedAck) {
        if ((void *)0 == (taskPtr = DataHandle_GetEmptyTaskPtr())) {
            goto DATA_FORWARD_FAILED;
        }
        if ((void *)0 == (taskPtr->StkPtr = (OS_STK *)OSMemGetOpt(LargeMemoryPtr, 10, TIME_DELAY_MS(50)))) {
            goto DATA_FORWARD_FAILED;
        }
        taskPtr->PkgSn = txPortBufPtr->Buffer[2];
        taskPtr->Command = txPortBufPtr->Buffer[3];
        taskPtr->PortNo = Usart_Rf;
        taskPtr->Mbox = OSMboxCreate((void *)0);
        taskPtr->Msg = (uint8 *)DataFrmPtr;
        if (OS_ERR_NONE != OSTaskCreate(DataHandle_WriteHostChannelTask, taskPtr, taskPtr->StkPtr + MEM_LARGE_BLOCK_LEN / sizeof(OS_STK) - 1, taskPtr->Prio)) {
            OSMemPut(LargeMemoryPtr, taskPtr->StkPtr);
            taskPtr->StkPtr = (void *)0;
            OSMboxDel(taskPtr->Mbox, OS_DEL_ALWAYS, &err);
            goto DATA_FORWARD_FAILED;
        }
    }

	if (txPortBufPtr->Property.PortNo < Port_Total &&
		OS_ERR_NONE != OSMboxPost(SerialPort.Port[txPortBufPtr->Property.PortNo].MboxTx, txPortBufPtr)) {
		OSMemPut(LargeMemoryPtr, txPortBufPtr);
		goto DATA_FORWARD_FAILED;
	} else {
		OSFlagPost(GlobalEventFlag, (OS_FLAGS)(1 << txPortBufPtr->Property.PortNo + SERIALPORT_TX_FLAG_OFFSET), OS_FLAG_SET, &err);
	}


    return NONE_ACK;

DATA_FORWARD_FAILED:
    if (NEED_ACK == DataFrmPtr->PkgProp.NeedAck) {
        DataFrmPtr->DataBuf[0] = OP_Failure;
        DataFrmPtr->DataLen = 1;
        return NEED_ACK;
    }
    return NONE_ACK;
}


/************************************************************************************************
* Function Name: DataHandle_RxCmdProc
* Decription   : 数据处理任务,只处理接收到的命令事件
* Input        : DataFrmPtr-数据帧的指针
* Output       : 无
* Others       : 该函数处理来自表端或服务器或PC机或手持机发送过来的指令并根据指令来应答
************************************************************************************************/
void DataHandle_RxCmdProc(DATA_FRAME_STRUCT *DataFrmPtr)
{
    bool postHandle, reversePath;
    PKG_PROPERTY ackPkgProperty;

    postHandle = DataFrmPtr->PkgProp.NeedAck;
    reversePath = REVERSED;
    ackPkgProperty = DataHandle_SetPkgProperty(XOR_OFF, NONE_ACK, ACK_PKG, UP_DIR);
    switch (DataFrmPtr->Command) {

        // 读集中器版本信息 0x40
        case Read_CONC_Version:
            // 下行:空数据域
            // 上行:程序版本(2)+硬件版本(2)+协议版本(2)
            DataFrmPtr->DataLen = 0;
            DataFrmPtr->DataBuf[DataFrmPtr->DataLen++] = (uint8)(SW_VERSION >> 8);
            DataFrmPtr->DataBuf[DataFrmPtr->DataLen++] = (uint8)SW_VERSION;
            DataFrmPtr->DataBuf[DataFrmPtr->DataLen++] = (uint8)(HW_VERSION >> 8);
            DataFrmPtr->DataBuf[DataFrmPtr->DataLen++] = (uint8)HW_VERSION;
            DataFrmPtr->DataBuf[DataFrmPtr->DataLen++] = (uint8)(PT_VERSION >> 8);
            DataFrmPtr->DataBuf[DataFrmPtr->DataLen++] = (uint8)PT_VERSION;
            break;

        // 读集中器ID 0x41
        case Read_CONC_ID:
            // 下行:空数据域
            // 上行:集中器ID的BCD码(6)
            memcpy(DataFrmPtr->DataBuf, Concentrator.LongAddr, LONG_ADDR_SIZE);
            DataFrmPtr->DataLen = LONG_ADDR_SIZE;
            break;

        // 写集中器ID 0x42
        case Write_CONC_ID:
            Data_SetConcentratorAddr(DataFrmPtr);
            break;

        // 读集中器时钟 0x43
        case Read_CONC_RTC:
            // 下行:空数据域
            // 上行:集中器时钟(7)
            Rtc_Get((RTC_TIME *)DataFrmPtr->DataBuf, Format_Bcd);
            DataFrmPtr->DataLen = 7;
            break;

        // 写集中器时钟 0x44
        case Write_CONC_RTC:
            // 下行:集中器时钟(7)
            // 上行:操作状态(1)
            if (SUCCESS == Rtc_Set(*(RTC_TIME *)(DataFrmPtr->DataBuf), Format_Bcd)) {
                DataFrmPtr->DataBuf[0] = OP_Succeed;
                RTCTimingTimer = RTCTIMING_INTERVAL_TIME;
            } else {
                DataFrmPtr->DataBuf[0] = OP_TimeAbnormal;
            }
            DataFrmPtr->DataLen = 1;
            break;

        // 读Gprs参数 0x45
        case Read_GPRS_Param:
            Data_GprsParameter(DataFrmPtr);
            break;

        // 写Gprs参数 0x46
        case Write_GPRS_Param:
            Data_GprsParameter(DataFrmPtr);
            break;

        // 读Gprs信号强度 0x47
        case Read_GPRS_RSSI:
            // 下行:无
            // 上行:信号强度
            DataFrmPtr->DataBuf[0] = Gprs_GetCSQ();
            DataFrmPtr->DataBuf[1] = Gprs.Online ? 0x01 : 0x00;
            DataFrmPtr->DataLen = 2;
            DataFrmPtr->DataLen += Gprs_GetIMSI(&DataFrmPtr->DataBuf[DataFrmPtr->DataLen]);
            DataFrmPtr->DataLen += Gprs_GetGMM(&DataFrmPtr->DataBuf[DataFrmPtr->DataLen]);
            break;

        // 集中器初始化 0x48
        case Initial_CONC_Cmd:
            // 下行:操作类别
            // 上行:操作类别+操作状态
            DataFrmPtr->DataBuf[1] = OP_Succeed;
            if (0 == DataFrmPtr->DataBuf[0]) {
                Data_ClearDatabase();
            } else if(1 == DataFrmPtr->DataBuf[0]) {
                Data_ClearMeterData();
            } else {
                DataFrmPtr->DataBuf[1] = OP_Failure;
            }
            DataFrmPtr->DataLen = 2;
            break;

        // 集中器重新启动 0x4C
        case Restart_CONC_Cmd:
            // 下行:无
            // 上行:操作状态
            DataFrmPtr->DataBuf[0] = OP_Succeed;
            DataFrmPtr->DataLen = 1;
            DevResetTimer = 5000;
            break;

        // 表具档案的总数量 0x50
        case Read_Meter_Total_Number:
            Data_ReadNodesCount(DataFrmPtr);
            break;

        // 读取表具档案信息 0x51
        case Read_Meters_Doc_Info:
            Data_ReadNodes(DataFrmPtr);
            break;

        // 写入表具档案信息 0x52
        case Write_Meters_Doc_Info:
            Data_WriteNodes(DataFrmPtr);
            break;

        // 删除表具档案信息 0x53
        case Delete_Meters_Doc_Info:
            Data_DeleteNodes(DataFrmPtr);
            break;

        // 修改表具档案信息 0x54
        case Modify_Meter_Doc_Info:
            Data_ModifyNodes(DataFrmPtr);
            break;

	    // 地锁状态 0x72
        case Lock_Status_Report_Cmd:
            postHandle = DataHandle_LockStatusReportProc(DataFrmPtr);
            break;

        // 服务器下发地锁开关命令 0x73
        case Lock_Status_Issued_Cmd:
            postHandle = DataHandle_LockStatusIssuedProc(DataFrmPtr);
            break;

        // 服务器下发地锁更新密钥命令 0x74
        case Lock_Key_Updata_Cmd:
            postHandle = DataHandle_LockKeyUpdataProc(DataFrmPtr);
            break;

        // 地锁传感器状态信息，上传服务器 0x75
        case Lock_Sensor_Status_Cmd:
            postHandle = DataHandle_LockSensorStatusProc(DataFrmPtr);
            break;

        // 获取集中器所保存的最新一条地锁状态信息 0x77
        case Read_Lock_Data_Cmd:
            DataHandle_ReadLockDataProc(DataFrmPtr);
            break;

        // 批量获取集中器所保存的最新一条地锁状态信息 0x78
        case Batch_Read_Lock_Data_Cmd:
            DataHandle_BatchReadLockDataProc(DataFrmPtr);
            break;

		// 读集中器的工作参数 0x79
		case Lock_Read_CONC_Work_Param:
			Data_RdWrConcentratorParam(DataFrmPtr);
			break;

		// 写集中器的工作参数 0x7A
		case Lock_Write_CONC_Work_Param:
			Data_RdWrConcentratorParam(DataFrmPtr);
			break;

		// 读主机工作信道 0x7B
		case Lock_Read_Host_Channel_Param:
            postHandle = DataHandle_ReadHostChannelProc(DataFrmPtr);
			break;

		// 写主机工作信道 0x7C
		case Lock_Write_Host_Channel_Param:
            postHandle = DataHandle_WriteHostChannelProc(DataFrmPtr);
			break;

        // 集中器程序升级 0xF1
        case Software_Update_Cmd:
            Data_SwUpdate(DataFrmPtr);
            break;

        // Eeprom检查 0xF3
        case Eeprom_Check_Cmd:
            Data_EepromCheckProc(DataFrmPtr);
            break;

		// 无线模块升级 0xF4
		case Module_Software_Update_Cmd:
			Data_Module_SwUpdate(DataFrmPtr);
			break;

        // 其他指令不支持
        default:
#if PRINT_INFO
            Gprs_OutputDebugMsg(TRUE, "--该指令暂不支持--\n");
#endif
            postHandle = NONE_ACK;
            OSMemPut(LargeMemoryPtr, DataFrmPtr);
            break;
    }

    if (NEED_ACK == postHandle) {
        DataFrmPtr->PkgProp = ackPkgProperty;
        DataFrmPtr->DeviceType = Dev_Concentrator;
        DataHandle_SetPkgPath(DataFrmPtr, reversePath);
        DataHandle_CreateTxData(DataFrmPtr);
    }
}

/************************************************************************************************
* Function Name: DataHandle_RxAckProc
* Decription   : 数据处理任务,只处理接收到的应答事件
* Input        : DataBufPtr-命令数据指针
* Output       : 无
* Others       : 该函数处理来自表端或服务器或PC机或手持机发送过来的应答
************************************************************************************************/
void DataHandle_RxAckProc(DATA_FRAME_STRUCT *DataFrmPtr)
{
    uint8 i;
    uint16 nodeId;
    DATA_HANDLE_TASK *taskPtr = NULL;

    // 查找应答节点是否在本档案中
    nodeId = Data_FindNodeId(0, DataFrmPtr->Route[0]);

    // 判断该应答帧应该传递给谁
    for (i = 0; i < MAX_DATA_HANDLE_TASK_NUM; i++) {
        taskPtr = &DataHandle_TaskArry[i];
        if ((void *)0 != taskPtr->StkPtr &&
            taskPtr->NodeId == nodeId &&
            taskPtr->Command == DataFrmPtr->Command &&
            taskPtr->PkgSn == DataFrmPtr->PkgSn) {
            if (OS_ERR_NONE != OSMboxPost(taskPtr->Mbox, DataFrmPtr)) {
                OSMemPut(LargeMemoryPtr, DataFrmPtr);
            }
            return;
        }
    }
    OSMemPut(LargeMemoryPtr, DataFrmPtr);
}

/************************************************************************************************
* Function Name: DataHandle_PassProc
* Decription   : 透传处理任务
* Input        : DataFrmPtr-接收到的数据指针
* Output       : TRUE-已经处理,FALSE-没有处理
* Others       : 目标地址不是自己时,传递到下一个节点
************************************************************************************************/
bool DataHandle_PassProc(DATA_FRAME_STRUCT *DataFrmPtr)
{
    static uint8 lastInPort = End_Port;

    // 如果是目标节点则跳至下一步处理
    if (DataFrmPtr->RouteInfo.CurPos == DataFrmPtr->RouteInfo.Level - 1) {
        return FALSE;
    }
    // 如果是上行且是倒数第二级则按照设备类型选择通讯端口,其他情况都用RF端口
    if (UP_DIR == DataFrmPtr->PkgProp.Direction &&
        DataFrmPtr->RouteInfo.CurPos == DataFrmPtr->RouteInfo.Level - 2) {
        DataFrmPtr->PortNo = lastInPort;
        DataFrmPtr->Life_Ack.AckChannel = DEFAULT_TX_CHANNEL;
    } else {
        lastInPort = DataFrmPtr->PortNo;
        DataFrmPtr->Life_Ack.AckChannel = DEFAULT_RX_CHANNEL;
        DataFrmPtr->PortNo = Usart_Rf;
    }
    DataHandle_CreateTxData(DataFrmPtr);
    return TRUE;
}

/************************************************************************************************
* Function Name: DataHandle_WriteNodes
* Decription   : 数据库写节点信息
* Input        : DataFrmPtr-完整的一包定时定量数据
************************************************************************************************/
uint8 DataHandle_WriteNodes(DATA_FRAME_STRUCT *DataFrmPtr)
{
    uint8 err;
    uint16 i;

    // 向节点列表中增加节点
    if (NULL_U16_ID != Data_FindNodeId(0, DataFrmPtr->Route[0])) {
        return 0;
    } else {
        if (Concentrator.MaxNodeId >= MAX_NODE_NUM) {
            return 0;
        } else {
            // 寻找空位置
            for (i = 0; i < Concentrator.MaxNodeId; i++) {
                if (0 == memcmp(SubNodes[i].LongAddr, NullAddress, LONG_ADDR_SIZE)) {
                    break;
                }
            }
            if (i >= Concentrator.MaxNodeId) {
                Concentrator.MaxNodeId++;
            }
            memcpy(SubNodes[i].LongAddr, DataFrmPtr->Route[0], LONG_ADDR_SIZE);
            SubNodes[i].DevType = (DEVICE_TYPE)DataFrmPtr->DeviceType;
            SubNodes[i].Property.LastResult = 2;
            SubNodes[i].Property.UploadData = TRUE;
            SubNodes[i].Property.UploadPrio = LOW;
            SubNodes[i].RxLastDataNum = 0;
            SubNodes[i].RxChannel = DEFAULT_TX_CHANNEL;
        }
    }

    // 延时后保存,必须延时同样的时间,否则易造成数据不一致的情况
    OSFlagPost(GlobalEventFlag, (OS_FLAGS)FLAG_DELAY_SAVE_TIMER, OS_FLAG_SET, &err);

    return 1;
}

/************************************************************************************************
* Function Name: DataHandle_Task
* Decription   : 数据处理任务,只处理接收事件
* Input        : *p_arg-参数指针
* Output       : 无
* Others       : 无
************************************************************************************************/
void DataHandle_Task(void *p_arg)
{
    uint8 i, err, *dat = NULL;
    OS_FLAGS eventFlag;
    DATA_FRAME_STRUCT *dataFrmPtr = NULL;
    EXTRACT_DATA_RESULT ret;
	DATA_HANDLE_TASK *taskPtr = NULL;

    // 初始化参数
    (void)p_arg;
    PkgNo = CalCrc8(Concentrator.LongAddr, LONG_ADDR_SIZE);
    for (i = 0; i < MAX_DATA_HANDLE_TASK_NUM; i++) {
        DataHandle_TaskArry[i].Prio = TASK_DATAHANDLE_DATA_PRIO + i;
        DataHandle_TaskArry[i].StkPtr = (void *)0;
    }
    TaskRunStatus.DataForward = FALSE;
    TaskRunStatus.DataReplenish = FALSE;
    TaskRunStatus.DataUpload = FALSE;
    TaskRunStatus.RTCService = FALSE;
    TaskRunStatus.RTCTiming = FALSE;

    // 数据初始化
    Data_Init();
    if(Concentrator.Param.DataNodeSave > 0x1){
        Concentrator.Param.DataNodeSave = 0x1;
    }

    while (TRUE) {
        // 获取发生的事件数据
        eventFlag = OSFlagPend(GlobalEventFlag, (OS_FLAGS)DATAHANDLE_EVENT_FILTER, (OS_FLAG_WAIT_SET_ANY | OS_FLAG_CONSUME), TIME_DELAY_MS(5000), &err);

        // 处理这些数据
        while (eventFlag != (OS_FLAGS)0) {
            dat = (void *)0;
            if (eventFlag & FLAG_USART_RF_RX) {
                // Rf模块收到了数据
                dat = OSMboxAccept(SerialPort.Port[Usart_Rf].MboxRx);
                eventFlag &= ~FLAG_USART_RF_RX;
            } else if (eventFlag & FLAG_GPRS_RX) {
                // Gprs模块收到了数据
                dat = OSMboxAccept(Gprs.MboxRx);
                eventFlag &= ~FLAG_GPRS_RX;
            } else if (eventFlag & FLAG_USB_RX) {
                // Usb端口收到了数据
                dat = OSMboxAccept(SerialPort.Port[Usb_Port].MboxRx);
                eventFlag &= ~FLAG_USB_RX;
            } else if (eventFlag & FLAG_USART_DEBUG_RX) {
                // Debug端口收到了数据
                dat = OSMboxAccept(SerialPort.Port[Usart_Debug].MboxRx);
                eventFlag &= ~FLAG_USART_DEBUG_RX;
            } else if (eventFlag & FLAG_UART_RS485_RX) {
                // 485端口收到了数据
                dat = OSMboxAccept(SerialPort.Port[Uart_Rs485].MboxRx);
                eventFlag &= ~FLAG_UART_RS485_RX;
            } else if (eventFlag & FLAG_USART_IR_RX) {
                // Ir端口收到了数据
                dat = OSMboxAccept(SerialPort.Port[Usart_Ir].MboxRx);
                eventFlag &= ~FLAG_USART_IR_RX;
            } else if (eventFlag & FLAG_DELAY_SAVE_TIMER) {
                // 数据延时保存
                eventFlag &= ~FLAG_DELAY_SAVE_TIMER;
                DataHandle_DataDelaySaveProc();
            } else if (eventFlag & FLAG_DATA_DOWNLOAD_TIMER) {
                // 数据下发处理
                eventFlag &= ~FLAG_DATA_DOWNLOAD_TIMER;
                DataHandle_DataDownloadProc();
            } else if (eventFlag & FLAG_DATA_UPLOAD_TIMER) {
                // 数据上传处理
                eventFlag &= ~FLAG_DATA_UPLOAD_TIMER;
                DataHandle_DataUploadProc();
            } else if (eventFlag & FLAG_DATA_UPHOST_TIMER) {
                // 升级主机处理
                eventFlag &= ~FLAG_DATA_UPHOST_TIMER;
                DataHandle_Updata_HostProc();
            } else if (eventFlag & FLAG_RTC_TIMING_TIMER) {
                // 时钟主动校时处理
                eventFlag &= ~FLAG_RTC_TIMING_TIMER;
                DataHandle_RTCTimingProc();
            }
            if ((void *)0 == dat) {
                continue;
            }

            // 从原数据中提取数据
            if(Ok_Data != DataHandle_ExtractData(dat)){
                if(Error_DstAddress == ret){

                }

				// 用于主机模块的信道频率读取和设置
				if( 0x55 == dat[3] && 0xAA == dat[4] ){
					// 判断该应答帧应该传递给谁
					for (i = 0; i < MAX_DATA_HANDLE_TASK_NUM; i++) {
						taskPtr = &DataHandle_TaskArry[i];
						if ((void *)0 != taskPtr->StkPtr &&	taskPtr->Command == dat[6] &&
							(taskPtr->PkgSn == dat[5] || taskPtr->PkgSn+0xB == dat[5])) {
							if (OS_ERR_NONE != OSMboxPost(taskPtr->Mbox, dat)) {
								OSMemPut(LargeMemoryPtr, dat);
							}
							break;
						}
					}
					continue;
				}else{
					OSMemPut(LargeMemoryPtr, dat);
					continue;
				}
            }

            dataFrmPtr = (DATA_FRAME_STRUCT *)dat;

            if( Concentrator.Param.DataNodeSave ){
                // 收到正确的表具数据后，如果集中器中没有档案则直接保存档案信息和表信息到集中器中
                DataHandle_WriteNodes(dataFrmPtr);
            } else {
              // 如果节点不在档案中，集中的工作模式为"手动设置节点到档案"
              // 对于地锁的 0x72 0x75 命令不予回应。
                if( (NULL_U16_ID == Data_FindNodeId(0, dataFrmPtr->Route[0])) &&
                    (Lock_Status_Report_Cmd == dataFrmPtr->Command ||
                     Lock_Sensor_Status_Cmd == dataFrmPtr->Command) ){
                    OSMemPut(LargeMemoryPtr, dat);
                    continue;
                }
            }

            // 确定监控信息上传的通道
            if (Usart_Debug == dataFrmPtr->PortNo || Usb_Port == dataFrmPtr->PortNo) {
                MonitorPort = (PORT_NO)(dataFrmPtr->PortNo);
            }

            // 如果目标地址不是自己则转发
            if (TRUE == DataHandle_PassProc(dataFrmPtr)) {
                continue;
            }

            // 分别处理命令帧和应答帧指令
            if (CMD_PKG == dataFrmPtr->PkgProp.PkgType) {
                // 如果是命令帧
                DataHandle_RxCmdProc(dataFrmPtr);
            } else {
                // 如果是应答帧
                DataHandle_RxAckProc(dataFrmPtr);
            }
        }

        OSTimeDlyHMSM(0, 0, 0, 50);
    }
}

/***************************************End of file*********************************************/



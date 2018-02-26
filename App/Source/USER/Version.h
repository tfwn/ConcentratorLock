/************************************************************************************************
*                                   SRWF-6009
*    (c) Copyright 2015, Software Department, Sunray Technology Co.Ltd
*                               All Rights Reserved
*
* FileName     : Version.h
* Description  :
* Version      :
* Function List:
*------------------------------Revision History--------------------------------------------------
* No.   Version     Date            Revised By      Item            Description
* 1     V1.0        10/08/2015      Zhangxp         SRWF-6009       Original Version
************************************************************************************************/

#ifndef  VERSION_H
#define  VERSION_H

/************************************************************************************************
*   版本发布列表
************************************************************************************************/
#define SRWF_CTP_20180112_V100                    0   // 第一版
#define SRWF_CTP_20180226_V101                    1

#define SRWF_CTP_TEST                    		  999   // 测试


// 当前使用的版本
#define CURRENT_VERSION                             SRWF_CTP_20180226_V101


/************************************************************************************************
*   版本详细描述信息
************************************************************************************************/
#if (CURRENT_VERSION == SRWF_CTP_20180112_V100)
/*-----------------------------------------------------------------------------------------------
软件说明:
1.第一版程序
-----------------------------------------------------------------------------------------------*/
// 条件编译选项:
#define EEPROM_SEL_M01                                      // 选择1M的EEPROM EEPROM_SEL_M512/EEPROM_SEL_M01
/*---------------------------------------------------------------------------------------------*/
// 版本信息
#define SW_VERSION                              0x0100      // 程序版本
#define HW_VERSION                              0x0200      // 硬件版本
#define PT_VERSION                              0x0101      // 协议版本
#define SOFTWARE_VERSION_INFO                   "SRWF-CTP-PARKING-20180112-Vsp1.00"
#endif

#if (CURRENT_VERSION == SRWF_CTP_20180116_V101)
/*-----------------------------------------------------------------------------------------------
软件说明:
1.增加集中器修改主机模块(2e28)频点功能。
2.修改集中器应答方式。20180201
-----------------------------------------------------------------------------------------------*/
// 条件编译选项:
#define EEPROM_SEL_M01                                      // 选择1M的EEPROM EEPROM_SEL_M512/EEPROM_SEL_M01
/*---------------------------------------------------------------------------------------------*/
// 版本信息
#define SW_VERSION                              0x0101      // 程序版本
#define HW_VERSION                              0x0200      // 硬件版本
#define PT_VERSION                              0x0101      // 协议版本
#define SOFTWARE_VERSION_INFO                   "SRWF-CTP-PARKING-20180116-Vsp1.01"
#endif

#if (CURRENT_VERSION == SRWF_CTP_20180226_V101)
/*-----------------------------------------------------------------------------------------------
软件说明:
1.增加集中器修改主机模块(2e28)频点功能。
2.修改集中器应答方式。
3.增加集中器升级主机模块(2e28)功能，需要先用上位机将程序下载到集中器flash中（服务器下发主机程序到集中器部分暂时未做）。
-----------------------------------------------------------------------------------------------*/
// 条件编译选项:
#define EEPROM_SEL_M01                                      // 选择1M的EEPROM EEPROM_SEL_M512/EEPROM_SEL_M01
/*---------------------------------------------------------------------------------------------*/
// 版本信息
#define SW_VERSION                              0x0101      // 程序版本
#define HW_VERSION                              0x0200      // 硬件版本
#define PT_VERSION                              0x0101      // 协议版本
#define SOFTWARE_VERSION_INFO                   "SRWF-CTP-PARKING-20180226-Vsp1.01"
#endif


#if (CURRENT_VERSION == SRWF_CTP_TEST)
/*-----------------------------------------------------------------------------------------------
软件说明:
1.数据测试，对于地锁的命令不予应答，只接收。
-----------------------------------------------------------------------------------------------*/
// 条件编译选项:
#define EEPROM_SEL_M01                                      // 选择1M的EEPROM EEPROM_SEL_M512/EEPROM_SEL_M01
/*---------------------------------------------------------------------------------------------*/
// 版本信息
#define SW_VERSION                              0x0999      // 程序版本
#define HW_VERSION                              0x0200      // 硬件版本
#define PT_VERSION                              0x0101      // 协议版本
#define SOFTWARE_VERSION_INFO                   "SRWF-CTP-PARKING-TEST-Vsp9.99"
#endif


#endif
/**************************************End of file**********************************************/


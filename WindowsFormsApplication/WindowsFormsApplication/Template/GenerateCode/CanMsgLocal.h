/*  (C) Copyright, JWPENG. Time:2025/10/20 10:25:50******************************/
#ifndef _CAMMSG_LOCAL_H_
#define _CAMMSG_LOCAL_H_
/*  Includes ********************************************************************/
#include "SysTypes.h"
/*  Enum ************************************************************************/
enum CAN_CLIENT_SIGNAL_DCDC0C4
{
    DCDC_DCDC2_OutputCurrent            = 0UL,
    DCDC_DCDC2_InputCurrent             = 1UL,
    DCDC_DCDC2_OutputVoltage            = 2UL,
    DCDC_DCDC2_InputVolt                = 3UL,
    DCDC_RollingCounterC4               = 4UL,
    DCDC_ChecksumC4                     = 5UL,
};

enum CAN_CLIENT_SIGNAL_EXECU30A
{
    EXECU_OTA_ModeSt                    = 0UL,
    EXECU_OTA_EstimatedUpgradeTime      = 1UL,
    EXECU_RollingCounter30A             = 2UL,
    EXECU_Checksum30A                   = 3UL,
};

enum CAN_CLIENT_SIGNAL_EXECU340
{
    EXECU_BCM_BattVolt_EBS2             = 0UL,
    EXECU_BCM_12VChrgReq                = 1UL,
};

enum CAN_CLIENT_SIGNAL_DCDC344
{
    DCDC_DCDC2_Temperature              = 0UL,
    DCDC_DCDC2_Fault                    = 1UL,
    DCDC_DCDC2_Efficiency               = 2UL,
    DCDC_DCDC2_HighVoltCnctAllowed      = 3UL,
    DCDC_DCDC2_State                    = 4UL,
    DCDC_DCDC2_OverTempSt               = 5UL,
    DCDC_RollingCounter344              = 6UL,
    DCDC_Checksum344                    = 7UL,
};

enum CAN_CLIENT_SIGNAL_EXECU372
{
    EXECU_PDCU_DCDC2VolReq              = 0UL,
    EXECU_PDCU_DCDC2Enable              = 1UL,
};

enum CAN_CLIENT_SIGNAL_DCDC59C
{
    DCDC_NMNode                         = 0UL,
    DCDC_ActiveWakeupBit                = 1UL,
    DCDC_RepeatMessageRequestBit        = 2UL,
    DCDC_RepeatSts                      = 3UL,
    DCDC_NMWakeupStype                  = 4UL,
    DCDC_NMWakeupPower                  = 5UL,
    DCDC_NMWakeupSource                 = 6UL,
};

/*  @brief The 0x0c4 Signal sent by DCDC*****************************************/
typedef struct tagDCDC_0C4_TX_SIG
{
    u16 u16DCDC2_OutputCurrent;
    u08 u08DCDC2_InputCurrent;
    u08 u08DCDC2_OutputVoltage;
    u16 u16DCDC2_InputVolt;
    u08 u08RollingCounterC4;
    u08 u08ChecksumC4;
}DCDC_0C4_TX_SIG;

/*  @brief The 0x30a Signal sent by EXECU****************************************/
typedef struct tagEXECU_30A_RX_SIG
{
    u08 u08OTA_ModeSt;
    u08 u08OTA_EstimatedUpgradeTime;
    u08 u08RollingCounter30A;
    u08 u08Checksum30A;
}EXECU_30A_RX_SIG;

/*  @brief The 0x340 Signal sent by EXECU****************************************/
typedef struct tagEXECU_340_RX_SIG
{
    u16 u16BCM_BattVolt_EBS2;
    u08 u08BCM_12VChrgReq;
}EXECU_340_RX_SIG;

/*  @brief The 0x344 Signal sent by DCDC*****************************************/
typedef struct tagDCDC_344_TX_SIG
{
    u08 u08DCDC2_Temperature;
    u08 u08DCDC2_Fault;
    u08 u08DCDC2_Efficiency;
    u08 u08DCDC2_HighVoltCnctAllowed;
    u08 u08DCDC2_State;
    u08 u08DCDC2_OverTempSt;
    u08 u08RollingCounter344;
    u08 u08Checksum344;
}DCDC_344_TX_SIG;

/*  @brief The 0x372 Signal sent by EXECU****************************************/
typedef struct tagEXECU_372_RX_SIG
{
    u08 u08PDCU_DCDC2VolReq;
    u08 u08PDCU_DCDC2Enable;
}EXECU_372_RX_SIG;

/*  @brief The 0x59c Signal sent by DCDC*****************************************/
typedef struct tagDCDC_59C_TX_SIG
{
    u08 u08NMNode;
    u08 u08ActiveWakeupBit;
    u08 u08RepeatMessageRequestBit;
    u08 u08RepeatSts;
    u08 u08NMWakeupStype;
    u08 u08NMWakeupPower;
    u08 u08NMWakeupSource;
}DCDC_59C_TX_SIG;

/*  Private Function ************************************************************/
u32 MsgFun_0C4(u32 ulIndex, u32 ulParam);
u32 MsgFun_30A(u32 ulIndex, u32 ulParam);
u32 MsgFun_340(u32 ulIndex, u32 ulParam);
u32 MsgFun_344(u32 ulIndex, u32 ulParam);
u32 MsgFun_372(u32 ulIndex, u32 ulParam);
u32 MsgFun_59C(u32 ulIndex, u32 ulParam);
#endif

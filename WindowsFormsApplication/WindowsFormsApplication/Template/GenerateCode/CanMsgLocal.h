/*  (C) Copyright, JWPENG. Time:2025/10/23 15:08:31******************************/
#ifndef _CAMMSG_LOCAL_H_
#define _CAMMSG_LOCAL_H_
/*  Includes ********************************************************************/
#include "SysTypes.h"
/*  Enum ************************************************************************/
enum CAN_CLIENT_SIGNAL_EXECU0A0
{
    EXECU_ESC_VehSpdValidFlag           = 0UL,
    EXECU_ESC_VehSpd                    = 1UL,
};

enum CAN_CLIENT_SIGNAL_DCDC0C4
{
    DCDC_DCDC2_OutputCurrent            = 0UL,
    DCDC_DCDC2_InputCurrent             = 1UL,
    DCDC_DCDC2_OutputVoltage            = 2UL,
    DCDC_DCDC2_InputVolt                = 3UL,
    DCDC_RollingCounterC4               = 4UL,
    DCDC_ChecksumC4                     = 5UL,
};

enum CAN_CLIENT_SIGNAL_EXECU0FE
{
    EXECU_PDCU_DriveReady               = 0UL,
};

enum CAN_CLIENT_SIGNAL_EXECU30A
{
    EXECU_OTA_ModeSt                    = 0UL,
    EXECU_OTA_EstimatedUpgradeTime      = 1UL,
    EXECU_RollingCounter30A             = 2UL,
    EXECU_Checksum30A                   = 3UL,
};

enum CAN_CLIENT_SIGNAL_EXECU320
{
    EXECU_IC_TotalOdmeter               = 0UL,
};

enum CAN_CLIENT_SIGNAL_EXECU33C
{
    EXECU_BCM_IgnitionSt                = 0UL,
};

enum CAN_CLIENT_SIGNAL_EXECU340
{
    EXECU_BCM_BattVolt_EBS2             = 0UL,
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

enum CAN_CLIENT_SIGNAL_EXECU4DA
{
    EXECU_MP5_TBOX_Time_Year            = 0UL,
    EXECU_MP5_TBOX_Time_Month           = 1UL,
    EXECU_MP5_TBOX_Time_Date            = 2UL,
    EXECU_MP5_TBOX_Time_Hour            = 3UL,
    EXECU_MP5_TBOX_Time_Minute          = 4UL,
    EXECU_MP5_TBOX_Time_Second          = 5UL,
    EXECU_MP5_TBOX_Time_YearMark        = 6UL,
    EXECU_MP5_TBOX_Time_Valid           = 7UL,
};

/*  @brief The 0x0a0 Signal sent by EXECU****************************************/
typedef struct tagEXECU_0A0_RX_SIG
{
    u08 u08ESC_VehSpdValidFlag;
    u16 u16ESC_VehSpd;
}EXECU_0A0_RX_SIG;

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

/*  @brief The 0x0fe Signal sent by EXECU****************************************/
typedef struct tagEXECU_0FE_RX_SIG
{
    u08 u08PDCU_DriveReady;
}EXECU_0FE_RX_SIG;

/*  @brief The 0x30a Signal sent by EXECU****************************************/
typedef struct tagEXECU_30A_RX_SIG
{
    u08 u08OTA_ModeSt;
    u08 u08OTA_EstimatedUpgradeTime;
    u08 u08RollingCounter30A;
    u08 u08Checksum30A;
}EXECU_30A_RX_SIG;

/*  @brief The 0x320 Signal sent by EXECU****************************************/
typedef struct tagEXECU_320_RX_SIG
{
    u32 u32IC_TotalOdmeter;
}EXECU_320_RX_SIG;

/*  @brief The 0x33c Signal sent by EXECU****************************************/
typedef struct tagEXECU_33C_RX_SIG
{
    u08 u08BCM_IgnitionSt;
}EXECU_33C_RX_SIG;

/*  @brief The 0x340 Signal sent by EXECU****************************************/
typedef struct tagEXECU_340_RX_SIG
{
    u16 u16BCM_BattVolt_EBS2;
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

/*  @brief The 0x4da Signal sent by EXECU****************************************/
typedef struct tagEXECU_4DA_RX_SIG
{
    u08 u08MP5_TBOX_Time_Year;
    u08 u08MP5_TBOX_Time_Month;
    u08 u08MP5_TBOX_Time_Date;
    u08 u08MP5_TBOX_Time_Hour;
    u08 u08MP5_TBOX_Time_Minute;
    u08 u08MP5_TBOX_Time_Second;
    u08 u08MP5_TBOX_Time_YearMark;
    u08 u08MP5_TBOX_Time_Valid;
}EXECU_4DA_RX_SIG;

/*  Private Function ************************************************************/
u32 MsgFun_0A0(u32 ulIndex, u32 ulParam);
u32 MsgFun_0C4(u32 ulIndex, u32 ulParam);
u32 MsgFun_0FE(u32 ulIndex, u32 ulParam);
u32 MsgFun_30A(u32 ulIndex, u32 ulParam);
u32 MsgFun_320(u32 ulIndex, u32 ulParam);
u32 MsgFun_33C(u32 ulIndex, u32 ulParam);
u32 MsgFun_340(u32 ulIndex, u32 ulParam);
u32 MsgFun_344(u32 ulIndex, u32 ulParam);
u32 MsgFun_372(u32 ulIndex, u32 ulParam);
u32 MsgFun_4DA(u32 ulIndex, u32 ulParam);
#endif

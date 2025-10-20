/*  (C) Copyright, JWPENG. Time:2025/10/20 10:25:50******************************/
#ifndef _CAMMSG_INTERFACE_H_
#define _CAMMSG_INTERFACE_H_
/*  Includes ********************************************************************/
 #include "AppCan.h"
/*  Export function *************************************************************/
const TS_CANMSG * getCanPublicMsgObj(void);
u32 getCanPublicMsgObjSize(void);
/*  Export ENUM *****************************************************************/
enum CAN_CLIENT_ID_CanMsg
{
    CAN_CLIENT_ID_DCDC_0C4          = 0x000000C4UL,
    CAN_CLIENT_ID_EXECU_30A         = 0x0000030AUL,
    CAN_CLIENT_ID_EXECU_340         = 0x00000340UL,
    CAN_CLIENT_ID_DCDC_344          = 0x00000344UL,
    CAN_CLIENT_ID_EXECU_372         = 0x00000372UL,
    CAN_CLIENT_ID_DCDC_59C          = 0x0000059CUL,
};
/*  @brief The 0C4 Signal sent by DCDC*******************************************/
u16 APP_CAN_u16GetDCDC_DCDC2_OutputCurrent(void);
u08 APP_CAN_u08GetDCDC_DCDC2_InputCurrent(void);
u08 APP_CAN_u08GetDCDC_DCDC2_OutputVoltage(void);
u16 APP_CAN_u16GetDCDC_DCDC2_InputVolt(void);
u08 APP_CAN_u08GetDCDC_RollingCounterC4(void);
u08 APP_CAN_u08GetDCDC_ChecksumC4(void);

/*  @brief The 30A Signal sent by EXECU******************************************/
u08 APP_CAN_u08GetEXECU_OTA_ModeSt(void);
u08 APP_CAN_u08GetEXECU_OTA_EstimatedUpgradeTime(void);
u08 APP_CAN_u08GetEXECU_RollingCounter30A(void);
u08 APP_CAN_u08GetEXECU_Checksum30A(void);

/*  @brief The 340 Signal sent by EXECU******************************************/
u16 APP_CAN_u16GetEXECU_BCM_BattVolt_EBS2(void);
u08 APP_CAN_u08GetEXECU_BCM_12VChrgReq(void);

/*  @brief The 344 Signal sent by DCDC*******************************************/
u08 APP_CAN_u08GetDCDC_DCDC2_Temperature(void);
u08 APP_CAN_u08GetDCDC_DCDC2_Fault(void);
u08 APP_CAN_u08GetDCDC_DCDC2_Efficiency(void);
u08 APP_CAN_u08GetDCDC_DCDC2_HighVoltCnctAllowed(void);
u08 APP_CAN_u08GetDCDC_DCDC2_State(void);
u08 APP_CAN_u08GetDCDC_DCDC2_OverTempSt(void);
u08 APP_CAN_u08GetDCDC_RollingCounter344(void);
u08 APP_CAN_u08GetDCDC_Checksum344(void);

/*  @brief The 372 Signal sent by EXECU******************************************/
u08 APP_CAN_u08GetEXECU_PDCU_DCDC2VolReq(void);
u08 APP_CAN_u08GetEXECU_PDCU_DCDC2Enable(void);

/*  @brief The 59C Signal sent by DCDC*******************************************/
u08 APP_CAN_u08GetDCDC_NMNode(void);
u08 APP_CAN_u08GetDCDC_ActiveWakeupBit(void);
u08 APP_CAN_u08GetDCDC_RepeatMessageRequestBit(void);
u08 APP_CAN_u08GetDCDC_RepeatSts(void);
u08 APP_CAN_u08GetDCDC_NMWakeupStype(void);
u08 APP_CAN_u08GetDCDC_NMWakeupPower(void);
u08 APP_CAN_u08GetDCDC_NMWakeupSource(void);

#endif
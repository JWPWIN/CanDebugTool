/*  (C) Copyright, GoldWei.Ltd. Time:2025/10/15 1:47:59**************************/
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
    CAN_CLIENT_ID_OBC233            = 0x00000233UL,
    CAN_CLIENT_ID_OBC234            = 0x00000234UL,
    CAN_CLIENT_ID_OBC333            = 0x00000333UL,
    CAN_CLIENT_ID_VCU433            = 0x00000433UL,
    CAN_CLIENT_ID_VCU434            = 0x00000434UL,
};
/*  @brief The 233 Signal sent by OBC********************************************/
u08 APP_CAN_u08GetOBC_ObcChgOutVolt(void);
u08 APP_CAN_u08GetOBC_ObcChgOutCur(void);
u08 APP_CAN_u08GetOBC_ObcChgInVolt(void);
u08 APP_CAN_u08GetOBC_ObcChgInCur(void);

/*  @brief The 234 Signal sent by OBC********************************************/
u08 APP_CAN_u08GetOBC_ObcDisChgOutVolt(void);
u08 APP_CAN_u08GetOBC_ObcDisChgOutCur(void);
u08 APP_CAN_u08GetOBC_ObcDisChgInVolt(void);
u08 APP_CAN_u08GetOBC_ObcDisChgInCur(void);

/*  @brief The 333 Signal sent by OBC********************************************/
u08 APP_CAN_u08GetOBC_DcdcOutVolt(void);
u08 APP_CAN_u08GetOBC_DcdcOutCur(void);
u08 APP_CAN_u08GetOBC_DcdcInVolt(void);
u08 APP_CAN_u08GetOBC_DcdcInCur(void);

/*  @brief The 433 Signal sent by VCU********************************************/
u08 APP_CAN_u08GetVCU_ObcChgOutVoltReq(void);
u08 APP_CAN_u08GetVCU_ObcChgOuCurReq(void);

/*  @brief The 434 Signal sent by VCU********************************************/
u08 APP_CAN_u08GetVCU_ObcWorkModeReq(void);

#endif
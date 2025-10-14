/*  (C) Copyright, GoldWei.Ltd. Time:2025/10/15 1:47:59**************************/
#ifndef _CAMMSG_LOCAL_H_
#define _CAMMSG_LOCAL_H_
/*  Includes ********************************************************************/
/*  Enum ************************************************************************/
enum CAN_CLIENT_SIGNAL_OBC233
{
    OBC_ObcChgOutVolt          = 0UL,
    OBC_ObcChgOutCur           = 1UL,
    OBC_ObcChgInVolt           = 2UL,
    OBC_ObcChgInCur            = 3UL,
};

enum CAN_CLIENT_SIGNAL_OBC234
{
    OBC_ObcDisChgOutVolt       = 0UL,
    OBC_ObcDisChgOutCur        = 1UL,
    OBC_ObcDisChgInVolt        = 2UL,
    OBC_ObcDisChgInCur         = 3UL,
};

enum CAN_CLIENT_SIGNAL_OBC333
{
    OBC_DcdcOutVolt            = 0UL,
    OBC_DcdcOutCur             = 1UL,
    OBC_DcdcInVolt             = 2UL,
    OBC_DcdcInCur              = 3UL,
};

enum CAN_CLIENT_SIGNAL_VCU433
{
    VCU_ObcChgOutVoltReq       = 0UL,
    VCU_ObcChgOuCurReq         = 1UL,
};

enum CAN_CLIENT_SIGNAL_VCU434
{
    VCU_ObcWorkModeReq         = 0UL,
};

/*  @brief The 563 Signal sent by OBC********************************************/
typedef struct
{
    u08 u08ObcChgOutVolt;
    u08 u08ObcChgOutCur;
    u08 u08ObcChgInVolt;
    u08 u08ObcChgInCur;
}OBC_233_TX_SIG ;

/*  @brief The 564 Signal sent by OBC********************************************/
typedef struct
{
    u08 u08ObcDisChgOutVolt;
    u08 u08ObcDisChgOutCur;
    u08 u08ObcDisChgInVolt;
    u08 u08ObcDisChgInCur;
}OBC_234_TX_SIG ;

/*  @brief The 819 Signal sent by OBC********************************************/
typedef struct
{
    u08 u08DcdcOutVolt;
    u08 u08DcdcOutCur;
    u08 u08DcdcInVolt;
    u08 u08DcdcInCur;
}OBC_333_TX_SIG ;

/*  @brief The 1075 Signal sent by VCU*******************************************/
typedef struct
{
    u08 u08ObcChgOutVoltReq;
    u08 u08ObcChgOuCurReq;
}VCU_433_RX_SIG ;

/*  @brief The 1076 Signal sent by VCU*******************************************/
typedef struct
{
    u08 u08ObcWorkModeReq;
}VCU_434_RX_SIG ;

/*  Private Function ************************************************************/
u32 MsgFun_233(u32 ulIndex, u32 ulParam);
u32 MsgFun_234(u32 ulIndex, u32 ulParam);
u32 MsgFun_333(u32 ulIndex, u32 ulParam);
u32 MsgFun_433(u32 ulIndex, u32 ulParam);
u32 MsgFun_434(u32 ulIndex, u32 ulParam);
#endif

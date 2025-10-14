/*  (C) Copyright, GoldWei.Ltd. Time:2025/10/15 1:47:59**************************/
/*  Includes ********************************************************************/
 #include "CanMsgLocal.h"
 #include "CanMsgInterface.h"
/*  Private variables ***********************************************************/
static const TS_CANMSG_OBJ DBC_OBC_233[4] =
{
    {OBC_ObcChgOutVolt           , "OBC_ObcChgOutVolt          " , 0         , 8         , MsgFun_233},
    {OBC_ObcChgOutCur            , "OBC_ObcChgOutCur           " , 8         , 8         , MsgFun_233},
    {OBC_ObcChgInVolt            , "OBC_ObcChgInVolt           " , 16        , 8         , MsgFun_233},
    {OBC_ObcChgInCur             , "OBC_ObcChgInCur            " , 24        , 8         , MsgFun_233},
};
static const TS_CANMSG_OBJ DBC_OBC_234[4] =
{
    {OBC_ObcDisChgOutVolt        , "OBC_ObcDisChgOutVolt       " , 0         , 8         , MsgFun_234},
    {OBC_ObcDisChgOutCur         , "OBC_ObcDisChgOutCur        " , 8         , 8         , MsgFun_234},
    {OBC_ObcDisChgInVolt         , "OBC_ObcDisChgInVolt        " , 16        , 8         , MsgFun_234},
    {OBC_ObcDisChgInCur          , "OBC_ObcDisChgInCur         " , 24        , 8         , MsgFun_234},
};
static const TS_CANMSG_OBJ DBC_OBC_333[4] =
{
    {OBC_DcdcOutVolt             , "OBC_DcdcOutVolt            " , 0         , 8         , MsgFun_333},
    {OBC_DcdcOutCur              , "OBC_DcdcOutCur             " , 8         , 8         , MsgFun_333},
    {OBC_DcdcInVolt              , "OBC_DcdcInVolt             " , 16        , 8         , MsgFun_333},
    {OBC_DcdcInCur               , "OBC_DcdcInCur              " , 24        , 8         , MsgFun_333},
};
static const TS_CANMSG_OBJ DBC_VCU_433[2] =
{
    {VCU_ObcChgOutVoltReq        , "VCU_ObcChgOutVoltReq       " , 0         , 8         , MsgFun_433},
    {VCU_ObcChgOuCurReq          , "VCU_ObcChgOuCurReq         " , 8         , 8         , MsgFun_433},
};
static const TS_CANMSG_OBJ DBC_VCU_434[1] =
{
    {VCU_ObcWorkModeReq          , "VCU_ObcWorkModeReq         " , 0         , 2         , MsgFun_434},
};
static const TS_CANMSG canMsgTab[5] = 
{
    {0, "OBC_1"              ,CAN_CLIENT_ID_OBC233            ， 100        ,8          , MOTOROLA_LSB, CANMSG_TYPE_T, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_OBC233           ,sizeof(DBC_OBC233)/sizeof(DBC_OBC233[0]), NULL},
    {1, "OBC_2"              ,CAN_CLIENT_ID_OBC234            ， 100        ,8          , MOTOROLA_LSB, CANMSG_TYPE_T, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_OBC234           ,sizeof(DBC_OBC234)/sizeof(DBC_OBC234[0]), NULL},
    {2, "DCDC_1"             ,CAN_CLIENT_ID_OBC333            ， 100        ,8          , MOTOROLA_LSB, CANMSG_TYPE_T, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_OBC333           ,sizeof(DBC_OBC333)/sizeof(DBC_OBC333[0]), NULL},
    {3, "VCU_1"              ,CAN_CLIENT_ID_VCU433            ， 10         ,8          , MOTOROLA_LSB, CANMSG_TYPE_R, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_VCU433           ,sizeof(DBC_VCU433)/sizeof(DBC_VCU433[0]), NULL},
    {4, "VCU_2"              ,CAN_CLIENT_ID_VCU434            ， 10         ,8          , MOTOROLA_LSB, CANMSG_TYPE_R, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_VCU434           ,sizeof(DBC_VCU434)/sizeof(DBC_VCU434[0]), NULL},
};

const TS_CANMSG * getCanPublicMsgObj(void)
{
    return canMsgTab;
}
u32 getCanPublicMsgObjSize(void)
{
    return sizeof(canMsgTab) / sizeof(canMsgTab[0]);
}

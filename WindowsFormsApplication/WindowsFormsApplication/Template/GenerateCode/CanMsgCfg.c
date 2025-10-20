/*  (C) Copyright, JWPENG. Time:2025/10/20 10:25:50******************************/
/*  Includes ********************************************************************/
 #include "CanMsgLocal.h"
 #include "CanMsgInterface.h"
/*  Private variables ***********************************************************/
static const TS_CANMSG_OBJ DBC_DCDC_0C4[6] =
{
    {DCDC_DCDC2_OutputCurrent             , "DCDC_DCDC2_OutputCurrent            " , 52        , 12        , MsgFun_0C4},
    {DCDC_DCDC2_InputCurrent              , "DCDC_DCDC2_InputCurrent             " , 0         , 8         , MsgFun_0C4},
    {DCDC_DCDC2_OutputVoltage             , "DCDC_DCDC2_OutputVoltage            " , 8         , 8         , MsgFun_0C4},
    {DCDC_DCDC2_InputVolt                 , "DCDC_DCDC2_InputVolt                " , 31        , 9         , MsgFun_0C4},
    {DCDC_RollingCounterC4                , "DCDC_RollingCounterC4               " , 48        , 4         , MsgFun_0C4},
    {DCDC_ChecksumC4                      , "DCDC_ChecksumC4                     " , 56        , 8         , MsgFun_0C4},
};
static const TS_CANMSG_OBJ DBC_EXECU_30A[4] =
{
    {EXECU_OTA_ModeSt                     , "EXECU_OTA_ModeSt                    " , 0         , 1         , MsgFun_30A},
    {EXECU_OTA_EstimatedUpgradeTime       , "EXECU_OTA_EstimatedUpgradeTime      " , 8         , 8         , MsgFun_30A},
    {EXECU_RollingCounter30A              , "EXECU_RollingCounter30A             " , 48        , 4         , MsgFun_30A},
    {EXECU_Checksum30A                    , "EXECU_Checksum30A                   " , 56        , 8         , MsgFun_30A},
};
static const TS_CANMSG_OBJ DBC_EXECU_340[2] =
{
    {EXECU_BCM_BattVolt_EBS2              , "EXECU_BCM_BattVolt_EBS2             " , 14        , 10        , MsgFun_340},
    {EXECU_BCM_12VChrgReq                 , "EXECU_BCM_12VChrgReq                " , 44        , 1         , MsgFun_340},
};
static const TS_CANMSG_OBJ DBC_DCDC_344[8] =
{
    {DCDC_DCDC2_Temperature               , "DCDC_DCDC2_Temperature              " , 0         , 8         , MsgFun_344},
    {DCDC_DCDC2_Fault                     , "DCDC_DCDC2_Fault                    " , 16        , 8         , MsgFun_344},
    {DCDC_DCDC2_Efficiency                , "DCDC_DCDC2_Efficiency               " , 24        , 7         , MsgFun_344},
    {DCDC_DCDC2_HighVoltCnctAllowed       , "DCDC_DCDC2_HighVoltCnctAllowed      " , 31        , 1         , MsgFun_344},
    {DCDC_DCDC2_State                     , "DCDC_DCDC2_State                    " , 37        , 3         , MsgFun_344},
    {DCDC_DCDC2_OverTempSt                , "DCDC_DCDC2_OverTempSt               " , 36        , 1         , MsgFun_344},
    {DCDC_RollingCounter344               , "DCDC_RollingCounter344              " , 48        , 4         , MsgFun_344},
    {DCDC_Checksum344                     , "DCDC_Checksum344                    " , 56        , 8         , MsgFun_344},
};
static const TS_CANMSG_OBJ DBC_EXECU_372[2] =
{
    {EXECU_PDCU_DCDC2VolReq               , "EXECU_PDCU_DCDC2VolReq              " , 33        , 7         , MsgFun_372},
    {EXECU_PDCU_DCDC2Enable               , "EXECU_PDCU_DCDC2Enable              " , 46        , 2         , MsgFun_372},
};
static const TS_CANMSG_OBJ DBC_DCDC_59C[7] =
{
    {DCDC_NMNode                          , "DCDC_NMNode                         " , 0         , 8         , MsgFun_59C},
    {DCDC_ActiveWakeupBit                 , "DCDC_ActiveWakeupBit                " , 12        , 1         , MsgFun_59C},
    {DCDC_RepeatMessageRequestBit         , "DCDC_RepeatMessageRequestBit        " , 8         , 1         , MsgFun_59C},
    {DCDC_RepeatSts                       , "DCDC_RepeatSts                      " , 16        , 1         , MsgFun_59C},
    {DCDC_NMWakeupStype                   , "DCDC_NMWakeupStype                  " , 52        , 4         , MsgFun_59C},
    {DCDC_NMWakeupPower                   , "DCDC_NMWakeupPower                  " , 48        , 4         , MsgFun_59C},
    {DCDC_NMWakeupSource                  , "DCDC_NMWakeupSource                 " , 56        , 8         , MsgFun_59C},
};
static const TS_CANMSG canMsgTab[6] = 
{
    {0, "DC4"                ,CAN_CLIENT_ID_DCDC_0C4         ,10         ,8          , MOTOROLA_LSB, CANMSG_TYPE_T, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_DCDC_0C4         ,sizeof(DBC_DCDC_0C4)/sizeof(DBC_DCDC_0C4[0]), NULL},
    {1, "OTAInfo"            ,CAN_CLIENT_ID_EXECU_30A        ,100        ,8          , MOTOROLA_LSB, CANMSG_TYPE_R, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_EXECU_30A        ,sizeof(DBC_EXECU_30A)/sizeof(DBC_EXECU_30A[0]), NULL},
    {2, "IBCM_EBS2"          ,CAN_CLIENT_ID_EXECU_340        ,100        ,8          , MOTOROLA_LSB, CANMSG_TYPE_R, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_EXECU_340        ,sizeof(DBC_EXECU_340)/sizeof(DBC_EXECU_340[0]), NULL},
    {3, "DC3"                ,CAN_CLIENT_ID_DCDC_344         ,100        ,8          , MOTOROLA_LSB, CANMSG_TYPE_T, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_DCDC_344         ,sizeof(DBC_DCDC_344)/sizeof(DBC_DCDC_344[0]), NULL},
    {4, "PDCU_CtrlDCDC"      ,CAN_CLIENT_ID_EXECU_372        ,100        ,8          , MOTOROLA_LSB, CANMSG_TYPE_R, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_EXECU_372        ,sizeof(DBC_EXECU_372)/sizeof(DBC_EXECU_372[0]), NULL},
    {5, "DCDC2_NM"           ,CAN_CLIENT_ID_DCDC_59C         ,500        ,8          , MOTOROLA_LSB, CANMSG_TYPE_T, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_DCDC_59C         ,sizeof(DBC_DCDC_59C)/sizeof(DBC_DCDC_59C[0]), NULL},
};

const TS_CANMSG * getCanPublicMsgObj(void)
{
    return canMsgTab;
}
u32 getCanPublicMsgObjSize(void)
{
    return sizeof(canMsgTab) / sizeof(canMsgTab[0]);
}

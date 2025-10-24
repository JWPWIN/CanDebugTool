/*  (C) Copyright, JWPENG. Time:2025/10/23 15:08:31******************************/
/*  Includes ********************************************************************/
 #include "CanMsgLocal.h"
 #include "CanMsgInterface.h"
/*  Private variables ***********************************************************/
static const TS_CANMSG_OBJ DBC_EXECU_0A0[2] =
{
    {EXECU_ESC_VehSpdValidFlag            , "EXECU_ESC_VehSpdValidFlag           " , 25        , 1         , MsgFun_0A0},
    {EXECU_ESC_VehSpd                     , "EXECU_ESC_VehSpd                    " , 56        , 16        , MsgFun_0A0},
};
static const TS_CANMSG_OBJ DBC_DCDC_0C4[6] =
{
    {DCDC_DCDC2_OutputCurrent             , "DCDC_DCDC2_OutputCurrent            " , 52        , 12        , MsgFun_0C4},
    {DCDC_DCDC2_InputCurrent              , "DCDC_DCDC2_InputCurrent             " , 0         , 8         , MsgFun_0C4},
    {DCDC_DCDC2_OutputVoltage             , "DCDC_DCDC2_OutputVoltage            " , 8         , 8         , MsgFun_0C4},
    {DCDC_DCDC2_InputVolt                 , "DCDC_DCDC2_InputVolt                " , 31        , 9         , MsgFun_0C4},
    {DCDC_RollingCounterC4                , "DCDC_RollingCounterC4               " , 48        , 4         , MsgFun_0C4},
    {DCDC_ChecksumC4                      , "DCDC_ChecksumC4                     " , 56        , 8         , MsgFun_0C4},
};
static const TS_CANMSG_OBJ DBC_EXECU_0FE[1] =
{
    {EXECU_PDCU_DriveReady                , "EXECU_PDCU_DriveReady               " , 0         , 1         , MsgFun_0FE},
};
static const TS_CANMSG_OBJ DBC_EXECU_30A[4] =
{
    {EXECU_OTA_ModeSt                     , "EXECU_OTA_ModeSt                    " , 0         , 1         , MsgFun_30A},
    {EXECU_OTA_EstimatedUpgradeTime       , "EXECU_OTA_EstimatedUpgradeTime      " , 8         , 8         , MsgFun_30A},
    {EXECU_RollingCounter30A              , "EXECU_RollingCounter30A             " , 48        , 4         , MsgFun_30A},
    {EXECU_Checksum30A                    , "EXECU_Checksum30A                   " , 56        , 8         , MsgFun_30A},
};
static const TS_CANMSG_OBJ DBC_EXECU_320[1] =
{
    {EXECU_IC_TotalOdmeter                , "EXECU_IC_TotalOdmeter               " , 16        , 20        , MsgFun_320},
};
static const TS_CANMSG_OBJ DBC_EXECU_33C[1] =
{
    {EXECU_BCM_IgnitionSt                 , "EXECU_BCM_IgnitionSt                " , 0         , 2         , MsgFun_33C},
};
static const TS_CANMSG_OBJ DBC_EXECU_340[1] =
{
    {EXECU_BCM_BattVolt_EBS2              , "EXECU_BCM_BattVolt_EBS2             " , 14        , 10        , MsgFun_340},
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
static const TS_CANMSG_OBJ DBC_EXECU_4DA[8] =
{
    {EXECU_MP5_TBOX_Time_Year             , "EXECU_MP5_TBOX_Time_Year            " , 40        , 8         , MsgFun_4DA},
    {EXECU_MP5_TBOX_Time_Month            , "EXECU_MP5_TBOX_Time_Month           " , 4         , 4         , MsgFun_4DA},
    {EXECU_MP5_TBOX_Time_Date             , "EXECU_MP5_TBOX_Time_Date            " , 8         , 5         , MsgFun_4DA},
    {EXECU_MP5_TBOX_Time_Hour             , "EXECU_MP5_TBOX_Time_Hour            " , 16        , 5         , MsgFun_4DA},
    {EXECU_MP5_TBOX_Time_Minute           , "EXECU_MP5_TBOX_Time_Minute          " , 24        , 6         , MsgFun_4DA},
    {EXECU_MP5_TBOX_Time_Second           , "EXECU_MP5_TBOX_Time_Second          " , 32        , 6         , MsgFun_4DA},
    {EXECU_MP5_TBOX_Time_YearMark         , "EXECU_MP5_TBOX_Time_YearMark        " , 14        , 1         , MsgFun_4DA},
    {EXECU_MP5_TBOX_Time_Valid            , "EXECU_MP5_TBOX_Time_Valid           " , 15        , 1         , MsgFun_4DA},
};
static const TS_CANMSG canMsgTab[10] = 
{
    {0, "ESC2"               ,CAN_CLIENT_ID_EXECU_0A0        ,10         ,8          , MOTOROLA_LSB, CANMSG_TYPE_R, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_EXECU_0A0        ,sizeof(DBC_EXECU_0A0)/sizeof(DBC_EXECU_0A0[0]), NULL},
    {1, "DC4"                ,CAN_CLIENT_ID_DCDC_0C4         ,10         ,8          , MOTOROLA_LSB, CANMSG_TYPE_T, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_DCDC_0C4         ,sizeof(DBC_DCDC_0C4)/sizeof(DBC_DCDC_0C4[0]), NULL},
    {2, "PDCU1"              ,CAN_CLIENT_ID_EXECU_0FE        ,20         ,8          , MOTOROLA_LSB, CANMSG_TYPE_R, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_EXECU_0FE        ,sizeof(DBC_EXECU_0FE)/sizeof(DBC_EXECU_0FE[0]), NULL},
    {3, "OTAInfo"            ,CAN_CLIENT_ID_EXECU_30A        ,100        ,8          , MOTOROLA_LSB, CANMSG_TYPE_R, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_EXECU_30A        ,sizeof(DBC_EXECU_30A)/sizeof(DBC_EXECU_30A[0]), NULL},
    {4, "IC1"                ,CAN_CLIENT_ID_EXECU_320        ,100        ,8          , MOTOROLA_LSB, CANMSG_TYPE_R, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_EXECU_320        ,sizeof(DBC_EXECU_320)/sizeof(DBC_EXECU_320[0]), NULL},
    {5, "IBCM3"              ,CAN_CLIENT_ID_EXECU_33C        ,100        ,8          , MOTOROLA_LSB, CANMSG_TYPE_R, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_EXECU_33C        ,sizeof(DBC_EXECU_33C)/sizeof(DBC_EXECU_33C[0]), NULL},
    {6, "IBCM_EBS2"          ,CAN_CLIENT_ID_EXECU_340        ,100        ,8          , MOTOROLA_LSB, CANMSG_TYPE_R, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_EXECU_340        ,sizeof(DBC_EXECU_340)/sizeof(DBC_EXECU_340[0]), NULL},
    {7, "DC3"                ,CAN_CLIENT_ID_DCDC_344         ,100        ,8          , MOTOROLA_LSB, CANMSG_TYPE_T, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_DCDC_344         ,sizeof(DBC_DCDC_344)/sizeof(DBC_DCDC_344[0]), NULL},
    {8, "PDCU_CtrlDCDC"      ,CAN_CLIENT_ID_EXECU_372        ,100        ,8          , MOTOROLA_LSB, CANMSG_TYPE_R, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_EXECU_372        ,sizeof(DBC_EXECU_372)/sizeof(DBC_EXECU_372[0]), NULL},
    {9, "MP5_Time"           ,CAN_CLIENT_ID_EXECU_4DA        ,1000       ,8          , MOTOROLA_LSB, CANMSG_TYPE_R, CANFD_BRS_FRAME_TYPE_STANDARD, CAN_ID_MASK0, DBC_EXECU_4DA        ,sizeof(DBC_EXECU_4DA)/sizeof(DBC_EXECU_4DA[0]), NULL},
};

const TS_CANMSG * getCanPublicMsgObj(void)
{
    return canMsgTab;
}
u32 getCanPublicMsgObjSize(void)
{
    return sizeof(canMsgTab) / sizeof(canMsgTab[0]);
}

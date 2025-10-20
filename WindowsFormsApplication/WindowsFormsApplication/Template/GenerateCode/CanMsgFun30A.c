/*  (C) Copyright, JWPENG. Time:2025/10/20 10:25:50******************************/
/*  Includes ********************************************************************/
#include "CanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static EXECU_30A_RX_SIG EXECU30aMsg = {0};

u32 MsgFun_30A(u32 ulIndex, u32 ulParam)
{
    switch (ulIndex)
    {
        case EXECU_OTA_ModeSt:
            EXECU30aMsg.u08OTA_ModeSt= (u08)ulParam;
            break;
        case EXECU_OTA_EstimatedUpgradeTime:
            EXECU30aMsg.u08OTA_EstimatedUpgradeTime= (u08)ulParam;
            break;
        case EXECU_RollingCounter30A:
            EXECU30aMsg.u08RollingCounter30A= (u08)ulParam;
            break;
        case EXECU_Checksum30A:
            EXECU30aMsg.u08Checksum30A= (u08)ulParam;
            break;
        default:
            break;
    }
    return 0;
}

u08 APP_CAN_u08GetEXECU_OTA_ModeSt(void)
{
    return EXECU30aMsg.u08OTA_ModeSt;
}

u08 APP_CAN_u08GetEXECU_OTA_EstimatedUpgradeTime(void)
{
    return EXECU30aMsg.u08OTA_EstimatedUpgradeTime;
}

u08 APP_CAN_u08GetEXECU_RollingCounter30A(void)
{
    return EXECU30aMsg.u08RollingCounter30A;
}

u08 APP_CAN_u08GetEXECU_Checksum30A(void)
{
    return EXECU30aMsg.u08Checksum30A;
}


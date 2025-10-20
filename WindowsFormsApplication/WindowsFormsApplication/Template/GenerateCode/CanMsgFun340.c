/*  (C) Copyright, JWPENG. Time:2025/10/20 10:25:50******************************/
/*  Includes ********************************************************************/
#include "CanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static EXECU_340_RX_SIG EXECU340Msg = {0};

u32 MsgFun_340(u32 ulIndex, u32 ulParam)
{
    switch (ulIndex)
    {
        case EXECU_BCM_BattVolt_EBS2:
            EXECU340Msg.u16BCM_BattVolt_EBS2= (u16)ulParam;
            break;
        case EXECU_BCM_12VChrgReq:
            EXECU340Msg.u08BCM_12VChrgReq= (u08)ulParam;
            break;
        default:
            break;
    }
    return 0;
}

u16 APP_CAN_u16GetEXECU_BCM_BattVolt_EBS2(void)
{
    return EXECU340Msg.u16BCM_BattVolt_EBS2;
}

u08 APP_CAN_u08GetEXECU_BCM_12VChrgReq(void)
{
    return EXECU340Msg.u08BCM_12VChrgReq;
}


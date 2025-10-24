/*  (C) Copyright, JWPENG. Time:2025/10/23 15:08:31******************************/
/*  Includes ********************************************************************/
#include "CanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static EXECU_33C_RX_SIG EXECU33cMsg = {0};

u32 MsgFun_33C(u32 ulIndex, u32 ulParam)
{
    switch (ulIndex)
    {
        case EXECU_BCM_IgnitionSt:
            EXECU33cMsg.u08BCM_IgnitionSt= (u08)ulParam;
            break;
        default:
            break;
    }
    return 0;
}

u08 APP_CAN_u08GetEXECU_BCM_IgnitionSt(void)
{
    return EXECU33cMsg.u08BCM_IgnitionSt;
}


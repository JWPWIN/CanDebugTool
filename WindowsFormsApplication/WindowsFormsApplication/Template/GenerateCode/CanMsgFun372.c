/*  (C) Copyright, JWPENG. Time:2025/10/23 15:08:31******************************/
/*  Includes ********************************************************************/
#include "CanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static EXECU_372_RX_SIG EXECU372Msg = {0};

u32 MsgFun_372(u32 ulIndex, u32 ulParam)
{
    switch (ulIndex)
    {
        case EXECU_PDCU_DCDC2VolReq:
            EXECU372Msg.u08PDCU_DCDC2VolReq= (u08)ulParam;
            break;
        case EXECU_PDCU_DCDC2Enable:
            EXECU372Msg.u08PDCU_DCDC2Enable= (u08)ulParam;
            break;
        default:
            break;
    }
    return 0;
}

u08 APP_CAN_u08GetEXECU_PDCU_DCDC2VolReq(void)
{
    return EXECU372Msg.u08PDCU_DCDC2VolReq;
}

u08 APP_CAN_u08GetEXECU_PDCU_DCDC2Enable(void)
{
    return EXECU372Msg.u08PDCU_DCDC2Enable;
}


/*  (C) Copyright, GoldWei.Ltd. Time:2025/10/15 1:47:59**************************/
/*  Includes ********************************************************************/
#include "AppCanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static VCU_433_RX_SIG VCU433Msg = {0};

u32 MsgFun433(u32 ulIndex, u32 ulParam)
{
    u32 ulRetValue = 0;
    switch (ulIndex)
    {
        case VCU_ObcChgOutVoltReq:
            VCU433Msg.u08ObcChgOutVoltReq= ulParam;
            break;
        case VCU_ObcChgOuCurReq:
            VCU433Msg.u08ObcChgOuCurReq= ulParam;
            break;
        default:
            break;
    }
    return 0;
}

u08 APP_CAN_u08GetVCU_ObcChgOutVoltReq(void)
{
    return VCU433Msg.u08ObcChgOutVoltReq;
}

u08 APP_CAN_u08GetVCU_ObcChgOuCurReq(void)
{
    return VCU433Msg.u08ObcChgOuCurReq;
}


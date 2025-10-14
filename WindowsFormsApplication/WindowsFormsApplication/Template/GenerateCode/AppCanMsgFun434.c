/*  (C) Copyright, GoldWei.Ltd. Time:2025/10/15 1:47:59**************************/
/*  Includes ********************************************************************/
#include "AppCanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static VCU_434_RX_SIG VCU434Msg = {0};

u32 MsgFun434(u32 ulIndex, u32 ulParam)
{
    u32 ulRetValue = 0;
    switch (ulIndex)
    {
        case VCU_ObcWorkModeReq:
            VCU434Msg.u08ObcWorkModeReq= ulParam;
            break;
        default:
            break;
    }
    return 0;
}

u08 APP_CAN_u08GetVCU_ObcWorkModeReq(void)
{
    return VCU434Msg.u08ObcWorkModeReq;
}


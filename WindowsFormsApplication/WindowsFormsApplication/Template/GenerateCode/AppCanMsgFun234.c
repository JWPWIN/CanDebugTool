/*  (C) Copyright, GoldWei.Ltd. Time:2025/10/15 1:47:59**************************/
/*  Includes ********************************************************************/
#include "AppCanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static OBC_234_TX_SIG OBC234Msg = {0};

u32 MsgFun234(u32 ulIndex, u32 ulParam)
{
    u32 ulRetValue = 0;
    switch (ulIndex)
    {
        case OBC_ObcDisChgOutVolt:
            ulRetValue = 0;
            break;
        case OBC_ObcDisChgOutCur:
            ulRetValue = 0;
            break;
        case OBC_ObcDisChgInVolt:
            ulRetValue = 0;
            break;
        case OBC_ObcDisChgInCur:
            ulRetValue = 0;
            break;
        default:
            break;
    }
    return ulRetValue;
}

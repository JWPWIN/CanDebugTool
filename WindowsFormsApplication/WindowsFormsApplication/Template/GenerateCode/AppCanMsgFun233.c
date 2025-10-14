/*  (C) Copyright, GoldWei.Ltd. Time:2025/10/15 1:47:59**************************/
/*  Includes ********************************************************************/
#include "AppCanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static OBC_233_TX_SIG OBC233Msg = {0};

u32 MsgFun233(u32 ulIndex, u32 ulParam)
{
    u32 ulRetValue = 0;
    switch (ulIndex)
    {
        case OBC_ObcChgOutVolt:
            ulRetValue = 0;
            break;
        case OBC_ObcChgOutCur:
            ulRetValue = 0;
            break;
        case OBC_ObcChgInVolt:
            ulRetValue = 0;
            break;
        case OBC_ObcChgInCur:
            ulRetValue = 0;
            break;
        default:
            break;
    }
    return ulRetValue;
}

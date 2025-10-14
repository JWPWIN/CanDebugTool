/*  (C) Copyright, GoldWei.Ltd. Time:2025/10/15 1:47:59**************************/
/*  Includes ********************************************************************/
#include "AppCanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static OBC_333_TX_SIG OBC333Msg = {0};

u32 MsgFun333(u32 ulIndex, u32 ulParam)
{
    u32 ulRetValue = 0;
    switch (ulIndex)
    {
        case OBC_DcdcOutVolt:
            ulRetValue = 0;
            break;
        case OBC_DcdcOutCur:
            ulRetValue = 0;
            break;
        case OBC_DcdcInVolt:
            ulRetValue = 0;
            break;
        case OBC_DcdcInCur:
            ulRetValue = 0;
            break;
        default:
            break;
    }
    return ulRetValue;
}

/*  (C) Copyright, JWPENG. Time:2025/10/20 10:25:50******************************/
/*  Includes ********************************************************************/
#include "CanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static DCDC_59C_TX_SIG DCDC59cMsg = {0};

u32 MsgFun_59C(u32 ulIndex, u32 ulParam)
{
    u32 ulRetValue = 0;
    switch (ulIndex)
    {
        case DCDC_NMNode:
            ulRetValue = 0;
            break;
        case DCDC_ActiveWakeupBit:
            ulRetValue = 0;
            break;
        case DCDC_RepeatMessageRequestBit:
            ulRetValue = 0;
            break;
        case DCDC_RepeatSts:
            ulRetValue = 0;
            break;
        case DCDC_NMWakeupStype:
            ulRetValue = 0;
            break;
        case DCDC_NMWakeupPower:
            ulRetValue = 0;
            break;
        case DCDC_NMWakeupSource:
            ulRetValue = 0;
            break;
        default:
            break;
    }
    return ulRetValue;
}

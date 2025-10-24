/*  (C) Copyright, JWPENG. Time:2025/10/23 15:08:31******************************/
/*  Includes ********************************************************************/
#include "CanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static DCDC_0C4_TX_SIG DCDC0c4Msg = {0};

u32 MsgFun_0C4(u32 ulIndex, u32 ulParam)
{
    u32 ulRetValue = 0;
    switch (ulIndex)
    {
        case DCDC_DCDC2_OutputCurrent:
            ulRetValue = 0;
            break;
        case DCDC_DCDC2_InputCurrent:
            ulRetValue = 0;
            break;
        case DCDC_DCDC2_OutputVoltage:
            ulRetValue = 0;
            break;
        case DCDC_DCDC2_InputVolt:
            ulRetValue = 0;
            break;
        case DCDC_RollingCounterC4:
            ulRetValue = 0;
            break;
        case DCDC_ChecksumC4:
            ulRetValue = 0;
            break;
        default:
            break;
    }
    return ulRetValue;
}

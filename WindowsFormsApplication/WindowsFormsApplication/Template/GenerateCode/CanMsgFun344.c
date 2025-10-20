/*  (C) Copyright, JWPENG. Time:2025/10/20 10:25:50******************************/
/*  Includes ********************************************************************/
#include "CanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static DCDC_344_TX_SIG DCDC344Msg = {0};

u32 MsgFun_344(u32 ulIndex, u32 ulParam)
{
    u32 ulRetValue = 0;
    switch (ulIndex)
    {
        case DCDC_DCDC2_Temperature:
            ulRetValue = 0;
            break;
        case DCDC_DCDC2_Fault:
            ulRetValue = 0;
            break;
        case DCDC_DCDC2_Efficiency:
            ulRetValue = 0;
            break;
        case DCDC_DCDC2_HighVoltCnctAllowed:
            ulRetValue = 0;
            break;
        case DCDC_DCDC2_State:
            ulRetValue = 0;
            break;
        case DCDC_DCDC2_OverTempSt:
            ulRetValue = 0;
            break;
        case DCDC_RollingCounter344:
            ulRetValue = 0;
            break;
        case DCDC_Checksum344:
            ulRetValue = 0;
            break;
        default:
            break;
    }
    return ulRetValue;
}

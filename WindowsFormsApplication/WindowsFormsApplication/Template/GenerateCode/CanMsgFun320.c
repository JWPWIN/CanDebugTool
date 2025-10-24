/*  (C) Copyright, JWPENG. Time:2025/10/23 15:08:31******************************/
/*  Includes ********************************************************************/
#include "CanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static EXECU_320_RX_SIG EXECU320Msg = {0};

u32 MsgFun_320(u32 ulIndex, u32 ulParam)
{
    switch (ulIndex)
    {
        case EXECU_IC_TotalOdmeter:
            EXECU320Msg.u32IC_TotalOdmeter= (u32)ulParam;
            break;
        default:
            break;
    }
    return 0;
}

u32 APP_CAN_u32GetEXECU_IC_TotalOdmeter(void)
{
    return EXECU320Msg.u32IC_TotalOdmeter;
}


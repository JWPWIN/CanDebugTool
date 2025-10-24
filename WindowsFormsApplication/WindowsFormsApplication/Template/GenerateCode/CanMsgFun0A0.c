/*  (C) Copyright, JWPENG. Time:2025/10/23 15:08:31******************************/
/*  Includes ********************************************************************/
#include "CanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static EXECU_0A0_RX_SIG EXECU0a0Msg = {0};

u32 MsgFun_0A0(u32 ulIndex, u32 ulParam)
{
    switch (ulIndex)
    {
        case EXECU_ESC_VehSpdValidFlag:
            EXECU0a0Msg.u08ESC_VehSpdValidFlag= (u08)ulParam;
            break;
        case EXECU_ESC_VehSpd:
            EXECU0a0Msg.u16ESC_VehSpd= (u16)ulParam;
            break;
        default:
            break;
    }
    return 0;
}

u08 APP_CAN_u08GetEXECU_ESC_VehSpdValidFlag(void)
{
    return EXECU0a0Msg.u08ESC_VehSpdValidFlag;
}

u16 APP_CAN_u16GetEXECU_ESC_VehSpd(void)
{
    return EXECU0a0Msg.u16ESC_VehSpd;
}


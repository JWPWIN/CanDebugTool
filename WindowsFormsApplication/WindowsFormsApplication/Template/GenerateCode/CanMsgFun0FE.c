/*  (C) Copyright, JWPENG. Time:2025/10/23 15:08:31******************************/
/*  Includes ********************************************************************/
#include "CanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static EXECU_0FE_RX_SIG EXECU0feMsg = {0};

u32 MsgFun_0FE(u32 ulIndex, u32 ulParam)
{
    switch (ulIndex)
    {
        case EXECU_PDCU_DriveReady:
            EXECU0feMsg.u08PDCU_DriveReady= (u08)ulParam;
            break;
        default:
            break;
    }
    return 0;
}

u08 APP_CAN_u08GetEXECU_PDCU_DriveReady(void)
{
    return EXECU0feMsg.u08PDCU_DriveReady;
}


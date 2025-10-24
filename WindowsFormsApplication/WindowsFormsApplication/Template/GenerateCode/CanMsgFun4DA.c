/*  (C) Copyright, JWPENG. Time:2025/10/23 15:08:31******************************/
/*  Includes ********************************************************************/
#include "CanMsgLocal.h"
#include "CanMsgInterface.h"
/*  Private variable ************************************************************/
static EXECU_4DA_RX_SIG EXECU4daMsg = {0};

u32 MsgFun_4DA(u32 ulIndex, u32 ulParam)
{
    switch (ulIndex)
    {
        case EXECU_MP5_TBOX_Time_Year:
            EXECU4daMsg.u08MP5_TBOX_Time_Year= (u08)ulParam;
            break;
        case EXECU_MP5_TBOX_Time_Month:
            EXECU4daMsg.u08MP5_TBOX_Time_Month= (u08)ulParam;
            break;
        case EXECU_MP5_TBOX_Time_Date:
            EXECU4daMsg.u08MP5_TBOX_Time_Date= (u08)ulParam;
            break;
        case EXECU_MP5_TBOX_Time_Hour:
            EXECU4daMsg.u08MP5_TBOX_Time_Hour= (u08)ulParam;
            break;
        case EXECU_MP5_TBOX_Time_Minute:
            EXECU4daMsg.u08MP5_TBOX_Time_Minute= (u08)ulParam;
            break;
        case EXECU_MP5_TBOX_Time_Second:
            EXECU4daMsg.u08MP5_TBOX_Time_Second= (u08)ulParam;
            break;
        case EXECU_MP5_TBOX_Time_YearMark:
            EXECU4daMsg.u08MP5_TBOX_Time_YearMark= (u08)ulParam;
            break;
        case EXECU_MP5_TBOX_Time_Valid:
            EXECU4daMsg.u08MP5_TBOX_Time_Valid= (u08)ulParam;
            break;
        default:
            break;
    }
    return 0;
}

u08 APP_CAN_u08GetEXECU_MP5_TBOX_Time_Year(void)
{
    return EXECU4daMsg.u08MP5_TBOX_Time_Year;
}

u08 APP_CAN_u08GetEXECU_MP5_TBOX_Time_Month(void)
{
    return EXECU4daMsg.u08MP5_TBOX_Time_Month;
}

u08 APP_CAN_u08GetEXECU_MP5_TBOX_Time_Date(void)
{
    return EXECU4daMsg.u08MP5_TBOX_Time_Date;
}

u08 APP_CAN_u08GetEXECU_MP5_TBOX_Time_Hour(void)
{
    return EXECU4daMsg.u08MP5_TBOX_Time_Hour;
}

u08 APP_CAN_u08GetEXECU_MP5_TBOX_Time_Minute(void)
{
    return EXECU4daMsg.u08MP5_TBOX_Time_Minute;
}

u08 APP_CAN_u08GetEXECU_MP5_TBOX_Time_Second(void)
{
    return EXECU4daMsg.u08MP5_TBOX_Time_Second;
}

u08 APP_CAN_u08GetEXECU_MP5_TBOX_Time_YearMark(void)
{
    return EXECU4daMsg.u08MP5_TBOX_Time_YearMark;
}

u08 APP_CAN_u08GetEXECU_MP5_TBOX_Time_Valid(void)
{
    return EXECU4daMsg.u08MP5_TBOX_Time_Valid;
}


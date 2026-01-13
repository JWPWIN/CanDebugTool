/***** (C) Copyright, Shenzhen SHINRY Technology Co.,Ltd. ******header file*****
* File name          : CanBitlib.h
* Author             : henry
* Brief              : application header file of CanBitlib.c
********************************************************************************
* modify
* Version   Date(YYYY/MM/DD)    Author  Described
* V1.00     2016/07/13          henry   First Issue
*******************************************************************************/

#ifndef APP_CAN_BITLIB_H_
#define APP_CAN_BITLIB_H_

#ifdef  APP_CAN_BITLIB_C
#define APP_CAN_BITLIB_DEC
#else
#define APP_CAN_BITLIB_DEC  extern
#endif

/* Includes ------------------------------------------------------------------*/
#include "SysTypes.h"

/* Export define -------------------------------------------------------------*/
#define DBC_INIT(sig, formt, start, len) \
        [sig] =                          \
        {                                \
            .bit_formt = formt,          \
            .bit_start = start,          \
            .bit_len = len               \
        }

/* Export macro --------------------------------------------------------------*/
/* Export typedef ------------------------------------------------------------*/
typedef enum
{
    INTEL_STANDARD,
    INTEL_SEQUENTIAL,
    MOTOROLA_LSB,
    MOTOROLA_MSB,
    MOTOROLA_BACKWARD,
    MOTOROLA_SEQUENTIAL
}CAN_SIG_FORMAT;

/* Export variables ----------------------------------------------------------*/
/* Export function -----------------------------------------------------------*/
//APP_CAN_BITLIB_DEC void set_frame_data(u08 frame_data[], CAN_SIG_FORMAT format, u16 bit_start, u16 bit_len, u32 value);
//APP_CAN_BITLIB_DEC void get_frame_data(u08 frame_data[], CAN_SIG_FORMAT format, u16 bit_start, u16 bit_len, u32* value);
//APP_CAN_BITLIB_DEC void set_frame_dataFD(u08 frame_data[], CAN_SIG_FORMAT format, u16 bit_start, u16 bit_len, u32 value);
//APP_CAN_BITLIB_DEC void get_frame_dataFD(u08 frame_data[], CAN_SIG_FORMAT format, u16 bit_start, u16 bit_len, u32* value);

#endif

/******** (C) Copyright, Shenzhen SHINRY Technology Co.,Ltd. ******** End *****/

#pragma once
#include "SysTypes.h"
#include "AppCanBitlib.h"

//CAN类型报文接口
__declspec(dllexport) extern void set_frame_data(u08 frame_data[], CAN_SIG_FORMAT format, u16 bit_start, u16 bit_len, u32 value);
__declspec(dllexport) extern void get_frame_data(u08 frame_data[], CAN_SIG_FORMAT format, u16 bit_start, u16 bit_len, u32* value);

//CANFD类型报文接口
__declspec(dllexport) extern void set_frame_dataFD(u08 frame_data[], CAN_SIG_FORMAT format, u16 bit_start, u16 bit_len, u32 value);
__declspec(dllexport) extern void get_frame_dataFD(u08 frame_data[], CAN_SIG_FORMAT format, u16 bit_start, u16 bit_len, u32* value);

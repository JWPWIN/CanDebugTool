
#include "AppCanBitlib.h"

/* Private typedef -----------------------------------------------------------*/
typedef struct
{
   u64 BYTE0:8;
   u64 BYTE1:8;
   u64 BYTE2:8;
   u64 BYTE3:8;

   u64 BYTE4:8;
   u64 BYTE5:8;
   u64 BYTE6:8;
   u64 BYTE7:8;
} TS_BYTES_DATA;

typedef union
{
	u64 all;
	TS_BYTES_DATA bit;
}TU_UINT64_DATA;

/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/

/* Private function property -------------------------------------------------*/
__declspec(dllexport) void set_frame_data(u08 frame_data[], CAN_SIG_FORMAT format, u16 bit_start, u16 bit_len, u32 value)
{
    volatile u32 index, mask;
    TU_UINT64_DATA long_data = {0};

    if(format == INTEL_STANDARD)
    {
        index = bit_start;
        mask = (u32)((0x1ULL << bit_len)-1);

        long_data.all = value & mask;
        long_data.all <<= index;

        frame_data[0] |= long_data.bit.BYTE0;
        frame_data[1] |= long_data.bit.BYTE1;
        frame_data[2] |= long_data.bit.BYTE2;
        frame_data[3] |= long_data.bit.BYTE3;
        frame_data[4] |= long_data.bit.BYTE4;
        frame_data[5] |= long_data.bit.BYTE5;
        frame_data[6] |= long_data.bit.BYTE6;
        frame_data[7] |= long_data.bit.BYTE7;
    }
    else if(format == INTEL_SEQUENTIAL)
    {
        index = bit_start^0x7;
        mask = (u32)((0x1ULL << bit_len)-1);
        long_data.all = value & mask;
        long_data.all <<= index;

        frame_data[0] |= long_data.bit.BYTE0;
        frame_data[1] |= long_data.bit.BYTE1;
        frame_data[2] |= long_data.bit.BYTE2;
        frame_data[3] |= long_data.bit.BYTE3;
        frame_data[4] |= long_data.bit.BYTE4;
        frame_data[5] |= long_data.bit.BYTE5;
        frame_data[6] |= long_data.bit.BYTE6;
        frame_data[7] |= long_data.bit.BYTE7;
    }
    else if(format == MOTOROLA_LSB)
    {
        index = bit_start^0x38;
        mask = (u32)((0x1ULL << bit_len)-1);
        long_data.all = value & mask;
        long_data.all <<= index;

        frame_data[7] |= long_data.bit.BYTE0;
        frame_data[6] |= long_data.bit.BYTE1;
        frame_data[5] |= long_data.bit.BYTE2;
        frame_data[4] |= long_data.bit.BYTE3;
        frame_data[3] |= long_data.bit.BYTE4;
        frame_data[2] |= long_data.bit.BYTE5;
        frame_data[1] |= long_data.bit.BYTE6;
        frame_data[0] |= long_data.bit.BYTE7;
    }
    else if(format == MOTOROLA_MSB)
    {
        index = ((bit_start^0x7)+bit_len-1)^0x3F;
        mask = (u32)((0x1ULL << bit_len)-1);
        long_data.all = value & mask;
        long_data.all <<= index;

        frame_data[7] |= long_data.bit.BYTE0;
        frame_data[6] |= long_data.bit.BYTE1;
        frame_data[5] |= long_data.bit.BYTE2;
        frame_data[4] |= long_data.bit.BYTE3;
        frame_data[3] |= long_data.bit.BYTE4;
        frame_data[2] |= long_data.bit.BYTE5;
        frame_data[1] |= long_data.bit.BYTE6;
        frame_data[0] |= long_data.bit.BYTE7;
    }
    else if(format == MOTOROLA_SEQUENTIAL)
    {
        index = 64-bit_len-bit_start;
        mask = (u32)((0x1ULL << bit_len)-1);
        long_data.all = value & mask;
        long_data.all <<= index;

        frame_data[7] |= long_data.bit.BYTE0;
        frame_data[6] |= long_data.bit.BYTE1;
        frame_data[5] |= long_data.bit.BYTE2;
        frame_data[4] |= long_data.bit.BYTE3;
        frame_data[3] |= long_data.bit.BYTE4;
        frame_data[2] |= long_data.bit.BYTE5;
        frame_data[1] |= long_data.bit.BYTE6;
        frame_data[0] |= long_data.bit.BYTE7;
    }
    else if(format == MOTOROLA_BACKWARD) //normally not used any more.
    {

    }
}

__declspec(dllexport) void get_frame_data(u08 frame_data[], CAN_SIG_FORMAT format, u16 bit_start, u16 bit_len, u32* value)
{
	u32 index, mask;
	TU_UINT64_DATA long_data;

    if(format == INTEL_STANDARD)
    {
        long_data.bit.BYTE0 = frame_data[0];
        long_data.bit.BYTE1 = frame_data[1];
        long_data.bit.BYTE2 = frame_data[2];
        long_data.bit.BYTE3 = frame_data[3];
        long_data.bit.BYTE4 = frame_data[4];
        long_data.bit.BYTE5 = frame_data[5];
        long_data.bit.BYTE6 = frame_data[6];
        long_data.bit.BYTE7 = frame_data[7];

        index = bit_start;
        mask = (u32)((0x1ULL << bit_len)-1);
        long_data.all >>= index;
        *value = (u32)(long_data.all & mask);
    }
    if(format == INTEL_SEQUENTIAL)
    {
        long_data.bit.BYTE0 = frame_data[0];
        long_data.bit.BYTE1 = frame_data[1];
        long_data.bit.BYTE2 = frame_data[2];
        long_data.bit.BYTE3 = frame_data[3];
        long_data.bit.BYTE4 = frame_data[4];
        long_data.bit.BYTE5 = frame_data[5];
        long_data.bit.BYTE6 = frame_data[6];
        long_data.bit.BYTE7 = frame_data[7];

        index = bit_start^0x7;
        mask = (u32)((0x1ULL << bit_len)-1);
        long_data.all >>= index;
        *value = (u32)(long_data.all & mask);
    }
    else if(format == MOTOROLA_LSB)
    {
        long_data.bit.BYTE0 = frame_data[7];
        long_data.bit.BYTE1 = frame_data[6];
        long_data.bit.BYTE2 = frame_data[5];
        long_data.bit.BYTE3 = frame_data[4];
        long_data.bit.BYTE4 = frame_data[3];
        long_data.bit.BYTE5 = frame_data[2];
        long_data.bit.BYTE6 = frame_data[1];
        long_data.bit.BYTE7 = frame_data[0];

        index = bit_start^0x38;
        mask = (u32)((0x1ULL << bit_len)-1);
        long_data.all >>= index;
        *value = (u32)(long_data.all & mask);
    }
    else if(format == MOTOROLA_MSB)
    {
        long_data.bit.BYTE0 = frame_data[7];
        long_data.bit.BYTE1 = frame_data[6];
        long_data.bit.BYTE2 = frame_data[5];
        long_data.bit.BYTE3 = frame_data[4];
        long_data.bit.BYTE4 = frame_data[3];
        long_data.bit.BYTE5 = frame_data[2];
        long_data.bit.BYTE6 = frame_data[1];
        long_data.bit.BYTE7 = frame_data[0];

        //index = 57 + (bit_start%8) - (bit_start/8 * 8) - bit_len;
        index = ((bit_start^0x7)+bit_len-1)^0x3F;
        mask = (u32)((0x1ULL << bit_len)-1);
        long_data.all >>= index;
        *value = (u32)(long_data.all & mask);
    }
    else if(format == MOTOROLA_SEQUENTIAL)
    {
        long_data.bit.BYTE0 = frame_data[7];
        long_data.bit.BYTE1 = frame_data[6];
        long_data.bit.BYTE2 = frame_data[5];
        long_data.bit.BYTE3 = frame_data[4];
        long_data.bit.BYTE4 = frame_data[3];
        long_data.bit.BYTE5 = frame_data[2];
        long_data.bit.BYTE6 = frame_data[1];
        long_data.bit.BYTE7 = frame_data[0];

        index = 64-bit_len-bit_start;
        mask = (u32)((0x1ULL << bit_len)-1);
        long_data.all >>= index;
        *value = (u32)(long_data.all & mask);
    }
}

#define ONE_BYTE_BITS 8

typedef struct{
    u32 mask;
    u08 byte;
    u08 bit;
    u08 num;
}TS_CANFD_FRAME_PARA;

static void setPara(TS_CANFD_FRAME_PARA * para, u16 bit_start, u16 bit_len)
{
    para->byte = bit_start / ONE_BYTE_BITS;
    para->bit = bit_start % ONE_BYTE_BITS;    
    para->num = (bit_len + para->bit) / ONE_BYTE_BITS;

    if ((bit_len + para->bit) % ONE_BYTE_BITS)
    {
        para->num += 1;
    }

    para->mask = (u32)((0x1ULL << bit_len) - 1);
}

static u08 getData(u32 * value, int i, u08 byte, u08 bit)
{     
    u32 data = *value;
    
    if (i == byte)/* first byte */
    {
        *value >>= (ONE_BYTE_BITS - bit);
        return ((data << bit) & 0x00FFU);
    }
    else/* other bytes */
    {
        *value >>= ONE_BYTE_BITS;
        return (data & 0x00FFU);
    }
}

static u32 setData(u08 frame_data, int i, u08 byte, u08 * bit)
{     
    u32 bitData = *bit;
    
    if (i == byte)/* first byte */
    {
        *bit = (ONE_BYTE_BITS - *bit);
        return ((frame_data & 0x00FFU) >> bitData);
    }
    else/* other bytes */
    {
        *bit += ONE_BYTE_BITS;
        return (frame_data & 0x00FFU) << bitData;
    }
}

__declspec(dllexport) void set_frame_dataFD(u08 frame_data[], CAN_SIG_FORMAT format, u16 bit_start, u16 bit_len, u32 value)
{
    u32 ulWriterData = 0;
    TS_CANFD_FRAME_PARA para = {0};

    setPara(&para, bit_start, bit_len);
    
    ulWriterData = value & para.mask;

    if(format == INTEL_STANDARD)
    {
        for (int i = para.byte; (i < para.byte + para.num) && (i < 64); i++)
        {
            frame_data[i] |= getData(&ulWriterData, i, para.byte, para.bit);
        }   
    }
    else if(format == INTEL_SEQUENTIAL)
    {
    }
    else if(format == MOTOROLA_LSB)
    {
        u08 size = 0;
        if(para.byte > para.num)
        {
            size = para.byte - para.num + 1;
        }
        
        for (int i = para.byte; i >= size; i--)
        {
            frame_data[i] |= getData(&ulWriterData, i, para.byte, para.bit);
        }   
    }
    else if(format == MOTOROLA_MSB)
    {
    }
    else if(format == MOTOROLA_SEQUENTIAL)
    {
    }
    else if(format == MOTOROLA_BACKWARD) //normally not used any more.
    {
    }
}

__declspec(dllexport) void get_frame_dataFD(u08 frame_data[], CAN_SIG_FORMAT format, u16 bit_start, u16 bit_len, u32* value)
{
    u32 ulWriterData = 0;
    TS_CANFD_FRAME_PARA para = {0};

    setPara(&para, bit_start, bit_len);

    if(format == INTEL_STANDARD)
    {
        for (int i = para.byte; (i < para.byte + para.num) && (i < 64); i++)
        {
            ulWriterData |= setData(frame_data[i], i, para.byte, &para.bit);
        } 
    }
    if(format == INTEL_SEQUENTIAL)
    {
    }
    else if(format == MOTOROLA_LSB)
    {
        u08 size = 0;
        if(para.byte > para.num)
        {
            size = para.byte - para.num + 1;
        }

        for (int i = para.byte; i >= size; i--)
        {
            ulWriterData |= setData(frame_data[i], i, para.byte, &para.bit);
        } 
    }
    else if(format == MOTOROLA_MSB)
    {
    }
    else if(format == MOTOROLA_SEQUENTIAL)
    {
    }

    *value = ulWriterData & para.mask;
}

/******** (C) Copyright, Shenzhen SHINRY Technology Co.,Ltd. ******** End *****/

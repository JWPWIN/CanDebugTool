using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public enum CAN_SIG_FORMAT
{
    INTEL_STANDARD,
    INTEL_SEQUENTIAL,
    MOTOROLA_LSB,
    MOTOROLA_MSB,
    MOTOROLA_BACKWARD,
    MOTOROLA_SEQUENTIAL
};

static public class CanBitLibTool
{
    //CAN类型报文接口
    [DllImport("CanBitLibInterface.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void set_frame_data(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len, uint value);

    [DllImport("CanBitLibInterface.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void get_frame_data(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len,  ref uint value);

    //CANFD类型报文接口
    [DllImport("CanBitLibInterface.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void set_frame_dataFD(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len, uint value);

    [DllImport("CanBitLibInterface.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void get_frame_dataFD(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len, ref uint value);

    //CAN帧设置信号到报文帧数据中
    static void CAN_set_frame_data(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len, uint value)
    {
        set_frame_data(frame_data, format, bit_start, bit_len, value);
    }

    //CAN帧从报文帧数据中获取信号值
    static uint CAN_get_frame_data(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len)
    {
        uint value = 0;
        get_frame_data(frame_data, format, bit_start, bit_len, ref value);

        return value;
    }

    //CANFD帧设置信号到报文帧数据中
    static void CAN_set_frame_dataFD(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len, uint value)
    {
        set_frame_dataFD(frame_data, format, bit_start, bit_len, value);
    }

    //CANFD帧从报文帧数据中获取信号值
    static uint CAN_get_frame_dataFD(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len)
    {
        uint value = 0;
        get_frame_dataFD(frame_data, format, bit_start, bit_len, ref value);

        return value;
    }

}

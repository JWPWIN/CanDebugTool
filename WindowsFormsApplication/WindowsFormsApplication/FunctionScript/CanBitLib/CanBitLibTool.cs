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
    unsafe static extern void get_frame_data(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len, uint *pValue);

    //CANFD类型报文接口
    [DllImport("CanBitLibInterface.dll", CallingConvention = CallingConvention.StdCall)]
    static extern void set_frame_dataFD(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len, uint value);

    [DllImport("CanBitLibInterface.dll", CallingConvention = CallingConvention.StdCall)]
    unsafe static extern void get_frame_dataFD(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len, uint *pValue);



    //CAN帧设置信号到报文帧数据中
    static public void CAN_set_frame_data(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len, uint value)
    {
        set_frame_data(frame_data, format, bit_start, bit_len, value);
    }

    //CAN帧从报文帧数据中获取信号值
    static public uint CAN_get_frame_data(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len)
    {
        uint value = 0;

        unsafe
        {
            uint* pValue = &value;
            get_frame_data(frame_data, format, bit_start, bit_len, pValue);
        }

        return value;
    }

    //CANFD帧设置信号到报文帧数据中
    static public void CAN_set_frame_dataFD(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len, uint value)
    {
        set_frame_dataFD(frame_data, format, bit_start, bit_len, value);
    }

    //CANFD帧从报文帧数据中获取信号值
    unsafe static public uint CAN_get_frame_dataFD(byte[] frame_data, CAN_SIG_FORMAT format, ushort bit_start, ushort bit_len)
    {
        uint value = 0;
        byte[] _tmpData = new byte[64];
        for (int i = 0; i < frame_data.Length; i++)
        {
            _tmpData[i] = frame_data[i];
        }

        get_frame_dataFD(_tmpData, format, bit_start, bit_len, &value);

        return value;
    }

}

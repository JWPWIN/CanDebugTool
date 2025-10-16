using System;

static public class CanOrderTool
{
    /// <summary>
    /// Motorola格式通过LSB起始位计算MSB起始位
    /// </summary>
    /// <param name="startBit_Lsb">Lsb开始位</param>
    /// <param name="length">信号长度</param>
    /// <returns>Msb开始位</returns>
    static public int MotorolaStartBit_Lsb2Msb(int startBit_Lsb, int length)
    {
        int startBit_Msb = startBit_Lsb;

        while (length > 1)
        {
            length--;
            //如果处于当前字节Bit7，则下一次Msb位跳到上一个字节Bit0
            if ((startBit_Msb + 1) % 8 == 0)
            {
                if ((startBit_Msb - 15) >= 0)
                {
                    startBit_Msb = startBit_Msb - 15;
                }
                else
                {
                    //如果上一个字节Bit0小于0则退出
                    break;
                }
            }
            else//如果未处于当前字节Bit7，则下一次Msb位+1
            {
                startBit_Msb++;
            }
        }

        return startBit_Msb;
    }

    /// <summary>
    /// Motorola格式通过MSB起始位计算LSB起始位
    /// </summary>
    /// <param name="startBit_Msb">Msb开始位</param>
    /// <param name="length">信号长度</param>
    /// <returns>Lsb开始位</returns>
    static public int MotorolaStartBit_Msb2Lsb(int startBit_Msb, int length)
    {
        int startBit_Lsb = startBit_Msb;

        while (length > 1)
        {
            length--;
            //如果处于当前字节Bit7，则下一次Lsb位跳到下一个字节Bit7
            if (startBit_Lsb % 8 == 0)
            {
                if ((startBit_Lsb + 15) <= 63)//8字节报文格式，可修改
                {
                    startBit_Lsb = startBit_Lsb + 15;
                }
                else
                {
                    //如果下一个字节Bit7大于63则退出
                    break;
                }
            }
            else//如果未处于当前字节Bit0，则下一次Lsb位-1
            {
                startBit_Lsb--;
            }
        }

        return startBit_Lsb;
    }
}
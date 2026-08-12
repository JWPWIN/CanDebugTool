using System;

static public class CanOrderTool
{
    /// <summary>CAN FD 最大载荷 64 字节时的最高位索引。</summary>
    public const uint MaxStartBitInclusive = 511;

    /// <summary>
    /// Motorola格式通过LSB起始位计算MSB起始位
    /// </summary>
    /// <param name="startBit_Lsb">Lsb开始位</param>
    /// <param name="length">信号长度</param>
    /// <returns>Msb开始位</returns>
    static public uint MotorolaStartBit_Lsb2Msb(uint startBit_Lsb, uint length)
    {
        uint startBit_Msb = startBit_Lsb;

        while (length > 1)
        {
            length--;
            //如果处于当前字节Bit7，则下一次Msb位跳到上一个字节Bit0
            if ((startBit_Msb + 1) % 8 == 0)
            {
                if (startBit_Msb >= 15)
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
    static public uint MotorolaStartBit_Msb2Lsb(uint startBit_Msb, uint length)
    {
        uint startBit_Lsb = startBit_Msb;

        while (length > 1)
        {
            length--;
            //如果处于当前字节Bit0，则下一次Lsb位跳到下一个字节Bit7
            if (startBit_Lsb % 8 == 0)
            {
                if ((startBit_Lsb + 15) <= MaxStartBitInclusive)
                {
                    startBit_Lsb = startBit_Lsb + 15;
                }
                else
                {
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

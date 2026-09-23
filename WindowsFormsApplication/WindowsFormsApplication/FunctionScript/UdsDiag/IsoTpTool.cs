using System;

/// <summary>
/// ISO 15765-2 传输层：经典 CAN（8 字节）与 CAN FD（最长 64）的 PCI 编解码。
/// </summary>
public static class IsoTpTool
{
    public const int MaxClassicDlc = 8;
    public const int MaxFdDlc = 64;
    public const int MaxPduLength = 4095;

    public enum PciKind
    {
        Single = 0,
        First = 1,
        Consecutive = 2,
        FlowControl = 3,
        Unknown = 0xF
    }

    public struct ParsedPci
    {
        public PciKind Kind;
        public int TotalLength;
        public int Seq;
        public byte FlowStatus;
        public byte BlockSize;
        public byte StMin;
        public int DataOffset;
        public int DataCount;
    }

    public static uint NormalizeCanId(uint rawId) => rawId & 0x1FFFFFFFu;

    public static byte ClampMaxDlc(bool isCanFd, byte requested)
    {
        int cap = isCanFd ? MaxFdDlc : MaxClassicDlc;
        int dlc = requested == 0 ? cap : requested;
        if (dlc > cap) dlc = cap;
        return RoundDlc(dlc, isCanFd);
    }

    public static byte RoundDlc(int length, bool isCanFd)
    {
        if (length <= 0) return 0;
        if (length <= 8) return (byte)length;
        if (!isCanFd) return MaxClassicDlc;

        if (length <= 12) return 12;
        if (length <= 16) return 16;
        if (length <= 20) return 20;
        if (length <= 24) return 24;
        if (length <= 32) return 32;
        if (length <= 48) return 48;
        return MaxFdDlc;
    }

    public static int SingleFrameMaxData(byte maxDlc)
    {
        if (maxDlc <= 8)
            return Math.Max(0, maxDlc - 1);
        return Math.Max(0, maxDlc - 2);
    }

    public static int FirstFrameMaxData(byte maxDlc)
    {
        return Math.Max(0, maxDlc - 2);
    }

    public static int ConsecutiveFrameMaxData(byte maxDlc)
    {
        return Math.Max(0, maxDlc - 1);
    }

    public static ulong StMinToMicroseconds(byte stMin)
    {
        if (stMin <= 0x7F)
            return (ulong)stMin * (ulong)TimeUnit.T_MS;
        if (stMin >= 0xF1 && stMin <= 0xF9)
            return (ulong)(stMin - 0xF0) * 100UL;
        return 0;
    }

    public static Canfd_Frame_Com CreateFrame(uint canId, bool isCanFd, byte maxDlc, byte[] pciAndData, byte? padByte)
    {
        var frame = new Canfd_Frame_Com
        {
            can_id = NormalizeCanId(canId),
            is_canfd = (byte)(isCanFd ? 1 : 0),
            data = new byte[64]
        };

        int used = pciAndData.Length;
        if (used > 64)
            used = 64;
        Array.Copy(pciAndData, frame.data, used);

        byte dlc;
        if (padByte.HasValue)
        {
            dlc = ClampMaxDlc(isCanFd, maxDlc);
            for (int i = used; i < dlc; i++)
                frame.data[i] = padByte.Value;
        }
        else
        {
            dlc = RoundDlc(used, isCanFd);
        }

        frame.len = dlc;
        return frame;
    }

    public static Canfd_Frame_Com BuildSingleFrame(
        uint canId, bool isCanFd, byte maxDlc, byte[] payload, byte? padByte)
    {
        maxDlc = ClampMaxDlc(isCanFd, maxDlc);
        int n = payload.Length;
        byte[] pci;
        byte frameMaxDlc;

        // 7 字节及以内：始终用经典单帧 PCI（0x0N）且 DLC=8。
        // CAN FD 长单帧（00 SF_DL + DLC>8）多数 ECU 的 UDS 栈不认，CANoe 等工具默认也不这样发。
        if (n <= 7)
        {
            pci = new byte[1 + n];
            pci[0] = (byte)(n & 0x0F);
            Array.Copy(payload, 0, pci, 1, n);
            frameMaxDlc = MaxClassicDlc;
        }
        else
        {
            pci = new byte[2 + n];
            pci[0] = 0x00;
            pci[1] = (byte)n;
            Array.Copy(payload, 0, pci, 2, n);
            frameMaxDlc = maxDlc;
        }

        return CreateFrame(canId, isCanFd, frameMaxDlc, pci, padByte);
    }

    public static Canfd_Frame_Com BuildFirstFrame(
        uint canId, bool isCanFd, byte maxDlc, byte[] payload, byte? padByte, out int consumed)
    {
        maxDlc = ClampMaxDlc(isCanFd, maxDlc);
        int total = payload.Length;
        int dataMax = FirstFrameMaxData(maxDlc);
        consumed = Math.Min(dataMax, total);
        byte[] pci = new byte[2 + consumed];
        pci[0] = (byte)(0x10 | ((total >> 8) & 0x0F));
        pci[1] = (byte)(total & 0xFF);
        Array.Copy(payload, 0, pci, 2, consumed);
        return CreateFrame(canId, isCanFd, maxDlc, pci, padByte);
    }

    public static Canfd_Frame_Com BuildConsecutiveFrame(
        uint canId, bool isCanFd, byte maxDlc, byte[] payload, int offset, int seq, byte? padByte, out int consumed)
    {
        maxDlc = ClampMaxDlc(isCanFd, maxDlc);
        int dataMax = ConsecutiveFrameMaxData(maxDlc);
        consumed = Math.Min(dataMax, payload.Length - offset);
        if (consumed < 0) consumed = 0;
        byte[] pci = new byte[1 + consumed];
        pci[0] = (byte)(0x20 | (seq & 0x0F));
        if (consumed > 0)
            Array.Copy(payload, offset, pci, 1, consumed);
        return CreateFrame(canId, isCanFd, maxDlc, pci, padByte);
    }

    public static Canfd_Frame_Com BuildFlowControl(
        uint canId, bool isCanFd, byte maxDlc, byte flowStatus, byte blockSize, byte stMin, byte? padByte)
    {
        maxDlc = ClampMaxDlc(isCanFd, maxDlc);
        byte[] pci = { (byte)(0x30 | (flowStatus & 0x0F)), blockSize, stMin };
        return CreateFrame(canId, isCanFd, maxDlc, pci, padByte);
    }

    public static bool TryParse(byte[] data, byte dlc, out ParsedPci pci)
    {
        pci = default;
        pci.Kind = PciKind.Unknown;
        if (data is null || dlc < 1)
            return false;

        int n = Math.Min(dlc, data.Length);
        byte b0 = data[0];
        int type = b0 >> 4;

        switch (type)
        {
            case 0:
                pci.Kind = PciKind.Single;
                if (n >= 2 && b0 == 0x00)
                {
                    pci.TotalLength = data[1];
                    pci.DataOffset = 2;
                }
                else
                {
                    pci.TotalLength = b0 & 0x0F;
                    pci.DataOffset = 1;
                }
                pci.DataCount = Math.Max(0, Math.Min(pci.TotalLength, n - pci.DataOffset));
                return pci.TotalLength >= 0 && pci.DataOffset <= n;

            case 1:
                pci.Kind = PciKind.First;
                if (n < 2) return false;
                pci.TotalLength = ((b0 & 0x0F) << 8) | data[1];
                pci.DataOffset = 2;
                pci.DataCount = Math.Max(0, n - 2);
                return pci.TotalLength > 0;

            case 2:
                pci.Kind = PciKind.Consecutive;
                pci.Seq = b0 & 0x0F;
                pci.DataOffset = 1;
                pci.DataCount = Math.Max(0, n - 1);
                return true;

            case 3:
                pci.Kind = PciKind.FlowControl;
                pci.FlowStatus = (byte)(b0 & 0x0F);
                pci.BlockSize = n > 1 ? data[1] : (byte)0;
                pci.StMin = n > 2 ? data[2] : (byte)0;
                pci.DataOffset = 3;
                pci.DataCount = 0;
                return true;

            default:
                return false;
        }
    }

    public static byte[] CopyPayload(byte[] data, int offset, int count)
    {
        if (count <= 0)
            return Array.Empty<byte>();
        if (data is null || offset >= data.Length)
            return Array.Empty<byte>();
        int n = Math.Min(count, data.Length - offset);
        byte[] part = new byte[n];
        Array.Copy(data, offset, part, 0, n);
        return part;
    }

    public static Canfd_Frame_Com CloneFrame(Canfd_Frame_Com src)
    {
        var dst = src;
        dst.data = new byte[64];
        if (src.data is not null)
        {
            int n = Math.Min(64, src.data.Length);
            Array.Copy(src.data, dst.data, n);
        }
        dst.can_id = NormalizeCanId(src.can_id);
        return dst;
    }
}

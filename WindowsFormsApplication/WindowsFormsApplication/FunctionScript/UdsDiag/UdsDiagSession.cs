using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 诊断会话：在通信线程上做 ISO-TP 收发与超时，UI 只启动/取消并读取结果。
/// </summary>
public class UdsDiagSession
{
    public enum Phase
    {
        Idle,
        WaitFlowControl,
        SendingCf,
        WaitResponse,
        Failed,
        Completed
    }

    public sealed class Request
    {
        public uint ReqId;
        public uint RespId;
        public bool IsCanFd;
        public byte MaxDlc = 8;
        public byte? PadByte = 0x00;
        public int TimeoutMs = 2000;
        public byte[] Payload = Array.Empty<byte>();
    }

    public sealed class Result
    {
        public bool Ok;
        public byte[] RequestPayload = Array.Empty<byte>();
        public byte[] Payload = Array.Empty<byte>();
        public string Status = string.Empty;
        public string Decode = string.Empty;
        public string Trace = string.Empty;
    }

    private readonly object _sync = new object();
    private readonly Queue<Canfd_Frame_Com> _rxQueue = new Queue<Canfd_Frame_Com>();
    private readonly List<string> _trace = new List<string>();
    private readonly List<byte> _rxPayload = new List<byte>(64);

    private Phase _phase = Phase.Idle;
    private Request _req;
    private byte[] _txPayload = Array.Empty<byte>();
    private int _txOffset;
    private int _txSeq;
    private int _blockRemain;
    private ulong _stMinUs;
    private ulong _nextCfDueUs;
    private ulong _deadlineUs;
    private ulong _pendingLimitUs;
    private int _rxExpected;
    private int _rxSeq;
    private Result _pendingResult;
    private string _status = "空闲";

    public bool IsBusy
    {
        get { lock (_sync) return _phase is Phase.WaitFlowControl or Phase.SendingCf or Phase.WaitResponse; }
    }

    public string StatusText
    {
        get { lock (_sync) return _status; }
    }

    /// <summary>会话线程：诊断响应 ID 的每一帧都入队（不要按 ID 合并）。</summary>
    public void CaptureRx(Canfd_Frame_Com frame)
    {
        lock (_sync)
        {
            if (!IsBusyUnlocked())
                return;
            if (_req is null)
                return;
            if (IsoTpTool.NormalizeCanId(frame.can_id) != _req.RespId)
                return;
            _rxQueue.Enqueue(IsoTpTool.CloneFrame(frame));
        }
    }

    public bool TryStart(Request request, DeviceInterfaceMng device, out string error)
    {
        error = null;
        if (request is null || request.Payload is null || request.Payload.Length == 0)
        {
            error = "请求数据为空";
            return false;
        }
        if (request.Payload.Length > IsoTpTool.MaxPduLength)
        {
            error = "请求数据超过 ISO-TP 12 位长度上限（4095 字节）";
            return false;
        }
        if (device is null || !device.canDeviceOpenFlag)
        {
            error = "请先连接 CAN 设备";
            return false;
        }

        lock (_sync)
        {
            if (IsBusyUnlocked())
            {
                error = "上一次诊断请求尚未结束";
                return false;
            }

            _req = request;
            _req.ReqId = IsoTpTool.NormalizeCanId(request.ReqId);
            _req.RespId = IsoTpTool.NormalizeCanId(request.RespId);
            _req.MaxDlc = IsoTpTool.ClampMaxDlc(request.IsCanFd, request.MaxDlc);
            _txPayload = request.Payload;
            _txOffset = 0;
            _txSeq = 1;
            _rxPayload.Clear();
            _rxQueue.Clear();
            _trace.Clear();
            _pendingResult = null;
            _rxExpected = 0;
            _rxSeq = 1;
            _blockRemain = 0;
            _stMinUs = 0;
            _nextCfDueUs = 0;

            ulong now = TimerTool.GetSysTime();
            _deadlineUs = now + (ulong)Math.Max(200, request.TimeoutMs) * (ulong)TimeUnit.T_MS;
            _pendingLimitUs = now + 30UL * (ulong)TimeUnit.T_S;

            int sfMax = IsoTpTool.SingleFrameMaxData(_req.MaxDlc);
            bool useSingle = _txPayload.Length <= 7
                || (_req.MaxDlc > 8 && _txPayload.Length <= sfMax);
            if (useSingle)
            {
                var sf = IsoTpTool.BuildSingleFrame(_req.ReqId, _req.IsCanFd, _req.MaxDlc, _txPayload, _req.PadByte);
                device.AddOneMsgToSend(sf);
                _phase = Phase.WaitResponse;
                _status = "已发送单帧，等待响应";
                AddTrace("TX SF " + DescribeFrame(sf) + "  PDU=" + UdsServiceDecode.FormatHex(_txPayload));
            }
            else
            {
                var ff = IsoTpTool.BuildFirstFrame(_req.ReqId, _req.IsCanFd, _req.MaxDlc, _txPayload, _req.PadByte, out int consumed);
                _txOffset = consumed;
                device.AddOneMsgToSend(ff);
                _phase = Phase.WaitFlowControl;
                _status = "已发送首帧，等待流控";
                AddTrace("TX FF len=" + _txPayload.Length + " " + DescribeFrame(ff));
            }
        }

        return true;
    }

    /// <summary>
    /// 发送单帧且不等待响应（Tester Present 3E 80 等）。会话忙时返回 false。
    /// </summary>
    public bool TrySendNoWait(Request request, DeviceInterfaceMng device, out string error)
    {
        error = null;
        if (device is null || !device.canDeviceOpenFlag)
        {
            error = "请先连接 CAN 设备";
            return false;
        }
        if (request?.Payload is null || request.Payload.Length == 0)
        {
            error = "请求数据为空";
            return false;
        }
        if (request.Payload.Length > 7)
        {
            error = "不等待发送仅支持不超过 7 字节的单帧";
            return false;
        }

        lock (_sync)
        {
            if (IsBusyUnlocked())
            {
                error = "上一次诊断请求尚未结束";
                return false;
            }

            var sf = IsoTpTool.BuildSingleFrame(
                IsoTpTool.NormalizeCanId(request.ReqId),
                request.IsCanFd,
                8,
                request.Payload,
                request.PadByte ?? (byte)0x00);
            device.AddOneMsgToSend(sf);
        }

        return true;
    }

    public void Cancel()
    {
        lock (_sync)
        {
            if (!IsBusyUnlocked())
                return;
            FinishLocked(false, Array.Empty<byte>(), "已取消");
        }
    }

    /// <summary>通信会话 1ms：处理接收队列、流控/连续帧与超时。</summary>
    public void Tick(DeviceInterfaceMng device)
    {
        if (device is null)
            return;

        lock (_sync)
        {
            if (!IsBusyUnlocked())
            {
                _rxQueue.Clear();
                return;
            }

            ulong now = TimerTool.GetSysTime();
            if (now > _deadlineUs)
            {
                FinishLocked(false, Array.Empty<byte>(), "等待诊断响应超时");
                return;
            }

            while (_rxQueue.Count > 0)
                HandleRxFrame(device, _rxQueue.Dequeue());

            if (_phase == Phase.SendingCf)
                SendDueConsecutive(device, now);
        }
    }

    public bool TryTakeResult(out Result result)
    {
        lock (_sync)
        {
            result = _pendingResult;
            _pendingResult = null;
            return result is not null;
        }
    }

    private bool IsBusyUnlocked()
    {
        return _phase is Phase.WaitFlowControl or Phase.SendingCf or Phase.WaitResponse;
    }

    private void HandleRxFrame(DeviceInterfaceMng device, Canfd_Frame_Com frame)
    {
        if (!IsoTpTool.TryParse(frame.data, frame.len, out var pci))
        {
            AddTrace("RX 无法解析 PCI " + DescribeFrame(frame));
            return;
        }

        switch (pci.Kind)
        {
            case IsoTpTool.PciKind.FlowControl:
                HandleFlowControl(device, pci, frame);
                break;
            case IsoTpTool.PciKind.Single:
                HandleSingle(pci, frame);
                break;
            case IsoTpTool.PciKind.First:
                HandleFirst(device, pci, frame);
                break;
            case IsoTpTool.PciKind.Consecutive:
                HandleConsecutive(pci, frame);
                break;
        }
    }

    private void HandleFlowControl(DeviceInterfaceMng device, IsoTpTool.ParsedPci pci, Canfd_Frame_Com frame)
    {
        AddTrace("RX FC FS=" + pci.FlowStatus + " BS=" + pci.BlockSize + " STmin=0x" + pci.StMin.ToString("X2")
            + " " + DescribeFrame(frame));

        if (_phase != Phase.WaitFlowControl && _phase != Phase.SendingCf)
            return;

        if (pci.FlowStatus == 2)
        {
            FinishLocked(false, Array.Empty<byte>(), "ECU 流控溢出 (OVFLW)");
            return;
        }
        if (pci.FlowStatus == 1)
        {
            _status = "ECU 流控等待 (WAIT)";
            ExtendDeadlineFromNow(_req.TimeoutMs);
            return;
        }
        if (pci.FlowStatus != 0)
        {
            FinishLocked(false, Array.Empty<byte>(), "未知流控状态 FS=" + pci.FlowStatus);
            return;
        }

        _stMinUs = IsoTpTool.StMinToMicroseconds(pci.StMin);
        _blockRemain = pci.BlockSize;
        _nextCfDueUs = TimerTool.GetSysTime() + _stMinUs;
        _phase = Phase.SendingCf;
        _status = "收到流控，发送连续帧";
        SendDueConsecutive(device, TimerTool.GetSysTime());
    }

    private void SendDueConsecutive(DeviceInterfaceMng device, ulong now)
    {
        while (_phase == Phase.SendingCf && _txOffset < _txPayload.Length)
        {
            if (now < _nextCfDueUs)
                return;

            var cf = IsoTpTool.BuildConsecutiveFrame(
                _req.ReqId, _req.IsCanFd, _req.MaxDlc, _txPayload, _txOffset, _txSeq, _req.PadByte, out int consumed);
            device.AddOneMsgToSend(cf);
            AddTrace("TX CF SN=" + _txSeq + " " + DescribeFrame(cf));
            _txOffset += consumed;
            _txSeq = (_txSeq + 1) & 0x0F;
            _nextCfDueUs = now + _stMinUs;

            if (_blockRemain > 0)
            {
                _blockRemain--;
                if (_blockRemain == 0 && _txOffset < _txPayload.Length)
                {
                    _phase = Phase.WaitFlowControl;
                    _status = "已发完本块连续帧，等待下一流控";
                    return;
                }
            }
        }

        if (_txOffset >= _txPayload.Length)
        {
            _phase = Phase.WaitResponse;
            _status = "请求已发完，等待响应";
        }
    }

    private void HandleSingle(IsoTpTool.ParsedPci pci, Canfd_Frame_Com frame)
    {
        byte[] pdu = IsoTpTool.CopyPayload(frame.data, pci.DataOffset, pci.DataCount);
        AddTrace("RX SF " + DescribeFrame(frame) + "  PDU=" + UdsServiceDecode.FormatHex(pdu));
        OnCompletePdu(pdu);
    }

    private void HandleFirst(DeviceInterfaceMng device, IsoTpTool.ParsedPci pci, Canfd_Frame_Com frame)
    {
        _rxExpected = pci.TotalLength;
        _rxPayload.Clear();
        _rxPayload.AddRange(IsoTpTool.CopyPayload(frame.data, pci.DataOffset, pci.DataCount));
        _rxSeq = 1;
        AddTrace("RX FF len=" + _rxExpected + " " + DescribeFrame(frame));

        var fc = IsoTpTool.BuildFlowControl(_req.ReqId, _req.IsCanFd, 8, 0, 0, 0, _req.PadByte ?? (byte)0x00);
        device.AddOneMsgToSend(fc);
        AddTrace("TX FC CTS BS=0 STmin=0");
        _phase = Phase.WaitResponse;
        _status = "已回流控，接收连续帧";
        ExtendDeadlineFromNow(_req.TimeoutMs);
    }

    private void HandleConsecutive(IsoTpTool.ParsedPci pci, Canfd_Frame_Com frame)
    {
        if (_rxExpected <= 0)
        {
            AddTrace("RX CF 无对应首帧，忽略 " + DescribeFrame(frame));
            return;
        }
        if (pci.Seq != _rxSeq)
        {
            FinishLocked(false, Array.Empty<byte>(), "连续帧序号错误，期望 " + _rxSeq + " 实际 " + pci.Seq);
            return;
        }

        _rxPayload.AddRange(IsoTpTool.CopyPayload(frame.data, pci.DataOffset, pci.DataCount));
        _rxSeq = (_rxSeq + 1) & 0x0F;
        AddTrace("RX CF SN=" + pci.Seq + " got=" + _rxPayload.Count + "/" + _rxExpected);

        if (_rxPayload.Count >= _rxExpected)
        {
            byte[] pdu = _rxPayload.GetRange(0, _rxExpected).ToArray();
            _rxExpected = 0;
            _rxPayload.Clear();
            OnCompletePdu(pdu);
        }
    }

    private void OnCompletePdu(byte[] pdu)
    {
        if (UdsServiceDecode.IsPending(pdu))
        {
            AddTrace("NRC 0x78 pending");
            _status = "ECU 处理中（NRC 78），继续等待";
            _phase = Phase.WaitResponse;
            _rxExpected = 0;
            _rxPayload.Clear();
            ExtendDeadlineFromNow(_req.TimeoutMs);
            if (TimerTool.GetSysTime() > _pendingLimitUs)
                FinishLocked(false, pdu, "等待最终响应超时（多次 NRC 78）");
            return;
        }

        string decode = UdsServiceDecode.Describe(pdu);
        bool ok = pdu.Length > 0 && pdu[0] != UdsServiceDecode.NegativeSid;
        FinishLocked(ok, pdu, decode);
    }

    private void ExtendDeadlineFromNow(int timeoutMs)
    {
        ulong now = TimerTool.GetSysTime();
        _deadlineUs = now + (ulong)Math.Max(200, timeoutMs) * (ulong)TimeUnit.T_MS;
    }

    private void FinishLocked(bool ok, byte[] payload, string status)
    {
        _phase = ok ? Phase.Completed : Phase.Failed;
        _status = status;
        _rxQueue.Clear();
        AddTrace(ok ? "完成" : "结束: " + status);
        _pendingResult = new Result
        {
            Ok = ok,
            RequestPayload = _txPayload ?? Array.Empty<byte>(),
            Payload = payload ?? Array.Empty<byte>(),
            Status = status,
            Decode = payload is { Length: > 0 } ? UdsServiceDecode.Describe(payload) : status,
            Trace = string.Join(Environment.NewLine, _trace)
        };
    }

    private void AddTrace(string line)
    {
        if (_trace.Count > 80)
            _trace.RemoveAt(0);
        _trace.Add(line);
    }

    private static string DescribeFrame(Canfd_Frame_Com frame)
    {
        int n = Math.Min(frame.len, frame.data?.Length ?? 0);
        var sb = new StringBuilder();
        sb.Append("ID=0x").Append(IsoTpTool.NormalizeCanId(frame.can_id).ToString("X"));
        sb.Append(" DLC=").Append(frame.len);
        sb.Append(" [");
        for (int i = 0; i < n; i++)
        {
            if (i > 0) sb.Append(' ');
            sb.Append(frame.data[i].ToString("X2"));
        }
        sb.Append(']');
        return sb.ToString();
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

/// <summary>
/// UDS（ISO 14229）服务名与否定响应码的可读解析。
/// </summary>
public static class UdsServiceDecode
{
    public const byte NegativeSid = 0x7F;
    public const byte NrcPending = 0x78;

    public static bool TryParseHexId(string text, out uint id, out string error)
    {
        id = 0;
        error = null;
        if (string.IsNullOrWhiteSpace(text))
        {
            error = "CAN ID 为空";
            return false;
        }

        string t = text.Trim();
        if (t.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            t = t.Substring(2);
        if (t.Length == 0
            || !uint.TryParse(t, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out id))
        {
            error = "CAN ID 不是有效十六进制";
            return false;
        }
        if (id > 0x1FFFFFFF)
        {
            error = "CAN ID 超出 29 位范围";
            return false;
        }
        return true;
    }

    public static bool TryParseHexBytes(string text, out byte[] data, out string error)
    {
        data = Array.Empty<byte>();
        error = null;
        if (string.IsNullOrWhiteSpace(text))
        {
            error = "请求数据为空";
            return false;
        }

        var tokens = new List<string>();
        var cur = new StringBuilder();
        foreach (char c in text)
        {
            if (char.IsWhiteSpace(c) || c == ',' || c == ';' || c == '-' || c == ':')
            {
                if (cur.Length > 0)
                {
                    tokens.Add(cur.ToString());
                    cur.Clear();
                }
                continue;
            }
            cur.Append(c);
        }
        if (cur.Length > 0)
            tokens.Add(cur.ToString());

        if (tokens.Count == 0)
        {
            error = "请求数据为空";
            return false;
        }

        var bytes = new List<byte>();
        if (tokens.Count == 1)
        {
            string blob = Strip0x(tokens[0]);
            if (blob.Length == 0 || (blob.Length % 2) != 0)
            {
                error = "十六进制字节长度必须为偶数";
                return false;
            }
            for (int i = 0; i < blob.Length; i += 2)
            {
                if (!byte.TryParse(blob.AsSpan(i, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte b))
                {
                    error = "请求数据含无效十六进制";
                    return false;
                }
                bytes.Add(b);
            }
        }
        else
        {
            foreach (string token in tokens)
            {
                string t = Strip0x(token);
                if (t.Length == 0 || t.Length > 2
                    || !byte.TryParse(t, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte b))
                {
                    error = "请求数据含无效字节: " + token;
                    return false;
                }
                bytes.Add(b);
            }
        }

        data = bytes.ToArray();
        return true;
    }

    private static string Strip0x(string token)
    {
        if (token.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            return token.Substring(2);
        return token;
    }

    public static string FormatHex(byte[] data)
    {
        if (data is null || data.Length == 0)
            return string.Empty;
        var sb = new StringBuilder(data.Length * 3);
        for (int i = 0; i < data.Length; i++)
        {
            if (i > 0) sb.Append(' ');
            sb.Append(data[i].ToString("X2"));
        }
        return sb.ToString();
    }

    public static string Describe(byte[] payload)
    {
        if (payload is null || payload.Length == 0)
            return "空响应";

        if (payload[0] == NegativeSid)
        {
            if (payload.Length < 3)
                return "否定响应（长度不足）";

            byte sid = payload[1];
            byte nrc = payload[2];
            string pending = nrc == NrcPending ? "，ECU 仍在处理，等待最终响应" : string.Empty;
            return "否定响应  SID=" + SidName(sid) + " (0x" + sid.ToString("X2") + ")"
                + "  NRC=" + NrcName(nrc) + " (0x" + nrc.ToString("X2") + ")" + pending;
        }

        byte posSid = payload[0];
        byte reqSid = (byte)(posSid >= 0x40 ? posSid - 0x40 : posSid);
        var sb = new StringBuilder();
        sb.Append("肯定响应  ").Append(SidName(reqSid)).Append(" (0x").Append(posSid.ToString("X2")).Append(')');

        if (reqSid == 0x22 && payload.Length >= 3)
        {
            uint did = ((uint)payload[1] << 8) | payload[2];
            sb.Append("  DID=0x").Append(did.ToString("X4"));
            if (payload.Length > 3)
            {
                byte[] data = Slice(payload, 3);
                sb.Append("  数据=").Append(FormatHex(data));
                string ascii = TryAscii(data);
                if (!string.IsNullOrEmpty(ascii))
                    sb.Append("  ASCII=\"").Append(ascii).Append('"');
            }
        }
        else if (reqSid == 0x10 && payload.Length >= 2)
        {
            byte session = (byte)(payload[1] & 0x7F);
            sb.Append("  会话=").Append(SessionName(session)).Append(" (0x").Append(session.ToString("X2")).Append(')');
        }
        else if (reqSid == 0x11 && payload.Length >= 2)
        {
            sb.Append("  复位类型=0x").Append((payload[1] & 0x7F).ToString("X2"));
        }
        else if (reqSid == 0x2E && payload.Length >= 3)
        {
            uint did = ((uint)payload[1] << 8) | payload[2];
            sb.Append("  DID=0x").Append(did.ToString("X4"));
        }
        else if (reqSid == 0x19)
            sb.Append(DescribeDtcResponse(payload));
        else if (payload.Length > 1)
        {
            sb.Append("  数据=").Append(FormatHex(Slice(payload, 1)));
        }

        return sb.ToString();
    }

    public static string SessionName(byte session)
    {
        return session switch
        {
            0x01 => "默认会话",
            0x02 => "编程会话",
            0x03 => "扩展会话",
            0x04 => "安全系统会话",
            _ => "会话 0x" + session.ToString("X2")
        };
    }

    public static string TryAscii(byte[] data)
    {
        if (data is null || data.Length == 0)
            return null;

        var sb = new StringBuilder(data.Length);
        int printable = 0;
        foreach (byte b in data)
        {
            if (b == 0)
                break;
            if (b >= 0x20 && b <= 0x7E)
            {
                sb.Append((char)b);
                printable++;
            }
            else
                sb.Append('.');
        }

        if (printable < 2 || printable * 2 < sb.Length)
            return null;
        return sb.ToString().Trim();
    }

    private static string DescribeDtcResponse(byte[] payload)
    {
        if (payload.Length < 2)
            return string.Empty;

        byte sub = payload[1];
        var sb = new StringBuilder();
        sb.Append("  sub=0x").Append(sub.ToString("X2"));

        // 59 02 availMask {DTC[3] status}*
        if (sub == 0x02 && payload.Length >= 3)
        {
            sb.Append("  mask=0x").Append(payload[2].ToString("X2"));
            int count = (payload.Length - 3) / 4;
            sb.Append("  DTC数=").Append(count);
            int limit = Math.Min(count, 12);
            for (int i = 0; i < limit; i++)
            {
                int o = 3 + i * 4;
                sb.Append("  ").Append(FormatDtc(payload[o], payload[o + 1], payload[o + 2]))
                    .Append('[').Append(payload[o + 3].ToString("X2")).Append(']');
            }
            if (count > limit)
                sb.Append("  …");
        }
        else if (payload.Length > 2)
            sb.Append("  数据=").Append(FormatHex(Slice(payload, 2)));

        return sb.ToString();
    }

    public static string FormatDtc(byte b0, byte b1, byte b2)
    {
        char[] cat = { 'P', 'C', 'B', 'U' };
        char c = cat[(b0 >> 6) & 0x03];
        int n1 = (b0 >> 4) & 0x03;
        int n2 = b0 & 0x0F;
        return string.Concat(c, n1.ToString("X"), n2.ToString("X"), b1.ToString("X2"), b2.ToString("X2"));
    }

    public static bool IsPending(byte[] payload)
    {
        return payload is not null
            && payload.Length >= 3
            && payload[0] == NegativeSid
            && payload[2] == NrcPending;
    }

    public static string SidName(byte sid)
    {
        return sid switch
        {
            0x10 => "DiagnosticSessionControl",
            0x11 => "ECUReset",
            0x14 => "ClearDiagnosticInformation",
            0x19 => "ReadDTCInformation",
            0x22 => "ReadDataByIdentifier",
            0x23 => "ReadMemoryByAddress",
            0x27 => "SecurityAccess",
            0x28 => "CommunicationControl",
            0x2E => "WriteDataByIdentifier",
            0x2F => "InputOutputControlByIdentifier",
            0x31 => "RoutineControl",
            0x34 => "RequestDownload",
            0x35 => "RequestUpload",
            0x36 => "TransferData",
            0x37 => "RequestTransferExit",
            0x3D => "WriteMemoryByAddress",
            0x3E => "TesterPresent",
            0x85 => "ControlDTCSetting",
            _ => "Service 0x" + sid.ToString("X2")
        };
    }

    public static string NrcName(byte nrc)
    {
        return nrc switch
        {
            0x10 => "generalReject",
            0x11 => "serviceNotSupported",
            0x12 => "subFunctionNotSupported",
            0x13 => "incorrectMessageLengthOrInvalidFormat",
            0x14 => "responseTooLong",
            0x21 => "busyRepeatRequest",
            0x22 => "conditionsNotCorrect",
            0x24 => "requestSequenceError",
            0x25 => "noResponseFromSubnetComponent",
            0x26 => "failurePreventsExecutionOfRequestedAction",
            0x31 => "requestOutOfRange",
            0x33 => "securityAccessDenied",
            0x35 => "invalidKey",
            0x36 => "exceedNumberOfAttempts",
            0x37 => "requiredTimeDelayNotExpired",
            0x70 => "uploadDownloadNotAccepted",
            0x71 => "transferDataSuspended",
            0x72 => "generalProgrammingFailure",
            0x73 => "wrongBlockSequenceCounter",
            0x78 => "requestCorrectlyReceived-ResponsePending",
            0x7E => "subFunctionNotSupportedInActiveSession",
            0x7F => "serviceNotSupportedInActiveSession",
            0x81 => "rpmTooHigh",
            0x82 => "rpmTooLow",
            0x83 => "engineIsRunning",
            0x84 => "engineIsNotRunning",
            0x85 => "engineRunTimeTooLow",
            0x86 => "temperatureTooHigh",
            0x87 => "temperatureTooLow",
            0x88 => "vehicleSpeedTooHigh",
            0x89 => "vehicleSpeedTooLow",
            0x8A => "throttlePedalTooHigh",
            0x8B => "throttlePedalTooLow",
            0x8C => "transmissionRangeNotInNeutral",
            0x8D => "transmissionRangeNotInGear",
            0x8F => "brakeSwitchNotClosed",
            0x90 => "shifterLeverNotInPark",
            0x91 => "torqueConverterClutchLocked",
            0x92 => "voltageTooHigh",
            0x93 => "voltageTooLow",
            _ => "NRC 0x" + nrc.ToString("X2")
        };
    }

    private static byte[] Slice(byte[] data, int start)
    {
        if (start >= data.Length)
            return Array.Empty<byte>();
        int n = data.Length - start;
        byte[] part = new byte[n];
        Array.Copy(data, start, part, 0, n);
        return part;
    }
}

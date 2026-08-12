using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

/// <summary>
/// 将 DBC 文本解析为临时报文字典（不写单例），供复杂 OEM DBC 导入使用。
/// </summary>
public static class DbcMatrixParser
{
    public const uint MuxNone = 0;
    public const uint MuxSwitch = 1;
    public const uint MuxValue = 2;

    public static bool TryParse(string dbcInfo, out Dictionary<uint, CanMessage> parsed, out List<string> warnings)
    {
        parsed = new Dictionary<uint, CanMessage>();
        warnings = new List<string>();

        if (string.IsNullOrWhiteSpace(dbcInfo))
            return false;

        List<string> lines = DbcTextReader.EnumerateLogicalLines(dbcInfo);
        if (lines.Count < 3)
            return false;

        bool isMessageValid = false;
        uint lastMsgId = 0;
        bool lastMsgAccepted = false;

        for (int i = 0; i < lines.Count; i++)
        {
            List<string> tok = DbcTextReader.Tokenize(lines[i]);
            if (tok.Count < 1)
                continue;

            string head = tok[0];
            try
            {
                switch (head)
                {
                    case "BO_":
                        ParseBo_(tok, parsed, warnings, i + 1, ref isMessageValid, ref lastMsgId, ref lastMsgAccepted);
                        break;
                    case "SG_":
                        ParseSg_(tok, parsed, warnings, i + 1, isMessageValid, lastMsgAccepted, lastMsgId);
                        break;
                    case "CM_":
                        ParseCm_(tok, parsed, warnings, i + 1);
                        break;
                    case "VAL_":
                        ParseVal_(tok, parsed, warnings, i + 1);
                        break;
                    case "BA_":
                        ParseBa_(tok, parsed, warnings, i + 1);
                        break;
                    case "BU_:":
                    case "BU_":
                    case "VERSION":
                    case "NS_":
                    case "BS_":
                    case "BA_DEF_":
                    case "BA_DEF_DEF_":
                    case "SIG_GROUP_":
                    case "SIG_VALTYPE_":
                    case "BO_TX_BU_":
                    case "VAL_TABLE_":
                        // 已知但本产品不落库 / 忽略
                        break;
                    default:
                        // 未知关键字跳过
                        break;
                }
            }
            catch (Exception ex)
            {
                warnings.Add("行/语句 " + (i + 1) + " (" + head + ") 解析异常: " + ex.Message);
            }
        }

        return parsed.Count > 0;
    }

    private static void ParseBo_(
        List<string> tok,
        Dictionary<uint, CanMessage> parsed,
        List<string> warnings,
        int lineNo,
        ref bool isMessageValid,
        ref uint lastMsgId,
        ref bool lastMsgAccepted)
    {
        // BO_ <id> <Name>: <dlc> <tx>   或   BO_ <id> <Name> : <dlc> <tx>
        if (tok.Count < 5)
        {
            warnings.Add("行 " + lineNo + " BO_ 字段不足");
            isMessageValid = false;
            lastMsgAccepted = false;
            return;
        }

        if (!uint.TryParse(tok[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out uint rawId))
        {
            warnings.Add("行 " + lineNo + " BO_ ID 无效");
            isMessageValid = false;
            lastMsgAccepted = false;
            return;
        }

        if (rawId == 0xC0000000)
        {
            isMessageValid = false;
            lastMsgAccepted = false;
            return;
        }

        string msgName;
        uint dlc;
        string transmitter;
        int idx = 2;
        if (tok[idx].EndsWith(":", StringComparison.Ordinal))
        {
            msgName = tok[idx].Substring(0, tok[idx].Length - 1);
            idx++;
        }
        else if (idx + 1 < tok.Count && tok[idx + 1] == ":")
        {
            msgName = tok[idx];
            idx += 2;
        }
        else
        {
            msgName = tok[idx].TrimEnd(':');
            idx++;
        }

        if (idx + 1 >= tok.Count)
        {
            warnings.Add("行 " + lineNo + " BO_ 缺 DLC/发送节点");
            isMessageValid = false;
            lastMsgAccepted = false;
            return;
        }

        dlc = uint.Parse(tok[idx], CultureInfo.InvariantCulture);
        transmitter = tok[idx + 1];

        uint id = NormalizeDbcMsgId(rawId, out bool isExt);
        if (parsed.ContainsKey(id))
        {
            warnings.Add("行 " + lineNo + ": 重复报文 ID 0x" + id.ToString("X") + "，已跳过");
            isMessageValid = false;
            lastMsgAccepted = false;
            return;
        }

        var message = new CanMessage
        {
            msgId = id,
            isExtended = isExt,
            msgName = msgName,
            msgSize = dlc,
            transmitter = transmitter
        };
        parsed.Add(id, message);
        lastMsgId = id;
        isMessageValid = true;
        lastMsgAccepted = true;
    }

    private static void ParseSg_(
        List<string> tok,
        Dictionary<uint, CanMessage> parsed,
        List<string> warnings,
        int lineNo,
        bool isMessageValid,
        bool lastMsgAccepted,
        uint lastMsgId)
    {
        if (!isMessageValid || !lastMsgAccepted || !parsed.ContainsKey(lastMsgId))
            return;
        if (tok.Count < 5)
        {
            warnings.Add("行 " + lineNo + " SG_ 字段不足");
            return;
        }

        var signal = new CanSignal
        {
            msgId = lastMsgId,
            sigName = tok[1],
            sigValueTable = new Dictionary<int, string>()
        };

        int idx = 2;
        // mux: M / mN / 无
        if (tok[idx] != ":")
        {
            string mux = tok[idx];
            if (mux == "M")
            {
                signal.muxType = MuxSwitch;
            }
            else if (mux.Length > 1 && (mux[0] == 'm' || mux[0] == 'M'))
            {
                if (uint.TryParse(mux.Substring(1), NumberStyles.Integer, CultureInfo.InvariantCulture, out uint muxId))
                {
                    signal.muxType = MuxValue;
                    signal.reuseFrameID = muxId;
                }
            }
            idx++;
        }

        if (idx >= tok.Count || tok[idx] != ":")
        {
            // 允许 "name:" 粘连已在 Tokenize 外处理；此处缺冒号则失败
            warnings.Add("行 " + lineNo + " SG_ 缺少 ':'");
            return;
        }
        idx++; // skip :

        if (idx >= tok.Count)
        {
            warnings.Add("行 " + lineNo + " SG_ 缺少布局字段");
            return;
        }

        // start|len@orderSign
        string layout = tok[idx++];
        string[] sp = layout.Split(new[] { '|', '@' }, StringSplitOptions.RemoveEmptyEntries);
        if (sp.Length < 3)
        {
            warnings.Add("行 " + lineNo + " SG_ 起止位格式错误");
            return;
        }

        signal.sigLen = uint.Parse(sp[1], CultureInfo.InvariantCulture);
        if (sp[2][0] == '0')
        {
            signal.sigOrderType = 0;
            signal.sigStartBit = CanOrderTool.MotorolaStartBit_Msb2Lsb(
                uint.Parse(sp[0], CultureInfo.InvariantCulture), signal.sigLen);
        }
        else
        {
            signal.sigOrderType = 1;
            signal.sigStartBit = uint.Parse(sp[0], CultureInfo.InvariantCulture);
        }
        signal.valueType = (sp[2].Length > 1 && sp[2][1] == '-') ? 1u : 0u;

        if (idx >= tok.Count)
        {
            warnings.Add("行 " + lineNo + " SG_ 缺少 factor/offset");
            return;
        }

        // (factor,offset) — 允许括号内有空格被拆成多 token
        string fo = tok[idx];
        if (fo.StartsWith("(", StringComparison.Ordinal) && !fo.EndsWith(")", StringComparison.Ordinal))
        {
            var foSb = new StringBuilder(fo);
            while (idx + 1 < tok.Count && !foSb.ToString().EndsWith(")", StringComparison.Ordinal))
            {
                idx++;
                foSb.Append(tok[idx]);
            }
            fo = foSb.ToString();
        }
        idx++;
        fo = fo.Trim().Trim('(', ')');
        string[] foParts = fo.Split(',');
        if (foParts.Length < 2)
        {
            warnings.Add("行 " + lineNo + " SG_ factor/offset 格式错误");
            return;
        }
        signal.sigFactor = double.Parse(foParts[0].Trim(), CultureInfo.InvariantCulture);
        signal.sigOffset = double.Parse(foParts[1].Trim(), CultureInfo.InvariantCulture);

        // [min|max]
        if (idx < tok.Count && tok[idx].StartsWith("[", StringComparison.Ordinal))
        {
            string rangeTok = tok[idx];
            if (!rangeTok.EndsWith("]", StringComparison.Ordinal))
            {
                var rsb = new StringBuilder(rangeTok);
                while (idx + 1 < tok.Count && !rsb.ToString().EndsWith("]", StringComparison.Ordinal))
                {
                    idx++;
                    rsb.Append(tok[idx]);
                }
                rangeTok = rsb.ToString();
            }
            idx++;
            string range = rangeTok.Trim().Trim('[', ']');
            string[] mm = range.Split('|');
            if (mm.Length >= 2)
            {
                double.TryParse(mm[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out signal.sigMin);
                double.TryParse(mm[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out signal.sigMax);
            }
        }

        // unit（Tokenize 已去掉引号；"" → 空串）然后接收节点
        if (idx < tok.Count)
            signal.sigUnit = tok[idx++];

        if (idx < tok.Count)
        {
            var recvParts = new List<string>();
            for (; idx < tok.Count; idx++)
            {
                string part = tok[idx];
                if (string.IsNullOrWhiteSpace(part) || part == "Vector__XXX")
                    continue;
                recvParts.Add(part);
            }
            signal.recvNode = string.Join(",", recvParts);
        }

        parsed[lastMsgId].signals.Add(signal);
    }

    private static void ParseCm_(List<string> tok, Dictionary<uint, CanMessage> parsed, List<string> warnings, int lineNo)
    {
        // CM_ SG_ <id> <sig> <desc>
        // CM_ BO_ <id> <desc>
        if (tok.Count < 4) return;

        if (tok[1] == "SG_" && tok.Count >= 5)
        {
            uint id = NormalizeDbcMsgId(uint.Parse(tok[2], CultureInfo.InvariantCulture), out _);
            if (!parsed.TryGetValue(id, out CanMessage msg)) return;
            string sigName = tok[3];
            string desc = tok[4]; // 已是完整引号内容
            foreach (var s in msg.signals)
            {
                if (s.sigName == sigName)
                    s.sigDesc = desc;
            }
            return;
        }

        if (tok[1] == "BO_" && tok.Count >= 4)
        {
            uint id = NormalizeDbcMsgId(uint.Parse(tok[2], CultureInfo.InvariantCulture), out _);
            if (!parsed.TryGetValue(id, out CanMessage msg)) return;
            msg.msgDesc = tok[3];
        }
    }

    private static void ParseVal_(List<string> tok, Dictionary<uint, CanMessage> parsed, List<string> warnings, int lineNo)
    {
        // VAL_ <id> <sig> <v> <label> <v> <label> ...
        if (tok.Count < 5) return;
        uint id = NormalizeDbcMsgId(uint.Parse(tok[1], CultureInfo.InvariantCulture), out _);
        if (!parsed.TryGetValue(id, out CanMessage msg)) return;
        string sigName = tok[2];

        foreach (var s in msg.signals)
        {
            if (s.sigName != sigName) continue;
            s.sigValueTable ??= new Dictionary<int, string>();
            s.sigValueTable.Clear();
            for (int i = 3; i + 1 < tok.Count; i += 2)
            {
                if (!int.TryParse(tok[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out int key))
                    continue;
                s.sigValueTable[key] = tok[i + 1];
            }
            s.sigValueTable = s.sigValueTable.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            break;
        }
    }

    private static void ParseBa_(List<string> tok, Dictionary<uint, CanMessage> parsed, List<string> warnings, int lineNo)
    {
        // BA_ "Attr" BO_ <id> <value>
        if (tok.Count < 5) return;
        if (tok[2] != "BO_") return;

        string attr = tok[1];
        uint id = NormalizeDbcMsgId(uint.Parse(tok[3], CultureInfo.InvariantCulture), out _);
        if (!parsed.TryGetValue(id, out CanMessage msg)) return;

        string value = tok[4];
        if (attr == "GenMsgCycleTime")
        {
            if (uint.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out uint cycle))
                msg.msgCycle = cycle;
            return;
        }

        if (attr == "VFrameFormat")
        {
            ApplyVFrameFormat(msg, value);
        }
    }

    private static void ApplyVFrameFormat(CanMessage msg, string value)
    {
        string v = value.Trim().Trim('"');
        bool isExtended = msg.isExtended;
        bool isCanfd = msg.isCanfd;

        if (v == "0") { isExtended = false; isCanfd = false; }
        else if (v == "1") { isExtended = true; isCanfd = false; }
        else if (v == "14") { isExtended = false; isCanfd = true; }
        else if (v == "15") { isExtended = true; isCanfd = true; }
        else
        {
            string name = v.Replace("-", "").Replace(" ", "");
            if (name.Equals("StandardCAN", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("StandardCan", StringComparison.OrdinalIgnoreCase))
            { isExtended = false; isCanfd = false; }
            else if (name.Equals("ExtendedCAN", StringComparison.OrdinalIgnoreCase) ||
                     name.Equals("ExtendedCan", StringComparison.OrdinalIgnoreCase))
            { isExtended = true; isCanfd = false; }
            else if (name.Equals("StandardCAN_FD", StringComparison.OrdinalIgnoreCase) ||
                     name.Equals("StandardCANFD", StringComparison.OrdinalIgnoreCase) ||
                     name.Equals("StandardCan_FD", StringComparison.OrdinalIgnoreCase))
            { isExtended = false; isCanfd = true; }
            else if (name.Equals("ExtendedCAN_FD", StringComparison.OrdinalIgnoreCase) ||
                     name.Equals("ExtendedCANFD", StringComparison.OrdinalIgnoreCase) ||
                     name.Equals("ExtendedCan_FD", StringComparison.OrdinalIgnoreCase))
            { isExtended = true; isCanfd = true; }
        }

        msg.isExtended = isExtended;
        msg.isCanfd = isCanfd;
    }

    public static uint NormalizeDbcMsgId(uint rawId, out bool isExtended)
    {
        isExtended = (rawId & 0x80000000u) != 0;
        return rawId & 0x7FFFFFFFu;
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

public class GenerateDBC
{
    /// <summary>
    /// ????APP???????CAN???????????DBC???
    /// </summary>
    /// <returns>DBC???????????????????? null</returns>
    static public string GenerateDbcForCanMatrix()
    {
        if (CanDbcDataManager.GetInstance()?.isLoadCfg != true)
            return null;

        var sb = new StringBuilder(16 * 1024);
        var msgSet = CanDbcDataManager.GetInstance().canMsgSet;

        sb.Append(GntVer());
        sb.Append(GntNS_());
        sb.Append(GntBS_());

        // ?????? + ?????????
        var nodes = new HashSet<string>(StringComparer.Ordinal);
        foreach (var msg in msgSet.Values)
        {
            if (!string.IsNullOrWhiteSpace(msg.transmitter))
                nodes.Add(msg.transmitter);
            foreach (var sig in msg.signals)
            {
                if (string.IsNullOrWhiteSpace(sig.recvNode) ||
                    string.Equals(sig.recvNode, "TBD", StringComparison.OrdinalIgnoreCase))
                    continue;
                foreach (string n in sig.recvNode.Split(','))
                {
                    string name = n.Trim();
                    if (name.Length > 0)
                        nodes.Add(name);
                }
            }
        }
        sb.Append(GntBU_(nodes));

        foreach (var item in msgSet)
        {
            sb.Append(GntBO_(item.Value));
            foreach (var sig in item.Value.signals)
            {
                sb.Append(' ');
                sb.Append(GntSG_(sig, item.Value));
            }
            if (item.Value.msgType == (uint)CanMsgType.DEBUG)
                sb.Append(" SG_ Group_Signal M : 56|8@1+ (1,0) [0|0] \"\"  Shinry");
            sb.Append("\r\n");
        }

        foreach (var item in msgSet)
        {
            if (!string.IsNullOrEmpty(item.Value.msgDesc))
                sb.Append(GntCmBo_(item.Value));
            foreach (var sig in item.Value.signals)
                sb.Append(GntCM_(item.Value, sig));
        }

        // ??????? + ????
        sb.Append("\r\n");
        sb.Append("BA_DEF_ BO_  \"GenMsgCycleTime\" INT 0 65535;\r\n");
        sb.Append("BA_DEF_ BO_  \"VFrameFormat\" ENUM  \"StandardCAN\",\"ExtendedCAN\",\"reserved\",\"reserved\",\"reserved\",\"reserved\",\"reserved\",\"reserved\",\"reserved\",\"reserved\",\"reserved\",\"reserved\",\"reserved\",\"reserved\",\"StandardCAN_FD\",\"ExtendedCAN_FD\";\r\n");
        sb.Append("BA_DEF_DEF_  \"GenMsgCycleTime\" 0;\r\n");
        sb.Append("BA_DEF_DEF_  \"VFrameFormat\" 14;\r\n");
        sb.Append("\r\n");

        // ????????????? / ??????
        foreach (var item in msgSet.Values)
        {
            uint dbcId = GetDbcCanId(item);
            sb.Append("BA_ \"GenMsgCycleTime\" BO_ ")
              .Append(dbcId.ToString(CultureInfo.InvariantCulture))
              .Append(' ')
              .Append(item.msgCycle.ToString(CultureInfo.InvariantCulture))
              .Append(";\r\n");

            int frameType = CanDbcDataManager.GetMsgFrameType(item.isCanfd, item.isExtended);
            sb.Append("BA_ \"VFrameFormat\" BO_ ")
              .Append(dbcId.ToString(CultureInfo.InvariantCulture))
              .Append(' ')
              .Append(frameType.ToString(CultureInfo.InvariantCulture))
              .Append(";\r\n");
        }
        sb.Append("\r\n");

        foreach (var item in msgSet)
        {
            foreach (var sig in item.Value.signals)
                sb.Append(GntVAL_(item.Value, sig));
        }

        foreach (var item in msgSet)
        {
            if (item.Value.msgType == (uint)CanMsgType.DEBUG)
                sb.Append(GntSigGroup_ReuseFrame(item.Value));
        }

        return sb.ToString();
    }

    /// <summary>DBC ???? CAN ID???????? DEBUG ????????? bit31??</summary>
    static public uint GetDbcCanId(CanMessage msg)
    {
        uint id = msg.msgId;
        if (msg.msgType == (uint)CanMsgType.DEBUG || msg.isExtended)
            id |= 0x80000000u;
        return id;
    }

    static private string EscapeDbcString(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;
        return text.Replace("\"", "'");
    }

    static private string GntVer()
    {
        return "VERSION \"V0.0.1\"\r\n";
    }

    static private string GntNS_()
    {
        return
              "NS_ :\r\n\t"
              + "NS_DESC_\r\n\t"
              + "CM_\r\n\t"
              + "BA_DEF_\r\n\t"
              + "BA_\r\n\t"
              + "VAL_\r\n\t"
              + "CAT_DEF_\r\n\t"
              + "CAT_\r\n\t"
              + "FILTER\r\n\t"
              + "BA_DEF_DEF_\r\n\t"
              + "EV_DATA_\r\n\t"
              + "ENVVAR_DATA_\r\n\t"
              + "SGTYPE_\r\n\t"
              + "SGTYPE_VAL_\r\n\t"
              + "BA_DEF_SGTYPE_\r\n\t"
              + "BA_SGTYPE_\r\n\t"
              + "SIG_TYPE_REF_\r\n\t"
              + "VAL_TABLE_\r\n\t"
              + "SIG_GROUP_\r\n\t"
              + "SIG_VALTYPE_\r\n\t"
              + "SIGTYPE_VALTYPE_\r\n\t"
              + "BO_TX_BU_\r\n\t"
              + "BA_DEF_REL_\r\n\t"
              + "BA_REL_\r\n\t"
              + "BA_DEF_DEF_REL_\r\n\t"
              + "BU_SG_REL_\r\n\t"
              + "BU_EV_REL_\r\n\t"
              + "BU_BO_REL_\r\n\t"
              + "SG_MUL_VAL_\r\n";
    }

    static private string GntBS_()
    {
        return "BS_ :\r\n";
    }

    static private string GntBU_(IEnumerable<string> ecuName)
    {
        var sb = new StringBuilder("BU_:");
        foreach (var item in ecuName)
        {
            if (string.IsNullOrWhiteSpace(item)) continue;
            sb.Append(' ').Append(item);
        }
        sb.Append("\r\n");
        return sb.ToString();
    }

    static private string GntSG_(CanSignal sig, CanMessage msg)
    {
        var sb = new StringBuilder("SG_ ");
        sb.Append(sig.sigName).Append(' ');

        // DEBUG ??????? ?? ??? Vector mux
        if (msg.msgType == (uint)CanMsgType.DEBUG)
            sb.Append('m').Append(sig.reuseFrameID).Append(' ');
        else if (sig.muxType == DbcMatrixParser.MuxSwitch)
            sb.Append("M ");
        else if (sig.muxType == DbcMatrixParser.MuxValue)
            sb.Append('m').Append(sig.reuseFrameID).Append(' ');

        sb.Append(": ");

        if (sig.sigOrderType == 0)
            sb.Append(CanOrderTool.MotorolaStartBit_Lsb2Msb(sig.sigStartBit, sig.sigLen).ToString(CultureInfo.InvariantCulture));
        else
            sb.Append(sig.sigStartBit.ToString(CultureInfo.InvariantCulture));

        sb.Append('|')
          .Append(sig.sigLen.ToString(CultureInfo.InvariantCulture))
          .Append('@')
          .Append(sig.sigOrderType)
          .Append(sig.valueType == 0 ? '+' : '-')
          .Append(' ')
          .Append('(')
          .Append(sig.sigFactor.ToString(CultureInfo.InvariantCulture))
          .Append(',')
          .Append(sig.sigOffset.ToString(CultureInfo.InvariantCulture))
          .Append(") ");

        double min = sig.sigMin;
        double max = sig.sigMax;
        if (max == 0 && min == 0 && sig.sigLen > 0)
            max = Math.Pow(2, sig.sigLen) * sig.sigFactor;

        sb.Append('[')
          .Append(min.ToString(CultureInfo.InvariantCulture))
          .Append('|')
          .Append(max.ToString(CultureInfo.InvariantCulture))
          .Append("] \"")
          .Append(EscapeDbcString(sig.sigUnit ?? string.Empty))
          .Append("\" ");

        if (string.IsNullOrEmpty(sig.recvNode))
            sb.Append("TBD");
        else
            sb.Append(sig.recvNode);

        sb.Append("\r\n");
        return sb.ToString();
    }

    static private string GntBO_(CanMessage msg)
    {
        return "BO_ "
            + GetDbcCanId(msg).ToString(CultureInfo.InvariantCulture) + " "
            + msg.msgName + ": "
            + msg.msgSize.ToString(CultureInfo.InvariantCulture) + " "
            + msg.transmitter
            + "\r\n";
    }

    static private string GntCmBo_(CanMessage msg)
    {
        return "CM_ BO_ "
            + GetDbcCanId(msg).ToString(CultureInfo.InvariantCulture) + " "
            + "\"" + EscapeDbcString(msg.msgDesc) + "\";"
            + "\r\n";
    }

    static private string GntCM_(CanMessage msg, CanSignal sig)
    {
        return "CM_ SG_ "
            + GetDbcCanId(msg).ToString(CultureInfo.InvariantCulture) + " "
            + sig.sigName + " "
            + "\"" + EscapeDbcString(sig.sigDesc) + "\";"
            + "\r\n";
    }

    static private string GntVAL_(CanMessage msg, CanSignal sig)
    {
        if (sig.sigValueTable is null || sig.sigValueTable.Count == 0)
            return string.Empty;

        var sb = new StringBuilder("VAL_ ");
        sb.Append(GetDbcCanId(msg).ToString(CultureInfo.InvariantCulture)).Append(' ');
        sb.Append(sig.sigName).Append(' ');
        foreach (var item in sig.sigValueTable.OrderBy(x => x.Key))
        {
            sb.Append(item.Key.ToString(CultureInfo.InvariantCulture)).Append(' ');
            sb.Append('"').Append(EscapeDbcString(item.Value)).Append("\" ");
        }
        if (sb[sb.Length - 1] == ' ')
            sb.Length--;
        sb.Append(";\r\n");
        return sb.ToString();
    }

    static private string GntSigGroup_ReuseFrame(CanMessage msg)
    {
        var sb = new StringBuilder();
        uint maxFrameID = 0;
        foreach (var item in msg.signals)
        {
            if (item.reuseFrameID >= maxFrameID)
                maxFrameID = item.reuseFrameID;
        }

        uint dbcId = GetDbcCanId(msg);
        for (int i = 0; i <= maxFrameID; i++)
        {
            sb.Append("SIG_GROUP_ ")
              .Append(dbcId.ToString(CultureInfo.InvariantCulture))
              .Append(" Signal_Group_")
              .Append(i)
              .Append(" 1 :");
            foreach (var item in msg.signals)
            {
                if (item.reuseFrameID == i)
                    sb.Append(' ').Append(item.sigName);
            }
            sb.Append(";\r\n");
        }

        return sb.ToString();
    }
}

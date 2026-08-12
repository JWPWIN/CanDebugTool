using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;
using System.Globalization;
using System.Windows.Forms;
using System.Linq;
using System.Text;

public class CanMessage
{
    public uint msgId = 0;
    public uint msgCycle = 0;
    public bool isExtended = false;
    public bool isCanfd = false;
    public string msgName = "";
    /// <summary>报文注释（CM_ BO_）。</summary>
    public string msgDesc = "";
    public uint msgSize = 0;
    public string transmitter = "";
    public uint msgType = 0;//(0:APP; 1:NM; 2:Debug-复用帧)
    public List<CanSignal> signals = new List<CanSignal>();
}

public class CanSignal
{
    public string sigName; //信号名
    public uint msgId;//信号所属报文ID
    public string sigDesc;//信号描述
    public uint sigOrderType;//0：Motorola-LSB；1：Intel
    public uint sigStartBit;//信号起始位
    public uint sigLen;//信号长度
    public double sigFactor; //信号精度
    public double sigOffset; //信号偏移
    public Dictionary<int, string> sigValueTable = new Dictionary<int, string>(); //信号值列表
    public uint valueType; //值类型：1-有符号；0：无符号
    public string recvNode;//接收节点（多节点逗号拼接）
    public uint reuseFrameID;//复用帧ID / mN 的 N
    /// <summary>0=无复用, 1=M 复用开关, 2=mN 复用信号。</summary>
    public uint muxType;
    /// <summary>物理单位（SG_ 引号字段）。</summary>
    public string sigUnit = "";
    public double sigMin;
    public double sigMax;
}

//excel�����ļ���ÿ�д����ĺ���
public enum CanDbcRows
{ 
    SigName,
    MsgName,
    MsgFrameType,
    MsgId,
    MsgSize,
    MsgCycle,
    SigDesc,
    SigOrderType,
    SigStartBit,
    SigLen,
    SigFactor,
    SigOffset,
    SigValueTable,
    ValueType,
    SendNode,
    RecvNode,
    ReuseFrameID,
    MsgType,

    MaxNum
}

//��������
public enum CanMsgType
{
    APP,//Ӧ�ñ���
    NM,//�����������
    DEBUG//���Ա���-����֡
}

public class CanDbcDataManager
{
    static private CanDbcDataManager instance;

    //CANͨ�ž��������ֵ�
    public Dictionary<uint,CanMessage> canMsgSet = new Dictionary<uint, CanMessage>();

    //�Ƿ����DBC�����ļ�
    public bool isLoadCfg = false;

    public CanDbcDataManager()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    static public CanDbcDataManager GetInstance()
    {
        // δ��ʼ��ʱ��Ĭ���� null�����������׶ε�������������
        return instance;
    }


    // ��Excel�е���CAN������Ϣ�����ļ�ѡ��Ի���UI �̵߳��ã�
    public void LoadCanMatrixFromExcel()
    {
        Dictionary<string, List<List<string>>> excelAllData = ExcelManager.ImportData();
        LoadCanMatrixFromExcelData(excelAllData);
    }

    /// <summary>
    /// ���Ѷ�ȡ�� Excel ���ݹ������󣨿��ں�̨�̵߳��ã���
    /// </summary>
    /// <returns>�Ƿ�ɹ�����</returns>
    public bool LoadCanMatrixFromExcelData(Dictionary<string, List<List<string>>> excelAllData)
    {
        //Ĭ��ʹ�õ�һ��sheet����
        List<List<string>> usedSheet = null;
        if (excelAllData != null) 
        {
            usedSheet = excelAllData.First().Value;
        }

        //���û�ж������ݣ����˳�
        if (usedSheet == null)
        {
            return false;
        }
        else
        {
            //���֮ǰ��DBC����
            ResetCanDbcCfg();
        }
        // ��ȡ�����ж�����
        int columns = usedSheet[0].Count;
        // ��ȡ�����ж����� 
        int rows = usedSheet.Count;

        //��һ��Ϊ��ͷ������ȡ��û�б�ͷ��0��ʼ(��ȡ����)
        //���ȶ�ȡCAN������Ϣ
        for (int i = 1; i < rows; i++)
        {
            bool flag = true;
            CanMessage msg = new CanMessage();
            for (int j = 0; j < columns; j++)
            {
                // ��ȡ������ָ����ָ���е����� 
                string value = usedSheet[i][j].ToString();

                switch ((CanDbcRows)j)
                {
                    case CanDbcRows.MsgId:
                        string str = value.Remove(0, 2);//�Ƴ�ǰ�������ַ�0x
                        msg.msgId = UInt32.Parse(str, System.Globalization.NumberStyles.HexNumber);
                        //�Ѿ���ȡ��ͬID������Ϣ,�����ظ����ӵ����ļ���
                        if (canMsgSet.ContainsKey(msg.msgId))
                        {
                            flag = false;
                        }
                        break;
                    case CanDbcRows.MsgCycle:
                        msg.msgCycle = uint.Parse(value);
                        break;
                    case CanDbcRows.MsgName:
                        msg.msgName = value;
                        break;
                    case CanDbcRows.MsgSize:
                        msg.msgSize = uint.Parse(value);
                        break;
                    case CanDbcRows.SendNode:
                        msg.transmitter = value;
                        break;
                    case CanDbcRows.MsgType:
                        msg.msgType = uint.Parse(value);
                        break;
                    case CanDbcRows.MsgFrameType:
                        string msgFrameTypeStr = value.Replace(" ","");

                        if (msgFrameTypeStr == "0")//standard-can
                        {
                            msg.isExtended = false;
                            msg.isCanfd = false;
                        }
                        else if (msgFrameTypeStr == "1")//externed-can
                        {
                            msg.isExtended = true;
                            msg.isCanfd = false;
                        }
                        else if (msgFrameTypeStr == "14")//standard-canfd
                        {
                            msg.isExtended = false;
                            msg.isCanfd = true;
                        }
                        else if (msgFrameTypeStr == "15")//externed-canfd
                        {
                            msg.isExtended = true;
                            msg.isCanfd = true;
                        }
                        else 
                        {
                            msg.isExtended = false;
                            msg.isCanfd = false;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (true == flag)
            {
                //���ӱ�����Ϣ���������ݼ���
                canMsgSet.Add(msg.msgId, msg);
            }

        }

        //��һ��Ϊ��ͷ������ȡ��û�б�ͷ��0��ʼ(��ȡ����)
        //��ȡCAN�ź�����
        for (int i = 1; i < rows; i++)
        {
            CanSignal tmpSig = new CanSignal();
            for (int j = 0; j < columns; j++)
            {
                // ��ȡ������ָ����ָ���е����� 
                string value = usedSheet[i][j].ToString();
                
                switch ((CanDbcRows)j)
                {
                    case CanDbcRows.SigName:
                        tmpSig.sigName = value;
                        break;
                    case CanDbcRows.MsgId:
                        string str = value.Remove(0, 2);//�Ƴ�ǰ�������ַ�0x
                        tmpSig.msgId = UInt32.Parse(str, System.Globalization.NumberStyles.HexNumber);
                        break;
                    case CanDbcRows.SigDesc:
                        tmpSig.sigDesc = value;
                        break;
                    case CanDbcRows.SigOrderType:
                        tmpSig.sigOrderType = uint.Parse(value);
                        break;
                    case CanDbcRows.SigStartBit:
                        tmpSig.sigStartBit = uint.Parse(value);
                        break;
                    case CanDbcRows.SigLen:
                        tmpSig.sigLen = uint.Parse(value);
                        break;
                    case CanDbcRows.SigFactor:
                        tmpSig.sigFactor = double.Parse(value);
                        break;
                    case CanDbcRows.SigOffset:
                        tmpSig.sigOffset = double.Parse(value);
                        break;
                    case CanDbcRows.SigValueTable:
                        if (value != null && value != "")
                        {
                            string[] valueAry = value.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                            for (int k = 0; k < valueAry.Length; k++)
                            {
                                string[] tmpArr = valueAry[k].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                tmpSig.sigValueTable.Add(int.Parse(tmpArr[0]), tmpArr[1]);
                            }

                            //�ź�ֵ�б�����
                            var _valueTableDict = tmpSig.sigValueTable.OrderBy(x => x.Key).ToDictionary<int, string>();
                            tmpSig.sigValueTable = _valueTableDict as Dictionary<int, string>;
                        }
                        else
                        {
                            tmpSig.sigValueTable = null;
                        }
                        break;
                    case CanDbcRows.ValueType:
                        tmpSig.valueType = uint.Parse(value);
                        break;
                    case CanDbcRows.RecvNode:
                        tmpSig.recvNode = value;
                        break;
                    case CanDbcRows.ReuseFrameID:
                        tmpSig.reuseFrameID = uint.Parse(value);
                        break;
                }
            }
            //���Ӹ��źŵ�CAN�������ݼ���
            canMsgSet[tmpSig.msgId].signals.Add(tmpSig);
        }
                isLoadCfg = true;

        AppLogMng.DisplayLog("��Excel�ļ�����ͨ��Э��ɹ�!", true);
        return true;
    }

    /// <summary>弹出文件选择并导入 DBC（UI 线程）。</summary>
    /// <returns>是否成功加载</returns>
    public bool LoadCanMatrixFromDBC()
    {
        string path = TextOperation.PickDbcFile();
        if (string.IsNullOrEmpty(path))
            return false;
        string dbcInfo = DbcTextReader.ReadDbcFile(path);
        return LoadCanMatrixFromDbcText(dbcInfo);
    }

    /// <summary>
    /// 从 DBC 文本解析 CAN 矩阵。先写入临时表，成功后再替换 canMsgSet（失败不破坏已有矩阵）。
    /// 可在 UI 线程调用；文件 I/O 请在调用前用 DbcTextReader.ReadDbcFile 完成。
    /// </summary>
    public bool LoadCanMatrixFromDbcText(string dbcInfo)
    {
        if (string.IsNullOrWhiteSpace(dbcInfo))
        {
            MessageBox.Show("DBC 文件为空");
            return false;
        }

        if (!DbcMatrixParser.TryParse(dbcInfo, out Dictionary<uint, CanMessage> parsed, out List<string> warnings))
        {
            MessageBox.Show("DBC 中未解析到有效报文（BO_），或文件格式错误");
            return false;
        }

        canMsgSet.Clear();
        foreach (var kv in parsed)
            canMsgSet.Add(kv.Key, kv.Value);
        isLoadCfg = true;

        if (warnings.Count > 0)
        {
            int show = Math.Min(warnings.Count, 8);
            var sb = new StringBuilder();
            sb.AppendLine("DBC 已加载，但有 " + warnings.Count + " 条告警：");
            for (int w = 0; w < show; w++)
                sb.AppendLine(warnings[w]);
            if (warnings.Count > show)
                sb.AppendLine("...");
            MessageBox.Show(sb.ToString(), "DBC 导入告警", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        AppLogMng.DisplayLog("从 DBC 文件导入通信协议成功!", true);
        return true;
    }

    private void ResetCanDbcCfg()
    {
        canMsgSet.Clear();
        isLoadCfg = false;
    }

    /// <summary>
    /// ����CANFD����չ֡��ʽ��ȡDBC�ڴ���CAN֡���͵�ֵ
    /// </summary>
    /// <param name="isCanfd"></param>
    /// <param name="isExtended"></param>
    /// <returns></returns>
    static public int GetMsgFrameType(bool isCanfd, bool isExtended)
    {
        int frameType = 0;

        //BA_DEF_ BO_ "VFrameFormat" ENUM  "StandardCAN","ExtendedCAN","reserved","reserved","reserved","reserved","reserved","reserved","reserved","reserved","reserved","reserved","reserved","reserved","StandardCAN_FD","ExtendedCAN_FD";
        if ((isCanfd == false) && (isExtended == false))//standard-can
        {
            frameType = 0;
        }
        else if ((isCanfd == false) && (isExtended == true))//externed-can
        {
            frameType = 1;
        }
        else if ((isCanfd == true) && (isExtended == false))//standard-canfd
        {
            frameType = 14;
        }
        else if ((isCanfd == true) && (isExtended == true))//externed-canfd
        {
            frameType = 15;
        }
        else
        {
            frameType = 0;
        }

        return frameType;
    }

    /// <summary>
    /// �жϵ�ǰ���ͱ��ĵķ��ͽڵ��Ƿ�ΪDBC�е�Ŀ��ECU
    /// </summary>
    /// <param name="transmiter">���ͽڵ�</param>
    /// <returns>true ����Ŀ��ECU</returns>
    static public bool IsMsgBelongToTargetEcu(string transmitter)
    {
        if (transmitter.Contains("OBC") || transmitter.Contains("DCDC") || transmitter.Contains("CDU"))
        { 
            return true;
        }
        return false;
    }
}

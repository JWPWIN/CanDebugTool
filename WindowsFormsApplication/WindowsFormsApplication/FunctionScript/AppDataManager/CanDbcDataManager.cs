using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;
using System.Windows.Forms;
using System.Linq;

public class CanMessage
{
    public uint msgId = 0;
    public uint msgCycle = 0;
    public bool isExtended = false;
    public bool isCanfd = false;
    public string msgName = "";
    public uint msgSize = 0;
    public string transmitter = "";
    public uint msgType = 0;//(0:APP; 1:NM; 2:Debug-����֡)
    public List<CanSignal> signals = new List<CanSignal>();
}

public class CanSignal
{
    public string sigName; //�ź���
    public uint msgId;//�ź���������ID
    public string sigDesc;//�ź�����
    public uint sigOrderType;//0��Motorola-LSB��1��Intel
    public uint sigStartBit;//�ź���ʼλ
    public uint sigLen;//�źų���
    public double sigFactor; //�źž���
    public double sigOffset; //�ź�ƫ��
    public Dictionary<int, string> sigValueTable = new Dictionary<int, string>(); //�ź�ֵ�б�<�ź�ֵ���ź�ֵ����>
    public uint valueType; //ֵ���ͣ�1-�з��ţ�0���޷���
    public string recvNode;//���սڵ�
    public uint reuseFrameID;//����֡ID����������ΪDebugģʽʱ����
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

    //��DBC�е���CAN������Ϣ
    public void LoadCanMatrixFromDBC()
    {
        //ѡ��DBC�ļ�����ȡ����
        string dbcInfo = TextOperation.ReadData();
        string[] bufferAry = dbcInfo.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string[] bufferAry_CheckErrLine = dbcInfo.Split(new string[] { "\r\n"},StringSplitOptions.None);//���ڼ�����������ԭʼ�ָ�����,��������

        if (dbcInfo == null)
        {
            MessageBox.Show("DBC�ļ��ǿյ�");
            return;
        }

        if (bufferAry.Length < 3)
        {
            MessageBox.Show("Dbc�ļ���ʽ����");
            return;
        }

        //�������֮ǰ��DBC����
        ResetCanDbcCfg();

        int lineNum = bufferAry.Length;
        bool isMessageValid = false;
        uint lastMsgId = 0;

        for (int i = 0; i < lineNum; i++)
        {
            string[] lineAry = bufferAry[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (lineAry.Length < 1)
            {
                MessageBox.Show("Dbc�ļ��и�ʽ����");
                return;
            }
            switch (lineAry[0])
            {
                case "VAL_":
                    {
                        try
                        {
                            //��ʽ������VAL_ 1072 HEVC_WakeUpSleepCommand 0 "Go to Sleep" 1 "reserved0" 2 "reserved1" 3 "WakeUp"; 
                            uint tmpId = uint.Parse(lineAry[1]);
                            if (canMsgSet.ContainsKey(tmpId))
                            {
                                foreach (var item in canMsgSet[tmpId].signals)
                                {
                                    if (lineAry[2] == item.sigName)
                                    {
                                        //ȡֵ�����ַ���
                                        string _valueTableStr = string.Empty;
                                        for (int j = 3; j < lineAry.Length; j++)
                                        {
                                            _valueTableStr += lineAry[j];
                                        }

                                        string[] tmpArr = _valueTableStr.Replace(";", string.Empty).Split(new char[] { '\"' }, StringSplitOptions.RemoveEmptyEntries);
                                        int tmpNum = 0;
                                        while (tmpNum < tmpArr.Length)
                                        {
                                            int _tmpValue = int.Parse(tmpArr[tmpNum].Replace(" ", ""));
                                            string _tmpDesc = tmpArr[tmpNum + 1];
                                            //����valueֵ��
                                            item.sigValueTable.Add(_tmpValue, _tmpDesc);

                                            tmpNum = tmpNum + 2;
                                        }

                                        //�ź�ֵ�б�����
                                        var _valueTableDict = item.sigValueTable.OrderBy(x => x.Key).ToDictionary<int, string>();
                                        item.sigValueTable = _valueTableDict as Dictionary<int, string>;
                                    }
                                }
                            }
                        }
                        catch(Exception)
                        {
                            string errorLineStr = bufferAry[i];
                            int errorLineNum = 0;
                            for (int j = 0; j < bufferAry_CheckErrLine.Length; j++)
                            {
                                if (bufferAry_CheckErrLine[j].Contains(errorLineStr)) errorLineNum = j + 1;
                            }
                            MessageBox.Show("����VAL_�ֶθ�ʽʧ�ܣ�" +  "\r\n"
                                             + "�����ֶ�������" + errorLineNum + "\r\n"
                                             + "�����ֶ����ݣ�" + errorLineStr + "\r\n"
                                             + "�����Ƿ������ʽ������VAL_ 1072 HEVC_WakeUpSleepCommand 0 \"Go to Sleep\" 1 \"reserved0\" 2 \"reserved1\" 3 \"WakeUp\";");
                        }

                        break;
                    }
                case "CM_":
                    {
                        try 
                        {
                            //��ʽ������ CM_ SG_ 129 HVCurrentRequest "��������������";
                            uint tmpId = uint.Parse(lineAry[2]);
                            if (canMsgSet.ContainsKey(tmpId))
                            {
                                foreach (var item in canMsgSet[tmpId].signals)
                                {
                                    if (lineAry[3] == item.sigName)
                                    {
                                        item.sigDesc = lineAry[4].Replace("\"", string.Empty).Replace(";", string.Empty);
                                    }
                                }
                            }
                        }
                        catch(Exception)
                        {
                            string errorLineStr = bufferAry[i];
                            int errorLineNum = 0;
                            for (int j = 0; j < bufferAry_CheckErrLine.Length; j++)
                            {
                                if (bufferAry_CheckErrLine[j].Contains(errorLineStr)) errorLineNum = j + 1;
                            }
                            MessageBox.Show("����CM_�ֶθ�ʽʧ�ܣ�" + "\r\n"
                                             + "�����ֶ�������" + errorLineNum + "\r\n"
                                             + "�����ֶ����ݣ�" + errorLineStr + "\r\n"
                                             + "�����Ƿ������ʽ������CM_ SG_ 129 HVCurrentRequest \"��������������\";");
                        }

                        break;
                    }
                case "BU_:":
                    {
                        for (int j = 1; j < (lineAry.Length); j++)
                        {
                            //TODO:�����ڵ���Ϣ
                        }
                        break;
                    }
                case "BO_":
                    {
                        try
                        {
                            //��ʽ������BO_ 1127 CDU_DCDC_1: 24 CDU
                            CanMessage message = new CanMessage();
                            uint id = Convert.ToUInt32(lineAry[1]);
                            //����Ĭ�ϵ���Ϣ
                            if (id == 0xC0000000)
                            {
                                isMessageValid = false;
                                break;
                            }
                            else
                            {
                                isMessageValid = true;
                            }
                            //���λΪ1��Ϊ��չ֡
                            if ((id & 0x80000000) != 0)
                            {
                                id &= 0x7FFFFFFF;
                                message.isExtended = true;
                            }
                            else
                            {
                                message.isExtended = false;
                            }
                            message.msgId = id;
                            message.msgName = lineAry[2].Substring(0, lineAry[2].Length - 1);
                            message.msgSize = Convert.ToUInt32(lineAry[3]);
                            message.transmitter = lineAry[4];

                            canMsgSet.Add(message.msgId, message);
                            lastMsgId = message.msgId;
                        }
                        catch (Exception)
                        {
                            string errorLineStr = bufferAry[i];
                            int errorLineNum = 0;
                            for (int j = 0; j < bufferAry_CheckErrLine.Length; j++)
                            {
                                if (bufferAry_CheckErrLine[j].Contains(errorLineStr)) errorLineNum = j + 1;
                            }
                            MessageBox.Show("����BO_�ֶθ�ʽʧ�ܣ�" + "\r\n"
                                             + "�����ֶ�������" + errorLineNum + "\r\n"
                                             + "�����ֶ����ݣ�" + errorLineStr + "\r\n"
                                             + "�����Ƿ������ʽ������BO_ 1127 CDU_DCDC_1: 24 CDU");
                        }

                        break;
                    }
                case "SG_":
                    {
                        try
                        {
                            //��ʽ������
                            //��ͨ֡�źŸ�ʽ�� SG_ OBC_ChgCurr : 23|16@0+ (0.05,0) [0|400] "A"  Vector__XXX
                            //����֡�źŸ�ʽ�� SG_ AAA00_DcdcInputVolt m0 : 0|16@1+ (0.1,0) [0|6553.6] "" EXECU
                            if (isMessageValid)
                            {
                                uint byteOffset = 0;
                                CanSignal signal = new CanSignal();

                                signal.sigName = lineAry[1];
                                if (lineAry[2] == ":")//��ͨ֡
                                {
                                    //TODO: �����źű�־λ��signal.multiplexerIndicator = -2;
                                    byteOffset = 0;
                                }
                                else//����֡
                                {
                                    byteOffset = 1;
                                    /* TODO: �����źű�־λ
                                    if (lineAry[2][0] == 'M')
                                    {
                                        signal.multiplexerIndicator = -1;
                                    }
                                    else if (lineAry[2][0] == 'm')
                                    {
                                        signal.multiplexerIndicator = Convert.ToInt32(lineAry[2].Substring(1, lineAry[2].Length - 1));
                                    }
                                    else
                                    {
                                        return ExceptionHandler.Report("Dbc�źŸ�ʽ����");
                                    }
                                    */
                                }

                                string[] sp = lineAry[3 + byteOffset].Split(new char[] { '|', '@' }, StringSplitOptions.RemoveEmptyEntries);

                                signal.sigLen = Convert.ToUInt32(sp[1]);
                                if (sp[2][0] == '0')
                                {
                                    signal.sigOrderType = 0;
                                    //DBC��MotorolaΪMSB����Ҫת��ΪLSB
                                    signal.sigStartBit = CanOrderTool.MotorolaStartBit_Msb2Lsb(Convert.ToUInt32(sp[0]), signal.sigLen);
                                }
                                else if (sp[2][0] == '1')
                                {
                                    signal.sigOrderType = 1;
                                    signal.sigStartBit = Convert.ToUInt32(sp[0]);
                                }

                                if (lineAry[3] == "+")
                                {
                                    signal.valueType = 0;
                                }
                                else if (lineAry[3] == "-")
                                {
                                    signal.valueType = 1;
                                }

                                string[] sp1 = lineAry[4 + byteOffset].Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                                signal.sigFactor = Convert.ToDouble(sp1[0]);
                                signal.sigOffset = Convert.ToDouble(sp1[1]);

                                //string[] sp2 = lineAry[5 + byteOffset].Split(new char[] { '[', '|', ']' }, StringSplitOptions.RemoveEmptyEntries);
                                //�����Сֵ
                                //signal.minimum = Convert.ToDouble(sp2[0]);
                                //signal.maximum = Convert.ToDouble(sp2[1]);

                                //�źŵ�λ
                                //signal.uintStr = lineAry[6 + byteOffset];

                                //�źŽ��սڵ�
                                if (7 + byteOffset <= lineAry.Length - 1)
                                {
                                    signal.recvNode = lineAry[7 + byteOffset].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                                }
                                canMsgSet[lastMsgId].signals.Add(signal);
                            }
                        }
                        catch (Exception)
                        {
                            string errorLineStr = bufferAry[i];
                            int errorLineNum = 0;
                            for (int j = 0; j < bufferAry_CheckErrLine.Length; j++)
                            {
                                if (bufferAry_CheckErrLine[j].Contains(errorLineStr)) errorLineNum = j + 1;
                            }
                            MessageBox.Show("����SG_�ֶθ�ʽʧ�ܣ�" + "\r\n"
                                             + "�����ֶ�������" + errorLineNum + "\r\n"
                                             + "�����ֶ����ݣ�" + errorLineStr + "\r\n"
                                             + "�����Ƿ������ʽ������SG_ OBC_ChgCurr : 23|16@0+ (0.05,0) [0|400] \"A\"  Vector__XXX");
                        }

                        break;
                    }
                case "BA_":
                    {
                        //��ȡ��������-��������
                        //��ʽ������BA_ "GenMsgCycleTime" BO_ 1118 100;
                        if ((lineAry[1].Replace("\"", "") == "GenMsgCycleTime") && (lineAry[2] == "BO_"))
                        {
                            try
                            {
                                uint tmpId = uint.Parse(lineAry[3]);
                                canMsgSet[tmpId].msgCycle = uint.Parse(lineAry[4].Replace(";",""));
                            }
                            catch (Exception)
                            {
                                string errorLineStr = bufferAry[i];
                                int errorLineNum = 0;
                                for (int j = 0; j < bufferAry_CheckErrLine.Length; j++)
                                {
                                    if (bufferAry_CheckErrLine[j].Contains(errorLineStr)) errorLineNum = j + 1;
                                }
                                MessageBox.Show("����BA_�ֶα������ڸ�ʽʧ�ܣ�" + "\r\n"
                                                 + "�����ֶ�������" + errorLineNum + "\r\n"
                                                 + "�����ֶ����ݣ�" + errorLineStr + "\r\n"
                                                 + "�����Ƿ������ʽ������BA_ \"GenMsgCycleTime\" BO_ 1118 100;");
                            }
                        }
                        else if ((lineAry[1].Replace("\"", "") == "VFrameFormat") && (lineAry[2] == "BO_"))
                        {
                            //��ȡ��������-����֡����
                            //��ʽ������BA_ "VFrameFormat" BO_ 520 14;��0:Standard-CAN; 1:Externed-CAN; 14:Standard-CANFD; 15:Externed-CANFD��
                            //BA_DEF_ BO_ "VFrameFormat" ENUM  "StandardCAN","ExtendedCAN","reserved","reserved","reserved","reserved","reserved","reserved","reserved","reserved","reserved","reserved","reserved","reserved","StandardCAN_FD","ExtendedCAN_FD";
                            try
                            {
                                uint tmpId = uint.Parse(lineAry[3]);
                                string msgFrameTypeStr = lineAry[4].Replace(";", "");
                                bool _tmpIsExtended = false;
                                bool _tmpIsCanfd = false;
                                if (msgFrameTypeStr == "0")//standard-can
                                {
                                    _tmpIsExtended = false;
                                    _tmpIsCanfd = false;
                                }
                                else if (msgFrameTypeStr == "1")//externed-can
                                {
                                    _tmpIsExtended = true;
                                    _tmpIsCanfd = false;
                                }
                                else if (msgFrameTypeStr == "14")//standard-canfd
                                {
                                    _tmpIsExtended = false;
                                    _tmpIsCanfd = true;
                                }
                                else if (msgFrameTypeStr == "15")//externed-canfd
                                {
                                    _tmpIsExtended = true;
                                    _tmpIsCanfd = true;
                                }
                                else { }

                                //��չ֡ID��Ҫ����һ��
                                if (_tmpIsExtended == true) tmpId &= 0x7FFFFFFF;

                                canMsgSet[tmpId].isExtended = _tmpIsExtended;
                                canMsgSet[tmpId].isCanfd = _tmpIsCanfd;
                            }
                            catch (Exception)
                            {
                                string errorLineStr = bufferAry[i];
                                int errorLineNum = 0;
                                for (int j = 0; j < bufferAry_CheckErrLine.Length; j++)
                                {
                                    if (bufferAry_CheckErrLine[j].Contains(errorLineStr)) errorLineNum = j + 1;
                                }
                                MessageBox.Show("����BA_�ֶα���֡���͸�ʽʧ�ܣ�" + "\r\n"
                                                 + "�����ֶ�������" + errorLineNum + "\r\n"
                                                 + "�����ֶ����ݣ�" + errorLineStr + "\r\n"
                                                 + "�����Ƿ������ʽ������BA_ \"VFrameFormat\" BO_ 520 14;");
                            }
                        }
                        else { }

                        break;
                    }

            }
        }
        isLoadCfg = true;

        AppLogMng.DisplayLog("��Excel�ļ�����ͨ��Э��ɹ�!", true);
    }

    //����Ѿ����ڵ�DBC����
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

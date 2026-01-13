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
    public uint msgType = 0;//(0:APP; 1:NM; 2:Debug-复用帧)
    public List<CanSignal> signals = new List<CanSignal>();
}

public class CanSignal
{
    public string sigName; //信号名
    public uint msgId;//信号所属报文ID
    public string sigDesc;//信号描述
    public uint sigOrderType;//0：Motorola-LSB，1：Intel
    public uint sigStartBit;//信号起始位
    public uint sigLen;//信号长度
    public double sigFactor; //信号精度
    public double sigOffset; //信号偏移
    public Dictionary<int, string> sigValueTable = new Dictionary<int, string>(); //信号值列表<信号意义，信号值>
    public uint valueType; //值类型：1-有符号，0：无符号
    public string recvNode;//接收节点
    public uint reuseFrameID;//复用帧ID，报文类型为Debug模式时启用
}

//excel配置文件中每列代表的含义
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

//报文类型
public enum CanMsgType
{
    APP,//应用报文
    NM,//网络管理报文
    DEBUG//调试报文-复用帧
}

public class CanDbcDataManager
{
    static private CanDbcDataManager instance;

    //CAN通信矩阵数据字典
    public Dictionary<uint,CanMessage> canMsgSet = new Dictionary<uint, CanMessage>();

    //是否加载DBC配置文件
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
        if (instance == null)
        {
            MessageBox.Show("CanDbcDataManager has not instance!");
            return null;
        }
        return instance;
    }


    // 从Excel中导入CAN矩阵信息
    public void LoadCanMatrixFromExcel()
    {
        //excel总数据信息: Dictionary<sheet表名, sheet表内容>
        Dictionary<string, List<List<string>>> excelAllData = ExcelManager.ImportData();

        //默认使用第一个sheet数据
        List<List<string>> usedSheet = null;
        if (excelAllData != null) 
        {
            usedSheet = excelAllData.First().Value;
        }

        //如果没有读到数据，则退出
        if (usedSheet == null)
        {
            return;
        }
        else
        {
            //清除之前的DBC配置
            ResetCanDbcCfg();
        }
        // 获取表格有多少列
        int columns = usedSheet[0].Count;
        // 获取表格有多少行 
        int rows = usedSheet.Count;

        //第一行为表头，不读取。没有表头从0开始(获取数据)
        //首先读取CAN报文信息
        for (int i = 1; i < rows; i++)
        {
            bool flag = true;
            CanMessage msg = new CanMessage();
            for (int j = 0; j < columns; j++)
            {
                // 获取表格中指定行指定列的数据 
                string value = usedSheet[i][j].ToString();

                switch ((CanDbcRows)j)
                {
                    case CanDbcRows.MsgId:
                        string str = value.Remove(0, 2);//移除前面两个字符0x
                        msg.msgId = UInt32.Parse(str, System.Globalization.NumberStyles.HexNumber);
                        //已经读取相同ID报文信息,不再重复添加到报文集合
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
                //添加报文信息到报文数据集合
                canMsgSet.Add(msg.msgId, msg);
            }

        }

        //第一行为表头，不读取。没有表头从0开始(获取数据)
        //读取CAN信号数据
        for (int i = 1; i < rows; i++)
        {
            CanSignal tmpSig = new CanSignal();
            for (int j = 0; j < columns; j++)
            {
                // 获取表格中指定行指定列的数据 
                string value = usedSheet[i][j].ToString();
                
                switch ((CanDbcRows)j)
                {
                    case CanDbcRows.SigName:
                        tmpSig.sigName = value;
                        break;
                    case CanDbcRows.MsgId:
                        string str = value.Remove(0, 2);//移除前面两个字符0x
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
            //添加该信号到CAN报文数据集合
            canMsgSet[tmpSig.msgId].signals.Add(tmpSig);
        }
        isLoadCfg = true;

        AppLogMng.DisplayLog("从DBC文件导入通信协议成功!",true);
    }

    //从DBC中导入CAN矩阵信息
    public void LoadCanMatrixFromDBC()
    {
        //选择DBC文件并读取数据
        string dbcInfo = TextOperation.ReadData();
        string[] bufferAry = dbcInfo.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string[] bufferAry_CheckErrLine = dbcInfo.Split(new string[] { "\r\n"},StringSplitOptions.None);//用于检测错误行数的原始分隔数据,包含空行

        if (dbcInfo == null)
        {
            MessageBox.Show("DBC文件是空的");
            return;
        }

        if (bufferAry.Length < 3)
        {
            MessageBox.Show("Dbc文件格式错误");
            return;
        }

        //首先清除之前的DBC配置
        ResetCanDbcCfg();

        int lineNum = bufferAry.Length;
        bool isMessageValid = false;
        uint lastMsgId = 0;

        for (int i = 0; i < lineNum; i++)
        {
            string[] lineAry = bufferAry[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (lineAry.Length < 1)
            {
                MessageBox.Show("Dbc文件行格式错误");
                return;
            }
            switch (lineAry[0])
            {
                case "VAL_":
                    {
                        try
                        {
                            //格式举例：VAL_ 1072 HEVC_WakeUpSleepCommand 0 "Go to Sleep" 1 "reserved0" 2 "reserved1" 3 "WakeUp"; 
                            uint tmpId = uint.Parse(lineAry[1]);
                            if (canMsgSet.ContainsKey(tmpId))
                            {
                                foreach (var item in canMsgSet[tmpId].signals)
                                {
                                    if (lineAry[2] == item.sigName)
                                    {
                                        //取值表段字符串
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
                                            //添加value值表
                                            item.sigValueTable.Add(_tmpValue, _tmpDesc);

                                            tmpNum = tmpNum + 2;
                                        }
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
                            MessageBox.Show("解析VAL_字段格式失败！" +  "\r\n"
                                             + "错误字段行数：" + errorLineNum + "\r\n"
                                             + "错误字段数据：" + errorLineStr + "\r\n"
                                             + "请检查是否满足格式举例：VAL_ 1072 HEVC_WakeUpSleepCommand 0 \"Go to Sleep\" 1 \"reserved0\" 2 \"reserved1\" 3 \"WakeUp\";");
                        }

                        break;
                    }
                case "CM_":
                    {
                        try 
                        {
                            //格式举例： CM_ SG_ 129 HVCurrentRequest "充电输出电流请求";
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
                            MessageBox.Show("解析CM_字段格式失败！" + "\r\n"
                                             + "错误字段行数：" + errorLineNum + "\r\n"
                                             + "错误字段数据：" + errorLineStr + "\r\n"
                                             + "请检查是否满足格式举例：CM_ SG_ 129 HVCurrentRequest \"充电输出电流请求\";");
                        }

                        break;
                    }
                case "BU_:":
                    {
                        for (int j = 1; j < (lineAry.Length); j++)
                        {
                            //TODO:存贮节点信息
                        }
                        break;
                    }
                case "BO_":
                    {
                        try
                        {
                            //格式举例：BO_ 1127 CDU_DCDC_1: 24 CDU
                            CanMessage message = new CanMessage();
                            uint id = Convert.ToUInt32(lineAry[1]);
                            //跳过默认的消息
                            if (id == 0xC0000000)
                            {
                                isMessageValid = false;
                                break;
                            }
                            else
                            {
                                isMessageValid = true;
                            }
                            //最高位为1的为扩展帧
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
                            MessageBox.Show("解析BO_字段格式失败！" + "\r\n"
                                             + "错误字段行数：" + errorLineNum + "\r\n"
                                             + "错误字段数据：" + errorLineStr + "\r\n"
                                             + "请检查是否满足格式举例：BO_ 1127 CDU_DCDC_1: 24 CDU");
                        }

                        break;
                    }
                case "SG_":
                    {
                        try
                        {
                            //格式举例：
                            //普通帧信号格式： SG_ OBC_ChgCurr : 23|16@0+ (0.05,0) [0|400] "A"  Vector__XXX
                            //复用帧信号格式： SG_ AAA00_DcdcInputVolt m0 : 0|16@1+ (0.1,0) [0|6553.6] "" EXECU
                            if (isMessageValid)
                            {
                                uint byteOffset = 0;
                                CanSignal signal = new CanSignal();

                                signal.sigName = lineAry[1];
                                if (lineAry[2] == ":")//普通帧
                                {
                                    //TODO: 复用信号标志位：signal.multiplexerIndicator = -2;
                                    byteOffset = 0;
                                }
                                else//复用帧
                                {
                                    byteOffset = 1;
                                    /* TODO: 复用信号标志位
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
                                        return ExceptionHandler.Report("Dbc信号格式错误");
                                    }
                                    */
                                }

                                string[] sp = lineAry[3 + byteOffset].Split(new char[] { '|', '@' }, StringSplitOptions.RemoveEmptyEntries);

                                signal.sigStartBit = Convert.ToUInt32(sp[0]);
                                signal.sigLen = Convert.ToUInt32(sp[1]);
                                if (sp[2][0] == '0')
                                {
                                    signal.sigOrderType = 0;
                                }
                                else if (sp[2][0] == '1')
                                {
                                    signal.sigOrderType = 1;
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
                                //最大最小值
                                //signal.minimum = Convert.ToDouble(sp2[0]);
                                //signal.maximum = Convert.ToDouble(sp2[1]);

                                //信号单位
                                //signal.uintStr = lineAry[6 + byteOffset];

                                //信号接收节点
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
                            MessageBox.Show("解析SG_字段格式失败！" + "\r\n"
                                             + "错误字段行数：" + errorLineNum + "\r\n"
                                             + "错误字段数据：" + errorLineStr + "\r\n"
                                             + "请检查是否满足格式举例：SG_ OBC_ChgCurr : 23|16@0+ (0.05,0) [0|400] \"A\"  Vector__XXX");
                        }

                        break;
                    }
                case "BA_":
                    {
                        //获取报文属性-报文周期
                        //格式举例：BA_ "GenMsgCycleTime" BO_ 1118 100;
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
                                MessageBox.Show("解析BA_字段报文周期格式失败！" + "\r\n"
                                                 + "错误字段行数：" + errorLineNum + "\r\n"
                                                 + "错误字段数据：" + errorLineStr + "\r\n"
                                                 + "请检查是否满足格式举例：BA_ \"GenMsgCycleTime\" BO_ 1118 100;");
                            }
                        }
                        else if ((lineAry[1].Replace("\"", "") == "VFrameFormat") && (lineAry[2] == "BO_"))
                        {
                            //获取报文属性-报文帧类型
                            //格式举例：BA_ "VFrameFormat" BO_ 520 14;（0:Standard-CAN; 1:Externed-CAN; 14:Standard-CANFD; 15:Externed-CANFD）
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

                                //扩展帧ID需要处理一下
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
                                MessageBox.Show("解析BA_字段报文帧类型格式失败！" + "\r\n"
                                                 + "错误字段行数：" + errorLineNum + "\r\n"
                                                 + "错误字段数据：" + errorLineStr + "\r\n"
                                                 + "请检查是否满足格式举例：BA_ \"VFrameFormat\" BO_ 520 14;");
                            }
                        }
                        else { }

                        break;
                    }

            }
        }
        isLoadCfg = true;

        AppLogMng.DisplayLog("从Excel文件导入通信协议成功!", true);
    }

    //清除已经存在的DBC配置
    private void ResetCanDbcCfg()
    {
        canMsgSet.Clear();
        isLoadCfg = false;
    }
}

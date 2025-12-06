using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Linq;

static public class GenerateXml
{
    static public void GenerateXmlForCanMatrix()
    { 
        CanRoot canRoot = new CanRoot();

        CanInfo canInfo = new CanInfo();
        canRoot.canInfo = canInfo;
        canInfo.MessageFlag_Max = 1000;
        canInfo.SignalFlag_Max = 1000;

        CanBus canBus = new CanBus();
        canInfo.canBus = canBus;
        canBus.CANMAP = 0;
        canBus.Name = "CAN BUS";
        canBus.Braud_normal = 11;
        canBus.Braud_data = 15;
        canBus.SamplePoint_normal = 80;
        canBus.SamplePoint_data = 80;
        canBus.EventDUT = 0;

        canBus.RxMessages = new List<Message>();
        canBus.TxMessages = new List<Message>();

        int msgCount = 1;
        int sigCount = 1;
        //默认添加诊断开启调试报文
        Message debugMsg = new Message();
        debugMsg.Flag = msgCount;
        msgCount++;
        debugMsg.Name = "AAA_Report";
        debugMsg.ECU = "UDS";
        debugMsg.ID = 2022;//0x7E6
        debugMsg.CycleTime = 1000;
        debugMsg.DLC = 8;
        debugMsg.Extern = 0;
        debugMsg.CANFD = 1;
        debugMsg.MultiplexingBit = 0;
        debugMsg.MultiplexingLength = 0;
        debugMsg.MultiplexingData = 0;
        debugMsg.Field = "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";

        debugMsg.signals = new List<Signal>();
        Signal debugSig= new Signal();
        debugSig.Flag = sigCount;
        sigCount++;
        debugSig.SingalType = 1;//0：常数；1：Enum
        debugSig.Name = "AAA_Report";
        debugSig.Description = "AAA调试报文开启";
        debugSig.Unit = "";
        debugSig.BitOrder = 0;
        debugSig.Signed = 0;
        debugSig.StartBit = 0;
        debugSig.BitLength = 32;
        debugSig.Resolution = 1;
        debugSig.Offset = 0;
        debugSig.MinData = 0;
        debugSig.MaxData = 1;
        debugSig.DefaultData = 0;
        debugSig.ChecksumAlgorithm = 0;

        debugSig.EnumStrings = new List<EnumString>();
        debugSig.EnumStrings.Add(new EnumString() {Data= 108547,String = "Report_AAA",Color = 16777215 });
        debugSig.EnumStrings.Add(new EnumString() {Data = 43011,String = "No_Report_AAA",Color = 16777215 });

        debugMsg.signals.Add(debugSig);
        canBus.TxMessages.Add(debugMsg);

        //从导入的Can矩阵中获取Can通信协议信息
        List<CanMessage> _debugMsgs = new List<CanMessage>();
        List<CanMessage> _appMsgs = new List<CanMessage>();
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
        {
            //复用帧报文xml生成
            if (item.msgType == (uint)CanMsgType.DEBUG)
            {
                //确认最大复用帧ID
                uint _maxFrameID = 0;
                foreach (var item1 in item.signals)
                {
                    if (item1.reuseFrameID >= _maxFrameID)
                        _maxFrameID = item1.reuseFrameID;
                }

                //遍历复用帧报文数据，按照帧ID进行信号分组
                for (int i = 0; i < _maxFrameID; i++)
                {
                    CanMessage message = new CanMessage();
                    message.msgName = "AAA_" + i.ToString();
                    message.msgId = item.msgId;
                    message.msgCycle = item.msgCycle;
                    message.isExternId = item.isExternId;
                    message.msgSize = item.msgSize;
                    message.transmitter = item.transmitter;
                    message.msgType = item.msgType;
                    message.signals = new List<CanSignal>();

                    for (int j = 0; j < item.signals.Count; j++)
                    {
                        if (item.signals[j].reuseFrameID == i)
                        {
                            message.signals.Add(item.signals[j]);
                        }

                    }

                    _debugMsgs.Add(message);
                }
            }
            else
            {
                _appMsgs.Add(item);
            }
        }

        List<CanMessage> _xmlDbcMsgs = new List<CanMessage>();
        int _xmlMsgType = 0;//0:APP; 1: Debug-复用帧

        //判断第一个报文为复用帧调试报文，则默认全部帧按照复用帧格式生成
        if (CanDbcDataManager.GetInstance().canMsgSet.First().Value.msgType == (uint)CanMsgType.DEBUG)
        {
            _xmlDbcMsgs = _debugMsgs;
            _xmlMsgType =1;
        }
        else 
        {
            _xmlDbcMsgs = _appMsgs;
            _xmlMsgType = 0;
        }

        //生成xml文本数据
        foreach (var item in _xmlDbcMsgs)
        {
            //设置报文信息
            Message msg = new Message();
            msg.Flag = msgCount;
            msgCount++;
            msg.Name = item.msgName;
            msg.ECU = item.transmitter;
            msg.ID = (int)item.msgId;
            msg.CycleTime = (int)item.msgCycle;
            msg.DLC = (int)item.msgSize;
            msg.Extern = item.isExternId ? 1 : 0;
            //设置帧格式
            if (item.msgType == 0)
            {
                msg.CANFD = 1;
            }
            else
            {
                msg.CANFD = 0;
            }
            //设置复用帧信息
            if (_xmlMsgType == 0)
            {
                msg.MultiplexingBit = 0;
                msg.MultiplexingLength = 0;
                msg.MultiplexingData = 0;
            }
            else
            {
                msg.MultiplexingBit = 56;
                msg.MultiplexingLength = 8;
                msg.MultiplexingData = (int)item.signals[0].reuseFrameID;
            }
            msg.Field = "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";

            //设置信号信息
            msg.signals = new List<Signal>();
            foreach (var item1 in item.signals)
            {
                Signal sig = new Signal();
                sig.Flag = sigCount;
                sigCount++;
                sig.SingalType = 0;//0：常数；1：Enum
                sig.Name = item1.sigName;
                sig.Description = item1.sigDesc;
                sig.Unit = "";
                sig.BitOrder = 1;
                sig.Signed = 0;
                sig.StartBit = (int)item1.sigStartBit;
                sig.BitLength = (int)item1.sigLen;
                sig.Resolution = item1.sigFactor;
                sig.Offset = item1.sigOffset;
                sig.MinData = 0;
                sig.MaxData = 1;
                sig.DefaultData = 0;
                sig.ChecksumAlgorithm = 0;

                sig.EnumStrings = new List<EnumString>();
                //设置值枚举信息
                if ((item1.sigValueTable != null) && (item1.sigValueTable.Count > 0))
                {
                    sig.SingalType = 1;//0：常数；1：Enum

                    foreach (var item2 in item1.sigValueTable)
                    {
                        EnumString enumTable = new EnumString();

                        enumTable.Data = item2.Key;
                        enumTable.String = item2.Value;
                        enumTable.Color = 16777215;

                        sig.EnumStrings.Add(enumTable);
                    }
 
                }

                msg.signals.Add(sig);
            }

            if ((item.transmitter == "OBC") || (item.transmitter == "DCDC"))//对上位机来说为接收报文
            {
                canBus.RxMessages.Add(msg);
            }
            else
            {
                canBus.TxMessages.Add(msg);
            }
        }

        //根据CanXml结构类信息生成Xml文本信息
        string xmlData = XmlSerializeHelper.XmlSerialize(canRoot);
        //保存文本信息
        TextOperation.WriteData("GenerateXml", FileType.XML, xmlData);
    }


}


[XmlRootAttribute("root", IsNullable = false)]
public class CanRoot
{
    [XmlElementAttribute("CANDB", IsNullable = false)]
    public CanInfo canInfo { get; set; }
}

public class CanInfo
{
    [XmlAttribute("SignalFlag_Max")]
    public int SignalFlag_Max { get; set; }

    [XmlAttribute("MessageFlag_Max")]
    public int MessageFlag_Max { get; set; }

    [XmlElementAttribute("CANBUS", IsNullable = false)]
    public CanBus canBus { get; set; }

}

public class CanBus
{
    [XmlAttribute("CANMAP")]
    public int CANMAP { get; set; }

    [XmlAttribute("Name")]
    public string Name { get; set; }

    [XmlAttribute("Braud_normal")]
    public int Braud_normal { get; set; }

    [XmlAttribute("Braud_data")]
    public int Braud_data { get; set; }

    [XmlAttribute("SamplePoint_normal")]
    public int SamplePoint_normal { get; set; }

    [XmlAttribute("SamplePoint_data")]
    public int SamplePoint_data { get; set; }

    [XmlAttribute("EventDUT")]
    public int EventDUT { get; set; }

    [XmlArrayAttribute("TxMessages")]
    public List<Message> TxMessages { get; set; }

    [XmlArrayAttribute("RxMessages")]
    public List<Message> RxMessages { get; set; }
}

[XmlRootAttribute("Message", IsNullable = false)]
public class Message
{
    [XmlAttribute("Flag")]
    public int Flag { get; set; }

    [XmlAttribute("Name")]
    public string Name { get; set; }

    [XmlAttribute("ECU")]
    public string ECU { get; set; }

    [XmlAttribute("ID")]
    public int ID { get; set; }

    [XmlAttribute("CycleTime")]
    public int CycleTime { get; set; }

    [XmlAttribute("DLC")]
    public int DLC { get; set; }

    [XmlAttribute("Extern")]
    public int Extern { get; set; }

    [XmlAttribute("CANFD")]
    public int CANFD { get; set; }

    [XmlAttribute("MultiplexingBit")]
    public int MultiplexingBit { get; set; }

    [XmlAttribute("MultiplexingLength")]
    public int MultiplexingLength { get; set; }

    [XmlAttribute("MultiplexingData")]
    public int MultiplexingData { get; set; }

    [XmlAttribute("Field")]
    public string Field { get; set; }

    [XmlElementAttribute("Signal", IsNullable = false)]
    public List<Signal> signals { get; set; }
}

[XmlRootAttribute("Signal", IsNullable = false)]
public class Signal
{
    [XmlAttribute("Flag")]
    public int Flag { get; set; }

    [XmlAttribute("SingalType")]
    public int SingalType { get; set; }

    [XmlAttribute("Name")]
    public string Name { get; set; }

    [XmlAttribute("Description")]
    public string Description { get; set; }

    [XmlAttribute("Unit")]
    public string Unit { get; set; }

    [XmlAttribute("BitOrder")]
    public int BitOrder { get; set; }

    [XmlAttribute("Signed")]
    public int Signed { get; set; }

    [XmlAttribute("StartBit")]
    public int StartBit { get; set; }

    [XmlAttribute("BitLength")]
    public int BitLength { get; set; }

    [XmlAttribute("Resolution")]
    public double Resolution { get; set; }

    [XmlAttribute("Offset")]
    public double Offset { get; set; }

    [XmlAttribute("MinData")]
    public int MinData { get; set; }

    [XmlAttribute("MaxData")]
    public int MaxData { get; set; }

    [XmlAttribute("DefaultData")]
    public int DefaultData { get; set; }

    [XmlAttribute("ChecksumAlgorithm")]
    public int ChecksumAlgorithm { get; set; }

    [XmlArrayAttribute("EnumStrings")]
    public List<EnumString> EnumStrings { get; set; }

}

[XmlRootAttribute("EnumString", IsNullable = false)]
public class EnumString
{
    [XmlAttribute("Data")]
    public int Data { get; set; }

    [XmlAttribute("String")]
    public string String { get; set; }

    [XmlAttribute("Color")]
    public int Color { get; set; }
}

/// <summary>
/// XML序列化公共处理类
/// </summary>
public static class XmlSerializeHelper
{
    /// <summary>
    /// 将实体对象转换成XML
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="obj">实体对象</param>
    public static string XmlSerialize<T>(T obj)
    {
        try
        {
            using (StringWriter sw = new StringWriter())
            {
                Type t = obj.GetType();
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(sw, obj);
                sw.Close();
                return sw.ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("将实体对象转换成XML异常", ex);
        }
    }

    /// <summary>
    /// 将XML转换成实体对象
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="strXML">XML</param>
    public static T DESerializer<T>(string strXML) where T : class
    {
        try
        {
            using (StringReader sr = new StringReader(strXML))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return serializer.Deserialize(sr) as T;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("将XML转换成实体对象异常", ex);
        }
    }
}
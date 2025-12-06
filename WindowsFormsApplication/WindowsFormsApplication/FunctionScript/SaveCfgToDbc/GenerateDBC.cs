using System;
using System.Collections;
using System.Collections.Generic;

public class GenerateDBC
{
    /// <summary>
    /// 根据APP已经识别到的CAN矩阵信息生成DBC文件
    /// </summary>
    /// <returns>DBC内容</returns>
    static public string GenerateDbcForCanMatrix()
    {
        //如果未加载配置，则直接退出
        if (CanDbcDataManager.GetInstance().isLoadCfg == false)
        {
            return null;
        }

        //生成DBC文本内容
        string allTxt = "";

        //生成版本号信息
        allTxt += GntVer();
        //生成NewSymbol新符号内容
        allTxt += GntNS_();
        //生成baudrate波特率相关内容
        allTxt += GntBS_();
        //生成ECU节点内容
        List<string> nodes = new List<string>();
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet)
        {
            //相同的节点就不重复写了
            if (nodes.Count > 0)
            {
                if (item.Value.transmitter != nodes[nodes.Count - 1])
                {
                    nodes.Add(item.Value.transmitter);
                }
            }
            else
            {
                nodes.Add(item.Value.transmitter);
            }
        }
        allTxt += GntBU_(nodes);

        //生成报文及信号内容
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet)
        {
            //生成报文信息
            allTxt += GntBO_(item.Value);
            //生成报文包含的所有信号
            foreach (var item1 in item.Value.signals)
            {
                allTxt += " ";
                allTxt += GntSG_(item1,item.Value);
            }

            allTxt += "\n";
        }

        //生成信号注释
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet)
        {
            foreach (var item1 in item.Value.signals)
            {
                //生成报文信息
                allTxt += GntCM_(item.Value.msgId, item1);
            }
        }

        //默认设置报文为Canfd标准帧格式
        allTxt += "\r\n";
        allTxt += "BA_DEF_ BO_  \"VFrameFormat\" INT 0 15;" + "\r\n";
        allTxt += "BA_DEF_DEF_  \"VFrameFormat\" 14;" + "\r\n";
        allTxt += "\r\n";

        //生成信号值列表
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet)
        {
            foreach (var item1 in item.Value.signals)
            {
                //生成报文信息
                allTxt += GntVAL_(item.Value.msgId, item1);
            }
        }

        //如果使用Debug-复用帧模式 DBC需要添加信号组数据
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet)
        {
            if(item.Value.msgType == (uint)CanMsgType.DEBUG)
                allTxt += GntSigGroup_ReuseFrame(item.Value);
        };

        //生成DBC文档
        return allTxt;
    }

    /// <summary>
    /// 生成版本内容内容
    /// </summary>
    /// <returns></returns>
    static private string GntVer()
    {
        string ver = "VERSION" +" "+ "\"V0.0.1\"";
        ver += "\r\n";//换行

        return ver;
    }

    /// <summary>
    /// 生成NewSymbol新符号内容
    /// </summary>
    /// <returns></returns>
    static private string GntNS_()
    {
        string NS_ =
              "NS_ :\r\n\t"
              + "NS_DESC_\r\n\t"
              + "CM_\r\n\t"
              + "BA_DEF_\r\n\t"
              + "BA_\n\t"
              + "VAL_\n\t"
              + "CAT_DEF_\n\t"
              + "CAT_\n\t"
              + "FILTER\n\t"
              + "BA_DEF_DEF_\n\t"
              + "EV_DATA_\n\t"
              + "ENVVAR_DATA_\n\t"
              + "SGTYPE_\n\t"
              + "SGTYPE_VAL_\n\t"
              + "BA_DEF_SGTYPE_\n\t"
              + "BA_SGTYPE_\n\t"
              + "SIG_TYPE_REF_\n\t"
              + "VAL_TABLE_\n\t"
              + "SIG_GROUP_\n\t"
              + "SIG_VALTYPE_\n\t"
              + "SIGTYPE_VALTYPE_\n\t"
              + "BO_TX_BU_\n\t"
              + "BA_DEF_REL_\n\t"
              + "BA_REL_\n\t"
              + "BA_DEF_DEF_REL_\n\t"
              + "BU_SG_REL_\n\t"
              + "BU_EV_REL_\n\t"
              + "BU_BO_REL_\n\t"
              + "SG_MUL_VAL_";

        NS_ += "\r\n";//换行

        return NS_;
    }

    /// <summary>
    /// 生成baudrate波特率相关内容
    /// </summary>
    /// <returns></returns>
    static private string GntBS_()
    {
        string BS_ = "BS_ :";
        BS_ += "\r\n";//换行
        return BS_;

    }

    /// <summary>
    /// 生成ECU节点内容
    /// </summary>
    /// <param name="ecuName">包含所有节点名的数组</param>
    /// <returns></returns>
    static private string GntBU_(List<string> ecuName)
    {
        string BU_ = "BU_: ";
 
        foreach (var item in ecuName)
        {
            BU_ += item + " ";
        }
        BU_ += "\r\n";//换行
        return BU_;
    }

    /// <summary>
    /// 生成DBC信号行
    /// </summary>
    /// <param name="sig">CAN信号结构体</param>
    /// <returns></returns>
    static private string GntSG_(CanSignal sig,CanMessage msg)
    {
        string SG_ = "SG_ ";

        SG_ += sig.sigName + " ";
        //调试报文需要添加复用帧信息
        if (msg.msgType == (uint)CanMsgType.DEBUG)
        {
            SG_ += "m" + sig.reuseFrameID + " ";
        }
        SG_ += ":" + " ";
        //Motorola/Intel起始位计算
        if (sig.sigOrderType == 0)
        {
            //Motorol
            SG_ += CanOrderTool.MotorolaStartBit_Lsb2Msb((int)sig.sigStartBit, (int)sig.sigLen).ToString() + "|";
        }
        else
        {
            //Intel
            SG_ += sig.sigStartBit.ToString() + "|";
        }
        SG_ += sig.sigLen.ToString() + "@";
        SG_ += sig.sigOrderType + " ";
        SG_ += ((sig.valueType == 0) ? "+" : "-") + " ";
        SG_ += "(" + sig.sigFactor + "," + sig.sigOffset + ")" + " "; 
        SG_ += "[" + "0"+ "|" +  System.Math.Pow(2, sig.sigLen)*sig.sigFactor + "]" + " ";
        SG_ += "\"\"" + " ";  //暂不支持生成单位
        if (sig.recvNode == null || sig.recvNode == "")
        {
            SG_ += "TBD";
        }
        else
        {
            SG_ += sig.recvNode;
        }
        SG_ += "\r\n";//换行

        return SG_;
    }

    /// <summary>
    /// 生成DBC报文行
    /// </summary>
    /// <param name="sig">CAN报文信息结构体</param>
    /// <returns></returns>
    static private string GntBO_(CanMessage msg)
    {
        string BO_ = "BO_ ";

        BO_ += msg.msgId.ToString() + " ";
        BO_ += msg.msgName+ ":" + " ";
        BO_ += msg.msgSize.ToString() + " ";
        BO_ += msg.transmitter;
        BO_ += "\r\n";//换行

        return BO_;
    }

    /// <summary>
    /// 生成DBC信号注释行
    /// </summary>
    static private string GntCM_(uint msgid, CanSignal sig)
    {
        string CM_ = "CM_ ";

        CM_ += "SG_" + " ";
        CM_ += msgid.ToString() + " ";
        CM_ += sig.sigName + " ";
        CM_ += "\"" + sig.sigDesc + "\"" + ";";
        CM_ += "\r\n";//换行

        return CM_;
    }

    /// <summary>
    /// 生成DBC信号值列表
    /// </summary>
    static private string GntVAL_(uint msgid, CanSignal sig)
    {
        string VAL_ = "";

        if (sig.sigValueTable != null)
        {
            VAL_ = "VAL_ ";
            VAL_ += msgid.ToString() + " ";
            VAL_ += sig.sigName + " ";
            foreach (var item in sig.sigValueTable)
            {
                VAL_ += item.Key + " ";
                VAL_ += "\"" + item.Value + "\"" + " ";
            }
            VAL_ = VAL_.Remove(VAL_.LastIndexOf(" "), 1);//去除最后一个空格

            VAL_ += ";";
            VAL_ += "\r\n";//换行
        }



        return VAL_;
    }

    /// <summary>
    /// 适用复用帧，生成复用帧信号组信息
    /// </summary>
    static private string GntSigGroup_ReuseFrame(CanMessage msg)
    {
        string SIG_GROUP_ = "";

        //确认最大复用帧ID
        uint maxFrameID = 0;
        foreach (var item in msg.signals)
        {
            if (item.reuseFrameID >= maxFrameID) 
                maxFrameID = item.reuseFrameID;
        }

        //遍历复用帧报文数据，按照帧ID进行信号分组
        for (int i = 0; i < maxFrameID; i++)
        {
            SIG_GROUP_ += "SIG_GROUP_" + " " + msg.msgId.ToString() + " " + "Signal_Group_" + i + " 1 :";
            foreach (var item in msg.signals)
            {
                if (item.reuseFrameID == i)
                    SIG_GROUP_ += " " + item.sigName;
            }
            SIG_GROUP_ += ";\r\n";
        }

        return SIG_GROUP_;
    }

}

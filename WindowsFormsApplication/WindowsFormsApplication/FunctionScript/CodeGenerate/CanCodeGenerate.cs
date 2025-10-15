using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

//信号代码及索引
public class SigCode
{
    public uint msgId; //报文ID
    public string sigName;//信号名称
    public string codeStr;//代码块字符串
}

public class CanCodeGenerate
{
    static string fourSpace = "    ";
    static string changeLine = "\r\n";

    /// <summary>
    /// 生成所有Can信号代码
    /// </summary>
    static public void GenerateAllCanCode()
    {
        FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
        folderBrowser.SelectedPath = ".";
        folderBrowser.Description = "请选择保存目录";

        if (folderBrowser.ShowDialog() == DialogResult.OK)
        {
            //自动生成代码
            string dbcContent = "";
            string savePath = folderBrowser.SelectedPath;

            //生成报文具体信号策略文件
            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet)
            {
                //如果是OBC和DCDC发送的，则按照发送信号代码模板生成，否则按照接收模板生成
                if (item.Value.transmitter == "OBC" || item.Value.transmitter == "DCDC")
                {
                    dbcContent = CanCodeGenerate.Gnt_MsgFunFile_Tx_ByCfg(item.Value);
                }
                else
                {
                    dbcContent = CanCodeGenerate.Gnt_MsgFunFile_Rx(item.Value);
                }

                if (dbcContent == null)
                {
                    return;
                }
                string fileName = "CanMsgFun" + item.Value.msgId.ToString("x3").ToUpper();
                TextOperation.WriteData(savePath, fileName, FileType.C_Code, dbcContent);
            }

            //生成CanMsgLocal配置.h文件
            dbcContent = CanCodeGenerate.Gnt_CanMsgLocal_H();
            string name1 = "CanMsgLocal";
            TextOperation.WriteData(savePath, name1, FileType.C_Head, dbcContent);

            //生成CanMsgInterface配置.h文件
            dbcContent = CanCodeGenerate.Gnt_CanMsgInterface_H();
            string name2 = "CanMsgInterface";
            TextOperation.WriteData(savePath, name2, FileType.C_Head, dbcContent);
            //生成CanMsgCfg配置.c文件
            dbcContent = CanCodeGenerate.Gnt_CanMsgCfg_C();
            string name3 = "CanMsgCfg";
            TextOperation.WriteData(savePath, name3, FileType.C_Code, dbcContent);

        }
    }

    /// <summary>
    /// 获取发送报文生成内容
    /// </summary>
    /// <param name="msg">报文信息</param>
    /// <returns>生成的代码字符串</returns>
    static public string Gnt_MsgFunFile_Tx(CanMessage msg)
    {
        string retVal = "";
        //生成文件头，包含版本信息和生成时间,
        retVal += GntCommentRow(" (C) Copyright, JWPENG. " + "Time:" + System.DateTime.Now.ToString());
        //生成头文件包含
        retVal += GntCommentRow(" Includes ");
        retVal += "#include \"CanMsgLocal.h\"" + changeLine;
        retVal += "#include \"CanMsgInterface.h\"" + changeLine;
        //生成信号值存贮的变量
        retVal += GntCommentRow(" Private variable ");
        retVal +=  "static " + msg.transmitter + "_" + msg.msgId.ToString("x3").ToUpper() + "_TX_SIG " + msg.transmitter + msg.msgId.ToString("x3") + "Msg = {0};" + changeLine;
        retVal += changeLine;


        //生成报文发送函数
        retVal += "u32 MsgFun" + msg.msgId.ToString("x3").ToUpper() + "(u32 ulIndex, u32 ulParam)" + changeLine;
        retVal += "{" + changeLine;
        retVal += fourSpace + "u32 ulRetValue = 0;" + changeLine;
        retVal += fourSpace + "switch (ulIndex)" + changeLine;
        retVal += fourSpace + "{" + changeLine;
        //生成每个信号发送的代码
        foreach (var item in msg.signals)
        {
            string sigType = GetSigValueTypeByLen(item.sigLen);
            retVal += fourSpace + fourSpace + "case "+ msg.transmitter.ToUpper() + "_" + item.sigName + ":" + changeLine;
            retVal += fourSpace + fourSpace + fourSpace + "ulRetValue = 0;" + changeLine;
            retVal += fourSpace + fourSpace + fourSpace + "break;" + changeLine;
        }
        retVal += fourSpace + fourSpace + "default:" + changeLine;
        retVal += fourSpace + fourSpace + fourSpace + "break;" + changeLine;
        retVal += fourSpace + "}" + changeLine;
        retVal += fourSpace + "return ulRetValue;" + changeLine;
        retVal += "}" + changeLine;

        return retVal;
    }

    //通过信号配置生成发送代码
    static public string Gnt_MsgFunFile_Tx_ByCfg(CanMessage msg /*, List<SigCode> sigCodeList*/)
    {
        string retVal = "";
        //生成文件头，包含版本信息和生成时间,
        retVal += GntCommentRow(" (C) Copyright, JWPENG. " + "Time:" + System.DateTime.Now.ToString());
        //生成头文件包含
        retVal += GntCommentRow(" Includes ");
        retVal += "#include \"CanMsgLocal.h\"" + changeLine;
        retVal += "#include \"CanMsgInterface.h\"" + changeLine;
        //生成信号值存贮的变量
        retVal += GntCommentRow(" Private variable ");
        retVal += "static " + msg.transmitter + "_" + msg.msgId.ToString("x3").ToUpper() + "_TX_SIG " + msg.transmitter + msg.msgId.ToString("x3") + "Msg = {0};" + changeLine;
        retVal += changeLine;


        //生成报文发送函数
        retVal += "u32 MsgFun" + "_" + msg.msgId.ToString("x3").ToUpper() + "(u32 ulIndex, u32 ulParam)" + changeLine;
        retVal += "{" + changeLine;
        retVal += fourSpace + "u32 ulRetValue = 0;" + changeLine;
        retVal += fourSpace + "switch (ulIndex)" + changeLine;
        retVal += fourSpace + "{" + changeLine;
        //生成每个信号发送的代码
        foreach (var item in msg.signals)
        {
            string sigType = GetSigValueTypeByLen(item.sigLen);
            retVal += fourSpace + fourSpace + "case " + msg.transmitter.ToUpper() + "_" + item.sigName + ":" + changeLine;
            //生成具体发送代码的策略
            string spaceStr = fourSpace + fourSpace + fourSpace;
            string codeStr = "";
            //发送函数代码块，有需求可以实现
            //foreach (var item1 in sigCodeList)
            //{
            //    if (item1.msgId == msg.msgId && item1.sigName == item.sigName)
            //    {
            //        codeStr = item1.codeStr;
            //    }
            //}
            string[] bufferAry = codeStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (codeStr == "")
            {
                retVal += spaceStr + "ulRetValue = 0;" + changeLine;
            }
            else
            {
                for (int i = 0; i < bufferAry.Length; i++)
                {
                    retVal += spaceStr + bufferAry[i] + changeLine;
                }

            }

            retVal += fourSpace + fourSpace + fourSpace + "break;" + changeLine;
        }
        retVal += fourSpace + fourSpace + "default:" + changeLine;
        retVal += fourSpace + fourSpace + fourSpace + "break;" + changeLine;
        retVal += fourSpace + "}" + changeLine;
        retVal += fourSpace + "return ulRetValue;" + changeLine;
        retVal += "}" + changeLine;

        return retVal;
    }

    static public string Gnt_MsgFunFile_Rx(CanMessage msg)
    {
        string retVal = "";
        //生成文件头，包含版本信息和生成时间,
        retVal += GntCommentRow(" (C) Copyright, JWPENG. " + "Time:" + System.DateTime.Now.ToString());
        //生成头文件包含
        retVal += GntCommentRow(" Includes ");
        retVal += "#include \"CanMsgLocal.h\"" + changeLine;
        retVal += "#include \"CanMsgInterface.h\"" + changeLine;
        //生成信号值存贮的变量
        retVal += GntCommentRow(" Private variable ");
        retVal += "static " + msg.transmitter + "_" + msg.msgId.ToString("x3").ToUpper() + "_RX_SIG " + msg.transmitter + msg.msgId.ToString("x3") + "Msg = {0};" + changeLine;
        retVal += changeLine;


        //生成报文发送函数
        retVal += "u32 MsgFun"  + "_" + msg.msgId.ToString("x3").ToUpper() + "(u32 ulIndex, u32 ulParam)" + changeLine;
        retVal += "{" + changeLine;
        retVal += fourSpace + "u32 ulRetValue = 0;" + changeLine;
        retVal += fourSpace + "switch (ulIndex)" + changeLine;
        retVal += fourSpace + "{" + changeLine;
        //生成每个信号发送的代码
        foreach (var item in msg.signals)
        {
            string sigType = GetSigValueTypeByLen(item.sigLen);
            retVal += fourSpace + fourSpace + "case " + msg.transmitter.ToUpper() + "_" + item.sigName + ":" + changeLine;
            retVal += fourSpace + fourSpace + fourSpace + msg.transmitter + msg.msgId.ToString("x3") + "Msg" + "." + sigType + item.sigName + "= (" + sigType + ")ulParam;" + changeLine;
            retVal += fourSpace + fourSpace + fourSpace + "break;" + changeLine;
        }
        retVal += fourSpace + fourSpace + "default:" + changeLine;
        retVal += fourSpace + fourSpace + fourSpace + "break;" + changeLine;
        retVal += fourSpace + "}" + changeLine;
        retVal += fourSpace + "return 0;" + changeLine;
        retVal += "}" + changeLine;
        retVal +=  changeLine;

        //生成信号接口：提供接口给外部读取接收信号
        foreach (var item in msg.signals)
        {
            string sigType = GetSigValueTypeByLen(item.sigLen);

            retVal += sigType + " APP_CAN_"+ sigType + "Get" + msg.transmitter  + "_"+ item.sigName + "(void)" + changeLine;
            retVal += "{" + changeLine;
            retVal += fourSpace + "return " + msg.transmitter + msg.msgId.ToString("x3") + "Msg" + "." + sigType + item.sigName +";"  + changeLine;
            retVal += "}" + changeLine;
            retVal += changeLine;
        }

        return retVal;
    }

    static public string Gnt_CanMsgLocal_H()
    {
        string retVal = "";

        //生成文件头，包含版本信息和生成时间,
        retVal += GntCommentRow(" (C) Copyright, JWPENG. " + "Time:" + System.DateTime.Now.ToString());

        //生成 .h文件包含宏定义
        retVal += "#ifndef _CAMMSG_LOCAL_H_" + changeLine;
        retVal += "#define _CAMMSG_LOCAL_H_" + changeLine;

        //生成头文件包含
        retVal += GntCommentRow(" Includes ");
        retVal += "#include \"SysTypes.h\"" + changeLine;

        //生成信号枚举
        retVal += GntCommentRow(" Enum ");

        //根据DBC生成信号枚举
        int maxSigNameLen = 0;//最长信号名长度
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
        {
            foreach (var item1 in item.signals)
            {
                if (item1.sigName.Length > maxSigNameLen)
                {
                    //获取最长信名长度，用于代码对齐
                    maxSigNameLen= item1.sigName.Length;
                }
            }
        }
        maxSigNameLen += 10;

        int m = 0;
        //根据信号枚举生成代码
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
        {
            m = 0;
            retVal += "enum CAN_CLIENT_SIGNAL_" + item.transmitter.ToUpper() + item.msgId.ToString("x3").ToUpper() + changeLine;
            retVal += "{" + changeLine;

            foreach (var item1 in item.signals)
            {
                retVal += fourSpace + GetStringWithAssignLen(item.transmitter.ToUpper()+ "_" + item1.sigName, maxSigNameLen) 
                    + "= " + m.ToString() + "UL," + changeLine;
                m++;
            }

            retVal += "};" + changeLine;
            retVal +=  changeLine;
        }

        //根据DBC生成信号结构体
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
        {
            //生成信号结构体注释
            retVal += GntCommentRow(" @brief The " + "0x" + item.msgId.ToString("x3") + " Signal sent by " + item.transmitter);

            retVal += "typedef struct ";

            if (item.transmitter == "DCDC" || item.transmitter == "OBC")
            {
                retVal += "tag" + item.transmitter + "_" + item.msgId.ToString("x3").ToUpper() + "_TX_SIG" + changeLine;
            }
            else
            {
                retVal += "tag" + item.transmitter + "_" + item.msgId.ToString("x3").ToUpper() + "_RX_SIG" + changeLine;

            }

            retVal += "{" + changeLine;

            foreach (var item1 in item.signals)
            {
                string sigType = GetSigValueTypeByLen(item1.sigLen);
                retVal += fourSpace + sigType + " " + sigType + item1.sigName + ";" + changeLine;
            }

            if (item.transmitter == "DCDC" || item.transmitter == "OBC")
            {
                retVal += "}" + item.transmitter + "_" + item.msgId.ToString("x3").ToUpper() + "_TX_SIG" + ";" + changeLine;
            }
            else
            {
                retVal += "}" + item.transmitter + "_" + item.msgId.ToString("x3").ToUpper() + "_RX_SIG" + ";" + changeLine;

            }

            retVal += changeLine;

        }

        //生成信号功能函数声明
        retVal += GntCommentRow(" Private Function ");
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
        {
            retVal += "u32 MsgFun_" + item.msgId.ToString("x3").ToUpper() + "(u32 ulIndex, u32 ulParam);" + changeLine;
        }

        retVal += "#endif" + changeLine;

        return retVal;
    }

    static public string Gnt_CanMsgInterface_H()
    {
        string retVal = "";

        //生成文件头，包含版本信息和生成时间,
        retVal += GntCommentRow(" (C) Copyright, JWPENG. " + "Time:" + System.DateTime.Now.ToString());

        //生成 .h文件包含宏定义
        retVal += "#ifndef _CAMMSG_INTERFACE_H_" + changeLine;
        retVal += "#define _CAMMSG_INTERFACE_H_" + changeLine;

        //生成包含头文件
        retVal += GntCommentRow(" Includes ");
        retVal += " #include \"AppCan.h\"" + changeLine;

        //生成外部函数声明
        retVal += GntCommentRow(" Export function ");
        retVal += "const TS_CANMSG * getCanPublicMsgObj(void);" + changeLine;
        retVal += "u32 getCanPublicMsgObjSize(void);" + changeLine;

        //生成报文ID枚举
        retVal += GntCommentRow(" Export ENUM ");
        retVal += "enum CAN_CLIENT_ID_CanMsg" + changeLine;
        retVal += "{" + changeLine;
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
        {
            retVal += fourSpace + GetStringWithAssignLen("CAN_CLIENT_ID_" + item.transmitter + "_" + item.msgId.ToString("x3").ToUpper() ,30)
                + " = 0x00000" + item.msgId.ToString("x3").ToUpper() + "UL," + changeLine;
        }
        retVal += "};" + changeLine;

        //生成接收函数
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
        {
            retVal += GntCommentRow(" @brief The " + item.msgId.ToString("x3").ToUpper() + " Signal sent by " + item.transmitter);

            foreach (var item1 in item.signals)
            {
                string sigType = GetSigValueTypeByLen(item1.sigLen);

                retVal += sigType + " APP_CAN_" + sigType + "Get" + item.transmitter + "_" + item1.sigName + "(void);" + changeLine;
            }
            retVal += changeLine;
        }

        retVal += "#endif";

        return retVal;

    }

    static public string Gnt_CanMsgCfg_C()
    {
        string retVal = "";
        //生成文件头，包含版本信息和生成时间,
        retVal += GntCommentRow(" (C) Copyright, JWPENG. " + "Time:" + System.DateTime.Now.ToString());

        //生成包含头文件
        retVal += GntCommentRow(" Includes ");
        retVal += " #include \"CanMsgLocal.h\"" + changeLine;
        retVal += " #include \"CanMsgInterface.h\"" + changeLine;

        //检查信号名字最大字符
        int maxSigNameLen = 0;//最长信号名长度
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
        {
            foreach (var item1 in item.signals)
            {
                if (item1.sigName.Length > maxSigNameLen)
                {
                    //获取最长信名长度，用于代码对齐
                    maxSigNameLen = item1.sigName.Length;
                }
            }
        }
        maxSigNameLen += 10;

        //生成DBC信号配置矩阵
        retVal += GntCommentRow(" Private variables ");
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
        {
            retVal += "static const " + "TS_CANMSG_OBJ " + "DBC_" + item.transmitter + "_" + 
                item.msgId.ToString("x3").ToUpper() + "[" + item.signals.Count + "]" + " =" + changeLine;

            retVal += "{" + changeLine;
            foreach (var item1 in item.signals)
            {
                string tmpStr = GetStringWithAssignLen(item.transmitter + "_" + item1.sigName, maxSigNameLen);
                retVal += fourSpace + "{" + tmpStr +  " , " + "\"" + tmpStr + "\"" + " , " 
                    + GetStringWithAssignLen(item1.sigStartBit.ToString(), 8) + " , "
                    + GetStringWithAssignLen(item1.sigLen.ToString(), 8) + " , "
                    + "MsgFun_" + item.msgId.ToString("x3").ToUpper() + "}," + changeLine;
            }
            retVal += "};" + changeLine;

        }

        //生成通信总配置表
        retVal += "static const TS_CANMSG canMsgTab" + "[" + CanDbcDataManager.GetInstance().canMsgSet.Count + "] = " + changeLine;
        retVal += "{" + changeLine;
        int tmpNum = 0;
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
        {
            retVal += fourSpace + "{";
            retVal += tmpNum.ToString() + ", ";
            retVal += GetStringWithAssignLen("\"" + item.msgName + "\"", 20) + ",";
            retVal += GetStringWithAssignLen("CAN_CLIENT_ID_" + item.transmitter+ "_" + item.msgId.ToString("x3").ToUpper(), 30) + ",";
            retVal += GetStringWithAssignLen(item.msgCycle.ToString(), 10) + ",";
            retVal += GetStringWithAssignLen(item.msgSize.ToString(), 10) + ", ";
            retVal += "MOTOROLA_LSB, ";
            retVal += (item.transmitter == "OBC" || item.transmitter == "DCDC") ? "CANMSG_TYPE_T, " : "CANMSG_TYPE_R, ";
            retVal += "CANFD_BRS_FRAME_TYPE_STANDARD, ";
            retVal += "CAN_ID_MASK0, ";
            retVal += GetStringWithAssignLen("DBC_" + item.transmitter + "_" + item.msgId.ToString("x3").ToUpper(), 20) + ",";
            retVal += "sizeof(" + "DBC_" + item.transmitter + "_"+ item.msgId.ToString("x3").ToUpper() + ")/"
                        + "sizeof(" + "DBC_" + item.transmitter + "_" + item.msgId.ToString("x3").ToUpper() + "[0]), ";
            retVal += "NULL";
            retVal += "}," + changeLine;

            tmpNum++;
        }
        retVal += "};" + changeLine;
        retVal += changeLine;

        //生成配置表接口函数
        retVal += "const TS_CANMSG * getCanPublicMsgObj(void)" + changeLine;
        retVal += "{" + changeLine;
        retVal += fourSpace + "return canMsgTab;" + changeLine;
        retVal += "}" + changeLine;

        retVal += "u32 getCanPublicMsgObjSize(void)" + changeLine;
        retVal += "{" + changeLine;
        retVal += fourSpace + "return sizeof(canMsgTab) / sizeof(canMsgTab[0]);" + changeLine;
        retVal += "}" + changeLine;

        return retVal;
    
    }

    //生成一个指定长度的字符串，多的补充空格字符
    static private string GetStringWithAssignLen(string str, int assignLen)
    {
        string ret = str;

        while (ret.Length <= assignLen)
        {
            ret += " ";
        }

        return ret;
    }


    //根据信号长度获取该信号值的类型
    static private string GetSigValueTypeByLen(uint sigLen)
    {
        string ret = "u32";
        if (sigLen <= 8)
        {
            ret = "u08";
        }
        else if (sigLen <= 16)
        {
            ret = "u16";
        }
        else if (sigLen <= 32)
        {
            ret = "u32";
        }
        else
        { }

        return ret;
    }

    //生成一行注释,生成注释字符数为80
    static private string GntCommentRow(string cmt)
    {
        string ret = "/* ";
        ret += cmt;
        while (ret.Length < 80)
        {
            ret += "*";
        }
        ret += "*/" + changeLine;

        return ret;
    }

}

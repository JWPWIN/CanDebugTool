using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceInfo
{ 
    public string interfaceName;   //接口名称
    public string interfaceDesc;   //接口描述
    public string headFileName;    //接口需要包含的头文件名
    public string factor;          //接口返回值精度
    public string offset;          //接口返回值偏移
    public GameObject obj = null;         //接口实际对象

}

//excel列含义枚举
public enum InterfaceColumn
{ 
    InterfaceName,
    InterfaceDesc,
    HeadFileName,
    Factor,
    Offset
}

//信号代码及索引
public class SigCode
{
    public uint msgId; //报文ID
    public string sigName;//信号名称
    public string codeStr;//代码块字符串
}

public class CodeGenerateCfgWin
{
    //是否初始化
    bool isInit = false;

    //代码生成窗口预设体路径
    static readonly string panelPath = "Prefabs/CodeGenerateWin";

    //窗口面板对象
    GameObject panelObj;

    //上一次选中的信号配置按钮
    Button lastCfgBtn = null;

    //代码生成信号配置预设物路径
    static readonly string sigCellPath = "Prefabs/CodeGntSigCfgCell";

    //报文信息显示单元格预设物路径
    static readonly string msgInfoCellPath = "Prefabs/SigCellMsgInfo";

    //接口信息显示单元格预设物路径
    static readonly string interfaceInfoCellPath = "Prefabs/CodeInterfaceCell";

    //报文数据列表，存贮报文信息
    List<MsgCellData> msgDataList = new List<MsgCellData>();

    //信号显示单元格列表
    List<SigCellData> sigCfgCells = new List<SigCellData>();

    //信号配置内容显示对象
    GameObject sigViewContent;

    //接口函数配置内容显示对象
    GameObject interfaceViewContent;

    //代码编辑区域Text对象
    InputField codeEditFiled = null;

    //显示当前配置的信号信息的文本
    Text curCfgSigInfo = null;

    //上一个信号代码
    SigCode lastSigCode = null;

    //配置单元格列表
    List<GameObject> cfgCells = new List<GameObject>();

    //接口配置列表信息
    List<InterfaceInfo> interfaceInfoSet = new List<InterfaceInfo>();

    //信号代码块列表
    List<SigCode> sigCodeList = new List<SigCode>();

    // Start is called before the first frame update
    public CodeGenerateCfgWin()
    {
        isInit = false;

        panelObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(panelPath));
        //设置窗口显示位置
        panelObj.transform.SetParent(UITool.GetChildObjectByName(UITool.FindCanvas(), "DataDisplayWin").transform, false);

        //获取显示信号信息列表的区域
        GameObject obj = UITool.GetChildObjectByName(panelObj, "SelectSigPanel");
        obj = UITool.GetChildObjectByName(obj, "CodeGntView");
        obj = UITool.GetChildObjectByName(obj, "Viewport");
        sigViewContent = UITool.GetChildObjectByName(obj, "Content");

        //获取显示当前配置信号的Text
        GameObject obj1 = UITool.GetChildObjectByName(panelObj, "SigCfgPanel");
        obj1 = UITool.GetChildObjectByName(obj1, "CurCfgSigInfo");
        curCfgSigInfo = UITool.GetOrAddComponentInChildByName<Text>(obj1,"Text");

        //获取显示接口函数信息区域
        GameObject obj2 = UITool.GetChildObjectByName(panelObj, "SigCfgPanel");
        obj2 = UITool.GetChildObjectByName(obj2, "LookInterfaceWin");
        obj2 = UITool.GetChildObjectByName(obj2, "InterfaceView");
        obj2 = UITool.GetChildObjectByName(obj2, "Viewport");
        interfaceViewContent = UITool.GetChildObjectByName(obj2, "Content");

        //设置加载接口函数描述信息按钮监听
        GameObject obj3 = UITool.GetChildObjectByName(panelObj, "SigCfgPanel");
        UITool.GetOrAddComponentInChildByName<Button>(obj3, "LoadInterfaceBtn").onClick.AddListener(LoadInterfaceCfg);

        //获取代码编辑输入框对象的文本
        GameObject obj4 = UITool.GetChildObjectByName(panelObj, "SigCfgPanel");
        codeEditFiled = UITool.GetOrAddComponentInChildByName<InputField>(obj4, "SigCodeInput");

        //设置保存信号配置代码按钮回调
        UITool.GetOrAddComponentInChildByName<Button>(panelObj, "SaveCfgBtn").onClick.AddListener(() =>
        {
            //如果已经加载DBC，则保存配置
            if (isInit == true)
            {
                string changeLine = "\r\n";//换行
                //设置代码保存内容
                string content = "@brief: Can Signal Code" + changeLine;
                foreach (var item in sigCodeList)
                {
                    //设置代码块索引
                    content += "</Index>" + changeLine;
                    content += item.msgId.ToString() + "|" + item.sigName + changeLine;
                    content += "<Index/>" + changeLine;

                    //设置代码块内容
                    content += "</Code>" + changeLine;
                    content += item.codeStr+changeLine;
                    content += "<Code/>" + changeLine;

                }
                string savePath = ".";//.代表当前exe程序文件夹路径
                TextReadAndWrite.WriteData(savePath, "CanSigCodeCfg", FileType.Text, content);
            }
            else
            {
                LogMng.GetInstance().DisplayLog("未加载DBC，不能保存配置！");
            }
        });

        //设置加载信号配置代码按钮回调
        UITool.GetOrAddComponentInChildByName<Button>(panelObj, "LoadCfgBtn").onClick.AddListener(() =>
        {
            //如果已经加载DBC，则可以加载配置
            if (isInit == true)
            {
                string loadPath = FileLoadAndSave.LoadFile();
                string codeCfgContent = TextReadAndWrite.ReadData(loadPath);
                string[] bufferAry = codeCfgContent.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (codeCfgContent == null)
                {
                    LogMng.GetInstance().DisplayLog("配置文件是空的");
                }
                else if (bufferAry.Length < 1)
                {
                    LogMng.GetInstance().DisplayLog("配置文件长度错误");
                }
                else if (bufferAry[0] != "@brief: Can Signal Code")
                {
                    LogMng.GetInstance().DisplayLog("配置文件格式错误");
                }
                else
                {
                    //解析配置文件信息
                    for (int i = 1; i < bufferAry.Length - 5; i++)
                    {
                        if (bufferAry[i] == "</Index>")
                        {
                            string indexStr = bufferAry[i+1];
                            if (indexStr != "")
                            {
                                string[] tmpData = indexStr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                                foreach (var item in sigCodeList)
                                {
                                    if (item.msgId == uint.Parse(tmpData[0]) && item.sigName == tmpData[1])
                                    {
                                        string code = "";
                                        bool flag = false;
                                        for (int j = i + 3; j < bufferAry.Length; j++)
                                        {
                                            if (bufferAry[j] == "<Code/>")//代码块结束
                                            {
                                                break;
                                            }

                                            //读取配置代码开始标志
                                            if (flag == true)
                                            {
                                                code += bufferAry[j] + "\r\n";
                                            }

                                            if (bufferAry[j] == "</Code>")//代码块开始
                                            {
                                                flag = true;
                                            }
                                        }
                                        item.codeStr = code;

                                        break;

                                    }
                                }

                            }
                        }
                    }

                }
            }
            else
            {
                LogMng.GetInstance().DisplayLog("未加载DBC，不能加载配置！");
            }

        });

        //设置生成代码按钮回调
        UITool.GetOrAddComponentInChildByName<Button>(panelObj, "GntCfgCodeBtn").onClick.AddListener(() =>
        {
            //如果未加载DBC则停止代码生成
            if (CanDbcDataManager.GetInstance().isLoadCfg == false)
            {
                return;
            }

            //自动生成代码
            string dbcContent = "";
            string savePath = FileLoadAndSave.GetFolderPath();
            //生成报文具体信号策略文件
            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet)
            {
                //如果是OBC和DCDC发送的，则按照发送信号代码模板生成，否则按照接收模板生成
                if (item.Value.transmitter == "OBC" || item.Value.transmitter == "DCDC")
                {
                    dbcContent = CanCodeGenerate.Gnt_MsgFunFile_Tx_ByCfg(item.Value,sigCodeList);
                }
                else
                {
                    dbcContent = CanCodeGenerate.Gnt_MsgFunFile_Rx(item.Value);
                }

                if (dbcContent == null)
                {
                    return;
                }
                string fileName = "AppCanMsgFun" + item.Value.msgId.ToString("x3").ToUpper();
                TextReadAndWrite.WriteData(savePath, fileName, FileType.C_Code, dbcContent);
            }

            //生成CanMsgLocal配置.h文件
            dbcContent = CanCodeGenerate.Gnt_CanMsgLocal_H();
            string name1 = "CanMsgLocal";
            TextReadAndWrite.WriteData(savePath, name1, FileType.C_Head, dbcContent);

            //生成CanMsgInterface配置.h文件
            dbcContent = CanCodeGenerate.Gnt_CanMsgInterface_H();
            string name2 = "CanMsgInterface";
            TextReadAndWrite.WriteData(savePath, name2, FileType.C_Head, dbcContent);
            //生成CanMsgCfg配置.c文件
            dbcContent = CanCodeGenerate.Gnt_CanMsgCfg_C();
            string name3 = "CanMsgInterface";
            TextReadAndWrite.WriteData(savePath, name3, FileType.C_Code, dbcContent);


        });


        //设置仅生成代码框架按钮回调
        UITool.GetOrAddComponentInChildByName<Button>(panelObj, "AutoGenerateCode").onClick.AddListener(() =>
        {
            //如果未加载DBC则停止代码生成
            if (CanDbcDataManager.GetInstance().isLoadCfg == false)
            {
                LogMng.GetInstance().DisplayLog("未加载DBC,不能生成代码");
                return;
            }

            //自动生成代码
            string dbcContent = "";
            string savePath = FileLoadAndSave.GetFolderPath();
            //生成报文具体信号策略文件
            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet)
            {
                //如果是OBC和DCDC发送的，则按照发送信号代码模板生成，否则按照接收模板生成
                if (item.Value.transmitter == "OBC" || item.Value.transmitter == "DCDC")
                {
                    dbcContent = CanCodeGenerate.Gnt_MsgFunFile_Tx(item.Value);
                }
                else
                {
                    dbcContent = CanCodeGenerate.Gnt_MsgFunFile_Rx(item.Value);
                }

                if (dbcContent == null)
                {
                    return;
                }
                string fileName = "AppCanMsgFun" + item.Value.msgId.ToString("x3").ToUpper();
                TextReadAndWrite.WriteData(savePath, fileName, FileType.C_Code, dbcContent);
            }

            //生成CanMsgLocal配置.h文件
            dbcContent = CanCodeGenerate.Gnt_CanMsgLocal_H();
            string name1 = "CanMsgLocal";
            TextReadAndWrite.WriteData(savePath, name1, FileType.C_Head, dbcContent);

            //生成CanMsgInterface配置.h文件
            dbcContent = CanCodeGenerate.Gnt_CanMsgInterface_H();
            string name2 = "CanMsgInterface";
            TextReadAndWrite.WriteData(savePath, name2, FileType.C_Head, dbcContent);
            //生成CanMsgCfg配置.c文件
            dbcContent = CanCodeGenerate.Gnt_CanMsgCfg_C();
            string name3 = "CanMsgInterface";
            TextReadAndWrite.WriteData(savePath, name3, FileType.C_Code, dbcContent);

        });

    }

    /// <summary>
    /// 重置初始化参数
    /// </summary>
    public void ResetCfg()
    {
        isInit = false;
    }

    /// <summary>
    /// 设置面板是否使能使用
    /// </summary>
    /// <param name="active">是否使能</param>
    public void SetPanelActive(bool active)
    {
        panelObj.SetActive(active);
    }

    // 更新信号列表显示
    public void UpdateCfg()
    {
        //更新信号列表
        if (true == CanDbcDataManager.GetInstance().isLoadCfg
            && false == isInit)
        {
            ClearWin();//重置窗口
            isInit = true;

            //显示OBC/DCDC发送报文信号列表
            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet)
            {
                //如果报文是OBC或者DCDC发的，则对于上位机是接收报文
                if ((item.Value.transmitter == "OBC") || (item.Value.transmitter == "DCDC"))
                {
                    //首先显示报文信息
                    GameObject sigCell;
                    sigCell = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(msgInfoCellPath));
                    //显示报文名
                    UITool.GetOrAddComponentInChildByName<Text>(sigCell, "Name").text = item.Value.msgName + "/0x" + item.Value.msgId.ToString("x3").ToUpper();

                    sigCell.transform.SetParent(sigViewContent.transform);

                    //存储单元格
                    SigCellData sigCellData = new SigCellData();
                    sigCellData.sigCellObj = sigCell;
                    sigCfgCells.Add(sigCellData);

                    //创建一个报文数据列表
                    MsgCellData msgData = new MsgCellData();
                    msgData.msgId = item.Value.msgId;
                    msgData.msgValue.allData = 0;
                    msgData.msgType = 0;
                    msgData.msgCycle = item.Value.msgCycle;

                    //显示该报文包含的所有信号信息
                    foreach (var sig in item.Value.signals)
                    {
                        GameObject sigCell1;
                        sigCell1 = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(sigCellPath));

                        //显示信号名
                        UITool.GetOrAddComponentInChildByName<Text>(sigCell1, "Name").text = sig.sigName;
                        //设置通信配置按钮点击函数
                        Button tmpBtn = UITool.GetOrAddComponentInChildByName<Button>(sigCell1, "CodeCfgBtn");
                        //初始化信号代码结构体
                        SigCode tmpSigCode = new SigCode();
                        tmpSigCode.sigName = sig.sigName;
                        tmpSigCode.msgId = item.Value.msgId;
                        tmpSigCode.codeStr = "";
                        sigCodeList.Add(tmpSigCode);

                        tmpBtn.onClick.AddListener(() =>
                            {
                                tmpBtn.image.color = Color.green;
                                if(lastCfgBtn != null)
                                {
                                    lastCfgBtn.image.color = Color.white;
                                }
                                lastCfgBtn = tmpBtn;
                                //显示当前配置的信号
                                curCfgSigInfo.text = "当前配置信号:" + sig.sigName + "[精度:" + sig.sigFactor.ToString()+ "][偏移:" + sig.sigOffset.ToString() + "]";

                                //如果之前有配置过代码
                                if(lastSigCode != null)
                                {
                                    //保存代码编辑框到该配置里面
                                    lastSigCode.codeStr = codeEditFiled.text;
                                }

                                //设置当前配置的代码存储结构体
                                foreach (var item1 in sigCodeList)
                                {
                                    if (item1.msgId == item.Value.msgId && item1.sigName == sig.sigName)
                                    {
                                        lastSigCode = item1;
                                        //根据选中的信号设置当前代码编辑区域内容
                                        codeEditFiled.text = item1.codeStr;
                                        break;
                                    }
                                }

                            });

                        sigCell1.transform.SetParent(sigViewContent.transform);

                        //创建一个单元格数据
                        SigCellData sigCellData_2 = new SigCellData();
                        sigCellData_2.sigCellObj = sigCell1;
                        sigCellData_2.sig = sig;//报文信息
                        sigCellData_2.sigValue = 0;//初始化信号值
                        sigCfgCells.Add(sigCellData_2);
                        msgData.sigList.Add(sigCellData_2);
                    }

                    msgDataList.Add(msgData);

                }
            }
         }

    }

    //加载接口函数描述信息显示
    public void LoadInterfaceCfg()
    {
        //选择要加载的文件路径
        string loadPath = "./CodeInterfaceCfg.xlsx";
        DataSet excelData = ExcelReadAndWrite.ReadExcel(loadPath);

        //如果没有读到数据，则退出
        if (excelData == null)
        {
            return;
        }
        else
        {
            //清除之前的接口配置
            foreach (var item in interfaceInfoSet)
            {
                if (item != null)
                {
                    GameObject.Destroy(item.obj);
                }
            }

            interfaceInfoSet.Clear();
        }
        // 获取表格有多少列
        int columns = excelData.Tables[0].Columns.Count;
        // 获取表格有多少行 
        int rows = excelData.Tables[0].Rows.Count;

        //第一行为表头，不读取。没有表头从0开始(获取数据)
        for (int i = 1; i < rows; i++)
        {
            InterfaceInfo interfaceInfo = new InterfaceInfo();
            for (int j = 0; j < columns; j++)
            {
                // 获取表格中指定行指定列的数据 
                string value = excelData.Tables[0].Rows[i][j].ToString();

                switch ((InterfaceColumn)j)
                {
                    case InterfaceColumn.InterfaceName:
                        interfaceInfo.interfaceName = value;
                        break;
                    case InterfaceColumn.InterfaceDesc:
                        interfaceInfo.interfaceDesc = value;
                        break;
                    case InterfaceColumn.HeadFileName:
                        interfaceInfo.headFileName = value;
                        break;
                    case InterfaceColumn.Factor:
                        interfaceInfo.factor = value;
                        break;
                    case InterfaceColumn.Offset:
                        interfaceInfo.offset = value;
                        break;
                    default:
                        break;
                }
            }
            GameObject tmpObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(interfaceInfoCellPath));

            UITool.GetOrAddComponentInChildByName<Text>(tmpObj, "Name").text = interfaceInfo.interfaceName;
            UITool.GetOrAddComponentInChildByName<Text>(tmpObj, "Desc").text = interfaceInfo.interfaceDesc;
            UITool.GetOrAddComponentInChildByName<Text>(tmpObj, "Factor").text = interfaceInfo.factor;
            UITool.GetOrAddComponentInChildByName<Text>(tmpObj, "Offset").text = interfaceInfo.offset;
            tmpObj.transform.SetParent(interfaceViewContent.transform);
            interfaceInfo.obj = tmpObj;


            //添加接口信息到接口集合
            interfaceInfoSet.Add(interfaceInfo);

        }

        //
    }

    /// <summary>
    /// 重置该窗口
    /// </summary>
    public void ClearWin()
    {
        //isInit = false;

        //if (cfgCells.Count > 0)
        //{
        //    foreach (var item in cfgCells)
        //    {
        //        GameObject.Destroy(item.gameObject);
        //    }
        //}

        //cfgCells.Clear();
    }
}

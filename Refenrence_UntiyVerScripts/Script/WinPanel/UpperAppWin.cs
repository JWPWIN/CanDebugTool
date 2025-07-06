using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


//每个信号的需要记录的数据
public class SigCellData
{
    public GameObject sigCellObj; //信号单元实体对象
    public CanSignal sig;  //信号信息
    public float sigValue;//信号值
}

//报文信号列表
public class MsgCellData
{
    public uint msgId; //报文ID
    public uint msgCycle;//报文发送周期
    public List<SigCellData> sigList = new List<SigCellData>();//信号列表
    public Can_Uint64_Data msgValue = new Can_Uint64_Data(); //标准CAN数据：8 byte = 64 bit
    public int msgType;//0:接收报文；1：发送报文
    public Int64 msgSendTimer = 0; //报文发送定时器，只对发送类型报文有效
}

//简易模式控制窗口单元格信息
public class SimpleModeCtrlCell
{
    public GameObject obj; //单元格实例对象
    public bool isSigCell;//单元格是否为信号单元格:true：控制信号单元格；false： 模式单元格
    public CanSignal canSignal;//适用于信号单元格；存储信号信息
    public GameObject modeObj; //该单元格归属的模式单元格，如果该单元格为模式单元格，则该值为它本身
}

/// <summary>
/// CAN帧信号排列格式
/// </summary>
public enum CanSigFormat
{
    INTEL_STANDARD,
    INTEL_SEQUENTIAL,
    MOTOROLA_LSB,
    MOTOROLA_MSB,
    MOTOROLA_SEQUENTIAL,
    MOTOROLA_BACKWARD,
}

[StructLayout(LayoutKind.Explicit)]
public struct Can_Uint64_Data
{
    [FieldOffset(0)] public ulong allData;
    [FieldOffset(0)] public byte BYTE0;
    [FieldOffset(1)] public byte BYTE1;
    [FieldOffset(2)] public byte BYTE2;
    [FieldOffset(3)] public byte BYTE3;
    [FieldOffset(4)] public byte BYTE4;
    [FieldOffset(5)] public byte BYTE5;
    [FieldOffset(6)] public byte BYTE6;
    [FieldOffset(7)] public byte BYTE7;
}

public class UpperAppWin
{
    //是否初始化
    bool isInit = false;

    //上位机面板预设体
    static readonly string panelPath = "Prefabs/UpperAppWin";

    //上位机面板对象
    GameObject panelObj;

    //信号接收单元格预设物路径
    static readonly string recvCellPath = "Prefabs/SigCellRecvMsg";

    //信号发送单元格预设物路径
    static readonly string sendCellPath = "Prefabs/SigCellSendMsg";

    //报文信息显示单元格预设物路径
    static readonly string msgInfoCellPath = "Prefabs/SigCellMsgInfo";

    //信号接收显示列表对象
    GameObject recvViewContent;

    //信号发送显示列表对象
    GameObject sendViewContent;

    //信号接收显示单元格列表
    List<SigCellData> sigRecvCells = new List<SigCellData>();

    //信号发送显示单元格列表
    List<SigCellData> sigSendCells = new List<SigCellData>();

    //报文数据列表，存贮报文信号值，用于发送和接收
    List<MsgCellData> msgDataList = new List<MsgCellData>();



    //通信上位机是否处于简易模式下
    bool isSimpleMode = false;

    //简易模式上位机面板预设体
    static readonly string simplePanelPath = "Prefabs/UpperAppWin_Simple";

    //上位机面板对象
    GameObject simplePanelObj;

    //模式控制信号单元按钮预设物路径
    static readonly string ctrlSigCellPath = "Prefabs/CtrlSigCell";

    //模式控制单元格预设物路径
    static readonly string ctrlWorkModePath = "Prefabs/CtrlWorkModeCell";

    //简易模式下信号接收显示列表对象
    GameObject recvViewContentInSimple;

    //简易模式下信号发送显示列表对象
    GameObject sendViewContentInSimple;
    
    //简易模式控制区域单元格列表
    List<SimpleModeCtrlCell> simpleModeCtrlCells = new List<SimpleModeCtrlCell>();

    //简易模式信号监控区域单元格列表
    List<SigCellData> monitorSigCells = new List<SigCellData>();

         

    //选择信号界面对象
    GameObject selectSigPanelObj;
    //选择信号界面显示
    GameObject selectSigViewContent;
    //选择信号单元格列表
    List<SigCellData> sigSelectCellList = new List<SigCellData>();
    //选择信号界面预设物路径
    static readonly string selectSigPanelPath = "Prefabs/SelectSigWin";
    //选择信号单元格预设体路径
    static readonly string selectSigCellPath = "Prefabs/SelectSigCell";
    //选择b报文信息单元格预设体路径
    static readonly string selectMsgInfoCellPath = "Prefabs/SelectMsgInfoCell";


    // Start is called before the first frame update
    public UpperAppWin()
    {
        panelObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(panelPath));
        //设置窗口显示位置
        panelObj.transform.SetParent(UITool.GetChildObjectByName(UITool.FindCanvas(), "DataDisplayWin").transform, false);
        //获取信号接收内容显示对象
        GameObject obj = UITool.GetChildObjectByName(panelObj, "MsgRecvView");
        obj = UITool.GetChildObjectByName(obj, "Viewport");
        recvViewContent = UITool.GetChildObjectByName(obj, "Content");
        //获取信号发送内容显示对象
        GameObject obj_1 = UITool.GetChildObjectByName(panelObj, "MsgSendView");
        obj_1 = UITool.GetChildObjectByName(obj_1, "Viewport");
        sendViewContent = UITool.GetChildObjectByName(obj_1, "Content");

        //设置打开简易窗口模式的按钮
        UITool.GetOrAddComponentInChildByName<Button>(panelObj, "OpenSimpleModeBtn").onClick.AddListener(() =>
        {
            isSimpleMode = true;//进入简易模式
            SetPanelActive(true);//设置上位机模式

            //如果未创建简易模式界面，则创建
            if (simplePanelObj == null)
            {
                simplePanelObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(simplePanelPath));
                //设置窗口显示位置
                simplePanelObj.transform.SetParent(UITool.GetChildObjectByName(UITool.FindCanvas(), "DataDisplayWin").transform, false);

                //简易模式下信号发送显示列表对象
                sendViewContentInSimple = UITool.GetChildObjectByName(UITool.GetChildObjectByName(simplePanelObj, "ControlWin"), "Content");
                //简易模式下信号接收显示列表对象
                recvViewContentInSimple = UITool.GetChildObjectByName(UITool.GetChildObjectByName(simplePanelObj, "MonitorWin"), "Content");

                //设置退出建简易模式按钮回调
                UITool.GetOrAddComponentInChildByName<Button>(simplePanelObj, "ExitSimpleBtn").onClick.AddListener(() =>
                {
                    isSimpleMode = false;
                    SetPanelActive(true);//设置上位机模式
                });

                //设置添加控制信号单元格按钮回调
                UITool.GetOrAddComponentInChildByName<Button>(UITool.GetChildObjectByName(simplePanelObj, "ControlWin"), "AddSigBtn").onClick.AddListener(() =>
                {
                    if (CanDbcDataManager.GetInstance().isLoadCfg == false)
                    {
                        LogMng.GetInstance().DisplayLog("未加载DBC文件！");
                        return;
                    }

                    //检查添加该信号之前是否有模式单元格
                    SimpleModeCtrlCell tmpModeCell = new SimpleModeCtrlCell(); //添加单元格，该信号将会添加在该模式下
                    foreach (var item in simpleModeCtrlCells)
                    {
                        if (item.isSigCell == false)
                        {
                            tmpModeCell = item;
                        } 
                    }
                    //未添加工作模式单元格，则直接退出
                    if (tmpModeCell.obj == null)
                    {
                        LogMng.GetInstance().DisplayLog("未添加模式！");
                        return;
                    }

                    if (selectSigPanelObj == null)
                    {
                        selectSigPanelObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(selectSigPanelPath));
                        //设置窗口显示位置
                        selectSigPanelObj.transform.SetParent(UITool.GetChildObjectByName(UITool.FindCanvas(), "DataDisplayWin").transform, false);

                        selectSigViewContent = UITool.GetChildObjectByName(selectSigPanelObj, "Content");

                        //按照通信协议设置选择信号单元格
                        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
                        {
                            //显示报文信息
                            GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(selectMsgInfoCellPath));
                            //设置单元格位置
                            obj.transform.SetParent(selectSigViewContent.transform);

                            SigCellData tmpData = new SigCellData();
                            tmpData.sigCellObj = obj;
                            tmpData.sig = null;
                            tmpData.sigValue = 0;

                            sigSelectCellList.Add(tmpData);

                            foreach (var item1 in item.signals)
                            {
                                //显示信号信息
                                GameObject obj1 = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(selectSigCellPath));
                                //设置单元格位置
                                obj1.transform.SetParent(selectSigViewContent.transform);

                                //显示信号名称及描述
                                UITool.GetOrAddComponentInChildByName<Text>(obj1, "Name").text = item1.sigName;
                                UITool.GetOrAddComponentInChildByName<Text>(obj1, "Desc").text = item1.sigDesc;

                                SigCellData tmpSig = new SigCellData();
                                tmpSig.sigCellObj = obj1;
                                tmpSig.sig = item1;
                                tmpSig.sigValue = 0;
                                sigSelectCellList.Add(tmpSig);

                                //确认按钮回调函数
                                UITool.GetOrAddComponentInChildByName<Button>(obj1, "ConfirmBtn").onClick.AddListener(() =>
                                {
                                    //在简易模式控制窗口添加控制信号单元格
                                    GameObject obj_1 = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(ctrlSigCellPath));
                                    //设置单元格位置
                                    obj_1.transform.SetParent(sendViewContentInSimple.transform);

                                    if (item1.sigDesc == null || item1.sigDesc == "")
                                    {
                                        UITool.GetOrAddComponentInChildByName<Text>(obj_1, "SigDesc").text = item1.sigName;
                                    }
                                    else
                                    {
                                        UITool.GetOrAddComponentInChildByName<Text>(obj_1, "SigDesc").text = item1.sigDesc;
                                    }

                                    SimpleModeCtrlCell cell = new SimpleModeCtrlCell();
                                    cell.obj = obj_1;
                                    cell.isSigCell = true;
                                    cell.canSignal = item1;
                                    cell.modeObj = tmpModeCell.obj;
                                    simpleModeCtrlCells.Add(cell);

                                    //点击确认按钮就关闭选择窗口
                                    ClearSelectSigWin();

                                });

                            }

                        }


                        //添加取消信号选择窗口按钮
                        UITool.GetOrAddComponentInChildByName<Button>(selectSigPanelObj, "CancleSelectWin").onClick.AddListener(() =>
                        {
                            //点击取消按钮就关闭选择窗口
                            ClearSelectSigWin();

                        });

                    }

                });

                //设置添加简易工作模式单元格按钮回调
                UITool.GetOrAddComponentInChildByName<Button>(UITool.GetChildObjectByName(simplePanelObj, "ControlWin"), "AddModeBtn").onClick.AddListener(() =>
                {
                    GameObject tmpObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(ctrlWorkModePath));
                    //设置单元格位置
                    tmpObj.transform.SetParent(sendViewContentInSimple.transform);

                    SimpleModeCtrlCell cell = new SimpleModeCtrlCell();
                    cell.obj = tmpObj;
                    cell.isSigCell = false;
                    cell.canSignal = null;
                    cell.modeObj = tmpObj;
                    simpleModeCtrlCells.Add(cell);
                });

                //设置添加监控信号的按钮
                UITool.GetOrAddComponentInChildByName<Button>(UITool.GetChildObjectByName(simplePanelObj, "MonitorWin"), "AddMonitorSigBtn").onClick.AddListener(() =>
                {
                    if (CanDbcDataManager.GetInstance().isLoadCfg == false)
                    {
                        LogMng.GetInstance().DisplayLog("未加载DBC文件！");
                        return;
                    }

                    if (selectSigPanelObj == null)
                    {
                        selectSigPanelObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(selectSigPanelPath));
                        //设置窗口显示位置
                        selectSigPanelObj.transform.SetParent(UITool.GetChildObjectByName(UITool.FindCanvas(), "DataDisplayWin").transform, false);

                        selectSigViewContent = UITool.GetChildObjectByName(selectSigPanelObj, "Content");

                        //按照通信协议设置选择信号单元格
                        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
                        {
                            //显示报文信息
                            GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(selectMsgInfoCellPath));
                            //设置单元格位置
                            obj.transform.SetParent(selectSigViewContent.transform);

                            SigCellData tmpData = new SigCellData();
                            tmpData.sigCellObj = obj;
                            tmpData.sig = null;
                            tmpData.sigValue = 0;

                            sigSelectCellList.Add(tmpData);

                            foreach (var item1 in item.signals)
                            {
                                //显示信号信息
                                GameObject obj1 = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(selectSigCellPath));
                                //设置单元格位置
                                obj1.transform.SetParent(selectSigViewContent.transform);

                                //显示信号名称及描述
                                UITool.GetOrAddComponentInChildByName<Text>(obj1, "Name").text = item1.sigName;
                                UITool.GetOrAddComponentInChildByName<Text>(obj1, "Desc").text = item1.sigDesc;

                                SigCellData tmpSig = new SigCellData();
                                tmpSig.sigCellObj = obj1;
                                tmpSig.sig = item1;
                                tmpSig.sigValue = 0;
                                sigSelectCellList.Add(tmpSig);

                                //确认按钮回调函数
                                UITool.GetOrAddComponentInChildByName<Button>(obj1, "ConfirmBtn").onClick.AddListener(() =>
                                {
                                    //在简易模式控制窗口添加控制信号单元格
                                    GameObject obj_1 = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(recvCellPath));
                                    //设置单元格位置
                                    obj_1.transform.SetParent(recvViewContentInSimple.transform);

                                    if (item1.sigDesc == null || item1.sigDesc == "")
                                    {
                                        UITool.GetOrAddComponentInChildByName<Text>(obj_1, "Name").text = item1.sigName;
                                    }
                                    else
                                    {
                                        UITool.GetOrAddComponentInChildByName<Text>(obj_1, "Name").text = item1.sigDesc;
                                    }

                                    SigCellData cell = new SigCellData();
                                    cell.sigCellObj = obj_1;
                                    cell.sig = item1;
                                    cell.sigValue = 0;
                                    monitorSigCells.Add(cell);

                                    //点击确认按钮就关闭选择窗口
                                    ClearSelectSigWin();

                                });

                            }

                        }


                        //添加取消信号选择窗口按钮
                        UITool.GetOrAddComponentInChildByName<Button>(selectSigPanelObj, "CancleSelectWin").onClick.AddListener(() =>
                        {
                            //点击取消按钮就关闭选择窗口
                            ClearSelectSigWin();

                        });

                    }
                });

                //设置保存简易模式配置的按钮
                UITool.GetOrAddComponentInChildByName<Button>(simplePanelObj, "SaveModeCfgBtn").onClick.AddListener(() =>
                {
                    //以excel文件保存DBC配置文件
                    string savePath = FileLoadAndSave.GetFolderPath();
                    //获取信号数量
                    int sigNum = monitorSigCells.Count + simpleModeCtrlCells.Count;

                    if (sigNum != 0)
                    {
                        //按通信协议设置excel格式的协议
                        string[,] data = new string[sigNum + 1, 5];
                        data[0, 0] = "信号类型\r\n(0：监控信号；\r\n1：控制信号)";
                        data[0, 1] = "报文ID";
                        data[0, 2] = "信号名";
                        data[0, 3] = "控制信号模式名（控制信号才有，监控信号无该属性）";
                        data[0, 4] = "信号默认值";

                        uint m = 1;
                        //存储监控信号配置信息
                        foreach (var item in monitorSigCells)
                        {
                            data[m, 0] = "0";
                            data[m, 1] = item.sig.msgId.ToString("x3").ToUpper();
                            data[m, 2] = item.sig.sigName;
                            data[m, 3] = "";
                            data[m, 4] = "";

                            m++;
                        }

                        //存储控制信号配置信息
                        foreach (var item in simpleModeCtrlCells)
                        {
                            if(item.isSigCell == true)
                            {
                                data[m, 0] = "1";
                                data[m, 1] = item.canSignal.msgId.ToString("x3").ToUpper();
                                data[m, 2] = item.canSignal.sigName;
                                data[m, 3] = UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(item.modeObj, "WorkModeName"), "Text").text;
                                data[m, 4] = UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(item.obj, "SigInputValue"), "Text").text;;

                                m++;

                            }

                        }

                        ExcelReadAndWrite.CreatExcel(savePath, data, m, 5);
                    }

                });


            }


        });

    }

    //清除选择信号窗口
    public void ClearSelectSigWin()
    {
        sigSelectCellList.Clear();
        GameObject.Destroy(selectSigPanelObj);
        GameObject.Destroy(selectSigViewContent);
    }

    /// <summary>
    /// 重置该窗口配置
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
        if (active == true)
        {
            if (isSimpleMode == true)
            {
                panelObj.SetActive(false);
                simplePanelObj?.SetActive(true);
            }
            else
            {
                panelObj.SetActive(true);
                simplePanelObj?.SetActive(false);

            }
        }
        else
        {
            panelObj.SetActive(false);
            simplePanelObj?.SetActive(false);
        }
    }

    // Update is called once per frame
    public void UpdateCfg()
    {
        if ((true == CanDbcDataManager.GetInstance().isLoadCfg) && (false == isInit))
        {
            //重置面板
            ClearWin();
            isInit = true;

            //更新报文数据显示
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

                    sigCell.transform.SetParent(recvViewContent.transform);

                    //创建一个单元格数据
                    SigCellData sigCellData_1 = new SigCellData();
                    sigCellData_1.sigCellObj = sigCell;
                    sigCellData_1.sig = null;//报文不代表具体信号
                    sigCellData_1.sigValue = 0;//报文没有具体的信号值
                    sigRecvCells.Add(sigCellData_1);

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
                        sigCell1 = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(recvCellPath));

                        //显示信号名
                        UITool.GetOrAddComponentInChildByName<Text>(sigCell1, "Name").text = sig.sigName;

                        //显示信号值:TODO：当增加报文接收功能后，该值为实际接收值
                        UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(sigCell1, "Value"), "Text").text = "0";

                        sigCell1.transform.SetParent(recvViewContent.transform);

                        //创建一个单元格数据
                        SigCellData sigCellData_2 = new SigCellData();
                        sigCellData_2.sigCellObj = sigCell1;
                        sigCellData_2.sig = sig;//报文信息
                        sigCellData_2.sigValue = 0;//初始化信号值
                        sigRecvCells.Add(sigCellData_2);
                        msgData.sigList.Add(sigCellData_2);
                    }

                    msgDataList.Add(msgData);

                }
                else    //如果报文不是OBC或者DCDC发的，则对于上位机是发送报文
                {
                    //首先显示报文信息
                    GameObject sigCell;
                    sigCell = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(msgInfoCellPath));
                    //显示报文名
                    UITool.GetOrAddComponentInChildByName<Text>(sigCell, "Name").text = item.Value.msgName + "/0x" + item.Value.msgId.ToString("x3").ToUpper();

                    sigCell.transform.SetParent(sendViewContent.transform);

                    //创建一个单元格数据
                    SigCellData sigCellData_1 = new SigCellData();
                    sigCellData_1.sigCellObj = sigCell;
                    sigCellData_1.sig = null;//报文不代表具体信号
                    sigCellData_1.sigValue = 0;//报文没有具体的信号值
                    sigRecvCells.Add(sigCellData_1);

                    //创建一个报文数据列表
                    MsgCellData msgData = new MsgCellData();
                    msgData.msgId = item.Value.msgId;
                    msgData.msgValue.allData = 0;
                    msgData.msgType = 1;
                    msgData.msgCycle = item.Value.msgCycle;

                    //显示该报文包含的所有信号信息
                    foreach (var sig in item.Value.signals)
                    {
                        GameObject sigCell1;
                        sigCell1 = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(sendCellPath));

                        //显示信号名
                        UITool.GetOrAddComponentInChildByName<Text>(sigCell1, "Name").text = sig.sigName;

                        sigCell1.transform.SetParent(sendViewContent.transform);

                        //创建一个单元格数据
                        SigCellData sigCellData_2 = new SigCellData();
                        sigCellData_2.sigCellObj = sigCell1;
                        sigCellData_2.sig = sig;//报文信息
                        sigCellData_2.sigValue = 0;//初始化信号值
                        sigRecvCells.Add(sigCellData_2);
                        msgData.sigList.Add(sigCellData_2);

                    }

                    msgDataList.Add(msgData);

                }
            }

        }
    }

    /// <summary>
    /// 更新上位机接收到的信号值
    /// </summary>
    public void CanMsgReceive()
    {
        //更新信号接收值
        uint msgId = 0;
        Can_Uint64_Data msgData = new Can_Uint64_Data();
        if (true == CanDeviceMng.GetInstance().Receive_CanFrame(ref msgId, ref msgData))
        {
            foreach (var item in msgDataList)
            {
                if (item.msgId == msgId && item.msgType == 0)
                {
                    LogMng.GetInstance().DisplayLog("接收到报文" + item.msgId.ToString("x3").ToUpper()+ "===" +
                        msgData.BYTE0 + " " +  msgData.BYTE1+ " " + msgData.BYTE2 + " " + msgData.BYTE3+ " " + msgData.BYTE4+ " " + msgData.BYTE5 + " " + msgData.BYTE6 + " " + msgData.BYTE7);

                    item.msgValue.allData = msgData.allData;

                    foreach (var item1 in item.sigList)
                    {
                        byte[] tmp = { msgData.BYTE0, msgData.BYTE1, msgData.BYTE2, msgData.BYTE3, msgData.BYTE4, msgData.BYTE5, msgData.BYTE6, msgData.BYTE7 };

                        uint recvData = Get_Frame_Data(tmp, CanSigFormat.MOTOROLA_MSB, (byte)item1.sig.sigStartBit, (ushort)item1.sig.sigLen);

                        //根据总线上传输的数据值 计算实际值
                        double tmpData = (double)recvData * item1.sig.sigFactor + item1.sig.sigOffset;

                        UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(item1.sigCellObj, "Value"), "Text").text 
                            = tmpData.ToString();

                    }
                }
            }

        }


    }

    /// <summary>
    /// 上位机发送信号
    /// </summary>
    public void CanMsgSend()
    {
        //获取系统时间戳（时间戳就是从1970年1月1日0时0分0秒起到现在的总毫秒数，为什么时1970/1/1/00:00:00，因为第一台计算机发明时间是这个时间，所以时间戳诞生了）
        TimeSpan st1 = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        Int64 curSystemTime = Convert.ToInt64(st1.TotalMilliseconds);

        //更新报文发送定时器
        foreach (var item in msgDataList)
        {
            //如果该报文是接收类型报文，跳到下一个报文ID
            if (item.msgType == 0)
            {
                break;
            }
            //如果当前系统时间 - 上次发送时间 >= 报文发送周期，这发送报文
            if (curSystemTime - item.msgSendTimer >= item.msgCycle)
            {
                item.msgSendTimer = curSystemTime;//刷新报文发送定时器

                //发送该报文
                //设置信号输入框里面的内容到该报文数据里面
                byte[] tmpCanData = { 0, 0, 0, 0, 0, 0, 0, 0 };
                foreach (var item1 in item.sigList)
                {
                    //如果有信号输入框，就是信号设置行，信号设置行需要获取输入框的数据
                    if (UITool.GetChildObjectByName(item1.sigCellObj, "InputField") != null)
                    {
                        string inputStr = UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(item1.sigCellObj, "InputField"), "Value").text;
                        double sigSetValue = 0;
                        if (inputStr != "")
                        {
                            sigSetValue = double.Parse(inputStr);
                        }
                        //根据设置的实际值计算总线上传输的数据值
                        sigSetValue = (sigSetValue - item1.sig.sigOffset) / item1.sig.sigFactor;

                        Set_Frame_Data(tmpCanData, CanSigFormat.MOTOROLA_LSB, (byte)item1.sig.sigStartBit, (ushort)item1.sig.sigLen, (uint)sigSetValue);
                    }
                }
                //调用CAN驱动接口发送报文
                CanDeviceMng.GetInstance().Transmit_CanFrame(item.msgId, tmpCanData);
            }
        }

    }

    //根据信号值设置报文数据
    public void Set_Frame_Data(byte[] frame_data , CanSigFormat format, byte bit_start, ushort bit_len, uint value)
    {
        byte index;
        uint mask;
        Can_Uint64_Data long_data = new Can_Uint64_Data();
        long_data.allData = 0;

        if (format == CanSigFormat.INTEL_STANDARD)
        {
            index = bit_start;
            mask = (uint)((0x1UL << bit_len)-1);

            long_data.allData = value & mask;
            long_data.allData <<= index;

            frame_data[0] |= long_data.BYTE0;
            frame_data[1] |= long_data.BYTE1;
            frame_data[2] |= long_data.BYTE2;
            frame_data[3] |= long_data.BYTE3;
            frame_data[4] |= long_data.BYTE4;
            frame_data[5] |= long_data.BYTE5;
            frame_data[6] |= long_data.BYTE6;
            frame_data[7] |= long_data.BYTE7;
        }
        else if (format == CanSigFormat.INTEL_SEQUENTIAL)
        {
            index = (byte)(bit_start ^ 0x7);
            mask = (uint)((0x1UL << bit_len)-1);
            long_data.allData = value & mask;
            long_data.allData <<= index;

            frame_data[0] |= long_data.BYTE0;
            frame_data[1] |= long_data.BYTE1;
            frame_data[2] |= long_data.BYTE2;
            frame_data[3] |= long_data.BYTE3;
            frame_data[4] |= long_data.BYTE4;
            frame_data[5] |= long_data.BYTE5;
            frame_data[6] |= long_data.BYTE6;
            frame_data[7] |= long_data.BYTE7;
        }
        else if (format == CanSigFormat.MOTOROLA_LSB)
        {
            index = (byte)(bit_start ^ 0x38);
            mask = (uint)((0x1UL << bit_len)-1);
            long_data.allData = value & mask;
            long_data.allData <<= index;

            frame_data[7] |= long_data.BYTE0;
            frame_data[6] |= long_data.BYTE1;
            frame_data[5] |= long_data.BYTE2;
            frame_data[4] |= long_data.BYTE3;
            frame_data[3] |= long_data.BYTE4;
            frame_data[2] |= long_data.BYTE5;
            frame_data[1] |= long_data.BYTE6;
            frame_data[0] |= long_data.BYTE7;
        }
        else if (format == CanSigFormat.MOTOROLA_MSB)
        {
            index = (byte)(((bit_start ^ 0x7) + bit_len - 1) ^ 0x3F);
            mask = (uint)((0x1UL << bit_len)-1);
            long_data.allData = value & mask;
            long_data.allData <<= index;

            frame_data[7] |= long_data.BYTE0;
            frame_data[6] |= long_data.BYTE1;
            frame_data[5] |= long_data.BYTE2;
            frame_data[4] |= long_data.BYTE3;
            frame_data[3] |= long_data.BYTE4;
            frame_data[2] |= long_data.BYTE5;
            frame_data[1] |= long_data.BYTE6;
            frame_data[0] |= long_data.BYTE7;
        }
        else if (format == CanSigFormat.MOTOROLA_SEQUENTIAL)
        {
            index = (byte)(64 - bit_len - bit_start);
            mask = (uint)((0x1UL << bit_len)-1);
            long_data.allData = value & mask;
            long_data.allData <<= index;

            frame_data[7] |= long_data.BYTE0;
            frame_data[6] |= long_data.BYTE1;
            frame_data[5] |= long_data.BYTE2;
            frame_data[4] |= long_data.BYTE3;
            frame_data[3] |= long_data.BYTE4;
            frame_data[2] |= long_data.BYTE5;
            frame_data[1] |= long_data.BYTE6;
            frame_data[0] |= long_data.BYTE7;
        }
        else if (format == CanSigFormat.MOTOROLA_BACKWARD) //normally not used any more.
        {

        }
    }

    //根据通信协议获取报文中的信号值
    public uint Get_Frame_Data(byte[] frame_data, CanSigFormat format, byte bit_start, ushort bit_len)
    {
        uint value = 0;
        byte index;
        uint mask;
        Can_Uint64_Data long_data = new Can_Uint64_Data();

        if (format == CanSigFormat.INTEL_STANDARD)
        {
            long_data.BYTE0 = frame_data[0];
            long_data.BYTE1 = frame_data[1];
            long_data.BYTE2 = frame_data[2];
            long_data.BYTE3 = frame_data[3];
            long_data.BYTE4 = frame_data[4];
            long_data.BYTE5 = frame_data[5];
            long_data.BYTE6 = frame_data[6];
            long_data.BYTE7 = frame_data[7];

            index = bit_start;
            mask = (uint)((0x1UL << bit_len)-1);
            long_data.allData >>= index;
            value = (uint)(long_data.allData & mask);
        }
        if (format == CanSigFormat.INTEL_SEQUENTIAL)
        {
            long_data.BYTE0 = frame_data[0];
            long_data.BYTE1 = frame_data[1];
            long_data.BYTE2 = frame_data[2];
            long_data.BYTE3 = frame_data[3];
            long_data.BYTE4 = frame_data[4];
            long_data.BYTE5 = frame_data[5];
            long_data.BYTE6 = frame_data[6];
            long_data.BYTE7 = frame_data[7];

            index = (byte)(bit_start ^ 0x7);
            mask = (uint)((0x1UL << bit_len)-1);
            long_data.allData >>= index;
            value = (uint)(long_data.allData & mask);
        }
        else if (format == CanSigFormat.MOTOROLA_LSB)
        {
            long_data.BYTE0 = frame_data[7];
            long_data.BYTE1 = frame_data[6];
            long_data.BYTE2 = frame_data[5];
            long_data.BYTE3 = frame_data[4];
            long_data.BYTE4 = frame_data[3];
            long_data.BYTE5 = frame_data[2];
            long_data.BYTE6 = frame_data[1];
            long_data.BYTE7 = frame_data[0];

            index = (byte)(bit_start ^ 0x38);
            mask = (uint)((0x1UL << bit_len)-1);
            long_data.allData >>= index;
            value = (uint)(long_data.allData & mask);
        }
        else if (format == CanSigFormat.MOTOROLA_MSB)
        {
            long_data.BYTE0 = frame_data[7];
            long_data.BYTE1 = frame_data[6];
            long_data.BYTE2 = frame_data[5];
            long_data.BYTE3 = frame_data[4];
            long_data.BYTE4 = frame_data[3];
            long_data.BYTE5 = frame_data[2];
            long_data.BYTE6 = frame_data[1];
            long_data.BYTE7 = frame_data[0];

            //index = 57 + (bit_start%8) - (bit_start/8 * 8) - bit_len;
            index = (byte)(((bit_start ^ 0x7) + bit_len - 1) ^ 0x3F);
            mask = (uint)((0x1UL << bit_len)-1);
            long_data.allData >>= index;
            value = (uint)(long_data.allData & mask);
        }
        else if (format == CanSigFormat.MOTOROLA_SEQUENTIAL)
        {
            long_data.BYTE0 = frame_data[7];
            long_data.BYTE1 = frame_data[6];
            long_data.BYTE2 = frame_data[5];
            long_data.BYTE3 = frame_data[4];
            long_data.BYTE4 = frame_data[3];
            long_data.BYTE5 = frame_data[2];
            long_data.BYTE6 = frame_data[1];
            long_data.BYTE7 = frame_data[0];

            index = (byte)(64 - bit_len - bit_start);
            mask = (uint)((0x1UL << bit_len)-1);
            long_data.allData >>= index;
            value = (uint)(long_data.allData & mask);
        }

        return value;
    }

    /// <summary>
    /// 重置该窗口
    /// </summary>
    public void ClearWin()
    {
        isInit = false;

        if (sigSendCells.Count > 0)
        {
            foreach (var item in sigSendCells)
            {
                GameObject.Destroy(item.sigCellObj);
            }
        }

        if (sigRecvCells.Count > 0)
        {
            foreach (var item in sigRecvCells)
            {
                GameObject.Destroy(item.sigCellObj);
            }
        }

        sigRecvCells.Clear();
        sigSendCells.Clear();

    }
}

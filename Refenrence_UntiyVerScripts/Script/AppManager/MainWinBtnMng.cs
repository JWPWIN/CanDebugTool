using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainWinBtnMng
{
    static private MainWinBtnMng instance;

    //开启CAN通信标志
    public bool startCanCommunationFlag = false;

    static public MainWinBtnMng GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("MainWinBtnMng instance dont existed !");
            return null;
        }
        return instance;
    }

    public MainWinBtnMng()
    {
        if (instance == null)
        {
            instance = this;
        }

        GameObject canva = UITool.FindCanvas();

        UITool.GetOrAddComponentInChildByName<Button>(canva, "SaveCfgExcelBtn").onClick.AddListener(SaveCfgExcelClick);

        UITool.GetOrAddComponentInChildByName<Button>(canva, "SaveCfgDBCBtn").onClick.AddListener(SaveCfgDbcClick);

        UITool.GetOrAddComponentInChildByName<Button>(canva, "LoadCfgExcelBtn").onClick.AddListener(LoadCfgExcelClick);

        UITool.GetOrAddComponentInChildByName<Button>(canva, "LoadCfgDBCBtn").onClick.AddListener(LoadCfgDbcClick);

        UITool.GetOrAddComponentInChildByName<Button>(canva, "ExitAppBtn").onClick.AddListener(ExitAppClick);

        UITool.GetOrAddComponentInChildByName<Button>(canva, "StartCommunicateBtn").onClick.AddListener(StartComClick);

        //开启上位机界面
        UITool.GetOrAddComponentInChildByName<Button>(UITool.GetChildObjectByName(canva, "AppModePnl"), "UpperAppMode").onClick.AddListener(UpperAppModeClick);

        //查看配置
        UITool.GetOrAddComponentInChildByName<Button>(UITool.GetChildObjectByName(canva, "AppModePnl"), "LookCfgMode").onClick.AddListener(LookCfgClick);

        //代码生成
        UITool.GetOrAddComponentInChildByName<Button>(UITool.GetChildObjectByName(canva, "AppModePnl"), "CodeGenerateMode").onClick.AddListener(CodeGenerateClick);

    }

    private void StartComClick()
    {
        //标志位，每点击一次该按钮，就会切换开启/停止通信
        if (startCanCommunationFlag == true)
        {
            startCanCommunationFlag = false;
            //停止通信时按钮显示灰色
            GameObject canva = UITool.FindCanvas();
            UITool.GetOrAddComponentInChildByName<Image>(canva, "StartCommunicateBtn").color = Color.white;
            //断开CAN设备关闭通信
            CanDeviceMng.GetInstance().CloseDevice();

        }
        else
        {
            //如果已经加载DBC，则打开CAN设备
            if (CanDbcDataManager.GetInstance().isLoadCfg)
            {
                startCanCommunationFlag = true;
                //开启通信时按钮显示绿色
                GameObject canva = UITool.FindCanvas();
                UITool.GetOrAddComponentInChildByName<Image>(canva, "StartCommunicateBtn").color = Color.green;
                //连接CAN设备开启通信
                CanDeviceMng.GetInstance().OpenDevice();
            }
            else
            {
                LogMng.GetInstance().DisplayLog("未加载DBC，开启通信失败!");
            }
        }
    }

    private void SaveCfgExcelClick()
    {
        //以excel文件保存DBC配置文件
        string savePath = FileLoadAndSave.GetFolderPath();
        //获取信号数量
        int sigNum = 0;
        foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
        {
            sigNum += item.signals.Count;
        }

        if (sigNum != 0)
        {
            //按通信协议设置excel格式的协议
            string[,] data = new string[sigNum + 1, (int)CanDbcRows.MaxNum];
            data[0, 0] = "信号名";
            data[0, 1] = "报文名";
            data[0, 2] = "报文ID";
            data[0, 3] = "报文Size";
            data[0, 4] = "报文发送周期";
            data[0, 5] = "信号描述";
            data[0, 6] = "信号排列格式\r\n0：motorola\r\n1：intel";
            data[0, 7] = "信号起始位";
            data[0, 8] = "信号长度/bit";
            data[0, 9] = "精度";
            data[0, 10] = "偏移";
            data[0, 11] = "信号值";
            data[0, 12] = "值类型\r\n1：有符号\r\n0：无符号";
            data[0, 13] = "发送节点";
            data[0, 14] = "接收节点";

            uint m = 1;
            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
            {
                foreach (var item1 in item.signals)
                {
                    data[m, 0] = item1.sigName;
                    data[m, 1] = item.msgName;
                    data[m, 2] = "0x" + item.msgId.ToString("x3");
                    data[m, 3] = item.msgSize.ToString();
                    data[m, 4] = item.msgCycle.ToString();
                    data[m, 5] = item1.sigDesc;
                    data[m, 6] = item1.sigOrderType.ToString();
                    data[m, 7] = item1.sigStartBit.ToString();
                    data[m, 8] = item1.sigLen.ToString();
                    data[m, 9] = item1.sigFactor.ToString();
                    data[m, 10] = item1.sigOffset.ToString();
                    if (item1.sigValueTable != null)
                    {
                        string tmpStr = "";
                        foreach (var item2 in item1.sigValueTable)
                        {
                            tmpStr += item2.Key.ToString() + ":" + item2.Value + "\n";
                        }
                        data[m, 11] = tmpStr;
                    }
                    else
                    {
                        data[m, 11] = "";
                    }

                    data[m, 12] = item1.valueType.ToString();
                    data[m, 13] = item.transmitter.ToString();

                    if (item1.recvNode == null || item1.recvNode == "")
                    {
                        data[m, 14] = "TBD";
                    }
                    else
                    {
                        data[m, 14] = item1.recvNode;
                    }
                    m++;

                }

            }

            ExcelReadAndWrite.CreatExcel(savePath, data, m, (int)CanDbcRows.MaxNum);
        }



    }

    private void SaveCfgDbcClick()
    {
        //保存CAN通信矩阵为DBC文件格式
        string dbcContent = GenerateDBC.GenerateDbcForCanMatrix();
        if (dbcContent == null)
        {
            return;
        }
        string savePath = FileLoadAndSave.GetFolderPath();
        string fileName = "CanMatrixDBC";
        TextReadAndWrite.WriteData(savePath, fileName, FileType.Text, dbcContent);

    }

    private void LoadCfgExcelClick()
    {
        //加载excel格式的DBC配置文件
        CanDbcDataManager.GetInstance().LoadCanMatrixFromExcel();
        //重置窗口配置
        WinPanelMng.GetInstance().UpdataCanCfg();
    }

    private void LoadCfgDbcClick()
    {
        //加载DBC格式的DBC配置文件
        CanDbcDataManager.GetInstance().LoadCanMatrixFromDBC();
        //重置窗口配置
        WinPanelMng.GetInstance().UpdataCanCfg();
    }

    private void LookCfgClick()
    {
        //设置APP为查看配置模式
        WinPanelMng.GetInstance().SetWinMode(WinMode.LookCanCfg);
    }

    private void UpperAppModeClick()
    {
        //设置APP为上位机模式
        WinPanelMng.GetInstance().SetWinMode(WinMode.UpperApp);
    }

    private void CodeGenerateClick()
    {
        //设置APP为代码生成模式
        WinPanelMng.GetInstance().SetWinMode(WinMode.CodeGenerate);
    }

    private void ExitAppClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}

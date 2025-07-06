using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum WinMode
{ 
    LookCanCfg,     //查看CAN配置
    CodeGenerate,   //代码生成
    UpperApp,       //通信上位机
    Calibration     //校准
}

public class WinPanelMng
{
    static private WinPanelMng instance;

    //数据显示窗口模式
    public WinMode winMode;

    //查看配置窗口
    LookCfgWin lookCfgWin;

    //报文收发窗口
    UpperAppWin upperAppWin;

    //代码生成配置窗口
    CodeGenerateCfgWin codeGntCfgWin;


    static public WinPanelMng GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("WinPanelMng instance dont existed !");
            return null;
        }
        return instance;
    }

    public WinPanelMng()
    {
        if (instance == null)
        {
            instance = this;
        }

        //初始化CAN配置面板对象，并设置为不使能
        lookCfgWin = new LookCfgWin();
        lookCfgWin.SetPanelActive(false);

        //初始化报文收发窗口对象，并设置为不使能
        upperAppWin = new UpperAppWin();
        upperAppWin.SetPanelActive(false);

        //初始化报文收发窗口对象，并设置为不使能
        codeGntCfgWin = new CodeGenerateCfgWin();
        codeGntCfgWin.SetPanelActive(false);

        //默认显示通信上位机面板
        SetWinMode(WinMode.UpperApp);
    }

    public void SetWinMode(WinMode mode)
    {
        winMode = mode;

        //清除模式按钮颜色
        GameObject canva = UITool.FindCanvas();

        UITool.GetOrAddComponentInChildByName<Image>(UITool.GetChildObjectByName(canva, "AppModePnl"), "CodeGenerateMode").color = Color.white;
        UITool.GetOrAddComponentInChildByName<Image>(UITool.GetChildObjectByName(canva, "AppModePnl"), "UpperAppMode").color = Color.white;
        UITool.GetOrAddComponentInChildByName<Image>(UITool.GetChildObjectByName(canva, "AppModePnl"), "CalibrationMode").color = Color.white;
        UITool.GetOrAddComponentInChildByName<Image>(UITool.GetChildObjectByName(canva, "AppModePnl"), "LookCfgMode").color = Color.white;
        //设置目前模式按钮颜色为绿色
        switch (winMode)
        {
            case WinMode.CodeGenerate:
                UITool.GetOrAddComponentInChildByName<Image>(UITool.GetChildObjectByName(canva, "AppModePnl"), "CodeGenerateMode").color = Color.green;
                break;
            case WinMode.UpperApp:
                UITool.GetOrAddComponentInChildByName<Image>(UITool.GetChildObjectByName(canva, "AppModePnl"), "UpperAppMode").color = Color.green;
                break;
            case WinMode.Calibration:
                UITool.GetOrAddComponentInChildByName<Image>(UITool.GetChildObjectByName(canva, "AppModePnl"), "CalibrationMode").color = Color.green;
                break;
            case WinMode.LookCanCfg:
                UITool.GetOrAddComponentInChildByName<Image>(UITool.GetChildObjectByName(canva, "AppModePnl"), "LookCfgMode").color = Color.green;
                break;
            default:
                break;

        }
    }

    public void UpdateWinPanel()
    {
        SetWinActive();//设置各个窗口使能

        switch (winMode)
        {
            case WinMode.LookCanCfg:
                //更新CAN通信配置
                lookCfgWin.UpdateCfg();
                break;
            case WinMode.CodeGenerate:
                //更新代码生成信号配置
                codeGntCfgWin.UpdateCfg();
                break;
            case WinMode.UpperApp:
                //更新can矩阵配置
                upperAppWin.UpdateCfg();

                if (MainWinBtnMng.GetInstance().startCanCommunationFlag == true)
                {
                    //如果开启通信，则进行报文收发
                    //接收CAN报文
                    upperAppWin.CanMsgReceive();
                    //发送CAN报文
                    upperAppWin.CanMsgSend();
                }
                break;
            case WinMode.Calibration:
                break;
            default:
                break;
        }
    }

    private void SetWinActive()
    {
        lookCfgWin.SetPanelActive(winMode == WinMode.LookCanCfg);
        upperAppWin.SetPanelActive(winMode == WinMode.UpperApp);
        codeGntCfgWin.SetPanelActive(winMode == WinMode.CodeGenerate);
    }

    /// <summary>
    /// CAN配置更新，窗口配置同步更新
    /// </summary>
    public void UpdataCanCfg()
    {
        //重置上位机窗口初始化参数
        upperAppWin.ResetCfg();

        //重置can配置窗口初始化参数
        lookCfgWin.ResetCfg();

        //重置can配置窗口初始化参数
        codeGntCfgWin.ResetCfg();

    }
}

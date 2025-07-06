using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogMng
{
    static private LogMng instance;

    //日志显示对象
    public Text logText;

    //日志内容显示列表
    private List<string> logLines;

    //最多同时显示log数
    private int maxDispNum = 6;

    //最多纪录log数
    private int maxRecordNum = 20;

    //最多同时显示

    static public LogMng GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("LogMng instance dont existed !");
            return null;
        }
        return instance;
    }

    public LogMng()
    {
        if (instance == null)
        {
            instance = this;
        }

        logLines = new List<string>();

        GameObject canva = UITool.FindCanvas();
        logText = UITool.GetOrAddComponentInChildByName<Text>(canva, "LogText");
    }

    /// <summary>
    /// 显示日志
    /// </summary>
    /// <param name="log">显示内容</param>
    public void DisplayLog(string log)
    {
        //最新消息增加*NEW*标志
        logLines.Add("*NEW*" + log);

        //如果记录内容大于最大记录数，则清除所有内容
        if (logLines.Count > maxRecordNum)
        {
            logLines.Clear();
        }

        string logContent = "";
        //显示最近的maxDispNum条log
        if (logLines.Count <= maxDispNum)
        {
            foreach (var item in logLines)
            {
                logContent += item;
                logContent += "\r\n";//换行
            }

        }
        else
        {
            for (int i = logLines.Count - 1 - maxDispNum; i < logLines.Count; i++)
            {
                logContent += logLines[i];
                logContent += "\r\n";//换行
            }
        }

        logText.text = logContent;

        //新消息显示完成后去掉*NEW*
        logLines[logLines.Count-1] = log;
    }
}

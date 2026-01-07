using System.Collections.Generic;
using System.Text.RegularExpressions;

static public class AppLogMng
{
    //日志内容显示列表
    static private List<string> logLines = new List<string>();

    //日志当前显示字符串
    static private string curLogStr = string.Empty;

    //最多同时显示log数
    static private int maxDispNum = 6;

    //最多纪录log数
    static private int maxRecordNum = 20;

    /// <summary>
    /// 显示日志
    /// </summary>
    /// <param name="log">显示内容</param>
    static public void DisplayLog(string log)
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

        //显示Log信息
        curLogStr = logContent;

        //新消息显示完成后去掉*NEW*
        logLines[logLines.Count - 1] = log;
    }

    /// <summary>
    /// 获取当前需要显示的日志字符串
    /// </summary>
    /// <returns></returns>
    static public string GetGobalLogStr()
    { 
        return curLogStr;
    }
}

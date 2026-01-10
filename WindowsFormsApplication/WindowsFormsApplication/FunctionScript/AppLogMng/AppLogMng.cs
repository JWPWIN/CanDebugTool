using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

static public class AppLogMng
{
    //当前日志显示字符串
    static private string curLogStr = string.Empty;

    //当前日志代表的状态标志 true:成功状态-字符将显示绿色  false:失败状态-字符将显示红色
    static private bool isSuccessFlag = false;

    /// <summary>
    /// 显示日志接口
    /// </summary>
    /// <param name="log">要显示的日志字符串信息</param>
    /// <param name="successFlag">成功状态</param>
    static public void DisplayLog(string log, bool successFlag)
    {
        //获取Log信息字符串
        curLogStr = log;
        //获取该Log信息状态
        isSuccessFlag = successFlag;
    }

    /// <summary>
    /// 获取当前需要显示的日志字符串
    /// </summary>
    /// <returns></returns>
    static public string GetGobalLogStr()
    { 
        return curLogStr;
    }

    /// <summary>
    /// 获取当前需要显示的日志字符串的颜色
    /// </summary>
    static public Color GetGobalLogStrColor()
    {
        Color color;

        if (isSuccessFlag)
        {
            //成功信息显示暗绿色
            color = Color.DarkGreen;
        }
        else
        {
            //失败信息显示暗绿色
            color = Color.DarkRed;
        }

        return color;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public enum TimeUnit
{ 
    T_US = 1, //1us
    T_MS = 1000 * T_US, //1ms
    T_S = 1000 * T_MS,//1s
    T_MIN = 60 * T_S,//1min
}

public class TimerTool
{
    /// <summary>
    /// 获取当前系统时间戳us微妙
    /// </summary>
    /// <returns></returns>
    public static ulong GetSysTime()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        
        return Convert.ToUInt64(ts.TotalMicroseconds);
    }

    /// <summary>
    /// 重置计时器为当前时间戳us微妙
    /// </summary>
    /// <param name="timer">计时器</param>
    public static void ResetTimer(ref ulong timer)
    {
        timer = GetSysTime();
    }

    /// <summary>
    /// 检查超时
    /// </summary>
    /// <param name="timer">计时器</param>
    /// <param name="timeout">超时时间/us微妙</param>
    /// <returns>是否超时</returns>
    public static bool CheckTimeOut(ulong timer, ulong timeout)
    {
        ulong curTime = GetSysTime();
        bool bRet = false;

        if (curTime > timer)
        {
            if (curTime - timer >= timeout)
            {
                bRet = true;
            }
            else
            {
                bRet = false;
            }
        }
        else
        {
            bRet = true;
        }
    
        return bRet;
    }
}

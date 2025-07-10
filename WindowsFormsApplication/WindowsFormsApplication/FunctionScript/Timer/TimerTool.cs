using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public enum TimeUnit
{ 
    T_MS = 1, //1ms
    T_S = 1000,//1s
    T_MIN = 60000,//1min
}

public class TimerTool
{
    /// <summary>
    /// 获取当前系统时间戳ms
    /// </summary>
    /// <returns></returns>
    public static long GetSysTime()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

        return Convert.ToInt64(ts.TotalMilliseconds);
    }
    
    /// <summary>
    /// 重置计时器为当前时间戳ms
    /// </summary>
    /// <param name="timer">计时器</param>
    public static void ResetTimer(ref long timer)
    {
        timer = GetSysTime();
    }

    /// <summary>
    /// 检查超时
    /// </summary>
    /// <param name="timer">计时器</param>
    /// <param name="timeout">超时时间/s</param>
    /// <returns>是否超时</returns>
    public static bool CheckTimeOut(long timer, long timeout)
    {
        long curTime = GetSysTime();
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

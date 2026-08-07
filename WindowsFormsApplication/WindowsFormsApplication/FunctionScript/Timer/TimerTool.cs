using System.Diagnostics;

public enum TimeUnit
{
    T_US = 1, //1us
    T_MS = 1000 * T_US, //1ms
    T_S = 1000 * T_MS,//1s
    T_MIN = 60 * T_S,//1min
}

/// <summary>
/// ����ʱ�Ӽ�ʱ���ߣ����� Stopwatch������ϵͳУʱӰ�죩��
/// </summary>
public class TimerTool
{
    private static readonly double TimestampToMicroseconds = 1_000_000.0 / Stopwatch.Frequency;

    /// <summary>
    /// ��ȡ��ǰ����ʱ�����΢�룩
    /// </summary>
    public static ulong GetSysTime()
    {
        return (ulong)(Stopwatch.GetTimestamp() * TimestampToMicroseconds);
    }

    /// <summary>
    /// ���ü�ʱ��Ϊ��ǰʱ�����΢�룩
    /// </summary>
    public static void ResetTimer(ref ulong timer)
    {
        timer = GetSysTime();
    }

    /// <summary>
    /// ����Ƿ�ʱ��timer==0 ��Ϊδ��ʼ�������� true �Ա��״δ�����
    /// </summary>
    public static bool CheckTimeOut(ulong timer, ulong timeout)
    {
        if (timer == 0)
            return true;

        ulong curTime = GetSysTime();
        return curTime - timer >= timeout;
    }
}

using System;
using System.Threading;

/// <summary>
/// 通信会话后台调度：只做收发，不直接操作 UI。
/// 热路径说明见同目录 HOTPATH.md。
/// </summary>
public class LongRunningThreadService
{
    private CancellationTokenSource _stopCts;
    private readonly ManualResetEvent _pauseEvent = new(false);
    private Thread _workerThread;

    /// <summary>1ms 会话回调（可选扩展），在工作线程执行；勿在此读写 WinForms 控件。</summary>
    public Action OnSession1ms;

    ulong TaskTimer_10us;
    ulong TaskTimer_100us;
    ulong TaskTimer_1ms;
    ulong TaskTimer_10ms;
    ulong TaskTimer_100ms;
    ulong TaskTimer_1s;

    public void Start()
    {
        if (_workerThread is { IsAlive: true })
            return;

        _stopCts = new CancellationTokenSource();
        _pauseEvent.Set();
        _workerThread = new Thread(ThreadJob)
        {
            IsBackground = true,
            Name = "CanDebugMainLoop"
        };
        _workerThread.Start(_stopCts.Token);
    }

    public void Pause() => _pauseEvent.Reset();

    public void Resume() => _pauseEvent.Set();

    public void Stop()
    {
        var cts = _stopCts;
        if (cts is null)
            return;

        try { cts.Cancel(); }
        catch (ObjectDisposedException) { }

        _pauseEvent.Set();

        Thread thread = _workerThread;
        if (thread is not null && thread.IsAlive)
            thread.Join(2000);

        _workerThread = null;
        try { cts.Dispose(); }
        catch (ObjectDisposedException) { }
        _stopCts = null;
    }

    private void ThreadJob(object obj)
    {
        var token = (CancellationToken)obj;
        while (!token.IsCancellationRequested)
        {
            _pauseEvent.WaitOne();
            if (token.IsCancellationRequested)
                break;

            Process();

            bool deviceOpen = DeviceInterfaceMng.GetInstance()?.canDeviceOpenFlag == true;
            if (deviceOpen)
                Thread.Sleep(0);
            else
                Thread.Sleep(1);
        }
    }

    private void Process()
    {
        if (TimerTool.CheckTimeOut(TaskTimer_10us, 10 * (ulong)TimeUnit.T_US))
        {
            Process_10us();
            TimerTool.ResetTimer(ref TaskTimer_10us);
        }

        if (TimerTool.CheckTimeOut(TaskTimer_100us, 100 * (ulong)TimeUnit.T_US))
        {
            Process_100us();
            TimerTool.ResetTimer(ref TaskTimer_100us);
        }

        if (TimerTool.CheckTimeOut(TaskTimer_1ms, 1 * (ulong)TimeUnit.T_MS))
        {
            Process_1ms();
            TimerTool.ResetTimer(ref TaskTimer_1ms);
        }

        if (TimerTool.CheckTimeOut(TaskTimer_10ms, 10 * (ulong)TimeUnit.T_MS))
        {
            Process_10ms();
            TimerTool.ResetTimer(ref TaskTimer_10ms);
        }

        if (TimerTool.CheckTimeOut(TaskTimer_100ms, 100 * (ulong)TimeUnit.T_MS))
        {
            Process_100ms();
            TimerTool.ResetTimer(ref TaskTimer_100ms);
        }

        if (TimerTool.CheckTimeOut(TaskTimer_1s, 1 * (ulong)TimeUnit.T_S))
        {
            Process_1s();
            TimerTool.ResetTimer(ref TaskTimer_1s);
        }
    }

    private void Process_10us()
    {
    }

    private void Process_100us()
    {
        DeviceInterfaceMng.GetInstance()?.MainLoopThread_Task_ReceiveMessagesFromDevice();
        DeviceInterfaceMng.GetInstance()?.MainLoopThread_Task_SendMessagesToDevice();
    }

    private void Process_1ms()
    {
        // 设备缓冲 → 会话待处理快照区
        DeviceInterfaceMng.GetInstance()?.MainLoopThread_Task_GetRecvMsgFromDeviceBuf();
        // 可选会话逻辑；周期载荷填充已改到 UI 泵，避免跨线程读发送控件
        OnSession1ms?.Invoke();
    }

    private void Process_10ms()
    {
    }

    private void Process_100ms()
    {
    }

    private void Process_1s()
    {
    }
}

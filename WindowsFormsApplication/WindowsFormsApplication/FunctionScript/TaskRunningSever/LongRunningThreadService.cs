using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowsFormsApplication;

/// <summary>
/// 长时间工作线程服务
/// </summary>
public class LongRunningThreadService
{
    private CancellationTokenSource _stopCts;
    private readonly ManualResetEvent _pauseEvent = new(false);
    private Thread _workerThread;

    //主窗口对象 用于获取主窗口下的所有子对象
    public MainWin mainWin;

    //任务调度计时器(1ms/10ms/100ms/1s)
    ulong TaskTimer_1ms;
    ulong TaskTimer_10ms;
    ulong TaskTimer_100ms;
    ulong TaskTimer_1s;

    public LongRunningThreadService(MainWin win)
    {
        mainWin = win;
    }

    public void Start()
    {
        _stopCts = new();
        _pauseEvent.Set();
        var thread = _workerThread ??= new Thread(ThreadJob) { IsBackground = true };
        thread.Start(_stopCts.Token);
    }

    public void Pause()
    {
        MessageBox.Show("Pausing Service...");
        _pauseEvent.Reset();
    }

    public void Resume()
    {
        MessageBox.Show("Resuming Service...");
        _pauseEvent.Set();
    }
    public void Stop()
    {
        MessageBox.Show("Stopping Service...");
        _stopCts.Cancel();
        _stopCts.Dispose();
        _stopCts = null;
        _workerThread = null;
    }
    /// <summary>
    /// 线程委托：轮询工作
    /// </summary>
    /// <param name="obj"></param>
    private void ThreadJob(object obj)
    {
        var token = (CancellationToken)obj;
        while (!token.IsCancellationRequested)
        {
            _pauseEvent.WaitOne();
            Process();
            Thread.Sleep(1);
        }
        MessageBox.Show("Service Stopped");
    }

    /// <summary>
    /// 线程主任务函数
    /// </summary>
    private void Process()
    {
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

    /// <summary>
    /// 1ms调度函数
    /// </summary>
    private void Process_1ms()
    {

    }

    /// <summary>
    /// 10ms调度函数
    /// </summary>
    private void Process_10ms()
    {

    }

    /// <summary>
    /// 100ms调度函数
    /// </summary>
    private void Process_100ms()
    {
        //任务一：更新状态栏信息
        mainWin.MainLoopThread_Task_UpdateStatusStripInfo();
    }

    /// <summary>
    /// 1s调度函数
    /// </summary>
    private void Process_1s()
    {

    }
}


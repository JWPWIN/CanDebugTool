using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

public enum CanDeviceType
{
    ZCAN_USBCANFD_200U = 41,
    ZCAN_USBCANFD_100U = 42,
    ZCAN_USBCANFD_MINI = 43
};

public enum CanFrameType
{
    CANFD,
    CAN
};

public struct Canfd_Frame_Com
{
    public uint can_id; /* 32 bit CAN_ID + EFF/RTR/ERR flags */
    public byte len; /* frame payload length in byte */
    public byte is_canfd;/* wether canfd or not */
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] data/* __attribute__((aligned(8)))*/;
}

public struct CycleSend_Canfd_Frame
{
    /// <summary>报文发送周期（微秒，与 TimerTool 一致）。</summary>
    public ulong sendCycle;
    /// <summary>报文发送计时器（微秒时间戳）。</summary>
    public ulong sendTimer;
    public Canfd_Frame_Com msgData;//发送报文数据
}

public class DeviceInterfaceMng
{
    static private DeviceInterfaceMng instance;//单例对象 

    public CanDeviceType curCanDeviceType = 0;//当前设备类型

    public CanFrameType curCanFrameType = 0;//当前CAN帧类型

    public bool canDeviceOpenFlag = false;//是否有设备打开

    private ZlgDevice zlgDevice = null;//周立功设备实例

    /// <summary>当前等待处理的接收报文（同 ID 保留最新一帧）。</summary>
    private readonly Dictionary<uint, Canfd_Frame_Com> waitToHandle_RecvCanMsgById = new Dictionary<uint, Canfd_Frame_Com>();
    private readonly object _recvSync = new object();

    private readonly Queue<Canfd_Frame_Com> waitToHandle_SendCanMsgBuf = new Queue<Canfd_Frame_Com>();

    //周期发送报文列表 <报文ID,周期发送报文数据>
    private Dictionary<uint, CycleSend_Canfd_Frame> task_CycleMsgSendDict = new Dictionary<uint, CycleSend_Canfd_Frame>();

    public DeviceInterfaceMng()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    static public DeviceInterfaceMng GetInstance()
    {
        // 未初始化时静默返回 null，避免启动阶段弹框阻塞主界面
        return instance;
    }

    /// <summary>
    /// 打开CAN卡设备
    /// </summary>
    /// <param name="selectDeviceType">设备类型下拉索引</param>
    /// <param name="selectCanType">CAN 帧类型下拉索引（0=CANFD, 1=CAN）</param>
    /// <returns>是否打开成功</returns>
    public bool OpenCanDevice(int selectDeviceType,int selectCanType)
    {
        //未打开设备 直接返回
        if (canDeviceOpenFlag == true)
        {
            AppLogMng.DisplayLog("已打开过设备!",false);
            return false;
        }

        //Step1: 获取设备类型 { "ZCAN_USBCANFD_100U", "ZCAN_USBCANFD_200U", "ZCAN_USBCANFD_MINI" }
        switch (selectDeviceType)
        {
            case 0:
                curCanDeviceType = CanDeviceType.ZCAN_USBCANFD_100U;
                break;
            case 1:
                curCanDeviceType = CanDeviceType.ZCAN_USBCANFD_200U;
                break;
            case 2:
                curCanDeviceType = CanDeviceType.ZCAN_USBCANFD_MINI;
                break;
            default:
                curCanDeviceType = CanDeviceType.ZCAN_USBCANFD_100U;
                break;  
        }

        //Step2: 获取CAN帧类型{ "CANFD", "CAN"} —— 使用帧类型下拉索引，而非设备类型
        switch (selectCanType)
        {
            case 0:
                curCanFrameType = CanFrameType.CANFD;
                break;
            case 1:
                curCanFrameType = CanFrameType.CAN;
                break;
            default:
                curCanFrameType = CanFrameType.CANFD;
                break;
        }

        //Step3: 根据设备类型创建对应的设备对象，并尝试打开设备
        bool successOpenFlag = false;
        switch (curCanDeviceType)
        {
            case CanDeviceType.ZCAN_USBCANFD_100U:
            case CanDeviceType.ZCAN_USBCANFD_200U:
            case CanDeviceType.ZCAN_USBCANFD_MINI:
                if(zlgDevice is null) zlgDevice = new ZlgDevice();//创建zlg设备
                successOpenFlag = zlgDevice.OpenDevice(curCanDeviceType,curCanFrameType);
                break;
            default:
                break;
        }

        if (successOpenFlag == true)
        {
            canDeviceOpenFlag = true;
            ClearSessionRuntimeBuffers();
            AppLogMng.DisplayLog("打开设备成功!", true);
            return true;
        }

        canDeviceOpenFlag = false;
        AppLogMng.DisplayLog("打开设备失败!", false);
        return false;
    }

    /// <summary>
    /// 关闭CAN卡设备
    /// </summary>
    public void CloseCanDevice()
    {
        //未打开设备 直接返回
        if(canDeviceOpenFlag == false)
        {
            AppLogMng.DisplayLog("未打开过设备!", false);
            return;
        }

        //根据设备类型关闭对应设备
        bool successCloseFlag = false;
        switch (curCanDeviceType)
        {
            case CanDeviceType.ZCAN_USBCANFD_100U:
            case CanDeviceType.ZCAN_USBCANFD_200U:
            case CanDeviceType.ZCAN_USBCANFD_MINI:
                //关闭zlg设备
                if (zlgDevice is not null) successCloseFlag = zlgDevice.CloseDevice();
                break;
            default:
                break;
        }

        // 先停会话侧标志与缓冲，避免 UI 泵在半关闭状态继续消费
        canDeviceOpenFlag = false;
        ClearSessionRuntimeBuffers();

        if (successCloseFlag == true)
            AppLogMng.DisplayLog("关闭设备成功!",true);
        else
            AppLogMng.DisplayLog("关闭设备失败!", false);

        ClearCurDeviceInfo();
    }

    /// <summary>
    /// 清除当前设备信息
    /// </summary>
    private void ClearCurDeviceInfo()
    {
        curCanDeviceType = 0;//清除当前设备类型

        curCanFrameType = 0;//清除当前CAN帧类型

        canDeviceOpenFlag = false;//清除是否有设备打开

        zlgDevice = null;//清除周立功设备实例

        ClearSessionRuntimeBuffers();
    }

    /// <summary>
    /// 清空接收快照与单帧发送队列（关设备 / 重连 / 换矩阵时调用）。不清理周期发送表。
    /// </summary>
    public void ClearSessionRuntimeBuffers()
    {
        ClearCurWaitToHandleRecvMsg();
        waitToHandle_SendCanMsgBuf.Clear();
    }

    /// <summary>
    /// 清空周期发送表（换矩阵后重建前调用）。
    /// </summary>
    public void ClearCycleMsgSendDict()
    {
        task_CycleMsgSendDict.Clear();
    }

    /// <summary>
    /// 从硬件设备接收报文到软件设备对象报文缓存区
    /// </summary>
    public void MainLoopThread_Task_ReceiveMessagesFromDevice()
    {
        //未打开设备 直接返回
        if (canDeviceOpenFlag == false)
        {
            return;
        }

        //根据设备类型从相应设备中获取接收到的报文
        switch (curCanDeviceType)
        {
            case CanDeviceType.ZCAN_USBCANFD_100U:
            case CanDeviceType.ZCAN_USBCANFD_200U:
            case CanDeviceType.ZCAN_USBCANFD_MINI:
                //zlg设备接收报文
                if (zlgDevice is not null) zlgDevice.ReceiveMessagesFromDevice();
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// 从软件设备对象获取接收报文到设备总接口未处理的接收报文缓存区
    /// </summary>
    public void MainLoopThread_Task_GetRecvMsgFromDeviceBuf()
    {
        //未打开设备 直接返回
        if (canDeviceOpenFlag == false)
        {
            return;
        }

        //根据设备类型从相应设备对象中获取接收到的报文
        switch (curCanDeviceType)
        {
            case CanDeviceType.ZCAN_USBCANFD_100U:
            case CanDeviceType.ZCAN_USBCANFD_200U:
            case CanDeviceType.ZCAN_USBCANFD_MINI:
                //zlg设备获取接收报文（按 ID 合并，保留最新）
                if (zlgDevice is not null)
                {
                    lock (_recvSync)
                        zlgDevice.GetRecvBufferValidMsg(waitToHandle_RecvCanMsgById);
                }
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// 获取当前等待处理的接收报文（列表快照，不清除缓冲）
    /// </summary>
    public List<Canfd_Frame_Com> GetCurWaitToHandleRecvMsg()
    {
        lock (_recvSync)
        {
            if (waitToHandle_RecvCanMsgById.Count == 0)
                return new List<Canfd_Frame_Com>();
            return new List<Canfd_Frame_Com>(waitToHandle_RecvCanMsgById.Values);
        }
    }

    /// <summary>
    /// 取出并清空待处理接收报文（供 UI 泵拉取，同 ID 为最新帧）
    /// </summary>
    public Dictionary<uint, Canfd_Frame_Com> TakeRecvSnapshot()
    {
        lock (_recvSync)
        {
            if (waitToHandle_RecvCanMsgById.Count == 0)
                return null;

            var snapshot = new Dictionary<uint, Canfd_Frame_Com>(waitToHandle_RecvCanMsgById);
            waitToHandle_RecvCanMsgById.Clear();
            return snapshot;
        }
    }

    /// <summary>
    /// 获取当前等待处理的接收报文字典（实时引用，调用方勿长期持有；优先用 TakeRecvSnapshot）
    /// </summary>
    public Dictionary<uint, Canfd_Frame_Com> GetCurWaitToHandleRecvMsgById()
    {
        return waitToHandle_RecvCanMsgById;
    }

    /// <summary>
    /// 清除所有当前等待处理的接收报文
    /// </summary>
    public void ClearCurWaitToHandleRecvMsg()
    {
        lock (_recvSync)
            waitToHandle_RecvCanMsgById.Clear();
    }

    /// <summary>
    /// 从周期发送任务列表中 增加或者删除一个报文周期发送
    /// </summary>
    /// <param name="msgId">周期报文ID</param>
    /// <param name="msgCycle">报文发送周期</param>
    /// <param name="isAddMsg">1：增加周期报文发送/0：删除周期报文发送</param>
    public void AddOrDelOneCycleMsgSend(CanMessage msgInfo , uint isAddMsg)
    {
        if (isAddMsg == 1)
        {
            if (task_CycleMsgSendDict.ContainsKey(msgInfo.msgId)) return;//已有该报文 退出

            // 产品规则：MsgCycle==0 表示不参与周期发送
            if (msgInfo.msgCycle == 0)
                return;

            CycleSend_Canfd_Frame cycleSend_Canfd_Frame = new CycleSend_Canfd_Frame();
            cycleSend_Canfd_Frame.msgData = new Canfd_Frame_Com();
            cycleSend_Canfd_Frame.msgData.can_id = msgInfo.msgId;
            cycleSend_Canfd_Frame.msgData.data = new byte[64];
            cycleSend_Canfd_Frame.msgData.len = (byte)msgInfo.msgSize;
            // Excel/矩阵中 MsgCycle 单位为 ms，TimerTool 使用 µs
            cycleSend_Canfd_Frame.sendCycle = (ulong)msgInfo.msgCycle * (ulong)TimeUnit.T_MS;

            task_CycleMsgSendDict.Add(msgInfo.msgId, cycleSend_Canfd_Frame);
        }
        else
        {
            if(task_CycleMsgSendDict.ContainsKey(msgInfo.msgId)) task_CycleMsgSendDict.Remove(msgInfo.msgId);
        }
    }

    /// <summary>
    /// 新增一个单帧报文到待发送列表
    /// </summary>
    /// <param name="frame">单帧报文帧数据</param>
    public void AddOneMsgToSend(Canfd_Frame_Com frame)
    {
        waitToHandle_SendCanMsgBuf.Enqueue(frame);
    }

    /// <summary>
    /// 尝试从软件设备对象报文缓存区发送一帧报文到总线
    /// </summary>
    public void MainLoopThread_Task_SendMessagesToDevice()
    {
        //未打开设备 直接返回
        if (canDeviceOpenFlag == false)
        {
            return;
        }

        Canfd_Frame_Com canfd_Frame_Com = new Canfd_Frame_Com();

        //优先发送当前等待发送的单帧报文
        if (waitToHandle_SendCanMsgBuf.Count > 0)
        {
            canfd_Frame_Com = waitToHandle_SendCanMsgBuf.Dequeue();
        }
        else//无单帧报文发送，尝试发送周期报文
        {
            //检测满足发送周期时间的报文
            foreach (var item in task_CycleMsgSendDict)
            {
                if (TimerTool.CheckTimeOut(item.Value.sendTimer, item.Value.sendCycle))
                {
                    //重置发送计时器
                    CycleSend_Canfd_Frame _tmpFrame = item.Value;
                    TimerTool.ResetTimer(ref _tmpFrame.sendTimer);
                    task_CycleMsgSendDict[item.Key] = _tmpFrame;
                    //设置发送帧数据
                    canfd_Frame_Com = task_CycleMsgSendDict[item.Key].msgData;

                    break;
                }
            }
            
        }

        if (canfd_Frame_Com.can_id == 0) return;//无报文数据发送，退出

        //根据设备类型从相应设备中获取接收到的报文
        switch (curCanDeviceType)
        {
            case CanDeviceType.ZCAN_USBCANFD_100U:
            case CanDeviceType.ZCAN_USBCANFD_200U:
            case CanDeviceType.ZCAN_USBCANFD_MINI:
                //zlg设备发送报文
                if (zlgDevice is not null) zlgDevice.TransmitMessagesToDevice(canfd_Frame_Com);
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// 获取周期发送报文数据列表，用于填充发送数据
    /// </summary>
    /// <returns></returns>
    public Dictionary<uint, CycleSend_Canfd_Frame> GetCycleMsgSendDict()
    {
        return task_CycleMsgSendDict;
    }

    /// <summary>
    /// 发送一帧诊断请求报文
    /// </summary>
    public void UDS_SendOneUdsDiagRequest(Canfd_Frame_Com frameData)
    {
        //未打开设备 直接返回
        if (canDeviceOpenFlag == false)
        {
            return;
        }

        AddOneMsgToSend(frameData);
    }
}

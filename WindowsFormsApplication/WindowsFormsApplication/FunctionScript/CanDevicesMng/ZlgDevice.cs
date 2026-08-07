using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

public class ZlgDevice
{
    //CAN设备句柄
    private uint canDeviceHandle;

    //CAN通道句柄
    private uint canChannelHandle = 0;

    //设备接收有效报文帧缓存数据列表
    List<ZCANDataObj_CSharp> receiveValidFrameBuffer = new List<ZCANDataObj_CSharp>();

    //此次连接设备已接收的有效报文数量
    uint hasRecvValidMsgNumDuringThisTime = 0;

    //设备接收错误帧缓存数据列表
    List<ZCANDataObj_CSharp> receiveErrFrameBuffer = new List<ZCANDataObj_CSharp>();

    //此次连接设备已接收的错误帧数量
    uint hasRecvErrFrameNumDuringThisTime = 0;

    struct Can_Init_Config
    {
        public uint can_type;
        public uint acc_code;
        public uint acc_mask;
        public uint abit_timing;
        public uint dbit_timing;
        public uint brp;
        public byte filter;
        public byte mode;
        public UInt16 pad;
        public uint reserved;
    }

    //用于C#程序的ZCANDataObj结构体数据
    struct ZCANDataObj_CSharp
    {
        public byte dataType;               // 数据类型, 参考eZCANDataDEF中 数据类型 部分定义
        public byte chnl;                   // 数据通道

        public ulong timeStamp;                  // 时间戳,数据接收时单位微秒(us),队列延时发送时,数据单位取决于flag.unionVal.txDelay

        //ZCANErrorData
        public byte errType;                    // 错误类型, 参考eZCANErrorDEF中 总线错误类型 部分值定义
        public byte errSubType;                 // 错误子类型, 参考eZCANErrorDEF中 总线错误子类型 部分值定义
        public byte nodeState;                  // 节点状态, 参考eZCANErrorDEF中 节点状态 部分值定义
        public byte rxErrCount;                 // 接收错误计数
        public byte txErrCount;                 // 发送错误计数
        public byte errData;                    // 错误数据, 和当前错误类型以及错误子类型定义的具体错误相关, 具体请参考使用手册

        //ZCANCANFDData
        public byte frameType;                  // 帧类型, 0:CAN帧, 1:CANFD帧
        public byte txDelay;                    // 队列发送延时, 发送有效. 0:无发送延时, 1:发送延时单位ms, 2:发送延时单位100us. 启用队列发送延时，延时时间存放在timeStamp字段
        public byte transmitType;               // 发送类型, 发送有效. 0:正常发送, 1:单次发送, 2:自发自收, 3:单次自发自收. 所有设备支持正常发送，其他类型请参考具体使用手册
        public byte txEchoRequest;              // 发送回显请求, 发送有效. 支持发送回显的设备,发送数据时将此位置1,设备可以通过接收接口将发送出去的数据帧返回,接收到的发送数据使用txEchoed位标记
        public byte txEchoed;                   // 报文是否是回显报文, 接收有效. 0:正常总线接收报文, 1:本设备发送回显报文.
        public canfd_frame canData;

    };

    struct canfd_frame
    {
        public uint can_id; /* 32 bit CAN_ID + EFF/RTR/ERR flags */
        public byte len; /* frame payload length in byte */
        public byte flags; /* additional flags for CAN FD,i.e error code */
        public byte __res0; /* reserved / padding */
        public byte __res1; /* reserved / padding */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] data/* __attribute__((aligned(8)))*/;
    }

    struct ZCANErrorData
    {
        uint timeStamp;//时间戳，表示错误产生的时间，时间单位为微秒(us)
        byte errType;//0未知错误;1总线错误;2控制器错误;3终端设备错误
        byte errSubType;//错误子类型，错误子类型的值根据错误类型不同表示不用的含义,具体请查文档
        byte nodeState;//1总线积极;2总线告警;3总线消极;4总线关闭
        byte rxErrCount;//接收错误计数，错误类型(errType)为总线错误(1)时有效
        byte txErrCount;//发送错误计数，错误类型(errType)为总线错误(1)时有效
        byte errData;//错误数据，错误类型(errType)为终端设备错误(3)且错误子类型(errSubType)为定时发送失败(3)时有效，用来存放定时发送帧的索引
        byte reserved1;
        byte reserved2;
    }

    [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint ZCAN_OpenDevice(uint device_type, uint device_index, uint reserved);

    [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint ZCAN_CloseDevice(uint device_handle);

    [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint ZCAN_SetValue(uint device_handle, string path, string value);

    [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
    static extern ref string ZCAN_GetValue(uint device_handle, string path);

    [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint ZCAN_InitCAN(uint device_handle, uint can_index, ref Can_Init_Config initConfig);

    [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint ZCAN_StartCAN(uint channel_handle);

    [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint ZCAN_ResetCAN(uint channel_handle);

    [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint ZCAN_GetReceiveNum(uint channel_handle, byte type);

    [DllImport("ZcanDeviceInterface.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint ZCAN_ReceiveData_Interface(uint device_handle, ref ZCANDataObj_CSharp pReceive_CSharp);

    [DllImport("ZcanDeviceInterface.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint ZCAN_TransmitData_Interface(uint device_handle, ZCANDataObj_CSharp pTransmit_CSharp);

    /// <summary>
    /// 打开CAN设备
    /// </summary>
    /// <returns>打开设备是否成功</returns>
    public bool OpenDevice(CanDeviceType deviceType, CanFrameType frameType)
    {
        //Step1：打开设备，deviceType代表CAN卡类型，
        //第2个参数为设备索引号，比如当只有一个 USBCANFD-200U 时，索引号为 0，这时再插入一个 USBCANFD - 200U，那么后面插入的这个设备索引号就是 1，以此类推
        canDeviceHandle = ZCAN_OpenDevice((uint)deviceType, 0, 0);
        if (canDeviceHandle == 0)
        {
            AppLogMng.DisplayLog("打开设备失败!", false);
            return false;
        }

        //Step2：设置波特率
        //仲裁域默认设置500K
        if (ZCAN_SetValue(canDeviceHandle, "0/canfd_abit_baud_rate", "500000") != 1)
        {
            AppLogMng.DisplayLog("设置仲裁域波特率失败!", false);
            return false;
        }
        //数据域设置(CAN-500K,CANFD-2M)
        if (frameType == CanFrameType.CANFD)
        {
            if (ZCAN_SetValue(canDeviceHandle, "0/canfd_dbit_baud_rate", "2000000") != 1)
            {
                AppLogMng.DisplayLog("设置数据域波特率失败!", false);
                return false;
            }
        }
        else
        {
            if (ZCAN_SetValue(canDeviceHandle, "0/canfd_dbit_baud_rate", "500000") != 1)
            {
                AppLogMng.DisplayLog("设置数据域波特率失败!", false);
                return false;
            }

        }

        //CAN通道初始化
        Can_Init_Config can_Init_Config = new Can_Init_Config();
        can_Init_Config.can_type = 1; //canfd 设备类型
        can_Init_Config.filter = 0;
        can_Init_Config.mode = 0; //0正常模式, 1 为只听模式
        can_Init_Config.acc_code = 0;
        can_Init_Config.acc_mask = 0xffffffff;
        can_Init_Config.brp = 0;
        canChannelHandle = ZCAN_InitCAN(canDeviceHandle, 0, ref can_Init_Config);
        if (canChannelHandle == 0)
        {
            AppLogMng.DisplayLog("初始化通道失败!", false);
            return false;
        }

        // 使能通道终端电阻
        if (0 == ZCAN_SetValue(canDeviceHandle, "0/initenal_resistance", "1"))
        {
            AppLogMng.DisplayLog("使能通道终端电阻失败!", false);
        }

        // 设置通道发送超时时间为 100ms
        if (0 == ZCAN_SetValue(canDeviceHandle, "0/tx_timeout", "100"))
        {
            AppLogMng.DisplayLog("设置通道发送超时时间失败!", false);
        }

        //// 仅对 0 通道设置滤波
        //if (0 == i)
        //{
        //    // 设置第一组滤波，只接收 ID 范围在 0x100-0x200 之间的标准帧
        //    ZCAN_SetValue(canDeviceHandle, "0/filter_mode", "0"); // 标准帧
        //    ZCAN_SetValue(canDeviceHandle, "0/filter_start", "0x100"); // 起始 ID
        //    ZCAN_SetValue(canDeviceHandle, "0/filter_end", "0x200"); // 结束 ID
        //                                                    // 设置第二组滤波，只接收 ID 范围在 0x1FFFF-0x2FFFF 之间的扩展帧
        //    ZCAN_SetValue(canDeviceHandle, "0/filter_mode", "1"); // 扩展帧
        //    ZCAN_SetValue(canDeviceHandle, "0/filter_start", "0x1FFFF"); // 起始 ID
        //    ZCAN_SetValue(canDeviceHandle, "0/filter_end", "0x2FFFF"); // 结束 ID
        //                                                      // 使能滤波
        //    ZCAN_SetValue(canDeviceHandle, "0/filter_ack", "0");
        //    // 清除滤波,此处仅举例，何时调用用户自由决定
        //    // ZCAN_SetValue(device, "0/filter_clear", "0");
        //}

        // 设置合并接收标志，启用合并发送，接收接口（只需设置 1 次）
        ZCAN_SetValue(canDeviceHandle, "0/set_device_recv_merge", "1");

        //启动CAN通道
        if (ZCAN_StartCAN(canChannelHandle) != 1)
        {
            AppLogMng.DisplayLog("启动通道失败!", false);
            return false;
        }

        return true;
    }

    /// <summary>
    /// 关闭当前打开设备
    /// </summary>
    /// <returns>关闭设备是否成功</returns>
    public bool CloseDevice()
    {
        uint tmp = 0;
        if (canDeviceHandle != 0)
        {
            tmp = ZCAN_CloseDevice(canDeviceHandle);
        }

        //清除can设备句柄和can通道句柄
        canDeviceHandle = 0;
        canChannelHandle = 0;

        if (tmp == 0)
        {
            AppLogMng.DisplayLog("关闭设备失败!", false);
            return false;
        }

        //关闭设备后清除当前接收报文计数
        hasRecvValidMsgNumDuringThisTime = 0;
        hasRecvErrFrameNumDuringThisTime = 0;

        return true;
    }

    //获取 can_id; /* 32 bit CAN_ID + EFF/RTR/ERR flags */
    /// <summary>
    /// 获取周立功ID
    /// </summary>
    /// <param name="id">报文帧实际ID</param>
    /// <param name="type1">第 31 位(最高位)代表扩展帧标志，=0 表示标准帧，=1 代表扩展帧</param>
    /// <param name="type2">第 30 位代表远程帧标志，=0 表示数据帧，=1 表示远程帧</param>
    /// <param name="type3">第 29 位代表错误帧标准，=0 表示 CAN 帧，=1 表示错误帧</param>
    /// <returns></returns>
    public uint GetZCANId(uint id, uint type1 = 0, uint type2 = 0, uint type3 = 0)
    {
        uint retId = 0;
        retId |= type1 << 31;
        retId |= type2 << 30;
        retId |= type3 << 29;
        retId |= id;

        return retId;
    }

    /// <summary>
    /// 发送报文至设备总线-每次调用发送一帧
    /// </summary>
    /// <param name="msgData">报文帧数据</param>
    /// <returns>报文发送是否成功</returns>
    public bool TransmitMessagesToDevice(Canfd_Frame_Com msgData)
    {
        uint succSendMsgNum = 0;
        ZCANDataObj_CSharp sendData = new ZCANDataObj_CSharp();
        sendData.canData = new canfd_frame();
        sendData.chnl = 0;
        sendData.canData.can_id = msgData.can_id;
        sendData.canData.len = msgData.len;
        sendData.frameType = msgData.is_canfd;
        sendData.canData.data = msgData.data;

        succSendMsgNum = ZCAN_TransmitData_Interface(canDeviceHandle, sendData);

        return (succSendMsgNum>0)?true:false;
    }

    private const uint RecvBatchMax = 64;

    /// <summary>
    /// 从当前设备批量获取接收报文（单次最多 RecvBatchMax 帧）
    /// </summary>
    public void ReceiveMessagesFromDevice()
    {
        uint recvMsgNum = ZCAN_GetReceiveNum(canChannelHandle, 2);//0=CAN，1=CANFD，2=合并接收
        if (recvMsgNum == 0)
            return;

        uint toRead = recvMsgNum > RecvBatchMax ? RecvBatchMax : recvMsgNum;
        bool gotAny = false;

        for (uint i = 0; i < toRead; i++)
        {
            ZCANDataObj_CSharp cur_recv_data = new ZCANDataObj_CSharp();
            ZCAN_ReceiveData_Interface(canDeviceHandle, ref cur_recv_data);

            if (cur_recv_data.dataType == 1)
            {
                receiveValidFrameBuffer.Add(cur_recv_data);
                hasRecvValidMsgNumDuringThisTime++;
                gotAny = true;
            }
            else if (cur_recv_data.dataType == 2)
            {
                receiveErrFrameBuffer.Add(cur_recv_data);
                hasRecvErrFrameNumDuringThisTime++;
                gotAny = true;
            }
        }

        // 按批节流日志，避免热路径每帧写日志
        if (gotAny)
        {
            AppLogMng.DisplayLog("接收有效报文数量：" + hasRecvValidMsgNumDuringThisTime.ToString() + "/"
                                + "接收错误帧数量：" + hasRecvErrFrameNumDuringThisTime.ToString()
                                , true);
        }
    }

    /// <summary>
    /// 将设备缓存有效报文合并到按 ID 索引的待处理字典（同 ID 保留最新）
    /// </summary>
    public void GetRecvBufferValidMsg(Dictionary<uint, Canfd_Frame_Com> getRecvMsgById)
    {
        if (receiveValidFrameBuffer.Count == 0 || getRecvMsgById is null)
            return;

        foreach (var item in receiveValidFrameBuffer)
        {
            Canfd_Frame_Com tmpMsg = new Canfd_Frame_Com();
            tmpMsg.can_id = item.canData.can_id;
            tmpMsg.len = item.canData.len;
            tmpMsg.data = item.canData.data;
            tmpMsg.is_canfd = item.frameType;
            getRecvMsgById[tmpMsg.can_id] = tmpMsg;
        }

        receiveValidFrameBuffer.Clear();
    }

    /// <summary>
    /// 获取当前缓存区中已接收到的错误数据
    /// </summary>
    public void GetRecvBufferErrData()
    {

    }
}

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

    //报文发送失败后尝试重发定时器
    private ulong resendTimer = TimerTool.GetSysTime();

    //设备接收报文缓存数据列表
    List<ZCANDataObj_CSharp> receiveMsgBuffer = new List<ZCANDataObj_CSharp>();

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
    static extern uint ZCAN_TransmitData(uint device_handle, ref ZCANDataObj_CSharp[] pTransmit_CSharp, uint len);

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
    /// 发送CAN报文
    /// </summary>
    /// <param name="msgId">报文ID</param>
    /// <param name="data">报文数据</param>
    /// <returns></returns>
    public bool Transmit_CanFrame(uint msgId, byte[] data)
    {
        //Can_Transmit_Data canData = new Can_Transmit_Data();
        //canData.can_id = GetZCANId(msgId);
        //canData.can_dlc = 8;
        //canData.data = data;
        //canData.transmit_type = 0;

        ////如果can通道未正常打开
        //if (canChannelHandle == 0)
        //{
        //    return false;
        //}

        //if (TimerTool.CheckTimeOut(resendTimer, 5*(ulong)TimeUnit.T_S) == true)//失败5s尝试重发一次
        //{
        //    if (ZCAN_Transmit(canChannelHandle, ref canData, 1) != 1)
        //    {
        //        AppLogMng.DisplayLog("报文发送失败，尝试重新发送!", false);

        //        TimerTool.ResetTimer(ref resendTimer);
        //        return false;
        //    }
        //}

        return true;
    }


    public bool Receive_CanFrame()
    {
        //ZCANDataObj canData = new ZCANDataObj();
        //canData.dataType = 1;
        //canData.chnl = 0;
        //canData.data = new ZCANDataObj_data();
        //canData.data.zcanCANFDData = new ZCANCANFDData();
        //canData.data.zcanCANFDData.flag = 1;//接收CANFD报文

        //uint recvMsgNum = 0;
        //recvMsgNum = ZCAN_GetReceiveNum(canChannelHandle, 2);//0=CAN，1=CANFD，2=合并接收

        //if (recvMsgNum == 0)
        //{
        //    return false;
        //}

        ////waitTime:缓冲区无数据，函数阻塞等待时间，单位毫秒。若为-1 则表示无超时，一直等待，默认值为 - 1
        //uint realRecvMsgNum  = ZCAN_ReceiveData(canDeviceHandle, ref canData, 1, -1);
        //if (realRecvMsgNum == 0)
        //{
        //    return false;
        //}
        //else
        //{
        //    AppLogMng.DisplayLog("111", true);
        //    //msgId = recvData.frame.can_id;
        //    //Can_Uint64_Data can_Uint64_Data = new Can_Uint64_Data();
        //    //can_Uint64_Data.BYTE0 = recvData.frame.data[0];
        //    //can_Uint64_Data.BYTE1 = recvData.frame.data[1];
        //    //can_Uint64_Data.BYTE2 = recvData.frame.data[2];
        //    //can_Uint64_Data.BYTE3 = recvData.frame.data[3];
        //    //can_Uint64_Data.BYTE4 = recvData.frame.data[4];
        //    //can_Uint64_Data.BYTE5 = recvData.frame.data[5];
        //    //can_Uint64_Data.BYTE6 = recvData.frame.data[6];
        //    //can_Uint64_Data.BYTE7 = recvData.frame.data[7];

        //    //msgData = can_Uint64_Data;
        //}

        return true;
    }

    /// <summary>
    /// 任务-从当前设备实时接收报文
    /// </summary>
    public void ReceiveMessagesFromDevice()
    {
        //判断总线是否有报文数据
        uint recvMsgNum = 0;
        recvMsgNum = ZCAN_GetReceiveNum(canChannelHandle, 2);//0=CAN，1=CANFD，2=合并接收

        if (recvMsgNum == 0)
        {
            return;
        }

        //接收报文数据一帧
        ZCANDataObj_CSharp cur_recv_data = new ZCANDataObj_CSharp(); // 定义报文接收结构体

        //剩余可以获取的报文数量
        uint availableDataNum = ZCAN_ReceiveData_Interface(canDeviceHandle,ref cur_recv_data);

        if (cur_recv_data.dataType > 0)
        {
            //存储接收报文到缓冲区
            receiveMsgBuffer.Add(cur_recv_data);

            AppLogMng.DisplayLog(receiveMsgBuffer.Count().ToString() + $"---0x{ cur_recv_data.canData.can_id.ToString("X4")}", true);

            //AppLogMng.DisplayLog(cur_recv_data.canData.can_id.ToString(), true);

            //for (int i = 0; i < rcount; i++)
            //{
            //    AppLogMng.DisplayLog(recv_data.canData.can_id.ToString(), true);

            //    if (recv_data[i].dataType != 0)
            //        AppLogMng.DisplayLog(recv_data[i].dataType.ToString(), true);

            //    if (recv_data[i].dataType != 1)
            //    {
            //        //只处理 CAN 或 CANFD 数据
            //        continue;
            //    }

            //    AppLogMng.DisplayLog(recv_data[i].data.zcanCANFDData.canfd_Frame.can_id.ToString(), true);


            //}
        }

        //while (g_thd_run)
        //{
        //    int rcount = ZCAN_ReceiveData(handle, recv_data, 100, 1);
        //    int lcount = rcount;
        //    while (g_thd_run && lcount > 0)
        //    {
        //        for (int i = 0; i < rcount; ++i, --lcount)
        //        {
        //            if (recv_data[i].dataType != ZCAN_DT_ZCAN_CAN_CANFD_DATA)
        //            { // 
        //                只处理 CAN 或 CANFD 数据
        //            continue;
        //            }
        //            std::cout << "CHNL: " << std::dec << (int)recv_data[i].chnl; // 打印通道
        //            ZCANCANFDData & can_data = recv_data[i].data.zcanCANFDData;
        //            std::cout << " TIME:" << std::fixed << std::setprecision(6) <<
        //            can_data.timeStamp / 1000000.0f << "s[" <<
        //            std::dec << can_data.timeStamp << "]"; // 打印时间戳
        //            if (can_data.flag.unionVal.txEchoed == 1)
        //            {
        //                std::cout << " [TX] "; // 发送帧
        //            }
        //            else
        //            {
        //                std::cout << " [RX] "; // 接收帧
        //            }
        //            std::cout << "ID: 0x" << std::hex << can_data.frame.can_id; // 打印 ID
        //            std::cout << " LEN " << std::dec << (int)can_data.frame.len; // 打印长度
        //            std::cout << " DATA " << std::hex; // 打印数据
        //            for (int ind = 0; ind < can_data.frame.len; ++ind)
        //            {
        //                std::cout << std::hex << " " << (int)can_data.frame.data[ind];
        //            }
        //            std::cout << std::endl;
        //        }
        //    }
        //    Sleep(10);
        //}
        //std::cout << "Thread exit" << std::endl;
    }
}

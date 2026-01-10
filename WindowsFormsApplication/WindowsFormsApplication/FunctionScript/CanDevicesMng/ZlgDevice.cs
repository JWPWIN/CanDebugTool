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

    struct ZCANDataObj
    {
        public byte dataType;  //当前结构的数据类型: 1-CAN/CANFD数据;2-错误数据;3-GPS数据;4-LIN数据;5-总线利用率数据;6-LIN错误数据
        public byte chnl;      //数据通道:数据类型表示CAN/CANFD/错误数据时，通道表示的是CAN通道 数据类型表示的是 LIN 数据时，通道表示的是设备的 LIN 通道
        public ushort flag;    //数据标志，暂未使用
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] extraData;//数据标志，暂未使用
        public ZCANDataObj_data data;//data 成员定义为联合体（CAN/CANFD 数据，dataType 值为 1 时有效；错误数据，dataType 值为 2 时有效）
    }

    //[StructLayout(LayoutKind.Explicit)]
    struct ZCANDataObj_data
    {
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 92)]
        //[FieldOffset(0)] public byte[] raw;
        /*[FieldOffset(0)]*/ public ZCANCANFDData zcanCANFDData;
        //[FieldOffset(0)] public ZCANErrorData zcanErrData;
    }

    struct ZCANCANFDData
    {
        //时间戳，作为接收帧时，时间戳单位微秒(us)。
        //正常发送时，timeStamp 字段无意义。
        //队列延迟发送数据时，timeStamp 字段存放发送当前帧后设备等待的时间，时间单位取决于 flag.unionVal.txDelay，等待时间结束后设备发送下一帧
        public ulong timeStamp;
        public uint flag; //flag 字段表示 CAN/CANFD 帧的标记信息，长度 4 字节
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] extraData;//数据标志，暂未使用
        public canfd_frame canfd_Frame;
    }

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

    [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint ZCAN_ReceiveData(uint device_handle, ref ZCANDataObj pReceive, uint len, int wait_time);

    [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint ZCAN_TransmitData(uint device_handle, ref ZCANDataObj pTransmit, uint len);

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
            if (ZCAN_SetValue(canDeviceHandle, "0/canfd_abit_baud_rate", "2000000") != 1)
            {
                AppLogMng.DisplayLog("设置数据域波特率失败!", false);
                return false;
            }
        }
        else
        {
            if (ZCAN_SetValue(canDeviceHandle, "0/canfd_abit_baud_rate", "500000") != 1)
            {
                AppLogMng.DisplayLog("设置数据域波特率失败!", false);
                return false;
            }

        }

        // 设置合并接收标志，启用合并发送，接收接口（只需设置 1 次）
        ZCAN_SetValue(canDeviceHandle, "0/set_device_recv_merge", "1");

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
        ZCANDataObj canData = new ZCANDataObj();
        canData.dataType = 1;
        canData.chnl = 0;
        canData.data = new ZCANDataObj_data();
        canData.data.zcanCANFDData = new ZCANCANFDData();
        canData.data.zcanCANFDData.flag = 1;//接收CANFD报文

        uint recvMsgNum = 0;
        recvMsgNum = ZCAN_GetReceiveNum(canChannelHandle, 2);//0=CAN，1=CANFD，2=合并接收

        if (recvMsgNum == 0)
        {
            return false;
        }

        //waitTime:缓冲区无数据，函数阻塞等待时间，单位毫秒。若为-1 则表示无超时，一直等待，默认值为 - 1
        uint realRecvMsgNum  = ZCAN_ReceiveData(canDeviceHandle, ref canData, 1, -1);
        if (realRecvMsgNum == 0)
        {
            return false;
        }
        else
        {
            AppLogMng.DisplayLog("111", true);
            //msgId = recvData.frame.can_id;
            //Can_Uint64_Data can_Uint64_Data = new Can_Uint64_Data();
            //can_Uint64_Data.BYTE0 = recvData.frame.data[0];
            //can_Uint64_Data.BYTE1 = recvData.frame.data[1];
            //can_Uint64_Data.BYTE2 = recvData.frame.data[2];
            //can_Uint64_Data.BYTE3 = recvData.frame.data[3];
            //can_Uint64_Data.BYTE4 = recvData.frame.data[4];
            //can_Uint64_Data.BYTE5 = recvData.frame.data[5];
            //can_Uint64_Data.BYTE6 = recvData.frame.data[6];
            //can_Uint64_Data.BYTE7 = recvData.frame.data[7];

            //msgData = can_Uint64_Data;
        }

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
        

        //接收报文数据
        ZCANDataObj[] recv_data = new ZCANDataObj[100]; // 定义接收数据缓冲区，100 仅用于举例，根据实际情况定义

        uint rcount = ZCAN_ReceiveData(canDeviceHandle, ref recv_data[0], 100, 1);

        if (rcount > 0)
        {
            for (int i = 0; i < rcount; i++)
            {
                if (recv_data[i].dataType != 1)
                {  
                    //只处理 CAN 或 CANFD 数据
                    continue;
                }

                AppLogMng.DisplayLog(recv_data[i].data.zcanCANFDData.canfd_Frame.can_id.ToString(),true);


            }
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

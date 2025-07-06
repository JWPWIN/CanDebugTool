using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class CanDeviceMng
{
    static private CanDeviceMng instance;

    //CAN设备句柄
    uint canDeviceHandle;

    //CAN通道句柄
    uint canChannelHandle = 0;

    //报文发送失败后尝试重发定时器：5s尝试重发一次
    float resendTimer = 5f;


    static public CanDeviceMng GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("CanDeviceMng instance dont existed !");
            return null;
        }
        return instance;
    }

    public CanDeviceMng()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

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

    struct Can_Transmit_Data
    {
        public uint can_id; /* 32 bit CAN_ID + EFF/RTR/ERR flags */
        public byte can_dlc; /* frame payload length in byte (0 .. CAN_MAX_DLEN) */
        public byte __pad; /* padding */
        public byte __res0; /* reserved / padding */
        public byte __res1; /* reserved / padding */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] data;/* __attribute__((aligned(8)))*/
        public uint transmit_type; //0=正常发送，1=单次发送，2=自发自收，3=单次自发自收
    }

    struct Can_Receive_Data
    {
        public Can_Transmit_Data frame;
        public UInt64 timeStamp;//时间戳，单位微秒，基于设备启动时间
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
    static extern uint ZCAN_Transmit(uint channel_handle, ref Can_Transmit_Data data, uint num );

    [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint ZCAN_GetReceiveNum(uint channel_handle, byte type);

    [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint ZCAN_Receive(uint channel_handle, ref Can_Receive_Data recvData, uint len,int waitTime);

    /// <summary>
    /// 打开CAN设备
    /// </summary>
    /// <returns>打开设备是否成功</returns>
    public bool OpenDevice()
    {
        //打开设备，43代表CAN卡类型，0代表0通道
        uint canDeviceHandle = ZCAN_OpenDevice(43, 0, 0);
        if (canDeviceHandle == 0)
        {
            LogMng.GetInstance().DisplayLog("打开设备失败");
            return false;
        }

        //设置CAN设备属性，波特率500k
        if (ZCAN_SetValue(canDeviceHandle, "0/canfd_abit_baud_rate", "500000") != 1)
        {
            LogMng.GetInstance().DisplayLog("设置波特率失败");
            return false;
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
            LogMng.GetInstance().DisplayLog("初始化通道失败");
            return false;
        }

        //启动CAN通道
        if (ZCAN_StartCAN(canChannelHandle) != 1)
        {
            LogMng.GetInstance().DisplayLog("启动通道失败");
            return false;
        }

        LogMng.GetInstance().DisplayLog("打开设备成功!");
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
            LogMng.GetInstance().DisplayLog("关闭设备失败");
            return false;
        }
        else
        {
            LogMng.GetInstance().DisplayLog("关闭设备成功");
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
        Can_Transmit_Data canData = new Can_Transmit_Data();
        canData.can_id = GetZCANId(msgId);
        canData.can_dlc = 8;
        canData.data = data;
        canData.transmit_type = 0;

        //如果can通道未正常打开
        if (canChannelHandle == 0)
        {
            return false;
        }

        if (resendTimer > 5)//5s尝试重发一次
        {
            if (ZCAN_Transmit(canChannelHandle, ref canData, 1) != 1)
            {
                LogMng.GetInstance().DisplayLog("报文发送失败，尝试重新发送");

                resendTimer = 0;
                return false;
            }
        }

        resendTimer += Time.deltaTime;

        return true;
    }


    public bool Receive_CanFrame(ref uint msgId, ref Can_Uint64_Data msgData)
    {
        uint recvMsgNum = 0;
        recvMsgNum = ZCAN_GetReceiveNum(canChannelHandle, 0);//0=CAN，1=CANFD，2=合并接收

        if (recvMsgNum == 0)
        {
            return false;
        }

        Can_Receive_Data recvData = new Can_Receive_Data();

        //waitTime:缓冲区无数据，函数阻塞等待时间，单位毫秒。若为-1 则表示无超时，一直等待，默认值为 - 1
        uint tmpNum = ZCAN_Receive(canChannelHandle, ref recvData, 1, -1);
        if (tmpNum == 0)
        {
            return false;
        }
        else
        {
            msgId = recvData.frame.can_id;
            Can_Uint64_Data can_Uint64_Data = new Can_Uint64_Data();
            can_Uint64_Data.BYTE0 = recvData.frame.data[0];
            can_Uint64_Data.BYTE1 = recvData.frame.data[1];
            can_Uint64_Data.BYTE2 = recvData.frame.data[2];
            can_Uint64_Data.BYTE3 = recvData.frame.data[3];
            can_Uint64_Data.BYTE4 = recvData.frame.data[4];
            can_Uint64_Data.BYTE5 = recvData.frame.data[5];
            can_Uint64_Data.BYTE6 = recvData.frame.data[6];
            can_Uint64_Data.BYTE7 = recvData.frame.data[7];

            msgData = can_Uint64_Data;
        }

        return true;
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

[StructLayout(LayoutKind.Explicit)]
public struct Can_Uint64_Data
{
    [FieldOffset(0)] public ulong allData;
    [FieldOffset(0)] public byte BYTE0;
    [FieldOffset(1)] public byte BYTE1;
    [FieldOffset(2)] public byte BYTE2;
    [FieldOffset(3)] public byte BYTE3;
    [FieldOffset(4)] public byte BYTE4;
    [FieldOffset(5)] public byte BYTE5;
    [FieldOffset(6)] public byte BYTE6;
    [FieldOffset(7)] public byte BYTE7;
}

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

public class DeviceInterfaceMng
{
    private CanDeviceType curCanDeviceType = 0;//当前设备类型

    private CanFrameType curCanFrameType = 0;//当前CAN帧类型

    private bool canDeviceOpenFlag = false;//是否有设备打开

    private ZlgDevice zlgDevice = null;//周立功设备实例

    /// <summary>
    /// 打开CAN卡设备
    /// </summary>
    /// <param name="selectCanType">下拉选项框选择的设备类型</param>
    /// <param name="canType">下拉选项框选择的Can类型</param>
    public void OpenCanDevice(int selectDeviceType,int selectCanType)
    {
        //未打开设备 直接返回
        if (canDeviceOpenFlag == true)
        {
            AppLogMng.DisplayLog("已打开过设备!");
            return;
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

        //Step2: 获取CAN帧类型{ "CANFD", "CAN"}
        switch (selectDeviceType)
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
            AppLogMng.DisplayLog("打开设备成功!");
        }

    }

    /// <summary>
    /// 关闭CAN卡设备
    /// </summary>
    public void CloseCanDevice()
    {
        //未打开设备 直接返回
        if(canDeviceOpenFlag == false)
        {
            AppLogMng.DisplayLog("未打开过设备!");
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

        if (successCloseFlag == true)
        {
            canDeviceOpenFlag = false;
            AppLogMng.DisplayLog("关闭设备成功!");
        }
    }


}

#include "typedef.h"
#include "SysTypes.h"
#include "zlgcan.h"

//用于C#程序的ZCANDataObj结构体数据
typedef struct ZCANDataObj_CSharp
{
    BYTE        dataType;               // 数据类型, 参考eZCANDataDEF中 数据类型 部分定义
    BYTE        chnl;                   // 数据通道

    UINT64  timeStamp;                  // 时间戳,数据接收时单位微秒(us),队列延时发送时,数据单位取决于flag.unionVal.txDelay

    //ZCANErrorData
    BYTE    errType;                    // 错误类型, 参考eZCANErrorDEF中 总线错误类型 部分值定义
    BYTE    errSubType;                 // 错误子类型, 参考eZCANErrorDEF中 总线错误子类型 部分值定义
    BYTE    nodeState;                  // 节点状态, 参考eZCANErrorDEF中 节点状态 部分值定义
    BYTE    rxErrCount;                 // 接收错误计数
    BYTE    txErrCount;                 // 发送错误计数
    BYTE    errData;                    // 错误数据, 和当前错误类型以及错误子类型定义的具体错误相关, 具体请参考使用手册

    //ZCANCANFDData
    BYTE    frameType;                  // 帧类型, 0:CAN帧, 1:CANFD帧
    BYTE    txDelay;                    // 队列发送延时, 发送有效. 0:无发送延时, 1:发送延时单位ms, 2:发送延时单位100us. 启用队列发送延时，延时时间存放在timeStamp字段
    BYTE    transmitType;               // 发送类型, 发送有效. 0:正常发送, 1:单次发送, 2:自发自收, 3:单次自发自收. 所有设备支持正常发送，其他类型请参考具体使用手册
    BYTE    txEchoRequest;              // 发送回显请求, 发送有效. 支持发送回显的设备,发送数据时将此位置1,设备可以通过接收接口将发送出去的数据帧返回,接收到的发送数据使用txEchoed位标记
    BYTE    txEchoed;                   // 报文是否是回显报文, 接收有效. 0:正常总线接收报文, 1:本设备发送回显报文.
    canfd_frame canData;

}ZCANDataObj_CSharp;


extern __declspec(dllimport) UINT ZCAN_ReceiveData(DEVICE_HANDLE device_handle, ZCANDataObj* pReceive, UINT len, int wait_time DEF(-1));

__declspec(dllexport) UINT ZCAN_ReceiveData_Interface(UINT device_handle, ZCANDataObj_CSharp* pReceive_CSharp)
{
    static ZCANDataObj ReceiveData_Buffer[100];
    int bufferNum = sizeof(ReceiveData_Buffer) / sizeof(ZCANDataObj);
    
    //判断缓存区内是否还有数据未取走
    int hasDataFlag = 0;
    for (int i = 0; i < bufferNum; i++)
    {
        if (ReceiveData_Buffer[i].dataType != 0)
        {
            hasDataFlag = 1;
            break;
        }
    }

    //尚有数据未取走，调用该函数仅取走数据，不再从设备重新获取数据
    if (hasDataFlag == 1)
    {
        for (int i = 0; i < bufferNum; i++)
        {
            //如果数据类型为0，代表没有数据，继续下一次判断
            if (ReceiveData_Buffer[i].dataType == 0) continue;

            pReceive_CSharp->dataType      = ReceiveData_Buffer[i].dataType;
            pReceive_CSharp->chnl          = ReceiveData_Buffer[i].chnl;
            pReceive_CSharp->timeStamp     = ReceiveData_Buffer[i].chnl;
            pReceive_CSharp->frameType     = ReceiveData_Buffer[i].data.zcanCANFDData.flag.unionVal.frameType;
            pReceive_CSharp->txDelay       = ReceiveData_Buffer[i].data.zcanCANFDData.flag.unionVal.txDelay;
            pReceive_CSharp->transmitType  = ReceiveData_Buffer[i].data.zcanCANFDData.flag.unionVal.transmitType;
            pReceive_CSharp->txEchoRequest = ReceiveData_Buffer[i].data.zcanCANFDData.flag.unionVal.txEchoRequest;
            pReceive_CSharp->txEchoed      = ReceiveData_Buffer[i].data.zcanCANFDData.flag.unionVal.txEchoed;
            pReceive_CSharp->canData       = ReceiveData_Buffer[i].data.zcanCANFDData.frame;

            //数据读取完成后，清除buffer内的该数据
            ReceiveData_Buffer[i].dataType = 0;

            break;
        }
    }
    else
    {
        //缓存区内没有数据，尝试重新从设备获取数据
        ZCAN_ReceiveData((DEVICE_HANDLE)device_handle, ReceiveData_Buffer, 100, 1);//每次最大接收100报文数据；无数据时阻塞等待1ms
    }

    //判断缓存区内还有多少数据未取走
    int _tmpNum = 0;
    for (int i = 0; i < bufferNum; i++)
    {
        if (ReceiveData_Buffer[i].dataType != 0)
        {
            _tmpNum++;
        }
    }

	return _tmpNum;
}
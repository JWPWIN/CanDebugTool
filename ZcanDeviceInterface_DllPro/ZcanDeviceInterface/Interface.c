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


//导入外部DLL接收和发送报文的接口（DLL来源:项目属性->链接器->输入->附加依赖项）
extern __declspec(dllimport) UINT ZCAN_ReceiveData(DEVICE_HANDLE device_handle, ZCANDataObj* pReceive, UINT len, int wait_time DEF(-1));
extern __declspec(dllimport) UINT ZCAN_TransmitData(DEVICE_HANDLE device_handle, ZCANDataObj *pTransmit, UINT len);


/// <summary>
/// 上层C#-Winfore程序用于接收一帧报文的接口
/// 如果当前接收Buffer有未取走的报文，取一帧报文出去；如果没有未取走的报文，尝试从设备读取
/// </summary>
/// <param name="device_handle">设备句柄</param>
/// <param name="pReceive_CSharp">C#接收报文帧数据的结构</param>
/// <returns>当前缓冲区未取走的报文数</returns>
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

/// <summary>
/// 上层C#-Winfore程序用于发送一帧报文的接口
/// </summary>
/// <param name="device_handle">设备句柄</param>
/// <param name="pTransmit_CSharp">C#发送报文帧数据的结构</param>
/// <returns>成功发送数量</returns>
__declspec(dllexport) UINT ZCAN_TransmitData_Interface(UINT device_handle,ZCANDataObj_CSharp pTransmit_CSharp)
{
    UINT sendMsgSuccNum = 0;

    ZCANDataObj sendMsgObj = {0};

    sendMsgObj.dataType = ZCAN_DT_ZCAN_CAN_CANFD_DATA;// CAN/CANFD数据
    sendMsgObj.chnl = pTransmit_CSharp.chnl;
    ZCANCANFDData* can_data = &(sendMsgObj.data.zcanCANFDData);
    can_data->frame.can_id = MAKE_CAN_ID(pTransmit_CSharp.canData.can_id, 0, 0, 0); // CAN ID 
    can_data->frame.len = pTransmit_CSharp.frameType ? 64 : 8; // CAN 数据长度 8
    can_data->flag.unionVal.transmitType = 0; // 正常发送
    can_data->flag.unionVal.txEchoRequest = 0; // 设置发送回显
    can_data->flag.unionVal.frameType = pTransmit_CSharp.frameType ? 1 : 0; // CAN or CANFD
    can_data->flag.unionVal.txDelay = ZCAN_TX_DELAY_NO_DELAY;// 直接发送报文到总线

    for (int i = 0; i < can_data->frame.len; ++i) { // 填充 CAN 报文 DATA
        can_data->frame.data[i] = pTransmit_CSharp.canData.data[i];
    }

    //每次调用接口尝试发送一帧数据
    sendMsgSuccNum = ZCAN_TransmitData(device_handle, &sendMsgObj, 1);

    return sendMsgSuccNum;
}

//导入外部DLL中UDS诊断收发相关的接口（DLL来源:项目属性->链接器->输入->附加依赖项）

/**
* @brief UDS诊断请求(总)
* @param[in] device_handle 设备句柄
* @param[in] requestData 请求信息
* @param[out] resp 响应信息, 可为nullptr, 表示不关心响应数据
* @param[out] dataBuf 响应数据缓存区, 存放积极响应的诊断数据(不包含SID), 实际长度为resp.positive.data_len
* @param[in] dataBufSize 响应数据缓存区总大小，如果小于响应诊断数据长度，返回 STATUS_BUFFER_TOO_SMALL
*/
extern __declspec(dllimport) ZCAN_RET_STATUS ZCAN_UDS_RequestEX(DEVICE_HANDLE device_handle, const ZCANUdsRequestDataObj* requestData, ZCAN_UDS_RESPONSE* resp, BYTE* dataBuf, UINT dataBufSize);

//用于C#程序的ZCANUdsRequestDataObj结构体数据
typedef struct ZCANUdsRequestDataObj_CSharp
{
    ZCAN_UDS_DATA_DEF    dataType;              // uint数据类型
    //基本UDS请求数据
    UINT req_id;                            // 请求事务ID，范围0~65535，本次请求的唯一标识
    BYTE channel;                           // 设备通道索引 0~255
    ZCAN_UDS_FRAME_TYPE frame_type;         // byte帧类型
    UINT src_addr;                          // 请求地址
    UINT dst_addr;                          // 响应地址
    BYTE suppress_response;                 // 1:抑制响应
    BYTE sid;                               // 请求服务id

    // 会话层参数
    UINT timeout;                       // 响应超时时间(ms)。因PC定时器误差，建议设置不小于200ms
    UINT enhanced_timeout;              // 收到消极响应错误码为0x78后的超时时间(ms)。因PC定时器误差，建议设置不小于200ms
    BYTE check_any_negative_response : 1; // 接收到非本次请求服务的消极响应时是否需要判定为响应错误
    BYTE wait_if_suppress_response : 1;   // 抑制响应时是否需要等待消极响应，等待时长为响应超时时间

    // 传输层参数
    ZCAN_UDS_TRANS_VER version;         // 传输协议版本, VERSION_0, VERSION_1
    BYTE max_data_len;                  // 单帧最大数据长度, can:8, canfd:64
    BYTE local_st_min;                  // 本程序发送流控时用，连续帧之间的最小间隔, 0x00-0x7F(0ms~127ms), 0xF1-0xF9(100us~900us)
    BYTE block_size;                    // 流控帧的块大小
    BYTE fill_byte;                     // 无效字节的填充数据
    BYTE ext_frame;                     // 0:标准帧 1:扩展帧
    BYTE is_modify_ecu_st_min;          // 是否忽略ECU返回流控的STmin，强制使用本程序设置的 remote_st_min
    BYTE remote_st_min;                 // 发送多帧时用, is_ignore_ecu_st_min = 1 时有效, 0x00-0x7F(0ms~127ms), 0xF1-0xF9(100us~900us)
    UINT fc_timeout;                    // 接收流控超时时间(ms), 如发送首帧后需要等待回应流控帧

    //请求报文数据
    BYTE data[64];                          // 数据数组(不包含SID),默认预留64
    UINT data_len;                          // 数据数组的长度
}ZCANUdsRequestDataObj_CSharp;

//用于C#程序的ZCAN_UDS_RESPONSE结构体数据
typedef struct ZCAN_UDS_RESPONSE_CSharp
{
    ZCAN_UDS_ERROR status;                  // byte响应状态
    ZCAN_UDS_RESPONSE_TYPE type;            // byte响应类型
    //positive正响应
    BYTE pos_sid;                       // 响应服务id
    UINT data_len;                  // 数据长度(不包含SID), 数据存放在接口传入的dataBuf中
    //negative负响应
    BYTE  neg_code;                 // 固定为0x7F
    BYTE  neg_sid;                      // 请求服务id
    BYTE  error_code;               // 错误码
    //正响应报文数据
    BYTE data[64];                          // 数据数组(不包含SID),默认预留64
}ZCAN_UDS_RESPONSE_CSharp;

/// <summary>
/// 上层C#-Winfore程序用于诊断报文收发的接口
/// </summary>
/// <param name="device_handle">设备句柄</param>
/// <param name="requestData">C#发送诊断报文数据的结构</param>
/// <param name="resp">C#获取诊断响应数据的结构</param>
/// <returns>诊断请求状态</returns>
__declspec(dllexport) UINT ZCAN_UDS_RequestEX_Interface(UINT device_handle, ZCANUdsRequestDataObj_CSharp requestData, ZCAN_UDS_RESPONSE_CSharp* resp)
{
    //填充UDS请求数据
    ZCANUdsRequestDataObj udsRequestDataObj = { 0 };

    //基本UDS请求数据
    udsRequestDataObj.dataType = DEF_CAN_UDS_DATA;//目前仅支持can/canfd数据格式
    ZCAN_UDS_REQUEST tmpUdsReq = { 0 };
    tmpUdsReq.req_id = requestData.req_id;                       // 请求事务ID，范围0~65535，本次请求的唯一标识
    tmpUdsReq.channel = requestData.channel;                     // 设备通道索引 0~255
    tmpUdsReq.frame_type = requestData.frame_type;               // byte帧类型
    tmpUdsReq.src_addr = requestData.src_addr;                   // 请求地址
    tmpUdsReq.dst_addr = requestData.dst_addr;                   // 响应地址
    tmpUdsReq.suppress_response = requestData.suppress_response; // 1:抑制响应
    tmpUdsReq.sid = requestData.sid;                             // 请求服务id
    // 会话层参数
    tmpUdsReq.session_param.timeout = requestData.timeout;       // 响应超时时间(ms)。因PC定时器误差，建议设置不小于200ms
    tmpUdsReq.session_param.enhanced_timeout = requestData.enhanced_timeout; // 收到消极响应错误码为0x78后的超时时间(ms)。因PC定时器误差，建议设置不小于200ms
    tmpUdsReq.session_param.check_any_negative_response = 0;     // 接收到非本次请求服务的消极响应时是否需要判定为响应错误
    tmpUdsReq.session_param.wait_if_suppress_response = 0;       // 抑制响应时是否需要等待消极响应，等待时长为响应超时时间
    // 传输层参数
    tmpUdsReq.trans_param.version = ZCAN_UDS_TRANS_VER_1;           // 传输协议版本, VERSION_0, VERSION_1
    tmpUdsReq.trans_param.max_data_len = requestData.max_data_len;  // 单帧最大数据长度, can:8, canfd:64
    tmpUdsReq.trans_param.local_st_min = requestData.local_st_min;  // 本程序发送流控时用，连续帧之间的最小间隔, 0x00-0x7F(0ms~127ms), 0xF1-0xF9(100us~900us)
    tmpUdsReq.trans_param.block_size = requestData.block_size;      // 流控帧的块大小
    tmpUdsReq.trans_param.fill_byte = requestData.fill_byte;        // 无效字节的填充数据
    tmpUdsReq.trans_param.ext_frame = requestData.ext_frame;        // 0:标准帧 1:扩展帧
    tmpUdsReq.trans_param.is_modify_ecu_st_min = 0;                 // 是否忽略ECU返回流控的STmin，强制使用本程序设置的 remote_st_min
    tmpUdsReq.trans_param.remote_st_min = 0;                        // 发送多帧时用, is_ignore_ecu_st_min = 1 时有效, 0x00-0x7F(0ms~127ms), 0xF1-0xF9(100us~900us)
    tmpUdsReq.trans_param.fc_timeout = requestData.fc_timeout;      // 接收流控超时时间(ms), 如发送首帧后需要等待回应流控帧
    //请求报文数据
    tmpUdsReq.data = requestData.data;                             // 数据数组(不包含SID)
    tmpUdsReq.data_len = requestData.data_len;                          // 数据数组的长度
    //UDS请求数据填充完毕
    udsRequestDataObj.data.zcanCANFDUdsData.req = &tmpUdsReq;

    //发送诊断请求
    ZCAN_UDS_RESPONSE udsResponseDataObj = { 0 };
    BYTE responseDataBuf[64];
    ZCAN_RET_STATUS udsReqResult = ZCAN_UDS_RequestEX(device_handle, &udsRequestDataObj, &udsResponseDataObj, &responseDataBuf[0], 64);

    if (udsReqResult == STATUS_OK)
    {
        resp->status = udsResponseDataObj.status;                           // byte响应状态
        resp->type = udsResponseDataObj.type;                               // byte响应类型
        if (resp->type == ZCAN_UDS_RT_POSITIVE)
        {
            //positive正响应
            resp->pos_sid = udsResponseDataObj.positive.sid;                        // 响应服务id
            resp->data_len = udsResponseDataObj.positive.data_len;              // 数据长度(不包含SID), 数据存放在接口传入的dataBuf中

            //响应报文数据
            resp->data[64] = responseDataBuf;                                  // 数据数组(不包含SID),默认预留64
            resp->data_len = udsResponseDataObj.positive.data_len;              // 数据数组的长度
        }
        else
        {
            //negative负响应
            resp->neg_code = udsResponseDataObj.negative.neg_code;             // 固定为0x7F
            resp->neg_sid = udsResponseDataObj.negative.sid;                       // 请求服务id
            resp->error_code = udsResponseDataObj.negative.error_code;         // 错误码
        }
    }
    
    return udsReqResult;
}
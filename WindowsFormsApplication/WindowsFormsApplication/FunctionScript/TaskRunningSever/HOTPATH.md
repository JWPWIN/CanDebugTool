# 通信热路径说明（P0～P2）

本文描述 CanDebugTool 在线通信相关主路径，便于后续改主循环时对照。

## 线程分工

| 线程 | 职责 | 不该做的事 |
|------|------|------------|
| 后台 `LongRunningThreadService` | 设备收发包、待处理接收合并、周期发送计时与发送、周期载荷脏重填（`OnSession1ms`） | 直接改控件、同步 `Invoke` 刷 UI |
| UI 线程（`MainWin` 20ms 泵） | `TakeRecvSnapshot` → 接收区显示 / FSM；状态栏（100ms 节流） | 长时间阻塞、直接调 ZLG 收发 |

## 数据流

```
ZLG 硬件
  → ReceiveMessagesFromDevice（批量，≤64）
  → 设备对象缓冲
  → GetRecvMsgFromDeviceBuf → waitToHandle_RecvCanMsgById（同 ID 留最新，有锁）
  → UI 泵 TakeRecvSnapshot（取出并清空）
  → 更新 _lastRecvFramesById + 可见信号行 + 模型视图 FSM
```

发送：

```
单帧队列 Queue 优先
  → 否则周期表按 sendCycle(µs) 到点取一帧
  → TransmitMessagesToDevice
周期载荷：仅 ValueEdited / 重建矩阵时脏标记，Session_UpdateCycleSendMsgData 重填
```

## 关键约定

- **MsgCycle**：Excel 为 **ms**；写入 `sendCycle` 时 × `TimeUnit.T_MS` 转为 **µs**。
- **MsgCycle == 0**：**不**加入周期发送表。
- **换矩阵**：`ClearSessionRuntimeBuffers` + `RebuildCycleSendMsgListFromDbc`（先清空再建）。
- **关设备**：`canDeviceOpenFlag = false` 并清接收/单帧发送缓冲，避免 UI 泵继续消费。
- **筛选**：隐藏行不刷控件；筛选变更后用 `_lastRecvFramesById` 补刷可见行。

## 相关文件

- `FunctionScript/TaskRunningSever/LongRunningThreadService.cs`
- `FunctionScript/CanDevicesMng/DeviceInterfaceMng.cs` / `ZlgDevice.cs`
- `FunctionScript/Timer/TimerTool.cs`
- `MainWin.cs`（UI 泵）
- `UI/UI_MainPanel/UI_ComUpper.cs`

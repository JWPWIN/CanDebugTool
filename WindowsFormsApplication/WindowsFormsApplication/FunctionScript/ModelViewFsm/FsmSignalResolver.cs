using System.Collections.Generic;

namespace WindowsFormsApplication.ModelViewFsm
{
    public static class FsmSignalResolver
    {
        public static bool TryGetSignal(uint msgId, string sigName, out CanSignal? signal, out CanMessage? message)
        {
            signal = null;
            message = null;
            var mgr = CanDbcDataManager.GetInstance();
            if (!mgr.isLoadCfg || !mgr.canMsgSet.TryGetValue(msgId, out message))
                return false;
            foreach (var s in message.signals)
            {
                if (s.sigName == sigName)
                {
                    signal = s;
                    return true;
                }
            }
            return false;
        }

        public static bool SignalRefExists(FsmSignalRef reference)
            => TryGetSignal(reference.MsgId, reference.SigName, out _, out _);

        public static bool TriggerExists(FsmTriggerCondition trigger)
            => TryGetSignal(trigger.MsgId, trigger.SigName, out _, out _);

        public static List<string> ValidateModel(FsmModel model)
        {
            var issues = new List<string>();
            if (!CanDbcDataManager.GetInstance().isLoadCfg)
            {
                issues.Add("未加载 Excel 通信矩阵，无法校验信号引用。");
                return issues;
            }

            foreach (var state in model.States)
            {
                foreach (var sig in state.DisplaySignals)
                {
                    if (!SignalRefExists(sig))
                        issues.Add($"状态 [{state.DisplayName}] 信号缺失: 0x{sig.MsgId:X} {sig.SigName}");
                }
            }

            foreach (var trans in model.Transitions)
            {
                foreach (var sig in trans.DisplaySignals)
                {
                    if (!SignalRefExists(sig))
                        issues.Add($"转移 [{trans.Label ?? trans.Id}] 展示信号缺失: 0x{sig.MsgId:X} {sig.SigName}");
                }
                foreach (var trig in trans.Triggers)
                {
                    if (!TriggerExists(trig))
                        issues.Add($"转移 [{trans.Label ?? trans.Id}] 触发信号缺失: 0x{trig.MsgId:X} {trig.SigName}");
                }
            }

            return issues;
        }

        public static string FormatSignalValue(CanSignal signal, uint rawValue)
        {
            double physical = System.Math.Round(rawValue * signal.sigFactor + signal.sigOffset, 2);
            if (signal.sigValueTable is { Count: > 0 })
            {
                foreach (var item in signal.sigValueTable)
                {
                    if ((int)physical == item.Key)
                        return item.Value;
                }
            }
            return physical.ToString();
        }

        public static uint ExtractRawFromFrame(CanSignal signal, bool isCanfd, Canfd_Frame_Com frame)
        {
            var sigFormat = signal.sigOrderType == 0 ? CAN_SIG_FORMAT.MOTOROLA_LSB : CAN_SIG_FORMAT.INTEL_STANDARD;
            if (isCanfd)
                return CanBitLibTool.CAN_get_frame_dataFD(frame.data, sigFormat, (ushort)signal.sigStartBit, (ushort)signal.sigLen);
            return CanBitLibTool.CAN_get_frame_data(frame.data, sigFormat, (ushort)signal.sigStartBit, (ushort)signal.sigLen);
        }

        public static bool TriggerMatches(FsmTriggerCondition trigger, uint rawValue, CanSignal signal)
        {
            if (trigger.ExpectedRaw.HasValue)
                return rawValue == (uint)trigger.ExpectedRaw.Value;

            // 与 FormatSignalValue 一致：按显示枚举标签匹配，避免 factor/offset 时 raw==key 失败
            if (!string.IsNullOrEmpty(trigger.ExpectedEnumLabel))
                return FormatSignalValue(signal, rawValue) == trigger.ExpectedEnumLabel;

            if (trigger.ExpectedPhysical.HasValue)
            {
                double physical = rawValue * signal.sigFactor + signal.sigOffset;
                return System.Math.Abs(physical - trigger.ExpectedPhysical.Value) < 0.001;
            }

            return false;
        }
    }
}

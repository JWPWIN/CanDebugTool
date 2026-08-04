using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApplication.ModelViewFsm
{
    public class FsmRuntimeEngine
    {
        public FsmModel Model { get; private set; } = new();
        public FsmRuntimeState Runtime { get; } = new();

        /// <summary>为 false 时仍刷新信号显示缓存，但不做状态转移评估。</summary>
        public bool IsRunning { get; set; } = true;

        /// <param name="resetRuntime">true=重置到初始态；false=尽量保留当前活动态（编辑模型时用）。</param>
        public void SetModel(FsmModel model, bool resetRuntime = true)
        {
            Model = model ?? new FsmModel();
            if (resetRuntime)
                ResetToInitialState();
            else
                ReconcileRuntimeWithModel();
        }

        public void ResetToInitialState()
        {
            var initial = Model.States.FirstOrDefault(s => s.IsInitial)
                ?? Model.States.FirstOrDefault();
            Runtime.ActiveStateId = initial?.Id;
            Runtime.LastFiredTransitionId = null;
        }

        /// <summary>活动状态/已触发转移若已不存在，则回退到安全状态。</summary>
        private void ReconcileRuntimeWithModel()
        {
            if (string.IsNullOrEmpty(Runtime.ActiveStateId)
                || !Model.States.Any(s => s.Id == Runtime.ActiveStateId))
            {
                ResetToInitialState();
                return;
            }

            if (!string.IsNullOrEmpty(Runtime.LastFiredTransitionId)
                && !Model.Transitions.Any(t => t.Id == Runtime.LastFiredTransitionId))
                Runtime.LastFiredTransitionId = null;
        }

        public void UpdateFromRecvFrames(List<Canfd_Frame_Com> recvFrames)
        {
            if (recvFrames is null || recvFrames.Count == 0)
                return;

            var frameById = new Dictionary<uint, Canfd_Frame_Com>();
            foreach (var frame in recvFrames)
            {
                if (frame.data is not null)
                    frameById[frame.can_id] = frame;
            }

            UpdateSignalCaches(frameById);
            if (IsRunning)
                EvaluateTransitions();
        }

        private void UpdateSignalCaches(Dictionary<uint, Canfd_Frame_Com> frameById)
        {
            var allRefs = CollectAllSignalRefs();
            foreach (var reference in allRefs)
            {
                if (!FsmSignalResolver.TryGetSignal(reference.MsgId, reference.SigName, out var signal, out var message))
                    continue;
                if (!frameById.TryGetValue(reference.MsgId, out var frame))
                    continue;

                uint raw = FsmSignalResolver.ExtractRawFromFrame(signal, message.isCanfd, frame);
                Runtime.SignalRawCache[reference.CacheKey] = raw;
                Runtime.SignalDisplayCache[reference.CacheKey] = FsmSignalResolver.FormatSignalValue(signal, raw);
            }
        }

        private IEnumerable<FsmSignalRef> CollectAllSignalRefs()
        {
            foreach (var state in Model.States)
            {
                foreach (var sig in state.DisplaySignals)
                    yield return sig;
            }
            foreach (var trans in Model.Transitions)
            {
                foreach (var sig in trans.DisplaySignals)
                    yield return sig;
                foreach (var trig in trans.Triggers)
                    yield return new FsmSignalRef { MsgId = trig.MsgId, SigName = trig.SigName };
            }
        }

        private void EvaluateTransitions()
        {
            foreach (var transition in Model.Transitions)
            {
                if (transition.Triggers.Count == 0)
                    continue;
                if (!string.IsNullOrEmpty(Runtime.ActiveStateId)
                    && transition.FromStateId != Runtime.ActiveStateId)
                    continue;
                if (!AllTriggersMatch(transition))
                    continue;

                Runtime.LastFiredTransitionId = transition.Id;
                Runtime.ActiveStateId = transition.ToStateId;
                return;
            }
        }

        private bool AllTriggersMatch(FsmTransition transition)
        {
            foreach (var trigger in transition.Triggers)
            {
                if (!FsmSignalResolver.TryGetSignal(trigger.MsgId, trigger.SigName, out var signal, out _))
                    return false;

                var key = $"{trigger.MsgId}:{trigger.SigName}";
                if (!Runtime.SignalRawCache.TryGetValue(key, out uint raw))
                    return false;

                if (!FsmSignalResolver.TriggerMatches(trigger, raw, signal))
                    return false;
            }
            return true;
        }

        public string? GetDisplayValue(FsmSignalRef reference)
        {
            return Runtime.SignalDisplayCache.TryGetValue(reference.CacheKey, out var value) ? value : null;
        }
    }
}

using System;
using System.Collections.Generic;

namespace WindowsFormsApplication.ModelViewFsm
{
    public class FsmModel
    {
        public string SchemaVersion { get; set; } = "1.0";
        public string ModelName { get; set; } = "ECU_FSM";
        public string? TargetEcu { get; set; }
        public List<FsmState> States { get; set; } = new();
        public List<FsmTransition> Transitions { get; set; } = new();
    }

    public class FsmState
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string DisplayName { get; set; } = "State";
        public float X { get; set; }
        public float Y { get; set; }
        public bool IsInitial { get; set; }
        public List<FsmSignalRef> DisplaySignals { get; set; } = new();
    }

    public class FsmTransition
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FromStateId { get; set; } = string.Empty;
        public string ToStateId { get; set; } = string.Empty;
        public string FromAnchor { get; set; } = "Bottom";
        public string ToAnchor { get; set; } = "Top";
        public string? Label { get; set; }
        public List<FsmTriggerCondition> Triggers { get; set; } = new();
        public List<FsmSignalRef> DisplaySignals { get; set; } = new();
    }

    public class FsmSignalRef
    {
        public uint MsgId { get; set; }
        public string SigName { get; set; } = string.Empty;

        public string CacheKey => $"{MsgId}:{SigName}";
    }

    public class FsmTriggerCondition
    {
        public uint MsgId { get; set; }
        public string SigName { get; set; } = string.Empty;
        public long? ExpectedRaw { get; set; }
        public double? ExpectedPhysical { get; set; }
        public string? ExpectedEnumLabel { get; set; }
    }

    public class FsmRuntimeState
    {
        public string? ActiveStateId { get; set; }
        public string? LastFiredTransitionId { get; set; }
        public Dictionary<string, uint> SignalRawCache { get; set; } = new();
        public Dictionary<string, string> SignalDisplayCache { get; set; } = new();
    }

    public static class FsmAnchorNames
    {
        public const string Top = "Top";
        public const string Bottom = "Bottom";
        public const string Left = "Left";
        public const string Right = "Right";

        public static readonly string[] All = { Top, Bottom, Left, Right };
    }
}

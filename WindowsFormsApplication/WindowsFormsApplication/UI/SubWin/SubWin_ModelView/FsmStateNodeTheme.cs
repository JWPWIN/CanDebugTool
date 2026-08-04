using System.Drawing;

namespace WindowsFormsApplication.UI.SubWin.SubWin_ModelView
{
    internal static class FsmStateNodeTheme
    {
        public const int NodeWidth = 248;
        public const int AnchorSize = 18;
        public const int HeaderHeight = 40;
        public const int ColumnHeaderHeight = 26;
        public const int SignalRowHeight = 28;
        public const int MinSignalAreaHeight = 48;
        public const int MaxSignalAreaHeight = 220;

        /// <summary>与 ModelCanvasPanel 背景一致，避免 Transparent 导致拖动尾影。</summary>
        public static readonly Color CanvasBack = Color.FromArgb(245, 245, 250);

        /// <summary>逻辑坐标下的网格间距（屏幕间距 = 逻辑间距 × Zoom）。</summary>
        public const float GridMinorLogical = 20f;
        public const float GridMajorLogical = 100f;
        public static readonly Color GridMinor = Color.FromArgb(228, 230, 238);
        public static readonly Color GridMajor = Color.FromArgb(210, 214, 226);

        public static readonly Color CardBorder = Color.FromArgb(148, 163, 184);
        public static readonly Color CardBack = Color.White;
        public static readonly Color HeaderNormal = Color.FromArgb(59, 130, 180);
        public static readonly Color HeaderInitial = Color.FromArgb(180, 140, 40);
        public static readonly Color HeaderActive = Color.FromArgb(34, 139, 84);
        public static readonly Color HeaderFore = Color.White;
        public static readonly Color ColumnHeaderBack = Color.FromArgb(243, 246, 249);
        public static readonly Color ColumnHeaderFore = Color.FromArgb(75, 85, 99);
        public static readonly Color SigNameFore = Color.FromArgb(31, 41, 55);
        public static readonly Color SigValueFore = Color.FromArgb(30, 64, 120);
        public static readonly Color SigInvalidFore = Color.FromArgb(194, 65, 12);
        public static readonly Color ValueBack = Color.FromArgb(241, 245, 249);
        public static readonly Color RowSeparator = Color.FromArgb(226, 232, 240);
        public static readonly Color EmptyHintFore = Color.FromArgb(156, 163, 175);
        public static readonly Color AnchorBack = Color.FromArgb(226, 232, 240);
        public static readonly Color AnchorFore = Color.FromArgb(71, 85, 105);
    }
}

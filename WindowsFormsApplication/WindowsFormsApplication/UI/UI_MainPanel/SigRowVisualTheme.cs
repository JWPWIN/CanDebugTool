using System.Drawing;

namespace WindowsFormsApplication.UI
{
    /// <summary>收发报文区视觉主题：紧凑行高 + 柔和配色 + 表格单线边框。</summary>
    internal static class SigRowVisualTheme
    {
        public static readonly Color PanelBackColor = Color.White;

        // 报文标题：淡蓝底 + 深色字
        public static readonly Color TitleBackColor = Color.FromArgb(219, 234, 254);
        public static readonly Color TitleForeColor = Color.FromArgb(30, 41, 59);

        // 列头
        public static readonly Color HeaderBackColor = Color.FromArgb(248, 250, 252);
        public static readonly Color HeaderForeColor = Color.FromArgb(100, 116, 139);

        // 信号行交替底色
        public static readonly Color FrameWhite = Color.White;
        public static readonly Color FrameBlue = Color.FromArgb(248, 250, 252);

        // 表格单线边框（信号单元格之间）
        public static readonly Color CellBorder = Color.FromArgb(203, 213, 225);
        public static readonly Color RowSeparator = CellBorder;
        public static readonly Color GroupSeparator = Color.FromArgb(186, 200, 214);
        public static readonly Color ColumnSeparator = CellBorder;

        public static readonly Color SigNameColor = Color.FromArgb(51, 65, 85);
        public static readonly Color SigDescColor = Color.FromArgb(100, 116, 139);
        public static readonly Color SigValueTextColor = Color.FromArgb(51, 65, 85);

        public static readonly Color ValueBackOnWhiteFrame = Color.FromArgb(248, 250, 252);
        public static readonly Color ValueBackOnBlueFrame = Color.White;
    }
}

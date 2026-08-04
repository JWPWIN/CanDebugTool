using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication.UI
{
    internal readonly struct SigRowColumnLayout
    {
        public int SigNameWidth { get; }
        public int SigValueWidth { get; }
        public int SigDescWidth { get; }
        public int TotalWidth => SigNameWidth + 1 + SigValueWidth + 1 + SigDescWidth;

        public SigRowColumnLayout(int sigNameWidth, int sigValueWidth, int sigDescWidth)
        {
            SigNameWidth = sigNameWidth;
            SigValueWidth = sigValueWidth;
            SigDescWidth = sigDescWidth;
        }

        public static SigRowColumnLayout Calculate(IEnumerable<Control> rowControls, Font font)
        {
            const int columnPadding = 12;
            const int minNameWidth = 80;
            const int minValueWidth = 60;
            const int minDescWidth = 100;
            const int maxNameWidth = 280;
            const int maxValueWidth = 220;
            const int maxDescWidth = 420;

            int nameWidth = minNameWidth;
            int valueWidth = minValueWidth;
            int descWidth = minDescWidth;
            int maxTitleWidth = 0;

            // 预估字符宽，避免对每个信号反复 TextRenderer.MeasureText
            float charWidth = Math.Max(6f, TextRenderer.MeasureText("测W8", font).Width / 3f);

            foreach (Control control in rowControls)
            {
                if (control is Label titleLabel && control is not UI_Row_RecvSigDisplay && control is not UI_Row_SendSigDisplay)
                {
                    maxTitleWidth = Math.Max(maxTitleWidth, EstimateWidth(titleLabel.Text, charWidth) + columnPadding);
                    continue;
                }

                if (control is UI_Row_RecvSigDisplay recvRow)
                {
                    nameWidth = Math.Max(nameWidth, EstimateWidth(recvRow.GetSigNameText(), charWidth) + columnPadding);
                    descWidth = Math.Max(descWidth, EstimateWidth(recvRow.GetSigDescText(), charWidth) + columnPadding);
                    valueWidth = Math.Max(valueWidth, EstimateWidth(recvRow.GetMaxValueDisplayText(), charWidth) + columnPadding);
                }
                else if (control is UI_Row_SendSigDisplay sendRow)
                {
                    nameWidth = Math.Max(nameWidth, EstimateWidth(sendRow.GetSigNameText(), charWidth) + columnPadding);
                    descWidth = Math.Max(descWidth, EstimateWidth(sendRow.GetSigDescText(), charWidth) + columnPadding);
                    valueWidth = Math.Max(valueWidth, EstimateWidth(sendRow.GetMaxValueEditorText(), charWidth) + columnPadding);
                }
            }

            nameWidth = Math.Min(Math.Max(nameWidth, minNameWidth), maxNameWidth);
            valueWidth = Math.Min(Math.Max(valueWidth, minValueWidth), maxValueWidth);
            descWidth = Math.Min(Math.Max(descWidth, minDescWidth), maxDescWidth);

            var layout = new SigRowColumnLayout(nameWidth, valueWidth, descWidth);
            int contentWidth = Math.Max(layout.TotalWidth, maxTitleWidth);
            if (contentWidth > layout.TotalWidth)
            {
                layout = new SigRowColumnLayout(
                    layout.SigNameWidth,
                    layout.SigValueWidth,
                    layout.SigDescWidth + (contentWidth - layout.TotalWidth));
            }

            return layout;
        }

        private static int EstimateWidth(string text, float charWidth)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            // CJK 按满宽，ASCII 按半宽近似
            float units = 0f;
            foreach (char c in text)
                units += c > 0x7F ? 1f : 0.55f;
            return (int)Math.Ceiling(units * charWidth) + 8;
        }
    }
}

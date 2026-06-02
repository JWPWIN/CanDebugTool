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

            int nameWidth = minNameWidth;
            int valueWidth = minValueWidth;
            int descWidth = minDescWidth;
            int maxTitleWidth = 0;

            foreach (Control control in rowControls)
            {
                if (control is Label titleLabel)
                {
                    maxTitleWidth = Math.Max(maxTitleWidth, MeasureTextWidth(titleLabel.Text, font) + columnPadding);
                    continue;
                }

                if (control is UI_Row_RecvSigDisplay recvRow)
                {
                    AccumulateRecvWidths(recvRow, font, columnPadding, ref nameWidth, ref valueWidth, ref descWidth);
                }
                else if (control is UI_Row_SendSigDisplay sendRow)
                {
                    AccumulateSendWidths(sendRow, font, columnPadding, ref nameWidth, ref valueWidth, ref descWidth);
                }
            }

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

        private static void AccumulateRecvWidths(
            UI_Row_RecvSigDisplay recvRow,
            Font font,
            int columnPadding,
            ref int nameWidth,
            ref int valueWidth,
            ref int descWidth)
        {
            nameWidth = Math.Max(nameWidth, MeasureTextWidth(recvRow.GetSigNameText(), font) + columnPadding);
            descWidth = Math.Max(descWidth, MeasureTextWidth(recvRow.GetSigDescText(), font) + columnPadding);
            valueWidth = Math.Max(valueWidth, MeasureTextWidth(recvRow.GetMaxValueDisplayText(), font) + columnPadding);
        }

        private static void AccumulateSendWidths(
            UI_Row_SendSigDisplay sendRow,
            Font font,
            int columnPadding,
            ref int nameWidth,
            ref int valueWidth,
            ref int descWidth)
        {
            nameWidth = Math.Max(nameWidth, MeasureTextWidth(sendRow.GetSigNameText(), font) + columnPadding);
            descWidth = Math.Max(descWidth, MeasureTextWidth(sendRow.GetSigDescText(), font) + columnPadding);
            valueWidth = Math.Max(valueWidth, MeasureTextWidth(sendRow.GetMaxValueEditorText(), font) + columnPadding);
        }

        private static int MeasureTextWidth(string text, Font font)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            return TextRenderer.MeasureText(text, font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding | TextFormatFlags.SingleLine).Width;
        }
    }
}

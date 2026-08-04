using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication.UI
{
    public partial class UI_Row_RecvSigDisplay : UserControl
    {
        private int _sigNameWidth = 150;
        private int _sigValueWidth = 60;
        private int _sigDescWidth = 200;

        private readonly Label label_SigName;
        private readonly Label label_SigValue;
        private readonly Label label_SigDesc;
        private readonly Panel separatorNameValue;
        private readonly Panel separatorValueDesc;

        CanSignal canSignalObj = new CanSignal();
        bool isCanfd = false;
        uint curSignalRawValue = 0;

        public UI_Row_RecvSigDisplay()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            Margin = Padding.Empty;
            Height = 24;
            Width = 480;
            BackColor = SigRowVisualTheme.FrameWhite;

            label_SigName = CreateNameLabel();
            label_SigValue = CreateValueLabel();
            label_SigDesc = CreateDescLabel();
            separatorNameValue = CreateVerticalSeparator();
            separatorValueDesc = CreateVerticalSeparator();

            Controls.Add(label_SigName);
            Controls.Add(label_SigValue);
            Controls.Add(label_SigDesc);
            Controls.Add(separatorNameValue);
            Controls.Add(separatorValueDesc);
            LayoutRowControls();
        }

        private static Panel CreateVerticalSeparator()
        {
            return new Panel
            {
                BackColor = SigRowVisualTheme.ColumnSeparator,
                Width = 1
            };
        }

        private static Label CreateNameLabel()
        {
            return new Label
            {
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = false,
                Padding = new Padding(8, 0, 4, 0),
                ForeColor = SigRowVisualTheme.SigNameColor
            };
        }

        private static Label CreateValueLabel()
        {
            return new Label
            {
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = false,
                Padding = new Padding(6, 0, 4, 0),
                ForeColor = SigRowVisualTheme.SigValueTextColor,
                BackColor = SigRowVisualTheme.ValueBackOnWhiteFrame
            };
        }

        private static Label CreateDescLabel()
        {
            return new Label
            {
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = false,
                Padding = new Padding(6, 0, 6, 0),
                ForeColor = SigRowVisualTheme.SigDescColor
            };
        }

        private void LayoutRowControls()
        {
            int rowHeight = Height;
            label_SigName.SetBounds(0, 0, _sigNameWidth, rowHeight);
            label_SigValue.SetBounds(_sigNameWidth, 0, _sigValueWidth, rowHeight);
            label_SigDesc.SetBounds(_sigNameWidth + _sigValueWidth + 2, 0, _sigDescWidth, rowHeight);
            separatorNameValue.SetBounds(_sigNameWidth, 0, 1, rowHeight);
            separatorValueDesc.SetBounds(_sigNameWidth + _sigValueWidth + 1, 0, 1, rowHeight);
            separatorNameValue.BringToFront();
            separatorValueDesc.BringToFront();
        }

        public void ApplyColumnLayout(int sigNameWidth, int sigValueWidth, int sigDescWidth)
        {
            _sigNameWidth = sigNameWidth;
            _sigValueWidth = sigValueWidth;
            _sigDescWidth = sigDescWidth;
            Width = sigNameWidth + sigValueWidth + sigDescWidth + 2;
            RefreshLayout();
        }

        public void ApplyFrameStyle(Color frameBackColor)
        {
            BackColor = frameBackColor;
            label_SigName.BackColor = frameBackColor;
            label_SigDesc.BackColor = frameBackColor;

            bool isAltFrame = frameBackColor == SigRowVisualTheme.FrameBlue;
            label_SigValue.BackColor = isAltFrame
                ? SigRowVisualTheme.ValueBackOnBlueFrame
                : SigRowVisualTheme.ValueBackOnWhiteFrame;
        }

        public string GetSigNameText() => label_SigName.Text;

        public string GetSigDescText() => label_SigDesc.Text;

        public string GetMaxValueDisplayText()
        {
            string widestText = "888888.88";
            if (canSignalObj.sigValueTable is { Count: > 0 })
            {
                foreach (var item in canSignalObj.sigValueTable)
                {
                    string enumText = item.Value?.ToString() ?? string.Empty;
                    if (enumText.Length > widestText.Length)
                        widestText = enumText;
                }
            }
            return widestText;
        }

        private Size _lastLaidOutSize;

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (label_SigName is null || Size == _lastLaidOutSize)
                return;
            _lastLaidOutSize = Size;
            LayoutRowControls();
        }

        public void RefreshLayout()
        {
            _lastLaidOutSize = Size;
            LayoutRowControls();
        }

        private static Font _sharedBoldFont;

        private static Font SharedBoldFont(Control owner)
        {
            _sharedBoldFont ??= new Font(owner.Font, FontStyle.Bold);
            return _sharedBoldFont;
        }

        public void InitSigInfo(CanSignal canSignal, bool isCanfd)
        {
            canSignalObj = canSignal;
            this.isCanfd = isCanfd;
            label_SigName.Text = canSignalObj.sigName;
            label_SigDesc.Text = canSignalObj.sigDesc;
            label_SigValue.Text = string.Empty;
            label_SigName.Font = SharedBoldFont(this);
            RefreshLayout();
        }

        public void UpdateSigValue(Canfd_Frame_Com msgData)
        {
            CAN_SIG_FORMAT sigFormat = (canSignalObj.sigOrderType == 0) ? CAN_SIG_FORMAT.MOTOROLA_LSB : CAN_SIG_FORMAT.INTEL_STANDARD;
            if (isCanfd)
                curSignalRawValue = CanBitLibTool.CAN_get_frame_dataFD(msgData.data, sigFormat, (ushort)canSignalObj.sigStartBit, (ushort)canSignalObj.sigLen);
            else
                curSignalRawValue = CanBitLibTool.CAN_get_frame_data(msgData.data, sigFormat, (ushort)canSignalObj.sigStartBit, (ushort)canSignalObj.sigLen);

            double sigRealPhysicalValue = Math.Round(curSignalRawValue * canSignalObj.sigFactor + canSignalObj.sigOffset, 2);
            string valueStr = sigRealPhysicalValue.ToString();
            if ((canSignalObj.sigValueTable is not null) && (canSignalObj.sigValueTable.Count > 0))
            {
                foreach (var item in canSignalObj.sigValueTable)
                {
                    if ((int)sigRealPhysicalValue == item.Key)
                    {
                        valueStr = item.Value.ToString();
                        break;
                    }
                }
            }

            if (label_SigValue.InvokeRequired)
                label_SigValue.Invoke(new Action(() => label_SigValue.Text = valueStr));
            else
                label_SigValue.Text = valueStr;
        }
    }
}

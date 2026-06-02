using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApplication.FunctionScript.SysControlOverride;

namespace WindowsFormsApplication.UI
{
    public partial class UI_Row_SendSigDisplay : UserControl
    {
        private int _sigNameWidth = 150;
        private int _sigValueWidth = 60;
        private int _sigDescWidth = 200;
        private Color _frameBackColor = SigRowVisualTheme.FrameWhite;

        private readonly Label label_SigName;
        private readonly Label label_SigDesc;
        private readonly Panel separatorNameValue;
        private readonly Panel separatorValueDesc;
        private Control valueEditor;

        CanSignal canSignalObj = new CanSignal();
        bool isCanfd = false;
        string curSignalPhyStr = string.Empty;
        uint curSignalRawValue = 0;

        public UI_Row_SendSigDisplay()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            Margin = Padding.Empty;
            Height = 32;
            Width = 480;
            BackColor = SigRowVisualTheme.FrameWhite;

            label_SigName = CreateNameLabel();
            label_SigDesc = CreateDescLabel();
            separatorNameValue = CreateVerticalSeparator();
            separatorValueDesc = CreateVerticalSeparator();

            Controls.Add(label_SigName);
            Controls.Add(label_SigDesc);
            Controls.Add(separatorNameValue);
            Controls.Add(separatorValueDesc);
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
                Padding = new Padding(10, 0, 6, 0),
                ForeColor = SigRowVisualTheme.SigNameColor
            };
        }

        private static Label CreateDescLabel()
        {
            return new Label
            {
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = false,
                Padding = new Padding(8, 0, 8, 0),
                ForeColor = SigRowVisualTheme.SigDescColor
            };
        }

        private void LayoutRowControls()
        {
            int rowHeight = Height;
            label_SigName.SetBounds(0, 0, _sigNameWidth, rowHeight);
            if (valueEditor is not null)
            {
                valueEditor.SetBounds(_sigNameWidth + 1, 1, _sigValueWidth - 2, rowHeight - 2);
            }
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
            _frameBackColor = frameBackColor;
            BackColor = frameBackColor;
            label_SigName.BackColor = frameBackColor;
            label_SigDesc.BackColor = frameBackColor;
            ApplyValueEditorStyle();
        }

        private void ApplyValueEditorStyle()
        {
            if (valueEditor is null) return;

            bool isBlueFrame = _frameBackColor.R < 250;
            Color editorBack = isBlueFrame
                ? SigRowVisualTheme.ValueBackOnBlueFrame
                : SigRowVisualTheme.ValueBackOnWhiteFrame;

            valueEditor.BackColor = editorBack;
            valueEditor.ForeColor = SigRowVisualTheme.SigValueTextColor;
        }

        public string GetSigNameText() => label_SigName.Text;

        public string GetSigDescText() => label_SigDesc.Text;

        public string GetMaxValueEditorText()
        {
            Font font = label_SigName.Font;
            string widestText = "888888";
            int widestWidth = TextRenderer.MeasureText(widestText, font).Width;

            if ((canSignalObj.sigValueTable is not null) && (canSignalObj.sigValueTable.Count > 0))
            {
                foreach (var item in canSignalObj.sigValueTable)
                {
                    string enumText = item.Key.ToString() + ":" + item.Value;
                    int textWidth = TextRenderer.MeasureText(enumText, font).Width;
                    if (textWidth > widestWidth)
                    {
                        widestWidth = textWidth;
                        widestText = enumText;
                    }
                }
            }

            return widestText;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (label_SigName is not null)
            {
                LayoutRowControls();
            }
        }

        public void RefreshLayout()
        {
            LayoutRowControls();
        }

        public void InitSigInfo(CanSignal canSignal, bool isCanfd)
        {
            canSignalObj = canSignal;
            this.isCanfd = isCanfd;
            label_SigName.Text = canSignalObj.sigName;
            label_SigDesc.Text = canSignalObj.sigDesc;
            label_SigName.Font = new Font(Font, FontStyle.Bold);

            if (valueEditor is not null)
            {
                Controls.Remove(valueEditor);
                valueEditor.Dispose();
                valueEditor = null;
            }

            if ((canSignalObj.sigValueTable is not null) && (canSignalObj.sigValueTable.Count > 0))
            {
                ComboBox sendValueUI_ComboBox = new ComboBox_NoWheel
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    FlatStyle = FlatStyle.Flat,
                    IntegralHeight = false
                };
                foreach (var item in canSignalObj.sigValueTable)
                {
                    sendValueUI_ComboBox.Items.Add(item.Key.ToString() + ":" + item.Value);
                }
                if (sendValueUI_ComboBox.Items.Count > 0)
                    sendValueUI_ComboBox.SelectedIndex = 0;
                sendValueUI_ComboBox.SelectedIndexChanged +=
                    (s, e) => { curSignalPhyStr = (sendValueUI_ComboBox.Text is not null) ? sendValueUI_ComboBox.Text.Split(":")[0] : "0"; };
                valueEditor = sendValueUI_ComboBox;
            }
            else
            {
                TextBox sendValueUI_TextBox = new TextBox
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = "0"
                };
                sendValueUI_TextBox.TextChanged +=
                    (s, e) => { curSignalPhyStr = (sendValueUI_TextBox.Text is not null) ? sendValueUI_TextBox.Text : "0"; };
                valueEditor = sendValueUI_TextBox;
                curSignalPhyStr = "0";
            }

            Controls.Add(valueEditor);
            Controls.Add(label_SigName);
            Controls.Add(label_SigDesc);
            Controls.Add(separatorNameValue);
            Controls.Add(separatorValueDesc);
            ApplyValueEditorStyle();
            RefreshLayout();
        }

        public uint GetSigValue()
        {
            uint physicalValue = 0;
            try
            {
                if (curSignalPhyStr != string.Empty) physicalValue = Convert.ToUInt32(curSignalPhyStr);
            }
            catch (Exception)
            {
                physicalValue = 0;
            }

            curSignalRawValue = (uint)((physicalValue - canSignalObj.sigOffset) / canSignalObj.sigFactor);
            return curSignalRawValue;
        }

        public void SetSigValueToMsg(byte[] msgData)
        {
            uint sendValue = GetSigValue();
            CAN_SIG_FORMAT sigFormat = (canSignalObj.sigOrderType == 0) ? CAN_SIG_FORMAT.MOTOROLA_LSB : CAN_SIG_FORMAT.INTEL_STANDARD;

            if (isCanfd)
                CanBitLibTool.CAN_set_frame_dataFD(msgData, sigFormat, (ushort)canSignalObj.sigStartBit, (ushort)canSignalObj.sigLen, sendValue);
            else
                CanBitLibTool.CAN_set_frame_data(msgData, sigFormat, (ushort)canSignalObj.sigStartBit, (ushort)canSignalObj.sigLen, sendValue);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using WindowsFormsApplication.UI.SubWin.SubWin_DiagView;
namespace WindowsFormsApplication.UI
{
    public partial class UI_ComUpper : UserControl
    {
        //设备管理对象
        DeviceInterfaceMng deviceInterfaceMng;


        //接收报文区域的信号行UI集合<报文ID,接收信号UI列表>
        Dictionary<uint, List<UI_Row_RecvSigDisplay>> recvMsgArea_sigRowUIDict = new Dictionary<uint, List<UI_Row_RecvSigDisplay>>();

        //接收报文区域的报文ID UI集合<报文ID,标签UI>
        Dictionary<uint, Label> recvMsgArea_msgIdTitleUIDict = new Dictionary<uint, Label>();


        //发送报文区域的信号行UI集合<报文ID,发送信号UI列表>
        Dictionary<uint, List<UI_Row_SendSigDisplay>> sendMsgArea_sigRowUIDict = new Dictionary<uint, List<UI_Row_SendSigDisplay>>();

        //发送报文区域的报文ID UI集合<报文ID,标签UI>
        Dictionary<CanMessage, Label> sendMsgArea_msgIdTitleUIDict = new Dictionary<CanMessage, Label>();

        //子窗口:模型视图对象
        SubWin_ModelView subWin_ModelView;

        //子窗口:诊断视图对象
        SubWin_DiagView subWin_DiagView;

        //已构建UI所对应的DBC指纹，避免每次切换页签都重建大量控件
        private int _builtForDbcFingerprint = -1;

        private const float ColumnHeaderHeight = 32F;
        private const float MsgTitleRowHeight = 34F;
        private const float MsgRowHeight = 32F;
        private const int RowSeparatorHeight = 1;
        private const int GroupSeparatorHeight = 2;

        public UI_ComUpper()
        {
            InitializeComponent();
            this.comboBox_CanDeviceType.SelectedIndex = 0;
            this.comboBox_CanType.SelectedIndex = 0;

            //实例化设备管理器对象
            deviceInterfaceMng = new DeviceInterfaceMng();

            EnableDoubleBuffered(tableLayoutPanel_RecvMsgArea);
            EnableDoubleBuffered(tableLayoutPanel_SendMsgArea);
        }

        /// <summary>
        /// DBC未变化时跳过重建，仅在首次进入或重新导入DBC后构建收发区域
        /// </summary>
        public void EnsureMsgAreasInitialized()
        {
            int dbcFingerprint = GetDbcFingerprint();
            if (dbcFingerprint == _builtForDbcFingerprint) return;

            SuspendLayout();
            tableLayoutPanel_RecvMsgArea.SuspendLayout();
            tableLayoutPanel_SendMsgArea.SuspendLayout();
            try
            {
                InitRecvMsgArea();
                InitSendMsgArea();
                InitCycleSendMsgList();
                _builtForDbcFingerprint = dbcFingerprint;
            }
            finally
            {
                tableLayoutPanel_SendMsgArea.ResumeLayout(true);
                tableLayoutPanel_RecvMsgArea.ResumeLayout(true);
                ResumeLayout(true);
            }
        }

        /// <summary>
        /// DBC重新导入后调用，使下次进入通信上位机页时重建UI
        /// </summary>
        public void InvalidateMsgAreas()
        {
            _builtForDbcFingerprint = -1;
        }

        private static int GetDbcFingerprint()
        {
            var dbcManager = CanDbcDataManager.GetInstance();
            if (dbcManager?.isLoadCfg != true || dbcManager.canMsgSet is null) return -1;

            int fingerprint = dbcManager.canMsgSet.Count;
            foreach (var msgId in dbcManager.canMsgSet.Keys)
            {
                fingerprint = unchecked(fingerprint * 31 + (int)msgId);
            }
            return fingerprint;
        }

        /// <summary>
        /// 根据通信协议初始化上位机报文接收窗口
        /// </summary>
        public void InitRecvMsgArea()
        {
            ClearRecvMsgArea();

            //判断是否加载过通协议
            if (CanDbcDataManager.GetInstance().isLoadCfg == false) return;

            var rowControls = new List<Control>();
            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
            {
                if (CanDbcDataManager.IsMsgBelongToTargetEcu(item.transmitter) == false) continue;

                Label tmpIdTitleLabel = CreateMsgTitleLabel(item.msgId, item.msgName, tableLayoutPanel_RecvMsgArea.Font);
                recvMsgArea_msgIdTitleUIDict.Add(item.msgId, tmpIdTitleLabel);
                rowControls.Add(tmpIdTitleLabel);

                List<UI_Row_RecvSigDisplay> tmpList = new List<UI_Row_RecvSigDisplay>();
                foreach (var item1 in item.signals)
                {
                    UI_Row_RecvSigDisplay recvMsg_Row = new UI_Row_RecvSigDisplay();
                    recvMsg_Row.InitSigInfo(item1, item.isCanfd);
                    tmpList.Add(recvMsg_Row);
                    rowControls.Add(recvMsg_Row);
                }
                recvMsgArea_sigRowUIDict.Add(item.msgId, tmpList);
            }

            PopulateMsgAreaPanel(tableLayoutPanel_RecvMsgArea, groupBox1, rowControls);
        }

        /// <summary>
        /// 根据通信协议初始化上位机报文发送窗口
        /// </summary>
        public void InitSendMsgArea()
        {
            ClearSendMsgArea();

            //判断是否加载过通协议
            if (CanDbcDataManager.GetInstance().isLoadCfg == false) return;

            var rowControls = new List<Control>();
            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
            {
                if (CanDbcDataManager.IsMsgBelongToTargetEcu(item.transmitter)) continue;

                Label tmpIdTitleLabel = CreateMsgTitleLabel(item.msgId, item.msgName, tableLayoutPanel_SendMsgArea.Font);
                sendMsgArea_msgIdTitleUIDict.Add(item, tmpIdTitleLabel);
                rowControls.Add(tmpIdTitleLabel);

                List<UI_Row_SendSigDisplay> tmpList = new List<UI_Row_SendSigDisplay>();
                foreach (var item1 in item.signals)
                {
                    UI_Row_SendSigDisplay sendMsg_Row = new UI_Row_SendSigDisplay();
                    sendMsg_Row.InitSigInfo(item1, item.isCanfd);
                    tmpList.Add(sendMsg_Row);
                    rowControls.Add(sendMsg_Row);
                }
                sendMsgArea_sigRowUIDict.Add(item.msgId, tmpList);
            }

            PopulateMsgAreaPanel(tableLayoutPanel_SendMsgArea, groupBox3, rowControls);
        }

        /// <summary>
        /// 初始化上位机周期发送报文列表
        /// </summary>
        public void InitCycleSendMsgList()
        {
            if (CanDbcDataManager.GetInstance().isLoadCfg == false) return;

            //遍历应用报文到设备周期发送列表
            foreach (var item in sendMsgArea_msgIdTitleUIDict)
            {
                DeviceInterfaceMng.GetInstance()?.AddOrDelOneCycleMsgSend(item.Key, 1);
            }
        }

        private void ClearRecvMsgArea()
        {
            recvMsgArea_sigRowUIDict.Clear();
            recvMsgArea_msgIdTitleUIDict.Clear();
            ClearMsgAreaPanel(tableLayoutPanel_RecvMsgArea);
        }

        private void ClearSendMsgArea()
        {
            sendMsgArea_sigRowUIDict.Clear();
            sendMsgArea_msgIdTitleUIDict.Clear();
            ClearMsgAreaPanel(tableLayoutPanel_SendMsgArea);
        }

        private static Label CreateMsgTitleLabel(uint msgId, string msgName, Font font)
        {
            Label titleLabel = new Label();
            titleLabel.AutoEllipsis = false;
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.Text = "  0x" + msgId.ToString("X3").ToUpper() + "  " + msgName;
            titleLabel.BackColor = SigRowVisualTheme.TitleBackColor;
            titleLabel.ForeColor = SigRowVisualTheme.TitleForeColor;
            titleLabel.Font = new Font(font, FontStyle.Bold);
            titleLabel.Padding = new Padding(4, 0, 8, 0);
            return titleLabel;
        }

        private static Panel CreateColumnHeaderPanel(SigRowColumnLayout layout, Font font)
        {
            int height = (int)ColumnHeaderHeight;
            Panel headerPanel = new Panel
            {
                BackColor = SigRowVisualTheme.HeaderBackColor,
                Height = height
            };

            Font headerFont = new Font(font, FontStyle.Bold);
            Label lblName = CreateHeaderCell("信号名", headerFont, 0, layout.SigNameWidth, height);
            Label lblValue = CreateHeaderCell("信号值", headerFont, layout.SigNameWidth + 1, layout.SigValueWidth, height);
            Label lblDesc = CreateHeaderCell("信号描述", headerFont, layout.SigNameWidth + layout.SigValueWidth + 2, layout.SigDescWidth, height);
            Panel sep1 = CreateVerticalSeparatorPanel(layout.SigNameWidth, height);
            Panel sep2 = CreateVerticalSeparatorPanel(layout.SigNameWidth + layout.SigValueWidth + 1, height);

            headerPanel.Controls.Add(lblName);
            headerPanel.Controls.Add(lblValue);
            headerPanel.Controls.Add(lblDesc);
            headerPanel.Controls.Add(sep1);
            headerPanel.Controls.Add(sep2);
            sep1.BringToFront();
            sep2.BringToFront();
            return headerPanel;
        }

        private static Label CreateHeaderCell(string text, Font font, int x, int width, int height)
        {
            return new Label
            {
                Text = text,
                Font = font,
                ForeColor = SigRowVisualTheme.HeaderForeColor,
                BackColor = SigRowVisualTheme.HeaderBackColor,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 6, 0),
                Location = new Point(x, 0),
                Size = new Size(width, height)
            };
        }

        private static Panel CreateVerticalSeparatorPanel(int x, int height)
        {
            return new Panel
            {
                BackColor = SigRowVisualTheme.ColumnSeparator,
                Location = new Point(x, 0),
                Size = new Size(1, height)
            };
        }

        private static void ClearMsgAreaPanel(Panel panel)
        {
            foreach (Control control in panel.Controls)
            {
                control.Dispose();
            }
            panel.Controls.Clear();
            panel.AutoScrollMinSize = Size.Empty;
            panel.Tag = null;
        }

        private static void ApplyColumnLayoutToRow(Control rowControl, SigRowColumnLayout layout)
        {
            if (rowControl is UI_Row_RecvSigDisplay recvRow)
            {
                recvRow.ApplyColumnLayout(layout.SigNameWidth, layout.SigValueWidth, layout.SigDescWidth);
            }
            else if (rowControl is UI_Row_SendSigDisplay sendRow)
            {
                sendRow.ApplyColumnLayout(layout.SigNameWidth, layout.SigValueWidth, layout.SigDescWidth);
            }
        }

        private static void ApplyFrameStyleToRow(Control rowControl, Color frameBackColor)
        {
            if (rowControl is UI_Row_RecvSigDisplay recvRow)
                recvRow.ApplyFrameStyle(frameBackColor);
            else if (rowControl is UI_Row_SendSigDisplay sendRow)
                sendRow.ApplyFrameStyle(frameBackColor);
        }

        private static bool IsMsgTitleLabel(Control control)
        {
            return control is Label && control is not UI_Row_RecvSigDisplay && control is not UI_Row_SendSigDisplay
                && control.Tag as string != "ColumnHeader";
        }

        private static void ApplyGroupBoxContentWidth(GroupBox groupBox, int contentWidth)
        {
            int chromePadding = groupBox.Padding.Left + groupBox.Padding.Right + 10;
            groupBox.Width = Math.Max(contentWidth + chromePadding, 220);
        }

        private static void PopulateMsgAreaPanel(Panel panel, GroupBox groupBox, List<Control> rowControls)
        {
            panel.SuspendLayout();
            groupBox.SuspendLayout();
            try
            {
                ClearMsgAreaPanel(panel);
                if (rowControls.Count == 0)
                {
                    ApplyGroupBoxContentWidth(groupBox, 220);
                    return;
                }

                SigRowColumnLayout columnLayout = SigRowColumnLayout.Calculate(rowControls, panel.Font);
                panel.Tag = columnLayout;

                int rowWidth = columnLayout.TotalWidth;
                int titleHeight = (int)MsgTitleRowHeight;
                int signalHeight = (int)MsgRowHeight;
                int y = 0;

                Panel columnHeader = CreateColumnHeaderPanel(columnLayout, panel.Font);
                columnHeader.Location = new Point(0, y);
                columnHeader.Size = new Size(rowWidth, (int)ColumnHeaderHeight);
                columnHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                columnHeader.Tag = "ColumnHeader";
                panel.Controls.Add(columnHeader);
                y += (int)ColumnHeaderHeight;

                Panel headerSeparator = CreateHorizontalRowSeparator(SigRowVisualTheme.GroupSeparator);
                headerSeparator.Location = new Point(0, y);
                headerSeparator.Size = new Size(rowWidth, GroupSeparatorHeight);
                headerSeparator.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                panel.Controls.Add(headerSeparator);
                y += GroupSeparatorHeight;

                int frameColorIndex = 0;
                bool firstMessage = true;
                for (int i = 0; i < rowControls.Count; i++)
                {
                    Control rowControl = rowControls[i];
                    bool isTitle = IsMsgTitleLabel(rowControl);
                    if (isTitle)
                    {
                        if (!firstMessage)
                            frameColorIndex = 1 - frameColorIndex;
                        firstMessage = false;
                    }

                    ApplyColumnLayoutToRow(rowControl, columnLayout);
                    int currentHeight = isTitle ? titleHeight : signalHeight;
                    rowControl.Location = new Point(0, y);
                    rowControl.Size = new Size(rowWidth, currentHeight);
                    rowControl.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    panel.Controls.Add(rowControl);
                    y += currentHeight;

                    if (!isTitle)
                        ApplyFrameStyleToRow(rowControl, GetFrameBackColor(frameColorIndex));

                    bool nextIsTitle = i < rowControls.Count - 1 && IsMsgTitleLabel(rowControls[i + 1]);
                    bool isLastRow = i == rowControls.Count - 1;
                    if (isLastRow) continue;

                    Color separatorColor = nextIsTitle ? SigRowVisualTheme.GroupSeparator : SigRowVisualTheme.RowSeparator;
                    int separatorHeight = nextIsTitle ? GroupSeparatorHeight : RowSeparatorHeight;
                    Panel rowSeparator = CreateHorizontalRowSeparator(separatorColor);
                    rowSeparator.Location = new Point(0, y);
                    rowSeparator.Size = new Size(rowWidth, separatorHeight);
                    rowSeparator.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    panel.Controls.Add(rowSeparator);
                    y += separatorHeight;
                }

                panel.AutoScrollMinSize = new Size(rowWidth, y + 4);
                ApplyGroupBoxContentWidth(groupBox, rowWidth);
            }
            finally
            {
                groupBox.ResumeLayout(true);
                panel.ResumeLayout(true);
            }
        }

        private static Color GetFrameBackColor(int colorIndex)
            => colorIndex == 0 ? SigRowVisualTheme.FrameWhite : SigRowVisualTheme.FrameBlue;

        private static Panel CreateHorizontalRowSeparator(Color backColor)
        {
            return new Panel
            {
                BackColor = backColor,
                Tag = "RowSeparator"
            };
        }

        private static bool IsHorizontalRowSeparator(Control control)
        {
            return control is Panel && "RowSeparator".Equals(control.Tag);
        }

        private static void EnableDoubleBuffered(Control control)
        {
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)?
                .SetValue(control, true, null);
        }

        /// <summary>
        /// 根据上位机发送报文设置值填充设备周期报文发送报文数据
        /// </summary>
        public void MainLoopThread_Task_UpdateCycleSendMsgData()
        {
            //获取设备周期发送报文列表
            if (DeviceInterfaceMng.GetInstance() is null) return;//无设备退出
            Dictionary<uint, CycleSend_Canfd_Frame> cycSendMsgList = DeviceInterfaceMng.GetInstance().GetCycleMsgSendDict();
            if (cycSendMsgList.Count == 0) return;//未获取到周期报文退出

            //遍历设备周期发送报文列表 从上位机获取设置数值 填充对应的发送数据
            foreach (var item in cycSendMsgList)
            {
                CycleSend_Canfd_Frame sendData = item.Value;
                byte[] tmpMsgData = new byte[64];

                //从发送UI获取信号设置值填充到临时报文帧数据
                foreach (var item1 in sendMsgArea_sigRowUIDict)
                {
                    if (item1.Key == item.Key)
                    {
                        foreach (var item2 in item1.Value)
                        {
                            item2.SetSigValueToMsg(tmpMsgData);
                        }
                        break;
                    }
                }
                //临时报文帧数据复制到周期发送报文缓存区
                sendData.msgData.data = tmpMsgData;
                cycSendMsgList[item.Key] = sendData;
            }
        }

        /// <summary>
        /// 从设备接口对象中获取已接收待处理的报文，显示在接收报文区域
        /// </summary>
        public void MainLoopThread_Task_UpdateRecvMsgArea()
        {
            //获取接收报文数据
            List<Canfd_Frame_Com> _recvMsgList = DeviceInterfaceMng.GetInstance().GetCurWaitToHandleRecvMsg();

            //实时更新上位机中显示接收数据
            foreach (var item in recvMsgArea_sigRowUIDict)
            {
                //获取对应ID的报文
                Canfd_Frame_Com _tmpMsg = new Canfd_Frame_Com();
                foreach (var item1 in _recvMsgList)
                {
                    if (item.Key == item1.can_id)
                    {
                        _tmpMsg = item1;

                        break;
                    }
                }

                //无对应ID的数据，跳过
                if (_tmpMsg.data is null) continue;

                //从报文中获取信号当前值 更新UI
                foreach (var item1 in item.Value)
                {
                    item1.UpdateSigValue(_tmpMsg);
                }
            }

            //清除待处理的接收报文数据
            DeviceInterfaceMng.GetInstance().ClearCurWaitToHandleRecvMsg();
        }

        private void Btn_ConnectDevice_Click(object sender, EventArgs e)
        {
            //打开设备
            if (deviceInterfaceMng is not null)
                deviceInterfaceMng.OpenCanDevice(this.comboBox_CanDeviceType.SelectedIndex, this.comboBox_CanType.SelectedIndex);
        }

        private void Btn_DisconnectDevice_Click(object sender, EventArgs e)
        {
            //关闭已经打开的设备
            if (deviceInterfaceMng is not null)
                deviceInterfaceMng.CloseCanDevice();
        }

        private void Btn_ModelView_Click(object sender, EventArgs e)
        {
            subWin_ModelView = new SubWin_ModelView();
            subWin_ModelView.Show();
        }

        private void Btn_DiagView_Click(object sender, EventArgs e)
        {
            subWin_DiagView = new SubWin_DiagView();
            subWin_DiagView.Show();
        }
    }
}

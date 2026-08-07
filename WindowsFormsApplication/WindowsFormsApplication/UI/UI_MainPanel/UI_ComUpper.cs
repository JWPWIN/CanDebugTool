using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsFormsApplication.FunctionScript.SysControlOverride;
using WindowsFormsApplication.UI.SubWin.SubWin_DiagView;
using WindowsFormsApplication.UI.SubWin.SubWin_ModelView;
namespace WindowsFormsApplication.UI
{
    public partial class UI_ComUpper : UserControl
    {
        private const int WM_SETREDRAW = 0x000B;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

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
        private Font _msgTitleBoldFont;

        // 冻结表头（置于滚动区外，仅同步水平滚动）
        private Panel _recvFrozenHeaderInner;
        private Panel _sendFrozenHeaderInner;
        private Panel _recvFilterBand;
        private Panel _sendFilterBand;
        private TextBox_NoWheel _recvFilterBox;
        private TextBox_NoWheel _sendFilterBox;
        private Label _recvAreaStateHint;
        private Label _sendAreaStateHint;
        private readonly List<Control> _recvAllRowControls = new List<Control>();
        private readonly List<Control> _sendAllRowControls = new List<Control>();
        private Timer _recvFilterDebounceTimer;
        private Timer _sendFilterDebounceTimer;
        private bool _matrixLoading;

        /// <summary>周期发送载荷需从 UI 重填。</summary>
        private volatile bool _cycleSendDirty = true;

        /// <summary>最近一次收到的帧（按 ID），供筛选切换后补刷可见行。</summary>
        private readonly Dictionary<uint, Canfd_Frame_Com> _lastRecvFramesById = new Dictionary<uint, Canfd_Frame_Com>();

        private const float ColumnHeaderHeight = 26F;
        private const float MsgTitleRowHeight = 26F;
        private const float MsgRowHeight = 24F;
        private const int RowSeparatorHeight = 1;
        private const int GroupSeparatorHeight = 1;
        private const int FilterBandHeight = 30;
        private const int FilterDebounceMs = 150;

        /// <summary>报文标题行筛选元数据。</summary>
        private sealed class MsgRowMeta
        {
            public uint MsgId;
            public string MsgName;
            public string SearchBlob;
        }

        public UI_ComUpper()
        {
            InitializeComponent();
            this.comboBox_CanDeviceType.SelectedIndex = 0;
            this.comboBox_CanType.SelectedIndex = 0;

            //实例化设备管理器对象
            deviceInterfaceMng = new DeviceInterfaceMng();

            EnableDoubleBuffered(this);
            EnableDoubleBuffered(groupBox1);
            EnableDoubleBuffered(groupBox3);
            EnableDoubleBuffered(tableLayoutPanel_RecvMsgArea);
            EnableDoubleBuffered(tableLayoutPanel_SendMsgArea);
            // 报文标题比正文大一号
            _msgTitleBoldFont = new Font(Font.FontFamily, Font.Size + 1f, FontStyle.Bold);

            BuildFrozenHeaderHosts();
            UpdateAreaStateHints();
        }

        /// <summary>
        /// 将表头与筛选框移出 AutoScroll 面板：垂直滚动时固定，水平滚动时表头与内容同步。
        /// </summary>
        private void BuildFrozenHeaderHosts()
        {
            WrapMsgAreaWithFrozenHeader(groupBox1, tableLayoutPanel_RecvMsgArea,
                out _recvFrozenHeaderInner, out _recvFilterBand, out _recvFilterBox, out _recvAreaStateHint);
            WrapMsgAreaWithFrozenHeader(groupBox3, tableLayoutPanel_SendMsgArea,
                out _sendFrozenHeaderInner, out _sendFilterBand, out _sendFilterBox, out _sendAreaStateHint);

            WireHeaderHScroll(tableLayoutPanel_RecvMsgArea, _recvFrozenHeaderInner, _recvFilterBand);
            WireHeaderHScroll(tableLayoutPanel_SendMsgArea, _sendFrozenHeaderInner, _sendFilterBand);

            _recvFilterDebounceTimer = CreateFilterDebounceTimer(() => ApplyMsgAreaFilter(isRecv: true));
            _sendFilterDebounceTimer = CreateFilterDebounceTimer(() => ApplyMsgAreaFilter(isRecv: false));
            _recvFilterBox.TextChanged += (_, _) =>
            {
                _recvFilterDebounceTimer.Stop();
                _recvFilterDebounceTimer.Start();
            };
            _sendFilterBox.TextChanged += (_, _) =>
            {
                _sendFilterDebounceTimer.Stop();
                _sendFilterDebounceTimer.Start();
            };
        }

        private static Timer CreateFilterDebounceTimer(Action applyFilter)
        {
            var timer = new Timer { Interval = FilterDebounceMs };
            timer.Tick += (_, _) =>
            {
                timer.Stop();
                applyFilter();
            };
            return timer;
        }

        private static void WrapMsgAreaWithFrozenHeader(
            GroupBox groupBox,
            Panel scrollPanel,
            out Panel headerInner,
            out Panel filterBand,
            out TextBox_NoWheel filterBox,
            out Label stateHint)
        {
            groupBox.Controls.Remove(scrollPanel);

            var host = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Name = scrollPanel.Name + "_Host"
            };
            EnableDoubleBuffered(host);

            int headerBandHeight = (int)ColumnHeaderHeight + GroupSeparatorHeight;
            var headerClip = new Panel
            {
                Dock = DockStyle.Top,
                Height = headerBandHeight,
                BackColor = SigRowVisualTheme.HeaderBackColor,
                Name = scrollPanel.Name + "_HeaderClip"
            };
            EnableDoubleBuffered(headerClip);

            headerInner = new Panel
            {
                Location = Point.Empty,
                Size = new Size(100, headerBandHeight),
                BackColor = SigRowVisualTheme.HeaderBackColor,
                Name = scrollPanel.Name + "_HeaderInner"
            };
            EnableDoubleBuffered(headerInner);
            headerClip.Controls.Add(headerInner);

            filterBand = new Panel
            {
                Dock = DockStyle.Top,
                Height = FilterBandHeight,
                BackColor = SigRowVisualTheme.HeaderBackColor,
                Padding = new Padding(6, 3, 6, 3),
                Name = scrollPanel.Name + "_FilterBand"
            };
            EnableDoubleBuffered(filterBand);

            filterBox = new TextBox_NoWheel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "筛选：报文ID / 报文名 / 信号名",
                Name = scrollPanel.Name + "_FilterBox"
            };
            filterBand.Controls.Add(filterBox);

            stateHint = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(100, 116, 139),
                BackColor = Color.White,
                Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                Visible = false,
                Name = scrollPanel.Name + "_StateHint"
            };

            scrollPanel.Dock = DockStyle.Fill;
            // Dock：先加 Fill，再加筛选带，最后加表头 → 自上而下为表头、筛选、内容
            host.Controls.Add(scrollPanel);
            host.Controls.Add(filterBand);
            host.Controls.Add(headerClip);
            host.Controls.Add(stateHint); // 最上层，空态/加载时盖住内容区
            groupBox.Controls.Add(host);
        }

        /// <summary>矩阵异步加载中/结束时由 MainWin 调用，刷新通信区空态提示。</summary>
        public void SetMatrixLoading(bool loading)
        {
            _matrixLoading = loading;
            UpdateAreaStateHints();
        }

        private void UpdateAreaStateHints()
        {
            UpdateOneAreaStateHint(
                _recvAreaStateHint,
                _recvAllRowControls.Count,
                CountVisibleDataRows(tableLayoutPanel_RecvMsgArea),
                _recvFilterBox?.Text,
                isRecv: true);
            UpdateOneAreaStateHint(
                _sendAreaStateHint,
                _sendAllRowControls.Count,
                CountVisibleDataRows(tableLayoutPanel_SendMsgArea),
                _sendFilterBox?.Text,
                isRecv: false);
        }

        private void UpdateOneAreaStateHint(
            Label hint,
            int allRowCount,
            int visibleDataRowCount,
            string filterText,
            bool isRecv)
        {
            if (hint is null) return;

            if (_matrixLoading)
            {
                hint.Text = "正在加载 CAN 矩阵…";
                hint.Visible = true;
                hint.BringToFront();
                return;
            }

            bool loaded = CanDbcDataManager.GetInstance()?.isLoadCfg == true;
            if (!loaded)
            {
                hint.Text = "未加载通信矩阵\n请点击左上角文件夹按钮导入 Excel";
                hint.Visible = true;
                hint.BringToFront();
                return;
            }

            if (visibleDataRowCount > 0)
            {
                hint.Visible = false;
                return;
            }

            bool filtering = !string.IsNullOrWhiteSpace(filterText);
            if (filtering)
                hint.Text = isRecv ? "无匹配的接收报文/信号" : "无匹配的发送报文/信号";
            else if (allRowCount == 0)
                hint.Text = isRecv ? "当前矩阵无接收报文" : "当前矩阵无发送报文";
            else
                hint.Text = isRecv ? "无匹配的接收报文/信号" : "无匹配的发送报文/信号";

            hint.Visible = true;
            hint.BringToFront();
        }

        private static int CountVisibleDataRows(Panel scrollPanel)
        {
            int count = 0;
            foreach (Control c in scrollPanel.Controls)
            {
                if (c is UI_Row_RecvSigDisplay or UI_Row_SendSigDisplay)
                    count++;
                else if (c is Label lbl && lbl.Tag is MsgRowMeta)
                    count++;
            }
            return count;
        }

        private static void WireHeaderHScroll(Panel scrollPanel, Panel headerInner, Panel filterBand)
        {
            void Sync() => SyncFrozenHeaderScroll(scrollPanel, headerInner, filterBand);

            scrollPanel.Scroll += (_, _) => Sync();
            // 滚轮滚动时部分情况下 Scroll 事件不可靠，布局后再同步一次
            scrollPanel.MouseWheel += (_, _) =>
            {
                if (scrollPanel.IsHandleCreated)
                    scrollPanel.BeginInvoke((Action)Sync);
            };
        }

        /// <summary>
        /// 同步冻结表头水平位移，并在出现纵向滚动条时预留右侧宽度，避免列错位。
        /// </summary>
        private static void SyncFrozenHeaderScroll(Panel scrollPanel, Panel headerInner, Panel filterBand = null)
        {
            int vScrollWidth = scrollPanel.VerticalScroll.Visible
                ? SystemInformation.VerticalScrollBarWidth
                : 0;

            if (headerInner.Parent is Panel headerClip && headerClip.Padding.Right != vScrollWidth)
                headerClip.Padding = new Padding(0, 0, vScrollWidth, 0);

            if (filterBand is not null)
            {
                var pad = filterBand.Padding;
                if (pad.Right != 6 + vScrollWidth)
                    filterBand.Padding = new Padding(6, 3, 6 + vScrollWidth, 3);
            }

            int x = scrollPanel.AutoScrollPosition.X;
            if (headerInner.Left != x)
                headerInner.Left = x;
        }

        /// <summary>收发信号行总数是否超过阈值（用于主窗口拖动性能策略）。</summary>
        public bool IsHeavyMsgUi(int rowThreshold)
        {
            int n = 0;
            foreach (var kv in recvMsgArea_sigRowUIDict)
                n += kv.Value?.Count ?? 0;
            if (n > rowThreshold)
                return true;
            foreach (var kv in sendMsgArea_sigRowUIDict)
                n += kv.Value?.Count ?? 0;
            return n > rowThreshold;
        }

        /// <summary>主窗口拖动缩放过程中调用：暂停收发区布局计算（画面由上层快照覆盖显示）。</summary>
        public void BeginLiveResize()
        {
            SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel_RecvMsgArea.SuspendLayout();
            tableLayoutPanel_SendMsgArea.SuspendLayout();
        }

        /// <summary>主窗口缩放结束后调用：恢复布局，配合主窗口移除快照后整体刷新。</summary>
        public void EndLiveResize()
        {
            tableLayoutPanel_SendMsgArea.ResumeLayout(false);
            tableLayoutPanel_RecvMsgArea.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>
        /// DBC未变化时跳过重建，仅在首次进入或重新导入DBC后构建收发区域
        /// </summary>
        public void EnsureMsgAreasInitialized()
        {
            int dbcFingerprint = GetDbcFingerprint();
            if (dbcFingerprint == _builtForDbcFingerprint) return;

            bool oldUseWaitCursor = UseWaitCursor;
            UseWaitCursor = true;
            SuspendLayout();
            tableLayoutPanel_RecvMsgArea.SuspendLayout();
            tableLayoutPanel_SendMsgArea.SuspendLayout();
            SetRedraw(tableLayoutPanel_RecvMsgArea, false);
            SetRedraw(tableLayoutPanel_SendMsgArea, false);
            try
            {
                InitRecvMsgArea();
                InitSendMsgArea();
                InitCycleSendMsgList();
                _builtForDbcFingerprint = dbcFingerprint;
            }
            finally
            {
                SetRedraw(tableLayoutPanel_SendMsgArea, true);
                SetRedraw(tableLayoutPanel_RecvMsgArea, true);
                tableLayoutPanel_SendMsgArea.ResumeLayout(false);
                tableLayoutPanel_RecvMsgArea.ResumeLayout(false);
                ResumeLayout(false);
                tableLayoutPanel_RecvMsgArea.PerformLayout();
                tableLayoutPanel_SendMsgArea.PerformLayout();
                PerformLayout();
                tableLayoutPanel_RecvMsgArea.Invalidate(true);
                tableLayoutPanel_SendMsgArea.Invalidate(true);
                UseWaitCursor = oldUseWaitCursor;
            }
        }

        /// <summary>
        /// DBC重新导入后调用，使下次进入通信上位机页时重建UI
        /// </summary>
        public void InvalidateMsgAreas()
        {
            _builtForDbcFingerprint = -1;
            _lastRecvFramesById.Clear();
        }

        /// <summary>
        /// 按当前 DBC 重建周期发送表（先清空再建）。换矩阵后务必调用，不依赖收发区 UI 是否已构建。
        /// </summary>
        public void RebuildCycleSendMsgListFromDbc()
        {
            var device = DeviceInterfaceMng.GetInstance();
            if (device is null) return;

            device.ClearCycleMsgSendDict();

            if (CanDbcDataManager.GetInstance()?.isLoadCfg != true)
            {
                _cycleSendDirty = true;
                return;
            }

            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
            {
                // 与发送区一致：目标 ECU 发送的报文不作为上位机周期发送
                if (CanDbcDataManager.IsMsgBelongToTargetEcu(item.transmitter))
                    continue;
                device.AddOrDelOneCycleMsgSend(item, 1);
            }

            _cycleSendDirty = true;
        }

        /// <summary>
        /// 初始化上位机周期发送报文列表
        /// </summary>
        public void InitCycleSendMsgList()
        {
            RebuildCycleSendMsgListFromDbc();
        }

        private static void SetRedraw(Control control, bool enable)
        {
            if (!control.IsHandleCreated) return;
            SendMessage(control.Handle, WM_SETREDRAW, enable ? new IntPtr(1) : IntPtr.Zero, IntPtr.Zero);
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
            DisposeOwnedRowControls(_recvAllRowControls);
            recvMsgArea_sigRowUIDict.Clear();
            recvMsgArea_msgIdTitleUIDict.Clear();

            //判断是否加载过通协议
            if (CanDbcDataManager.GetInstance().isLoadCfg == false)
            {
                PopulateMsgAreaPanel(tableLayoutPanel_RecvMsgArea, groupBox1, _recvAllRowControls,
                    _recvFrozenHeaderInner, _recvFilterBand, disposeRowControls: true);
                UpdateAreaStateHints();
                return;
            }

            var rowControls = new List<Control>();
            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
            {
                if (CanDbcDataManager.IsMsgBelongToTargetEcu(item.transmitter) == false) continue;

                Label tmpIdTitleLabel = CreateMsgTitleLabel(item.msgId, item.msgName);
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

            _recvAllRowControls.AddRange(rowControls);
            ApplyMsgAreaFilter(isRecv: true, disposeRowControls: true);
            UpdateAreaStateHints();
        }

        /// <summary>
        /// 根据通信协议初始化上位机报文发送窗口
        /// </summary>
        public void InitSendMsgArea()
        {
            DisposeOwnedRowControls(_sendAllRowControls);
            sendMsgArea_sigRowUIDict.Clear();
            sendMsgArea_msgIdTitleUIDict.Clear();

            //判断是否加载过通协议
            if (CanDbcDataManager.GetInstance().isLoadCfg == false)
            {
                PopulateMsgAreaPanel(tableLayoutPanel_SendMsgArea, groupBox3, _sendAllRowControls,
                    _sendFrozenHeaderInner, _sendFilterBand, disposeRowControls: true);
                UpdateAreaStateHints();
                return;
            }

            var rowControls = new List<Control>();
            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
            {
                if (CanDbcDataManager.IsMsgBelongToTargetEcu(item.transmitter)) continue;

                Label tmpIdTitleLabel = CreateMsgTitleLabel(item.msgId, item.msgName, item.msgCycle, showNonCycleHint: true);
                sendMsgArea_msgIdTitleUIDict.Add(item, tmpIdTitleLabel);
                rowControls.Add(tmpIdTitleLabel);

                List<UI_Row_SendSigDisplay> tmpList = new List<UI_Row_SendSigDisplay>();
                foreach (var item1 in item.signals)
                {
                    UI_Row_SendSigDisplay sendMsg_Row = new UI_Row_SendSigDisplay();
                    sendMsg_Row.InitSigInfo(item1, item.isCanfd);
                    sendMsg_Row.ValueEdited += MarkCycleSendDirty;
                    tmpList.Add(sendMsg_Row);
                    rowControls.Add(sendMsg_Row);
                }
                sendMsgArea_sigRowUIDict.Add(item.msgId, tmpList);
            }

            _sendAllRowControls.AddRange(rowControls);
            _cycleSendDirty = true;
            ApplyMsgAreaFilter(isRecv: false, disposeRowControls: true);
            UpdateAreaStateHints();
        }

        private void MarkCycleSendDirty() => _cycleSendDirty = true;

        private void ApplyMsgAreaFilter(bool isRecv, bool disposeRowControls = false)
        {
            if (isRecv)
            {
                var visible = FilterMsgAreaRows(_recvAllRowControls, _recvFilterBox?.Text);
                PopulateMsgAreaPanel(tableLayoutPanel_RecvMsgArea, groupBox1, visible,
                    _recvFrozenHeaderInner, _recvFilterBand, _recvAllRowControls, disposeRowControls);
                RefreshVisibleRecvFromLastFrames();
            }
            else
            {
                var visible = FilterMsgAreaRows(_sendAllRowControls, _sendFilterBox?.Text);
                PopulateMsgAreaPanel(tableLayoutPanel_SendMsgArea, groupBox3, visible,
                    _sendFrozenHeaderInner, _sendFilterBand, _sendAllRowControls, disposeRowControls);
            }

            UpdateAreaStateHints();
        }

        /// <summary>筛选变更后，用最近接收帧补刷当前可见信号行。</summary>
        private void RefreshVisibleRecvFromLastFrames()
        {
            if (_lastRecvFramesById.Count == 0 || recvMsgArea_sigRowUIDict.Count == 0)
                return;

            foreach (var kv in recvMsgArea_sigRowUIDict)
            {
                if (!_lastRecvFramesById.TryGetValue(kv.Key, out Canfd_Frame_Com frame))
                    continue;
                if (frame.data is null)
                    continue;

                List<UI_Row_RecvSigDisplay> rows = kv.Value;
                for (int i = 0; i < rows.Count; i++)
                {
                    UI_Row_RecvSigDisplay row = rows[i];
                    if (row.Parent is null || !row.Visible)
                        continue;
                    row.ApplyDisplayValue(row.ComputeDisplayValue(frame));
                }
            }
        }

        /// <summary>
        /// 按报文ID / 报文名 / 信号名局部匹配筛选；报文命中则显示其全部信号，否则仅显示命中信号（保留报文标题）。
        /// </summary>
        private static List<Control> FilterMsgAreaRows(List<Control> allRows, string filterText)
        {
            if (allRows is null || allRows.Count == 0)
                return new List<Control>();

            string key = filterText?.Trim() ?? string.Empty;
            if (key.Length == 0)
                return new List<Control>(allRows);

            key = key.ToLowerInvariant();
            var result = new List<Control>();
            int i = 0;
            while (i < allRows.Count)
            {
                if (!IsMsgTitleLabel(allRows[i]))
                {
                    i++;
                    continue;
                }

                Control title = allRows[i++];
                var signals = new List<Control>();
                while (i < allRows.Count && !IsMsgTitleLabel(allRows[i]))
                    signals.Add(allRows[i++]);

                if (TitleMatchesFilter(title, key))
                {
                    result.Add(title);
                    result.AddRange(signals);
                    continue;
                }

                var matchedSignals = new List<Control>();
                for (int s = 0; s < signals.Count; s++)
                {
                    string sigName = GetSigNameText(signals[s]);
                    if (!string.IsNullOrEmpty(sigName) && sigName.ToLowerInvariant().Contains(key))
                        matchedSignals.Add(signals[s]);
                }

                if (matchedSignals.Count > 0)
                {
                    result.Add(title);
                    result.AddRange(matchedSignals);
                }
            }

            return result;
        }

        private static bool TitleMatchesFilter(Control title, string keyLower)
        {
            if (title.Tag is MsgRowMeta meta && meta.SearchBlob.Contains(keyLower))
                return true;
            return (title.Text ?? string.Empty).ToLowerInvariant().Contains(keyLower);
        }

        private static string GetSigNameText(Control rowControl)
        {
            if (rowControl is UI_Row_RecvSigDisplay recvRow)
                return recvRow.GetSigNameText();
            if (rowControl is UI_Row_SendSigDisplay sendRow)
                return sendRow.GetSigNameText();
            return string.Empty;
        }

        private Label CreateMsgTitleLabel(uint msgId, string msgName, uint msgCycle = 1, bool showNonCycleHint = false)
        {
            string name = msgName ?? string.Empty;
            string hex = msgId.ToString("X");
            string hex3 = msgId.ToString("X3");

            string titleText = "  0x" + hex3.ToUpper() + "  " + name;
            if (showNonCycleHint && msgCycle == 0)
                titleText += "  · 不周期发送";

            Label titleLabel = new Label();
            titleLabel.AutoEllipsis = false;
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.Text = titleText;
            titleLabel.BackColor = SigRowVisualTheme.TitleBackColor;
            titleLabel.ForeColor = showNonCycleHint && msgCycle == 0
                ? Color.FromArgb(71, 85, 105)
                : SigRowVisualTheme.TitleForeColor;
            titleLabel.Font = _msgTitleBoldFont;
            titleLabel.Padding = new Padding(8, 0, 8, 0);
            titleLabel.Tag = new MsgRowMeta
            {
                MsgId = msgId,
                MsgName = name,
                SearchBlob = $"0x{hex} 0x{hex3} {hex} {hex3} {msgId} {name}".ToLowerInvariant()
            };
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

            Font headerFont = new Font(font.FontFamily, Math.Max(8f, font.Size - 0.5f), FontStyle.Bold);
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
                Padding = new Padding(8, 0, 4, 0),
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

        private static void DisposeOwnedRowControls(List<Control> rows)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                Control c = rows[i];
                if (c is not null && !c.IsDisposed)
                    c.Dispose();
            }
            rows.Clear();
        }

        private static void ClearMsgAreaPanel(Panel panel, bool disposeRowControls = true)
        {
            if (panel.Controls.Count == 0)
            {
                panel.AutoScrollMinSize = Size.Empty;
                panel.Tag = null;
                return;
            }

            panel.SuspendLayout();
            try
            {
                Control[] old = new Control[panel.Controls.Count];
                panel.Controls.CopyTo(old, 0);
                panel.Controls.Clear();
                for (int i = 0; i < old.Length; i++)
                {
                    Control c = old[i];
                    bool isDataRow = IsMsgTitleLabel(c)
                        || c is UI_Row_RecvSigDisplay
                        || c is UI_Row_SendSigDisplay;
                    if (disposeRowControls || !isDataRow)
                        c.Dispose();
                }
                panel.AutoScrollMinSize = Size.Empty;
                panel.Tag = null;
            }
            finally
            {
                panel.ResumeLayout(false);
            }
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

        private static void ClearFrozenHeader(Panel frozenHeaderInner)
        {
            if (frozenHeaderInner.Controls.Count == 0)
            {
                frozenHeaderInner.Left = 0;
                frozenHeaderInner.Size = new Size(0, (int)ColumnHeaderHeight + GroupSeparatorHeight);
                return;
            }

            Control[] old = new Control[frozenHeaderInner.Controls.Count];
            frozenHeaderInner.Controls.CopyTo(old, 0);
            frozenHeaderInner.Controls.Clear();
            for (int i = 0; i < old.Length; i++)
                old[i].Dispose();
            frozenHeaderInner.Left = 0;
            frozenHeaderInner.Size = new Size(0, (int)ColumnHeaderHeight + GroupSeparatorHeight);
        }

        private static void UpdateFrozenHeader(Panel frozenHeaderInner, SigRowColumnLayout layout, int rowWidth, Font font)
        {
            ClearFrozenHeader(frozenHeaderInner);

            Panel columnHeader = CreateColumnHeaderPanel(layout, font);
            columnHeader.Location = Point.Empty;
            columnHeader.Size = new Size(rowWidth, (int)ColumnHeaderHeight);
            columnHeader.Tag = "ColumnHeader";

            Panel headerSeparator = CreateHorizontalRowSeparator(SigRowVisualTheme.GroupSeparator);
            headerSeparator.Location = new Point(0, (int)ColumnHeaderHeight);
            headerSeparator.Size = new Size(rowWidth, GroupSeparatorHeight);

            frozenHeaderInner.Size = new Size(rowWidth, (int)ColumnHeaderHeight + GroupSeparatorHeight);
            frozenHeaderInner.Controls.Add(columnHeader);
            frozenHeaderInner.Controls.Add(headerSeparator);
        }

        private static void PopulateMsgAreaPanel(
            Panel panel,
            GroupBox groupBox,
            List<Control> rowControls,
            Panel frozenHeaderInner,
            Panel filterBand,
            List<Control> layoutSourceRows = null,
            bool disposeRowControls = true)
        {
            panel.SuspendLayout();
            groupBox.SuspendLayout();
            try
            {
                ClearMsgAreaPanel(panel, disposeRowControls);
                ClearFrozenHeader(frozenHeaderInner);

                List<Control> layoutRows = layoutSourceRows is { Count: > 0 } ? layoutSourceRows : rowControls;
                if (layoutRows.Count == 0)
                {
                    ApplyGroupBoxContentWidth(groupBox, 220);
                    return;
                }

                // 列宽按完整数据源计算，避免筛选时列宽跳动
                SigRowColumnLayout columnLayout = SigRowColumnLayout.Calculate(layoutRows, panel.Font);
                panel.Tag = columnLayout;

                int rowWidth = columnLayout.TotalWidth;
                int titleHeight = (int)MsgTitleRowHeight;
                int signalHeight = (int)MsgRowHeight;
                int y = 0;

                // 表头放在滚动区外的冻结带，内容区仅布置报文/信号行
                UpdateFrozenHeader(frozenHeaderInner, columnLayout, rowWidth, panel.Font);

                if (rowControls.Count == 0)
                {
                    panel.AutoScrollMinSize = new Size(rowWidth, 4);
                    ApplyGroupBoxContentWidth(groupBox, rowWidth);
                    return;
                }

                // 先离屏布置，再一次性 AddRange，减少中间布局/重绘
                var staged = new List<Control>(rowControls.Count * 2 + 4);

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
                    staged.Add(rowControl);
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
                    staged.Add(rowSeparator);
                    y += separatorHeight;
                }

                panel.Controls.AddRange(staged.ToArray());
                panel.AutoScrollMinSize = new Size(rowWidth, y + 4);
                ApplyGroupBoxContentWidth(groupBox, rowWidth);
            }
            finally
            {
                groupBox.ResumeLayout(false);
                panel.ResumeLayout(false);
                SyncFrozenHeaderScroll(panel, frozenHeaderInner, filterBand);
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
        /// 会话线程：仅脏时重填周期发送载荷（读信号字符串缓存，不碰控件树布局）。
        /// </summary>
        public void Session_UpdateCycleSendMsgData()
        {
            if (!_cycleSendDirty) return;
            if (DeviceInterfaceMng.GetInstance() is null) return;
            Dictionary<uint, CycleSend_Canfd_Frame> cycSendMsgList = DeviceInterfaceMng.GetInstance().GetCycleMsgSendDict();
            if (cycSendMsgList.Count == 0) return;

            _cycleSendDirty = false;

            foreach (var item in cycSendMsgList)
            {
                CycleSend_Canfd_Frame sendData = item.Value;
                byte[] payload = sendData.msgData.data;
                if (payload is null || payload.Length < 64)
                {
                    payload = new byte[64];
                    sendData.msgData.data = payload;
                }
                else
                {
                    Array.Clear(payload, 0, payload.Length);
                }

                if (sendMsgArea_sigRowUIDict.TryGetValue(item.Key, out List<UI_Row_SendSigDisplay> rows))
                {
                    for (int i = 0; i < rows.Count; i++)
                        rows[i].SetSigValueToMsg(payload);
                }

                cycSendMsgList[item.Key] = sendData;
            }
        }

        /// <summary>
        /// UI 泵：拉取会话接收快照，更新模型视图与接收区显示（必须在 UI 线程调用）。
        /// </summary>
        public void UiPump_ApplyRecvAndModel()
        {
            var device = DeviceInterfaceMng.GetInstance();
            if (device is null || !device.canDeviceOpenFlag)
                return;

            Dictionary<uint, Canfd_Frame_Com> framesById = device.TakeRecvSnapshot();
            if (framesById is null || framesById.Count == 0)
                return;

            // 先缓存最近帧（含筛选隐藏的报文），再刷新可见行与模型视图
            foreach (var kv in framesById)
                _lastRecvFramesById[kv.Key] = kv.Value;

            if (subWin_ModelView is { IsDisposed: false })
                subWin_ModelView.OnUiPumpUpdate(new List<Canfd_Frame_Com>(framesById.Values));

            foreach (var kv in framesById)
            {
                if (!recvMsgArea_sigRowUIDict.TryGetValue(kv.Key, out List<UI_Row_RecvSigDisplay> rows))
                    continue;
                if (kv.Value.data is null)
                    continue;

                for (int i = 0; i < rows.Count; i++)
                {
                    UI_Row_RecvSigDisplay row = rows[i];
                    // 筛选隐藏（无 Parent）或不可见行跳过刷新，显示时由 RefreshVisibleRecvFromLastFrames 补
                    if (row.Parent is null || !row.Visible)
                        continue;

                    string text = row.ComputeDisplayValue(kv.Value);
                    row.ApplyDisplayValue(text);
                }
            }
        }

        private void Btn_ConnectDevice_Click(object sender, EventArgs e)
        {
            if (deviceInterfaceMng is null)
                return;

            if (deviceInterfaceMng.canDeviceOpenFlag)
            {
                MessageBox.Show(FindForm(), "设备已连接，请先断开后再重新连接。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool ok = deviceInterfaceMng.OpenCanDevice(
                comboBox_CanDeviceType.SelectedIndex,
                comboBox_CanType.SelectedIndex);
            if (ok)
            {
                AppLogMng.DisplayLog(
                    $"已连接 {comboBox_CanDeviceType.Text} / {comboBox_CanType.Text}", true);
                return;
            }

            string detail = AppLogMng.GetGobalLogStr();
            if (string.IsNullOrWhiteSpace(detail))
                detail = "请检查设备是否插好、驱动是否正常，以及是否被其他程序占用。";

            MessageBox.Show(FindForm(),
                "连接 CAN 设备失败。\n\n" + detail,
                "连接失败",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void Btn_DisconnectDevice_Click(object sender, EventArgs e)
        {
            if (deviceInterfaceMng is null)
                return;

            if (!deviceInterfaceMng.canDeviceOpenFlag)
            {
                MessageBox.Show(FindForm(), "当前未连接设备。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            deviceInterfaceMng.CloseCanDevice();
        }

        private void Btn_ModelView_Click(object sender, EventArgs e)
        {
            if (subWin_ModelView is { IsDisposed: false })
            {
                subWin_ModelView.Activate();
                return;
            }
            subWin_ModelView = new SubWin_ModelView();
            subWin_ModelView.FormClosed += (_, _) => subWin_ModelView = null;
            subWin_ModelView.Show();
        }

        public void TryUpdateOpenModelView()
        {
            if (subWin_ModelView is { IsDisposed: false })
                subWin_ModelView.OnMainLoopUpdate();
        }

        public void NotifyModelViewDbcChanged()
        {
            if (subWin_ModelView is { IsDisposed: false })
                subWin_ModelView.OnDbcChanged();
        }

        public static void NotifyModelViewClosed(SubWin_ModelView closed)
        {
            // reserved for future global registry
        }

        public void CloseModelViewIfOpen()
        {
            if (subWin_ModelView is { IsDisposed: false })
            {
                subWin_ModelView.Close();
                subWin_ModelView = null;
            }
        }

        private void Btn_DiagView_Click(object sender, EventArgs e)
        {
            subWin_DiagView = new SubWin_DiagView();
            subWin_DiagView.Show();
        }
    }
}

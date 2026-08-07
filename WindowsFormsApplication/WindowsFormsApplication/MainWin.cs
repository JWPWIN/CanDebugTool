using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication.UI;

namespace WindowsFormsApplication
{
    public partial class MainWin : Form
    {
        private const int WM_ENTERSIZEMOVE = 0x0231;
        private const int WM_EXITSIZEMOVE = 0x0232;

        public readonly string AppVerStr = "-V1.0-20260206";

        //主循环线程（仅会话收发）
        public LongRunningThreadService mainLoopThread;

        /// <summary>UI 泵：拉取会话快照并刷新界面。</summary>
        private Timer _uiSessionTimer;
        private bool _matrixLoadInProgress;
        private int _statusStripAccumMs;
        private bool _inSizeMove;

        private const int UiSessionPumpIntervalMs = 20;
        private const int StatusStripIntervalMs = 100;
        /// <summary>超过该行数视为重 UI：拖动时不做全树截图。</summary>
        private const int HeavyUiRowThreshold = 80;

        // 状态栏节流：仅内容变化时写控件
        private string _lastStatusTimeText = string.Empty;
        private string _lastStatusPageText = string.Empty;
        private string _lastStatusDeviceText = string.Empty;
        private string _lastStatusLogText = string.Empty;
        private Color _lastStatusDeviceColor;
        private Color _lastStatusLogColor;
        private string _lastDbcStateText = string.Empty;

        public MainWin()
        {
            // 必须在 InitializeComponent 之前创建：子控件构造期会访问 GetInstance()
            _ = new CanDbcDataManager();

            InitializeComponent();
            InitOpenCanMatrixButton();
            EnableSmoothResize();
            UpdateDbcLoadStateIndicator();

            // 会话线程：只做收发 + 周期载荷填充
            mainLoopThread = new LongRunningThreadService();
            mainLoopThread.OnSession1ms = () => uI_ComUpper.Session_UpdateCycleSendMsgData();
            mainLoopThread.Start();

            // UI 线程定时泵：接收显示 / FSM；状态栏降频
            _uiSessionTimer = new Timer { Interval = UiSessionPumpIntervalMs };
            _uiSessionTimer.Tick += UiSessionTimer_Tick;
            _uiSessionTimer.Start();
        }

        private void UiSessionTimer_Tick(object sender, EventArgs e)
        {
            // 拖动/缩放窗口期间不刷 UI，避免与 DWM 抢 UI 线程
            if (_inSizeMove)
                return;

            uI_ComUpper.UiPump_ApplyRecvAndModel();

            _statusStripAccumMs += UiSessionPumpIntervalMs;
            if (_statusStripAccumMs >= StatusStripIntervalMs)
            {
                _statusStripAccumMs = 0;
                UpdateStatusStripInfo();
            }
        }

        private PictureBox _resizeFreezeOverlay;
        private Bitmap _resizeFreezeBitmap;

        /// <summary>
        /// 移动/缩放时冻结内容区并暂停 UI 泵，避免海量子控件重绘导致拖动卡顿。
        /// 不使用窗体级 WS_EX_COMPOSITED：状态栏定时刷新时会连带重绘页签文字，造成闪烁。
        /// </summary>
        private void EnableSmoothResize()
        {
            EnableDoubleBuffered(this);
            EnableDoubleBuffered(toolStripContainer1.ContentPanel);
            EnableDoubleBuffered(tableLayoutPanel_MainContent);
            EnableDoubleBuffered(tabControl_AllFunsSplit);
        }

        private static void EnableDoubleBuffered(Control control)
        {
            typeof(Control).GetProperty("DoubleBuffered",
                    BindingFlags.Instance | BindingFlags.NonPublic)?
                .SetValue(control, true, null);
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_ENTERSIZEMOVE)
                BeginSizeMoveInteraction();
            else if (m.Msg == WM_EXITSIZEMOVE)
                EndSizeMoveInteraction();

            base.WndProc(ref m);
        }

        private void BeginSizeMoveInteraction()
        {
            if (_inSizeMove)
                return;
            _inSizeMove = true;
            _uiSessionTimer?.Stop();
            BeginContentFreezeOverlay();
        }

        private void EndSizeMoveInteraction()
        {
            if (!_inSizeMove)
                return;
            _inSizeMove = false;
            EndContentFreezeOverlay();
            if (_uiSessionTimer is not null && !_matrixLoadInProgress)
                _uiSessionTimer.Start();
        }

        private void BeginContentFreezeOverlay()
        {
            Control target = toolStripContainer1.ContentPanel;
            ClearContentFreezeOverlay();

            if (!target.IsHandleCreated || target.ClientSize.Width < 1 || target.ClientSize.Height < 1)
                return;

            bool heavy = uI_ComUpper?.IsHeavyMsgUi(HeavyUiRowThreshold) == true;

            // 重 UI：全树 DrawToBitmap 本身很慢，改用纯色遮罩；轻 UI：截图更自然
            if (!heavy)
            {
                target.Update();
                _resizeFreezeBitmap = new Bitmap(target.ClientSize.Width, target.ClientSize.Height);
                target.DrawToBitmap(_resizeFreezeBitmap, new Rectangle(Point.Empty, target.ClientSize));
            }

            _resizeFreezeOverlay = new PictureBox
            {
                Name = "_resizeFreezeOverlay",
                Image = _resizeFreezeBitmap,
                SizeMode = PictureBoxSizeMode.Normal,
                Dock = DockStyle.Fill,
                BackColor = target.BackColor
            };
            target.Controls.Add(_resizeFreezeOverlay);
            _resizeFreezeOverlay.BringToFront();

            uI_ComUpper?.BeginLiveResize();
        }

        private void EndContentFreezeOverlay()
        {
            uI_ComUpper?.EndLiveResize();
            ClearContentFreezeOverlay();

            Control target = toolStripContainer1.ContentPanel;
            target.Invalidate(true);
            target.Update();
        }

        private void ClearContentFreezeOverlay()
        {
            if (_resizeFreezeOverlay is not null)
            {
                Control parent = _resizeFreezeOverlay.Parent;
                parent?.Controls.Remove(_resizeFreezeOverlay);
                _resizeFreezeOverlay.Image = null;
                _resizeFreezeOverlay.Dispose();
                _resizeFreezeOverlay = null;
            }

            if (_resizeFreezeBitmap is not null)
            {
                _resizeFreezeBitmap.Dispose();
                _resizeFreezeBitmap = null;
            }
        }

        private void InitOpenCanMatrixButton()
        {
            Btn_OpenCanMatrix.Image = CreateFolderIcon(26);
            Btn_OpenCanMatrix.ImageAlign = ContentAlignment.MiddleCenter;
            Btn_OpenCanMatrix.Text = string.Empty;
            Btn_OpenCanMatrix.Size = new Size(34, 34);
            toolTip_Main.SetToolTip(Btn_OpenCanMatrix, "加载 CAN 通信协议配置（Excel）");
        }

        private static Image CreateFolderIcon(int size)
        {
            var bmp = new Bitmap(size, size);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);

            float s = size;
            // 文件夹盖
            using (var brush = new SolidBrush(Color.FromArgb(245, 196, 82)))
            {
                PointF[] tab =
                {
                    new(s * 0.10f, s * 0.22f),
                    new(s * 0.38f, s * 0.22f),
                    new(s * 0.48f, s * 0.34f),
                    new(s * 0.90f, s * 0.34f),
                    new(s * 0.90f, s * 0.44f),
                    new(s * 0.10f, s * 0.44f)
                };
                g.FillPolygon(brush, tab);
            }

            // 文件夹主体
            using (var brush = new SolidBrush(Color.FromArgb(250, 204, 99)))
            using (var pen = new Pen(Color.FromArgb(180, 130, 40), Math.Max(1f, s / 18f)))
            {
                var body = new RectangleF(s * 0.10f, s * 0.40f, s * 0.80f, s * 0.48f);
                g.FillRectangle(brush, body);
                g.DrawRectangle(pen, body.X, body.Y, body.Width, body.Height);
            }

            return bmp;
        }

        private void UpdateDbcLoadStateIndicator()
        {
            bool loaded = CanDbcDataManager.GetInstance()?.isLoadCfg == true;
            string stateText = loaded ? "已加载矩阵" : "未加载矩阵";
            if (stateText == _lastDbcStateText)
                return;

            _lastDbcStateText = stateText;
            Color stateColor = loaded ? Color.FromArgb(34, 139, 84) : Color.FromArgb(107, 114, 128);

            label_DbcLoadState.Text = stateText;
            label_DbcLoadState.ForeColor = stateColor;
            panel_DbcStatusDot.BackColor = stateColor;
            toolStripStatusLabel_DBCState.Text = "{" + stateText + "}";
            toolStripStatusLabel_DBCState.ForeColor = stateColor;
        }

        /// <summary>在 UI 线程更新状态栏（100ms 节流 + 内容变化才写控件）。</summary>
        private void UpdateStatusStripInfo()
        {
            bool deviceOpen = DeviceInterfaceMng.GetInstance()?.canDeviceOpenFlag == true;
            string deviceTypeName = deviceOpen
                ? DeviceInterfaceMng.GetInstance().curCanDeviceType.ToString()
                : string.Empty;

            string timeText = "{" + DateTime.Now.ToString("HH:mm:ss") + "}";
            if (timeText != _lastStatusTimeText)
            {
                _lastStatusTimeText = timeText;
                toolStripStatusLabel_CurSysTime.Text = timeText;
            }

            if (tabControl_AllFunsSplit.SelectedTab is not null)
            {
                string pageText = "{" + tabControl_AllFunsSplit.SelectedTab.Text + "}";
                if (pageText != _lastStatusPageText)
                {
                    _lastStatusPageText = pageText;
                    toolStripStatusLabel_CurPageName.Text = pageText;
                }
            }

            UpdateDbcLoadStateIndicator();

            string deviceText = deviceOpen
                ? "{" + $"已连接设备:{deviceTypeName}" + "}"
                : "{未连接设备}";
            Color deviceColor = deviceOpen ? Color.Green : Color.Gray;
            if (deviceText != _lastStatusDeviceText || deviceColor != _lastStatusDeviceColor)
            {
                _lastStatusDeviceText = deviceText;
                _lastStatusDeviceColor = deviceColor;
                toolStripStatusLabel_DeviceCntState.Text = deviceText;
                toolStripStatusLabel_DeviceCntState.ForeColor = deviceColor;
            }

            string logText = "{" + $"日志:{AppLogMng.GetGobalLogStr()}" + "}";
            Color logColor = AppLogMng.GetGobalLogStrColor();
            if (logText != _lastStatusLogText || logColor != _lastStatusLogColor)
            {
                _lastStatusLogText = logText;
                _lastStatusLogColor = logColor;
                toolStripStatusLabel_GlobalLogBox.Text = logText;
                toolStripStatusLabel_GlobalLogBox.ForeColor = logColor;
            }
        }

        private async void Btn_OpenCanMatrix_Click(object sender, EventArgs e)
        {
            if (_matrixLoadInProgress)
                return;

            string filePath = ExcelManager.PickExcelFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            _matrixLoadInProgress = true;
            bool oldWait = UseWaitCursor;
            UseWaitCursor = true;
            Btn_OpenCanMatrix.Enabled = false;
            uI_ComUpper.SetMatrixLoading(true);
            try
            {
                bool loaded = await Task.Run(() =>
                {
                    Dictionary<string, List<List<string>>> excelAllData = ExcelManager.ImportDataFromFile(filePath);
                    if (excelAllData is null || excelAllData.Count == 0)
                        return false;
                    return CanDbcDataManager.GetInstance().LoadCanMatrixFromExcelData(excelAllData);
                }).ConfigureAwait(true);

                if (!loaded)
                {
                    AppLogMng.DisplayLog("未选择有效矩阵或文件无数据", false);
                    return;
                }

                UpdateDbcLoadStateIndicator();
                RefreshComUpperAfterDbcChanged();
            }
            catch (Exception ex)
            {
                AppLogMng.DisplayLog("加载矩阵失败: " + ex.Message, false);
                MessageBox.Show(this, "加载 CAN 矩阵失败：\n" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                uI_ComUpper.SetMatrixLoading(false);
                Btn_OpenCanMatrix.Enabled = true;
                UseWaitCursor = oldWait;
                _matrixLoadInProgress = false;
            }
        }

        private void RefreshComUpperAfterDbcChanged()
        {
            uI_ComUpper.InvalidateMsgAreas();
            uI_CanMatrix.InvalidateMatrixCache();
            uI_ComUpper.NotifyModelViewDbcChanged();

            // 换矩阵：清会话缓冲并重建周期发送表（即使当前不在通信页）
            DeviceInterfaceMng.GetInstance()?.ClearSessionRuntimeBuffers();
            uI_ComUpper.RebuildCycleSendMsgListFromDbc();

            if (tabControl_AllFunsSplit.SelectedTab.Name == "tabPage_ComUpper")
            {
                uI_ComUpper.EnsureMsgAreasInitialized();
            }
            else if (tabControl_AllFunsSplit.SelectedTab.Name == "tabPage_CanMatrix")
            {
                uI_CanMatrix.UpdateMsgTableView();
            }
        }

        private void tabControl_AllFunsSplit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //如果切换页面到上位机页面，初始化窗口UI
            if (tabControl_AllFunsSplit.SelectedTab.Name == "tabPage_ComUpper")
            {
                uI_ComUpper.EnsureMsgAreasInitialized();
            }

            //如果切换页面到CAN通信矩阵显示页面，显示通信协议
            if (tabControl_AllFunsSplit.SelectedTab.Name == "tabPage_CanMatrix")
            {
               uI_CanMatrix.UpdateMsgTableView();
            }
        }

        //Form.Closing 事件：此事件在窗口关闭之前立即发生，通常用于执行一些清理工作，如保存数据或询问用户是否真的要关闭窗口
        //可以通过设置CancelEventArgs的Cancel属性来阻止窗口关闭
        private void MainWin_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (_uiSessionTimer is not null)
            {
                _uiSessionTimer.Stop();
                _uiSessionTimer.Tick -= UiSessionTimer_Tick;
                _uiSessionTimer.Dispose();
                _uiSessionTimer = null;
            }

            // 先停主循环，避免关闭过程中继续访问设备
            mainLoopThread?.Stop();
            uI_ComUpper.CloseModelViewIfOpen();
        }

        //Form.Closed 事件：此事件在窗口关闭之后发生。它主要用于执行在窗口关闭后需要进行的操作，如释放资源或启动其他窗体
        private void MainWin_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            ClearContentFreezeOverlay();

            //退出主窗口时保证关闭CAN设备
            if (DeviceInterfaceMng.GetInstance()?.canDeviceOpenFlag == true)
                DeviceInterfaceMng.GetInstance().CloseCanDevice();
        }
    }
}

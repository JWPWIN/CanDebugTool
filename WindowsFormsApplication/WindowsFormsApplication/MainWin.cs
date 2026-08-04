using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WindowsFormsApplication.UI;

namespace WindowsFormsApplication
{
    public partial class MainWin : Form
    {
        public readonly string AppVerStr = "-V1.0-20260206";

        //主循环线程
        public LongRunningThreadService mainLoopThread;

        public MainWin()
        {
            InitializeComponent();
            InitOpenCanMatrixButton();
            EnableSmoothResize();

            //创建任务主循环线程 用于长时间持续执行的任务
            mainLoopThread = new LongRunningThreadService(this);
            //开启主循环线程
            mainLoopThread.Start();

            //初始化APP数据
            CanDbcDataManager canDbcDataManager = new CanDbcDataManager();
            UpdateDbcLoadStateIndicator();
        }

        private PictureBox _resizeFreezeOverlay;
        private Bitmap _resizeFreezeBitmap;

        /// <summary>
        /// 缩放时用静态快照覆盖内容区：拖动过程保持画面可见且不逐帧重绘，松开后再整体刷新。
        /// </summary>
        private void EnableSmoothResize()
        {
            typeof(Control).GetProperty("DoubleBuffered",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?
                .SetValue(this, true, null);
            typeof(Control).GetProperty("DoubleBuffered",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?
                .SetValue(toolStripContainer1.ContentPanel, true, null);

            ResizeBegin += (_, _) => BeginResizeFreezeOverlay();
            ResizeEnd += (_, _) => EndResizeFreezeOverlay();
        }

        private void BeginResizeFreezeOverlay()
        {
            Control target = toolStripContainer1.ContentPanel;
            ClearResizeFreezeOverlay();

            if (!target.IsHandleCreated || target.ClientSize.Width < 1 || target.ClientSize.Height < 1)
                return;

            // 先确保当前帧已画完，再截图
            target.Update();
            _resizeFreezeBitmap = new Bitmap(target.ClientSize.Width, target.ClientSize.Height);
            target.DrawToBitmap(_resizeFreezeBitmap, new Rectangle(Point.Empty, target.ClientSize));

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

            // 底层暂停布局，避免拖动过程中计算海量子控件
            uI_ComUpper?.BeginLiveResize();
        }

        private void EndResizeFreezeOverlay()
        {
            uI_ComUpper?.EndLiveResize();
            ClearResizeFreezeOverlay();

            Control target = toolStripContainer1.ContentPanel;
            target.Invalidate(true);
            target.Update();
        }

        private void ClearResizeFreezeOverlay()
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

        /// <summary>启用窗口合成，减轻缩放时闪烁与撕裂。</summary>
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_COMPOSITED = 0x02000000;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_COMPOSITED;
                return cp;
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
            Color stateColor = loaded ? Color.FromArgb(34, 139, 84) : Color.FromArgb(107, 114, 128);

            label_DbcLoadState.Text = stateText;
            label_DbcLoadState.ForeColor = stateColor;
            panel_DbcStatusDot.BackColor = stateColor;
            toolStripStatusLabel_DBCState.Text = "{" + stateText + "}";
            toolStripStatusLabel_DBCState.ForeColor = stateColor;
        }

        /// <summary>
        /// 更新状态栏信息
        /// </summary>
        public void MainLoopThread_Task_UpdateStatusStripInfo()
        {
            //实时更新状态栏信息
            if (statusStrip.InvokeRequired)
            {
                //在UI线程上异步执行访问控件操作
                //更新系统时间信息
                statusStrip.Invoke(new Action(() => toolStripStatusLabel_CurSysTime.Text = "{" + DateTime.Now.ToString() + "}"));
                //更新当前页签名称
                statusStrip.Invoke(new Action(() => toolStripStatusLabel_CurPageName.Text = "{" + tabControl_AllFunsSplit.SelectedTab.Text + "}"));
                //显示DBC状态
                statusStrip.Invoke(new Action(UpdateDbcLoadStateIndicator));
                //显示设备连接状态
                if (DeviceInterfaceMng.GetInstance().canDeviceOpenFlag == true)
                {
                    statusStrip.Invoke(new Action(() => toolStripStatusLabel_DeviceCntState.Text = "{" + $"已连接设备:{DeviceInterfaceMng.GetInstance().curCanDeviceType.ToString()}" + "}"));
                    statusStrip.Invoke(new Action(() => toolStripStatusLabel_DeviceCntState.ForeColor = Color.Green));
                }
                else
                {
                    statusStrip.Invoke(new Action(() => toolStripStatusLabel_DeviceCntState.Text = "{未连接设备}"));
                    statusStrip.Invoke(new Action(() => toolStripStatusLabel_DeviceCntState.ForeColor = Color.Gray));
                }
                //更新全局Log信息
                statusStrip.Invoke(new Action(() => toolStripStatusLabel_GlobalLogBox.Text = "{" + $"日志:{AppLogMng.GetGobalLogStr()}" + "}"));
                statusStrip.Invoke(new Action(() => toolStripStatusLabel_GlobalLogBox.ForeColor = AppLogMng.GetGobalLogStrColor()));
            }
            else
            {
                //在UI线程上直接访问控件
                //由于确认该函数是在异步线程上访问的本UI线程控件 因此该处不做处理
            }



        }

        private void Btn_OpenCanMatrix_Click(object sender, EventArgs e)
        {
            CanDbcDataManager.GetInstance().LoadCanMatrixFromExcel();
            UpdateDbcLoadStateIndicator();
            RefreshComUpperAfterDbcChanged();
        }

        private void RefreshComUpperAfterDbcChanged()
        {
            uI_ComUpper.InvalidateMsgAreas();
            uI_CanMatrix.InvalidateMatrixCache();
            uI_ComUpper.NotifyModelViewDbcChanged();
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

        public void MainLoopThread_Task_UpdateComUpperUI()
        {
            if(DeviceInterfaceMng.GetInstance()?.canDeviceOpenFlag == false) return;

            //模型视图在清除接收缓冲前读取帧并评估转移
            uI_ComUpper.TryUpdateOpenModelView();
            //更新上位机接收报文窗口区域
            uI_ComUpper.MainLoopThread_Task_UpdateRecvMsgArea();
            //更新上位机发送区域数据到周期报文帧
            uI_ComUpper.MainLoopThread_Task_UpdateCycleSendMsgData();
        }

        //Form.Closing 事件：此事件在窗口关闭之前立即发生，通常用于执行一些清理工作，如保存数据或询问用户是否真的要关闭窗口
        //可以通过设置CancelEventArgs的Cancel属性来阻止窗口关闭
        private void MainWin_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            uI_ComUpper.CloseModelViewIfOpen();
        }

        //Form.Closed 事件：此事件在窗口关闭之后发生。它主要用于执行在窗口关闭后需要进行的操作，如释放资源或启动其他窗体
        private void MainWin_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            ClearResizeFreezeOverlay();

            //退出主窗口时保证关闭CAN设备
            if(DeviceInterfaceMng.GetInstance().canDeviceOpenFlag == true)
                DeviceInterfaceMng.GetInstance().CloseCanDevice();
        }
    }
}

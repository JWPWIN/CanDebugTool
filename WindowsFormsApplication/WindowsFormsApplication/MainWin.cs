using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //创建任务主循环线程 用于长时间持续执行的任务
            mainLoopThread = new LongRunningThreadService(this);
            //开启主循环线程
            mainLoopThread.Start();

            //初始化APP数据
            CanDbcDataManager canDbcDataManager = new CanDbcDataManager();
            UpdateDbcLoadStateIndicator();
        }

        private void UpdateDbcLoadStateIndicator()
        {
            bool loaded = CanDbcDataManager.GetInstance()?.isLoadCfg == true;
            string stateText = loaded ? "已加载 DBC" : "未加载 DBC";
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

        private void Btn_ImpExcelDBC_Click(object sender, EventArgs e)
        {
            CanDbcDataManager.GetInstance().LoadCanMatrixFromExcel();
            UpdateDbcLoadStateIndicator();
            RefreshComUpperAfterDbcChanged();
        }

        private void button_ImportTxtDbc_Click(object sender, EventArgs e)
        {
            CanDbcDataManager.GetInstance().LoadCanMatrixFromDBC();
            UpdateDbcLoadStateIndicator();
            RefreshComUpperAfterDbcChanged();
        }

        private void RefreshComUpperAfterDbcChanged()
        {
            uI_ComUpper.InvalidateMsgAreas();
            uI_CanMatrix.InvalidateMatrixCache();
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

            //更新上位机接收报文窗口区域
            uI_ComUpper.MainLoopThread_Task_UpdateRecvMsgArea();
            //更新上位机发送区域数据到周期报文帧
            uI_ComUpper.MainLoopThread_Task_UpdateCycleSendMsgData();
        }

        //Form.Closing 事件：此事件在窗口关闭之前立即发生，通常用于执行一些清理工作，如保存数据或询问用户是否真的要关闭窗口
        //可以通过设置CancelEventArgs的Cancel属性来阻止窗口关闭
        private void MainWin_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            // 保存数据或执行其他清理工作
            // 如果需要取消关闭，设置e.Cancel = true;
        }

        //Form.Closed 事件：此事件在窗口关闭之后发生。它主要用于执行在窗口关闭后需要进行的操作，如释放资源或启动其他窗体
        private void MainWin_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            // 释放资源或执行其他清理工作
            // 可以使用e.CloseReason属性获取关闭的原因等信息

            //退出主窗口时保证关闭CAN设备
            if(DeviceInterfaceMng.GetInstance().canDeviceOpenFlag == true)
                DeviceInterfaceMng.GetInstance().CloseCanDevice();
        }
    }
}

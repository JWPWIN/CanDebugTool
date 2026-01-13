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
                statusStrip.Invoke(new Action(() => toolStripStatusLabel_CurSysTime.Text =  "{" + DateTime.Now.ToString() + "}"));
                //更新当前页签名称
                statusStrip.Invoke(new Action(() => toolStripStatusLabel_CurPageName.Text = "{" + tabControl_AllFunsSplit.SelectedTab.Text + "}"));
                //显示DBC状态
                if (CanDbcDataManager.GetInstance().isLoadCfg == true)
                {
                    statusStrip.Invoke(new Action(() => toolStripStatusLabel_DBCState.Text = "{已加载DBC}"));
                    statusStrip.Invoke(new Action(() => toolStripStatusLabel_DBCState.ForeColor = Color.Green));
                }
                else
                {
                    statusStrip.Invoke(new Action(() => toolStripStatusLabel_DBCState.Text = "{未加载DBC}"));
                    statusStrip.Invoke(new Action(() => toolStripStatusLabel_DBCState.ForeColor = Color.Gray));
                }
                //显示设备连接状态
                if (DeviceInterfaceMng.GetInstance().canDeviceOpenFlag == true)
                {
                    statusStrip.Invoke(new Action(() => toolStripStatusLabel_DeviceCntState.Text = "{"+ $"已连接设备:{DeviceInterfaceMng.GetInstance().curCanDeviceType.ToString()}" + "}"));
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
    }
}

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

        //Dbc数据管理窗口
        Win_DbcDataManager win_DbcDataManager;
        //Can通信上位机窗口
        Win_ComUpper win_ComUpper;

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

        private void Btn_DisplayDbcDataManager_Click(object sender, EventArgs e)
        {
            win_DbcDataManager = new Win_DbcDataManager();
            win_DbcDataManager.Show();
        }

        private void Btn_DisplayComUpper_Click(object sender, EventArgs e)
        {
            win_ComUpper = new Win_ComUpper();
            win_ComUpper.Show();
        }

        /// <summary>
        /// 更新全局Log文本
        /// </summary>
        public void MainLoopThread_Task_UpdateGlobalLogText()
        {
            if (textBox_GlobalLog.InvokeRequired)
            {
                // 在UI线程上异步执行访问控件操作
                textBox_GlobalLog.Invoke(new Action(() => textBox_GlobalLog.Text = AppLogMng.GetGobalLogStr()));
            }
            else
            {
                // 在UI线程上直接访问控件
                textBox_GlobalLog.Text = "控件已访问";
            }
        }
    }
}

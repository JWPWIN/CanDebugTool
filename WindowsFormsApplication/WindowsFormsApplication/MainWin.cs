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
        Win_DbcDataManager win_DbcDataManager;//Dbc数据管理窗口
        Win_ComUpper win_ComUpper;//Can通信上位机窗口


        public MainWin()
        {
            InitializeComponent();

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
    }
}

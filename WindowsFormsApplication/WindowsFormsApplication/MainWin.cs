using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication
{
    public partial class MainWin : Form
    {
        Win_CanMsgMatrix win_CanMsgMatrix;

        public MainWin()
        {
            InitializeComponent();
        }

        private void Btn_ImpExcelDBC_Click(object sender, EventArgs e)
        {
            CanDbcDataManager test = new CanDbcDataManager();
            CanDbcDataManager.GetInstance().LoadCanMatrixFromExcel();

        }

        private void Btn_DisplayCanMatix_Click(object sender, EventArgs e)
        {
            win_CanMsgMatrix = new Win_CanMsgMatrix();
            win_CanMsgMatrix.ShowDialog();
        }
    }
}

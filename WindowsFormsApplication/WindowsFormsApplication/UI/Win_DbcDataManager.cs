using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication.UI
{
    public partial class Win_DbcDataManager : Form
    {
        Win_CanMsgMatrix win_CanMsgMatrix;

        public Win_DbcDataManager()
        {
            InitializeComponent();
        }

        private void Btn_ImpExcelDBC_Click(object sender, EventArgs e)
        {
            CanDbcDataManager.GetInstance().LoadCanMatrixFromExcel();

            //如果DBC数据加载成功，按钮显示绿色
            if (CanDbcDataManager.GetInstance().isLoadCfg == true)
            {
                this.Btn_ImpExcelDBC.BackColor = System.Drawing.Color.Green;
            }

        }

        private void Btn_DisplayCanMatix_Click(object sender, EventArgs e)
        {
            win_CanMsgMatrix = new Win_CanMsgMatrix();
            win_CanMsgMatrix.ShowDialog();
        }

        private void Btn_ExportDbc_Click(object sender, EventArgs e)
        {
            string dbc = GenerateDBC.GenerateDbcForCanMatrix();
            if (dbc != null)
            {
                TextOperation.WriteData("GenerateDbc", FileType.DBC, dbc);
                MessageBox.Show("导出DBC成功");
            }
        }

        private void Btn_GntCanCode_Click(object sender, EventArgs e)
        {
            //如果DBC数据加载成功，才可以生成Can代码
            if (CanDbcDataManager.GetInstance().isLoadCfg == true)
            {
                CanCodeGenerate.GenerateAllCanCode();
                MessageBox.Show("Can代码生成成功");
            }

        }

        private void Btn_ExportXml_Click(object sender, EventArgs e)
        {
            //如果DBC数据加载成功，才可以生成Xml
            if (CanDbcDataManager.GetInstance().isLoadCfg == true)
            {
                GenerateXml.GenerateXmlForCanMatrix();
                MessageBox.Show("CanXml文件生成成功");
            }


        }
    }
}

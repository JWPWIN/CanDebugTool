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
        public MainWin()
        {
            InitializeComponent();
        }

        private void ImpExcelDBC_Click(object sender, EventArgs e)
        {
            CanDbcDataManager test = new CanDbcDataManager();
            CanDbcDataManager.GetInstance().LoadCanMatrixFromExcel();

            //Display test infomation in to test box
            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
            {
                this.TestTextBox.Text += item.msgId.ToString() + " ";
            }


        }

        private void UpdateMsglistView()
        {
            if (CanDbcDataManager.GetInstance().isLoadCfg == false)
            {
                //清除ListView所有数据
                this.MsglistView.Clear();

                //添加表头（列）
                this.MsglistView.Columns.Clear();

                this.MsglistView.Columns.Add(item, 100, HorizontalAlignment.Center);

                //添加表数据（行）
                foreach (var item in MsglistView.dataValueInfos)
                {
                    ListViewItem listItem = new ListViewItem();
                    listItem.SubItems.Clear();

                    for (int i = 0; i < item.Count; i++)
                    {
                        if (0 == i)
                        {
                            listItem.SubItems[0].Text = item[i].ToString();
                        }
                        else
                        {
                            listItem.SubItems.Add(item[i]);
                        }
                    }
                    this.Data_listView.Items.Add(listItem);
                }
            }

        }
    }
}

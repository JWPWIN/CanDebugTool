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
    public partial class Win_CanMsgMatrix : Form
    {
        public Win_CanMsgMatrix()
        {
            InitializeComponent();
            UpdateMsgTableView();
        }

        private void UpdateMsgTableView()
        {
            if (CanDbcDataManager.GetInstance().isLoadCfg == true)
            {
                DataTable dt = new DataTable();
                //添加表头（列）
                this.MsgGridView.Columns.Clear();
                dt.Columns.Add("SigName");
                dt.Columns.Add("MsgName");
                dt.Columns.Add("MsgID");
                dt.Columns.Add("MsgSize");
                dt.Columns.Add("MsgCycle");
                dt.Columns.Add("SigDesc");
                dt.Columns.Add("SigOrder");
                dt.Columns.Add("StartBit");
                dt.Columns.Add("SigLen");
                dt.Columns.Add("Factor");
                dt.Columns.Add("Offset");
                dt.Columns.Add("SigValue");
                dt.Columns.Add("ValueType");
                dt.Columns.Add("SendNode");
                dt.Columns.Add("RecvNode");
                //添加表格数据
                dt.Rows.Clear();
                //添加表数据（行）
                foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
                {
                    DataRow dr = dt.NewRow();
                    foreach (var item1 in item.signals)
                    {
                        dr["SigName"] = item1.sigName;
                        dr["MsgName"] = item.msgName;
                        dr["MsgID"] = item.msgId.ToString();
                        dr["MsgSize"] = item.msgSize.ToString();
                        dr["MsgCycle"] = item.msgCycle.ToString();
                        dr["SigDesc"] = item1.sigDesc;
                        dr["SigOrder"] = item1.sigOrderType.ToString();
                        dr["StartBit"] = item1.sigStartBit.ToString();
                        dr["SigLen"] = item1.sigLen.ToString();
                        dr["Factor"] = item1.sigFactor.ToString();
                        dr["Offset"] = item1.sigOffset.ToString();
                        dr["SigValue"] = "";
                        dr["ValueType"] = item1.valueType.ToString();
                        dr["SendNode"] = item.transmitter;
                        dr["RecvNode"] = item1.recvNode;
                    }
                    dt.Rows.Add(dr);
                }

                this.MsgGridView.DataSource = dt;
            }

        }

    }
}

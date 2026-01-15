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
    public partial class UI_CanMatrix : UserControl
    {
        public UI_CanMatrix()
        {
            InitializeComponent();
        }

        public void UpdateMsgTableView()
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
                dt.Columns.Add("ReuseFrameID");
                dt.Columns.Add("MsgType");
                dt.Columns.Add("MsgFrameType");
                //添加表格数据
                dt.Rows.Clear();
                //添加表数据（行）
                foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
                {
                    foreach (var item1 in item.signals)
                    {
                        DataRow dr = dt.NewRow();
                        dr["SigName"] = item1.sigName;
                        dr["MsgName"] = item.msgName;
                        dr["MsgID"] = "0x" + item.msgId.ToString("x3").ToUpper();
                        dr["MsgSize"] = item.msgSize.ToString();
                        dr["MsgCycle"] = item.msgCycle.ToString();
                        dr["SigDesc"] = item1.sigDesc;
                        dr["SigOrder"] = item1.sigOrderType.ToString();
                        dr["StartBit"] = item1.sigStartBit.ToString();
                        dr["SigLen"] = item1.sigLen.ToString();
                        dr["Factor"] = item1.sigFactor.ToString();
                        dr["Offset"] = item1.sigOffset.ToString();

                        //获取信号值表
                        if (item1.sigValueTable != null)
                        {
                            var _valueTableDict = item1.sigValueTable;
                            string _valueTableStr = string.Empty;
                            foreach (var item2 in _valueTableDict)
                            {
                                _valueTableStr += item2.Key.ToString() + ":" + item2.Value.ToString() + "\r\n";
                            }
                            dr["SigValue"] = _valueTableStr;
                        }
                        else
                        {
                            dr["SigValue"] = "";
                        }

                        dr["ValueType"] = item1.valueType.ToString();
                        dr["SendNode"] = item.transmitter;
                        dr["RecvNode"] = item1.recvNode;
                        dr["ReuseFrameID"] = item1.reuseFrameID;
                        dr["MsgType"] = item.msgType;

                        //获取报文CAN帧类型
                        string canFrameType = string.Empty;
                        if ((item.isExtended == false) && (item.isCanfd == false))
                        {
                            canFrameType = "Standard-Can";
                        }
                        else if ((item.isExtended == false) && (item.isCanfd == true))
                        {
                            canFrameType = "Standard-CanFd";
                        }
                        else if ((item.isExtended == true) && (item.isCanfd == false))
                        {
                            canFrameType = "Extended-Can";
                        }
                        else if ((item.isExtended == true) && (item.isCanfd == true))
                        {
                            canFrameType = "Extended-CanFd";
                        }
                        else
                        {
                            canFrameType = "Standard-Can";
                        }
                        dr["MsgFrameType"] = canFrameType;


                        dt.Rows.Add(dr);
                    }

                }

                this.MsgGridView.DataSource = dt;
            }

        }
    }
}

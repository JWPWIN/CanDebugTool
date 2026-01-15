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
    public partial class UI_DbcDataManager : UserControl
    {
        public UI_DbcDataManager()
        {
            InitializeComponent();
        }

        private void Btn_ImpExcelDBC_Click(object sender, EventArgs e)
        {
            CanDbcDataManager.GetInstance().LoadCanMatrixFromExcel();
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

        private void button_ExportExcelDbc_Click(object sender, EventArgs e)
        {
            //如果DBC数据加载成功，才可以生成Excel的DBC数据
            if (CanDbcDataManager.GetInstance().isLoadCfg == true)
            {
                //生成显示DBC的Excel表头
                List<string> titleList = new List<string>();
                for (int i = 0; i < (int)CanDbcRows.MaxNum; i++)
                {
                    titleList.Add(((CanDbcRows)i).ToString());
                }
                //生成CAN通信矩阵Excel数据
                List<List<string>> sigRowList = new List<List<string>>();
                foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
                {
                    foreach (var item1 in item.signals)
                    {
                        //每个信号使用一行数据
                        List<string> sigRow = new List<string>();

                        for (int i = 0; i < (int)CanDbcRows.MaxNum; i++)
                        {
                            switch ((CanDbcRows)i)
                            {
                                case CanDbcRows.SigName:sigRow.Add(item1.sigName);break;
                                case CanDbcRows.MsgName:sigRow.Add(item.msgName);break;
                                case CanDbcRows.MsgFrameType:sigRow.Add(GenerateDBC.GetMsgFrameType(item.isCanfd,item.isExtended).ToString());break;
                                case CanDbcRows.MsgId:sigRow.Add($"0x{item.msgId.ToString("X").ToUpper()}");break;
                                case CanDbcRows.MsgSize:sigRow.Add(item.msgSize.ToString());break;
                                case CanDbcRows.MsgCycle:sigRow.Add(item.msgCycle.ToString());break;
                                case CanDbcRows.SigDesc:sigRow.Add(item1.sigDesc);break;
                                case CanDbcRows.SigOrderType:sigRow.Add(item1.sigOrderType.ToString());break;
                                case CanDbcRows.SigStartBit:sigRow.Add(item1.sigStartBit.ToString());break;
                                case CanDbcRows.SigLen:sigRow.Add(item1.sigLen.ToString());break;
                                case CanDbcRows.SigFactor:sigRow.Add(item1.sigFactor.ToString());break;
                                case CanDbcRows.SigOffset:sigRow.Add(item1.sigOffset.ToString());break;
                                case CanDbcRows.SigValueTable:
                                    string _tableStr = string.Empty;
                                    if (item1.sigValueTable is not null)
                                    {
                                        foreach (var item2 in item1.sigValueTable)
                                        {
                                            _tableStr += item2.Key.ToString() + ":" + item2.Value.ToString() + "\r\n";
                                        }
                                    }
                                    sigRow.Add(_tableStr); 
                                    break;
                                case CanDbcRows.ValueType:sigRow.Add(item1.valueType.ToString());break;
                                case CanDbcRows.SendNode:sigRow.Add(item.transmitter); break;
                                case CanDbcRows.RecvNode:sigRow.Add(item1.recvNode);break;
                                case CanDbcRows.ReuseFrameID:sigRow.Add(item1.reuseFrameID.ToString()); break;
                                case CanDbcRows.MsgType:sigRow.Add(item.msgType.ToString());break;
                                default:break;
                            }
                        }

                        sigRowList.Add(sigRow);
                    }
                }

                //导出DBC数据到Excel
                ExcelManager.ExportData(sigRowList,titleList);
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

        private void button_ImportTxtDbc_Click(object sender, EventArgs e)
        {
            CanDbcDataManager.GetInstance().LoadCanMatrixFromDBC();
        }
    }
}

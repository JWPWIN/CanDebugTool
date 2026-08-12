using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication.UI
{
    public partial class UI_DbcDataManager : UserControl
    {
        private bool _dbcImportInProgress;

        public UI_DbcDataManager()
        {
            InitializeComponent();
        }

        private void Btn_ExportDbc_Click(object sender, EventArgs e)
        {
            if (CanDbcDataManager.GetInstance()?.isLoadCfg != true)
            {
                MessageBox.Show(this, "请先导入 Excel 或 DBC 通信矩阵。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string dbc = GenerateDBC.GenerateDbcForCanMatrix();
            if (dbc is null)
            {
                MessageBox.Show(this, "生成 DBC 失败：未加载有效矩阵。", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool ok = TextOperation.WriteData("GenerateDbc", FileType.DBC, dbc);
            if (ok)
                MessageBox.Show(this, "导出 DBC 成功", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void Btn_ImportDbc_Click(object sender, EventArgs e)
        {
            if (_dbcImportInProgress)
                return;

            string filePath = TextOperation.PickDbcFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            var mainWin = FindForm() as MainWin;
            _dbcImportInProgress = true;
            bool oldWait = UseWaitCursor;
            UseWaitCursor = true;
            Btn_ImportDbc.Enabled = false;
            mainWin?.SetMatrixLoadingState(true);
            try
            {
                string dbcText = await Task.Run(() => TextOperation.ReadDbcText(filePath)).ConfigureAwait(true);
                if (string.IsNullOrWhiteSpace(dbcText))
                {
                    AppLogMng.DisplayLog("DBC 文件为空或无法读取", false);
                    MessageBox.Show(this, "DBC 文件为空或无法读取。", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool loaded = CanDbcDataManager.GetInstance().LoadCanMatrixFromDbcText(dbcText);
                if (!loaded)
                {
                    AppLogMng.DisplayLog("导入 DBC 失败", false);
                    return;
                }

                if (mainWin is not null)
                    mainWin.NotifyDbcMatrixReloaded();
                else
                    AppLogMng.DisplayLog("DBC 已加载，但未能刷新主窗口会话区", false);
            }
            catch (Exception ex)
            {
                AppLogMng.DisplayLog("加载 DBC 失败: " + ex.Message, false);
                MessageBox.Show(this, "加载 DBC 失败：\n" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mainWin?.SetMatrixLoadingState(false);
                Btn_ImportDbc.Enabled = true;
                UseWaitCursor = oldWait;
                _dbcImportInProgress = false;
            }
        }

        private void button_ExportExcelDbc_Click(object sender, EventArgs e)
        {
            if (CanDbcDataManager.GetInstance().isLoadCfg != true)
            {
                MessageBox.Show(this, "请先导入通信矩阵。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<string> titleList = new List<string>();
            for (int i = 0; i < (int)CanDbcRows.MaxNum; i++)
                titleList.Add(((CanDbcRows)i).ToString());

            List<List<string>> sigRowList = new List<List<string>>();
            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
            {
                foreach (var item1 in item.signals)
                {
                    List<string> sigRow = new List<string>();
                    for (int i = 0; i < (int)CanDbcRows.MaxNum; i++)
                    {
                        switch ((CanDbcRows)i)
                        {
                            case CanDbcRows.SigName: sigRow.Add(item1.sigName); break;
                            case CanDbcRows.MsgName: sigRow.Add(item.msgName); break;
                            case CanDbcRows.MsgFrameType: sigRow.Add(CanDbcDataManager.GetMsgFrameType(item.isCanfd, item.isExtended).ToString()); break;
                            case CanDbcRows.MsgId: sigRow.Add($"0x{item.msgId.ToString("X").ToUpper()}"); break;
                            case CanDbcRows.MsgSize: sigRow.Add(item.msgSize.ToString()); break;
                            case CanDbcRows.MsgCycle: sigRow.Add(item.msgCycle.ToString()); break;
                            case CanDbcRows.SigDesc: sigRow.Add(item1.sigDesc); break;
                            case CanDbcRows.SigOrderType: sigRow.Add(item1.sigOrderType.ToString()); break;
                            case CanDbcRows.SigStartBit: sigRow.Add(item1.sigStartBit.ToString()); break;
                            case CanDbcRows.SigLen: sigRow.Add(item1.sigLen.ToString()); break;
                            case CanDbcRows.SigFactor: sigRow.Add(item1.sigFactor.ToString()); break;
                            case CanDbcRows.SigOffset: sigRow.Add(item1.sigOffset.ToString()); break;
                            case CanDbcRows.SigValueTable:
                                string tableStr = string.Empty;
                                if (item1.sigValueTable is not null)
                                {
                                    foreach (var item2 in item1.sigValueTable)
                                        tableStr += item2.Key.ToString() + ":" + item2.Value + "\r\n";
                                }
                                sigRow.Add(tableStr);
                                break;
                            case CanDbcRows.ValueType: sigRow.Add(item1.valueType.ToString()); break;
                            case CanDbcRows.SendNode: sigRow.Add(item.transmitter); break;
                            case CanDbcRows.RecvNode: sigRow.Add(item1.recvNode); break;
                            case CanDbcRows.ReuseFrameID: sigRow.Add(item1.reuseFrameID.ToString()); break;
                            case CanDbcRows.MsgType: sigRow.Add(item.msgType.ToString()); break;
                            default: break;
                        }
                    }
                    sigRowList.Add(sigRow);
                }
            }

            ExcelManager.ExportData(sigRowList, titleList);
        }

        private void Btn_GntCanCode_Click(object sender, EventArgs e)
        {
            if (CanDbcDataManager.GetInstance().isLoadCfg == true)
            {
                CanCodeGenerate.GenerateAllCanCode();
                MessageBox.Show("Can代码生成成功");
            }
        }

        private void Btn_ExportXml_Click(object sender, EventArgs e)
        {
            if (CanDbcDataManager.GetInstance().isLoadCfg == true)
            {
                GenerateXml.GenerateXmlForCanMatrix();
                MessageBox.Show("CanXml文件生成成功");
            }
        }
    }
}

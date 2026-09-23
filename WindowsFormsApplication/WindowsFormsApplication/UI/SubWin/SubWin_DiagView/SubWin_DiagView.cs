using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication.UI.SubWin.SubWin_DiagView
{
    public partial class SubWin_DiagView : Form
    {
        private const string ColDidEnabled = "col_DidEnabled";
        private const string ColDid = "col_Did";
        private const string ColDidName = "col_DidName";
        private const string ColDidRaw = "col_DidRaw";
        private const string ColDidAscii = "col_DidAscii";
        private const string ColDidResult = "col_DidResult";

        private readonly Timer _statusTimer = new Timer { Interval = 80 };
        private readonly Queue<int> _batchRowIndices = new Queue<int>();
        private ulong _lastTesterPresentUs;
        private Control[] _busyLockControls;
        private bool _loadingConfig;
        private int _batchTotal;
        private int _batchCurrentRow = -1;
        private int _pendingWriteRow = -1;
        private byte[] _pendingWriteData;

        public SubWin_DiagView()
        {
            InitializeComponent();
            comboBox_DiagFrameType.SelectedIndex = 0;
            comboBox_DiagPadding.SelectedIndex = 0;
            comboBox_DiagSession.SelectedIndex = 2;
            comboBox_DidWriteFmt.SelectedIndex = 0;

            InitDidGrid();
            bool hasSavedConfig = File.Exists(UdsDiagConfigStore.FilePath);
            LoadConfigToUi();
            if (!hasSavedConfig)
                SyncFrameTypeFromDevice();

            _busyLockControls = new Control[]
            {
                Btn_SendDiagReq, Btn_EnterSession, Btn_ReadDid, Btn_WriteDid, Btn_ReadDtc, Btn_ClearDtc, Btn_EcuReset,
                Btn_DidAdd, Btn_DidRemove, Btn_DidReadAll, Btn_DidWriteSelected,
                Btn_RespIdPlus8, Btn_RespIdPlus80,
                textBox_UdsReqID, textBox_DiagRespID, comboBox_DiagFrameType, comboBox_DiagPadding,
                numericUpDown_DiagTimeout, comboBox_DiagSession, textBox_DiagDid,
                comboBox_DidWriteFmt, textBox_DiagWriteData
            };

            textBox_UdsReqID.Leave += (_, _) => PersistConfig();
            textBox_DiagRespID.Leave += (_, _) => PersistConfig();
            comboBox_DiagFrameType.SelectedIndexChanged += (_, _) => PersistConfig();
            comboBox_DiagPadding.SelectedIndexChanged += (_, _) => PersistConfig();
            comboBox_DiagSession.SelectedIndexChanged += (_, _) => PersistConfig();
            numericUpDown_DiagTimeout.ValueChanged += (_, _) => PersistConfig();
            numericUpDown_TpPeriod.ValueChanged += (_, _) => PersistConfig();

            _statusTimer.Tick += StatusTimer_Tick;
            _statusTimer.Start();
            FormClosing += (_, _) => PersistConfig();
            FormClosed += (_, _) =>
            {
                _statusTimer.Stop();
                _statusTimer.Dispose();
                DeviceInterfaceMng.GetInstance()?.udsDiagSession.Cancel();
            };
        }

        private void InitDidGrid()
        {
            dataGridView_Did.AutoGenerateColumns = false;
            dataGridView_Did.AllowUserToResizeRows = false;
            dataGridView_Did.RowTemplate.Height = 24;
            dataGridView_Did.Columns.Clear();

            var colEn = new DataGridViewCheckBoxColumn
            {
                Name = ColDidEnabled,
                HeaderText = "读",
                Width = 36,
                TrueValue = true,
                FalseValue = false
            };
            var colDid = new DataGridViewTextBoxColumn
            {
                Name = ColDid,
                HeaderText = "DID",
                Width = 72,
                MaxInputLength = 6
            };
            colDid.DefaultCellStyle.Font = new Font("Consolas", 9F);
            var colName = new DataGridViewTextBoxColumn
            {
                Name = ColDidName,
                HeaderText = "名称",
                Width = 160,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                MaxInputLength = 64
            };
            var colRaw = new DataGridViewTextBoxColumn
            {
                Name = ColDidRaw,
                HeaderText = "原始值",
                Width = 200,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 50
            };
            colRaw.DefaultCellStyle.Font = new Font("Consolas", 9F);
            var colAscii = new DataGridViewTextBoxColumn
            {
                Name = ColDidAscii,
                HeaderText = "ASCII",
                Width = 140,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 30
            };
            var colResult = new DataGridViewTextBoxColumn
            {
                Name = ColDidResult,
                HeaderText = "结果",
                Width = 140,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 28
            };

            dataGridView_Did.Columns.AddRange(colEn, colDid, colName, colRaw, colAscii, colResult);
            dataGridView_Did.CurrentCellDirtyStateChanged += dataGridView_Did_CurrentCellDirtyStateChanged;
            dataGridView_Did.CellEndEdit += (_, _) => PersistConfig();
            dataGridView_Did.CellValueChanged += dataGridView_Did_CellValueChanged;
            dataGridView_Did.SelectionChanged += dataGridView_Did_SelectionChanged;
        }

        private void dataGridView_Did_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView_Did.IsCurrentCellDirty
                && dataGridView_Did.CurrentCell is DataGridViewCheckBoxCell)
            {
                dataGridView_Did.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView_Did_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_loadingConfig || e.RowIndex < 0)
                return;
            string name = dataGridView_Did.Columns[e.ColumnIndex].Name;
            if (name is ColDidEnabled or ColDid or ColDidName)
                PersistConfig();
        }

        private void LoadConfigToUi()
        {
            _loadingConfig = true;
            try
            {
                var cfg = UdsDiagConfigStore.LoadOrDefault();
                if (!string.IsNullOrWhiteSpace(cfg.ReqId))
                    textBox_UdsReqID.Text = cfg.ReqId;
                if (!string.IsNullOrWhiteSpace(cfg.RespId))
                    textBox_DiagRespID.Text = cfg.RespId;
                SelectComboIndex(comboBox_DiagFrameType, cfg.FrameTypeIndex);
                SelectComboIndex(comboBox_DiagPadding, cfg.PaddingIndex);
                SelectComboIndex(comboBox_DiagSession, cfg.SessionIndex);
                SetNumeric(numericUpDown_DiagTimeout, cfg.TimeoutMs);
                SetNumeric(numericUpDown_TpPeriod, cfg.TpPeriodMs);
                checkBox_TesterPresent.Checked = cfg.TesterPresent;
                FillDidGrid(cfg.Dids);
            }
            finally
            {
                _loadingConfig = false;
            }
        }

        private void FillDidGrid(List<UdsDidItem> dids)
        {
            dataGridView_Did.Rows.Clear();
            if (dids is null)
                return;
            foreach (var item in dids)
            {
                dataGridView_Did.Rows.Add(
                    item.Enabled,
                    NormalizeDidText(item.Did),
                    item.Name ?? string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty);
            }
        }

        private void PersistConfig()
        {
            if (_loadingConfig || IsDisposed)
                return;
            dataGridView_Did.EndEdit();
            UdsDiagConfigStore.TrySave(CollectConfigFromUi());
        }

        private UdsDiagConfig CollectConfigFromUi()
        {
            var cfg = new UdsDiagConfig
            {
                ReqId = textBox_UdsReqID.Text?.Trim() ?? string.Empty,
                RespId = textBox_DiagRespID.Text?.Trim() ?? string.Empty,
                FrameTypeIndex = Math.Max(0, comboBox_DiagFrameType.SelectedIndex),
                PaddingIndex = Math.Max(0, comboBox_DiagPadding.SelectedIndex),
                TimeoutMs = (int)numericUpDown_DiagTimeout.Value,
                SessionIndex = Math.Max(0, comboBox_DiagSession.SelectedIndex),
                TesterPresent = checkBox_TesterPresent.Checked,
                TpPeriodMs = (int)numericUpDown_TpPeriod.Value
            };
            foreach (DataGridViewRow row in dataGridView_Did.Rows)
            {
                if (row.IsNewRow)
                    continue;
                cfg.Dids.Add(new UdsDidItem
                {
                    Enabled = IsDidRowEnabled(row),
                    Did = Convert.ToString(row.Cells[ColDid].Value)?.Trim() ?? string.Empty,
                    Name = Convert.ToString(row.Cells[ColDidName].Value)?.Trim() ?? string.Empty
                });
            }
            return cfg;
        }

        private static void SelectComboIndex(ComboBox combo, int index)
        {
            if (index >= 0 && index < combo.Items.Count)
                combo.SelectedIndex = index;
        }

        private static void SetNumeric(NumericUpDown control, int value)
        {
            decimal v = value;
            if (v < control.Minimum)
                v = control.Minimum;
            if (v > control.Maximum)
                v = control.Maximum;
            control.Value = v;
        }

        private static string NormalizeDidText(string did)
        {
            if (string.IsNullOrWhiteSpace(did))
                return string.Empty;
            string t = did.Trim();
            if (t.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                t = t.Substring(2);
            return t.ToUpperInvariant();
        }

        private static bool IsDidRowEnabled(DataGridViewRow row)
        {
            object v = row.Cells[ColDidEnabled].Value;
            if (v is bool b)
                return b;
            return v is not null && Convert.ToBoolean(v);
        }

        private void Btn_DidAdd_Click(object sender, EventArgs e)
        {
            string did = NormalizeDidText(textBox_DiagDid.Text);
            if (did.Length > 0
                && (!UdsServiceDecode.TryParseHexId(did, out uint id, out _) || id > 0xFFFF))
            {
                did = string.Empty;
            }

            int index = dataGridView_Did.Rows.Add(true, did, string.Empty, string.Empty, string.Empty, string.Empty);
            dataGridView_Did.ClearSelection();
            dataGridView_Did.Rows[index].Selected = true;
            dataGridView_Did.CurrentCell = dataGridView_Did.Rows[index].Cells[string.IsNullOrEmpty(did) ? ColDid : ColDidName];
            PersistConfig();
        }

        private void Btn_DidRemove_Click(object sender, EventArgs e)
        {
            if (dataGridView_Did.CurrentRow is null || dataGridView_Did.CurrentRow.IsNewRow)
            {
                ShowWarn("请先选中要删除的 DID 行。");
                return;
            }
            dataGridView_Did.Rows.Remove(dataGridView_Did.CurrentRow);
            PersistConfig();
        }

        private void Btn_DidReadAll_Click(object sender, EventArgs e)
        {
            var session = DeviceInterfaceMng.GetInstance()?.udsDiagSession;
            if (session is not null && session.IsBusy)
            {
                ShowWarn("上一次诊断请求尚未结束。");
                return;
            }

            dataGridView_Did.EndEdit();
            _batchRowIndices.Clear();
            _batchCurrentRow = -1;

            var indices = new List<int>();
            for (int i = 0; i < dataGridView_Did.Rows.Count; i++)
            {
                var row = dataGridView_Did.Rows[i];
                if (row.IsNewRow || !IsDidRowEnabled(row))
                    continue;
                string didText = Convert.ToString(row.Cells[ColDid].Value);
                if (string.IsNullOrWhiteSpace(didText))
                {
                    SetDidRowResult(i, string.Empty, string.Empty, "DID 为空");
                    continue;
                }
                indices.Add(i);
                SetDidRowResult(i, string.Empty, string.Empty, "等待…");
            }

            if (indices.Count == 0)
            {
                ShowWarn("没有勾选有效 DID。请勾选表格中要读取的行。");
                return;
            }

            foreach (int i in indices)
                _batchRowIndices.Enqueue(i);
            _batchTotal = indices.Count;
            SendNextBatchItem();
        }

        private void SendNextBatchItem()
        {
            while (_batchRowIndices.Count > 0)
            {
                int rowIndex = _batchRowIndices.Dequeue();
                if (rowIndex < 0 || rowIndex >= dataGridView_Did.Rows.Count)
                    continue;

                string didText = Convert.ToString(dataGridView_Did.Rows[rowIndex].Cells[ColDid].Value);
                if (!UdsServiceDecode.TryParseHexId(didText, out uint did, out string err) || did > 0xFFFF)
                {
                    SetDidRowResult(rowIndex, string.Empty, string.Empty, "无效 DID" + (string.IsNullOrEmpty(err) ? string.Empty : "：" + err));
                    continue;
                }

                int done = _batchTotal - _batchRowIndices.Count;
                _batchCurrentRow = rowIndex;
                SetDidRowResult(rowIndex, string.Empty, string.Empty, "读取中…");
                textBox_DiagDid.Text = did.ToString("X4");

                if (!TrySendPayload(new byte[] { 0x22, (byte)(did >> 8), (byte)did }, out string sendErr))
                {
                    SetDidRowResult(rowIndex, string.Empty, string.Empty, sendErr);
                    _batchCurrentRow = -1;
                    if (IsTransportError(sendErr))
                    {
                        AbortRemainingBatch(sendErr);
                        ShowWarn(sendErr);
                        SetBusy(false);
                        SetStatus(sendErr, true);
                        return;
                    }
                    continue;
                }

                SetStatus("批量读 " + done + "/" + _batchTotal + "：0x" + did.ToString("X4"), false);
                return;
            }

            _batchCurrentRow = -1;
            SetBusy(false);
            SetStatus("批量读完成", false);
        }

        private void AbortRemainingBatch(string reason)
        {
            while (_batchRowIndices.Count > 0)
            {
                int rowIndex = _batchRowIndices.Dequeue();
                if (rowIndex >= 0 && rowIndex < dataGridView_Did.Rows.Count)
                    SetDidRowResult(rowIndex, string.Empty, string.Empty, reason);
            }
            _batchCurrentRow = -1;
        }

        private static bool IsTransportError(string error)
        {
            if (string.IsNullOrEmpty(error))
                return false;
            return error.Contains("请先连接") || error.Contains("经典 CAN") || error.Contains("尚未结束");
        }

        private void SetDidRowResult(int rowIndex, string raw, string ascii, string result)
        {
            if (rowIndex < 0 || rowIndex >= dataGridView_Did.Rows.Count)
                return;
            var row = dataGridView_Did.Rows[rowIndex];
            row.Cells[ColDidRaw].Value = raw ?? string.Empty;
            row.Cells[ColDidAscii].Value = ascii ?? string.Empty;
            row.Cells[ColDidResult].Value = result ?? string.Empty;
        }

        private void ApplyDidReadResult(int rowIndex, UdsDiagSession.Result result)
        {
            SplitDidResult(result, out string raw, out string ascii, out string outcome);
            SetDidRowResult(rowIndex, raw, ascii, outcome);
        }

        private static void SplitDidResult(UdsDiagSession.Result result, out string raw, out string ascii, out string outcome)
        {
            raw = string.Empty;
            ascii = string.Empty;
            byte[] payload = result?.Payload;
            if (result is not null && result.Ok && payload is { Length: >= 3 } && payload[0] == 0x62)
            {
                int n = payload.Length - 3;
                byte[] data = new byte[n];
                if (n > 0)
                    Array.Copy(payload, 3, data, 0, n);
                raw = UdsServiceDecode.FormatHex(data);
                ascii = UdsServiceDecode.TryAscii(data) ?? string.Empty;
                outcome = "成功";
                return;
            }
            if (payload is { Length: >= 3 } && payload[0] == UdsServiceDecode.NegativeSid)
            {
                outcome = UdsServiceDecode.NrcName(payload[2]);
                return;
            }
            outcome = result is null
                ? "失败"
                : (result.Ok ? (string.IsNullOrEmpty(result.Decode) ? "成功" : result.Decode) : result.Status);
        }

        private bool IsBatchRunning => _batchCurrentRow >= 0 || _batchRowIndices.Count > 0;

        private void SyncFrameTypeFromDevice()
        {
            var device = DeviceInterfaceMng.GetInstance();
            if (device is null || !device.canDeviceOpenFlag)
                return;
            comboBox_DiagFrameType.SelectedIndex = device.curCanFrameType == CanFrameType.CAN ? 1 : 0;
        }

        private void Btn_SendDiagReq_Click(object sender, EventArgs e) => SendCurrentRequest();

        private void Btn_EnterSession_Click(object sender, EventArgs e)
        {
            byte session = comboBox_DiagSession.SelectedIndex switch
            {
                1 => 0x02,
                2 => 0x03,
                _ => 0x01
            };
            FillAndSend(new byte[] { 0x10, session });
        }

        private void Btn_RespIdPlus8_Click(object sender, EventArgs e) => ApplyRespIdOffset(0x08);

        private void Btn_RespIdPlus80_Click(object sender, EventArgs e) => ApplyRespIdOffset(0x80);

        private void ApplyRespIdOffset(uint offset)
        {
            if (!UdsServiceDecode.TryParseHexId(textBox_UdsReqID.Text, out uint reqId, out string err))
            {
                ShowWarn("请求ID：" + err);
                return;
            }

            ulong sum = (ulong)reqId + offset;
            if (sum > 0x1FFFFFFF)
            {
                ShowWarn("请求ID + 偏移超出 29 位 CAN ID 范围。");
                return;
            }

            textBox_DiagRespID.Text = FormatCanId((uint)sum, textBox_UdsReqID.Text);
            PersistConfig();
        }

        private static string FormatCanId(uint id, string sample)
        {
            string src = sample?.Trim() ?? string.Empty;
            bool prefix = src.StartsWith("0x", StringComparison.OrdinalIgnoreCase);
            string hex = id <= 0x7FF ? id.ToString("X3") : id.ToString("X8");
            return prefix || src.Length == 0 ? "0x" + hex : hex;
        }

        private void Btn_ReadDid_Click(object sender, EventArgs e)
        {
            string didText = (textBox_DiagDid.Text ?? string.Empty).Replace(" ", "");
            if (!UdsServiceDecode.TryParseHexId(didText, out uint did, out string err) || did > 0xFFFF)
            {
                ShowWarn("DID：" + (err ?? "需为 16 位十六进制，如 F187"));
                return;
            }
            FillAndSend(new byte[] { 0x22, (byte)(did >> 8), (byte)did });
        }

        private void Btn_WriteDid_Click(object sender, EventArgs e)
        {
            if (!TryParseDidFromBox(out uint did, out string err))
            {
                ShowWarn("DID：" + err);
                return;
            }
            StartWriteDid(did, FindDidRow(did));
        }

        private void Btn_DidWriteSelected_Click(object sender, EventArgs e)
        {
            dataGridView_Did.EndEdit();
            if (dataGridView_Did.CurrentRow is null || dataGridView_Did.CurrentRow.IsNewRow)
            {
                ShowWarn("请先选中要写入的 DID 行。");
                return;
            }

            int rowIndex = dataGridView_Did.CurrentRow.Index;
            string didText = Convert.ToString(dataGridView_Did.Rows[rowIndex].Cells[ColDid].Value);
            if (!UdsServiceDecode.TryParseHexId(didText, out uint did, out string err) || did > 0xFFFF)
            {
                ShowWarn("选中行 DID：" + (err ?? "需为 16 位十六进制，如 F187"));
                return;
            }

            textBox_DiagDid.Text = did.ToString("X4");
            StartWriteDid(did, rowIndex);
        }

        private bool TryParseDidFromBox(out uint did, out string error)
        {
            string didText = (textBox_DiagDid.Text ?? string.Empty).Replace(" ", "");
            if (!UdsServiceDecode.TryParseHexId(didText, out did, out error) || did > 0xFFFF)
            {
                did = 0;
                error ??= "需为 16 位十六进制，如 F187";
                return false;
            }
            return true;
        }

        private int FindDidRow(uint did)
        {
            if (dataGridView_Did.CurrentRow is { IsNewRow: false } current)
            {
                string curText = Convert.ToString(current.Cells[ColDid].Value);
                if (UdsServiceDecode.TryParseHexId(curText, out uint curDid, out _) && curDid == did)
                    return current.Index;
            }

            for (int i = 0; i < dataGridView_Did.Rows.Count; i++)
            {
                var row = dataGridView_Did.Rows[i];
                if (row.IsNewRow)
                    continue;
                string text = Convert.ToString(row.Cells[ColDid].Value);
                if (UdsServiceDecode.TryParseHexId(text, out uint rowDid, out _) && rowDid == did)
                    return i;
            }
            return -1;
        }

        private void StartWriteDid(uint did, int rowIndex)
        {
            var session = DeviceInterfaceMng.GetInstance()?.udsDiagSession;
            if (session is not null && session.IsBusy)
            {
                ShowWarn("上一次诊断请求尚未结束。");
                return;
            }

            if (!TryParseWriteData(out byte[] data, out string parseErr))
            {
                ShowWarn(parseErr);
                return;
            }
            if (data.Length == 0)
            {
                ShowWarn("写入数据为空。");
                return;
            }
            if (data.Length + 3 > IsoTpTool.MaxPduLength)
            {
                ShowWarn("写入数据超过 ISO-TP 长度上限。");
                return;
            }

            int prevLen = CountHexBytes(rowIndex >= 0
                ? Convert.ToString(dataGridView_Did.Rows[rowIndex].Cells[ColDidRaw].Value)
                : null);
            string ascii = UdsServiceDecode.TryAscii(data);
            var sb = new StringBuilder();
            sb.Append("确认向 DID 0x").Append(did.ToString("X4")).Append(" 写入 ")
                .Append(data.Length).Append(" 字节？").AppendLine().AppendLine()
                .Append(UdsServiceDecode.FormatHex(data));
            if (!string.IsNullOrEmpty(ascii))
                sb.AppendLine().Append("ASCII: \"").Append(ascii).Append('"');
            if (prevLen > 0 && prevLen != data.Length)
            {
                sb.AppendLine().AppendLine()
                    .Append("注意：该行上次读取为 ").Append(prevLen)
                    .Append(" 字节，本次写入 ").Append(data.Length).Append(" 字节。");
            }

            if (MessageBox.Show(this, sb.ToString(), "写 DID 确认",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                != DialogResult.Yes)
                return;

            var payload = new byte[3 + data.Length];
            payload[0] = 0x2E;
            payload[1] = (byte)(did >> 8);
            payload[2] = (byte)did;
            Array.Copy(data, 0, payload, 3, data.Length);

            _pendingWriteRow = rowIndex;
            _pendingWriteData = data;
            if (rowIndex >= 0)
                dataGridView_Did.Rows[rowIndex].Cells[ColDidResult].Value = "写入中…";

            if (!TrySendPayload(payload, out string sendErr))
            {
                _pendingWriteRow = -1;
                _pendingWriteData = null;
                if (rowIndex >= 0)
                    dataGridView_Did.Rows[rowIndex].Cells[ColDidResult].Value = sendErr;
                ShowWarn(sendErr);
            }
        }

        private bool TryParseWriteData(out byte[] data, out string error)
        {
            data = Array.Empty<byte>();
            error = null;
            string text = textBox_DiagWriteData.Text ?? string.Empty;
            if (comboBox_DidWriteFmt.SelectedIndex == 1)
            {
                if (text.Length == 0)
                {
                    error = "写入数据为空（ASCII）。";
                    return false;
                }
                var bytes = new List<byte>(text.Length);
                foreach (char c in text)
                {
                    if (c > 0x7F)
                    {
                        error = "ASCII 模式仅支持 0x00–0x7F，含非 ASCII：" + c;
                        return false;
                    }
                    bytes.Add((byte)c);
                }
                data = bytes.ToArray();
                return true;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                error = "写入数据为空（十六进制，如 31 32 33 或 313233）。";
                return false;
            }
            return UdsServiceDecode.TryParseHexBytes(text, out data, out error);
        }

        private static int CountHexBytes(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                return 0;
            return UdsServiceDecode.TryParseHexBytes(hex, out byte[] data, out _) ? data.Length : 0;
        }

        private void dataGridView_Did_SelectionChanged(object sender, EventArgs e)
        {
            if (_loadingConfig || dataGridView_Did.CurrentRow is null || dataGridView_Did.CurrentRow.IsNewRow)
                return;

            var row = dataGridView_Did.CurrentRow;
            string didText = NormalizeDidText(Convert.ToString(row.Cells[ColDid].Value));
            if (didText.Length > 0)
                textBox_DiagDid.Text = didText;

            if (!string.IsNullOrWhiteSpace(textBox_DiagWriteData.Text))
                return;

            string ascii = Convert.ToString(row.Cells[ColDidAscii].Value);
            string raw = Convert.ToString(row.Cells[ColDidRaw].Value);
            if (!string.IsNullOrWhiteSpace(ascii))
            {
                comboBox_DidWriteFmt.SelectedIndex = 1;
                textBox_DiagWriteData.Text = ascii;
            }
            else if (!string.IsNullOrWhiteSpace(raw))
            {
                comboBox_DidWriteFmt.SelectedIndex = 0;
                textBox_DiagWriteData.Text = raw;
            }
        }

        private void ApplyDidWriteResult(int rowIndex, UdsDiagSession.Result result)
        {
            string outcome = DescribeWriteOutcome(result);
            if (rowIndex < 0 || rowIndex >= dataGridView_Did.Rows.Count)
                return;

            var row = dataGridView_Did.Rows[rowIndex];
            row.Cells[ColDidResult].Value = outcome;
            if (result is not null && result.Ok && result.Payload is { Length: >= 1 } && result.Payload[0] == 0x6E
                && _pendingWriteData is not null)
            {
                row.Cells[ColDidRaw].Value = UdsServiceDecode.FormatHex(_pendingWriteData);
                row.Cells[ColDidAscii].Value = UdsServiceDecode.TryAscii(_pendingWriteData) ?? string.Empty;
            }
        }

        private static string DescribeWriteOutcome(UdsDiagSession.Result result)
        {
            byte[] payload = result?.Payload;
            if (result is not null && result.Ok && payload is { Length: >= 1 } && payload[0] == 0x6E)
                return "写入成功";
            if (payload is { Length: >= 3 } && payload[0] == UdsServiceDecode.NegativeSid)
            {
                string nrc = UdsServiceDecode.NrcName(payload[2]);
                if (payload[2] == 0x33)
                    return nrc + "（需先安全访问 0x27）";
                return nrc;
            }
            return result is null ? "失败" : (result.Ok ? "写入成功" : result.Status);
        }

        private void Btn_ReadDtc_Click(object sender, EventArgs e)
        {
            FillAndSend(new byte[] { 0x19, 0x02, 0xFF });
        }

        private void Btn_ClearDtc_Click(object sender, EventArgs e)
        {
            FillAndSend(new byte[] { 0x14, 0xFF, 0xFF, 0xFF });
        }

        private void Btn_EcuReset_Click(object sender, EventArgs e)
        {
            FillAndSend(new byte[] { 0x11, 0x01 });
        }

        private void FillAndSend(byte[] payload)
        {
            if (!TrySendPayload(payload, out string error))
                ShowWarn(error);
        }

        private void SendCurrentRequest()
        {
            if (!TrySendPayloadFromBox(out string error))
                ShowWarn(error);
        }

        private bool TrySendPayload(byte[] payload, out string error)
        {
            textBox_DiagReqData.Text = UdsServiceDecode.FormatHex(payload);
            return TrySendPayloadFromBox(out error);
        }

        private bool TrySendPayloadFromBox(out string error)
        {
            if (!TryBuildRequest(out var request, out error))
                return false;

            var device = DeviceInterfaceMng.GetInstance();
            textBox_DiagRespData.Clear();
            textBox_DiagDecode.Clear();
            textBox_DiagTrace.Clear();

            if (!device.udsDiagSession.TryStart(request, device, out error))
                return false;

            SetBusy(true);
            SetStatus("正在发送…", false);
            AppLogMng.DisplayLog("已发送 UDS 请求 " + UdsServiceDecode.FormatHex(request.Payload), true);
            return true;
        }

        private bool TryBuildTransport(out uint reqId, out uint respId, out bool isCanFd, out string error)
        {
            reqId = 0;
            respId = 0;
            isCanFd = true;
            var device = DeviceInterfaceMng.GetInstance();
            if (device is null || !device.canDeviceOpenFlag)
            {
                error = "请先连接 CAN 设备。";
                return false;
            }
            if (!UdsServiceDecode.TryParseHexId(textBox_UdsReqID.Text, out reqId, out error))
            {
                error = "请求ID：" + error;
                return false;
            }
            if (!UdsServiceDecode.TryParseHexId(textBox_DiagRespID.Text, out respId, out error))
            {
                error = "响应ID：" + error;
                return false;
            }

            isCanFd = comboBox_DiagFrameType.SelectedIndex != 1;
            if (device.curCanFrameType == CanFrameType.CAN && isCanFd)
            {
                error = "当前设备以经典 CAN 打开，请将帧类型改为 CAN，或断开后以 CAN FD 重连。";
                return false;
            }

            error = null;
            return true;
        }

        private bool TryBuildRequest(out UdsDiagSession.Request request, out string error)
        {
            request = null;
            if (!TryBuildTransport(out uint reqId, out uint respId, out bool isCanFd, out error))
                return false;
            if (!UdsServiceDecode.TryParseHexBytes(textBox_DiagReqData.Text, out byte[] payload, out error))
                return false;

            request = new UdsDiagSession.Request
            {
                ReqId = reqId,
                RespId = respId,
                IsCanFd = isCanFd,
                MaxDlc = 8,
                PadByte = ReadPadByte(),
                TimeoutMs = (int)numericUpDown_DiagTimeout.Value,
                Payload = payload
            };
            error = null;
            return true;
        }

        private void Btn_CancelDiag_Click(object sender, EventArgs e)
        {
            AbortRemainingBatch("已取消");
            if (_pendingWriteRow >= 0 && _pendingWriteRow < dataGridView_Did.Rows.Count)
                dataGridView_Did.Rows[_pendingWriteRow].Cells[ColDidResult].Value = "已取消";
            _pendingWriteRow = -1;
            _pendingWriteData = null;
            DeviceInterfaceMng.GetInstance()?.udsDiagSession.Cancel();
        }

        private void checkBox_TesterPresent_CheckedChanged(object sender, EventArgs e)
        {
            _lastTesterPresentUs = 0;
            if (checkBox_TesterPresent.Checked)
                SendTesterPresent(force: true);
            PersistConfig();
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            if (!IsBatchRunning)
                SendTesterPresent(force: false);

            var session = DeviceInterfaceMng.GetInstance()?.udsDiagSession;
            if (session is null)
                return;

            bool busy = session.IsBusy;
            if (!IsBatchRunning)
                SetBusy(busy);
            else
                SetBusy(true);

            if (busy)
                SetStatus(session.StatusText, false);

            if (!session.TryTakeResult(out var result))
                return;

            textBox_DiagRespData.Text = UdsServiceDecode.FormatHex(result.Payload);
            textBox_DiagDecode.Text = result.Decode;
            textBox_DiagTrace.Text = result.Trace;
            AppendHistory(result);
            AppLogMng.DisplayLog(result.Ok ? "诊断响应成功" : "诊断结束: " + result.Status, result.Ok);

            if (_pendingWriteRow >= 0 || _pendingWriteData is not null)
            {
                if (_pendingWriteRow >= 0)
                    ApplyDidWriteResult(_pendingWriteRow, result);
                _pendingWriteRow = -1;
                _pendingWriteData = null;
                SetStatus(result.Status, !result.Ok);
                SetBusy(false);
                return;
            }

            if (_batchCurrentRow >= 0)
            {
                ApplyDidReadResult(_batchCurrentRow, result);
                _batchCurrentRow = -1;
                SendNextBatchItem();
                return;
            }

            SetStatus(result.Status, !result.Ok);
            SetBusy(false);
        }

        private void SendTesterPresent(bool force)
        {
            if (!checkBox_TesterPresent.Checked)
                return;

            var device = DeviceInterfaceMng.GetInstance();
            if (device is null || !device.canDeviceOpenFlag)
                return;
            if (device.udsDiagSession.IsBusy || IsBatchRunning)
                return;

            ulong now = TimerTool.GetSysTime();
            ulong period = (ulong)numericUpDown_TpPeriod.Value * (ulong)TimeUnit.T_MS;
            if (!force && _lastTesterPresentUs != 0 && now - _lastTesterPresentUs < period)
                return;

            if (!TryBuildTransport(out uint reqId, out uint respId, out bool isCanFd, out _))
                return;

            var request = new UdsDiagSession.Request
            {
                ReqId = reqId,
                RespId = respId,
                IsCanFd = isCanFd,
                MaxDlc = 8,
                PadByte = ReadPadByte(),
                TimeoutMs = (int)numericUpDown_DiagTimeout.Value,
                Payload = new byte[] { 0x3E, 0x80 }
            };
            if (!device.udsDiagSession.TrySendNoWait(request, device, out _))
                return;

            _lastTesterPresentUs = now;
        }

        private void AppendHistory(UdsDiagSession.Result result)
        {
            var item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
            item.SubItems.Add(UdsServiceDecode.FormatHex(result.RequestPayload));
            item.SubItems.Add(UdsServiceDecode.FormatHex(result.Payload));
            item.SubItems.Add(result.Ok ? result.Decode : result.Status);
            item.Tag = UdsServiceDecode.FormatHex(result.RequestPayload);
            item.ForeColor = result.Ok ? Color.FromArgb(21, 87, 36) : Color.FromArgb(153, 27, 27);
            listView_History.Items.Insert(0, item);
            while (listView_History.Items.Count > 80)
                listView_History.Items.RemoveAt(listView_History.Items.Count - 1);
        }

        private void listView_History_DoubleClick(object sender, EventArgs e)
        {
            if (listView_History.SelectedItems.Count == 0)
                return;
            if (listView_History.SelectedItems[0].Tag is string hex && hex.Length > 0)
                textBox_DiagReqData.Text = hex;
        }

        private byte? ReadPadByte()
        {
            return comboBox_DiagPadding.SelectedIndex switch
            {
                0 => (byte)0x00,
                1 => (byte)0x55,
                2 => (byte)0xAA,
                _ => null
            };
        }

        private void SetBusy(bool busy)
        {
            Btn_CancelDiag.Enabled = busy;
            textBox_DiagReqData.ReadOnly = busy;
            dataGridView_Did.ReadOnly = busy;
            foreach (Control c in _busyLockControls)
                c.Enabled = !busy;
        }

        private void SetStatus(string text, bool isError)
        {
            label_DiagStatus.Text = "状态：" + (text ?? string.Empty);
            label_DiagStatus.ForeColor = isError
                ? Color.FromArgb(153, 27, 27)
                : Color.FromArgb(71, 85, 105);
        }

        private void ShowWarn(string message)
        {
            MessageBox.Show(this, message, "诊断", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}

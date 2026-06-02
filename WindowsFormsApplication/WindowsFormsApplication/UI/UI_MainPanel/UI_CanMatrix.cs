using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication.UI
{
    public partial class UI_CanMatrix : UserControl
    {
        private static readonly Color FrameColorWhite = Color.White;
        private static readonly Color FrameColorBlue = Color.FromArgb(232, 241, 255);
        private static readonly Color CellForeColor = Color.FromArgb(31, 41, 55);
        private static readonly Color MsgColumnForeColor = Color.FromArgb(30, 64, 120);
        private static readonly Color FrameSeparatorColor = Color.FromArgb(165, 190, 230);

        private readonly Font _msgColumnFont;
        private readonly Font _sigNameFont;
        private readonly DataGridViewCellStyle _frameStyleWhite;
        private readonly DataGridViewCellStyle _frameStyleBlue;
        private readonly Pen _frameSeparatorPen;

        private bool _columnsConfigured;
        private int _builtForDbcFingerprint = -1;

        public UI_CanMatrix()
        {
            InitializeComponent();
            _msgColumnFont = new Font(MsgGridView.Font, FontStyle.Bold);
            _sigNameFont = new Font(MsgGridView.Font, FontStyle.Bold);
            _frameStyleWhite = CreateFrameRowStyle(FrameColorWhite, Color.FromArgb(214, 226, 248));
            _frameStyleBlue = CreateFrameRowStyle(FrameColorBlue, Color.FromArgb(188, 210, 245));
            _frameSeparatorPen = new Pen(FrameSeparatorColor, 2f);

            EnableGridDoubleBuffer(MsgGridView);
            MsgGridView.RowPostPaint += MsgGridView_RowPostPaint;
            MsgGridView.DataBindingComplete += MsgGridView_DataBindingComplete;
            Disposed += (_, _) =>
            {
                _msgColumnFont.Dispose();
                _sigNameFont.Dispose();
                _frameSeparatorPen.Dispose();
            };
        }

        public void UpdateMsgTableView()
        {
            if (CanDbcDataManager.GetInstance().isLoadCfg != true)
            {
                MsgGridView.DataSource = null;
                _builtForDbcFingerprint = -1;
                return;
            }

            int fingerprint = GetDbcFingerprint();
            if (fingerprint == _builtForDbcFingerprint && MsgGridView.DataSource != null)
                return;

            DataTable dt = BuildMatrixDataTable();
            MsgGridView.SuspendLayout();
            try
            {
                MsgGridView.DataSource = dt;
                _builtForDbcFingerprint = fingerprint;
            }
            finally
            {
                MsgGridView.ResumeLayout();
            }
        }

        public void InvalidateMatrixCache()
        {
            _builtForDbcFingerprint = -1;
        }

        private static int GetDbcFingerprint()
        {
            var manager = CanDbcDataManager.GetInstance();
            if (manager?.isLoadCfg != true || manager.canMsgSet is null)
                return -1;

            int fingerprint = manager.canMsgSet.Count;
            foreach (var msgId in manager.canMsgSet.Keys)
                fingerprint = unchecked(fingerprint * 31 + msgId.GetHashCode());
            return fingerprint;
        }

        private static DataTable BuildMatrixDataTable()
        {
            var manager = CanDbcDataManager.GetInstance();
            int estimatedRows = 0;
            foreach (var msg in manager.canMsgSet.Values)
                estimatedRows += msg.signals?.Count ?? 0;

            DataTable dt = new DataTable();
            dt.Columns.Add("MsgID");
            dt.Columns.Add("MsgName");
            dt.Columns.Add("SigName");
            dt.Columns.Add("SigDesc");
            dt.Columns.Add("StartBit");
            dt.Columns.Add("SigLen");
            dt.Columns.Add("Factor");
            dt.Columns.Add("Offset");
            dt.Columns.Add("SigValue");
            dt.Columns.Add("ValueType");
            dt.Columns.Add("SigOrder");
            dt.Columns.Add("MsgSize");
            dt.Columns.Add("MsgCycle");
            dt.Columns.Add("SendNode");
            dt.Columns.Add("RecvNode");
            dt.Columns.Add("MsgFrameType");
            dt.Columns.Add("MsgType");
            dt.Columns.Add("ReuseFrameID");
            dt.MinimumCapacity = Math.Max(estimatedRows, 16);

            var messages = manager.canMsgSet.Values
                .OrderBy(m => m.msgId)
                .ToList();

            foreach (var item in messages)
            {
                string msgIdText = "0x" + item.msgId.ToString("X");
                string msgSizeText = item.msgSize.ToString();
                string msgCycleText = item.msgCycle.ToString();
                string frameType = GetCanFrameType(item.isExtended, item.isCanfd);
                string msgTypeText = item.msgType.ToString();
                string transmitter = item.transmitter;

                foreach (var signal in item.signals)
                {
                    DataRow dr = dt.NewRow();
                    dr["MsgID"] = msgIdText;
                    dr["MsgName"] = item.msgName;
                    dr["SigName"] = signal.sigName;
                    dr["SigDesc"] = signal.sigDesc;
                    dr["StartBit"] = signal.sigStartBit.ToString();
                    dr["SigLen"] = signal.sigLen.ToString();
                    dr["Factor"] = signal.sigFactor.ToString();
                    dr["Offset"] = signal.sigOffset.ToString();
                    dr["SigValue"] = BuildSignalValueText(signal.sigValueTable);
                    dr["ValueType"] = signal.valueType.ToString();
                    dr["SigOrder"] = signal.sigOrderType.ToString();
                    dr["MsgSize"] = msgSizeText;
                    dr["MsgCycle"] = msgCycleText;
                    dr["SendNode"] = transmitter;
                    dr["RecvNode"] = signal.recvNode;
                    dr["MsgFrameType"] = frameType;
                    dr["MsgType"] = msgTypeText;
                    dr["ReuseFrameID"] = signal.reuseFrameID;
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        private static string BuildSignalValueText(System.Collections.Generic.Dictionary<int, string> valueTable)
        {
            if (valueTable == null || valueTable.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();
            foreach (var kv in valueTable)
            {
                if (sb.Length > 0)
                    sb.Append("; ");
                sb.Append(kv.Key).Append(": ").Append(kv.Value);
            }
            return sb.ToString();
        }

        private static string GetCanFrameType(bool isExtended, bool isCanfd)
        {
            if (!isExtended && !isCanfd) return "Standard-CAN";
            if (!isExtended && isCanfd) return "Standard-CANFD";
            if (isExtended && !isCanfd) return "Extended-CAN";
            if (isExtended && isCanfd) return "Extended-CANFD";
            return "Standard-CAN";
        }

        private void MsgGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (!_columnsConfigured)
            {
                ConfigureColumns();
                _columnsConfigured = true;
            }

            ApplyMessageFrameRowStyles();
        }

        private void ConfigureColumns()
        {
            SetColumn("MsgID", "报文 ID", 88, frozen: true, useMsgStyle: true);
            SetColumn("MsgName", "报文名", 140, frozen: true, useMsgStyle: true);
            SetColumn("SigName", "信号名", 120, useSigNameStyle: true);
            SetColumn("SigDesc", "信号描述", 180);
            SetColumn("StartBit", "起始位", 64);
            SetColumn("SigLen", "长度", 52);
            SetColumn("Factor", "Factor", 72);
            SetColumn("Offset", "Offset", 72);
            SetColumn("SigValue", "信号值表", 220);
            SetColumn("ValueType", "值类型", 80);
            SetColumn("SigOrder", "字节序", 72);
            SetColumn("MsgSize", "DLC", 52);
            SetColumn("MsgCycle", "周期(ms)", 72);
            SetColumn("SendNode", "发送节点", 100);
            SetColumn("RecvNode", "接收节点", 100);
            SetColumn("MsgFrameType", "帧类型", 110);
            SetColumn("MsgType", "报文类型", 80);
            SetColumn("ReuseFrameID", "复用帧ID", 80);
        }

        private void SetColumn(string name, string header, int width, bool frozen = false,
            bool useMsgStyle = false, bool useSigNameStyle = false)
        {
            if (!MsgGridView.Columns.Contains(name)) return;

            DataGridViewColumn col = MsgGridView.Columns[name];
            col.HeaderText = header;
            col.Width = width;
            col.MinimumWidth = Math.Min(width, 48);
            col.Frozen = frozen;
            col.SortMode = DataGridViewColumnSortMode.NotSortable;

            if (useMsgStyle)
            {
                col.DefaultCellStyle.Font = _msgColumnFont;
                col.DefaultCellStyle.ForeColor = MsgColumnForeColor;
                col.DefaultCellStyle.SelectionForeColor = MsgColumnForeColor;
            }
            else if (useSigNameStyle)
            {
                col.DefaultCellStyle.Font = _sigNameFont;
            }
        }

        private void ApplyMessageFrameRowStyles()
        {
            int colorIndex = 0;
            string prevMsgId = null;

            foreach (DataGridViewRow row in MsgGridView.Rows)
            {
                if (row.IsNewRow) continue;

                string msgId = row.Cells["MsgID"].Value as string;
                if (prevMsgId != null && !string.Equals(msgId, prevMsgId, StringComparison.Ordinal))
                    colorIndex = 1 - colorIndex;

                prevMsgId = msgId;
                row.DefaultCellStyle = colorIndex == 0 ? _frameStyleWhite : _frameStyleBlue;
            }
        }

        private void MsgGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex >= MsgGridView.Rows.Count - 1) return;

            DataGridViewRow row = MsgGridView.Rows[e.RowIndex];
            DataGridViewRow nextRow = MsgGridView.Rows[e.RowIndex + 1];
            if (row.IsNewRow || nextRow.IsNewRow) return;

            string msgId = row.Cells["MsgID"].Value as string;
            string nextMsgId = nextRow.Cells["MsgID"].Value as string;
            if (string.Equals(msgId, nextMsgId, StringComparison.Ordinal)) return;

            int y = e.RowBounds.Bottom - 1;
            e.Graphics.DrawLine(_frameSeparatorPen, e.RowBounds.Left, y, e.RowBounds.Right, y);
        }

        private DataGridViewCellStyle CreateFrameRowStyle(Color backColor, Color selectionBackColor)
        {
            var style = new DataGridViewCellStyle(MsgGridView.RowsDefaultCellStyle)
            {
                BackColor = backColor,
                ForeColor = CellForeColor,
                SelectionBackColor = selectionBackColor,
                SelectionForeColor = CellForeColor,
                WrapMode = DataGridViewTriState.False
            };
            return style;
        }

        private static void EnableGridDoubleBuffer(DataGridView grid)
        {
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                grid,
                new object[] { true });
        }
    }
}

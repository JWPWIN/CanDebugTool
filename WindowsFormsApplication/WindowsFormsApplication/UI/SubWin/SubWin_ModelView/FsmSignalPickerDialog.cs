using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApplication.ModelViewFsm;

namespace WindowsFormsApplication.UI.SubWin.SubWin_ModelView
{
    public class FsmSignalPickerDialog : Form
    {
        private readonly ListView _listView;
        private readonly TextBox _searchBox;
        private readonly List<(uint MsgId, string SigName, string MsgName)> _allSignals = new();

        public FsmSignalRef? SelectedRef { get; private set; }

        public FsmSignalPickerDialog()
        {
            Text = "选择 CAN 信号";
            Size = new Size(520, 480);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            _searchBox = new TextBox { Dock = DockStyle.Top, PlaceholderText = "搜索信号或报文..." };
            _searchBox.TextChanged += (_, _) => ApplyFilter();

            _listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                MultiSelect = false
            };
            _listView.Columns.Add("报文", 120);
            _listView.Columns.Add("ID", 70);
            _listView.Columns.Add("信号", 180);
            _listView.DoubleClick += (_, _) => ConfirmSelection();

            var panelButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 44,
                Padding = new Padding(8)
            };
            var btnOk = new Button { Text = "确定", DialogResult = DialogResult.OK, Width = 80 };
            var btnCancel = new Button { Text = "取消", DialogResult = DialogResult.Cancel, Width = 80 };
            btnOk.Click += (_, _) => ConfirmSelection();
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Controls.Add(btnOk);

            Controls.Add(_listView);
            Controls.Add(panelButtons);
            Controls.Add(_searchBox);

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            LoadSignals();
        }

        private void LoadSignals()
        {
            _allSignals.Clear();
            var mgr = CanDbcDataManager.GetInstance();
            if (!mgr.isLoadCfg)
            {
                MessageBox.Show("请先通过顶部文件夹图标加载 Excel 通信矩阵。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var msg in mgr.canMsgSet.Values.OrderBy(m => m.msgId))
            {
                foreach (var sig in msg.signals)
                    _allSignals.Add((msg.msgId, sig.sigName, msg.msgName));
            }
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string filter = _searchBox.Text.Trim();
            _listView.BeginUpdate();
            _listView.Items.Clear();
            foreach (var item in _allSignals)
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    bool match = item.SigName.Contains(filter, StringComparison.OrdinalIgnoreCase)
                        || item.MsgName.Contains(filter, StringComparison.OrdinalIgnoreCase)
                        || $"0x{item.MsgId:X}".Contains(filter, StringComparison.OrdinalIgnoreCase);
                    if (!match) continue;
                }

                var lvItem = new ListViewItem(item.MsgName);
                lvItem.SubItems.Add($"0x{item.MsgId:X}");
                lvItem.SubItems.Add(item.SigName);
                lvItem.Tag = new FsmSignalRef { MsgId = item.MsgId, SigName = item.SigName };
                _listView.Items.Add(lvItem);
            }
            _listView.EndUpdate();
        }

        private void ConfirmSelection()
        {
            if (_listView.SelectedItems.Count == 0)
            {
                DialogResult = DialogResult.None;
                return;
            }
            SelectedRef = _listView.SelectedItems[0].Tag as FsmSignalRef;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApplication.ModelViewFsm;

namespace WindowsFormsApplication.UI.SubWin.SubWin_ModelView
{
    public class FsmTransitionEditDialog : Form
    {
        private readonly FsmTransition _transition;
        private readonly List<FsmTriggerCondition> _workingTriggers;
        private readonly List<FsmSignalRef> _workingDisplay;
        private readonly TextBox _txtLabel;
        private readonly ListView _lvTriggers;
        private readonly ListView _lvDisplay;

        public FsmTransitionEditDialog(FsmTransition transition)
        {
            _transition = transition;
            _workingTriggers = CloneTriggers(transition.Triggers);
            _workingDisplay = CloneDisplaySignals(transition.DisplaySignals);

            Text = "编辑状态转移";
            Size = new Size(560, 520);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(480, 400);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 45));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));

            var labelPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
            labelPanel.Controls.Add(new Label { Text = "转移名称:", AutoSize = true, Padding = new Padding(0, 8, 0, 0) });
            _txtLabel = new TextBox { Width = 320, Text = transition.Label ?? string.Empty };
            labelPanel.Controls.Add(_txtLabel);
            layout.Controls.Add(labelPanel, 0, 0);

            layout.Controls.Add(CreateSignalSection("触发条件（全部满足时激活转移）", out _lvTriggers,
                () => AddTrigger(), () => RemoveSelected(_lvTriggers, _workingTriggers)), 0, 1);

            layout.Controls.Add(new Label
            {
                Text = "连线上展示信号",
                Dock = DockStyle.Fill,
                AutoSize = true
            }, 0, 2);

            layout.Controls.Add(CreateSignalSection(null, out _lvDisplay,
                () => AddDisplaySignal(), () => RemoveSelected(_lvDisplay, _workingDisplay)), 0, 3);

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };
            var btnOk = new Button { Text = "确定", DialogResult = DialogResult.OK, Width = 80 };
            var btnCancel = new Button { Text = "取消", DialogResult = DialogResult.Cancel, Width = 80 };
            btnOk.Click += (_, _) => SaveToModel();
            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnOk);
            layout.Controls.Add(btnPanel, 0, 4);

            Controls.Add(layout);
            AcceptButton = btnOk;
            CancelButton = btnCancel;

            RefreshLists();
        }

        private Control CreateSignalSection(string? title, out ListView listView,
            Action onAdd, Action onRemove)
        {
            var panel = new Panel { Dock = DockStyle.Fill };
            if (!string.IsNullOrEmpty(title))
            {
                panel.Controls.Add(new Label { Text = title, Dock = DockStyle.Top, Height = 22, AutoSize = false });
            }

            var tool = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 32,
                FlowDirection = FlowDirection.LeftToRight
            };
            var btnAdd = new Button { Text = "添加", Width = 64 };
            var btnDel = new Button { Text = "删除", Width = 64 };
            btnAdd.Click += (_, _) => { onAdd(); RefreshLists(); };
            btnDel.Click += (_, _) => { onRemove(); RefreshLists(); };
            tool.Controls.Add(btnAdd);
            tool.Controls.Add(btnDel);

            listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            listView.Columns.Add("报文ID", 80);
            listView.Columns.Add("信号", 140);
            listView.Columns.Add("期望值", 120);

            panel.Controls.Add(listView);
            panel.Controls.Add(tool);
            return panel;
        }

        private void AddTrigger()
        {
            using var picker = new FsmSignalPickerDialog();
            if (picker.ShowDialog(this) != DialogResult.OK || picker.SelectedRef is null)
                return;

            using var valueDlg = new FsmTriggerValueDialog(picker.SelectedRef);
            if (valueDlg.ShowDialog(this) != DialogResult.OK)
                return;

            _workingTriggers.Add(valueDlg.Condition);
        }

        private void AddDisplaySignal()
        {
            using var picker = new FsmSignalPickerDialog();
            if (picker.ShowDialog(this) != DialogResult.OK || picker.SelectedRef is null)
                return;
            if (_workingDisplay.Any(s => s.MsgId == picker.SelectedRef.MsgId && s.SigName == picker.SelectedRef.SigName))
                return;
            _workingDisplay.Add(new FsmSignalRef
            {
                MsgId = picker.SelectedRef.MsgId,
                SigName = picker.SelectedRef.SigName
            });
        }

        private static void RemoveSelected<T>(ListView lv, List<T> list)
        {
            if (lv.SelectedIndices.Count == 0) return;
            list.RemoveAt(lv.SelectedIndices[0]);
        }

        private void RefreshLists()
        {
            _lvTriggers.Items.Clear();
            foreach (var t in _workingTriggers)
            {
                string expected = t.ExpectedEnumLabel
                    ?? t.ExpectedPhysical?.ToString()
                    ?? t.ExpectedRaw?.ToString()
                    ?? "";
                var item = new ListViewItem($"0x{t.MsgId:X}");
                item.SubItems.Add(t.SigName);
                item.SubItems.Add(expected);
                _lvTriggers.Items.Add(item);
            }

            _lvDisplay.Items.Clear();
            foreach (var s in _workingDisplay)
            {
                var item = new ListViewItem($"0x{s.MsgId:X}");
                item.SubItems.Add(s.SigName);
                item.SubItems.Add("");
                _lvDisplay.Items.Add(item);
            }
        }

        private void SaveToModel()
        {
            _transition.Label = string.IsNullOrWhiteSpace(_txtLabel.Text) ? null : _txtLabel.Text.Trim();
            _transition.Triggers = CloneTriggers(_workingTriggers);
            _transition.DisplaySignals = CloneDisplaySignals(_workingDisplay);
        }

        private static List<FsmTriggerCondition> CloneTriggers(IEnumerable<FsmTriggerCondition> source)
            => source.Select(tr => new FsmTriggerCondition
            {
                MsgId = tr.MsgId,
                SigName = tr.SigName,
                ExpectedRaw = tr.ExpectedRaw,
                ExpectedPhysical = tr.ExpectedPhysical,
                ExpectedEnumLabel = tr.ExpectedEnumLabel
            }).ToList();

        private static List<FsmSignalRef> CloneDisplaySignals(IEnumerable<FsmSignalRef> source)
            => source.Select(s => new FsmSignalRef { MsgId = s.MsgId, SigName = s.SigName }).ToList();
    }

    internal class FsmTriggerValueDialog : Form
    {
        public FsmTriggerCondition Condition { get; private set; } = new();

        public FsmTriggerValueDialog(FsmSignalRef reference)
        {
            Text = "设置触发期望值";
            Size = new Size(360, 220);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;

            var combo = new ComboBox { Dock = DockStyle.Top, DropDownStyle = ComboBoxStyle.DropDownList };
            combo.Items.AddRange(new object[] { "物理值", "原始值", "枚举值" });

            var valuePanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };
            var txtValue = new TextBox { Dock = DockStyle.Top };
            var comboEnum = new ComboBox { Dock = DockStyle.Top, Visible = false, DropDownStyle = ComboBoxStyle.DropDownList };

            if (FsmSignalResolver.TryGetSignal(reference.MsgId, reference.SigName, out var signal, out _)
                && signal.sigValueTable is { Count: > 0 })
            {
                foreach (var kv in signal.sigValueTable)
                    comboEnum.Items.Add(kv.Value);
                combo.SelectedIndex = 2;
            }
            else
            {
                combo.SelectedIndex = 0;
            }

            void ApplyModeVisibility()
            {
                bool isEnum = combo.SelectedIndex == 2;
                txtValue.Visible = !isEnum;
                comboEnum.Visible = isEnum;
            }

            combo.SelectedIndexChanged += (_, _) => ApplyModeVisibility();
            ApplyModeVisibility();

            valuePanel.Controls.Add(comboEnum);
            valuePanel.Controls.Add(txtValue);
            valuePanel.Controls.Add(new Label { Text = "匹配方式:", Dock = DockStyle.Top, Height = 20 });
            valuePanel.Controls.Add(combo);

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                FlowDirection = FlowDirection.RightToLeft
            };
            var btnOk = new Button { Text = "确定", DialogResult = DialogResult.OK, Width = 72 };
            btnOk.Click += (_, _) =>
            {
                Condition = new FsmTriggerCondition { MsgId = reference.MsgId, SigName = reference.SigName };
                if (combo.SelectedIndex == 0)
                {
                    if (!double.TryParse(txtValue.Text, out double phys))
                    {
                        MessageBox.Show("请输入有效物理值。");
                        DialogResult = DialogResult.None;
                        return;
                    }
                    Condition.ExpectedPhysical = phys;
                }
                else if (combo.SelectedIndex == 1)
                {
                    if (!long.TryParse(txtValue.Text, out long raw))
                    {
                        MessageBox.Show("请输入有效原始值。");
                        DialogResult = DialogResult.None;
                        return;
                    }
                    Condition.ExpectedRaw = raw;
                }
                else
                {
                    if (comboEnum.SelectedItem is null)
                    {
                        MessageBox.Show("请选择枚举值。");
                        DialogResult = DialogResult.None;
                        return;
                    }
                    Condition.ExpectedEnumLabel = comboEnum.SelectedItem.ToString();
                }
            };
            btnPanel.Controls.Add(new Button { Text = "取消", DialogResult = DialogResult.Cancel, Width = 72 });
            btnPanel.Controls.Add(btnOk);

            Controls.Add(btnPanel);
            Controls.Add(valuePanel);
            AcceptButton = btnOk;
        }
    }
}

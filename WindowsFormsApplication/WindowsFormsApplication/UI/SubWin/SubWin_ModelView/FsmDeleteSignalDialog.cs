using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApplication.ModelViewFsm;

namespace WindowsFormsApplication.UI.SubWin.SubWin_ModelView
{
    internal class FsmDeleteSignalDialog : Form
    {
        private readonly ListView _listView;
        public FsmSignalRef? SelectedRef { get; private set; }

        public FsmDeleteSignalDialog(IReadOnlyList<FsmSignalRef> signals)
        {
            Text = "删除信号";
            Size = new Size(360, 320);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            _listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            _listView.Columns.Add("报文ID", 80);
            _listView.Columns.Add("信号名", 200);
            foreach (var sig in signals)
            {
                var item = new ListViewItem($"0x{sig.MsgId:X}");
                item.SubItems.Add(sig.SigName);
                item.Tag = sig;
                _listView.Items.Add(item);
            }
            _listView.DoubleClick += (_, _) => Confirm();

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                FlowDirection = FlowDirection.RightToLeft
            };
            var ok = new Button { Text = "删除", DialogResult = DialogResult.OK, Width = 72 };
            ok.Click += (_, _) => Confirm();
            buttons.Controls.Add(new Button { Text = "取消", DialogResult = DialogResult.Cancel, Width = 72 });
            buttons.Controls.Add(ok);

            Controls.Add(_listView);
            Controls.Add(buttons);
            AcceptButton = ok;
        }

        private void Confirm()
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

using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication.UI.SubWin.SubWin_ModelView
{
    internal class FsmRenameDialog : Form
    {
        private readonly TextBox _textBox;
        public string InputText => _textBox.Text;

        public FsmRenameDialog(string title, string defaultText)
        {
            Text = title;
            Size = new Size(360, 140);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            _textBox = new TextBox { Dock = DockStyle.Top, Text = defaultText };
            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                FlowDirection = FlowDirection.RightToLeft
            };
            var ok = new Button { Text = "确定", DialogResult = DialogResult.OK, Width = 72 };
            buttons.Controls.Add(new Button { Text = "取消", DialogResult = DialogResult.Cancel, Width = 72 });
            buttons.Controls.Add(ok);

            Controls.Add(buttons);
            Controls.Add(_textBox);
            AcceptButton = ok;
        }
    }
}

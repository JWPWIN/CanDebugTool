using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class SigDisplay
{
    public string sigName;
    public uint sigValue;
    public string sigDesc;
}

namespace WindowsFormsApplication.UI
{
    public partial class UI_Row_RecvSigDisplay : UserControl
    {
        SigDisplay sigDisplay = new SigDisplay();

        public UI_Row_RecvSigDisplay()
        {
            InitializeComponent();
            //全部填充父控件
            this.Dock = DockStyle.Fill;
        }

        public void InitSigInfo(string sigName,string desc) 
        {
            sigDisplay.sigName = sigName;
            sigDisplay.sigDesc = desc;

            //初始化UI信息
            label_SigName.Text = sigDisplay.sigName;
            label_SigValue.Text = sigDisplay.sigValue.ToString();
            label_SigDesc.Text = sigDisplay.sigDesc;
        }

        public void SetSigValue(uint value)
        { 
            sigDisplay.sigValue = value;
        }
    }
}

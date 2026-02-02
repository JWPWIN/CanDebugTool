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
    public partial class UI_Model_StateNode : UserControl
    {
        public string stateNodeName { set; get; }

        public UI_Model_StateNode()
        {
            InitializeComponent();
        }

        public void SetStateNodeName(string stateNodeName)
        { 
            this.stateNodeName = stateNodeName;
            Btn_StateName.Text = stateNodeName;
        }
    }
}

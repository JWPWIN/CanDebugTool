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
    public partial class Win_ComUpper : Form
    {
        public Win_ComUpper()
        {
            InitializeComponent();
            this.comboBox_CanDeviceType.SelectedIndex = 0;
            this.comboBox_CanType.SelectedIndex = 0;
        }

        private void comboBox_CanDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Btn_ConnectDevice_Click(object sender, EventArgs e)
        {
            DeviceInterfaceMng.GetInstance().OpenCanDevice(this.comboBox_CanDeviceType.SelectedIndex,this.comboBox_CanType.SelectedIndex);
        }

        private void comboBox_CanType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

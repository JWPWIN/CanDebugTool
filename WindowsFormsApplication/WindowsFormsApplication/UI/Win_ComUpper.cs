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
        //设备管理对象
        DeviceInterfaceMng deviceInterfaceMng;

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
            //实例化设备管理对象
            deviceInterfaceMng = new DeviceInterfaceMng();

            deviceInterfaceMng.OpenCanDevice(this.comboBox_CanDeviceType.SelectedIndex, this.comboBox_CanType.SelectedIndex);
        }

        private void Btn_DisconnectDevice_Click(object sender, EventArgs e)
        {
            //关闭已经打开的设备
            if (deviceInterfaceMng is not null) deviceInterfaceMng.CloseCanDevice();
            else { AppLogMng.DisplayLog("请先连接设备!"); }
        }

        private void comboBox_CanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //根据Can卡显示仲裁域和数据域波特率
            this.label_ABitRate.Text = "500K";
            if (comboBox_CanType.SelectedIndex == 0)//CANFD
            {
                this.label_DBitRate.Text = "2M";
            }
            else
            {
                this.label_DBitRate.Text = "500K";
            }
        }
    }
}

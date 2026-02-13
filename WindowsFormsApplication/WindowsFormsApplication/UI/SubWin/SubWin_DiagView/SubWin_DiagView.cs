using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using static OfficeOpenXml.ExcelErrorValue;

namespace WindowsFormsApplication.UI.SubWin.SubWin_DiagView
{
    public partial class SubWin_DiagView : Form
    {
        public SubWin_DiagView()
        {
            InitializeComponent();
        }

        private void Btn_SendDiagReq_Click(object sender, EventArgs e)
        {
            if (DeviceInterfaceMng.GetInstance()?.canDeviceOpenFlag == true)
            {
                Canfd_Frame_Com udsFrame = new Canfd_Frame_Com();
                uint reqID = UInt32.Parse(textBox_UdsReqID.Text.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber);
                udsFrame.can_id = reqID;
                udsFrame.is_canfd = 1;//TODO UI
                udsFrame.len = 8;//TODO UI
                string[] reqData = textBox_DiagReqData.Text.Split(" ");
                udsFrame.data = new byte[64];
                int tmpNum = 0;
                foreach (var item in reqData)
                {
                    udsFrame.data[tmpNum] = Byte.Parse(item, System.Globalization.NumberStyles.HexNumber);
                }

                DeviceInterfaceMng.GetInstance().UDS_SendOneUdsDiagRequest(udsFrame);
            }
        }
    }
}

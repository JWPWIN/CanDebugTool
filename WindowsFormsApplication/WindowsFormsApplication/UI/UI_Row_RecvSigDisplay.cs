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
    public partial class UI_Row_RecvSigDisplay : UserControl
    {
        CanSignal canSignalObj = new CanSignal();//当前信号属性

        uint curSignalRawValue = 0;//当前信号值（总线原始值）

        public UI_Row_RecvSigDisplay()
        {
            InitializeComponent();
            //全部填充父控件
            this.Dock = DockStyle.Fill;
        }

        public void InitSigInfo(CanSignal canSignal) 
        {
            canSignalObj = canSignal;

            label_SigName.Text = canSignalObj.msgId.ToString("X3").ToUpper() + "-" + canSignalObj.sigName;
            label_SigDesc.Text = canSignalObj.sigDesc;
        }

        public void UpdateSigValue(Canfd_Frame_Com msgData)
        {
            CAN_SIG_FORMAT sigFormat = (canSignalObj.sigOrderType == 0) ? CAN_SIG_FORMAT.MOTOROLA_LSB : CAN_SIG_FORMAT.INTEL_STANDARD;
            curSignalRawValue = CanBitLibTool.CAN_get_frame_dataFD(msgData.data, sigFormat, (ushort)canSignalObj.sigStartBit, (ushort)canSignalObj.sigLen);

            //计算实际物理值,仅保留显示小数点后2位
            double sigRealPhysicalValue = Math.Round(curSignalRawValue * canSignalObj.sigFactor + canSignalObj.sigOffset, 2);

            //判断是否需要显示信号枚举值
            string valueStr = sigRealPhysicalValue.ToString();
            if ((canSignalObj.sigValueTable is not null) && (canSignalObj.sigValueTable.Count > 0) )
            {
                foreach (var item in canSignalObj.sigValueTable)
                {
                    if ((int)sigRealPhysicalValue == item.Key)
                    {
                        valueStr = item.Value.ToString();
                        break;
                    }
                }
            }

            label_SigValue.Invoke(new Action(() => label_SigValue.Text = valueStr));
        }
    }
}

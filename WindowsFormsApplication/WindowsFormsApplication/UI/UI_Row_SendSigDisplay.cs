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
    public partial class UI_Row_SendSigDisplay : UserControl
    {
        CanSignal canSignalObj = new CanSignal();//当前信号属性

        bool isCanfd = false;//是否为canfd

        bool hasValueTable = false;//该信号值是否有值列表

        ComboBox sendValueUI_ComboBox = new ComboBox();//信号值下拉选项框控件

        TextBox sendValueUI_TextBox = new TextBox();//信号值文本输入框控件

        uint curSignalRawValue = 0;//当前信号值（总线原始值）

        public UI_Row_SendSigDisplay()
        {
            InitializeComponent();
            //全部填充父控件
            this.Dock = DockStyle.Fill;
        }

        public void InitSigInfo(CanSignal canSignal, bool isCanfd)
        {
            canSignalObj = canSignal;
            this.isCanfd = isCanfd;
            //更新信号名及信号描述
            label_SigName.Text = canSignalObj.sigName;
            label_SigDesc.Text = canSignalObj.sigDesc;
            //根据发送报文有值列表 判断生成文本输入框 还是 下拉选项框
            if ((canSignalObj.sigValueTable is not null) && (canSignalObj.sigValueTable.Count > 0))
            {
                hasValueTable = true;
                //有值列表 发送信号值为下拉选项框
                foreach (var item in canSignalObj.sigValueTable)
                {
                    //根据值表生成下拉选项
                    sendValueUI_ComboBox.Items.Add(item.Key.ToString() + ":" + item.Value);
                }
                tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
                tableLayoutPanel1.Controls.Add(sendValueUI_ComboBox, 1, 0);
                sendValueUI_ComboBox.Dock = DockStyle .Fill;
            }
            else
            {
                hasValueTable = false;
                //有值列表 发送信号值为文本输入框
                tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
                tableLayoutPanel1.Controls.Add(sendValueUI_TextBox, 1, 0);
                sendValueUI_TextBox.Dock = DockStyle .Fill;
            }
        }

        /// <summary>
        /// 获取该信号的实时发送值
        /// </summary>
        /// <returns></returns>
        public uint GetSigValue()
        {
            uint retValue = 0;
            uint physicalValue = 0;

            string valueStr = string.Empty;
            //值列表，从下拉选项框中获取信号发送物理值
            if (hasValueTable) sendValueUI_ComboBox.Invoke(new Action( ()=> valueStr = sendValueUI_ComboBox.SelectedText.Split(":")[0]) );
            //非值列表，从文本输入框中获取信号发送物理值
            else sendValueUI_TextBox.Invoke(new Action(() => valueStr = sendValueUI_TextBox.Text));

            //获取设置的发送信号值
            try
            {
                if(valueStr != string.Empty) physicalValue = Convert.ToUInt32(valueStr);
            } 
            catch (Exception ex)
            {
                physicalValue = 0;
            }

            //计算总线信号值
            curSignalRawValue = (uint)((physicalValue - canSignalObj.sigOffset) / canSignalObj.sigFactor);

            retValue = curSignalRawValue;

            return retValue;
        }

        /// <summary>
        /// 填充该信号UI控件内的值到报文帧数据内
        /// </summary>
        /// <param name="msgData">报文帧数据 64Byte</param>
        public void SetSigValueToMsg(byte[] msgData)
        {
            uint _sendValue = GetSigValue();

            CAN_SIG_FORMAT sigFormat = (canSignalObj.sigOrderType == 0) ? CAN_SIG_FORMAT.MOTOROLA_LSB : CAN_SIG_FORMAT.INTEL_STANDARD;

            if (isCanfd)
                CanBitLibTool.CAN_set_frame_dataFD(msgData, sigFormat, (ushort)canSignalObj.sigStartBit, (ushort)canSignalObj.sigLen, _sendValue);
            else
                CanBitLibTool.CAN_set_frame_data(msgData, sigFormat, (ushort)canSignalObj.sigStartBit, (ushort)canSignalObj.sigLen, _sendValue);

        }
    }
}


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
    public partial class UI_ComUpper : UserControl
    {
        //设备管理对象
        DeviceInterfaceMng deviceInterfaceMng;

        //接收报文区域的报文UI集合<报文ID,接收信号UI列表>
        Dictionary<uint,List<UI_Row_RecvSigDisplay>> recvMsgAreaControlDict = new Dictionary<uint, List<UI_Row_RecvSigDisplay>>();

        public UI_ComUpper()
        {
            InitializeComponent();
            this.comboBox_CanDeviceType.SelectedIndex = 0;
            this.comboBox_CanType.SelectedIndex = 0;

            //实例化设备管理器对象
            deviceInterfaceMng = new DeviceInterfaceMng();
        }

        /// <summary>
        /// 根据通信协议初始化上位机报文接收窗口
        /// </summary>
        public void InitRecvMsgArea()
        {
            //判断是否加载过通协议
            if (CanDbcDataManager.GetInstance().isLoadCfg == false) return;

            //将通信协议中ECU发送的报文作为上位机接收的报文显示,创建通信协议接收报文UI集合
            int recvSigAmount = 0;//接收信号总数
            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
            {
                if (item.transmitter == "OBC" || item.transmitter == "DCDC" || item.transmitter == "CDU")
                {
                    List<UI_Row_RecvSigDisplay> tmpList = new List<UI_Row_RecvSigDisplay>();

                    foreach (var item1 in item.signals)
                    {
                        //创建UI用于显示信号
                        UI_Row_RecvSigDisplay recvMsg_Row = new UI_Row_RecvSigDisplay();
                        recvMsg_Row.InitSigInfo(item1.sigName,item1.sigDesc);

                        tmpList.Add(recvMsg_Row);
                    }
                    recvSigAmount += item.signals.Count;
                    recvMsgAreaControlDict.Add(item.msgId,tmpList);
                }
            }

            //设置报文接收窗口UI
            if (recvSigAmount == 0) return;
            tableLayoutPanel_RecvMsgArea.RowCount = recvSigAmount;
            int rowCount = 0;
            foreach (var item in recvMsgAreaControlDict.Values)
            {
                foreach (var item1 in item)
                {
                    tableLayoutPanel_RecvMsgArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
                    tableLayoutPanel_RecvMsgArea.Controls.Add(item1, 0, rowCount);
                    rowCount++;
                }
            }
        
        }

        private void Btn_ConnectDevice_Click(object sender, EventArgs e)
        {
            //打开设备
            if (deviceInterfaceMng is not null) 
                deviceInterfaceMng.OpenCanDevice(this.comboBox_CanDeviceType.SelectedIndex, this.comboBox_CanType.SelectedIndex);
        }

        private void Btn_DisconnectDevice_Click(object sender, EventArgs e)
        {
            //关闭已经打开的设备
            if (deviceInterfaceMng is not null) 
                deviceInterfaceMng.CloseCanDevice();
        }

    }
}

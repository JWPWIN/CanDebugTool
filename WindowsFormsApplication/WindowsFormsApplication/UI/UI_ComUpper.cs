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

        //接收报文区域的信号行UI集合<报文ID,接收信号UI列表>
        Dictionary<uint,List<UI_Row_RecvSigDisplay>> recvMsgArea_sigRowUIDict = new Dictionary<uint, List<UI_Row_RecvSigDisplay>>();

        //接收报文区域的报文ID UI集合<报文ID,标签UI>
        Dictionary<uint, Label> recvMsgArea_msgIdTitleUIDict = new Dictionary<uint, Label>();

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
            int recvMsgAmount = 0;//接收报文总数
            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet.Values)
            {
                if (item.transmitter == "OBC" || item.transmitter == "DCDC" || item.transmitter == "CDU")
                {
                    //生成报文ID UI标题
                    Label tmpIdTitleLabel = new Label();
                    tmpIdTitleLabel.Dock = DockStyle.Fill;
                    tmpIdTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
                    tmpIdTitleLabel.Text = "0x" + item.msgId.ToString("X3").ToUpper() + " " + item.msgName;
                    recvMsgArea_msgIdTitleUIDict.Add(item.msgId, tmpIdTitleLabel);
                    recvMsgAmount++;

                    //生成信号UI行
                    List<UI_Row_RecvSigDisplay> tmpList = new List<UI_Row_RecvSigDisplay>();
                    foreach (var item1 in item.signals)
                    {
                        //创建UI用于显示信号
                        UI_Row_RecvSigDisplay recvMsg_Row = new UI_Row_RecvSigDisplay();
                        recvMsg_Row.InitSigInfo(item1);

                        tmpList.Add(recvMsg_Row);
                    }
                    recvSigAmount += item.signals.Count;
                    recvMsgArea_sigRowUIDict.Add(item.msgId,tmpList);
                }
            }

            //设置报文接收窗口UI
            if (recvSigAmount == 0) return;
            tableLayoutPanel_RecvMsgArea.RowCount = recvSigAmount + recvMsgAmount;
            int rowCount = 0;
            foreach (var item in recvMsgArea_sigRowUIDict)
            {
                //显示报文ID的UI标题
                foreach (var item1 in recvMsgArea_msgIdTitleUIDict)
                {
                    if (item1.Key == item.Key)
                    {
                        tableLayoutPanel_RecvMsgArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
                        tableLayoutPanel_RecvMsgArea.Controls.Add(item1.Value, 0, rowCount);
                        rowCount++;
                        break;
                    }
                }

                //显示信号值UI
                foreach (var item1 in item.Value)
                {
                    tableLayoutPanel_RecvMsgArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
                    tableLayoutPanel_RecvMsgArea.Controls.Add(item1, 0, rowCount);
                    rowCount++;
                }
            }
        }

        /// <summary>
        /// 从设备接口对象中获取已接收待处理的报文，显示在接收报文区域
        /// </summary>
        public void MainLoopThread_Task_UpdateRecvMsgArea()
        {
            //获取接收报文数据
            List<Canfd_Frame_Com> _recvMsgList = DeviceInterfaceMng.GetInstance().GetCurWaitToHandleRecvMsg();

            //实时更新上位机中显示接收数据
            foreach (var item in recvMsgArea_sigRowUIDict)
            {
                //获取对应ID的报文
                Canfd_Frame_Com _tmpMsg = new Canfd_Frame_Com();
                foreach (var item1 in _recvMsgList)
                {
                    if (item.Key == item1.can_id)
                    {
                        _tmpMsg = item1;

                        break;
                    }
                }

                //无对应ID的数据，跳过
                if (_tmpMsg.data is null) continue;

                //从报文中获取信号当前值 更新UI
                foreach (var item1 in item.Value)
                {
                    item1.UpdateSigValue(_tmpMsg);
                }
            }

            //清除待处理的接收报文数据
            DeviceInterfaceMng.GetInstance().ClearCurWaitToHandleRecvMsg();
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

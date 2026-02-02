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
    public partial class SubWin_ModelView : Form
    {
        //存储模型状态节点字典<状态节点名称,UI对象>
        Dictionary<string, UI_Model_StateNode> modelStateNodeDict = new Dictionary<string, UI_Model_StateNode>();

        public SubWin_ModelView()
        {
            InitializeComponent();
        }

        private void Btn_AddState_Click(object sender, EventArgs e)
        {
            UI_Model_StateNode uI_Model_StateNode = new UI_Model_StateNode();
            //生成不重复的节点名称
            string stateNodeName = string.Empty;
            int tmpNum = 0;
            while (true)
            {
                string tmpStr = "State" + tmpNum.ToString();

                foreach (var item in modelStateNodeDict.Keys)
                {
                    if (item == tmpStr)
                    {
                        tmpStr = string.Empty;
                        break;
                    }
                }

                if (tmpStr != string.Empty)
                {
                    stateNodeName = tmpStr;
                    break;
                }

                tmpNum++;
            }
            //新状态节点存入状态字典
            uI_Model_StateNode.SetStateNodeName(stateNodeName);
            modelStateNodeDict.Add(stateNodeName,uI_Model_StateNode);

            //节点载入状态绘制区域中间
            uI_Model_StateNode.Location = new System.Drawing.Point(this.groupBox_ModelArea.Width / 2, this.groupBox_ModelArea.Height / 2);
            this.groupBox_ModelArea.Controls.Add(uI_Model_StateNode);
        }
    }
}

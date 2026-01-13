namespace WindowsFormsApplication
{
    partial class MainWin
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                this.mainLoopThread.Stop();//结束主循环线程
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            tabControl_AllFunsSplit = new System.Windows.Forms.TabControl();
            tabPage_FirstPage = new System.Windows.Forms.TabPage();
            uI_DbcDataManager = new WindowsFormsApplication.UI.UI_DbcDataManager();
            tabPage_CanMatrix = new System.Windows.Forms.TabPage();
            uI_CanMatrix = new WindowsFormsApplication.UI.UI_CanMatrix();
            tabPage_ComUpper = new System.Windows.Forms.TabPage();
            uI_ComUpper = new WindowsFormsApplication.UI.UI_ComUpper();
            statusStrip = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel_CurSysTime = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel_CurPageName = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel_DBCState = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel_DeviceCntState = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel_GlobalLogBox = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            tabControl_AllFunsSplit.SuspendLayout();
            tabPage_FirstPage.SuspendLayout();
            tabPage_CanMatrix.SuspendLayout();
            tabPage_ComUpper.SuspendLayout();
            statusStrip.SuspendLayout();
            toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl_AllFunsSplit
            // 
            tabControl_AllFunsSplit.Controls.Add(tabPage_FirstPage);
            tabControl_AllFunsSplit.Controls.Add(tabPage_CanMatrix);
            tabControl_AllFunsSplit.Controls.Add(tabPage_ComUpper);
            tabControl_AllFunsSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl_AllFunsSplit.Location = new System.Drawing.Point(0, 0);
            tabControl_AllFunsSplit.Name = "tabControl_AllFunsSplit";
            tabControl_AllFunsSplit.SelectedIndex = 0;
            tabControl_AllFunsSplit.Size = new System.Drawing.Size(800, 578);
            tabControl_AllFunsSplit.TabIndex = 5;
            // 
            // tabPage_FirstPage
            // 
            tabPage_FirstPage.Controls.Add(uI_DbcDataManager);
            tabPage_FirstPage.Location = new System.Drawing.Point(4, 26);
            tabPage_FirstPage.Name = "tabPage_FirstPage";
            tabPage_FirstPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage_FirstPage.Size = new System.Drawing.Size(792, 548);
            tabPage_FirstPage.TabIndex = 0;
            tabPage_FirstPage.Text = "首页";
            tabPage_FirstPage.UseVisualStyleBackColor = true;
            // 
            // uI_DbcDataManager
            // 
            uI_DbcDataManager.Dock = System.Windows.Forms.DockStyle.Fill;
            uI_DbcDataManager.Location = new System.Drawing.Point(3, 3);
            uI_DbcDataManager.Name = "uI_DbcDataManager";
            uI_DbcDataManager.Size = new System.Drawing.Size(786, 542);
            uI_DbcDataManager.TabIndex = 0;
            // 
            // tabPage_CanMatrix
            // 
            tabPage_CanMatrix.Controls.Add(uI_CanMatrix);
            tabPage_CanMatrix.Location = new System.Drawing.Point(4, 26);
            tabPage_CanMatrix.Name = "tabPage_CanMatrix";
            tabPage_CanMatrix.Padding = new System.Windows.Forms.Padding(3);
            tabPage_CanMatrix.Size = new System.Drawing.Size(792, 548);
            tabPage_CanMatrix.TabIndex = 1;
            tabPage_CanMatrix.Text = "CAN矩阵";
            tabPage_CanMatrix.UseVisualStyleBackColor = true;
            // 
            // uI_CanMatrix
            // 
            uI_CanMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            uI_CanMatrix.Location = new System.Drawing.Point(3, 3);
            uI_CanMatrix.Name = "uI_CanMatrix";
            uI_CanMatrix.Size = new System.Drawing.Size(786, 542);
            uI_CanMatrix.TabIndex = 0;
            // 
            // tabPage_ComUpper
            // 
            tabPage_ComUpper.Controls.Add(uI_ComUpper);
            tabPage_ComUpper.Location = new System.Drawing.Point(4, 26);
            tabPage_ComUpper.Name = "tabPage_ComUpper";
            tabPage_ComUpper.Padding = new System.Windows.Forms.Padding(3);
            tabPage_ComUpper.Size = new System.Drawing.Size(792, 548);
            tabPage_ComUpper.TabIndex = 2;
            tabPage_ComUpper.Text = "通信上位机";
            tabPage_ComUpper.UseVisualStyleBackColor = true;
            // 
            // uI_ComUpper
            // 
            uI_ComUpper.Dock = System.Windows.Forms.DockStyle.Fill;
            uI_ComUpper.Location = new System.Drawing.Point(3, 3);
            uI_ComUpper.Name = "uI_ComUpper";
            uI_ComUpper.Size = new System.Drawing.Size(786, 542);
            uI_ComUpper.TabIndex = 0;
            // 
            // statusStrip
            // 
            statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel_CurSysTime, toolStripStatusLabel_CurPageName, toolStripStatusLabel_DBCState, toolStripStatusLabel_DeviceCntState, toolStripStatusLabel_GlobalLogBox });
            statusStrip.Location = new System.Drawing.Point(0, 0);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(800, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel_CurSysTime
            // 
            toolStripStatusLabel_CurSysTime.Name = "toolStripStatusLabel_CurSysTime";
            toolStripStatusLabel_CurSysTime.Size = new System.Drawing.Size(56, 17);
            toolStripStatusLabel_CurSysTime.Text = "系统时间";
            // 
            // toolStripStatusLabel_CurPageName
            // 
            toolStripStatusLabel_CurPageName.Name = "toolStripStatusLabel_CurPageName";
            toolStripStatusLabel_CurPageName.Size = new System.Drawing.Size(68, 17);
            toolStripStatusLabel_CurPageName.Text = "当前页签名";
            // 
            // toolStripStatusLabel_DBCState
            // 
            toolStripStatusLabel_DBCState.Name = "toolStripStatusLabel_DBCState";
            toolStripStatusLabel_DBCState.Size = new System.Drawing.Size(57, 17);
            toolStripStatusLabel_DBCState.Text = "DBC状态";
            // 
            // toolStripStatusLabel_DeviceCntState
            // 
            toolStripStatusLabel_DeviceCntState.Name = "toolStripStatusLabel_DeviceCntState";
            toolStripStatusLabel_DeviceCntState.Size = new System.Drawing.Size(80, 17);
            toolStripStatusLabel_DeviceCntState.Text = "设备连接状态";
            // 
            // toolStripStatusLabel_GlobalLogBox
            // 
            toolStripStatusLabel_GlobalLogBox.Name = "toolStripStatusLabel_GlobalLogBox";
            toolStripStatusLabel_GlobalLogBox.Size = new System.Drawing.Size(78, 17);
            toolStripStatusLabel_GlobalLogBox.Text = "全局Log消息";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            toolStripContainer1.BottomToolStripPanel.Controls.Add(statusStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            toolStripContainer1.ContentPanel.AutoScroll = true;
            toolStripContainer1.ContentPanel.Controls.Add(tabControl_AllFunsSplit);
            toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(800, 578);
            toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripContainer1.LeftToolStripPanelVisible = false;
            toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            toolStripContainer1.Name = "toolStripContainer1";
            toolStripContainer1.RightToolStripPanelVisible = false;
            toolStripContainer1.Size = new System.Drawing.Size(800, 600);
            toolStripContainer1.TabIndex = 6;
            toolStripContainer1.Text = "toolStripContainer1";
            toolStripContainer1.TopToolStripPanelVisible = false;
            // 
            // MainWin
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 600);
            Controls.Add(toolStripContainer1);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "MainWin";
            Text = "CAN调试工具";
            tabControl_AllFunsSplit.ResumeLayout(false);
            tabPage_FirstPage.ResumeLayout(false);
            tabPage_CanMatrix.ResumeLayout(false);
            tabPage_ComUpper.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            toolStripContainer1.BottomToolStripPanel.PerformLayout();
            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl_AllFunsSplit;
        private System.Windows.Forms.TabPage tabPage_FirstPage;
        private System.Windows.Forms.TabPage tabPage_CanMatrix;
        private System.Windows.Forms.TabPage tabPage_ComUpper;
        private UI.UI_DbcDataManager uI_DbcDataManager;
        private UI.UI_CanMatrix uI_CanMatrix;
        private UI.UI_ComUpper uI_ComUpper;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_CurPageName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_DBCState;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_DeviceCntState;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_CurSysTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_GlobalLogBox;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    }
}


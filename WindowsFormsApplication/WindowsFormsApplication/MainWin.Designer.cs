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
            panel_ImportDbc = new System.Windows.Forms.Panel();
            panel_ImportDbcSeparator = new System.Windows.Forms.Panel();
            tableLayoutPanel_ImportDbc = new System.Windows.Forms.TableLayoutPanel();
            label_ImportSection = new System.Windows.Forms.Label();
            flowLayoutPanel_ImportButtons = new System.Windows.Forms.FlowLayoutPanel();
            Btn_ImpExcelDBC = new System.Windows.Forms.Button();
            button_ImportTxtDbc = new System.Windows.Forms.Button();
            flowLayoutPanel_DbcStatus = new System.Windows.Forms.FlowLayoutPanel();
            panel_DbcStatusDot = new System.Windows.Forms.Panel();
            label_DbcLoadState = new System.Windows.Forms.Label();
            statusStrip = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel_CurSysTime = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel_CurPageName = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel_DBCState = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel_DeviceCntState = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel_GlobalLogBox = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            tableLayoutPanel_MainContent = new System.Windows.Forms.TableLayoutPanel();
            tabControl_AllFunsSplit.SuspendLayout();
            tabPage_FirstPage.SuspendLayout();
            tabPage_CanMatrix.SuspendLayout();
            tabPage_ComUpper.SuspendLayout();
            panel_ImportDbc.SuspendLayout();
            tableLayoutPanel_ImportDbc.SuspendLayout();
            flowLayoutPanel_ImportButtons.SuspendLayout();
            flowLayoutPanel_DbcStatus.SuspendLayout();
            statusStrip.SuspendLayout();
            tableLayoutPanel_MainContent.SuspendLayout();
            toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl_AllFunsSplit
            // 
            tabControl_AllFunsSplit.Controls.Add(tabPage_ComUpper);
            tabControl_AllFunsSplit.Controls.Add(tabPage_CanMatrix);
            tabControl_AllFunsSplit.Controls.Add(tabPage_FirstPage);
            tabControl_AllFunsSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl_AllFunsSplit.Location = new System.Drawing.Point(0, 0);
            tabControl_AllFunsSplit.Name = "tabControl_AllFunsSplit";
            tabControl_AllFunsSplit.Padding = new System.Drawing.Point(8, 4);
            tabControl_AllFunsSplit.SelectedIndex = 0;
            tabControl_AllFunsSplit.Size = new System.Drawing.Size(800, 578);
            tabControl_AllFunsSplit.TabIndex = 5;
            tabControl_AllFunsSplit.SelectedIndexChanged += tabControl_AllFunsSplit_SelectedIndexChanged;
            // 
            // tabPage_FirstPage
            // 
            tabPage_FirstPage.Controls.Add(uI_DbcDataManager);
            tabPage_FirstPage.Location = new System.Drawing.Point(4, 26);
            tabPage_FirstPage.Name = "tabPage_FirstPage";
            tabPage_FirstPage.Padding = new System.Windows.Forms.Padding(10);
            tabPage_FirstPage.Size = new System.Drawing.Size(792, 548);
            tabPage_FirstPage.TabIndex = 2;
            tabPage_FirstPage.Text = "工具";
            tabPage_FirstPage.UseVisualStyleBackColor = true;
            tabPage_FirstPage.BackColor = System.Drawing.Color.White;
            // 
            // uI_DbcDataManager
            // 
            uI_DbcDataManager.Dock = System.Windows.Forms.DockStyle.Fill;
            uI_DbcDataManager.Location = new System.Drawing.Point(10, 10);
            uI_DbcDataManager.Name = "uI_DbcDataManager";
            uI_DbcDataManager.Size = new System.Drawing.Size(772, 528);
            uI_DbcDataManager.TabIndex = 0;
            // 
            // tabPage_CanMatrix
            // 
            tabPage_CanMatrix.Controls.Add(uI_CanMatrix);
            tabPage_CanMatrix.Location = new System.Drawing.Point(4, 26);
            tabPage_CanMatrix.Name = "tabPage_CanMatrix";
            tabPage_CanMatrix.Padding = new System.Windows.Forms.Padding(10);
            tabPage_CanMatrix.Size = new System.Drawing.Size(806, 548);
            tabPage_CanMatrix.TabIndex = 1;
            tabPage_CanMatrix.Text = "CAN矩阵";
            tabPage_CanMatrix.UseVisualStyleBackColor = true;
            tabPage_CanMatrix.BackColor = System.Drawing.Color.White;
            // 
            // uI_CanMatrix
            // 
            uI_CanMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            uI_CanMatrix.Location = new System.Drawing.Point(10, 10);
            uI_CanMatrix.Name = "uI_CanMatrix";
            uI_CanMatrix.Size = new System.Drawing.Size(786, 528);
            uI_CanMatrix.TabIndex = 0;
            // 
            // tabPage_ComUpper
            // 
            tabPage_ComUpper.Controls.Add(uI_ComUpper);
            tabPage_ComUpper.Location = new System.Drawing.Point(4, 26);
            tabPage_ComUpper.Name = "tabPage_ComUpper";
            tabPage_ComUpper.Padding = new System.Windows.Forms.Padding(10);
            tabPage_ComUpper.Size = new System.Drawing.Size(792, 548);
            tabPage_ComUpper.TabIndex = 0;
            tabPage_ComUpper.Text = "通信上位机";
            tabPage_ComUpper.UseVisualStyleBackColor = true;
            tabPage_ComUpper.BackColor = System.Drawing.Color.White;
            // 
            // uI_ComUpper
            // 
            uI_ComUpper.Dock = System.Windows.Forms.DockStyle.Fill;
            uI_ComUpper.Location = new System.Drawing.Point(10, 10);
            uI_ComUpper.Name = "uI_ComUpper";
            uI_ComUpper.Size = new System.Drawing.Size(772, 528);
            uI_ComUpper.TabIndex = 0;
            // 
            // panel_ImportDbc
            // 
            panel_ImportDbc.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            panel_ImportDbc.Controls.Add(tableLayoutPanel_ImportDbc);
            panel_ImportDbc.Controls.Add(panel_ImportDbcSeparator);
            panel_ImportDbc.Dock = System.Windows.Forms.DockStyle.Fill;
            panel_ImportDbc.Location = new System.Drawing.Point(0, 0);
            panel_ImportDbc.Margin = new System.Windows.Forms.Padding(0);
            panel_ImportDbc.Name = "panel_ImportDbc";
            panel_ImportDbc.Padding = new System.Windows.Forms.Padding(12, 10, 12, 0);
            panel_ImportDbc.Size = new System.Drawing.Size(800, 58);
            panel_ImportDbc.TabIndex = 7;
            // 
            // panel_ImportDbcSeparator
            // 
            panel_ImportDbcSeparator.BackColor = System.Drawing.Color.FromArgb(210, 214, 220);
            panel_ImportDbcSeparator.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel_ImportDbcSeparator.Location = new System.Drawing.Point(12, 57);
            panel_ImportDbcSeparator.Name = "panel_ImportDbcSeparator";
            panel_ImportDbcSeparator.Size = new System.Drawing.Size(776, 1);
            panel_ImportDbcSeparator.TabIndex = 1;
            // 
            // tableLayoutPanel_ImportDbc
            // 
            tableLayoutPanel_ImportDbc.ColumnCount = 4;
            tableLayoutPanel_ImportDbc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayoutPanel_ImportDbc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayoutPanel_ImportDbc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel_ImportDbc.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayoutPanel_ImportDbc.Controls.Add(label_ImportSection, 0, 0);
            tableLayoutPanel_ImportDbc.Controls.Add(flowLayoutPanel_ImportButtons, 1, 0);
            tableLayoutPanel_ImportDbc.Controls.Add(flowLayoutPanel_DbcStatus, 3, 0);
            tableLayoutPanel_ImportDbc.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel_ImportDbc.Location = new System.Drawing.Point(12, 10);
            tableLayoutPanel_ImportDbc.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel_ImportDbc.Name = "tableLayoutPanel_ImportDbc";
            tableLayoutPanel_ImportDbc.RowCount = 1;
            tableLayoutPanel_ImportDbc.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel_ImportDbc.Size = new System.Drawing.Size(776, 47);
            tableLayoutPanel_ImportDbc.TabIndex = 0;
            // 
            // label_ImportSection
            // 
            label_ImportSection.Anchor = System.Windows.Forms.AnchorStyles.Left;
            label_ImportSection.AutoSize = true;
            label_ImportSection.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label_ImportSection.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            label_ImportSection.Location = new System.Drawing.Point(0, 14);
            label_ImportSection.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            label_ImportSection.Name = "label_ImportSection";
            label_ImportSection.Size = new System.Drawing.Size(68, 17);
            label_ImportSection.TabIndex = 0;
            label_ImportSection.Text = "CAN 矩阵";
            // 
            // flowLayoutPanel_ImportButtons
            // 
            flowLayoutPanel_ImportButtons.Anchor = System.Windows.Forms.AnchorStyles.Left;
            flowLayoutPanel_ImportButtons.AutoSize = true;
            flowLayoutPanel_ImportButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_ImportButtons.Controls.Add(Btn_ImpExcelDBC);
            flowLayoutPanel_ImportButtons.Controls.Add(button_ImportTxtDbc);
            flowLayoutPanel_ImportButtons.Location = new System.Drawing.Point(84, 8);
            flowLayoutPanel_ImportButtons.Margin = new System.Windows.Forms.Padding(0);
            flowLayoutPanel_ImportButtons.Name = "flowLayoutPanel_ImportButtons";
            flowLayoutPanel_ImportButtons.Size = new System.Drawing.Size(232, 31);
            flowLayoutPanel_ImportButtons.TabIndex = 1;
            flowLayoutPanel_ImportButtons.WrapContents = false;
            // 
            // Btn_ImpExcelDBC
            // 
            Btn_ImpExcelDBC.AutoSize = true;
            Btn_ImpExcelDBC.Location = new System.Drawing.Point(0, 0);
            Btn_ImpExcelDBC.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            Btn_ImpExcelDBC.MinimumSize = new System.Drawing.Size(108, 31);
            Btn_ImpExcelDBC.Name = "Btn_ImpExcelDBC";
            Btn_ImpExcelDBC.Size = new System.Drawing.Size(108, 31);
            Btn_ImpExcelDBC.TabIndex = 0;
            Btn_ImpExcelDBC.Text = "导入 Excel 数据";
            Btn_ImpExcelDBC.UseVisualStyleBackColor = true;
            Btn_ImpExcelDBC.Click += Btn_ImpExcelDBC_Click;
            // 
            // button_ImportTxtDbc
            // 
            button_ImportTxtDbc.AutoSize = true;
            button_ImportTxtDbc.Location = new System.Drawing.Point(116, 0);
            button_ImportTxtDbc.Margin = new System.Windows.Forms.Padding(0);
            button_ImportTxtDbc.MinimumSize = new System.Drawing.Size(116, 31);
            button_ImportTxtDbc.Name = "button_ImportTxtDbc";
            button_ImportTxtDbc.Size = new System.Drawing.Size(116, 31);
            button_ImportTxtDbc.TabIndex = 1;
            button_ImportTxtDbc.Text = "导入 DBC 数据";
            button_ImportTxtDbc.UseVisualStyleBackColor = true;
            button_ImportTxtDbc.Click += button_ImportTxtDbc_Click;
            // 
            // flowLayoutPanel_DbcStatus
            // 
            flowLayoutPanel_DbcStatus.Anchor = System.Windows.Forms.AnchorStyles.Right;
            flowLayoutPanel_DbcStatus.AutoSize = true;
            flowLayoutPanel_DbcStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_DbcStatus.Controls.Add(panel_DbcStatusDot);
            flowLayoutPanel_DbcStatus.Controls.Add(label_DbcLoadState);
            flowLayoutPanel_DbcStatus.Location = new System.Drawing.Point(688, 13);
            flowLayoutPanel_DbcStatus.Margin = new System.Windows.Forms.Padding(0);
            flowLayoutPanel_DbcStatus.Name = "flowLayoutPanel_DbcStatus";
            flowLayoutPanel_DbcStatus.Size = new System.Drawing.Size(88, 21);
            flowLayoutPanel_DbcStatus.TabIndex = 2;
            flowLayoutPanel_DbcStatus.WrapContents = false;
            // 
            // panel_DbcStatusDot
            // 
            panel_DbcStatusDot.BackColor = System.Drawing.Color.Gray;
            panel_DbcStatusDot.Location = new System.Drawing.Point(0, 6);
            panel_DbcStatusDot.Margin = new System.Windows.Forms.Padding(0, 6, 6, 0);
            panel_DbcStatusDot.Name = "panel_DbcStatusDot";
            panel_DbcStatusDot.Size = new System.Drawing.Size(9, 9);
            panel_DbcStatusDot.TabIndex = 0;
            // 
            // label_DbcLoadState
            // 
            label_DbcLoadState.Anchor = System.Windows.Forms.AnchorStyles.Left;
            label_DbcLoadState.AutoSize = true;
            label_DbcLoadState.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            label_DbcLoadState.Location = new System.Drawing.Point(15, 2);
            label_DbcLoadState.Margin = new System.Windows.Forms.Padding(0);
            label_DbcLoadState.Name = "label_DbcLoadState";
            label_DbcLoadState.Size = new System.Drawing.Size(73, 17);
            label_DbcLoadState.TabIndex = 1;
            label_DbcLoadState.Text = "未加载 DBC";
            label_DbcLoadState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // tableLayoutPanel_MainContent
            // 
            tableLayoutPanel_MainContent.ColumnCount = 1;
            tableLayoutPanel_MainContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel_MainContent.Controls.Add(panel_ImportDbc, 0, 0);
            tableLayoutPanel_MainContent.Controls.Add(tabControl_AllFunsSplit, 0, 1);
            tableLayoutPanel_MainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel_MainContent.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel_MainContent.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel_MainContent.Name = "tableLayoutPanel_MainContent";
            tableLayoutPanel_MainContent.RowCount = 2;
            tableLayoutPanel_MainContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            tableLayoutPanel_MainContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel_MainContent.Size = new System.Drawing.Size(800, 578);
            tableLayoutPanel_MainContent.TabIndex = 0;
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
            toolStripContainer1.ContentPanel.AutoScroll = false;
            toolStripContainer1.ContentPanel.Controls.Add(tableLayoutPanel_MainContent);
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
            ClientSize = new System.Drawing.Size(1000, 600);
            Controls.Add(toolStripContainer1);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "MainWin";
            Text = "CAN调试工具"+ AppVerStr;
            FormClosing += MainWin_FormClosing;
            FormClosed += MainWin_FormClosed;
            tabControl_AllFunsSplit.ResumeLayout(false);
            tabPage_FirstPage.ResumeLayout(false);
            tabPage_CanMatrix.ResumeLayout(false);
            tabPage_ComUpper.ResumeLayout(false);
            panel_ImportDbc.ResumeLayout(false);
            tableLayoutPanel_ImportDbc.ResumeLayout(false);
            tableLayoutPanel_ImportDbc.PerformLayout();
            flowLayoutPanel_ImportButtons.ResumeLayout(false);
            flowLayoutPanel_ImportButtons.PerformLayout();
            flowLayoutPanel_DbcStatus.ResumeLayout(false);
            flowLayoutPanel_DbcStatus.PerformLayout();
            tableLayoutPanel_MainContent.ResumeLayout(false);
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_MainContent;
        private System.Windows.Forms.Panel panel_ImportDbc;
        private System.Windows.Forms.Panel panel_ImportDbcSeparator;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_ImportDbc;
        private System.Windows.Forms.Label label_ImportSection;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_ImportButtons;
        private System.Windows.Forms.Button Btn_ImpExcelDBC;
        private System.Windows.Forms.Button button_ImportTxtDbc;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_DbcStatus;
        private System.Windows.Forms.Panel panel_DbcStatusDot;
        private System.Windows.Forms.Label label_DbcLoadState;
    }
}


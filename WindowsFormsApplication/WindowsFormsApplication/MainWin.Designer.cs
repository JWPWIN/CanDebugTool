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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            Btn_DisplayCanMatix = new System.Windows.Forms.Button();
            Btn_DisplayComUpper = new System.Windows.Forms.Button();
            textBox_GlobalLog = new System.Windows.Forms.TextBox();
            tabControl_AllFunsSplit = new System.Windows.Forms.TabControl();
            tabPage_FirstPage = new System.Windows.Forms.TabPage();
            tabPage_CanMatrix = new System.Windows.Forms.TabPage();
            tabPage_ComUpper = new System.Windows.Forms.TabPage();
            tabControl_AllFunsSplit.SuspendLayout();
            SuspendLayout();
            // 
            // Btn_DisplayCanMatix
            // 
            Btn_DisplayCanMatix.Location = new System.Drawing.Point(21, 423);
            Btn_DisplayCanMatix.Margin = new System.Windows.Forms.Padding(4);
            Btn_DisplayCanMatix.Name = "Btn_DisplayCanMatix";
            Btn_DisplayCanMatix.Size = new System.Drawing.Size(112, 33);
            Btn_DisplayCanMatix.TabIndex = 2;
            Btn_DisplayCanMatix.Text = "功能-DBC管理";
            Btn_DisplayCanMatix.UseVisualStyleBackColor = true;
            Btn_DisplayCanMatix.Click += Btn_DisplayDbcDataManager_Click;
            // 
            // Btn_DisplayComUpper
            // 
            Btn_DisplayComUpper.Location = new System.Drawing.Point(141, 423);
            Btn_DisplayComUpper.Margin = new System.Windows.Forms.Padding(4);
            Btn_DisplayComUpper.Name = "Btn_DisplayComUpper";
            Btn_DisplayComUpper.Size = new System.Drawing.Size(112, 33);
            Btn_DisplayComUpper.TabIndex = 3;
            Btn_DisplayComUpper.Text = "功能-通信上位机";
            Btn_DisplayComUpper.UseVisualStyleBackColor = true;
            Btn_DisplayComUpper.Click += Btn_DisplayComUpper_Click;
            // 
            // textBox_GlobalLog
            // 
            textBox_GlobalLog.Location = new System.Drawing.Point(3, 472);
            textBox_GlobalLog.Multiline = true;
            textBox_GlobalLog.Name = "textBox_GlobalLog";
            textBox_GlobalLog.ReadOnly = true;
            textBox_GlobalLog.Size = new System.Drawing.Size(384, 128);
            textBox_GlobalLog.TabIndex = 4;
            // 
            // tabControl_AllFunsSplit
            // 
            tabControl_AllFunsSplit.Controls.Add(tabPage_FirstPage);
            tabControl_AllFunsSplit.Controls.Add(tabPage_CanMatrix);
            tabControl_AllFunsSplit.Controls.Add(tabPage_ComUpper);
            tabControl_AllFunsSplit.Location = new System.Drawing.Point(3, 2);
            tabControl_AllFunsSplit.Name = "tabControl_AllFunsSplit";
            tabControl_AllFunsSplit.SelectedIndex = 0;
            tabControl_AllFunsSplit.Size = new System.Drawing.Size(785, 414);
            tabControl_AllFunsSplit.TabIndex = 5;
            // 
            // tabPage_FirstPage
            // 
            tabPage_FirstPage.Location = new System.Drawing.Point(4, 26);
            tabPage_FirstPage.Name = "tabPage_FirstPage";
            tabPage_FirstPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage_FirstPage.Size = new System.Drawing.Size(777, 384);
            tabPage_FirstPage.TabIndex = 0;
            tabPage_FirstPage.Text = "首页";
            tabPage_FirstPage.UseVisualStyleBackColor = true;
            // 
            // tabPage_CanMatrix
            // 
            tabPage_CanMatrix.Location = new System.Drawing.Point(4, 26);
            tabPage_CanMatrix.Name = "tabPage_CanMatrix";
            tabPage_CanMatrix.Padding = new System.Windows.Forms.Padding(3);
            tabPage_CanMatrix.Size = new System.Drawing.Size(777, 384);
            tabPage_CanMatrix.TabIndex = 1;
            tabPage_CanMatrix.Text = "CAN矩阵";
            tabPage_CanMatrix.UseVisualStyleBackColor = true;
            // 
            // tabPage_ComUpper
            // 
            tabPage_ComUpper.Location = new System.Drawing.Point(4, 26);
            tabPage_ComUpper.Name = "tabPage_ComUpper";
            tabPage_ComUpper.Padding = new System.Windows.Forms.Padding(3);
            tabPage_ComUpper.Size = new System.Drawing.Size(777, 384);
            tabPage_ComUpper.TabIndex = 2;
            tabPage_ComUpper.Text = "通信上位机";
            tabPage_ComUpper.UseVisualStyleBackColor = true;
            // 
            // MainWin
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 600);
            Controls.Add(tabControl_AllFunsSplit);
            Controls.Add(textBox_GlobalLog);
            Controls.Add(Btn_DisplayComUpper);
            Controls.Add(Btn_DisplayCanMatix);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "MainWin";
            Text = "CAN调试工具";
            tabControl_AllFunsSplit.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btn_DisplayCanMatix;
        private System.Windows.Forms.Button Btn_DisplayComUpper;
        private System.Windows.Forms.TextBox textBox_GlobalLog;
        private System.Windows.Forms.TabControl tabControl_AllFunsSplit;
        private System.Windows.Forms.TabPage tabPage_FirstPage;
        private System.Windows.Forms.TabPage tabPage_CanMatrix;
        private System.Windows.Forms.TabPage tabPage_ComUpper;
    }
}


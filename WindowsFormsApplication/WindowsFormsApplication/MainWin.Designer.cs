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
            textBox_GlobalLog = new System.Windows.Forms.TextBox();
            tabControl_AllFunsSplit = new System.Windows.Forms.TabControl();
            tabPage_FirstPage = new System.Windows.Forms.TabPage();
            uI_DbcDataManager = new WindowsFormsApplication.UI.UI_DbcDataManager();
            tabPage_CanMatrix = new System.Windows.Forms.TabPage();
            tabPage_ComUpper = new System.Windows.Forms.TabPage();
            uI_CanMatrix = new WindowsFormsApplication.UI.UI_CanMatrix();
            uI_ComUpper = new WindowsFormsApplication.UI.UI_ComUpper();
            tabControl_AllFunsSplit.SuspendLayout();
            tabPage_FirstPage.SuspendLayout();
            tabPage_CanMatrix.SuspendLayout();
            tabPage_ComUpper.SuspendLayout();
            SuspendLayout();
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
            tabPage_FirstPage.Controls.Add(uI_DbcDataManager);
            tabPage_FirstPage.Location = new System.Drawing.Point(4, 26);
            tabPage_FirstPage.Name = "tabPage_FirstPage";
            tabPage_FirstPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage_FirstPage.Size = new System.Drawing.Size(777, 384);
            tabPage_FirstPage.TabIndex = 0;
            tabPage_FirstPage.Text = "首页";
            tabPage_FirstPage.UseVisualStyleBackColor = true;
            // 
            // uI_DbcDataManager
            // 
            uI_DbcDataManager.Dock = System.Windows.Forms.DockStyle.Fill;
            uI_DbcDataManager.Location = new System.Drawing.Point(3, 3);
            uI_DbcDataManager.Name = "uI_DbcDataManager";
            uI_DbcDataManager.Size = new System.Drawing.Size(771, 378);
            uI_DbcDataManager.TabIndex = 0;
            // 
            // tabPage_CanMatrix
            // 
            tabPage_CanMatrix.Controls.Add(uI_CanMatrix);
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
            tabPage_ComUpper.Controls.Add(uI_ComUpper);
            tabPage_ComUpper.Location = new System.Drawing.Point(4, 26);
            tabPage_ComUpper.Name = "tabPage_ComUpper";
            tabPage_ComUpper.Padding = new System.Windows.Forms.Padding(3);
            tabPage_ComUpper.Size = new System.Drawing.Size(777, 384);
            tabPage_ComUpper.TabIndex = 2;
            tabPage_ComUpper.Text = "通信上位机";
            tabPage_ComUpper.UseVisualStyleBackColor = true;
            // 
            // uI_CanMatrix
            // 
            uI_CanMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            uI_CanMatrix.Location = new System.Drawing.Point(3, 3);
            uI_CanMatrix.Name = "uI_CanMatrix";
            uI_CanMatrix.Size = new System.Drawing.Size(771, 378);
            uI_CanMatrix.TabIndex = 0;
            // 
            // uI_ComUpper
            // 
            uI_ComUpper.Dock = System.Windows.Forms.DockStyle.Fill;
            uI_ComUpper.Location = new System.Drawing.Point(3, 3);
            uI_ComUpper.Name = "uI_ComUpper";
            uI_ComUpper.Size = new System.Drawing.Size(771, 378);
            uI_ComUpper.TabIndex = 0;
            // 
            // MainWin
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 600);
            Controls.Add(tabControl_AllFunsSplit);
            Controls.Add(textBox_GlobalLog);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "MainWin";
            Text = "CAN调试工具";
            tabControl_AllFunsSplit.ResumeLayout(false);
            tabPage_FirstPage.ResumeLayout(false);
            tabPage_CanMatrix.ResumeLayout(false);
            tabPage_ComUpper.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox_GlobalLog;
        private System.Windows.Forms.TabControl tabControl_AllFunsSplit;
        private System.Windows.Forms.TabPage tabPage_FirstPage;
        private System.Windows.Forms.TabPage tabPage_CanMatrix;
        private System.Windows.Forms.TabPage tabPage_ComUpper;
        private UI.UI_DbcDataManager uI_DbcDataManager;
        private UI.UI_CanMatrix uI_CanMatrix;
        private UI.UI_ComUpper uI_ComUpper;
    }
}


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
            SuspendLayout();
            // 
            // Btn_DisplayCanMatix
            // 
            Btn_DisplayCanMatix.Location = new System.Drawing.Point(13, 31);
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
            Btn_DisplayComUpper.Location = new System.Drawing.Point(133, 31);
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
            textBox_GlobalLog.Location = new System.Drawing.Point(379, 361);
            textBox_GlobalLog.Multiline = true;
            textBox_GlobalLog.Name = "textBox_GlobalLog";
            textBox_GlobalLog.ReadOnly = true;
            textBox_GlobalLog.Size = new System.Drawing.Size(239, 99);
            textBox_GlobalLog.TabIndex = 4;
            // 
            // MainWin
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(620, 462);
            Controls.Add(textBox_GlobalLog);
            Controls.Add(Btn_DisplayComUpper);
            Controls.Add(Btn_DisplayCanMatix);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "MainWin";
            Text = "CAN调试工具";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btn_DisplayCanMatix;
        private System.Windows.Forms.Button Btn_DisplayComUpper;
        private System.Windows.Forms.TextBox textBox_GlobalLog;
    }
}


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
            this.Btn_DisplayCanMatix = new System.Windows.Forms.Button();
            this.Btn_DisplayComUpper = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Btn_DisplayCanMatix
            // 
            this.Btn_DisplayCanMatix.Location = new System.Drawing.Point(127, 20);
            this.Btn_DisplayCanMatix.Name = "Btn_DisplayCanMatix";
            this.Btn_DisplayCanMatix.Size = new System.Drawing.Size(136, 36);
            this.Btn_DisplayCanMatix.TabIndex = 2;
            this.Btn_DisplayCanMatix.Text = "功能-DBC管理";
            this.Btn_DisplayCanMatix.UseVisualStyleBackColor = true;
            this.Btn_DisplayCanMatix.Click += new System.EventHandler(this.Btn_DisplayDbcDataManager_Click);
            // 
            // Btn_DisplayComUpper
            // 
            this.Btn_DisplayComUpper.Location = new System.Drawing.Point(127, 74);
            this.Btn_DisplayComUpper.Name = "Btn_DisplayComUpper";
            this.Btn_DisplayComUpper.Size = new System.Drawing.Size(136, 37);
            this.Btn_DisplayComUpper.TabIndex = 3;
            this.Btn_DisplayComUpper.Text = "功能-通信上位机";
            this.Btn_DisplayComUpper.UseVisualStyleBackColor = true;
            this.Btn_DisplayComUpper.Click += new System.EventHandler(this.Btn_DisplayComUpper_Click);
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.Btn_DisplayComUpper);
            this.Controls.Add(this.Btn_DisplayCanMatix);
            this.Name = "MainWin";
            this.Text = "CAN调试工具";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_DisplayCanMatix;
        private System.Windows.Forms.Button Btn_DisplayComUpper;
    }
}


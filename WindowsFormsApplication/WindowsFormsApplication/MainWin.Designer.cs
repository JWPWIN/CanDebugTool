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
            this.Btn_ImpExcelDBC = new System.Windows.Forms.Button();
            this.Btn_DisplayCanMatix = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Btn_ImpExcelDBC
            // 
            this.Btn_ImpExcelDBC.Location = new System.Drawing.Point(121, 58);
            this.Btn_ImpExcelDBC.Name = "Btn_ImpExcelDBC";
            this.Btn_ImpExcelDBC.Size = new System.Drawing.Size(75, 23);
            this.Btn_ImpExcelDBC.TabIndex = 0;
            this.Btn_ImpExcelDBC.Text = "导入Excel";
            this.Btn_ImpExcelDBC.UseVisualStyleBackColor = true;
            this.Btn_ImpExcelDBC.Click += new System.EventHandler(this.Btn_ImpExcelDBC_Click);
            // 
            // Btn_DisplayCanMatix
            // 
            this.Btn_DisplayCanMatix.Location = new System.Drawing.Point(108, 115);
            this.Btn_DisplayCanMatix.Name = "Btn_DisplayCanMatix";
            this.Btn_DisplayCanMatix.Size = new System.Drawing.Size(99, 23);
            this.Btn_DisplayCanMatix.TabIndex = 2;
            this.Btn_DisplayCanMatix.Text = "显示CAN协议";
            this.Btn_DisplayCanMatix.UseVisualStyleBackColor = true;
            this.Btn_DisplayCanMatix.Click += new System.EventHandler(this.Btn_DisplayCanMatix_Click);
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 635);
            this.Controls.Add(this.Btn_DisplayCanMatix);
            this.Controls.Add(this.Btn_ImpExcelDBC);
            this.Name = "MainWin";
            this.Text = "MainWin";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_ImpExcelDBC;
        private System.Windows.Forms.Button Btn_DisplayCanMatix;
    }
}


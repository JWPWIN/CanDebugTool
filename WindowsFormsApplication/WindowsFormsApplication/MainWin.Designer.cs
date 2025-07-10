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
            this.ImpExcelDBC = new System.Windows.Forms.Button();
            this.MsglistView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // ImpExcelDBC
            // 
            this.ImpExcelDBC.Location = new System.Drawing.Point(121, 58);
            this.ImpExcelDBC.Name = "ImpExcelDBC";
            this.ImpExcelDBC.Size = new System.Drawing.Size(75, 23);
            this.ImpExcelDBC.TabIndex = 0;
            this.ImpExcelDBC.Text = "导入Excel";
            this.ImpExcelDBC.UseVisualStyleBackColor = true;
            this.ImpExcelDBC.Click += new System.EventHandler(this.ImpExcelDBC_Click);
            // 
            // MsglistView
            // 
            this.MsglistView.HideSelection = false;
            this.MsglistView.Location = new System.Drawing.Point(95, 181);
            this.MsglistView.Name = "MsglistView";
            this.MsglistView.Size = new System.Drawing.Size(121, 97);
            this.MsglistView.TabIndex = 1;
            this.MsglistView.UseCompatibleStateImageBehavior = false;
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 635);
            this.Controls.Add(this.MsglistView);
            this.Controls.Add(this.ImpExcelDBC);
            this.Name = "MainWin";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ImpExcelDBC;
        private System.Windows.Forms.ListView MsglistView;
    }
}


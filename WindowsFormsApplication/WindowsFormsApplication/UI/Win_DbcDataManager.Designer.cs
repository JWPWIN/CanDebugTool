namespace WindowsFormsApplication.UI
{
    partial class Win_DbcDataManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Btn_ExportXml = new System.Windows.Forms.Button();
            this.Btn_GntCanCode = new System.Windows.Forms.Button();
            this.Btn_ExportDbc = new System.Windows.Forms.Button();
            this.Btn_DisplayCanMatix = new System.Windows.Forms.Button();
            this.Btn_ImpExcelDBC = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Btn_ExportXml
            // 
            this.Btn_ExportXml.Location = new System.Drawing.Point(142, 169);
            this.Btn_ExportXml.Name = "Btn_ExportXml";
            this.Btn_ExportXml.Size = new System.Drawing.Size(108, 39);
            this.Btn_ExportXml.TabIndex = 10;
            this.Btn_ExportXml.Text = "导出Xml";
            this.Btn_ExportXml.UseVisualStyleBackColor = true;
            this.Btn_ExportXml.Click += new System.EventHandler(this.Btn_ExportXml_Click);
            // 
            // Btn_GntCanCode
            // 
            this.Btn_GntCanCode.Location = new System.Drawing.Point(142, 214);
            this.Btn_GntCanCode.Name = "Btn_GntCanCode";
            this.Btn_GntCanCode.Size = new System.Drawing.Size(108, 39);
            this.Btn_GntCanCode.TabIndex = 9;
            this.Btn_GntCanCode.Text = "生成CAN模板代码";
            this.Btn_GntCanCode.UseVisualStyleBackColor = true;
            this.Btn_GntCanCode.Click += new System.EventHandler(this.Btn_GntCanCode_Click);
            // 
            // Btn_ExportDbc
            // 
            this.Btn_ExportDbc.Location = new System.Drawing.Point(142, 124);
            this.Btn_ExportDbc.Name = "Btn_ExportDbc";
            this.Btn_ExportDbc.Size = new System.Drawing.Size(108, 39);
            this.Btn_ExportDbc.TabIndex = 8;
            this.Btn_ExportDbc.Text = "导出DBC";
            this.Btn_ExportDbc.UseVisualStyleBackColor = true;
            this.Btn_ExportDbc.Click += new System.EventHandler(this.Btn_ExportDbc_Click);
            // 
            // Btn_DisplayCanMatix
            // 
            this.Btn_DisplayCanMatix.Location = new System.Drawing.Point(142, 82);
            this.Btn_DisplayCanMatix.Name = "Btn_DisplayCanMatix";
            this.Btn_DisplayCanMatix.Size = new System.Drawing.Size(108, 36);
            this.Btn_DisplayCanMatix.TabIndex = 7;
            this.Btn_DisplayCanMatix.Text = "显示CAN协议";
            this.Btn_DisplayCanMatix.UseVisualStyleBackColor = true;
            this.Btn_DisplayCanMatix.Click += new System.EventHandler(this.Btn_DisplayCanMatix_Click);
            // 
            // Btn_ImpExcelDBC
            // 
            this.Btn_ImpExcelDBC.Location = new System.Drawing.Point(142, 43);
            this.Btn_ImpExcelDBC.Name = "Btn_ImpExcelDBC";
            this.Btn_ImpExcelDBC.Size = new System.Drawing.Size(108, 33);
            this.Btn_ImpExcelDBC.TabIndex = 6;
            this.Btn_ImpExcelDBC.Text = "导入Excel数据";
            this.Btn_ImpExcelDBC.UseVisualStyleBackColor = true;
            this.Btn_ImpExcelDBC.Click += new System.EventHandler(this.Btn_ImpExcelDBC_Click);
            // 
            // Win_DbcDataManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.Btn_ExportXml);
            this.Controls.Add(this.Btn_GntCanCode);
            this.Controls.Add(this.Btn_ExportDbc);
            this.Controls.Add(this.Btn_DisplayCanMatix);
            this.Controls.Add(this.Btn_ImpExcelDBC);
            this.Name = "Win_DbcDataManager";
            this.Text = "DBC管理";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_ExportXml;
        private System.Windows.Forms.Button Btn_GntCanCode;
        private System.Windows.Forms.Button Btn_ExportDbc;
        private System.Windows.Forms.Button Btn_DisplayCanMatix;
        private System.Windows.Forms.Button Btn_ImpExcelDBC;
    }
}
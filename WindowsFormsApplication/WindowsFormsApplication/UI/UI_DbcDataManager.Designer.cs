namespace WindowsFormsApplication.UI
{
    partial class UI_DbcDataManager
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            button_ImportTxtDbc = new System.Windows.Forms.Button();
            Btn_ExportXml = new System.Windows.Forms.Button();
            Btn_GntCanCode = new System.Windows.Forms.Button();
            Btn_ExportDbc = new System.Windows.Forms.Button();
            Btn_ImpExcelDBC = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // button_ImportTxtDbc
            // 
            button_ImportTxtDbc.Location = new System.Drawing.Point(176, 29);
            button_ImportTxtDbc.Margin = new System.Windows.Forms.Padding(4);
            button_ImportTxtDbc.Name = "button_ImportTxtDbc";
            button_ImportTxtDbc.Size = new System.Drawing.Size(126, 47);
            button_ImportTxtDbc.TabIndex = 17;
            button_ImportTxtDbc.Text = "导入DBC数据";
            button_ImportTxtDbc.UseVisualStyleBackColor = true;
            button_ImportTxtDbc.Click += button_ImportTxtDbc_Click;
            // 
            // Btn_ExportXml
            // 
            Btn_ExportXml.Location = new System.Drawing.Point(43, 207);
            Btn_ExportXml.Margin = new System.Windows.Forms.Padding(4);
            Btn_ExportXml.Name = "Btn_ExportXml";
            Btn_ExportXml.Size = new System.Drawing.Size(126, 55);
            Btn_ExportXml.TabIndex = 16;
            Btn_ExportXml.Text = "导出Xml";
            Btn_ExportXml.UseVisualStyleBackColor = true;
            Btn_ExportXml.Click += Btn_ExportXml_Click;
            // 
            // Btn_GntCanCode
            // 
            Btn_GntCanCode.Location = new System.Drawing.Point(43, 271);
            Btn_GntCanCode.Margin = new System.Windows.Forms.Padding(4);
            Btn_GntCanCode.Name = "Btn_GntCanCode";
            Btn_GntCanCode.Size = new System.Drawing.Size(126, 55);
            Btn_GntCanCode.TabIndex = 15;
            Btn_GntCanCode.Text = "生成CAN模板代码";
            Btn_GntCanCode.UseVisualStyleBackColor = true;
            Btn_GntCanCode.Click += Btn_GntCanCode_Click;
            // 
            // Btn_ExportDbc
            // 
            Btn_ExportDbc.Location = new System.Drawing.Point(43, 144);
            Btn_ExportDbc.Margin = new System.Windows.Forms.Padding(4);
            Btn_ExportDbc.Name = "Btn_ExportDbc";
            Btn_ExportDbc.Size = new System.Drawing.Size(126, 55);
            Btn_ExportDbc.TabIndex = 14;
            Btn_ExportDbc.Text = "导出DBC";
            Btn_ExportDbc.UseVisualStyleBackColor = true;
            Btn_ExportDbc.Click += Btn_ExportDbc_Click;
            // 
            // Btn_ImpExcelDBC
            // 
            Btn_ImpExcelDBC.Location = new System.Drawing.Point(43, 29);
            Btn_ImpExcelDBC.Margin = new System.Windows.Forms.Padding(4);
            Btn_ImpExcelDBC.Name = "Btn_ImpExcelDBC";
            Btn_ImpExcelDBC.Size = new System.Drawing.Size(126, 47);
            Btn_ImpExcelDBC.TabIndex = 12;
            Btn_ImpExcelDBC.Text = "导入Excel数据";
            Btn_ImpExcelDBC.UseVisualStyleBackColor = true;
            Btn_ImpExcelDBC.Click += Btn_ImpExcelDBC_Click;
            // 
            // UI_DbcDataManager
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(button_ImportTxtDbc);
            Controls.Add(Btn_ExportXml);
            Controls.Add(Btn_GntCanCode);
            Controls.Add(Btn_ExportDbc);
            Controls.Add(Btn_ImpExcelDBC);
            Name = "UI_DbcDataManager";
            Size = new System.Drawing.Size(774, 533);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button button_ImportTxtDbc;
        private System.Windows.Forms.Button Btn_ExportXml;
        private System.Windows.Forms.Button Btn_GntCanCode;
        private System.Windows.Forms.Button Btn_ExportDbc;
        private System.Windows.Forms.Button Btn_ImpExcelDBC;
    }
}

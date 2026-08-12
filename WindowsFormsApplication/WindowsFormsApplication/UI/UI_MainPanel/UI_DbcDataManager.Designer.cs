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
            panel_ToolsRoot = new System.Windows.Forms.Panel();
            groupBox2 = new System.Windows.Forms.GroupBox();
            flowLayoutPanel_Export = new System.Windows.Forms.FlowLayoutPanel();
            Btn_ImportDbc = new System.Windows.Forms.Button();
            Btn_ExportDbc = new System.Windows.Forms.Button();
            button_ExportExcelDbc = new System.Windows.Forms.Button();
            Btn_ExportXml = new System.Windows.Forms.Button();
            Btn_GntCanCode = new System.Windows.Forms.Button();
            label_ExportHint = new System.Windows.Forms.Label();
            panel_ToolsRoot.SuspendLayout();
            groupBox2.SuspendLayout();
            flowLayoutPanel_Export.SuspendLayout();
            SuspendLayout();
            // 
            // panel_ToolsRoot
            // 
            panel_ToolsRoot.AutoScroll = true;
            panel_ToolsRoot.BackColor = System.Drawing.Color.White;
            panel_ToolsRoot.Controls.Add(groupBox2);
            panel_ToolsRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            panel_ToolsRoot.Location = new System.Drawing.Point(0, 0);
            panel_ToolsRoot.Name = "panel_ToolsRoot";
            panel_ToolsRoot.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            panel_ToolsRoot.Size = new System.Drawing.Size(1000, 600);
            panel_ToolsRoot.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox2.Controls.Add(flowLayoutPanel_Export);
            groupBox2.Controls.Add(label_ExportHint);
            groupBox2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            groupBox2.Location = new System.Drawing.Point(0, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(12, 8, 12, 12);
            groupBox2.Size = new System.Drawing.Size(992, 148);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "导入 / 导出数据";
            // 
            // flowLayoutPanel_Export
            // 
            flowLayoutPanel_Export.AutoSize = true;
            flowLayoutPanel_Export.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flowLayoutPanel_Export.Controls.Add(Btn_ImportDbc);
            flowLayoutPanel_Export.Controls.Add(Btn_ExportDbc);
            flowLayoutPanel_Export.Controls.Add(button_ExportExcelDbc);
            flowLayoutPanel_Export.Controls.Add(Btn_ExportXml);
            flowLayoutPanel_Export.Controls.Add(Btn_GntCanCode);
            flowLayoutPanel_Export.Dock = System.Windows.Forms.DockStyle.Bottom;
            flowLayoutPanel_Export.Location = new System.Drawing.Point(12, 58);
            flowLayoutPanel_Export.Margin = new System.Windows.Forms.Padding(0);
            flowLayoutPanel_Export.Name = "flowLayoutPanel_Export";
            flowLayoutPanel_Export.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            flowLayoutPanel_Export.Size = new System.Drawing.Size(968, 62);
            flowLayoutPanel_Export.TabIndex = 1;
            flowLayoutPanel_Export.WrapContents = true;
            // 
            // Btn_ImportDbc
            // 
            Btn_ImportDbc.AutoSize = true;
            Btn_ImportDbc.Location = new System.Drawing.Point(0, 8);
            Btn_ImportDbc.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            Btn_ImportDbc.MinimumSize = new System.Drawing.Size(96, 31);
            Btn_ImportDbc.Name = "Btn_ImportDbc";
            Btn_ImportDbc.Size = new System.Drawing.Size(96, 31);
            Btn_ImportDbc.TabIndex = 0;
            Btn_ImportDbc.Text = "导入 DBC";
            Btn_ImportDbc.UseVisualStyleBackColor = true;
            Btn_ImportDbc.Click += Btn_ImportDbc_Click;
            // 
            // Btn_ExportDbc
            // 
            Btn_ExportDbc.AutoSize = true;
            Btn_ExportDbc.Location = new System.Drawing.Point(106, 8);
            Btn_ExportDbc.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            Btn_ExportDbc.MinimumSize = new System.Drawing.Size(96, 31);
            Btn_ExportDbc.Name = "Btn_ExportDbc";
            Btn_ExportDbc.Size = new System.Drawing.Size(96, 31);
            Btn_ExportDbc.TabIndex = 1;
            Btn_ExportDbc.Text = "导出 DBC";
            Btn_ExportDbc.UseVisualStyleBackColor = true;
            Btn_ExportDbc.Click += Btn_ExportDbc_Click;
            // 
            // button_ExportExcelDbc
            // 
            button_ExportExcelDbc.AutoSize = true;
            button_ExportExcelDbc.Location = new System.Drawing.Point(212, 8);
            button_ExportExcelDbc.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            button_ExportExcelDbc.MinimumSize = new System.Drawing.Size(96, 31);
            button_ExportExcelDbc.Name = "button_ExportExcelDbc";
            button_ExportExcelDbc.Size = new System.Drawing.Size(96, 31);
            button_ExportExcelDbc.TabIndex = 2;
            button_ExportExcelDbc.Text = "导出 Excel";
            button_ExportExcelDbc.UseVisualStyleBackColor = true;
            button_ExportExcelDbc.Click += button_ExportExcelDbc_Click;
            // 
            // Btn_ExportXml
            // 
            Btn_ExportXml.AutoSize = true;
            Btn_ExportXml.Location = new System.Drawing.Point(318, 8);
            Btn_ExportXml.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            Btn_ExportXml.MinimumSize = new System.Drawing.Size(96, 31);
            Btn_ExportXml.Name = "Btn_ExportXml";
            Btn_ExportXml.Size = new System.Drawing.Size(96, 31);
            Btn_ExportXml.TabIndex = 3;
            Btn_ExportXml.Text = "导出 Xml";
            Btn_ExportXml.UseVisualStyleBackColor = true;
            Btn_ExportXml.Click += Btn_ExportXml_Click;
            // 
            // Btn_GntCanCode
            // 
            Btn_GntCanCode.AutoSize = true;
            Btn_GntCanCode.Location = new System.Drawing.Point(424, 8);
            Btn_GntCanCode.Margin = new System.Windows.Forms.Padding(0, 4, 10, 4);
            Btn_GntCanCode.MinimumSize = new System.Drawing.Size(140, 31);
            Btn_GntCanCode.Name = "Btn_GntCanCode";
            Btn_GntCanCode.Size = new System.Drawing.Size(140, 31);
            Btn_GntCanCode.TabIndex = 4;
            Btn_GntCanCode.Text = "导出 CAN 框架代码";
            Btn_GntCanCode.UseVisualStyleBackColor = true;
            Btn_GntCanCode.Click += Btn_GntCanCode_Click;
            // 
            // label_ExportHint
            // 
            label_ExportHint.AutoSize = true;
            label_ExportHint.Dock = System.Windows.Forms.DockStyle.Top;
            label_ExportHint.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            label_ExportHint.Location = new System.Drawing.Point(12, 24);
            label_ExportHint.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            label_ExportHint.Name = "label_ExportHint";
            label_ExportHint.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            label_ExportHint.Size = new System.Drawing.Size(500, 25);
            label_ExportHint.TabIndex = 0;
            label_ExportHint.Text = "可通过顶部文件夹导入 Excel，或在此导入 DBC；导入成功后再执行导出。";
            // 
            // UI_DbcDataManager
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(panel_ToolsRoot);
            Name = "UI_DbcDataManager";
            Size = new System.Drawing.Size(1000, 600);
            panel_ToolsRoot.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            flowLayoutPanel_Export.ResumeLayout(false);
            flowLayoutPanel_Export.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel_ToolsRoot;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Export;
        private System.Windows.Forms.Label label_ExportHint;
        private System.Windows.Forms.Button Btn_ExportXml;
        private System.Windows.Forms.Button Btn_GntCanCode;
        private System.Windows.Forms.Button Btn_ImportDbc;
        private System.Windows.Forms.Button Btn_ExportDbc;
        private System.Windows.Forms.Button button_ExportExcelDbc;
    }
}

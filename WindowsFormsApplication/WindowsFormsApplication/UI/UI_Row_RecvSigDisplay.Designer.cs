namespace WindowsFormsApplication.UI
{
    partial class UI_Row_RecvSigDisplay
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
            label_SigName = new System.Windows.Forms.Label();
            label_SigValue = new System.Windows.Forms.Label();
            label_SigDesc = new System.Windows.Forms.Label();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // label_SigName
            // 
            label_SigName.BackColor = System.Drawing.SystemColors.ActiveCaption;
            label_SigName.Dock = System.Windows.Forms.DockStyle.Fill;
            label_SigName.Location = new System.Drawing.Point(3, 0);
            label_SigName.Name = "label_SigName";
            label_SigName.Size = new System.Drawing.Size(125, 40);
            label_SigName.TabIndex = 1;
            label_SigName.Text = "信号名";
            label_SigName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_SigValue
            // 
            label_SigValue.BackColor = System.Drawing.SystemColors.ActiveBorder;
            label_SigValue.Dock = System.Windows.Forms.DockStyle.Fill;
            label_SigValue.Location = new System.Drawing.Point(134, 0);
            label_SigValue.Name = "label_SigValue";
            label_SigValue.Size = new System.Drawing.Size(125, 40);
            label_SigValue.TabIndex = 2;
            label_SigValue.Text = "信号值";
            label_SigValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_SigDesc
            // 
            label_SigDesc.BackColor = System.Drawing.SystemColors.ActiveCaption;
            label_SigDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            label_SigDesc.Location = new System.Drawing.Point(265, 0);
            label_SigDesc.Name = "label_SigDesc";
            label_SigDesc.Size = new System.Drawing.Size(60, 40);
            label_SigDesc.TabIndex = 3;
            label_SigDesc.Text = "信号描述";
            label_SigDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tableLayoutPanel1.Controls.Add(label_SigName, 0, 0);
            tableLayoutPanel1.Controls.Add(label_SigDesc, 2, 0);
            tableLayoutPanel1.Controls.Add(label_SigValue, 1, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new System.Drawing.Size(328, 40);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // UI_Row_RecvSigDisplay
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(tableLayoutPanel1);
            Name = "UI_Row_RecvSigDisplay";
            Size = new System.Drawing.Size(328, 40);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label label_SigName;
        private System.Windows.Forms.Label label_SigValue;
        private System.Windows.Forms.Label label_SigDesc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

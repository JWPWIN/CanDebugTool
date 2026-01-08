namespace WindowsFormsApplication.UI
{
    partial class UI_CanMatrix
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            MsgGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)MsgGridView).BeginInit();
            SuspendLayout();
            // 
            // MsgGridView
            // 
            MsgGridView.AllowUserToAddRows = false;
            MsgGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            MsgGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            MsgGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            MsgGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            MsgGridView.Location = new System.Drawing.Point(4, 4);
            MsgGridView.Margin = new System.Windows.Forms.Padding(4);
            MsgGridView.Name = "MsgGridView";
            MsgGridView.RowHeadersVisible = false;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            MsgGridView.RowsDefaultCellStyle = dataGridViewCellStyle1;
            MsgGridView.RowTemplate.Height = 20;
            MsgGridView.Size = new System.Drawing.Size(1000, 800);
            MsgGridView.TabIndex = 3;
            // 
            // UI_CanMatrix
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(MsgGridView);
            Name = "UI_CanMatrix";
            Size = new System.Drawing.Size(1800, 800);
            ((System.ComponentModel.ISupportInitialize)MsgGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView MsgGridView;
    }
}

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
            System.Windows.Forms.DataGridViewCellStyle headerStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle rowStyle = new System.Windows.Forms.DataGridViewCellStyle();
            panel_MatrixRoot = new System.Windows.Forms.Panel();
            MsgGridView = new System.Windows.Forms.DataGridView();
            panel_MatrixRoot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MsgGridView).BeginInit();
            SuspendLayout();
            // 
            // panel_MatrixRoot
            // 
            panel_MatrixRoot.BackColor = System.Drawing.Color.White;
            panel_MatrixRoot.Controls.Add(MsgGridView);
            panel_MatrixRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            panel_MatrixRoot.Location = new System.Drawing.Point(0, 0);
            panel_MatrixRoot.Name = "panel_MatrixRoot";
            panel_MatrixRoot.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            panel_MatrixRoot.Size = new System.Drawing.Size(1000, 600);
            panel_MatrixRoot.TabIndex = 0;
            // 
            // MsgGridView
            // 
            MsgGridView.AllowUserToAddRows = false;
            MsgGridView.AllowUserToDeleteRows = false;
            MsgGridView.AllowUserToResizeRows = false;
            MsgGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            MsgGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            MsgGridView.BackgroundColor = System.Drawing.Color.White;
            MsgGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            MsgGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            MsgGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            MsgGridView.ColumnHeadersHeight = 36;
            MsgGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            headerStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            headerStyle.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            headerStyle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            headerStyle.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            headerStyle.Padding = new System.Windows.Forms.Padding(6, 0, 4, 0);
            headerStyle.SelectionBackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            headerStyle.SelectionForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            headerStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            MsgGridView.ColumnHeadersDefaultCellStyle = headerStyle;
            MsgGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            MsgGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            MsgGridView.EnableHeadersVisualStyles = false;
            MsgGridView.GridColor = System.Drawing.Color.FromArgb(226, 232, 240);
            MsgGridView.Location = new System.Drawing.Point(0, 0);
            MsgGridView.Margin = new System.Windows.Forms.Padding(0);
            MsgGridView.MultiSelect = false;
            MsgGridView.Name = "MsgGridView";
            MsgGridView.ReadOnly = true;
            MsgGridView.RowHeadersVisible = false;
            rowStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            rowStyle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            rowStyle.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            rowStyle.Padding = new System.Windows.Forms.Padding(6, 4, 4, 4);
            rowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(214, 226, 248);
            rowStyle.SelectionForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            rowStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            MsgGridView.RowsDefaultCellStyle = rowStyle;
            MsgGridView.RowTemplate.Height = 28;
            MsgGridView.RowTemplate.MinimumHeight = 28;
            MsgGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            MsgGridView.Size = new System.Drawing.Size(1000, 596);
            MsgGridView.TabIndex = 0;
            // 
            // UI_CanMatrix
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            Controls.Add(panel_MatrixRoot);
            Name = "UI_CanMatrix";
            Size = new System.Drawing.Size(1000, 600);
            panel_MatrixRoot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MsgGridView).EndInit();
            ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.Panel panel_MatrixRoot;
        private System.Windows.Forms.DataGridView MsgGridView;
    }
}

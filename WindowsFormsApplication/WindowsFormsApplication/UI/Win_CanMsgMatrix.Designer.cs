using System.Windows.Forms;

namespace WindowsFormsApplication
{
    partial class Win_CanMsgMatrix
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            MsgGridView = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)MsgGridView).BeginInit();
            SuspendLayout();
            // 
            // MsgGridView
            // 
            MsgGridView.AllowUserToAddRows = false;
            MsgGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            MsgGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            MsgGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            MsgGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
            MsgGridView.Location = new System.Drawing.Point(14, 17);
            MsgGridView.Margin = new Padding(4, 4, 4, 4);
            MsgGridView.Name = "MsgGridView";
            MsgGridView.RowHeadersVisible = false;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            MsgGridView.RowsDefaultCellStyle = dataGridViewCellStyle1;
            MsgGridView.RowTemplate.Height = 20;
            MsgGridView.Size = new System.Drawing.Size(1983, 820);
            MsgGridView.TabIndex = 2;
            // 
            // Win_CanMsgMatrix
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1924, 850);
            Controls.Add(MsgGridView);
            Margin = new Padding(4, 4, 4, 4);
            Name = "Win_CanMsgMatrix";
            Text = "CAN通信矩阵";
            ((System.ComponentModel.ISupportInitialize)MsgGridView).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView MsgGridView;
    }
}
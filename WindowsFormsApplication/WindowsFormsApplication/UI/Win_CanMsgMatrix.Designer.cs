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
            this.MsgGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.MsgGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // MsgGridView
            // 
            this.MsgGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MsgGridView.Location = new System.Drawing.Point(12, 12);
            this.MsgGridView.Name = "MsgGridView";
            this.MsgGridView.RowTemplate.Height = 20;
            this.MsgGridView.Size = new System.Drawing.Size(1600, 800);
            this.MsgGridView.TabIndex = 2;
            this.MsgGridView.AllowUserToAddRows = false;//不允许手动添加行
            this.MsgGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;//不可编辑
            //this.MsgGridView.ColumnHeadersVisible = false;
            this.MsgGridView.RowHeadersVisible = false;//列头隐藏
            // 
            // Win_CanMsgMatrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1600, 800);
            this.Controls.Add(this.MsgGridView);
            this.Name = "Win_CanMsgMatrix";
            this.Text = "Win_CanMsgMatrix";
            ((System.ComponentModel.ISupportInitialize)(this.MsgGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView MsgGridView;
    }
}
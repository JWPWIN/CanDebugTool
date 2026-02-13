namespace WindowsFormsApplication.UI.SubWin.SubWin_DiagView
{
    partial class SubWin_DiagView
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
            groupBox1 = new System.Windows.Forms.GroupBox();
            textBox_DiagRespData = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            textBox_DiagReqData = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            textBox_DiagRespID = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            textBox_UdsReqID = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            Btn_SendDiagReq = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(Btn_SendDiagReq);
            groupBox1.Controls.Add(textBox_DiagRespData);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(textBox_DiagReqData);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(textBox_DiagRespID);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(textBox_UdsReqID);
            groupBox1.Controls.Add(label1);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(800, 308);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "UDS诊断";
            // 
            // textBox_DiagRespData
            // 
            textBox_DiagRespData.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox_DiagRespData.Location = new System.Drawing.Point(15, 212);
            textBox_DiagRespData.Name = "textBox_DiagRespData";
            textBox_DiagRespData.Size = new System.Drawing.Size(773, 23);
            textBox_DiagRespData.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(15, 192);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(56, 17);
            label4.TabIndex = 6;
            label4.Text = "响应数据";
            // 
            // textBox_DiagReqData
            // 
            textBox_DiagReqData.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox_DiagReqData.Location = new System.Drawing.Point(15, 90);
            textBox_DiagReqData.Name = "textBox_DiagReqData";
            textBox_DiagReqData.Size = new System.Drawing.Size(773, 23);
            textBox_DiagReqData.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(15, 70);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(160, 17);
            label3.TabIndex = 4;
            label3.Text = "请求数据(字节间空格隔开)：";
            // 
            // textBox_DiagRespID
            // 
            textBox_DiagRespID.Location = new System.Drawing.Point(298, 26);
            textBox_DiagRespID.Name = "textBox_DiagRespID";
            textBox_DiagRespID.Size = new System.Drawing.Size(100, 23);
            textBox_DiagRespID.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(223, 29);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(69, 17);
            label2.TabIndex = 2;
            label2.Text = "诊断响应ID";
            // 
            // textBox_UdsReqID
            // 
            textBox_UdsReqID.Location = new System.Drawing.Point(90, 26);
            textBox_UdsReqID.Name = "textBox_UdsReqID";
            textBox_UdsReqID.Size = new System.Drawing.Size(100, 23);
            textBox_UdsReqID.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(15, 29);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(69, 17);
            label1.TabIndex = 0;
            label1.Text = "诊断请求ID";
            // 
            // Btn_SendDiagReq
            // 
            Btn_SendDiagReq.Location = new System.Drawing.Point(15, 133);
            Btn_SendDiagReq.Name = "Btn_SendDiagReq";
            Btn_SendDiagReq.Size = new System.Drawing.Size(111, 39);
            Btn_SendDiagReq.TabIndex = 8;
            Btn_SendDiagReq.Text = "发送请求";
            Btn_SendDiagReq.UseVisualStyleBackColor = true;
            Btn_SendDiagReq.Click += Btn_SendDiagReq_Click;
            // 
            // SubWin_DiagView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(groupBox1);
            Name = "SubWin_DiagView";
            Text = "SubWin_DiagView";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_UdsReqID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_DiagRespID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_DiagReqData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_DiagRespData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Btn_SendDiagReq;
    }
}
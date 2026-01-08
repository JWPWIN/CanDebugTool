namespace WindowsFormsApplication.UI
{
    partial class UI_ComUpper
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
            groupBox2 = new System.Windows.Forms.GroupBox();
            Btn_DisconnectDevice = new System.Windows.Forms.Button();
            label_DBitRate = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label_ABitRate = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            comboBox_CanType = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            comboBox_CanDeviceType = new System.Windows.Forms.ComboBox();
            Btn_ConnectDevice = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            radioButton2 = new System.Windows.Forms.RadioButton();
            radioButton1 = new System.Windows.Forms.RadioButton();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(Btn_DisconnectDevice);
            groupBox2.Controls.Add(label_DBitRate);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(label_ABitRate);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(comboBox_CanType);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(comboBox_CanDeviceType);
            groupBox2.Controls.Add(Btn_ConnectDevice);
            groupBox2.Location = new System.Drawing.Point(4, 15);
            groupBox2.Margin = new System.Windows.Forms.Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4);
            groupBox2.Size = new System.Drawing.Size(453, 150);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "设备管理";
            // 
            // Btn_DisconnectDevice
            // 
            Btn_DisconnectDevice.Location = new System.Drawing.Point(103, 101);
            Btn_DisconnectDevice.Margin = new System.Windows.Forms.Padding(4);
            Btn_DisconnectDevice.Name = "Btn_DisconnectDevice";
            Btn_DisconnectDevice.Size = new System.Drawing.Size(74, 41);
            Btn_DisconnectDevice.TabIndex = 11;
            Btn_DisconnectDevice.Text = "断开连接";
            Btn_DisconnectDevice.UseVisualStyleBackColor = true;
            // 
            // label_DBitRate
            // 
            label_DBitRate.AutoSize = true;
            label_DBitRate.Location = new System.Drawing.Point(367, 67);
            label_DBitRate.Name = "label_DBitRate";
            label_DBitRate.Size = new System.Drawing.Size(23, 17);
            label_DBitRate.TabIndex = 10;
            label_DBitRate.Text = "***";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(309, 67);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(44, 17);
            label8.TabIndex = 9;
            label8.Text = "数据域";
            // 
            // label_ABitRate
            // 
            label_ABitRate.AutoSize = true;
            label_ABitRate.Location = new System.Drawing.Point(367, 33);
            label_ABitRate.Name = "label_ABitRate";
            label_ABitRate.Size = new System.Drawing.Size(23, 17);
            label_ABitRate.TabIndex = 8;
            label_ABitRate.Text = "***";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(309, 33);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(44, 17);
            label6.TabIndex = 7;
            label6.Text = "仲裁域";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(22, 71);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(54, 17);
            label2.TabIndex = 4;
            label2.Text = "Can类型";
            // 
            // comboBox_CanType
            // 
            comboBox_CanType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox_CanType.FormattingEnabled = true;
            comboBox_CanType.Items.AddRange(new object[] { "CANFD", "CAN" });
            comboBox_CanType.Location = new System.Drawing.Point(93, 62);
            comboBox_CanType.Name = "comboBox_CanType";
            comboBox_CanType.Size = new System.Drawing.Size(170, 25);
            comboBox_CanType.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(22, 40);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(66, 17);
            label1.TabIndex = 2;
            label1.Text = "Can卡类型";
            // 
            // comboBox_CanDeviceType
            // 
            comboBox_CanDeviceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox_CanDeviceType.FormattingEnabled = true;
            comboBox_CanDeviceType.Items.AddRange(new object[] { "ZCAN_USBCANFD_100U", "ZCAN_USBCANFD_200U", "ZCAN_USBCANFD_MINI" });
            comboBox_CanDeviceType.Location = new System.Drawing.Point(93, 31);
            comboBox_CanDeviceType.Name = "comboBox_CanDeviceType";
            comboBox_CanDeviceType.Size = new System.Drawing.Size(170, 25);
            comboBox_CanDeviceType.TabIndex = 1;
            // 
            // Btn_ConnectDevice
            // 
            Btn_ConnectDevice.Location = new System.Drawing.Point(21, 101);
            Btn_ConnectDevice.Margin = new System.Windows.Forms.Padding(4);
            Btn_ConnectDevice.Name = "Btn_ConnectDevice";
            Btn_ConnectDevice.Size = new System.Drawing.Size(74, 41);
            Btn_ConnectDevice.TabIndex = 0;
            Btn_ConnectDevice.Text = "连接";
            Btn_ConnectDevice.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Location = new System.Drawing.Point(480, 15);
            groupBox1.Margin = new System.Windows.Forms.Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4);
            groupBox1.Size = new System.Drawing.Size(271, 65);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "发送模式";
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new System.Drawing.Point(126, 29);
            radioButton2.Margin = new System.Windows.Forms.Padding(4);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new System.Drawing.Size(75, 21);
            radioButton2.TabIndex = 2;
            radioButton2.TabStop = true;
            radioButton2.Text = "DBC发送";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new System.Drawing.Point(8, 29);
            radioButton1.Margin = new System.Windows.Forms.Padding(4);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new System.Drawing.Size(74, 21);
            radioButton1.TabIndex = 1;
            radioButton1.TabStop = true;
            radioButton1.Text = "报文发送";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // UI_ComUpper
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "UI_ComUpper";
            Size = new System.Drawing.Size(759, 488);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Btn_DisconnectDevice;
        private System.Windows.Forms.Label label_DBitRate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label_ABitRate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_CanType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_CanDeviceType;
        private System.Windows.Forms.Button Btn_ConnectDevice;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}

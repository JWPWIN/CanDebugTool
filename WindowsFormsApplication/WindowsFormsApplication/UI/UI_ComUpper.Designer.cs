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
            label2 = new System.Windows.Forms.Label();
            comboBox_CanType = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            comboBox_CanDeviceType = new System.Windows.Forms.ComboBox();
            Btn_ConnectDevice = new System.Windows.Forms.Button();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(Btn_DisconnectDevice);
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
            Btn_DisconnectDevice.Click += Btn_DisconnectDevice_Click;
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
            Btn_ConnectDevice.Click += Btn_ConnectDevice_Click;
            // 
            // UI_ComUpper
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBox2);
            Name = "UI_ComUpper";
            Size = new System.Drawing.Size(759, 488);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Btn_DisconnectDevice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_CanType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_CanDeviceType;
        private System.Windows.Forms.Button Btn_ConnectDevice;
    }
}

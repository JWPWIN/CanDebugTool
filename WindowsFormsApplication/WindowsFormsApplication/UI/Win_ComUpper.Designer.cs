namespace WindowsFormsApplication.UI
{
    partial class Win_ComUpper
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
            Btn_ConnectDevice = new System.Windows.Forms.Button();
            radioButton1 = new System.Windows.Forms.RadioButton();
            groupBox1 = new System.Windows.Forms.GroupBox();
            radioButton2 = new System.Windows.Forms.RadioButton();
            groupBox2 = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            comboBox_CanDeviceType = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            comboBox_CanType = new System.Windows.Forms.ComboBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // Btn_ConnectDevice
            // 
            Btn_ConnectDevice.Location = new System.Drawing.Point(315, 36);
            Btn_ConnectDevice.Margin = new System.Windows.Forms.Padding(4);
            Btn_ConnectDevice.Name = "Btn_ConnectDevice";
            Btn_ConnectDevice.Size = new System.Drawing.Size(130, 65);
            Btn_ConnectDevice.TabIndex = 0;
            Btn_ConnectDevice.Text = "连接";
            Btn_ConnectDevice.UseVisualStyleBackColor = true;
            Btn_ConnectDevice.Click += Btn_ConnectDevice_Click;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new System.Drawing.Point(7, 28);
            radioButton1.Margin = new System.Windows.Forms.Padding(4);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new System.Drawing.Size(74, 21);
            radioButton1.TabIndex = 1;
            radioButton1.TabStop = true;
            radioButton1.Text = "报文发送";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Location = new System.Drawing.Point(500, 13);
            groupBox1.Margin = new System.Windows.Forms.Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4);
            groupBox1.Size = new System.Drawing.Size(271, 65);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "发送模式";
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new System.Drawing.Point(125, 28);
            radioButton2.Margin = new System.Windows.Forms.Padding(4);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new System.Drawing.Size(75, 21);
            radioButton2.TabIndex = 2;
            radioButton2.TabStop = true;
            radioButton2.Text = "DBC发送";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(comboBox_CanType);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(comboBox_CanDeviceType);
            groupBox2.Controls.Add(Btn_ConnectDevice);
            groupBox2.Location = new System.Drawing.Point(24, 13);
            groupBox2.Margin = new System.Windows.Forms.Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4);
            groupBox2.Size = new System.Drawing.Size(453, 124);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "发送模式";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(21, 39);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(66, 17);
            label1.TabIndex = 2;
            label1.Text = "Can卡类型";
            // 
            // comboBox_CanDeviceType
            // 
            comboBox_CanDeviceType.FormattingEnabled = true;
            comboBox_CanDeviceType.Items.AddRange(new object[] { "ZCAN_USBCANFD_100U", "ZCAN_USBCANFD_200U", "ZCAN_USBCANFD_MINI" });
            comboBox_CanDeviceType.Location = new System.Drawing.Point(93, 31);
            comboBox_CanDeviceType.Name = "comboBox_CanDeviceType";
            comboBox_CanDeviceType.Size = new System.Drawing.Size(121, 25);
            comboBox_CanDeviceType.TabIndex = 1;
            comboBox_CanDeviceType.SelectedIndexChanged += comboBox_CanDeviceType_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(21, 84);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(54, 17);
            label2.TabIndex = 4;
            label2.Text = "Can类型";
            // 
            // comboBox_CanType
            // 
            comboBox_CanType.FormattingEnabled = true;
            comboBox_CanType.Items.AddRange(new object[] { "CANFD", "CAN"});
            comboBox_CanType.Location = new System.Drawing.Point(93, 76);
            comboBox_CanType.Name = "comboBox_CanType";
            comboBox_CanType.Size = new System.Drawing.Size(121, 25);
            comboBox_CanType.TabIndex = 3;
            comboBox_CanType.SelectedIndexChanged += comboBox_CanType_SelectedIndexChanged;
            // 
            // Win_ComUpper
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(948, 345);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "Win_ComUpper";
            Text = "通信上位机";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_ConnectDevice;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_CanDeviceType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_CanType;
    }
}
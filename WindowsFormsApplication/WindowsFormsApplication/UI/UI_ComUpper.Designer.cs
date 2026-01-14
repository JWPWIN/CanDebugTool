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
            tableLayoutPanel_RecvMsgArea = new System.Windows.Forms.TableLayoutPanel();
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
            groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox2.Location = new System.Drawing.Point(0, 0);
            groupBox2.Margin = new System.Windows.Forms.Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4);
            groupBox2.Size = new System.Drawing.Size(759, 95);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "设备管理";
            // 
            // Btn_DisconnectDevice
            // 
            Btn_DisconnectDevice.Location = new System.Drawing.Point(264, 53);
            Btn_DisconnectDevice.Margin = new System.Windows.Forms.Padding(4);
            Btn_DisconnectDevice.Name = "Btn_DisconnectDevice";
            Btn_DisconnectDevice.Size = new System.Drawing.Size(74, 24);
            Btn_DisconnectDevice.TabIndex = 11;
            Btn_DisconnectDevice.Text = "断开连接";
            Btn_DisconnectDevice.UseVisualStyleBackColor = true;
            Btn_DisconnectDevice.Click += Btn_DisconnectDevice_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 63);
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
            comboBox_CanType.Location = new System.Drawing.Point(83, 54);
            comboBox_CanType.Name = "comboBox_CanType";
            comboBox_CanType.Size = new System.Drawing.Size(170, 25);
            comboBox_CanType.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 32);
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
            comboBox_CanDeviceType.Location = new System.Drawing.Point(83, 23);
            comboBox_CanDeviceType.Name = "comboBox_CanDeviceType";
            comboBox_CanDeviceType.Size = new System.Drawing.Size(170, 25);
            comboBox_CanDeviceType.TabIndex = 1;
            // 
            // Btn_ConnectDevice
            // 
            Btn_ConnectDevice.Location = new System.Drawing.Point(264, 22);
            Btn_ConnectDevice.Margin = new System.Windows.Forms.Padding(4);
            Btn_ConnectDevice.Name = "Btn_ConnectDevice";
            Btn_ConnectDevice.Size = new System.Drawing.Size(74, 24);
            Btn_ConnectDevice.TabIndex = 0;
            Btn_ConnectDevice.Text = "连接";
            Btn_ConnectDevice.UseVisualStyleBackColor = true;
            Btn_ConnectDevice.Click += Btn_ConnectDevice_Click;
            // 
            // tableLayoutPanel_RecvMsgArea
            // 
            tableLayoutPanel_RecvMsgArea.BackColor = System.Drawing.SystemColors.ActiveCaption;
            tableLayoutPanel_RecvMsgArea.ColumnCount = 1;
            tableLayoutPanel_RecvMsgArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel_RecvMsgArea.Dock = System.Windows.Forms.DockStyle.Left;
            tableLayoutPanel_RecvMsgArea.Location = new System.Drawing.Point(0, 95);
            tableLayoutPanel_RecvMsgArea.Name = "tableLayoutPanel_RecvMsgArea";
            tableLayoutPanel_RecvMsgArea.RowCount = 10;
            tableLayoutPanel_RecvMsgArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel_RecvMsgArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel_RecvMsgArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel_RecvMsgArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel_RecvMsgArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel_RecvMsgArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel_RecvMsgArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel_RecvMsgArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel_RecvMsgArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel_RecvMsgArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel_RecvMsgArea.Size = new System.Drawing.Size(311, 393);
            tableLayoutPanel_RecvMsgArea.TabIndex = 6;
            // 
            // UI_ComUpper
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel_RecvMsgArea);
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_RecvMsgArea;
    }
}

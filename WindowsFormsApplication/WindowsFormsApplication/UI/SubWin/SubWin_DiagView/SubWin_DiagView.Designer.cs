namespace WindowsFormsApplication.UI.SubWin.SubWin_DiagView
{
    partial class SubWin_DiagView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tableLayout_Root = new System.Windows.Forms.TableLayoutPanel();
            groupBox_Request = new System.Windows.Forms.GroupBox();
            tableLayout_Request = new System.Windows.Forms.TableLayoutPanel();
            flowLayout_Ids = new System.Windows.Forms.FlowLayoutPanel();
            label_ReqId = new System.Windows.Forms.Label();
            textBox_UdsReqID = new System.Windows.Forms.TextBox();
            label_RespId = new System.Windows.Forms.Label();
            textBox_DiagRespID = new System.Windows.Forms.TextBox();
            Btn_RespIdPlus8 = new System.Windows.Forms.Button();
            Btn_RespIdPlus80 = new System.Windows.Forms.Button();
            label_FrameType = new System.Windows.Forms.Label();
            comboBox_DiagFrameType = new System.Windows.Forms.ComboBox();
            label_Padding = new System.Windows.Forms.Label();
            comboBox_DiagPadding = new System.Windows.Forms.ComboBox();
            label_Timeout = new System.Windows.Forms.Label();
            numericUpDown_DiagTimeout = new System.Windows.Forms.NumericUpDown();
            flowLayout_Services = new System.Windows.Forms.FlowLayoutPanel();
            comboBox_DiagSession = new System.Windows.Forms.ComboBox();
            Btn_EnterSession = new System.Windows.Forms.Button();
            label_Did = new System.Windows.Forms.Label();
            textBox_DiagDid = new System.Windows.Forms.TextBox();
            Btn_ReadDid = new System.Windows.Forms.Button();
            Btn_WriteDid = new System.Windows.Forms.Button();
            Btn_ReadDtc = new System.Windows.Forms.Button();
            Btn_ClearDtc = new System.Windows.Forms.Button();
            Btn_EcuReset = new System.Windows.Forms.Button();
            label_ReqData = new System.Windows.Forms.Label();
            textBox_DiagReqData = new System.Windows.Forms.TextBox();
            flowLayout_Buttons = new System.Windows.Forms.FlowLayoutPanel();
            Btn_SendDiagReq = new System.Windows.Forms.Button();
            Btn_CancelDiag = new System.Windows.Forms.Button();
            checkBox_TesterPresent = new System.Windows.Forms.CheckBox();
            numericUpDown_TpPeriod = new System.Windows.Forms.NumericUpDown();
            groupBox_DidList = new System.Windows.Forms.GroupBox();
            tableLayout_Did = new System.Windows.Forms.TableLayoutPanel();
            flowLayout_DidTools = new System.Windows.Forms.FlowLayoutPanel();
            Btn_DidAdd = new System.Windows.Forms.Button();
            Btn_DidRemove = new System.Windows.Forms.Button();
            Btn_DidReadAll = new System.Windows.Forms.Button();
            label_DidWrite = new System.Windows.Forms.Label();
            comboBox_DidWriteFmt = new System.Windows.Forms.ComboBox();
            textBox_DiagWriteData = new System.Windows.Forms.TextBox();
            Btn_DidWriteSelected = new System.Windows.Forms.Button();
            dataGridView_Did = new System.Windows.Forms.DataGridView();
            groupBox_Response = new System.Windows.Forms.GroupBox();
            tableLayout_Response = new System.Windows.Forms.TableLayoutPanel();
            label_RespData = new System.Windows.Forms.Label();
            textBox_DiagRespData = new System.Windows.Forms.TextBox();
            label_Decode = new System.Windows.Forms.Label();
            textBox_DiagDecode = new System.Windows.Forms.TextBox();
            label_History = new System.Windows.Forms.Label();
            listView_History = new System.Windows.Forms.ListView();
            col_HistTime = new System.Windows.Forms.ColumnHeader();
            col_HistReq = new System.Windows.Forms.ColumnHeader();
            col_HistResp = new System.Windows.Forms.ColumnHeader();
            col_HistResult = new System.Windows.Forms.ColumnHeader();
            label_Trace = new System.Windows.Forms.Label();
            textBox_DiagTrace = new System.Windows.Forms.TextBox();
            panel_Status = new System.Windows.Forms.Panel();
            label_DiagStatus = new System.Windows.Forms.Label();
            tableLayout_Root.SuspendLayout();
            groupBox_Request.SuspendLayout();
            tableLayout_Request.SuspendLayout();
            flowLayout_Ids.SuspendLayout();
            flowLayout_Services.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_DiagTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_TpPeriod).BeginInit();
            flowLayout_Buttons.SuspendLayout();
            groupBox_DidList.SuspendLayout();
            tableLayout_Did.SuspendLayout();
            flowLayout_DidTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_Did).BeginInit();
            groupBox_Response.SuspendLayout();
            tableLayout_Response.SuspendLayout();
            panel_Status.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayout_Root
            // 
            tableLayout_Root.ColumnCount = 1;
            tableLayout_Root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout_Root.Controls.Add(groupBox_Request, 0, 0);
            tableLayout_Root.Controls.Add(groupBox_DidList, 0, 1);
            tableLayout_Root.Controls.Add(groupBox_Response, 0, 2);
            tableLayout_Root.Controls.Add(panel_Status, 0, 3);
            tableLayout_Root.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayout_Root.Location = new System.Drawing.Point(0, 0);
            tableLayout_Root.Name = "tableLayout_Root";
            tableLayout_Root.Padding = new System.Windows.Forms.Padding(8);
            tableLayout_Root.RowCount = 4;
            tableLayout_Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayout_Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42F));
            tableLayout_Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58F));
            tableLayout_Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            tableLayout_Root.Size = new System.Drawing.Size(944, 760);
            tableLayout_Root.TabIndex = 0;
            // 
            // groupBox_Request
            // 
            groupBox_Request.AutoSize = true;
            groupBox_Request.Controls.Add(tableLayout_Request);
            groupBox_Request.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox_Request.Location = new System.Drawing.Point(11, 11);
            groupBox_Request.Name = "groupBox_Request";
            groupBox_Request.Padding = new System.Windows.Forms.Padding(8, 6, 8, 8);
            groupBox_Request.Size = new System.Drawing.Size(922, 230);
            groupBox_Request.TabIndex = 0;
            groupBox_Request.TabStop = false;
            groupBox_Request.Text = "诊断请求";
            // 
            // tableLayout_Request
            // 
            tableLayout_Request.AutoSize = true;
            tableLayout_Request.ColumnCount = 1;
            tableLayout_Request.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout_Request.Controls.Add(flowLayout_Ids, 0, 0);
            tableLayout_Request.Controls.Add(flowLayout_Services, 0, 1);
            tableLayout_Request.Controls.Add(label_ReqData, 0, 2);
            tableLayout_Request.Controls.Add(textBox_DiagReqData, 0, 3);
            tableLayout_Request.Controls.Add(flowLayout_Buttons, 0, 4);
            tableLayout_Request.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayout_Request.Location = new System.Drawing.Point(8, 22);
            tableLayout_Request.Name = "tableLayout_Request";
            tableLayout_Request.RowCount = 5;
            tableLayout_Request.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayout_Request.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayout_Request.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayout_Request.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            tableLayout_Request.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayout_Request.Size = new System.Drawing.Size(906, 200);
            tableLayout_Request.TabIndex = 0;
            // 
            // flowLayout_Ids
            // 
            flowLayout_Ids.AutoSize = true;
            flowLayout_Ids.Controls.Add(label_ReqId);
            flowLayout_Ids.Controls.Add(textBox_UdsReqID);
            flowLayout_Ids.Controls.Add(label_RespId);
            flowLayout_Ids.Controls.Add(textBox_DiagRespID);
            flowLayout_Ids.Controls.Add(Btn_RespIdPlus8);
            flowLayout_Ids.Controls.Add(Btn_RespIdPlus80);
            flowLayout_Ids.Controls.Add(label_FrameType);
            flowLayout_Ids.Controls.Add(comboBox_DiagFrameType);
            flowLayout_Ids.Controls.Add(label_Padding);
            flowLayout_Ids.Controls.Add(comboBox_DiagPadding);
            flowLayout_Ids.Controls.Add(label_Timeout);
            flowLayout_Ids.Controls.Add(numericUpDown_DiagTimeout);
            flowLayout_Ids.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayout_Ids.Location = new System.Drawing.Point(0, 0);
            flowLayout_Ids.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            flowLayout_Ids.Name = "flowLayout_Ids";
            flowLayout_Ids.Size = new System.Drawing.Size(906, 31);
            flowLayout_Ids.TabIndex = 0;
            flowLayout_Ids.WrapContents = true;
            // 
            // label_ReqId
            // 
            label_ReqId.AutoSize = true;
            label_ReqId.Margin = new System.Windows.Forms.Padding(0, 6, 4, 0);
            label_ReqId.Name = "label_ReqId";
            label_ReqId.Size = new System.Drawing.Size(47, 17);
            label_ReqId.TabIndex = 0;
            label_ReqId.Text = "请求ID";
            // 
            // textBox_UdsReqID
            // 
            textBox_UdsReqID.Location = new System.Drawing.Point(51, 3);
            textBox_UdsReqID.Margin = new System.Windows.Forms.Padding(0, 3, 12, 3);
            textBox_UdsReqID.Name = "textBox_UdsReqID";
            textBox_UdsReqID.PlaceholderText = "0x7E0";
            textBox_UdsReqID.Size = new System.Drawing.Size(88, 23);
            textBox_UdsReqID.TabIndex = 1;
            textBox_UdsReqID.Text = "0x7E0";
            // 
            // label_RespId
            // 
            label_RespId.AutoSize = true;
            label_RespId.Margin = new System.Windows.Forms.Padding(0, 6, 4, 0);
            label_RespId.Name = "label_RespId";
            label_RespId.Size = new System.Drawing.Size(47, 17);
            label_RespId.TabIndex = 2;
            label_RespId.Text = "响应ID";
            // 
            // textBox_DiagRespID
            // 
            textBox_DiagRespID.Location = new System.Drawing.Point(202, 3);
            textBox_DiagRespID.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            textBox_DiagRespID.Name = "textBox_DiagRespID";
            textBox_DiagRespID.PlaceholderText = "0x7E8";
            textBox_DiagRespID.Size = new System.Drawing.Size(88, 23);
            textBox_DiagRespID.TabIndex = 3;
            textBox_DiagRespID.Text = "0x7E8";
            // 
            // Btn_RespIdPlus8
            // 
            Btn_RespIdPlus8.Location = new System.Drawing.Point(298, 1);
            Btn_RespIdPlus8.Margin = new System.Windows.Forms.Padding(0, 1, 4, 1);
            Btn_RespIdPlus8.Name = "Btn_RespIdPlus8";
            Btn_RespIdPlus8.Size = new System.Drawing.Size(36, 26);
            Btn_RespIdPlus8.TabIndex = 4;
            Btn_RespIdPlus8.Text = "+8";
            Btn_RespIdPlus8.UseVisualStyleBackColor = true;
            Btn_RespIdPlus8.Click += Btn_RespIdPlus8_Click;
            // 
            // Btn_RespIdPlus80
            // 
            Btn_RespIdPlus80.Location = new System.Drawing.Point(338, 1);
            Btn_RespIdPlus80.Margin = new System.Windows.Forms.Padding(0, 1, 12, 1);
            Btn_RespIdPlus80.Name = "Btn_RespIdPlus80";
            Btn_RespIdPlus80.Size = new System.Drawing.Size(44, 26);
            Btn_RespIdPlus80.TabIndex = 5;
            Btn_RespIdPlus80.Text = "+80";
            Btn_RespIdPlus80.UseVisualStyleBackColor = true;
            Btn_RespIdPlus80.Click += Btn_RespIdPlus80_Click;
            // 
            // label_FrameType
            // 
            label_FrameType.AutoSize = true;
            label_FrameType.Margin = new System.Windows.Forms.Padding(0, 6, 4, 0);
            label_FrameType.Name = "label_FrameType";
            label_FrameType.Size = new System.Drawing.Size(44, 17);
            label_FrameType.TabIndex = 4;
            label_FrameType.Text = "帧类型";
            // 
            // comboBox_DiagFrameType
            // 
            comboBox_DiagFrameType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox_DiagFrameType.FormattingEnabled = true;
            comboBox_DiagFrameType.Items.AddRange(new object[] { "CAN FD", "CAN" });
            comboBox_DiagFrameType.Location = new System.Drawing.Point(338, 3);
            comboBox_DiagFrameType.Margin = new System.Windows.Forms.Padding(0, 3, 12, 3);
            comboBox_DiagFrameType.Name = "comboBox_DiagFrameType";
            comboBox_DiagFrameType.Size = new System.Drawing.Size(88, 25);
            comboBox_DiagFrameType.TabIndex = 5;
            // 
            // label_Padding
            // 
            label_Padding.AutoSize = true;
            label_Padding.Margin = new System.Windows.Forms.Padding(0, 6, 4, 0);
            label_Padding.Name = "label_Padding";
            label_Padding.Size = new System.Drawing.Size(32, 17);
            label_Padding.TabIndex = 6;
            label_Padding.Text = "填充";
            // 
            // comboBox_DiagPadding
            // 
            comboBox_DiagPadding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox_DiagPadding.FormattingEnabled = true;
            comboBox_DiagPadding.Items.AddRange(new object[] { "00", "55", "AA", "不填充" });
            comboBox_DiagPadding.Location = new System.Drawing.Point(470, 3);
            comboBox_DiagPadding.Margin = new System.Windows.Forms.Padding(0, 3, 12, 3);
            comboBox_DiagPadding.Name = "comboBox_DiagPadding";
            comboBox_DiagPadding.Size = new System.Drawing.Size(80, 25);
            comboBox_DiagPadding.TabIndex = 7;
            // 
            // label_Timeout
            // 
            label_Timeout.AutoSize = true;
            label_Timeout.Margin = new System.Windows.Forms.Padding(0, 6, 4, 0);
            label_Timeout.Name = "label_Timeout";
            label_Timeout.Size = new System.Drawing.Size(63, 17);
            label_Timeout.TabIndex = 8;
            label_Timeout.Text = "超时(ms)";
            // 
            // numericUpDown_DiagTimeout
            // 
            numericUpDown_DiagTimeout.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numericUpDown_DiagTimeout.Location = new System.Drawing.Point(617, 3);
            numericUpDown_DiagTimeout.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            numericUpDown_DiagTimeout.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
            numericUpDown_DiagTimeout.Minimum = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDown_DiagTimeout.Name = "numericUpDown_DiagTimeout";
            numericUpDown_DiagTimeout.Size = new System.Drawing.Size(80, 23);
            numericUpDown_DiagTimeout.TabIndex = 9;
            numericUpDown_DiagTimeout.Value = new decimal(new int[] { 2000, 0, 0, 0 });
            // 
            // flowLayout_Services
            // 
            flowLayout_Services.AutoSize = true;
            flowLayout_Services.Controls.Add(comboBox_DiagSession);
            flowLayout_Services.Controls.Add(Btn_EnterSession);
            flowLayout_Services.Controls.Add(label_Did);
            flowLayout_Services.Controls.Add(textBox_DiagDid);
            flowLayout_Services.Controls.Add(Btn_ReadDid);
            flowLayout_Services.Controls.Add(Btn_WriteDid);
            flowLayout_Services.Controls.Add(Btn_ReadDtc);
            flowLayout_Services.Controls.Add(Btn_ClearDtc);
            flowLayout_Services.Controls.Add(Btn_EcuReset);
            flowLayout_Services.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayout_Services.Location = new System.Drawing.Point(0, 35);
            flowLayout_Services.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            flowLayout_Services.Name = "flowLayout_Services";
            flowLayout_Services.Size = new System.Drawing.Size(906, 36);
            flowLayout_Services.TabIndex = 1;
            flowLayout_Services.WrapContents = true;
            // 
            // comboBox_DiagSession
            // 
            comboBox_DiagSession.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox_DiagSession.FormattingEnabled = true;
            comboBox_DiagSession.Items.AddRange(new object[] { "默认会话 10 01", "编程会话 10 02", "扩展会话 10 03" });
            comboBox_DiagSession.Location = new System.Drawing.Point(0, 3);
            comboBox_DiagSession.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            comboBox_DiagSession.Name = "comboBox_DiagSession";
            comboBox_DiagSession.Size = new System.Drawing.Size(140, 25);
            comboBox_DiagSession.TabIndex = 0;
            // 
            // Btn_EnterSession
            // 
            Btn_EnterSession.Location = new System.Drawing.Point(146, 1);
            Btn_EnterSession.Margin = new System.Windows.Forms.Padding(0, 1, 10, 1);
            Btn_EnterSession.Name = "Btn_EnterSession";
            Btn_EnterSession.Size = new System.Drawing.Size(80, 28);
            Btn_EnterSession.TabIndex = 1;
            Btn_EnterSession.Text = "进入会话";
            Btn_EnterSession.UseVisualStyleBackColor = true;
            Btn_EnterSession.Click += Btn_EnterSession_Click;
            // 
            // label_Did
            // 
            label_Did.AutoSize = true;
            label_Did.Margin = new System.Windows.Forms.Padding(0, 7, 4, 0);
            label_Did.Name = "label_Did";
            label_Did.Size = new System.Drawing.Size(27, 17);
            label_Did.TabIndex = 2;
            label_Did.Text = "DID";
            // 
            // textBox_DiagDid
            // 
            textBox_DiagDid.Location = new System.Drawing.Point(267, 3);
            textBox_DiagDid.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            textBox_DiagDid.Name = "textBox_DiagDid";
            textBox_DiagDid.PlaceholderText = "F187";
            textBox_DiagDid.Size = new System.Drawing.Size(64, 23);
            textBox_DiagDid.TabIndex = 3;
            textBox_DiagDid.Text = "F187";
            // 
            // Btn_ReadDid
            // 
            Btn_ReadDid.Location = new System.Drawing.Point(337, 1);
            Btn_ReadDid.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            Btn_ReadDid.Name = "Btn_ReadDid";
            Btn_ReadDid.Size = new System.Drawing.Size(72, 28);
            Btn_ReadDid.TabIndex = 4;
            Btn_ReadDid.Text = "读 DID";
            Btn_ReadDid.UseVisualStyleBackColor = true;
            Btn_ReadDid.Click += Btn_ReadDid_Click;
            // 
            // Btn_WriteDid
            // 
            Btn_WriteDid.Location = new System.Drawing.Point(417, 1);
            Btn_WriteDid.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            Btn_WriteDid.Name = "Btn_WriteDid";
            Btn_WriteDid.Size = new System.Drawing.Size(72, 28);
            Btn_WriteDid.TabIndex = 5;
            Btn_WriteDid.Text = "写 DID";
            Btn_WriteDid.UseVisualStyleBackColor = true;
            Btn_WriteDid.Click += Btn_WriteDid_Click;
            // 
            // Btn_ReadDtc
            // 
            Btn_ReadDtc.Location = new System.Drawing.Point(417, 1);
            Btn_ReadDtc.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            Btn_ReadDtc.Name = "Btn_ReadDtc";
            Btn_ReadDtc.Size = new System.Drawing.Size(72, 28);
            Btn_ReadDtc.TabIndex = 5;
            Btn_ReadDtc.Text = "读 DTC";
            Btn_ReadDtc.UseVisualStyleBackColor = true;
            Btn_ReadDtc.Click += Btn_ReadDtc_Click;
            // 
            // Btn_ClearDtc
            // 
            Btn_ClearDtc.Location = new System.Drawing.Point(497, 1);
            Btn_ClearDtc.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            Btn_ClearDtc.Name = "Btn_ClearDtc";
            Btn_ClearDtc.Size = new System.Drawing.Size(72, 28);
            Btn_ClearDtc.TabIndex = 6;
            Btn_ClearDtc.Text = "清 DTC";
            Btn_ClearDtc.UseVisualStyleBackColor = true;
            Btn_ClearDtc.Click += Btn_ClearDtc_Click;
            // 
            // Btn_EcuReset
            // 
            Btn_EcuReset.Location = new System.Drawing.Point(577, 1);
            Btn_EcuReset.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            Btn_EcuReset.Name = "Btn_EcuReset";
            Btn_EcuReset.Size = new System.Drawing.Size(80, 28);
            Btn_EcuReset.TabIndex = 7;
            Btn_EcuReset.Text = "ECU复位";
            Btn_EcuReset.UseVisualStyleBackColor = true;
            Btn_EcuReset.Click += Btn_EcuReset_Click;
            // 
            // label_ReqData
            // 
            label_ReqData.AutoSize = true;
            label_ReqData.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            label_ReqData.Name = "label_ReqData";
            label_ReqData.Size = new System.Drawing.Size(268, 17);
            label_ReqData.TabIndex = 2;
            label_ReqData.Text = "请求数据（十六进制，空格或连续均可，如 22 F1 90）";
            // 
            // textBox_DiagReqData
            // 
            textBox_DiagReqData.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox_DiagReqData.Font = new System.Drawing.Font("Consolas", 10F);
            textBox_DiagReqData.Location = new System.Drawing.Point(0, 90);
            textBox_DiagReqData.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            textBox_DiagReqData.Multiline = true;
            textBox_DiagReqData.Name = "textBox_DiagReqData";
            textBox_DiagReqData.PlaceholderText = "22 F1 90";
            textBox_DiagReqData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox_DiagReqData.Size = new System.Drawing.Size(906, 50);
            textBox_DiagReqData.TabIndex = 3;
            // 
            // flowLayout_Buttons
            // 
            flowLayout_Buttons.AutoSize = true;
            flowLayout_Buttons.Controls.Add(Btn_SendDiagReq);
            flowLayout_Buttons.Controls.Add(Btn_CancelDiag);
            flowLayout_Buttons.Controls.Add(checkBox_TesterPresent);
            flowLayout_Buttons.Controls.Add(numericUpDown_TpPeriod);
            flowLayout_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayout_Buttons.Location = new System.Drawing.Point(0, 146);
            flowLayout_Buttons.Margin = new System.Windows.Forms.Padding(0);
            flowLayout_Buttons.Name = "flowLayout_Buttons";
            flowLayout_Buttons.Size = new System.Drawing.Size(906, 40);
            flowLayout_Buttons.TabIndex = 4;
            // 
            // Btn_SendDiagReq
            // 
            Btn_SendDiagReq.Location = new System.Drawing.Point(0, 0);
            Btn_SendDiagReq.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            Btn_SendDiagReq.Name = "Btn_SendDiagReq";
            Btn_SendDiagReq.Size = new System.Drawing.Size(111, 36);
            Btn_SendDiagReq.TabIndex = 0;
            Btn_SendDiagReq.Text = "发送请求";
            Btn_SendDiagReq.UseVisualStyleBackColor = true;
            Btn_SendDiagReq.Click += Btn_SendDiagReq_Click;
            // 
            // Btn_CancelDiag
            // 
            Btn_CancelDiag.Enabled = false;
            Btn_CancelDiag.Location = new System.Drawing.Point(121, 0);
            Btn_CancelDiag.Margin = new System.Windows.Forms.Padding(0);
            Btn_CancelDiag.Name = "Btn_CancelDiag";
            Btn_CancelDiag.Size = new System.Drawing.Size(88, 36);
            Btn_CancelDiag.TabIndex = 1;
            Btn_CancelDiag.Text = "取消";
            Btn_CancelDiag.UseVisualStyleBackColor = true;
            Btn_CancelDiag.Click += Btn_CancelDiag_Click;
            // 
            // checkBox_TesterPresent
            // 
            checkBox_TesterPresent.AutoSize = true;
            checkBox_TesterPresent.Location = new System.Drawing.Point(225, 8);
            checkBox_TesterPresent.Margin = new System.Windows.Forms.Padding(16, 8, 4, 0);
            checkBox_TesterPresent.Name = "checkBox_TesterPresent";
            checkBox_TesterPresent.Size = new System.Drawing.Size(191, 21);
            checkBox_TesterPresent.TabIndex = 2;
            checkBox_TesterPresent.Text = "Tester Present (3E 80) 周期ms";
            checkBox_TesterPresent.UseVisualStyleBackColor = true;
            checkBox_TesterPresent.CheckedChanged += checkBox_TesterPresent_CheckedChanged;
            // 
            // numericUpDown_TpPeriod
            // 
            numericUpDown_TpPeriod.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numericUpDown_TpPeriod.Location = new System.Drawing.Point(424, 6);
            numericUpDown_TpPeriod.Margin = new System.Windows.Forms.Padding(4, 6, 0, 3);
            numericUpDown_TpPeriod.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown_TpPeriod.Minimum = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDown_TpPeriod.Name = "numericUpDown_TpPeriod";
            numericUpDown_TpPeriod.Size = new System.Drawing.Size(72, 23);
            numericUpDown_TpPeriod.TabIndex = 3;
            numericUpDown_TpPeriod.Value = new decimal(new int[] { 2000, 0, 0, 0 });
            // 
            // groupBox_DidList
            // 
            groupBox_DidList.Controls.Add(tableLayout_Did);
            groupBox_DidList.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox_DidList.Location = new System.Drawing.Point(11, 247);
            groupBox_DidList.Name = "groupBox_DidList";
            groupBox_DidList.Padding = new System.Windows.Forms.Padding(8, 6, 8, 8);
            groupBox_DidList.Size = new System.Drawing.Size(922, 200);
            groupBox_DidList.TabIndex = 1;
            groupBox_DidList.TabStop = false;
            groupBox_DidList.Text = "DID 列表";
            // 
            // tableLayout_Did
            // 
            tableLayout_Did.ColumnCount = 1;
            tableLayout_Did.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout_Did.Controls.Add(flowLayout_DidTools, 0, 0);
            tableLayout_Did.Controls.Add(dataGridView_Did, 0, 1);
            tableLayout_Did.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayout_Did.Location = new System.Drawing.Point(8, 22);
            tableLayout_Did.Name = "tableLayout_Did";
            tableLayout_Did.RowCount = 2;
            tableLayout_Did.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayout_Did.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout_Did.Size = new System.Drawing.Size(906, 170);
            tableLayout_Did.TabIndex = 0;
            // 
            // flowLayout_DidTools
            // 
            flowLayout_DidTools.AutoSize = true;
            flowLayout_DidTools.Controls.Add(Btn_DidAdd);
            flowLayout_DidTools.Controls.Add(Btn_DidRemove);
            flowLayout_DidTools.Controls.Add(Btn_DidReadAll);
            flowLayout_DidTools.Controls.Add(label_DidWrite);
            flowLayout_DidTools.Controls.Add(comboBox_DidWriteFmt);
            flowLayout_DidTools.Controls.Add(textBox_DiagWriteData);
            flowLayout_DidTools.Controls.Add(Btn_DidWriteSelected);
            flowLayout_DidTools.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayout_DidTools.Location = new System.Drawing.Point(0, 0);
            flowLayout_DidTools.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            flowLayout_DidTools.Name = "flowLayout_DidTools";
            flowLayout_DidTools.Size = new System.Drawing.Size(906, 32);
            flowLayout_DidTools.TabIndex = 0;
            // 
            // Btn_DidAdd
            // 
            Btn_DidAdd.Location = new System.Drawing.Point(0, 0);
            Btn_DidAdd.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            Btn_DidAdd.Name = "Btn_DidAdd";
            Btn_DidAdd.Size = new System.Drawing.Size(72, 28);
            Btn_DidAdd.TabIndex = 0;
            Btn_DidAdd.Text = "添加";
            Btn_DidAdd.UseVisualStyleBackColor = true;
            Btn_DidAdd.Click += Btn_DidAdd_Click;
            // 
            // Btn_DidRemove
            // 
            Btn_DidRemove.Location = new System.Drawing.Point(80, 0);
            Btn_DidRemove.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            Btn_DidRemove.Name = "Btn_DidRemove";
            Btn_DidRemove.Size = new System.Drawing.Size(72, 28);
            Btn_DidRemove.TabIndex = 1;
            Btn_DidRemove.Text = "删除";
            Btn_DidRemove.UseVisualStyleBackColor = true;
            Btn_DidRemove.Click += Btn_DidRemove_Click;
            // 
            // Btn_DidReadAll
            // 
            Btn_DidReadAll.Location = new System.Drawing.Point(160, 0);
            Btn_DidReadAll.Margin = new System.Windows.Forms.Padding(0);
            Btn_DidReadAll.Name = "Btn_DidReadAll";
            Btn_DidReadAll.Size = new System.Drawing.Size(96, 28);
            Btn_DidReadAll.TabIndex = 2;
            Btn_DidReadAll.Text = "批量读勾选";
            Btn_DidReadAll.UseVisualStyleBackColor = true;
            Btn_DidReadAll.Click += Btn_DidReadAll_Click;
            // 
            // label_DidWrite
            // 
            label_DidWrite.AutoSize = true;
            label_DidWrite.Margin = new System.Windows.Forms.Padding(16, 7, 4, 0);
            label_DidWrite.Name = "label_DidWrite";
            label_DidWrite.Size = new System.Drawing.Size(32, 17);
            label_DidWrite.TabIndex = 3;
            label_DidWrite.Text = "写入";
            // 
            // comboBox_DidWriteFmt
            // 
            comboBox_DidWriteFmt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox_DidWriteFmt.FormattingEnabled = true;
            comboBox_DidWriteFmt.Items.AddRange(new object[] { "十六进制", "ASCII" });
            comboBox_DidWriteFmt.Location = new System.Drawing.Point(312, 3);
            comboBox_DidWriteFmt.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            comboBox_DidWriteFmt.Name = "comboBox_DidWriteFmt";
            comboBox_DidWriteFmt.Size = new System.Drawing.Size(88, 25);
            comboBox_DidWriteFmt.TabIndex = 4;
            // 
            // textBox_DiagWriteData
            // 
            textBox_DiagWriteData.Location = new System.Drawing.Point(406, 3);
            textBox_DiagWriteData.Margin = new System.Windows.Forms.Padding(0, 3, 8, 3);
            textBox_DiagWriteData.Name = "textBox_DiagWriteData";
            textBox_DiagWriteData.PlaceholderText = "要写入的数据";
            textBox_DiagWriteData.Size = new System.Drawing.Size(200, 23);
            textBox_DiagWriteData.TabIndex = 5;
            // 
            // Btn_DidWriteSelected
            // 
            Btn_DidWriteSelected.Location = new System.Drawing.Point(614, 0);
            Btn_DidWriteSelected.Margin = new System.Windows.Forms.Padding(0);
            Btn_DidWriteSelected.Name = "Btn_DidWriteSelected";
            Btn_DidWriteSelected.Size = new System.Drawing.Size(80, 28);
            Btn_DidWriteSelected.TabIndex = 6;
            Btn_DidWriteSelected.Text = "写选中行";
            Btn_DidWriteSelected.UseVisualStyleBackColor = true;
            Btn_DidWriteSelected.Click += Btn_DidWriteSelected_Click;
            // 
            // dataGridView_Did
            // 
            dataGridView_Did.AllowUserToAddRows = false;
            dataGridView_Did.AllowUserToDeleteRows = false;
            dataGridView_Did.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_Did.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView_Did.Location = new System.Drawing.Point(0, 36);
            dataGridView_Did.Margin = new System.Windows.Forms.Padding(0);
            dataGridView_Did.MultiSelect = false;
            dataGridView_Did.Name = "dataGridView_Did";
            dataGridView_Did.RowHeadersWidth = 28;
            dataGridView_Did.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridView_Did.Size = new System.Drawing.Size(906, 134);
            dataGridView_Did.TabIndex = 1;
            // 
            // groupBox_Response
            // 
            groupBox_Response.Controls.Add(tableLayout_Response);
            groupBox_Response.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox_Response.Location = new System.Drawing.Point(11, 453);
            groupBox_Response.Name = "groupBox_Response";
            groupBox_Response.Padding = new System.Windows.Forms.Padding(8, 6, 8, 8);
            groupBox_Response.Size = new System.Drawing.Size(922, 390);
            groupBox_Response.TabIndex = 2;
            groupBox_Response.TabStop = false;
            groupBox_Response.Text = "诊断响应";
            // 
            // tableLayout_Response
            // 
            tableLayout_Response.ColumnCount = 1;
            tableLayout_Response.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout_Response.Controls.Add(label_RespData, 0, 0);
            tableLayout_Response.Controls.Add(textBox_DiagRespData, 0, 1);
            tableLayout_Response.Controls.Add(label_Decode, 0, 2);
            tableLayout_Response.Controls.Add(textBox_DiagDecode, 0, 3);
            tableLayout_Response.Controls.Add(label_History, 0, 4);
            tableLayout_Response.Controls.Add(listView_History, 0, 5);
            tableLayout_Response.Controls.Add(label_Trace, 0, 6);
            tableLayout_Response.Controls.Add(textBox_DiagTrace, 0, 7);
            tableLayout_Response.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayout_Response.Location = new System.Drawing.Point(8, 22);
            tableLayout_Response.Name = "tableLayout_Response";
            tableLayout_Response.RowCount = 8;
            tableLayout_Response.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayout_Response.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22F));
            tableLayout_Response.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayout_Response.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            tableLayout_Response.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayout_Response.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36F));
            tableLayout_Response.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayout_Response.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26F));
            tableLayout_Response.Size = new System.Drawing.Size(906, 360);
            tableLayout_Response.TabIndex = 0;
            // 
            // label_RespData
            // 
            label_RespData.AutoSize = true;
            label_RespData.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            label_RespData.Name = "label_RespData";
            label_RespData.Size = new System.Drawing.Size(80, 17);
            label_RespData.TabIndex = 0;
            label_RespData.Text = "响应数据 PDU";
            // 
            // textBox_DiagRespData
            // 
            textBox_DiagRespData.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox_DiagRespData.Font = new System.Drawing.Font("Consolas", 10F);
            textBox_DiagRespData.Location = new System.Drawing.Point(0, 21);
            textBox_DiagRespData.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            textBox_DiagRespData.Multiline = true;
            textBox_DiagRespData.Name = "textBox_DiagRespData";
            textBox_DiagRespData.ReadOnly = true;
            textBox_DiagRespData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox_DiagRespData.Size = new System.Drawing.Size(906, 50);
            textBox_DiagRespData.TabIndex = 1;
            // 
            // label_Decode
            // 
            label_Decode.AutoSize = true;
            label_Decode.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            label_Decode.Name = "label_Decode";
            label_Decode.Size = new System.Drawing.Size(32, 17);
            label_Decode.TabIndex = 2;
            label_Decode.Text = "解析";
            // 
            // textBox_DiagDecode
            // 
            textBox_DiagDecode.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox_DiagDecode.Location = new System.Drawing.Point(0, 96);
            textBox_DiagDecode.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            textBox_DiagDecode.Multiline = true;
            textBox_DiagDecode.Name = "textBox_DiagDecode";
            textBox_DiagDecode.ReadOnly = true;
            textBox_DiagDecode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox_DiagDecode.Size = new System.Drawing.Size(906, 36);
            textBox_DiagDecode.TabIndex = 3;
            // 
            // label_History
            // 
            label_History.AutoSize = true;
            label_History.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            label_History.Name = "label_History";
            label_History.Size = new System.Drawing.Size(176, 17);
            label_History.TabIndex = 4;
            label_History.Text = "收发记录（双击回填请求）";
            // 
            // listView_History
            // 
            listView_History.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { col_HistTime, col_HistReq, col_HistResp, col_HistResult });
            listView_History.Dock = System.Windows.Forms.DockStyle.Fill;
            listView_History.FullRowSelect = true;
            listView_History.GridLines = true;
            listView_History.HideSelection = false;
            listView_History.Location = new System.Drawing.Point(0, 154);
            listView_History.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            listView_History.Name = "listView_History";
            listView_History.Size = new System.Drawing.Size(906, 80);
            listView_History.TabIndex = 5;
            listView_History.UseCompatibleStateImageBehavior = false;
            listView_History.View = System.Windows.Forms.View.Details;
            listView_History.DoubleClick += listView_History_DoubleClick;
            // 
            // col_HistTime
            // 
            col_HistTime.Text = "时间";
            col_HistTime.Width = 90;
            // 
            // col_HistReq
            // 
            col_HistReq.Text = "请求";
            col_HistReq.Width = 180;
            // 
            // col_HistResp
            // 
            col_HistResp.Text = "响应";
            col_HistResp.Width = 260;
            // 
            // col_HistResult
            // 
            col_HistResult.Text = "结果";
            col_HistResult.Width = 340;
            // 
            // label_Trace
            // 
            label_Trace.AutoSize = true;
            label_Trace.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            label_Trace.Name = "label_Trace";
            label_Trace.Size = new System.Drawing.Size(80, 17);
            label_Trace.TabIndex = 6;
            label_Trace.Text = "ISO-TP 日志";
            // 
            // textBox_DiagTrace
            // 
            textBox_DiagTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox_DiagTrace.Font = new System.Drawing.Font("Consolas", 9F);
            textBox_DiagTrace.Location = new System.Drawing.Point(0, 260);
            textBox_DiagTrace.Margin = new System.Windows.Forms.Padding(0);
            textBox_DiagTrace.Multiline = true;
            textBox_DiagTrace.Name = "textBox_DiagTrace";
            textBox_DiagTrace.ReadOnly = true;
            textBox_DiagTrace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox_DiagTrace.Size = new System.Drawing.Size(906, 100);
            textBox_DiagTrace.TabIndex = 7;
            // 
            // panel_Status
            // 
            panel_Status.Controls.Add(label_DiagStatus);
            panel_Status.Dock = System.Windows.Forms.DockStyle.Fill;
            panel_Status.Location = new System.Drawing.Point(11, 643);
            panel_Status.Name = "panel_Status";
            panel_Status.Size = new System.Drawing.Size(922, 26);
            panel_Status.TabIndex = 3;
            // 
            // label_DiagStatus
            // 
            label_DiagStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            label_DiagStatus.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            label_DiagStatus.Location = new System.Drawing.Point(0, 0);
            label_DiagStatus.Name = "label_DiagStatus";
            label_DiagStatus.Size = new System.Drawing.Size(922, 26);
            label_DiagStatus.TabIndex = 0;
            label_DiagStatus.Text = "状态：空闲";
            label_DiagStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SubWin_DiagView
            // 
            AcceptButton = Btn_SendDiagReq;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(960, 780);
            Controls.Add(tableLayout_Root);
            MinimumSize = new System.Drawing.Size(780, 600);
            Name = "SubWin_DiagView";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "诊断视图";
            tableLayout_Root.ResumeLayout(false);
            tableLayout_Root.PerformLayout();
            groupBox_Request.ResumeLayout(false);
            groupBox_Request.PerformLayout();
            tableLayout_Request.ResumeLayout(false);
            tableLayout_Request.PerformLayout();
            flowLayout_Ids.ResumeLayout(false);
            flowLayout_Ids.PerformLayout();
            flowLayout_Services.ResumeLayout(false);
            flowLayout_Services.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_DiagTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_TpPeriod).EndInit();
            flowLayout_Buttons.ResumeLayout(false);
            flowLayout_Buttons.PerformLayout();
            groupBox_DidList.ResumeLayout(false);
            tableLayout_Did.ResumeLayout(false);
            tableLayout_Did.PerformLayout();
            flowLayout_DidTools.ResumeLayout(false);
            flowLayout_DidTools.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_Did).EndInit();
            groupBox_Response.ResumeLayout(false);
            tableLayout_Response.ResumeLayout(false);
            tableLayout_Response.PerformLayout();
            panel_Status.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel tableLayout_Root;
        private System.Windows.Forms.GroupBox groupBox_Request;
        private System.Windows.Forms.TableLayoutPanel tableLayout_Request;
        private System.Windows.Forms.FlowLayoutPanel flowLayout_Ids;
        private System.Windows.Forms.Label label_ReqId;
        private System.Windows.Forms.TextBox textBox_UdsReqID;
        private System.Windows.Forms.Label label_RespId;
        private System.Windows.Forms.TextBox textBox_DiagRespID;
        private System.Windows.Forms.Button Btn_RespIdPlus8;
        private System.Windows.Forms.Button Btn_RespIdPlus80;
        private System.Windows.Forms.Label label_FrameType;
        private System.Windows.Forms.ComboBox comboBox_DiagFrameType;
        private System.Windows.Forms.Label label_Padding;
        private System.Windows.Forms.ComboBox comboBox_DiagPadding;
        private System.Windows.Forms.Label label_Timeout;
        private System.Windows.Forms.NumericUpDown numericUpDown_DiagTimeout;
        private System.Windows.Forms.FlowLayoutPanel flowLayout_Services;
        private System.Windows.Forms.ComboBox comboBox_DiagSession;
        private System.Windows.Forms.Button Btn_EnterSession;
        private System.Windows.Forms.Label label_Did;
        private System.Windows.Forms.TextBox textBox_DiagDid;
        private System.Windows.Forms.Button Btn_ReadDid;
        private System.Windows.Forms.Button Btn_WriteDid;
        private System.Windows.Forms.Button Btn_ReadDtc;
        private System.Windows.Forms.Button Btn_ClearDtc;
        private System.Windows.Forms.Button Btn_EcuReset;
        private System.Windows.Forms.Label label_ReqData;
        private System.Windows.Forms.TextBox textBox_DiagReqData;
        private System.Windows.Forms.FlowLayoutPanel flowLayout_Buttons;
        private System.Windows.Forms.Button Btn_SendDiagReq;
        private System.Windows.Forms.Button Btn_CancelDiag;
        private System.Windows.Forms.CheckBox checkBox_TesterPresent;
        private System.Windows.Forms.NumericUpDown numericUpDown_TpPeriod;
        private System.Windows.Forms.GroupBox groupBox_DidList;
        private System.Windows.Forms.TableLayoutPanel tableLayout_Did;
        private System.Windows.Forms.FlowLayoutPanel flowLayout_DidTools;
        private System.Windows.Forms.Button Btn_DidAdd;
        private System.Windows.Forms.Button Btn_DidRemove;
        private System.Windows.Forms.Button Btn_DidReadAll;
        private System.Windows.Forms.Label label_DidWrite;
        private System.Windows.Forms.ComboBox comboBox_DidWriteFmt;
        private System.Windows.Forms.TextBox textBox_DiagWriteData;
        private System.Windows.Forms.Button Btn_DidWriteSelected;
        private System.Windows.Forms.DataGridView dataGridView_Did;
        private System.Windows.Forms.GroupBox groupBox_Response;
        private System.Windows.Forms.TableLayoutPanel tableLayout_Response;
        private System.Windows.Forms.Label label_RespData;
        private System.Windows.Forms.TextBox textBox_DiagRespData;
        private System.Windows.Forms.Label label_Decode;
        private System.Windows.Forms.TextBox textBox_DiagDecode;
        private System.Windows.Forms.Label label_History;
        private System.Windows.Forms.ListView listView_History;
        private System.Windows.Forms.ColumnHeader col_HistTime;
        private System.Windows.Forms.ColumnHeader col_HistReq;
        private System.Windows.Forms.ColumnHeader col_HistResp;
        private System.Windows.Forms.ColumnHeader col_HistResult;
        private System.Windows.Forms.Label label_Trace;
        private System.Windows.Forms.TextBox textBox_DiagTrace;
        private System.Windows.Forms.Panel panel_Status;
        private System.Windows.Forms.Label label_DiagStatus;
    }
}

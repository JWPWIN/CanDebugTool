namespace WindowsFormsApplication.UI.SubWin.SubWin_ModelView
{
    partial class UI_Model_StateNode
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
            AnchorTop = new System.Windows.Forms.Button();
            AnchorBottom = new System.Windows.Forms.Button();
            AnchorLeft = new System.Windows.Forms.Button();
            AnchorRight = new System.Windows.Forms.Button();
            panel_Card = new System.Windows.Forms.Panel();
            panel_SignalArea = new System.Windows.Forms.Panel();
            panel_SignalList = new System.Windows.Forms.Panel();
            lbl_EmptyHint = new System.Windows.Forms.Label();
            panel_ColumnHeader = new System.Windows.Forms.Panel();
            lbl_HeaderValue = new System.Windows.Forms.Label();
            lbl_HeaderName = new System.Windows.Forms.Label();
            panel_Header = new System.Windows.Forms.Panel();
            lbl_InitialBadge = new System.Windows.Forms.Label();
            lbl_StateTitle = new System.Windows.Forms.Label();
            tableLayout_Root.SuspendLayout();
            panel_Card.SuspendLayout();
            panel_SignalArea.SuspendLayout();
            panel_ColumnHeader.SuspendLayout();
            panel_Header.SuspendLayout();
            SuspendLayout();
            //
            // tableLayout_Root — 锚点围边 + 中心卡片
            //
            tableLayout_Root.ColumnCount = 3;
            tableLayout_Root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            tableLayout_Root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout_Root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            tableLayout_Root.Controls.Add(AnchorTop, 1, 0);
            tableLayout_Root.Controls.Add(AnchorLeft, 0, 1);
            tableLayout_Root.Controls.Add(panel_Card, 1, 1);
            tableLayout_Root.Controls.Add(AnchorRight, 2, 1);
            tableLayout_Root.Controls.Add(AnchorBottom, 1, 2);
            tableLayout_Root.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayout_Root.Location = new System.Drawing.Point(0, 0);
            tableLayout_Root.Name = "tableLayout_Root";
            tableLayout_Root.RowCount = 3;
            tableLayout_Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            tableLayout_Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout_Root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            tableLayout_Root.Size = new System.Drawing.Size(248, 200);
            tableLayout_Root.TabIndex = 0;
            //
            // AnchorTop
            //
            AnchorTop.Anchor = System.Windows.Forms.AnchorStyles.None;
            AnchorTop.FlatAppearance.BorderSize = 0;
            AnchorTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            AnchorTop.Location = new System.Drawing.Point(103, 1);
            AnchorTop.Name = "AnchorTop";
            AnchorTop.Size = new System.Drawing.Size(18, 18);
            AnchorTop.TabIndex = 0;
            AnchorTop.Text = "○";
            AnchorTop.UseVisualStyleBackColor = true;
            //
            // AnchorBottom
            //
            AnchorBottom.Anchor = System.Windows.Forms.AnchorStyles.None;
            AnchorBottom.FlatAppearance.BorderSize = 0;
            AnchorBottom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            AnchorBottom.Location = new System.Drawing.Point(103, 1);
            AnchorBottom.Name = "AnchorBottom";
            AnchorBottom.Size = new System.Drawing.Size(18, 18);
            AnchorBottom.TabIndex = 1;
            AnchorBottom.Text = "○";
            AnchorBottom.UseVisualStyleBackColor = true;
            //
            // AnchorLeft
            //
            AnchorLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            AnchorLeft.FlatAppearance.BorderSize = 0;
            AnchorLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            AnchorLeft.Location = new System.Drawing.Point(1, 81);
            AnchorLeft.Name = "AnchorLeft";
            AnchorLeft.Size = new System.Drawing.Size(18, 18);
            AnchorLeft.TabIndex = 2;
            AnchorLeft.Text = "○";
            AnchorLeft.UseVisualStyleBackColor = true;
            //
            // AnchorRight
            //
            AnchorRight.Anchor = System.Windows.Forms.AnchorStyles.None;
            AnchorRight.FlatAppearance.BorderSize = 0;
            AnchorRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            AnchorRight.Location = new System.Drawing.Point(1, 81);
            AnchorRight.Name = "AnchorRight";
            AnchorRight.Size = new System.Drawing.Size(18, 18);
            AnchorRight.TabIndex = 3;
            AnchorRight.Text = "○";
            AnchorRight.UseVisualStyleBackColor = true;
            //
            // panel_Card
            //
            panel_Card.Controls.Add(panel_SignalArea);
            panel_Card.Controls.Add(panel_Header);
            panel_Card.Dock = System.Windows.Forms.DockStyle.Fill;
            panel_Card.Location = new System.Drawing.Point(25, 25);
            panel_Card.Name = "panel_Card";
            panel_Card.Padding = new System.Windows.Forms.Padding(1);
            panel_Card.Size = new System.Drawing.Size(198, 150);
            panel_Card.TabIndex = 4;
            //
            // panel_Header — 状态标题区（可拖拽）
            //
            panel_Header.Controls.Add(lbl_InitialBadge);
            panel_Header.Controls.Add(lbl_StateTitle);
            panel_Header.Dock = System.Windows.Forms.DockStyle.Top;
            panel_Header.Location = new System.Drawing.Point(1, 1);
            panel_Header.Name = "panel_Header";
            panel_Header.Size = new System.Drawing.Size(196, 40);
            panel_Header.TabIndex = 0;
            //
            // lbl_StateTitle
            //
            lbl_StateTitle.AutoEllipsis = true;
            lbl_StateTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_StateTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            lbl_StateTitle.ForeColor = System.Drawing.Color.White;
            lbl_StateTitle.Location = new System.Drawing.Point(0, 0);
            lbl_StateTitle.Name = "lbl_StateTitle";
            lbl_StateTitle.Padding = new System.Windows.Forms.Padding(10, 0, 4, 0);
            lbl_StateTitle.Size = new System.Drawing.Size(156, 40);
            lbl_StateTitle.TabIndex = 0;
            lbl_StateTitle.Text = "状态";
            lbl_StateTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lbl_InitialBadge
            //
            lbl_InitialBadge.Dock = System.Windows.Forms.DockStyle.Right;
            lbl_InitialBadge.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F);
            lbl_InitialBadge.ForeColor = System.Drawing.Color.White;
            lbl_InitialBadge.Location = new System.Drawing.Point(156, 0);
            lbl_InitialBadge.Name = "lbl_InitialBadge";
            lbl_InitialBadge.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            lbl_InitialBadge.Size = new System.Drawing.Size(40, 40);
            lbl_InitialBadge.TabIndex = 1;
            lbl_InitialBadge.Text = "初始";
            lbl_InitialBadge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            lbl_InitialBadge.Visible = false;
            //
            // panel_SignalArea — 信号列表区
            //
            panel_SignalArea.Controls.Add(panel_SignalList);
            panel_SignalArea.Controls.Add(lbl_EmptyHint);
            panel_SignalArea.Controls.Add(panel_ColumnHeader);
            panel_SignalArea.Dock = System.Windows.Forms.DockStyle.Fill;
            panel_SignalArea.Location = new System.Drawing.Point(1, 41);
            panel_SignalArea.Name = "panel_SignalArea";
            panel_SignalArea.Size = new System.Drawing.Size(196, 108);
            panel_SignalArea.TabIndex = 1;
            //
            // panel_ColumnHeader
            //
            panel_ColumnHeader.Controls.Add(lbl_HeaderValue);
            panel_ColumnHeader.Controls.Add(lbl_HeaderName);
            panel_ColumnHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panel_ColumnHeader.Location = new System.Drawing.Point(0, 0);
            panel_ColumnHeader.Name = "panel_ColumnHeader";
            panel_ColumnHeader.Size = new System.Drawing.Size(196, 26);
            panel_ColumnHeader.TabIndex = 0;
            //
            // lbl_HeaderName
            //
            lbl_HeaderName.Dock = System.Windows.Forms.DockStyle.Left;
            lbl_HeaderName.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            lbl_HeaderName.Location = new System.Drawing.Point(0, 0);
            lbl_HeaderName.Name = "lbl_HeaderName";
            lbl_HeaderName.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            lbl_HeaderName.Size = new System.Drawing.Size(110, 26);
            lbl_HeaderName.TabIndex = 0;
            lbl_HeaderName.Text = "信号名";
            lbl_HeaderName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lbl_HeaderValue
            //
            lbl_HeaderValue.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_HeaderValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            lbl_HeaderValue.Location = new System.Drawing.Point(110, 0);
            lbl_HeaderValue.Name = "lbl_HeaderValue";
            lbl_HeaderValue.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            lbl_HeaderValue.Size = new System.Drawing.Size(86, 26);
            lbl_HeaderValue.TabIndex = 1;
            lbl_HeaderValue.Text = "当前值";
            lbl_HeaderValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lbl_EmptyHint
            //
            lbl_EmptyHint.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_EmptyHint.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            lbl_EmptyHint.ForeColor = System.Drawing.Color.Gray;
            lbl_EmptyHint.Location = new System.Drawing.Point(0, 26);
            lbl_EmptyHint.Name = "lbl_EmptyHint";
            lbl_EmptyHint.Size = new System.Drawing.Size(196, 82);
            lbl_EmptyHint.TabIndex = 1;
            lbl_EmptyHint.Text = "右键添加 CAN 信号";
            lbl_EmptyHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // panel_SignalList
            //
            panel_SignalList.AutoScroll = true;
            panel_SignalList.Dock = System.Windows.Forms.DockStyle.Fill;
            panel_SignalList.Location = new System.Drawing.Point(0, 26);
            panel_SignalList.Name = "panel_SignalList";
            panel_SignalList.Size = new System.Drawing.Size(196, 82);
            panel_SignalList.TabIndex = 2;
            panel_SignalList.Visible = false;
            //
            // UI_Model_StateNode
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Transparent;
            Controls.Add(tableLayout_Root);
            MinimumSize = new System.Drawing.Size(248, 120);
            Name = "UI_Model_StateNode";
            Size = new System.Drawing.Size(248, 200);
            tableLayout_Root.ResumeLayout(false);
            panel_Card.ResumeLayout(false);
            panel_SignalArea.ResumeLayout(false);
            panel_ColumnHeader.ResumeLayout(false);
            panel_Header.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel tableLayout_Root;
        private System.Windows.Forms.Button AnchorTop;
        private System.Windows.Forms.Button AnchorBottom;
        private System.Windows.Forms.Button AnchorLeft;
        private System.Windows.Forms.Button AnchorRight;
        private System.Windows.Forms.Panel panel_Card;
        private System.Windows.Forms.Panel panel_Header;
        private System.Windows.Forms.Label lbl_StateTitle;
        private System.Windows.Forms.Label lbl_InitialBadge;
        private System.Windows.Forms.Panel panel_SignalArea;
        private System.Windows.Forms.Panel panel_ColumnHeader;
        private System.Windows.Forms.Label lbl_HeaderName;
        private System.Windows.Forms.Label lbl_HeaderValue;
        private System.Windows.Forms.Label lbl_EmptyHint;
        private System.Windows.Forms.Panel panel_SignalList;
    }
}

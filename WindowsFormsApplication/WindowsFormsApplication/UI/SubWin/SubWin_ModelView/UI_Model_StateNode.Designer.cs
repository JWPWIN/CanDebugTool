namespace WindowsFormsApplication.UI
{
    partial class UI_Model_StateNode
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
            Btn_StateName = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            button5 = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // Btn_StateName
            // 
            Btn_StateName.Location = new System.Drawing.Point(22, 20);
            Btn_StateName.Name = "Btn_StateName";
            Btn_StateName.Size = new System.Drawing.Size(80, 80);
            Btn_StateName.TabIndex = 0;
            Btn_StateName.Text = "状态";
            Btn_StateName.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Dock = System.Windows.Forms.DockStyle.Bottom;
            button2.Location = new System.Drawing.Point(0, 95);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(124, 23);
            button2.TabIndex = 1;
            button2.Text = "o";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Dock = System.Windows.Forms.DockStyle.Top;
            button3.Location = new System.Drawing.Point(0, 0);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(124, 23);
            button3.TabIndex = 2;
            button3.Text = "o";
            button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Dock = System.Windows.Forms.DockStyle.Right;
            button4.Location = new System.Drawing.Point(101, 23);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(23, 72);
            button4.TabIndex = 3;
            button4.Text = "o";
            button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Dock = System.Windows.Forms.DockStyle.Left;
            button5.Location = new System.Drawing.Point(0, 23);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(23, 72);
            button5.TabIndex = 4;
            button5.Text = "o";
            button5.UseVisualStyleBackColor = true;
            // 
            // UI_Model_StateNode
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ActiveCaption;
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(Btn_StateName);
            Name = "UI_Model_StateNode";
            Size = new System.Drawing.Size(124, 118);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button Btn_StateName;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}

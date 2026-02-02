namespace WindowsFormsApplication.UI
{
    partial class SubWin_ModelView
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
            Btn_AddState = new System.Windows.Forms.Button();
            groupBox_Tool = new System.Windows.Forms.GroupBox();
            groupBox_ModelArea = new System.Windows.Forms.GroupBox();
            groupBox_Tool.SuspendLayout();
            SuspendLayout();
            // 
            // Btn_AddState
            // 
            Btn_AddState.Location = new System.Drawing.Point(12, 35);
            Btn_AddState.Name = "Btn_AddState";
            Btn_AddState.Size = new System.Drawing.Size(75, 36);
            Btn_AddState.TabIndex = 0;
            Btn_AddState.Text = "新增状态";
            Btn_AddState.UseVisualStyleBackColor = true;
            Btn_AddState.Click += Btn_AddState_Click;
            // 
            // groupBox_Tool
            // 
            groupBox_Tool.Controls.Add(Btn_AddState);
            groupBox_Tool.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox_Tool.Location = new System.Drawing.Point(0, 0);
            groupBox_Tool.Name = "groupBox_Tool";
            groupBox_Tool.Size = new System.Drawing.Size(800, 100);
            groupBox_Tool.TabIndex = 1;
            groupBox_Tool.TabStop = false;
            groupBox_Tool.Text = "模型组件";
            // 
            // groupBox_ModelArea
            // 
            groupBox_ModelArea.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox_ModelArea.Location = new System.Drawing.Point(0, 100);
            groupBox_ModelArea.Name = "groupBox_ModelArea";
            groupBox_ModelArea.Size = new System.Drawing.Size(800, 500);
            groupBox_ModelArea.TabIndex = 2;
            groupBox_ModelArea.TabStop = false;
            groupBox_ModelArea.Text = "模型区域";
            // 
            // SubWin_ModelView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 600);
            Controls.Add(groupBox_ModelArea);
            Controls.Add(groupBox_Tool);
            Name = "SubWin_ModelView";
            Text = "模型视图";
            groupBox_Tool.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button Btn_AddState;
        private System.Windows.Forms.GroupBox groupBox_Tool;
        private System.Windows.Forms.GroupBox groupBox_ModelArea;
    }
}
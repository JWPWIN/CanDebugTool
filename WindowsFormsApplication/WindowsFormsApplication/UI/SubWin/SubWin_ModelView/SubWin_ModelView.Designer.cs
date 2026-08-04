namespace WindowsFormsApplication.UI.SubWin.SubWin_ModelView
{
    partial class SubWin_ModelView
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
            panel_Palette = new System.Windows.Forms.Panel();
            groupBox_Tools = new System.Windows.Forms.GroupBox();
            Btn_AddState = new System.Windows.Forms.Button();
            Btn_Delete = new System.Windows.Forms.Button();
            Btn_RunStop = new System.Windows.Forms.Button();
            Btn_ResetRuntime = new System.Windows.Forms.Button();
            Btn_Undo = new System.Windows.Forms.Button();
            Btn_Redo = new System.Windows.Forms.Button();
            Btn_ZoomIn = new System.Windows.Forms.Button();
            Btn_ZoomOut = new System.Windows.Forms.Button();
            Btn_ResetView = new System.Windows.Forms.Button();
            Btn_Import = new System.Windows.Forms.Button();
            Btn_Export = new System.Windows.Forms.Button();
            label_Status = new System.Windows.Forms.Label();
            modelCanvas = new ModelCanvasPanel();
            panel_Palette.SuspendLayout();
            groupBox_Tools.SuspendLayout();
            SuspendLayout();
            //
            // panel_Palette
            //
            panel_Palette.AutoScroll = true;
            panel_Palette.Controls.Add(groupBox_Tools);
            panel_Palette.Dock = System.Windows.Forms.DockStyle.Left;
            panel_Palette.Location = new System.Drawing.Point(0, 0);
            panel_Palette.Name = "panel_Palette";
            panel_Palette.Size = new System.Drawing.Size(168, 600);
            panel_Palette.TabIndex = 0;
            //
            // groupBox_Tools
            //
            groupBox_Tools.Controls.Add(label_Status);
            groupBox_Tools.Controls.Add(Btn_Export);
            groupBox_Tools.Controls.Add(Btn_Import);
            groupBox_Tools.Controls.Add(Btn_ResetView);
            groupBox_Tools.Controls.Add(Btn_ZoomOut);
            groupBox_Tools.Controls.Add(Btn_ZoomIn);
            groupBox_Tools.Controls.Add(Btn_Redo);
            groupBox_Tools.Controls.Add(Btn_Undo);
            groupBox_Tools.Controls.Add(Btn_ResetRuntime);
            groupBox_Tools.Controls.Add(Btn_RunStop);
            groupBox_Tools.Controls.Add(Btn_Delete);
            groupBox_Tools.Controls.Add(Btn_AddState);
            groupBox_Tools.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox_Tools.Location = new System.Drawing.Point(0, 0);
            groupBox_Tools.Name = "groupBox_Tools";
            groupBox_Tools.Padding = new System.Windows.Forms.Padding(8);
            groupBox_Tools.Size = new System.Drawing.Size(168, 560);
            groupBox_Tools.TabIndex = 0;
            groupBox_Tools.TabStop = false;
            groupBox_Tools.Text = "模型组件";
            //
            // Btn_AddState
            //
            Btn_AddState.Location = new System.Drawing.Point(12, 28);
            Btn_AddState.Name = "Btn_AddState";
            Btn_AddState.Size = new System.Drawing.Size(140, 28);
            Btn_AddState.TabIndex = 0;
            Btn_AddState.Text = "新增状态";
            Btn_AddState.UseVisualStyleBackColor = true;
            Btn_AddState.Click += Btn_AddState_Click;
            //
            // Btn_Delete
            //
            Btn_Delete.Location = new System.Drawing.Point(12, 60);
            Btn_Delete.Name = "Btn_Delete";
            Btn_Delete.Size = new System.Drawing.Size(140, 28);
            Btn_Delete.TabIndex = 1;
            Btn_Delete.Text = "删除选中";
            Btn_Delete.UseVisualStyleBackColor = true;
            Btn_Delete.Click += Btn_Delete_Click;
            //
            // Btn_RunStop
            //
            Btn_RunStop.Location = new System.Drawing.Point(12, 100);
            Btn_RunStop.Name = "Btn_RunStop";
            Btn_RunStop.Size = new System.Drawing.Size(140, 28);
            Btn_RunStop.TabIndex = 2;
            Btn_RunStop.Text = "停止监控";
            Btn_RunStop.UseVisualStyleBackColor = true;
            Btn_RunStop.Click += Btn_RunStop_Click;
            //
            // Btn_ResetRuntime
            //
            Btn_ResetRuntime.Location = new System.Drawing.Point(12, 132);
            Btn_ResetRuntime.Name = "Btn_ResetRuntime";
            Btn_ResetRuntime.Size = new System.Drawing.Size(140, 28);
            Btn_ResetRuntime.TabIndex = 3;
            Btn_ResetRuntime.Text = "复位状态";
            Btn_ResetRuntime.UseVisualStyleBackColor = true;
            Btn_ResetRuntime.Click += Btn_ResetRuntime_Click;
            //
            // Btn_Undo
            //
            Btn_Undo.Enabled = false;
            Btn_Undo.Location = new System.Drawing.Point(12, 172);
            Btn_Undo.Name = "Btn_Undo";
            Btn_Undo.Size = new System.Drawing.Size(68, 28);
            Btn_Undo.TabIndex = 4;
            Btn_Undo.Text = "撤销";
            Btn_Undo.UseVisualStyleBackColor = true;
            Btn_Undo.Click += Btn_Undo_Click;
            //
            // Btn_Redo
            //
            Btn_Redo.Enabled = false;
            Btn_Redo.Location = new System.Drawing.Point(84, 172);
            Btn_Redo.Name = "Btn_Redo";
            Btn_Redo.Size = new System.Drawing.Size(68, 28);
            Btn_Redo.TabIndex = 5;
            Btn_Redo.Text = "重做";
            Btn_Redo.UseVisualStyleBackColor = true;
            Btn_Redo.Click += Btn_Redo_Click;
            //
            // Btn_ZoomIn
            //
            Btn_ZoomIn.Location = new System.Drawing.Point(12, 212);
            Btn_ZoomIn.Name = "Btn_ZoomIn";
            Btn_ZoomIn.Size = new System.Drawing.Size(68, 28);
            Btn_ZoomIn.TabIndex = 6;
            Btn_ZoomIn.Text = "放大";
            Btn_ZoomIn.UseVisualStyleBackColor = true;
            Btn_ZoomIn.Click += Btn_ZoomIn_Click;
            //
            // Btn_ZoomOut
            //
            Btn_ZoomOut.Location = new System.Drawing.Point(84, 212);
            Btn_ZoomOut.Name = "Btn_ZoomOut";
            Btn_ZoomOut.Size = new System.Drawing.Size(68, 28);
            Btn_ZoomOut.TabIndex = 7;
            Btn_ZoomOut.Text = "缩小";
            Btn_ZoomOut.UseVisualStyleBackColor = true;
            Btn_ZoomOut.Click += Btn_ZoomOut_Click;
            //
            // Btn_ResetView
            //
            Btn_ResetView.Location = new System.Drawing.Point(12, 244);
            Btn_ResetView.Name = "Btn_ResetView";
            Btn_ResetView.Size = new System.Drawing.Size(140, 28);
            Btn_ResetView.TabIndex = 8;
            Btn_ResetView.Text = "复位视图";
            Btn_ResetView.UseVisualStyleBackColor = true;
            Btn_ResetView.Click += Btn_ResetView_Click;
            //
            // Btn_Import
            //
            Btn_Import.Location = new System.Drawing.Point(12, 284);
            Btn_Import.Name = "Btn_Import";
            Btn_Import.Size = new System.Drawing.Size(140, 28);
            Btn_Import.TabIndex = 9;
            Btn_Import.Text = "导入配置";
            Btn_Import.UseVisualStyleBackColor = true;
            Btn_Import.Click += Btn_Import_Click;
            //
            // Btn_Export
            //
            Btn_Export.Location = new System.Drawing.Point(12, 316);
            Btn_Export.Name = "Btn_Export";
            Btn_Export.Size = new System.Drawing.Size(140, 28);
            Btn_Export.TabIndex = 10;
            Btn_Export.Text = "导出配置";
            Btn_Export.UseVisualStyleBackColor = true;
            Btn_Export.Click += Btn_Export_Click;
            //
            // label_Status
            //
            label_Status.Location = new System.Drawing.Point(12, 360);
            label_Status.Name = "label_Status";
            label_Status.Size = new System.Drawing.Size(140, 72);
            label_Status.TabIndex = 11;
            label_Status.Text = "状态: 运行中\r\n缩放: 100%";
            //
            // modelCanvas
            //
            modelCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            modelCanvas.Location = new System.Drawing.Point(168, 0);
            modelCanvas.Name = "modelCanvas";
            modelCanvas.Size = new System.Drawing.Size(832, 600);
            modelCanvas.TabIndex = 1;
            //
            // SubWin_ModelView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1000, 600);
            Controls.Add(modelCanvas);
            Controls.Add(panel_Palette);
            KeyPreview = true;
            Name = "SubWin_ModelView";
            Text = "模型视图";
            panel_Palette.ResumeLayout(false);
            groupBox_Tools.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panel_Palette;
        private System.Windows.Forms.GroupBox groupBox_Tools;
        private System.Windows.Forms.Button Btn_AddState;
        private System.Windows.Forms.Button Btn_Delete;
        private System.Windows.Forms.Button Btn_RunStop;
        private System.Windows.Forms.Button Btn_ResetRuntime;
        private System.Windows.Forms.Button Btn_Undo;
        private System.Windows.Forms.Button Btn_Redo;
        private System.Windows.Forms.Button Btn_ZoomIn;
        private System.Windows.Forms.Button Btn_ZoomOut;
        private System.Windows.Forms.Button Btn_ResetView;
        private System.Windows.Forms.Button Btn_Import;
        private System.Windows.Forms.Button Btn_Export;
        private System.Windows.Forms.Label label_Status;
        private ModelCanvasPanel modelCanvas;
    }
}

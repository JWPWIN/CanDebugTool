using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApplication.ModelViewFsm;
using WindowsFormsApplication.UI;

namespace WindowsFormsApplication.UI.SubWin.SubWin_ModelView
{
    public partial class SubWin_ModelView : Form
    {
        private readonly FsmRuntimeEngine _runtimeEngine = new();
        private readonly FsmEditHistory _history = new();
        private FsmModel _checkpoint;
        private bool _applyingHistory;
        private int _stateCounter;
        private readonly Timer _statusTimer = new() { Interval = 500 };

        public SubWin_ModelView()
        {
            InitializeComponent();
            KeyPreview = true;
            _checkpoint = FsmModelSerializer.Clone(modelCanvas.BuildModel());
            modelCanvas.AttachRuntime(_runtimeEngine);
            modelCanvas.ModelChanged += OnCanvasModelChanged;
            modelCanvas.ViewChanged += (_, _) => UpdateStatusUi();
            modelCanvas.TransitionEditRequested += OnTransitionEditRequested;
            modelCanvas.MouseEnter += (_, _) => modelCanvas.Focus();
            _statusTimer.Tick += (_, _) => UpdateStatusUi();
            _statusTimer.Start();
            UpdateRunStopButton();
            UpdateUndoRedoButtons();
            UpdateStatusUi();
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (keyData == Keys.Delete)
            {
                modelCanvas.DeleteSelection();
                return true;
            }
            if (keyData == Keys.Escape)
            {
                modelCanvas.ClearSelection();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Z))
            {
                PerformUndo();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Y) || keyData == (Keys.Control | Keys.Shift | Keys.Z))
            {
                PerformRedo();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Oemplus) || keyData == (Keys.Control | Keys.Add))
            {
                modelCanvas.ZoomIn();
                return true;
            }
            if (keyData == (Keys.Control | Keys.OemMinus) || keyData == (Keys.Control | Keys.Subtract))
            {
                modelCanvas.ZoomOut();
                return true;
            }
            if (keyData == (Keys.Control | Keys.D0) || keyData == (Keys.Control | Keys.NumPad0))
            {
                modelCanvas.ResetView();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>UI 泵推送的接收快照（已在 UI 线程）。</summary>
        public void OnUiPumpUpdate(List<Canfd_Frame_Com> recvFrames)
        {
            if (recvFrames is null || recvFrames.Count == 0)
                return;

            _runtimeEngine.UpdateFromRecvFrames(recvFrames);
            ApplyRuntimeToUi();
        }

        /// <summary>兼容旧入口：自行取快照（不清除会话缓冲时仅只读拷贝）。</summary>
        public void OnMainLoopUpdate()
        {
            bool deviceOpen = DeviceInterfaceMng.GetInstance() is not null
                && DeviceInterfaceMng.GetInstance().canDeviceOpenFlag;

            if (!deviceOpen)
                return;

            var recv = DeviceInterfaceMng.GetInstance().GetCurWaitToHandleRecvMsg();
            OnUiPumpUpdate(recv);
        }

        private void ApplyRuntimeToUi() => modelCanvas.ApplyRuntimeState();

        private void OnCanvasModelChanged(object? sender, EventArgs e)
        {
            if (!_applyingHistory)
            {
                _history.PushUndo(FsmModelSerializer.Clone(_checkpoint));
                UpdateUndoRedoButtons();
            }

            _checkpoint = FsmModelSerializer.Clone(modelCanvas.BuildModel());
            _runtimeEngine.SetModel(_checkpoint, resetRuntime: false);
        }

        private void Btn_AddState_Click(object sender, EventArgs e)
        {
            string name = $"State{_stateCounter++}";
            while (modelCanvas.BuildModel().States.Any(s => s.DisplayName == name))
                name = $"State{_stateCounter++}";
            modelCanvas.AddStateAtCenter(name);
        }

        private void Btn_Delete_Click(object sender, EventArgs e) => modelCanvas.DeleteSelection();

        private void Btn_RunStop_Click(object sender, EventArgs e)
        {
            _runtimeEngine.IsRunning = !_runtimeEngine.IsRunning;
            UpdateRunStopButton();
            UpdateStatusUi();
        }

        private void Btn_ResetRuntime_Click(object sender, EventArgs e)
        {
            _runtimeEngine.ResetToInitialState();
            modelCanvas.ApplyRuntimeState();
            UpdateStatusUi();
        }

        private void Btn_Undo_Click(object sender, EventArgs e) => PerformUndo();

        private void Btn_Redo_Click(object sender, EventArgs e) => PerformRedo();

        private void Btn_ZoomIn_Click(object sender, EventArgs e) => modelCanvas.ZoomIn();

        private void Btn_ZoomOut_Click(object sender, EventArgs e) => modelCanvas.ZoomOut();

        private void Btn_ResetView_Click(object sender, EventArgs e) => modelCanvas.ResetView();

        private void PerformUndo()
        {
            if (!_history.CanUndo) return;
            var current = FsmModelSerializer.Clone(modelCanvas.BuildModel());
            if (!_history.TryUndo(current, out var previous)) return;

            _applyingHistory = true;
            modelCanvas.LoadModel(previous, raiseChanged: false, resetRuntime: false);
            _checkpoint = FsmModelSerializer.Clone(previous);
            _runtimeEngine.SetModel(_checkpoint, resetRuntime: false);
            _applyingHistory = false;
            UpdateUndoRedoButtons();
            modelCanvas.RevalidateSignalRefs();
            modelCanvas.ApplyRuntimeState();
        }

        private void PerformRedo()
        {
            if (!_history.CanRedo) return;
            var current = FsmModelSerializer.Clone(modelCanvas.BuildModel());
            if (!_history.TryRedo(current, out var next)) return;

            _applyingHistory = true;
            modelCanvas.LoadModel(next, raiseChanged: false, resetRuntime: false);
            _checkpoint = FsmModelSerializer.Clone(next);
            _runtimeEngine.SetModel(_checkpoint, resetRuntime: false);
            _applyingHistory = false;
            UpdateUndoRedoButtons();
            modelCanvas.RevalidateSignalRefs();
            modelCanvas.ApplyRuntimeState();
        }

        private void Btn_Import_Click(object sender, EventArgs e)
        {
            if (!FsmModelSerializer.TryPickOpenPath(out string? path) || path is null)
                return;
            try
            {
                var model = FsmModelSerializer.Load(path);
                modelCanvas.LoadModel(model, raiseChanged: true, resetRuntime: true);
                var issues = FsmSignalResolver.ValidateModel(model);
                if (issues.Count > 0)
                    MessageBox.Show(string.Join(Environment.NewLine, issues.Take(12))
                        + (issues.Count > 12 ? Environment.NewLine + "..." : ""),
                        "导入完成（存在信号校验警告）", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                modelCanvas.RevalidateSignalRefs();
                Text = $"模型视图 - {model.ModelName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Export_Click(object sender, EventArgs e)
        {
            var model = modelCanvas.BuildModel();
            string suggested = string.IsNullOrWhiteSpace(model.ModelName) ? "ECU_FSM" : model.ModelName;
            if (!FsmModelSerializer.TryPickSavePath(suggested, out string? path) || path is null)
                return;
            try
            {
                FsmModelSerializer.Save(path, model);
                MessageBox.Show("导出成功。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnTransitionEditRequested(object? sender, FsmTransition transition)
        {
            // 转移对象已在画布列表中；确定后刷新并记入撤销（取消则保持对话框前的触发条件）。
            using var dlg = new FsmTransitionEditDialog(transition);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                modelCanvas.InvalidateLines(true);
                OnCanvasModelChanged(modelCanvas, EventArgs.Empty);
            }
        }

        public void OnDbcChanged()
        {
            modelCanvas.RevalidateSignalRefs();
            _runtimeEngine.SetModel(modelCanvas.BuildModel(), resetRuntime: false);
        }

        private void UpdateRunStopButton()
        {
            Btn_RunStop.Text = _runtimeEngine.IsRunning ? "停止监控" : "开始监控";
        }

        private void UpdateUndoRedoButtons()
        {
            Btn_Undo.Enabled = _history.CanUndo;
            Btn_Redo.Enabled = _history.CanRedo;
        }

        private void UpdateStatusUi()
        {
            bool deviceOpen = DeviceInterfaceMng.GetInstance() is not null
                && DeviceInterfaceMng.GetInstance().canDeviceOpenFlag;
            string runText = !deviceOpen
                ? "设备未打开"
                : (_runtimeEngine.IsRunning ? "运行中" : "已停止");
            int zoomPct = (int)Math.Round(modelCanvas.Zoom * 100);
            label_Status.Text = $"状态: {runText}\r\n缩放: {zoomPct}%\r\n中键拖动画布";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _statusTimer.Stop();
            _statusTimer.Dispose();
            UI_ComUpper.NotifyModelViewClosed(this);
            base.OnFormClosed(e);
        }
    }
}

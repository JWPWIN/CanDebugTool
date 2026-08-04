using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApplication.ModelViewFsm;

namespace WindowsFormsApplication.UI.SubWin.SubWin_ModelView
{
    public class ModelCanvasPanel : Panel
    {
        private readonly TransitionLineOverlay _lineOverlay;

        private readonly Dictionary<string, UI_Model_StateNode> _nodes = new();
        private readonly List<FsmTransition> _transitions = new();
        private string? _selectedTransitionId;
        private string? _selectedStateId;

        private string? _pendingFromStateId;
        private string? _pendingFromAnchor;

        private FsmRuntimeEngine? _runtimeEngine;
        private HashSet<string> _invalidSignalKeys = new();

        private string _schemaVersion = "1.0";
        private string _modelName = "ECU_FSM";
        private string? _targetEcu;

        private float _zoom = 1f;
        private Point _panOffset = Point.Empty;
        private bool _panning;
        private Point _panStartMouse;
        private Point _panStartOffset;
        private readonly WheelFilter _wheelFilter;
        private bool _wheelFilterReady;

        public event EventHandler ModelChanged;
        public event EventHandler ViewChanged;
        public event EventHandler<FsmTransition> TransitionEditRequested;

        private const int InvalidatePadding = 48;
        public const float MinZoom = 0.4f;
        public const float MaxZoom = 2.5f;

        public string? SelectedTransitionId => _selectedTransitionId;
        public float Zoom => _zoom;
        public Point PanOffset => _panOffset;

        public ModelCanvasPanel()
        {
            DoubleBuffered = true;
            BackColor = FsmStateNodeTheme.CanvasBack;
            AllowDrop = false;
            KeyDown += OnKeyDown;
            TabStop = true;
            _wheelFilter = new WheelFilter(this);

            _lineOverlay = new TransitionLineOverlay(this);
            Controls.Add(_lineOverlay);

            MouseDown += OnCanvasMouseDown;
            MouseMove += OnCanvasMouseMove;
            MouseUp += OnCanvasMouseUp;
            MouseDoubleClick += OnCanvasMouseDoubleClick;
            MouseWheel += OnCanvasMouseWheel;
            MouseEnter += (_, _) => Focus();
        }

        private void AddNodeControl(UI_Model_StateNode node)
        {
            Controls.Add(node);
            node.BringToFront();
            _lineOverlay.SendToBack();
            node.ApplyViewTransform(_zoom, _panOffset);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!_wheelFilterReady)
            {
                Application.AddMessageFilter(_wheelFilter);
                _wheelFilterReady = true;
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (_wheelFilterReady)
            {
                Application.RemoveMessageFilter(_wheelFilter);
                _wheelFilterReady = false;
            }
            base.OnHandleDestroyed(e);
        }

        private sealed class WheelFilter : IMessageFilter
        {
            private readonly ModelCanvasPanel _canvas;
            private const int WM_MOUSEWHEEL = 0x020A;

            public WheelFilter(ModelCanvasPanel canvas) => _canvas = canvas;

            public bool PreFilterMessage(ref System.Windows.Forms.Message m)
            {
                if (m.Msg != WM_MOUSEWHEEL || !_canvas.IsHandleCreated)
                    return false;

                var screenPt = Control.MousePosition;
                var clientPt = _canvas.PointToClient(screenPt);
                if (!_canvas.ClientRectangle.Contains(clientPt))
                    return false;

                int delta = (short)((m.WParam.ToInt64() >> 16) & 0xFFFF);
                float factor = delta > 0 ? 1.1f : 1f / 1.1f;
                _canvas.ZoomAt(clientPt, _canvas.Zoom * factor);
                return true;
            }
        }

        private void OnCanvasMouseDown(object sender, MouseEventArgs e)
        {
            Focus();

            if (e.Button == MouseButtons.Middle)
            {
                _panning = true;
                _panStartMouse = e.Location;
                _panStartOffset = _panOffset;
                Cursor = Cursors.SizeAll;
                return;
            }

            if (e.Button != MouseButtons.Left)
                return;

            if (HitTestTransitionPath(e.Location, out var hitTransId))
            {
                SelectTransition(hitTransId);
                return;
            }

            ClearSelection();
        }

        private void OnCanvasMouseMove(object? sender, MouseEventArgs e)
        {
            if (!_panning) return;
            _panOffset = new Point(
                _panStartOffset.X + (e.X - _panStartMouse.X),
                _panStartOffset.Y + (e.Y - _panStartMouse.Y));
            ApplyViewTransform();
        }

        private void OnCanvasMouseUp(object? sender, MouseEventArgs e)
        {
            if (!_panning) return;
            _panning = false;
            Cursor = Cursors.Default;
            ViewChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnCanvasMouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (HitTestTransitionPath(e.Location, out var transId))
                RequestEditTransition(transId);
        }

        private void OnCanvasMouseWheel(object? sender, MouseEventArgs e)
        {
            // Ctrl+滚轮缩放；普通滚轮也缩放（本画布无纵向滚动条）
            float factor = e.Delta > 0 ? 1.1f : 1f / 1.1f;
            ZoomAt(e.Location, _zoom * factor);
        }

        public void ZoomIn() => ZoomAt(new Point(ClientSize.Width / 2, ClientSize.Height / 2), _zoom * 1.15f);

        public void ZoomOut() => ZoomAt(new Point(ClientSize.Width / 2, ClientSize.Height / 2), _zoom / 1.15f);

        public void ResetView()
        {
            _zoom = 1f;
            _panOffset = Point.Empty;
            ApplyViewTransform();
            ViewChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ZoomAt(Point clientPivot, float newZoom)
        {
            newZoom = Math.Max(MinZoom, Math.Min(MaxZoom, newZoom));
            if (Math.Abs(newZoom - _zoom) < 0.001f)
                return;

            // 保持枢轴下逻辑点不动： client = logical * zoom + pan
            float logicalX = (clientPivot.X - _panOffset.X) / _zoom;
            float logicalY = (clientPivot.Y - _panOffset.Y) / _zoom;
            _zoom = newZoom;
            _panOffset = new Point(
                (int)Math.Round(clientPivot.X - logicalX * _zoom),
                (int)Math.Round(clientPivot.Y - logicalY * _zoom));
            ApplyViewTransform();
            ViewChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ApplyViewTransform()
        {
            foreach (var node in _nodes.Values)
                node.ApplyViewTransform(_zoom, _panOffset);
            InvalidateLines(true);
        }

        internal IReadOnlyList<FsmTransition> GetTransitions() => _transitions;

        internal bool TryGetNodes(string fromId, string toId,
            out UI_Model_StateNode? fromNode, out UI_Model_StateNode? toNode)
        {
            fromNode = null;
            toNode = null;
            return _nodes.TryGetValue(fromId, out fromNode)
                && _nodes.TryGetValue(toId, out toNode);
        }

        internal bool IsTransitionFired(string transitionId)
            => _runtimeEngine?.Runtime.LastFiredTransitionId == transitionId;

        internal string GetTransitionLabel(FsmTransition trans)
        {
            string label = trans.Label ?? "转移";
            if (_runtimeEngine is not null)
            {
                var parts = trans.DisplaySignals.Take(2)
                    .Select(s => _runtimeEngine.GetDisplayValue(s))
                    .Where(v => !string.IsNullOrEmpty(v))
                    .ToList();
                if (parts.Count > 0)
                    label += " | " + string.Join(", ", parts);
            }
            return label;
        }

        internal bool TryGetPendingAnchorPoint(out Point pt)
        {
            pt = default;
            if (_pendingFromStateId is null
                || !_nodes.TryGetValue(_pendingFromStateId, out var pendingNode))
                return false;
            pt = pendingNode.GetAnchorPoint(_pendingFromAnchor ?? FsmAnchorNames.Bottom);
            return true;
        }

        internal bool HitTestTransitionPath(Point pt, out string? transitionId)
        {
            transitionId = null;
            foreach (var trans in _transitions)
            {
                if (!TryGetNodes(trans.FromStateId, trans.ToStateId, out var fromNode, out var toNode))
                    continue;
                var path = FsmTransitionGeometry.GetPathPoints(trans, fromNode, toNode);
                int threshold = Math.Max(4, (int)Math.Round(FsmTransitionGeometry.LineHitThreshold * _zoom));
                if (FsmTransitionGeometry.HitTestPath(pt, path, threshold))
                {
                    transitionId = trans.Id;
                    return true;
                }
            }
            return false;
        }

        internal void SelectTransition(string transitionId)
        {
            _selectedTransitionId = transitionId;
            _selectedStateId = null;
            InvalidateLines();
        }

        internal void ClearSelection()
        {
            _selectedTransitionId = null;
            _selectedStateId = null;
            ClearPendingAnchor();
            InvalidateLines();
        }

        internal void FocusCanvas() => Focus();

        internal void NotifyModelChanged() => ModelChanged?.Invoke(this, EventArgs.Empty);

        internal void RequestEditTransition(string transitionId)
        {
            var trans = _transitions.FirstOrDefault(t => t.Id == transitionId);
            if (trans is not null)
                TransitionEditRequested?.Invoke(this, trans);
        }

        public void InvalidateLines(bool invalidateChildren = false)
            => _lineOverlay.Invalidate(invalidateChildren);

        public void InvalidateDragRegion(UI_Model_StateNode dragged, Rectangle prevBounds, Rectangle newBounds)
        {
            var region = Rectangle.Union(
                Rectangle.Inflate(prevBounds, InvalidatePadding, InvalidatePadding),
                Rectangle.Inflate(newBounds, InvalidatePadding, InvalidatePadding));

            foreach (var trans in _transitions)
            {
                if (trans.FromStateId != dragged.StateId && trans.ToStateId != dragged.StateId)
                    continue;
                if (!TryGetNodes(trans.FromStateId, trans.ToStateId, out var fromNode, out var toNode))
                    continue;

                var prevPath = FsmTransitionGeometry.GetPathPoints(trans, fromNode, toNode, dragged.StateId, prevBounds);
                var newPath = FsmTransitionGeometry.GetPathPoints(trans, fromNode, toNode, dragged.StateId, newBounds);

                region = Rectangle.Union(region, FsmTransitionGeometry.GetPathBounds(prevPath));
                region = Rectangle.Union(region, FsmTransitionGeometry.GetPathBounds(newPath));
                var other = trans.FromStateId == dragged.StateId ? toNode : fromNode;
                region = Rectangle.Union(region, Rectangle.Inflate(other.Bounds, InvalidatePadding, InvalidatePadding));
            }

            region.Intersect(_lineOverlay.ClientRectangle);
            if (region.Width > 0 && region.Height > 0)
            {
                _lineOverlay.Invalidate(region);
                _lineOverlay.Update();
            }
        }

        public void AttachRuntime(FsmRuntimeEngine engine) => _runtimeEngine = engine;

        public FsmModel BuildModel()
        {
            var model = new FsmModel
            {
                SchemaVersion = _schemaVersion,
                ModelName = _modelName,
                TargetEcu = _targetEcu
            };
            foreach (var node in _nodes.Values)
                model.States.Add(node.ToFsmState());
            model.Transitions.AddRange(_transitions.Select(CloneTransition));
            return model;
        }

        /// <param name="raiseChanged">撤销/重做加载时传 false，避免再次入栈。</param>
        public void LoadModel(FsmModel model, bool raiseChanged = true, bool resetRuntime = true)
        {
            ClearNodeControls();
            _nodes.Clear();
            _transitions.Clear();
            _selectedStateId = null;
            _selectedTransitionId = null;
            ClearPendingAnchor();

            _schemaVersion = string.IsNullOrWhiteSpace(model.SchemaVersion) ? "1.0" : model.SchemaVersion;
            _modelName = string.IsNullOrWhiteSpace(model.ModelName) ? "ECU_FSM" : model.ModelName;
            _targetEcu = model.TargetEcu;

            foreach (var state in model.States)
            {
                var node = CreateNodeFromState(state);
                _nodes[state.Id] = node;
                AddNodeControl(node);
            }

            _transitions.AddRange(model.Transitions.Select(CloneTransition));
            _runtimeEngine?.SetModel(model, resetRuntime);
            InvalidateLines(true);
            if (raiseChanged)
                ModelChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ClearCanvas()
        {
            ClearNodeControls();
            _nodes.Clear();
            _transitions.Clear();
            _selectedStateId = null;
            _selectedTransitionId = null;
            _schemaVersion = "1.0";
            _modelName = "ECU_FSM";
            _targetEcu = null;
            ClearPendingAnchor();
            InvalidateLines(true);
        }

        public UI_Model_StateNode AddStateAtCenter(string displayName)
        {
            float logicalX = (ClientSize.Width / 2f - UI_Model_StateNode.NodeWidth * _zoom / 2f - _panOffset.X) / Math.Max(0.01f, _zoom);
            float logicalY = (ClientSize.Height / 2f - 60f * _zoom - _panOffset.Y) / Math.Max(0.01f, _zoom);
            var state = new FsmState
            {
                DisplayName = displayName,
                X = logicalX,
                Y = logicalY
            };
            var node = CreateNodeFromState(state);
            _nodes[state.Id] = node;
            AddNodeControl(node);
            InvalidateLines(true);
            ModelChanged?.Invoke(this, EventArgs.Empty);
            return node;
        }

        public void DeleteSelection()
        {
            if (!string.IsNullOrEmpty(_selectedStateId) && _nodes.ContainsKey(_selectedStateId))
            {
                string id = _selectedStateId;
                Controls.Remove(_nodes[id]);
                _nodes.Remove(id);
                _transitions.RemoveAll(t => t.FromStateId == id || t.ToStateId == id);
                _selectedStateId = null;
            }
            else if (!string.IsNullOrEmpty(_selectedTransitionId))
            {
                _transitions.RemoveAll(t => t.Id == _selectedTransitionId);
                _selectedTransitionId = null;
            }
            else
            {
                return;
            }
            InvalidateLines(true);
            ModelChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RevalidateSignalRefs()
        {
            _invalidSignalKeys.Clear();
            if (!CanDbcDataManager.GetInstance().isLoadCfg)
                return;

            foreach (var node in _nodes.Values)
            {
                foreach (var sig in node.GetDisplaySignals())
                {
                    if (!FsmSignalResolver.SignalRefExists(sig))
                        _invalidSignalKeys.Add(sig.CacheKey);
                }
            }
            foreach (var trans in _transitions)
            {
                foreach (var sig in trans.DisplaySignals)
                {
                    if (!FsmSignalResolver.SignalRefExists(sig))
                        _invalidSignalKeys.Add(sig.CacheKey);
                }
                foreach (var trig in trans.Triggers)
                {
                    var key = $"{trig.MsgId}:{trig.SigName}";
                    if (!FsmSignalResolver.TriggerExists(trig))
                        _invalidSignalKeys.Add(key);
                }
            }
            InvalidateLines();
        }

        public void ApplyRuntimeState()
        {
            if (_runtimeEngine is null) return;

            string? activeId = _runtimeEngine.Runtime.ActiveStateId;

            foreach (var node in _nodes.Values)
            {
                node.SetActiveHighlight(node.StateId == activeId);
                node.UpdateSignalValues(_runtimeEngine.Runtime.SignalDisplayCache, _invalidSignalKeys);
            }

            InvalidateLines();
        }

        private UI_Model_StateNode CreateNodeFromState(FsmState state)
        {
            var node = new UI_Model_StateNode();
            node.BindState(state);
            node.AnchorClicked += OnNodeAnchorClicked;
            node.RequestAddSignal += OnRequestAddSignal;
            node.RequestRemoveSignal += OnRequestRemoveSignal;
            node.RequestDeleteState += OnRequestDeleteState;
            node.RequestRename += OnRequestRename;
            node.RequestSetInitial += OnRequestSetInitial;
            node.NodeDragCompleted += (_, _) => ModelChanged?.Invoke(this, EventArgs.Empty);
            node.Click += (_, _) =>
            {
                _selectedStateId = node.StateId;
                _selectedTransitionId = null;
                Focus();
                InvalidateLines();
            };
            return node;
        }

        private void OnRequestRemoveSignal(object? sender, FsmSignalRef reference)
        {
            if (sender is UI_Model_StateNode)
                ModelChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnRequestAddSignal(object? sender, EventArgs e)
        {
            if (sender is not UI_Model_StateNode node) return;
            if (!CanDbcDataManager.GetInstance().isLoadCfg)
            {
                MessageBox.Show("请先通过顶部文件夹图标加载 Excel 通信矩阵。", "提示");
                return;
            }
            using var picker = new FsmSignalPickerDialog();
            if (picker.ShowDialog(FindForm()) != DialogResult.OK || picker.SelectedRef is null)
                return;
            if (node.TryAddDisplaySignal(picker.SelectedRef))
                ModelChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnRequestDeleteState(object? sender, EventArgs e)
        {
            if (sender is not UI_Model_StateNode node) return;
            _selectedStateId = node.StateId;
            DeleteSelection();
        }

        private void OnRequestRename(object? sender, EventArgs e)
        {
            if (sender is not UI_Model_StateNode node) return;
            using var renameDlg = new FsmRenameDialog("重命名状态", node.DisplayName);
            if (renameDlg.ShowDialog(FindForm()) == DialogResult.OK && !string.IsNullOrWhiteSpace(renameDlg.InputText))
            {
                node.SetDisplayName(renameDlg.InputText.Trim());
                ModelChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnRequestSetInitial(object? sender, EventArgs e)
        {
            if (sender is not UI_Model_StateNode node) return;
            foreach (var n in _nodes.Values)
                n.IsInitial = false;
            node.IsInitial = true;
            _runtimeEngine?.ResetToInitialState();
            ModelChanged?.Invoke(this, EventArgs.Empty);
            InvalidateLines();
        }

        private void OnNodeAnchorClicked(object? sender, string anchor)
        {
            if (sender is not UI_Model_StateNode node) return;

            if (_pendingFromStateId is null)
            {
                _pendingFromStateId = node.StateId;
                _pendingFromAnchor = anchor;
                InvalidateLines();
                return;
            }

            if (_pendingFromStateId == node.StateId)
            {
                ClearPendingAnchor();
                InvalidateLines();
                return;
            }

            if (!_nodes.TryGetValue(_pendingFromStateId, out _))
                return;

            var transition = new FsmTransition
            {
                FromStateId = _pendingFromStateId,
                ToStateId = node.StateId,
                FromAnchor = _pendingFromAnchor ?? FsmAnchorNames.Bottom,
                ToAnchor = anchor,
                Label = "转移"
            };
            _transitions.Add(transition);
            ClearPendingAnchor();
            _selectedTransitionId = transition.Id;
            InvalidateLines(true);
            ModelChanged?.Invoke(this, EventArgs.Empty);
            TransitionEditRequested?.Invoke(this, transition);
        }

        private void ClearPendingAnchor()
        {
            _pendingFromStateId = null;
            _pendingFromAnchor = null;
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelection();
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Escape)
            {
                ClearPendingAnchor();
                InvalidateLines();
            }
        }

        private void ClearNodeControls()
        {
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                if (Controls[i] is UI_Model_StateNode)
                    Controls.RemoveAt(i);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using var brush = new SolidBrush(BackColor);
            e.Graphics.FillRectangle(brush, e.ClipRectangle);
        }

        private static FsmTransition CloneTransition(FsmTransition source)
        {
            return new FsmTransition
            {
                Id = source.Id,
                FromStateId = source.FromStateId,
                ToStateId = source.ToStateId,
                FromAnchor = source.FromAnchor,
                ToAnchor = source.ToAnchor,
                Label = source.Label,
                Triggers = source.Triggers.Select(tr => new FsmTriggerCondition
                {
                    MsgId = tr.MsgId,
                    SigName = tr.SigName,
                    ExpectedRaw = tr.ExpectedRaw,
                    ExpectedPhysical = tr.ExpectedPhysical,
                    ExpectedEnumLabel = tr.ExpectedEnumLabel
                }).ToList(),
                DisplaySignals = source.DisplaySignals.Select(s => new FsmSignalRef { MsgId = s.MsgId, SigName = s.SigName }).ToList()
            };
        }
    }
}

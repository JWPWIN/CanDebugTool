using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApplication.ModelViewFsm;

namespace WindowsFormsApplication.UI.SubWin.SubWin_ModelView
{
    public partial class UI_Model_StateNode : UserControl
    {
        public const int MaxDisplaySignals = 20;
        public static int NodeWidth => FsmStateNodeTheme.NodeWidth;

        public string StateId { get; private set; } = Guid.NewGuid().ToString();
        public string DisplayName => lbl_StateTitle.Text;
        public bool IsInitial { get; set; }
        public bool IsActive { get; private set; }

        public event EventHandler<string>? AnchorClicked;
        public event EventHandler? RequestAddSignal;
        public event EventHandler<FsmSignalRef>? RequestRemoveSignal;
        public event EventHandler? RequestDeleteState;
        public event EventHandler? RequestRename;
        public event EventHandler? RequestSetInitial;
        public event EventHandler? NodeMoved;
        /// <summary>节点拖动结束（用于记录撤销快照）。</summary>
        public event EventHandler? NodeDragCompleted;

        private readonly List<FsmSignalRef> _displaySignals = new();
        private readonly Dictionary<string, Label> _signalValueLabels = new();
        private Point _dragStartScreen;
        private PointF _dragStartLogical;
        private Rectangle _dragPrevBounds;
        private bool _dragging;
        private float _viewZoom = 1f;
        private bool _layouting;

        /// <summary>逻辑坐标（与缩放/平移无关，导出时使用）。</summary>
        public PointF LogicalLocation { get; private set; }

        public UI_Model_StateNode()
        {
            InitializeComponent();
            EnableDoubleBuffering();
            MinimumSize = new Size(1, 1);
            Width = FsmStateNodeTheme.NodeWidth;
            ApplyTheme();
            WireAnchors();
            WireDrag(panel_Header);
            WireDrag(lbl_StateTitle);

            var menu = new ContextMenuStrip();
            menu.Items.Add("重命名", null, (_, _) => RequestRename?.Invoke(this, EventArgs.Empty));
            menu.Items.Add("添加信号", null, (_, _) => RequestAddSignal?.Invoke(this, EventArgs.Empty));
            menu.Items.Add("删除信号", null, OnDeleteSignalMenuClick);
            menu.Items.Add("设为初始状态", null, (_, _) => RequestSetInitial?.Invoke(this, EventArgs.Empty));
            menu.Items.Add("删除状态", null, (_, _) => RequestDeleteState?.Invoke(this, EventArgs.Empty));
            ContextMenuStrip = menu;

            RefreshSignalPanel();
        }

        private void ApplyTheme()
        {
            panel_Card.BackColor = FsmStateNodeTheme.CardBack;
            panel_SignalArea.BackColor = FsmStateNodeTheme.CardBack;
            panel_ColumnHeader.BackColor = FsmStateNodeTheme.ColumnHeaderBack;
            lbl_HeaderName.ForeColor = FsmStateNodeTheme.ColumnHeaderFore;
            lbl_HeaderValue.ForeColor = FsmStateNodeTheme.ColumnHeaderFore;
            lbl_EmptyHint.ForeColor = FsmStateNodeTheme.EmptyHintFore;

            StyleAnchorButton(AnchorTop);
            StyleAnchorButton(AnchorBottom);
            StyleAnchorButton(AnchorLeft);
            StyleAnchorButton(AnchorRight);
            ApplyVisualState();
        }

        private static void StyleAnchorButton(Button btn)
        {
            btn.BackColor = FsmStateNodeTheme.AnchorBack;
            btn.ForeColor = FsmStateNodeTheme.AnchorFore;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 7F);
        }

        private void WireAnchors()
        {
            WireAnchor(AnchorTop, FsmAnchorNames.Top);
            WireAnchor(AnchorBottom, FsmAnchorNames.Bottom);
            WireAnchor(AnchorLeft, FsmAnchorNames.Left);
            WireAnchor(AnchorRight, FsmAnchorNames.Right);
        }

        private void WireAnchor(Button btn, string anchorName)
        {
            btn.Tag = anchorName;
            btn.Click += (_, _) => AnchorClicked?.Invoke(this, anchorName);
        }

        private void WireDrag(Control control)
        {
            control.MouseDown += Title_MouseDown;
            control.MouseMove += Title_MouseMove;
            control.MouseUp += Title_MouseUp;
        }

        public void BindState(FsmState state)
        {
            StateId = state.Id;
            lbl_StateTitle.Text = state.DisplayName;
            IsInitial = state.IsInitial;
            _displaySignals.Clear();
            _displaySignals.AddRange(state.DisplaySignals);
            LogicalLocation = new PointF(state.X, state.Y);
            if (GetCanvasPanel() is { } canvas)
                ApplyViewTransform(canvas.Zoom, canvas.PanOffset);
            else
                Location = new Point((int)state.X, (int)state.Y);
            RefreshSignalPanel();
            ApplyVisualState();
        }

        public void ApplyViewTransform(float zoom, Point pan)
        {
            Location = new Point(
                (int)Math.Round(LogicalLocation.X * zoom + pan.X),
                (int)Math.Round(LogicalLocation.Y * zoom + pan.Y));

            if (Math.Abs(zoom - _viewZoom) > 0.001f)
            {
                _viewZoom = Math.Max(0.01f, zoom);
                ApplyZoomLayout();
            }
        }

        /// <summary>按当前 Zoom 缩放节点宽高、锚点、字体与内部行高。</summary>
        private void ApplyZoomLayout()
        {
            if (_layouting) return;
            _layouting = true;
            SuspendLayout();
            try
            {
                float z = _viewZoom;
                int nodeW = Math.Max(72, Sz(FsmStateNodeTheme.NodeWidth));
                Width = nodeW;

                int slot = Math.Max(12, Sz(22));
                int anchor = Math.Max(10, Sz(FsmStateNodeTheme.AnchorSize));
                tableLayout_Root.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, slot);
                tableLayout_Root.ColumnStyles[2] = new ColumnStyle(SizeType.Absolute, slot);
                tableLayout_Root.RowStyles[0] = new RowStyle(SizeType.Absolute, slot);
                tableLayout_Root.RowStyles[2] = new RowStyle(SizeType.Absolute, slot);

                SetAnchorSize(AnchorTop, anchor);
                SetAnchorSize(AnchorBottom, anchor);
                SetAnchorSize(AnchorLeft, anchor);
                SetAnchorSize(AnchorRight, anchor);

                ReplaceFont(lbl_StateTitle, Math.Max(6f, 10f * z), FontStyle.Bold);
                ReplaceFont(lbl_InitialBadge, Math.Max(5.5f, 8f * z), FontStyle.Regular);
                ReplaceFont(lbl_HeaderName, Math.Max(5.5f, 8.25f * z), FontStyle.Regular);
                ReplaceFont(lbl_HeaderValue, Math.Max(5.5f, 8.25f * z), FontStyle.Regular);
                ReplaceFont(lbl_EmptyHint, Math.Max(5.5f, 8.25f * z), FontStyle.Regular);

                panel_Header.Height = Math.Max(18, Sz(FsmStateNodeTheme.HeaderHeight));
                panel_ColumnHeader.Height = Math.Max(12, Sz(FsmStateNodeTheme.ColumnHeaderHeight));
                lbl_HeaderName.Width = Math.Max(40, Sz(110));
                lbl_HeaderValue.Left = lbl_HeaderName.Width;
            }
            finally
            {
                ResumeLayout(true);
                _layouting = false;
            }

            RefreshSignalPanel();
        }

        private int Sz(int logical) => Math.Max(1, (int)Math.Round(logical * _viewZoom));

        private static void SetAnchorSize(Button btn, int size)
        {
            btn.Size = new Size(size, size);
            btn.Font = new Font("Segoe UI", Math.Max(5f, size * 0.4f));
        }

        private static void ReplaceFont(Control control, float emSize, FontStyle style)
        {
            control.Font = new Font("Microsoft YaHei UI", emSize, style);
        }

        public void SetLogicalLocation(PointF logical) => LogicalLocation = logical;

        public FsmState ToFsmState()
        {
            return new FsmState
            {
                Id = StateId,
                DisplayName = DisplayName,
                X = LogicalLocation.X,
                Y = LogicalLocation.Y,
                IsInitial = IsInitial,
                DisplaySignals = _displaySignals.Select(s => new FsmSignalRef { MsgId = s.MsgId, SigName = s.SigName }).ToList()
            };
        }

        public void SetDisplayName(string name) => lbl_StateTitle.Text = name;

        public IReadOnlyList<FsmSignalRef> GetDisplaySignals() => _displaySignals;

        public bool TryAddDisplaySignal(FsmSignalRef reference)
        {
            if (_displaySignals.Count >= MaxDisplaySignals)
            {
                MessageBox.Show($"单状态最多绑定 {MaxDisplaySignals} 个展示信号。", "提示");
                return false;
            }
            if (_displaySignals.Any(s => s.MsgId == reference.MsgId && s.SigName == reference.SigName))
                return false;
            _displaySignals.Add(new FsmSignalRef { MsgId = reference.MsgId, SigName = reference.SigName });
            RefreshSignalPanel();
            return true;
        }

        public void RemoveDisplaySignal(FsmSignalRef reference)
        {
            _displaySignals.RemoveAll(s => s.MsgId == reference.MsgId && s.SigName == reference.SigName);
            RefreshSignalPanel();
        }

        public void UpdateSignalValues(Dictionary<string, string> displayCache, HashSet<string> invalidKeys)
        {
            foreach (var sig in _displaySignals)
            {
                if (!_signalValueLabels.TryGetValue(sig.CacheKey, out var lbl))
                    continue;
                lbl.Text = displayCache.TryGetValue(sig.CacheKey, out var v) ? v : "-";
                lbl.ForeColor = invalidKeys.Contains(sig.CacheKey)
                    ? FsmStateNodeTheme.SigInvalidFore
                    : FsmStateNodeTheme.SigValueFore;
            }
        }

        public void SetActiveHighlight(bool active)
        {
            IsActive = active;
            ApplyVisualState();
        }

        private void ApplyVisualState()
        {
            Color headerColor = IsActive
                ? FsmStateNodeTheme.HeaderActive
                : (IsInitial ? FsmStateNodeTheme.HeaderInitial : FsmStateNodeTheme.HeaderNormal);

            panel_Header.BackColor = headerColor;
            lbl_StateTitle.ForeColor = FsmStateNodeTheme.HeaderFore;
            lbl_InitialBadge.Visible = IsInitial;
            lbl_InitialBadge.ForeColor = FsmStateNodeTheme.HeaderFore;

            panel_Card.Padding = IsActive ? new Padding(2) : new Padding(1);
            // 勿用 Transparent：WinForms 拖动时旧区域无法被正确擦除，会出现尾影
            BackColor = IsActive ? Color.FromArgb(220, 252, 231) : FsmStateNodeTheme.CanvasBack;
        }

        private void EnableDoubleBuffering()
        {
            // 勿启用 UserPaint：未实现 OnPaint 时会导致控件区域显示为黑屏
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        public Point GetAnchorPoint(string anchor) => GetAnchorPoint(Bounds, anchor);

        public static Point GetAnchorPoint(Rectangle bounds, string anchor)
        {
            int inset = Math.Max(4, Math.Min(bounds.Width, bounds.Height) / 12);
            return anchor switch
            {
                FsmAnchorNames.Top => new Point(bounds.Left + bounds.Width / 2, bounds.Top + inset),
                FsmAnchorNames.Bottom => new Point(bounds.Left + bounds.Width / 2, bounds.Bottom - inset),
                FsmAnchorNames.Left => new Point(bounds.Left + inset, bounds.Top + bounds.Height / 2),
                FsmAnchorNames.Right => new Point(bounds.Right - inset, bounds.Top + bounds.Height / 2),
                _ => new Point(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2)
            };
        }

        private void RefreshSignalPanel()
        {
            if (_layouting) return;
            _layouting = true;
            try
            {
                panel_SignalList.Controls.Clear();
                _signalValueLabels.Clear();

                bool hasSignals = _displaySignals.Count > 0;
                lbl_EmptyHint.Visible = !hasSignals;
                panel_SignalList.Visible = hasSignals;

                int rowH = Math.Max(12, Sz(FsmStateNodeTheme.SignalRowHeight));
                int y = 0;
                int contentWidth = panel_SignalList.ClientSize.Width > 0
                    ? panel_SignalList.ClientSize.Width
                    : Math.Max(40, Width - Sz(50));

                foreach (var sig in _displaySignals)
                {
                    bool valid = FsmSignalResolver.TryGetSignal(sig.MsgId, sig.SigName, out _, out _);
                    var row = CreateSignalRow(sig, valid, contentWidth, rowH);
                    row.Top = y;
                    panel_SignalList.Controls.Add(row);
                    y += rowH;
                }

                int listHeight = Math.Min(Sz(FsmStateNodeTheme.MaxSignalAreaHeight),
                    Math.Max(Sz(FsmStateNodeTheme.MinSignalAreaHeight), y));
                panel_SignalList.Height = listHeight;

                int totalHeight = Sz(FsmStateNodeTheme.HeaderHeight)
                    + Sz(FsmStateNodeTheme.ColumnHeaderHeight)
                    + listHeight
                    + Sz(44);
                Height = Math.Max(Sz(120), totalHeight);
                panel_Card.PerformLayout();
            }
            finally
            {
                _layouting = false;
            }
        }

        private void OnDeleteSignalMenuClick(object? sender, EventArgs e)
        {
            if (_displaySignals.Count == 0)
            {
                MessageBox.Show("当前状态未绑定信号。", "提示");
                return;
            }
            using var dlg = new FsmDeleteSignalDialog(_displaySignals);
            if (dlg.ShowDialog(FindForm()) != DialogResult.OK || dlg.SelectedRef is null)
                return;
            RemoveDisplaySignal(dlg.SelectedRef);
            RequestRemoveSignal?.Invoke(this, dlg.SelectedRef);
        }

        private Panel CreateSignalRow(FsmSignalRef sig, bool valid, int width, int rowH)
        {
            var row = new Panel
            {
                Width = width,
                Height = rowH,
                BackColor = FsmStateNodeTheme.CardBack
            };

            var rowMenu = new ContextMenuStrip();
            var sigRef = new FsmSignalRef { MsgId = sig.MsgId, SigName = sig.SigName };
            rowMenu.Items.Add("删除此信号", null, (_, _) =>
            {
                RemoveDisplaySignal(sigRef);
                RequestRemoveSignal?.Invoke(this, sigRef);
            });
            row.ContextMenuStrip = rowMenu;

            int nameW = Math.Max(36, Sz(108));
            float rowFont = Math.Max(5.5f, 8.25f * _viewZoom);
            var lblName = new Label
            {
                Text = sig.SigName,
                Left = 0,
                Top = 0,
                Width = nameW,
                Height = rowH,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                Padding = new Padding(Math.Max(2, Sz(8)), 0, 2, 0),
                ForeColor = valid ? FsmStateNodeTheme.SigNameFore : FsmStateNodeTheme.SigInvalidFore,
                BackColor = FsmStateNodeTheme.CardBack,
                Font = new Font("Microsoft YaHei UI", rowFont)
            };

            var sep = new Panel
            {
                Left = nameW,
                Top = Math.Max(1, rowH / 6),
                Width = 1,
                Height = Math.Max(4, rowH - rowH / 3),
                BackColor = FsmStateNodeTheme.RowSeparator
            };

            var lblVal = new Label
            {
                Text = "-",
                Left = nameW + 4,
                Top = 0,
                Width = Math.Max(24, width - nameW - 8),
                Height = rowH,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                Padding = new Padding(Math.Max(2, Sz(6)), 0, 2, 0),
                ForeColor = FsmStateNodeTheme.SigValueFore,
                BackColor = FsmStateNodeTheme.ValueBack,
                Font = new Font("Microsoft YaHei UI", rowFont)
            };

            var bottomLine = new Panel
            {
                Left = 0,
                Top = rowH - 1,
                Width = width,
                Height = 1,
                BackColor = FsmStateNodeTheme.RowSeparator
            };

            row.Controls.Add(lblName);
            row.Controls.Add(sep);
            row.Controls.Add(lblVal);
            row.Controls.Add(bottomLine);
            _signalValueLabels[sig.CacheKey] = lblVal;
            return row;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (!_layouting && panel_SignalList is not null && _displaySignals.Count > 0)
                RefreshSignalPanel();
        }

        private void Title_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _dragging = true;
            _dragPrevBounds = Bounds;
            BringToFront();
            var ctrl = (Control?)sender ?? panel_Header;
            _dragStartScreen = ctrl.PointToScreen(e.Location);
            _dragStartLogical = LogicalLocation;
        }

        private void Title_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!_dragging || Parent is null) return;
            if (GetCanvasPanel() is not { } canvas) return;

            var ctrl = (Control?)sender ?? panel_Header;
            var cur = ctrl.PointToScreen(e.Location);
            float zoom = Math.Max(0.01f, canvas.Zoom);
            float dx = (cur.X - _dragStartScreen.X) / zoom;
            float dy = (cur.Y - _dragStartScreen.Y) / zoom;
            LogicalLocation = new PointF(_dragStartLogical.X + dx, _dragStartLogical.Y + dy);

            var newLoc = new Point(
                (int)Math.Round(LogicalLocation.X * zoom + canvas.PanOffset.X),
                (int)Math.Round(LogicalLocation.Y * zoom + canvas.PanOffset.Y));

            if (newLoc == Location)
                return;

            canvas.InvalidateDragRegion(this, _dragPrevBounds, new Rectangle(newLoc, Size));
            Location = newLoc;
            _dragPrevBounds = Bounds;
            NodeMoved?.Invoke(this, EventArgs.Empty);
        }

        private void Title_MouseUp(object? sender, MouseEventArgs e)
        {
            if (!_dragging) return;
            if (GetCanvasPanel() is { } canvas)
            {
                canvas.InvalidateDragRegion(this, _dragPrevBounds, Bounds);
                canvas.InvalidateLines(true);
                NodeDragCompleted?.Invoke(this, EventArgs.Empty);
            }
            _dragging = false;
        }

        private ModelCanvasPanel? GetCanvasPanel() => Parent as ModelCanvasPanel;
    }
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WindowsFormsApplication.ModelViewFsm;

namespace WindowsFormsApplication.UI.SubWin.SubWin_ModelView
{
    /// <summary>连线绘制层（位于节点下层；含淡色网格；鼠标穿透到画布）。</summary>
    internal class TransitionLineOverlay : Panel
    {
        private const int WM_NCHITTEST = 0x0084;
        private const int HTTRANSPARENT = -1;

        private readonly ModelCanvasPanel _canvas;

        public TransitionLineOverlay(ModelCanvasPanel canvas)
        {
            _canvas = canvas;
            DoubleBuffered = true;
            BackColor = FsmStateNodeTheme.CanvasBack;
            Dock = DockStyle.Fill;
            TabStop = false;
        }

        /// <summary>让空白区/连线点击落到父画布，节点仍因 z-order 在上层优先命中。</summary>
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_NCHITTEST)
            {
                m.Result = (IntPtr)HTTRANSPARENT;
                return;
            }
            base.WndProc(ref m);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var g = e.Graphics;
            using (var brush = new SolidBrush(FsmStateNodeTheme.CanvasBack))
                g.FillRectangle(brush, ClientRectangle);

            DrawGrid(g);
        }

        private void DrawGrid(Graphics g)
        {
            float zoom = Math.Max(0.01f, _canvas.Zoom);
            Point pan = _canvas.PanOffset;
            float minor = FsmStateNodeTheme.GridMinorLogical * zoom;
            float major = FsmStateNodeTheme.GridMajorLogical * zoom;

            // 过密时跳过细网格，避免糊成一片
            if (minor >= 6f)
                DrawGridLines(g, minor, FsmStateNodeTheme.GridMinor, pan);
            if (major >= 8f)
                DrawGridLines(g, major, FsmStateNodeTheme.GridMajor, pan);
        }

        private void DrawGridLines(Graphics g, float step, Color color, Point pan)
        {
            if (step < 1f) return;

            float originX = pan.X % step;
            if (originX < 0) originX += step;
            float originY = pan.Y % step;
            if (originY < 0) originY += step;

            using var pen = new Pen(color, 1f);
            int w = ClientSize.Width;
            int h = ClientSize.Height;

            for (float x = originX; x < w; x += step)
                g.DrawLine(pen, x, 0, x, h);
            for (float y = originY; y < h; y += step)
                g.DrawLine(pen, 0, y, w, y);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var clip = e.ClipRectangle;
            float zoom = Math.Max(0.01f, _canvas.Zoom);
            float lineScale = Math.Max(0.6f, zoom);

            foreach (var trans in _canvas.GetTransitions())
            {
                if (!_canvas.TryGetNodes(trans.FromStateId, trans.ToStateId, out var fromNode, out var toNode)
                    || fromNode is null || toNode is null)
                    continue;

                var path = FsmTransitionGeometry.GetPathPoints(trans, fromNode, toNode);
                if (!FsmTransitionGeometry.GetPathBounds(path).IntersectsWith(clip))
                    continue;

                bool selected = trans.Id == _canvas.SelectedTransitionId;
                bool fired = _canvas.IsTransitionFired(trans.Id);

                Color lineColor = fired ? Color.Red : (selected ? Color.DodgerBlue : Color.Gray);
                float width = (fired ? 3f : 2f) * lineScale;
                using var pen = new Pen(lineColor, width);
                float arrow = Math.Max(3f, 5f * lineScale);
                pen.CustomEndCap = new AdjustableArrowCap(arrow, arrow, true);
                e.Graphics.DrawLine(pen, path[0], path[1]);

                string label = _canvas.GetTransitionLabel(trans);
                var labelPos = FsmTransitionGeometry.GetLabelPosition(path);
                float fontSize = Math.Max(6f, Font.Size * zoom);
                using var font = new Font(Font.FontFamily, fontSize, Font.Style);
                using var brush = new SolidBrush(fired ? Color.DarkRed : (selected ? Color.DodgerBlue : Color.DimGray));
                e.Graphics.DrawString(label, font, brush, labelPos);
            }

            if (_canvas.TryGetPendingAnchorPoint(out var pendingPt))
            {
                int r = Math.Max(3, (int)Math.Round(5 * zoom));
                e.Graphics.FillEllipse(Brushes.Orange, pendingPt.X - r, pendingPt.Y - r, r * 2, r * 2);
            }
        }
    }
}

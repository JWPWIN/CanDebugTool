using System;
using System.Collections.Generic;
using System.Drawing;
using WindowsFormsApplication.ModelViewFsm;

namespace WindowsFormsApplication.UI.SubWin.SubWin_ModelView
{
    internal static class FsmTransitionGeometry
    {
        public const int LineHitThreshold = 8;

        public static List<Point> GetPathPoints(
            FsmTransition transition,
            UI_Model_StateNode fromNode,
            UI_Model_StateNode toNode,
            string? overrideStateId = null,
            Rectangle? overrideBounds = null)
        {
            Point fromPt = overrideStateId == fromNode.StateId && overrideBounds.HasValue
                ? UI_Model_StateNode.GetAnchorPoint(overrideBounds.Value, transition.FromAnchor)
                : fromNode.GetAnchorPoint(transition.FromAnchor);
            Point toPt = overrideStateId == toNode.StateId && overrideBounds.HasValue
                ? UI_Model_StateNode.GetAnchorPoint(overrideBounds.Value, transition.ToAnchor)
                : toNode.GetAnchorPoint(transition.ToAnchor);

            return new List<Point> { fromPt, toPt };
        }

        public static Rectangle GetPathBounds(IReadOnlyList<Point> path, int padding = 16)
        {
            if (path.Count == 0)
                return Rectangle.Empty;

            int minX = path[0].X, minY = path[0].Y, maxX = path[0].X, maxY = path[0].Y;
            foreach (var p in path)
            {
                minX = Math.Min(minX, p.X);
                minY = Math.Min(minY, p.Y);
                maxX = Math.Max(maxX, p.X);
                maxY = Math.Max(maxY, p.Y);
            }
            return Rectangle.FromLTRB(minX - padding, minY - padding, maxX + padding, maxY + padding);
        }

        public static bool HitTestPath(Point pt, IReadOnlyList<Point> path, int threshold = LineHitThreshold)
        {
            if (path.Count < 2)
                return false;
            return DistancePointToSegment(pt, path[0], path[1]) <= threshold;
        }

        public static Point GetLabelPosition(IReadOnlyList<Point> path)
        {
            if (path.Count < 2)
                return Point.Empty;
            return new Point((path[0].X + path[1].X) / 2, (path[0].Y + path[1].Y) / 2);
        }

        public static double DistancePointToSegment(Point p, Point a, Point b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            if (dx == 0 && dy == 0)
                return Math.Sqrt((p.X - a.X) * (p.X - a.X) + (p.Y - a.Y) * (p.Y - a.Y));
            double t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / (dx * dx + dy * dy);
            t = Math.Max(0, Math.Min(1, t));
            double projX = a.X + t * dx;
            double projY = a.Y + t * dy;
            return Math.Sqrt((p.X - projX) * (p.X - projX) + (p.Y - projY) * (p.Y - projY));
        }
    }
}

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApplication.FunctionScript.SysControlOverride
{
    /// <summary>
    /// 将子控件上的鼠标滚轮转发给最近的 AutoScroll 父容器，避免中间编辑列吞掉滚轮导致无法滚动视图。
    /// </summary>
    internal static class MouseWheelScrollForwarder
    {
        private const int WM_MOUSEWHEEL = 0x020A;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public static bool TryForward(Control source, ref System.Windows.Forms.Message m)
        {
            if (m.Msg != WM_MOUSEWHEEL || source is null)
                return false;

            ScrollableControl scrollParent = FindAutoScrollParent(source);
            if (scrollParent is null || !scrollParent.IsHandleCreated)
                return false;

            SendMessage(scrollParent.Handle, m.Msg, m.WParam, m.LParam);
            return true;
        }

        private static ScrollableControl FindAutoScrollParent(Control control)
        {
            for (Control cur = control.Parent; cur is not null; cur = cur.Parent)
            {
                if (cur is ScrollableControl { AutoScroll: true } sc)
                    return sc;
            }
            return null;
        }
    }
}

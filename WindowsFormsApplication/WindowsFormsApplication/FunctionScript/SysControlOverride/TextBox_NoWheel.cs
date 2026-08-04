using System.Windows.Forms;

namespace WindowsFormsApplication.FunctionScript.SysControlOverride
{
    /// <summary>
    /// 数值 TextBox 获得焦点时，把滚轮交给外层 AutoScroll 面板，避免卡在中间列无法滚动。
    /// </summary>
    public class TextBox_NoWheel : TextBox
    {
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (MouseWheelScrollForwarder.TryForward(this, ref m))
                return;

            base.WndProc(ref m);
        }
    }
}

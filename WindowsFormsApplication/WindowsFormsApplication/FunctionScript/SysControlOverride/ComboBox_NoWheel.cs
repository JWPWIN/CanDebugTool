using System.Windows.Forms;

namespace WindowsFormsApplication.FunctionScript.SysControlOverride
{
    /// <summary>
    /// 
    /// </summary>
    public class ComboBox_NoWheel:ComboBox
    {
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg != 0x020A)
            {
                base.WndProc(ref m);
            }
        }

    }
}

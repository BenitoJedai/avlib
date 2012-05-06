using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AVLib.Utils;

namespace VALib.Draw.Controls.Core
{
    public class BehaviourChecked : BaseBehaviour
    {
        public override void Apply(BaseControl control)
        {
            base.Apply(control);
            CurrentControl.MouseDown += CurrentControl_MouseDown;
            CurrentControl.KeyDown += CurrentControl_KeyDown;
        }

        public override void UnApply()
        {
            CurrentControl.MouseDown -= CurrentControl_MouseDown;
            CurrentControl.KeyDown -= CurrentControl_KeyDown;
            CurrentControl.Value["Checked"] = false;
            base.UnApply();
        }

        private void CurrentControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                CurrentControl.Value["Checked"] = !CurrentControl.Value["Checked"].AsBoolean();
            }
        }

        void CurrentControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                CurrentControl.Value["Checked"] = !CurrentControl.Value["Checked"].AsBoolean();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Draw.Controls.Core
{
    public static class Behaviours
    {
        public static BehaviourChecked Checked()
        {
            return new BehaviourChecked();
        }

        public static BehaviourProgress Progress()
        {
            return new BehaviourProgress();
        }

        public static BehaviourMouseDrag MouseDrag()
        {
            return new BehaviourMouseDrag();
        }
    }
}

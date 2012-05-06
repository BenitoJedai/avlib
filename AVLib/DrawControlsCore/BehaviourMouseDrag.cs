using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AVLib.Utils;

namespace VALib.Draw.Controls.Core
{
    public enum DragDirection
    {
        Vertical,
        Horizontal,
        Both
    }

    public class BehaviourMouseDrag : BaseBehaviour
    {
        public DragDirection DragDirection = DragDirection.Both;

        public override void Apply(BaseControl control)
        {
            base.Apply(control);
            CurrentControl.MouseDown += CurrentControl_MouseDown;
            CurrentControl.MouseUp += CurrentControl_MouseUp;
            CurrentControl.MouseMove += CurrentControl_MouseMove;
            CurrentControl.MouseLeave += CurrentControl_MouseLeave;
        }

        public override void UnApply()
        {
            CurrentControl.MouseDown -= CurrentControl_MouseDown;
            CurrentControl.MouseUp -= CurrentControl_MouseUp;
            CurrentControl.MouseMove -= CurrentControl_MouseMove;
            CurrentControl.MouseLeave -= CurrentControl_MouseLeave;

            if (m_inDrag)
            {
                m_inDrag = false;
                CurrentControl.Value["InDrag"] = m_inDrag;
            }

            base.UnApply();
        }

        private void CurrentControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            m_dragStartPoint = e.Location;
            m_propStart = new Point(CurrentControl.Value["PropX"].AsInteger(), CurrentControl.Value["PropY"].AsInteger());
            m_inDrag = true;
            CurrentControl.Value["DragX"] = m_dragStartPoint.X;
            CurrentControl.Value["DragY"] = m_dragStartPoint.Y;
            CurrentControl.Value["InDrag"] = m_inDrag;
        }

        private void CurrentControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (m_inDrag)
            {
                m_inDrag = false;
                CurrentControl.Value["InDrag"] = m_inDrag;
            }
        }

        private void CurrentControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (m_inDrag)
            {
                CurrentControl.Value["DragDX"] = e.X - m_dragStartPoint.X;
                CurrentControl.Value["DragDY"] = e.Y - m_dragStartPoint.Y;
                if (DragDirection != DragDirection.Vertical) 
                    CurrentControl.Value["PropX"] = m_propStart.X + e.X - m_dragStartPoint.X;
                if (DragDirection != DragDirection.Horizontal)  
                    CurrentControl.Value["PropY"] = m_propStart.Y + e.Y - m_dragStartPoint.Y;
            }
        }

        private void CurrentControl_MouseLeave(object sender, EventArgs e)
        {
            if (m_inDrag && !CurrentControl.CaptureMouseClick)
            {
                m_inDrag = false;
                CurrentControl.Value["InDrag"] = m_inDrag;
            }
        }

        private Point m_dragStartPoint;
        private Point m_propStart;
        private bool m_inDrag;
    }
}

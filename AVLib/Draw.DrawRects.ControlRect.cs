using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AVLib.Draw.DrawRects
{
    public class ControlRect : DrawRect
    {
        public event MouseEventHandler MouseMove;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseClick;
        public event EventHandler Click;
        public event MouseEventHandler MouseDoubleClick;
        public event EventHandler DoubleClick;
        public event EventHandler MouseHover;
        public event MouseEventHandler MouseWheel;
        public event EventHandler EnabledChanged;

        private bool m_enabled = true;
        private ControlRect m_lastMouseControl = null;

        public bool Enabled
        {
            get { return m_enabled; }
            set
            {
                if (m_enabled != value)
                {
                    m_enabled = true;
                    if (EnabledChanged != null) EnabledChanged(this, new EventArgs());
                    Invalidate();
                }
            }
        }

        public ControlRect(Control control, Point pos, int width, int height) : base(control, pos, width, height)
        {
        }

        protected ControlRect(): base()
        {
            
        }

        protected override DrawRect CreateInstance()
        {
            return new ControlRect();
        }

        public ControlRect GetChildControlEt(Point pt)
        {
            var res = GetChildEt(pt);
            while (res != null && !(res is ControlRect))
                res = res.GetChildEt(pt);
            return res == null ? null : res as ControlRect;
        }

        public ControlRect MouseChild(Point pt)
        {
            var ch = GetChildControlEt(pt);
            if (ch != m_lastMouseControl)
            {
                if (m_lastMouseControl != null) m_lastMouseControl.OnMouseLeave(new EventArgs());
                if (ch != null) ch.OnMouseEnter(new EventArgs());
            }
            m_lastMouseControl = ch;
            return ch;
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            if (!m_enabled) return;
            var ch = MouseChild(e.Location);
            if (MouseMove != null) MouseMove(this, e);
            if (ch != null) ch.OnMouseMove(e);
        }

        public void OnMouseEnter(EventArgs e)
        {
            if (!m_enabled) return;
            if (m_lastMouseControl != null)
            {
                m_lastMouseControl.OnMouseLeave(e);
                m_lastMouseControl = null;
            }
            if (MouseEnter != null) MouseEnter(this, e);
        }

        public void OnMouseLeave(EventArgs e)
        {
            if (!m_enabled) return;
            if (m_lastMouseControl != null)
            {
                m_lastMouseControl.OnMouseLeave(e);
                m_lastMouseControl = null;
            }
            if (MouseLeave != null) MouseLeave(this, e);
        }

        public void OnMouseDown(MouseEventArgs e)
        {
            if (!m_enabled) return;
            var ch = MouseChild(e.Location);
            if (MouseDown != null) MouseDown(this, e);
            if (ch != null) ch.OnMouseDown(e);
        }

        public void OnMouseUp(MouseEventArgs e)
        {
            if (!m_enabled) return;
            var ch = MouseChild(e.Location);
            if (MouseUp != null) MouseUp(this, e);
            if (ch != null) ch.OnMouseUp(e);
        }

        public void OnMouseClick(MouseEventArgs e)
        {
            if (!m_enabled) return;
            var ch = MouseChild(e.Location);
            if (MouseClick != null) MouseClick(this, e);
            if (Click != null) Click(this, e);
            if (ch != null) ch.OnMouseClick(e);
        }

        public void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (!m_enabled) return;
            var ch = MouseChild(e.Location);
            if (MouseDoubleClick != null) MouseDoubleClick(this, e);
            if (DoubleClick != null) DoubleClick(this, e);
            if (ch != null) ch.OnMouseDoubleClick(e);
        }

        public void OnMouseHover(EventArgs e)
        {
            if (!m_enabled) return;
            if (m_lastMouseControl != null)
                m_lastMouseControl.OnMouseHover(e);
            else
            {
                if (MouseHover != null) MouseHover(this, e);
            }
        }

        public void OnMouseWheel(MouseEventArgs e)
        {
            if (!m_enabled) return;
            var ch = MouseChild(e.Location);
            if (MouseWheel != null) MouseWheel(this, e);
            if (ch != null) ch.OnMouseWheel(e);
        }
    }
}

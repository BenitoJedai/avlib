using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AVLib.Draw.DrawRects;
using AVLib.Draw.DrawRects.Painters;

namespace AVLib.Controls
{
    public partial class DrawControl : Control
    {
        private ControlRect m_mainRect;
        private object lockObject = new object();

        public ControlRect MainRect
        {
            get { return m_mainRect; }
        }

        public DrawControl()
        {
            this.DoubleBuffered = true;
            InitializeComponent();

            m_mainRect = new ControlRect(this, new Point(0, 0), Width, Height);
            m_mainRect.Painters[0].FillRect(SystemColors.Control);
            this.Resize += new EventHandler(DrawControl_Resize);

            m_mainRect.OnInvalidate += new DrawRect.OnValidateHandler(m_mainRect_OnInvalidate);
            m_mainRect.OnChildMoved += new DrawRect.NotifyHandler(m_mainRect_OnChildMoved);

            InitializeRects();
        }

        private void m_mainRect_OnChildMoved(object sender)
        {
            var r = m_mainRect.MouseChild(m_mainRect.LastMousePos);
            while (r != null)
                r = r.MouseChild(m_mainRect.LastMousePos);
        }

        private void DrawControl_Resize(object sender, EventArgs e)
        {
            m_mainRect.Size = new Size(Width, Height);
        }

        private delegate void InvalidateHandler(Region rect);
        private void m_mainRect_OnInvalidate(Region rect)
        {
            if (InvokeRequired)
                Invoke(new InvalidateHandler(m_mainRect_OnInvalidate), rect);
            else
                Invalidate(rect);
        }

        protected virtual void InitializeRects()
        {
            
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            this.m_mainRect.Paint(pe.Graphics);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            m_mainRect.OnMouseMove(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            m_mainRect.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            m_mainRect.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            m_mainRect.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            m_mainRect.OnMouseUp(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            m_mainRect.OnMouseClick(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            m_mainRect.OnMouseDoubleClick(e);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            m_mainRect.OnMouseHover(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            m_mainRect.OnMouseWheel(e);
        }
    }
}

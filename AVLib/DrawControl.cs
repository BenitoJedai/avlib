﻿using System;
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
        private Bitmap m_paintMap;
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

            InitializeRects();
        }

        private delegate void InvalidateHandler(Region rect);
        private void m_mainRect_OnInvalidate(Region rect)
        {
            if (InvokeRequired)
                Invoke(new InvalidateHandler(m_mainRect_OnInvalidate), rect);
            else
                Invalidate(rect);
        }

        private object LockObject()
        {
            return (m_paintMap == null) ? lockObject : (object)m_paintMap;
        }

        private void RecreateDrawMap()
        {
            if (ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
            {
                lock (LockObject())
                {
                    if (m_paintMap == null || m_paintMap.Width < ClientRectangle.Width || m_paintMap.Height < ClientRectangle.Height)
                    {
                        m_paintMap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
                        m_mainRect.Image = m_paintMap;
                        m_mainRect.Graf = Graphics.FromImage(m_paintMap);
                    }
                    else
                    {
                        if (m_mainRect.Graf == null)
                            m_mainRect.Graf = Graphics.FromImage(m_paintMap);
                    }
                }
                m_mainRect.Size = new Size(ClientRectangle.Width, ClientRectangle.Height);
            }
            else
            {
                m_mainRect.Graf = null;
            }
        }

        private void DrawControl_Resize(object sender, EventArgs e)
        {
            RecreateDrawMap();
        }

        protected virtual void InitializeRects()
        {
            
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (m_paintMap == null) RecreateDrawMap();
            if (m_paintMap != null)
                lock (m_paintMap)
                {
                    pe.Graphics.DrawImage(m_paintMap, 0, 0);
                }
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

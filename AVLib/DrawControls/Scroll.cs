using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AVLib.Draw.DrawRects;
using AVLib.Draw.DrawRects.Painters;
using AVLib.Utils;

namespace VALib.Draw.Controls
{
    public enum DrawScrollAlign
    {
        Horizontal,
        Vertical
    }

    public class DrawScroll : ControlRect
    {
        private DrawScrollAlign m_align = DrawScrollAlign.Vertical;
        public DrawScrollAlign Align
        {
            get { return m_align; }
            set
            {
                if (m_align != value)
                {
                    m_align = value;
                }
            }
        }

        private Color m_color = SystemColors.Control;
        public Color Color
        {
            get { return m_color; }
            set
            {
                if (m_color != value)
                {
                    m_color = value;
                    button1.Color = value;
                    button2.Color = value;
                    scrollButton.Color = value;
                }
            }
        }

        private int m_scrollPos = 0;
        private int m_scrollWidth = 10;
        private int ScrollMaxWay()
        {
            var sz = m_align == DrawScrollAlign.Vertical
                         ? Rect.Height - button1.Rect.Height - button2.Rect.Height
                         : Rect.Width - button1.Rect.Width - button2.Rect.Width;
            sz -= m_scrollWidth;
            sz -= BorderSize*2;
            return sz;
        }
        private void SetScrollPos(int newValue)
        {
            var max = ScrollMaxWay();
            if (newValue < 0) newValue = 0;
            if (newValue > max) newValue = max;
            if (newValue != m_scrollPos)
            {
                m_scrollPos = newValue;
                scrollButton.Pos = m_align == DrawScrollAlign.Vertical ? new Point(0, ScrollPos()) : new Point(ScrollPos(), 0);
            }
        }
        private int ScrollPos()
        {
            return m_align == DrawScrollAlign.Vertical
                       ? button1.Rect.Height + m_scrollPos
                       : button1.Rect.Width + m_scrollPos;
        }

        private DrawButton button1;
        public DrawButton Button1 { get { return button1; } }
        private DrawButton button2;
        public DrawRect Button2 { get { return button2; } }
        private DrawButton scrollButton;
        public DrawButton ScrollButton { get { return scrollButton; } }

        protected override void InitializeControl()
        {
            button1 = new DrawButton();
            button1.Size = Size;
            button1.CornerRadius = 1;
            button1.BorderSize = 1;
            button1.Alignment = RectAlignment.Top;
            button1.Transparent = true;
            button1.Flat = true;
            this.Add(button1);

            scrollButton = new DrawButton();
            scrollButton.Pos = new Point(0, button1.Height);
            scrollButton.Size = Size;
            scrollButton.CornerRadius = 1;
            scrollButton.BorderSize = 1;
            scrollButton.Alignment = RectAlignment.Absolute;
            scrollButton.Transparent = true;
            scrollButton.MouseDown += new System.Windows.Forms.MouseEventHandler(scrollButton_MouseDown);
            scrollButton.MouseUp += new System.Windows.Forms.MouseEventHandler(scrollButton_MouseUp);
            scrollButton.MouseMove += new System.Windows.Forms.MouseEventHandler(scrollButton_MouseMove);
            this.Add(scrollButton);

            button2 = new DrawButton();
            button2.Size = Size;
            button2.CornerRadius = 1;
            button2.BorderSize = 1;
            button2.Alignment = RectAlignment.Bottom;
            button2.Transparent = true;
            button2.Flat = true;
            this.Add(button2);

            this.Painters[0].Add(new CustomPainter(new PainterPaintHandler(PaintBar), "bar"));
            this.Resize += new ItemChangeHandler(DrawScroll_Resize);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(DrawScroll_MouseWheel);
        }

        private void DrawScroll_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            SetScrollPos(m_scrollPos + (int)(-e.Delta/120*5));
        }

        private Point downMousePoint;
        private int downScrollPos;
        private bool inMouseDown;
        private void scrollButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            downMousePoint = e.Location;
            downScrollPos = m_scrollPos;
            inMouseDown = true;
        }

        private void scrollButton_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            inMouseDown = false;
        }

        void scrollButton_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (inMouseDown)
            {
                int dx = e.Location.X - downMousePoint.X;
                int dy = e.Location.Y - downMousePoint.Y;
                if (m_align == DrawScrollAlign.Vertical)
                    SetScrollPos(downScrollPos + dy);
                else
                    SetScrollPos(downScrollPos + dx);
            }
        }

        private void DrawScroll_Resize(DrawRect rect)
        {
            int size = (m_align == DrawScrollAlign.Vertical ? Rect.Width : Rect.Height) - BorderSize*2;

            DisableInvalidate();
            try
            {
                button1.Size = new Size(size, size);
                button2.Size = new Size(size, size);
                scrollButton.Size = m_align == DrawScrollAlign.Vertical ? new Size(size, m_scrollWidth) : new Size(m_scrollWidth, size);
                scrollButton.Pos = m_align == DrawScrollAlign.Vertical ? new Point(0, ScrollPos()) : new Point(ScrollPos(), 0);
            }
            finally
            {
                EnableInvalidate();
            }
        }

        private void PaintBar(DrawRect rect, Graphics graf)
        {
            var drawRect = rect.Rect;
            if (BorderSize > 0)
            {
                DrawRectMethods.DrawRectangle(rect.Rect, graf, m_color.DarkColor(30), BorderSize);
                drawRect.Inflate(-BorderSize, -BorderSize);
            }
            //DrawRectMethods.FillRect(drawRect, graf, m_color.BrightColor(20));
            DrawRectMethods.FillRectGradient(drawRect, graf, m_color.BrightColor(20), m_color.DarkColor(5), SimpleDirection.Horisontal);

//            var pen = new Pen(m_color.DarkColor(10), 4);
//            switch (m_align)
//            {
//                case DrawScrollAlign.Vertical:
//                    graf.DrawLine(pen, drawRect.X + 1, drawRect.Y, drawRect.X + 1, drawRect.Bottom);
//                    graf.DrawLine(pen, drawRect.Right - 1, drawRect.Y, drawRect.Right - 1, drawRect.Bottom);
//                    break;
//                case DrawScrollAlign.Horizontal:
//                    graf.DrawLine(pen, drawRect.X , drawRect.Y + 1, drawRect.Right, drawRect.Y + 1);
//                    graf.DrawLine(pen, drawRect.X, drawRect.Bottom - 1, drawRect.Right, drawRect.Bottom - 1);
//                    break;
//            }
            
        }
    }
}

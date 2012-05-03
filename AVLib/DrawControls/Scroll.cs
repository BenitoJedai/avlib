using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AVLib.Draw.DrawRects;
using AVLib.Draw.DrawRects.Painters;
using AVLib.Utils;
using VALib.Draw.Controls.Core;

namespace VALib.Draw.Controls
{
    public class DrawScroll : BaseEventReaction
    {

        #region Properties

        public Orientation Align
        {
            get { return Value["Align"].As<Orientation>(); }
            set { Value["Align"] = value; }
        }

        public DrawButton Button1 { get { return button1; } }
        public DrawRect Button2 { get { return button2; } }
        public DrawButton ScrollButton { get { return scrollButton; } }

        #endregion

        protected override void InitializeControl()
        {
            base.InitializeControl();
            InitProperties();

            button1 = new DrawButton();
            button1.Size = Size;
            button1.CornerRadius = 1;
            button1.BorderSize = 1;
            button1.Alignment = RectAlignment.Top;
            button1.Transparent = true;
            button1.Flat = true;
            button1.Value["Color"] = Property["Color"];
            this.Add(button1);

            scrollButton = new DrawButton();
            scrollButton.Pos = new Point(0, button1.Height);
            scrollButton.Size = Size;
            scrollButton.CornerRadius = 1;
            scrollButton.BorderSize = 1;
            scrollButton.Alignment = RectAlignment.Absolute;
            scrollButton.Transparent = true;
            scrollButton.CaptureMouseClick = true;
            scrollButton.Value["Color"] = Property["Color"];
            scrollButton.MouseDown += scrollButton_MouseDown;
            scrollButton.MouseMove += scrollButton_MouseMove;
            this.Add(scrollButton);

            button2 = new DrawButton();
            button2.Size = Size;
            button2.CornerRadius = 1;
            button2.BorderSize = 1;
            button2.Alignment = RectAlignment.Bottom;
            button2.Transparent = true;
            button2.Flat = true;
            button2.Value["Color"] = Property["Color"];
            this.Add(button2);

            this.Painters[0].Add(PaintBody, "body");
            this.Painters[0].Add(PaintBorder, "border");
            this.Resize += DrawScroll_Resize;
            this.MouseWheel += DrawScroll_MouseWheel;
        }

        public void InitProperties()
        {
            Align = Orientation.Vertical;
        }

        private DrawButton button1;
        private DrawButton button2;
        private DrawButton scrollButton;
        

        private int m_scrollPos = 0;
        private int m_scrollWidth = 10;
        private int ScrollMaxWay()
        {
            var sz = Align == Orientation.Vertical
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
                scrollButton.Pos = Align == Orientation.Vertical ? new Point(0, ScrollPos()) : new Point(ScrollPos(), 0);
            }
        }
        private int ScrollPos()
        {
            return Align == Orientation.Vertical
                       ? button1.Rect.Height + m_scrollPos
                       : button1.Rect.Width + m_scrollPos;
        }

        private void DrawScroll_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            SetScrollPos(m_scrollPos + (int)(-e.Delta/120*5));
        }

        private Point downMousePoint;
        private int downScrollPos;
        private void scrollButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            downMousePoint = e.Location;
            downScrollPos = m_scrollPos;
        }

        private int i = 0;
        void scrollButton_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (scrollButton.MouseIsDown)
            {
                int dx = e.Location.X - downMousePoint.X;
                int dy = e.Location.Y - downMousePoint.Y;
                if (Align == Orientation.Vertical)
                    SetScrollPos(downScrollPos + dy);
                else
                    SetScrollPos(downScrollPos + dx);
            }
        }

        private void DrawScroll_Resize(DrawRect rect)
        {
            int size = (Align == Orientation.Vertical ? Rect.Width : Rect.Height) - BorderSize * 2;

            DisableInvalidate();
            try
            {
                button1.Size = new Size(size, size);
                button2.Size = new Size(size, size);
                scrollButton.Size = Align == Orientation.Vertical ? new Size(size, m_scrollWidth) : new Size(m_scrollWidth, size);
                scrollButton.Pos = Align == Orientation.Vertical ? new Point(0, ScrollPos()) : new Point(ScrollPos(), 0);
            }
            finally
            {
                EnableInvalidate();
            }
        }

        private void PaintBorder(DrawRect rect, Graphics graf)
        {
            var drawRect = rect.Rect;
            if (BorderSize > 0)
            {
                DrawRectMethods.DrawRectangle(rect.Rect, graf, Color.DarkColor(30), BorderSize);
                drawRect.Inflate(-BorderSize, -BorderSize);
            }
        }

        private void PaintBody(DrawRect rect, Graphics graf)
        {
            DrawRectMethods.FillRectGradient(rect.Rect, graf, Color.BrightColor(20), Color.DarkColor(5), SimpleDirection.Horisontal);
        }
    }
}

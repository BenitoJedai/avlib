using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AVLib.Animations;
using AVLib.Draw.DrawRects;
using AVLib.Draw.DrawRects.Painters;
using AVLib.Utils;

namespace VALib.Draw.Controls
{
    public class DrawButton : ControlRect
    {
        private Color m_color = SystemColors.Control;
        public Color Color
        {
            get { return m_color; }
            set
            {
                if (m_color != value)
                {
                    m_color = value;
                    if (!m_mouse_over) m_currentColor = value;
                    Invalidate();
                }
            }
        }

        private Color m_mouseOverColor = Color.LightYellow;
        public Color MouseOverColor
        {
            get { return m_mouseOverColor; }
            set
            {
                if (m_mouseOverColor != value)
                {
                    m_mouseOverColor = value;
                    if (m_mouse_over) CurrentColor = value;
                }
            }
        }

        private Color m_currentColor = SystemColors.Control;
        public Color CurrentColor
        {
            get { return m_currentColor; }
            set
            {
                if (m_currentColor != value)
                {
                    m_currentColor = value;
                    Invalidate();
                }
            }
        }


        private Color m_focusColor = Color.Blue;
        public Color FocusColor
        {
            get { return m_focusColor; }
            set
            {
                if (m_focusColor != value)
                {
                    m_focusColor = value;
                    if (Focused) Invalidate();
                }
            }
        }

        private bool m_drawFocus = false;
        public bool DrawFocus
        {
            get { return m_drawFocus; }
            set
            {
                if (m_drawFocus != value)
                {
                    m_drawFocus = value;
                    if (Focused) Invalidate();
                }
            }
        }

        private bool m_gradient = false;
        public bool Gradient
        {
            get { return m_gradient; }
            set
            {
                if (m_gradient != value)
                {
                    m_gradient = value;
                    Invalidate();
                }
            }
        }

        private bool m_flat = false;
        public bool Flat
        {
            get { return m_flat; }
            set
            {
                if (m_flat != value)
                {
                    m_flat = value;
                    Invalidate();
                }
            }
        }

        private int m_cornerRadius = 0;
        public int CornerRadius
        {
            get { return m_cornerRadius; }
            set
            {
                if (m_cornerRadius != value)
                {
                    m_cornerRadius = value;
                    Invalidate();
                }
            }
        }

        private string m_text = "";
        public string Text
        {
            get { return m_text; }
            set
            {
                var s = value == null ? "" : value;
                if (s != m_text)
                {
                    m_text = s;
                    Invalidate();
                }
            }
        }

        private Color m_textColor = Color.Black;
        public Color TextColor
        {
            get { return m_textColor; }
            set
            {
                if (m_textColor != value)
                {
                    m_textColor = value;
                    Invalidate();
                }
            }
        }

        #region Button Property

        public event EventHandler Click;
        public event EventHandler OnOff;

        private bool m_switch = false;
        public bool Switch
        {
            get { return m_switch; }
            set
            {
                if (m_switch != value)
                {
                    m_switch = value;
                    if (!m_switch) m_down = false;
                    Invalidate();
                }
            }
        }

        private bool m_down = false;
        public bool Down
        {
            get { return m_down; }
            set
            {
                if (m_down != value)
                {
                    m_down = value;
                    Invalidate();
                    if (OnOff != null) OnOff(this, new EventArgs());
                }
            }
        }

        private bool m_captureDown = false;
        public bool CaptuReDown
        {
            get { return m_captureDown; }
            set { m_captureDown = value; }
        }

        private DrawRect m_markRect;
        public DrawRect MarkRect
        {
            get { return m_markRect; }
        }

        private DrawRect m_contentRect;
        public DrawRect ContentRect
        {
            get { return m_contentRect; }
        }

        private Font m_font = new Font("Arial", 8);
        public Font Font
        {
            get { return m_font; }
            set
            {
                m_font = value;
                Invalidate();
            }
        }

        #endregion

        protected override void InitializeControl()
        {
            TabStop = true;
            borderPainter = new CustomPainter(new PainterPaintHandler(PaintBorder), "border");
            bodyPainter = new CustomPainter(new PainterPaintHandler(PaintBody), "body");
            markPainter = new CustomPainter(new PainterPaintHandler(PaintMarkRect), "mark");
            contentPainter = new CustomPainter(new PainterPaintHandler(PaintContent), "content");
            this.Painters[0].Add(bodyPainter);
            this.Painters[0].Add(borderPainter);

            this.MouseEnter += new EventHandler(DrawButton_MouseEnter);
            this.MouseLeave += new EventHandler(DrawButton_MouseLeave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(DrawButton_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(DrawButton_MouseUp);

            m_contentRect = new DrawRect(RectAlignment.Fill, 10);
            m_contentRect.Transparent = true;
            m_contentRect.Painters[0].Add(contentPainter);
            this.Add(m_contentRect);

            m_markRect = new DrawRect(RectAlignment.Fill, 10);
            m_markRect.Transparent = true;
            m_markRect.Painters[0].Add(markPainter);
            this.Add(m_markRect);
        }

        private void DrawButton_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_inMouseDown = false;
                if (!Switch || !m_goDown) Down = false;
                Capture(this, false);
                if (!Switch && m_mouse_over && Click != null) Click(this, new EventArgs());
            }
        }

        private bool m_goDown = false;
        private bool m_inMouseDown = false;
        private void DrawButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_goDown = !Down;
                m_inMouseDown = true;
                if (!Down) Down = true;
                Capture(this, true);
            }
        }

        private void DrawButton_MouseLeave(object sender, EventArgs e)
        {
            m_mouse_over = false;
            if (m_inMouseDown && m_goDown && !m_captureDown) Down = false;

            if (m_mouseOverColor != m_color)
            {
                this.AnimeCancel("color");
                this.Anime("color", 500).Change("CurrentColor", m_color);
            }
        }

        private void DrawButton_MouseEnter(object sender, EventArgs e)
        {
            m_mouse_over = true;
            if (m_inMouseDown && m_goDown && !m_captureDown) Down = true;

            if (m_mouseOverColor != m_color)
            {
                this.AnimeCancel("color");
                this.Anime("color", 200).ChangeDec("CurrentColor", m_mouseOverColor);
            }
        }

        #region Painters

        private bool m_mouse_over = false;

        private CustomPainter borderPainter;
        private void PaintBorder(DrawRect rect, Graphics graf)
        {
            if (m_flat && !m_mouse_over) return;

            var drawRect = rect.Rect;

            RectSides darkSides, brightSides;
            if (m_down)
            {
                brightSides = new RectSides() { right = true, bottom = true };
                darkSides = new RectSides() { left = true, top = true };
            }
            else
            {
                darkSides = new RectSides() { right = true, bottom = true };
                brightSides = new RectSides() { left = true, top = true };
            }
            DrawRectMethods.DrawRectangle(drawRect, graf, m_currentColor.DarkColor(40), BorderSize, m_cornerRadius, darkSides);
            DrawRectMethods.DrawRectangle(drawRect, graf, m_currentColor.BrightColor(40), BorderSize, m_cornerRadius, brightSides);
            DrawRectMethods.DrawRectangle(drawRect, graf, m_currentColor.DarkColor(60), 1, m_cornerRadius, darkSides);
            DrawRectMethods.DrawRectangle(drawRect, graf, m_currentColor.DarkColor(30), 1, m_cornerRadius, brightSides);
        }

        private CustomPainter bodyPainter;
        private void PaintBody(DrawRect rect, Graphics graf)
        {
            if (Gradient)
                DrawRectMethods.FillRectGradient(rect.Rect, graf, m_currentColor.BrightColor(40), m_currentColor.DarkColor(10), SimpleDirection.Vertical, m_cornerRadius);
            else
                DrawRectMethods.FillRect(rect.Rect, graf, m_currentColor, m_cornerRadius);
        }

        private CustomPainter markPainter;
        private void PaintMarkRect(DrawRect rect, Graphics graf)
        {
            if (m_drawFocus && Focused && Enabled)
                DrawRectMethods.DrawRectangle(rect.Rect, graf, FocusColor.Transparent(80), 2);

            if (!Enabled)
                DrawRectMethods.FillRect(rect.Rect, graf, Color.Silver.Transparent(80));
        }

        private CustomPainter contentPainter;
        private void PaintContent(DrawRect rect, Graphics graf)
        {
            var sz = graf.MeasureString(m_text, m_font);
            int x, y, dx, dy;
            dx = (int) (rect.Rect.Width - sz.Width);
            dy = (int) (rect.Rect.Height - sz.Height);
            if (dx > 0) x = dx/2; else x = 1;
            if (dy > 0) y = dy / 2; else y = 1;

            if (m_down) y++;

            graf.DrawString(m_text, m_font, new SolidBrush(m_textColor), rect.Rect.X + x, rect.Rect.Y + y);
            if (!Enabled)
            {
                x++;
                graf.DrawString(m_text, m_font, new SolidBrush(m_textColor.BrightColor(70)), rect.Rect.X + x, rect.Rect.Y + y);
            }
        }

        #endregion
    }
}

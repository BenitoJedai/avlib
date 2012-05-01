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
        public Color Color
        {
            get { return this["color", SystemColors.Control].AsColor(); }
            set
            {
                bool setMouseOver = Color == MouseOverColor;
                if (SetProperty("color", value))
                {
                    if (setMouseOver) MouseOverColor = value;
                    if (!m_mouse_over) CurrentColor = value;
                }
            }
        }

        public Color MouseOverColor
        {
            get { return this["MouseOverColor", SystemColors.Control].AsColor(); }
            set
            {
                if (SetProperty("MouseOverColor", value))
                    if (m_mouse_over) CurrentColor = value;
            }
        }

        public Color CurrentColor
        {
            get { return this["CurrentColor", Color].AsColor(); }
            set
            {
                if (SetProperty("CurrentColor", value))
                    Invalidate();
            }
        }


        public Color FocusColor
        {
            get { return this["FocusColor", Color.Blue].AsColor(); }
            set
            {
                if (SetProperty("FocusColor", value))
                    if (Focused) Invalidate();
            }
        }

        public bool DrawFocus
        {
            get { return this["DrawFocus"].AsBoolean(); }
            set
            {
                if (SetProperty("DrawFocus", value))
                    if (Focused) Invalidate();
            }
        }

        public bool Gradient
        {
            get { return this["Gradient"].AsBoolean(); }
            set
            {
                if (SetProperty("Gradient", value))
                    Invalidate();
            }
        }

        public bool Flat
        {
            get { return this["Flat"].AsBoolean(); }
            set
            {
                if (SetProperty("Flat", value))
                    Invalidate();
            }
        }

        public int CornerRadius
        {
            get { return this["CornerRadius"].AsInteger(); }
            set
            {
                if (SetProperty("CornerRadius", value))
                    Invalidate();
            }
        }

        public string Text
        {
            get { return this["Text"].AsString(); }
            set
            {
                if (SetProperty("Text", value == null ? "" : value))
                    Invalidate();
            }
        }

        public Color TextColor
        {
            get { return this["TextColor", Color.Navy].AsColor(); }
            set
            {
                if (SetProperty("TextColor", value))
                    Invalidate();
            }
        }

        #region Button Property

        public event EventHandler Click;
        public event EventHandler OnOff;

        public bool Switch
        {
            get { return this["Switch"].AsBoolean(); }
            set
            {
                if (SetProperty("Switch", value))
                {
                    if (!value) Down = false;
                    Invalidate();
                }
            }
        }

        public bool Down
        {
            get { return this["Down"].AsBoolean(); }
            set
            {
                if (SetProperty("Down", value))
                {
                    Invalidate();
                    if (OnOff != null) OnOff(this, new EventArgs());
                }
            }
        }

        public bool CaptuReDown
        {
            get { return this["CaptuReDown"].AsBoolean(); }
            set { SetProperty("CaptuReDown", value); }
        }

        public DrawRect MarkRect
        {
            get { return Property<DrawRect>("MarkRect", null); }
            set { SetProperty("MarkRect", value); }
        }

        public DrawRect ContentRect
        {
            get { return Property<DrawRect>("ContentRect", null); }
            set { SetProperty("ContentRect", value); }
        }

        public Font Font
        {
            get
            {
                var res = Property<Font>("Font", null);
                if (res == null) SetProperty("Font", new Font("Arial", 8));
                return Property<Font>("Font", null);
            }
            set
            {
                if (SetProperty("Font", value))
                    Invalidate();
            }
        }

        #endregion

        protected override void InitializeControl()
        {
            TabStop = true;

            this.Painters[0].Add(PaintBody, "body");
            this.Painters[0].Add(PaintBorder, "border");

            this.MouseEnter += DrawButton_MouseEnter;
            this.MouseLeave += DrawButton_MouseLeave;
            this.MouseDown += DrawButton_MouseDown;
            this.MouseUp += DrawButton_MouseUp;

            ContentRect = new DrawRect(RectAlignment.Fill, 10);
            ContentRect.Transparent = true;
            ContentRect.Painters[0].Add(PaintContent, "content");
            this.Add(ContentRect);

            MarkRect = new DrawRect(RectAlignment.Fill, 10);
            MarkRect.Transparent = true;
            MarkRect.Painters[0].Add(PaintMarkRect, "mark");
            this.Add(MarkRect);
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
            if (m_inMouseDown && m_goDown && !CaptuReDown) Down = false;

            if (MouseOverColor != Color)
            {
                this.AnimeCancel("color");
                this.Anime("color", 500).Change("CurrentColor", Color);
            }
            else
                Invalidate();
        }

        private void DrawButton_MouseEnter(object sender, EventArgs e)
        {
            m_mouse_over = true;
            if (m_inMouseDown && m_goDown && !CaptuReDown) Down = true;

            if (MouseOverColor != Color)
            {
                this.AnimeCancel("color");
                this.Anime("color", 200).ChangeDec("CurrentColor", MouseOverColor);
            }
            else
                Invalidate();
        }

        #region Painters

        private bool m_mouse_over = false;

        private void PaintBorder(DrawRect rect, Graphics graf)
        {
            if (Flat && !m_mouse_over) return;

            var drawRect = rect.Rect;

            RectSides darkSides, brightSides;
            if (Down)
            {
                brightSides = new RectSides() { right = true, bottom = true };
                darkSides = new RectSides() { left = true, top = true };
            }
            else
            {
                darkSides = new RectSides() { right = true, bottom = true };
                brightSides = new RectSides() { left = true, top = true };
            }
            DrawRectMethods.DrawRectangle(drawRect, graf, CurrentColor.DarkColor(40), BorderSize, CornerRadius, darkSides);
            DrawRectMethods.DrawRectangle(drawRect, graf, CurrentColor.BrightColor(40), BorderSize, CornerRadius, brightSides);
            DrawRectMethods.DrawRectangle(drawRect, graf, CurrentColor.DarkColor(60), 1, CornerRadius, darkSides);
            DrawRectMethods.DrawRectangle(drawRect, graf, CurrentColor.DarkColor(30), 1, CornerRadius, brightSides);
        }

        private void PaintBody(DrawRect rect, Graphics graf)
        {
            if (Gradient)
                DrawRectMethods.FillRectGradient(rect.Rect, graf, CurrentColor.BrightColor(40), CurrentColor.DarkColor(10), SimpleDirection.Vertical, CornerRadius);
            else
                DrawRectMethods.FillRect(rect.Rect, graf, CurrentColor, CornerRadius);
        }

        private void PaintMarkRect(DrawRect rect, Graphics graf)
        {
            if (DrawFocus && Focused && Enabled)
                DrawRectMethods.DrawRectangle(rect.Rect, graf, FocusColor.Transparent(80), 2);

            if (!Enabled)
                DrawRectMethods.FillRect(rect.Rect, graf, Color.Silver.Transparent(80));
        }

        private void PaintContent(DrawRect rect, Graphics graf)
        {
            var sz = graf.MeasureString(Text, Font);
            int x, y, dx, dy;
            dx = (int) (rect.Rect.Width - sz.Width);
            dy = (int) (rect.Rect.Height - sz.Height);
            if (dx > 0) x = dx/2; else x = 1;
            if (dy > 0) y = dy / 2; else y = 1;

            if (Down) y++;

            var clr = Enabled ? TextColor : TextColor.BrightColor(50);
            graf.DrawString(Text, Font, new SolidBrush(clr), rect.Rect.X + x, rect.Rect.Y + y);
        }

        #endregion
    }
}

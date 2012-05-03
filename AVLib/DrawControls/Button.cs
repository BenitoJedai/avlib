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
using VALib.Draw.Controls.Core;

namespace VALib.Draw.Controls
{
    public class DrawButton : BaseEventReaction
    {
        #region Properties

        public bool Gradient
        {
            get { return Value["Gradient"].AsBoolean(); }
            set { Value["Gradient"] = value; }
        }

        public bool Flat
        {
            get { return Value["Flat"].AsBoolean(); }
            set { Value["Flat"] = value; }
        }

        public int CornerRadius
        {
            get { return Value["CornerRadius"].AsInteger(); }
            set { Value["CornerRadius"] = value; }
        }

        public string Text
        {
            get { return Value["Text"].AsString(); }
            set { Value["Text"] = value; }
        }

        public Color TextColor
        {
            get { return Value["TextColor"].AsColor(); }
            set { Value["TextColor"] = value; }
        }

        public bool Switch
        {
            get { return Value["Switch"].AsBoolean(); }
            set { Value["Switch"] = value; }
        }

        public bool Down
        {
            get { return Value["Down"].AsBoolean(); }
            set { Value["Down"] = value; }
        }

        public DrawRect MarkRect
        {
            get { return Value["MarkRect"].As<DrawRect>(); }
            set { Value["MarkRect"] =  value; }
        }

        public DrawRect ContentRect
        {
            get { return Value["ContentRect"].As<DrawRect>(); }
            set { Value["ContentRect"] = value; }
        }

        public bool HoldDownIfMouseLeave
        {
            get { return Value["HoldDownIfMouseLeave"].AsBoolean(); }
            set { Value["HoldDownIfMouseLeave"] = value; }
        }

        public Font Font
        {
            get { return Value["Font"].As<Font>(); }
            set { Value["Font"] = value; }
        }

        #endregion

        public event EventHandler Click;
        public event EventHandler OnOff;

        protected override void InitializeControl()
        {
            base.InitializeControl();
            InitProperties();

            TabStop = true;
            this.CaptureMouseClick = true;

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

        public void InitProperties()
        {
            Property["Gradient", false]
                .Changed += () => { Invalidate(); };
            Property["Flat", false]
                .Changed += () => { Invalidate(); };
            Property["CornerRadius", 0]
                .Changed += () => { Invalidate(); };
            Property["Text", ""]
                .Changed += () => { Invalidate(); };
            Property["TextColor", Color.Navy]
                .Changed += () => { Invalidate(); };
            Property["Switch", false]
                .Changed += () =>
                                {
                                    if (!Switch) Down = false;
                                    Invalidate();
                                };
            Property["Down", false]
                .Changed += () =>
                                {
                                    Invalidate();
                                    if (OnOff != null) OnOff(this, new EventArgs());
                                };
            Property["HoldDownIfMouseLeave", false]
                .Changed += () =>
                                {
                                    if (Down && !MouseIsOver) Down = false;
                                };
            Property["Font", new Font("Arial", 8)]
                .Changed += () => { Invalidate(); };
        }
        


        private void DrawButton_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!Switch || !m_goDown) Down = false;
                if (!Switch && MouseIsOver && Click != null) Click(this, new EventArgs());
            }
        }

        private bool m_goDown = false;
        private void DrawButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_goDown = !Down;
                if (!Down) Down = true;
            }
        }

        private void DrawButton_MouseLeave(object sender, EventArgs e)
        {
            if (MouseIsDown && m_goDown && !HoldDownIfMouseLeave) Down = false;
        }

        private void DrawButton_MouseEnter(object sender, EventArgs e)
        {
            if (MouseIsDown && m_goDown && !HoldDownIfMouseLeave) Down = true;
        }

        #region Painters
        
        private void PaintBorder(DrawRect rect, Graphics graf)
        {
            if (Flat && !MouseIsOver && !Down) return;

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

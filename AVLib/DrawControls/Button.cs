using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AVLib.Animations;
using AVLib.Draw.DrawRects;
using AVLib.Draw.DrawRects.Painters;
using AVLib.Draw.DrawRects.Painters.ControlSimple;
using AVLib.Utils;
using VALib.Draw.Controls.Core;

namespace VALib.Draw.Controls
{
    public class DrawButton : BaseEventReaction
    {
        #region Properties

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

        #endregion

        public event EventHandler Click;
        public event EventHandler OnOff;

        protected override void InitializeControl()
        {
            base.InitializeControl();
            InitProperties();

            ContentRect = new DrawRect(RectAlignment.Fill, 10);
            ContentRect.Transparent = true;
            this.Add(ContentRect);

            MarkRect = new DrawRect(RectAlignment.Fill, 10);
            MarkRect.Transparent = true;
            //MarkRect.Painters[0].Add(PaintMarkRect, "mark");
            this.Add(MarkRect);

            InitPainters();

            TabStop = true;
            this.CaptureMouseClick = true;

            this.MouseEnter += DrawButton_MouseEnter;
            this.MouseLeave += DrawButton_MouseLeave;
            this.MouseDown += DrawButton_MouseDown;
            this.MouseUp += DrawButton_MouseUp;
        }

        public void InitProperties()
        {
            AddValidatedProperty("Gradient", false);
            AddValidatedProperty("Flat", false);
            AddValidatedProperty("CornerRadius", 0);
            AddValidatedProperty("Text", "");
            AddValidatedProperty("TextColor", Color.Navy);
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
            AddValidatedProperty("Font", () => { return new Font("Arial", 8); });
            TextAlignment = StringAlignment.Center;
            DrawFocus = true;

            Value["FlatIsNow"] = new Func<object>(() => { return Flat && !MouseIsOver && !Down; });
        }
        
        public void InitPainters()
        {
            var body = Painters[0].Add(ControlSimpleDraw.Body, "body");
            body.Value["Color", (c) => { return Gradient ? c.AsColor().BrightColor(40) : c; }] = Property["CurrentColor"];
            body.Value["Gradient"] = Property["Gradient"];
            body.Value["Color2", (c) => { return c.AsColor().DarkColor(10); }] = Property["CurrentColor"];
            body.Value["Direction"] = Property["Direction"];
            body.Value["CornerRadius"] = new Func<object>(() => { return Value["FlatIsNow"].AsBoolean() ? 0 : CornerRadius; }); 

            var border = Painters[0].Add(ControlSimpleDraw.ButtonBorder, "border");
            border.Value["Color"] = Property["CurrentColor"];
            border.Value["Down"] = Property["Down"];
            border.Value["CornerRadius"] = Property["CornerRadius"];
            border.Value["Enabled"] = new Func<object>(() => { return !Value["FlatIsNow"].AsBoolean(); });

            var content = ContentRect.Painters[0].Add(BasePainters.Text, "content");
            content.Value["Text"] = Property["Text"];
            content.Value["Font"] = Property["Font"];
            content.Value["Alignment"] = Property["TextAlignment"];
            content.Value["VertAlignment"] = Property["TextVertAlignment"];
            content.Value["Offset"] = new Func<object>(() => { return Down ? new Point(0, 1) : new Point(0, 0); });
            content.Value["Color"] = new Func<object>(() => { return Enabled ? TextColor : TextColor.BrightColor(50); }); 

            var focus = MarkRect.Painters[0].Add(BasePainters.Rect, "focus");
            focus.Value["Color", (c) => { return c.AsColor().Transparent(80); }] = Property["FocusColor"];
            focus.Value["Width"] = 2;
            //focus.Value["CornerRadius"] = Property["CornerRadius"];
            focus.Value["Enabled"] = new Func<object>(() => { return DrawFocus && Focused && Enabled; });

            var disabled = MarkRect.Painters[0].Add(BasePainters.FillRect, "disabled");
            disabled.Value["Color"] = Color.Silver.Transparent(80);
            disabled.Value["CornerRadius"] = Property["CornerRadius"];
            disabled.Value["Enabled"] = new Func<object>(() => { return !Enabled; });
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
    }
}

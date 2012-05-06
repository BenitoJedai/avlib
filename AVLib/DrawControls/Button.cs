using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class DrawButton : BaseControl
    {
        #region Properties

        public bool Switch
        {
            get { return Value["Switch"].AsBoolean(); }
            set { Value["Switch"] = value; }
        }

        public bool Checked
        {
            get { return Value["Checked"].AsBoolean(); }
            set { Value["Checked"] = value; }
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

        #endregion

        
        public event EventHandler CheckedChanged;

        protected override void InitializeControl()
        {
            base.InitializeControl();
            InitProperties();

            ContentRect = new DrawRect(RectAlignment.Fill, 10);
            ContentRect.Transparent = true;
            this.Add(ContentRect);

            MarkRect = new DrawRect(RectAlignment.Fill, 10);
            MarkRect.Transparent = true;
            this.Add(MarkRect);

            InitPainters();

            TabStop = true;
            CaptureMouseClick = true;

            this.KeyDown += DrawButton_KeyDown;
            this.KeyUp += DrawButton_KeyUp;
            this.Leave += DrawButton_Leave;
        }

        public void InitProperties()
        {
            AddValidatedProperty("Gradient", false);
            AddValidatedProperty("Flat", false);
            AddValidatedProperty("CornerRadius", 0);
            AddValidatedProperty("Text", "");
            AddValidatedProperty("TextColor", Color.Navy);
            AddValidatedProperty("MouseIsDown", false);
            AddValidatedProperty("ControlKeyPressed", false);
            Property["Switch", false]
                .Changed += () =>
                                {
                                    if (Switch)
                                        Behaviours.Add(Core.Behaviours.Checked(), "checked");
                                    else
                                        Behaviours.Remove("checked");
                                    Invalidate();
                                };
            Value["Down"] = new Func<object>(() => { return MouseIsDown || Checked || Value["ControlKeyPressed"].AsBoolean(); });

            AddValidatedProperty("Font", new Font("Arial", 8)); 
            TextAlignment = StringAlignment.Center;
            DrawFocus = true;
            Value["FlatIsNow"] = new Func<object>(() => { return Flat && !MouseIsOver && !Down; });
            Property["Checked", false].Changed += () =>
                                                      {
                                                          Invalidate();
                                                          if (CheckedChanged != null) CheckedChanged(this, new EventArgs());
                                                      };
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
            focus.Value["Enabled"] = new Func<object>(() => { return DrawFocus && Focused && Enabled; });

            var disabled = MarkRect.Painters[0].Add(BasePainters.FillRect, "disabled");
            disabled.Value["Color"] = Color.Silver.Transparent(80);
            disabled.Value["CornerRadius"] = Property["CornerRadius"];
            disabled.Value["Enabled"] = new Func<object>(() => { return !Enabled; });
        }

        private void DrawButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
                Value["ControlKeyPressed"] = true;
        }

        private void DrawButton_Leave(object sender, EventArgs e)
        {
            Value["ControlKeyPressed"] = false;
        }

        private void DrawButton_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter) && Value["ControlKeyPressed"].AsBoolean())
            {
                Value["ControlKeyPressed"] = false;
                DoClick();
            }
        }
    }
}

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
    public class DrawScroll : BaseControl
    {

        #region Properties

        public event EventHandler PosChanged;

        public Orientation Align
        {
            get { return Value["Align"].As<Orientation>(); }
            set { Value["Align"] = value; }
        }

        public DrawButton Button1 { get { return button1; } }
        public DrawRect Button2 { get { return button2; } }
        public DrawButton ScrollButton { get { return scrollButton; } }

        public int Position
        {
            get { return Value["Position"].AsInteger(); }
            set { Value["Position"] = value; }
        }

        public int MaxPosition
        {
            get { return Value["MaxPosition"].AsInteger(); }
            set { Value["MaxPosition"] = value; }
        }

        public int SmallChange
        {
            get { return Value["SmallChange"].AsInteger(); }
            set { Value["SmallChange"] = value; }
        }

        public int LargeChange
        {
            get { return Value["LargeChange"].AsInteger(); }
            set { Value["LargeChange"] = value; }
        }

        private int TranslatedPosition
        {
            get { return Value["TranslatedPosition"].AsInteger(); }
            set { Value["TranslatedPosition"] = value; }
        }

        private int TranslatedMaxPosition
        {
            get { return Value["TranslatedMaxPosition"].AsInteger(); }
            set { Value["TranslatedMaxPosition"] = value; }
        }

        #endregion

        private BehaviourMouseDrag scrollButtonDragBehaviour;
        private BehaviourProgress progressBehaviour;
        protected override void InitializeControl()
        {
            base.InitializeControl();
            progressBehaviour = new BehaviourProgress();
            Behaviours.Add(progressBehaviour, "progress");

            button1 = new DrawButton();
            button1.Size = Size;
            button1.CornerRadius = 1;
            button1.BorderSize = 1;
            button1.Alignment = RectAlignment.Top;
            button1.Transparent = true;
            button1.Flat = true;
            button1.TabStop = false;
            button1.Value["Color"] = Property["Color"];
            button1.Value["MouseOverColor", (c) => { return c.AsColor().DarkColor(20); }] = Property["Color"];
            button1.UseMouseOverColor = true;
            button1.Gradient = true;
            button1.Value["Direction"] = Property["Direction"];
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
            scrollButton.Value["MouseOverColor", (c) => { return c.AsColor().DarkColor(20); }] = Property["Color"];
            scrollButton.UseMouseOverColor = true;
            scrollButton.TabStop = false;
            scrollButtonDragBehaviour = new BehaviourMouseDrag();
            scrollButton.Behaviours.Add(scrollButtonDragBehaviour, "drag");
            scrollButton.Gradient = true;
            scrollButton.Value["Direction"] = Property["Direction"];
            this.Add(scrollButton);

            button2 = new DrawButton();
            button2.Size = Size;
            button2.CornerRadius = 1;
            button2.BorderSize = 1;
            button2.Alignment = RectAlignment.Bottom;
            button2.Transparent = true;
            button2.Flat = true;
            button2.TabStop = false;
            button2.Value["Color"] = Property["Color"];
            button2.Value["MouseOverColor", (c) => { return c.AsColor().DarkColor(20); }] = Property["Color"];
            button2.UseMouseOverColor = true;
            button2.Gradient = true;
            button2.Value["Direction"] = Property["Direction"];
            this.Add(button2);

            InitProperties();
            InitializePeinters();

            this.Resize += DrawScroll_Resize;
            this.PostMouseWheel += DrawScroll_MouseWheel;
            this.MouseDown += DrawScroll_MouseDown;
            this.MouseUp += DrawScroll_MouseUp;
            this.MouseEnter += (s, e) => { button1.Flat = false; button2.Flat = false; };
            this.MouseLeave += (s, e) => { button1.Flat = true; button2.Flat = true; };

            button1.MouseDown += (s, e) =>
                                     {
                                         Position -= SmallChange;
                                         button1.AnimeQueue("pos").Wait(250).Custom(100, MaxPosition, (d) => { Position -= SmallChange; });
                                     };
            button1.MouseUp += (s, e) => { button1.AnimeCancel("pos"); };
            button2.MouseDown += (s, e) =>
                                     {
                                         Position += SmallChange;
                                         button2.AnimeQueue("pos").Wait(250).Custom(100, MaxPosition, (d) => { Position += SmallChange; });
                                     };
            button2.MouseUp += (s, e) => { button2.AnimeCancel("pos"); };
        }
        

        public void InitProperties()
        {
            Property["Align", Orientation.Vertical]
                .AnyChanged += () => { scrollButtonDragBehaviour.DragDirection = Align == Orientation.Horizontal ? DragDirection.Horizontal : DragDirection.Vertical; };
            scrollButtonDragBehaviour.DragDirection = DragDirection.Vertical;

            Property["TranslatedPosition"].AnyChanged +=
                () =>
                {
                    scrollButton.Pos = Align == Orientation.Vertical
                                           ? new Point(0, ScrollPos())
                                           : new Point(ScrollPos(), 0);
                };
            scrollButton.Value["PropX"] = Property["TranslatedPosition"];
            scrollButton.Value["PropY"] = Property["TranslatedPosition"];

            Property["TranslatedPosition"]
                .AnyChanged += () => { SetScrollPos(); };

            Property["Position"].AnyChanged += () => { if (PosChanged != null) PosChanged(this, new EventArgs()); };
            Value["Direction"] = new Func<object>(() =>
                                                         {
                                                             if (this.Align == Orientation.Horizontal)
                                                                 return SimpleDirection.Vertical;
                                                             return SimpleDirection.Horisontal;

                                                         });
            SmallChange = 1;
            LargeChange = 10;
        }

        public void InitializePeinters()
        {
            var body = this.Painters[0].Add(BasePainters.FillGradient, "body");
            body.Value["Color", (c) => { return c.AsColor().BrightColor(20); }] = Property["CurrentColor"];
            body.Value["Color2", (c) => { return c.AsColor().DarkColor(5); }] = Property["CurrentColor"];
            body.Value["Direction"] = Property["Direction"];
            body.Value["CornerRadius"] = Property["CornerRadius"];

            var border = this.Painters[0].Add(BasePainters.Rect, "border");
            border.Value["Color", (c) => { return c.AsColor().DarkColor(30); }] = Property["CurrentColor"];
            border.Value["CornerRadius"] = Property["CornerRadius"];

            var btArrowUp = button1.Painters[0].Add(ControlSimpleDraw.ScrollArrowUp, "arrowup");
            btArrowUp.Value["Color"] = new Func<object>(() => { return button1.MouseIsOver ? Color.Aqua : Color; });
            btArrowUp.Value["Enabled", (a) => { return a.As<Orientation>() == Orientation.Vertical; }] = Property["Align"];

            var btArrowLeft = button1.Painters[0].Add(ControlSimpleDraw.ScrollArrowLeft, "arrowleft");
            btArrowLeft.Value["Color"] = new Func<object>(() => { return button1.MouseIsOver ? Color.Aqua : Color; });
            btArrowLeft.Value["Enabled", (a) => { return a.As<Orientation>() == Orientation.Horizontal; }] = Property["Align"];

            var btArrowDown = button2.Painters[0].Add(ControlSimpleDraw.ScrollArrowDown, "arrowdown");
            btArrowDown.Value["Color"] = new Func<object>(() => { return button2.MouseIsOver ? Color.Aqua : Color; });
            btArrowDown.Value["Enabled", (a) => { return a.As<Orientation>() == Orientation.Vertical; }] = Property["Align"];

            var btArrowRight = button2.Painters[0].Add(ControlSimpleDraw.ScrollArrowRight, "arrowright");
            btArrowRight.Value["Color"] = new Func<object>(() => { return button2.MouseIsOver ? Color.Aqua : Color; });
            btArrowRight.Value["Enabled", (a) => { return a.As<Orientation>() == Orientation.Horizontal; }] = Property["Align"];
        }

        private DrawButton button1;
        private DrawButton button2;
        private DrawButton scrollButton;


        private int m_scrollWidth = 10;

        private int ScrollMaxWay()
        {
            var sz = Align == Orientation.Vertical
                         ? Rect.Height - button1.Rect.Height - button2.Rect.Height
                         : Rect.Width - button1.Rect.Width - button2.Rect.Width;
            sz -= m_scrollWidth;
            sz -= BorderSize * 2;
            return sz;
        }

        private void SetScrollPos()
        {
            scrollButton.Pos = Align == Orientation.Vertical ? new Point(0, ScrollPos()) : new Point(ScrollPos(), 0);
        }

        private int ScrollPos()
        {
            return Align == Orientation.Vertical
                       ? button1.Rect.Height + TranslatedPosition
                       : button1.Rect.Width + TranslatedPosition;
        }

        private void DrawScroll_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            TranslatedPosition += -e.Delta / 120 * 5;
        }

        private void DrawScroll_MouseDown(object sender, MouseEventArgs e)
        {
            this.AnimeCancel("scroll");

            int dif = 0;
            if (Align == Orientation.Vertical)
            {
                if (e.Y < scrollButton.Rect.Top) dif = e.Y - scrollButton.Rect.Top;
                else
                    if (e.Y > scrollButton.Rect.Bottom) dif = e.Y - scrollButton.Rect.Bottom;
            }
            else
            {
                if (e.X < scrollButton.Rect.Left) dif = e.X - scrollButton.Rect.Left;
                else
                    if (e.X > scrollButton.Rect.Right) dif = e.X - scrollButton.Rect.Right;
            }

            if (dif == 0) return;
            int maxDif = progressBehaviour.PosToTranslated(LargeChange);
            int to = TranslatedPosition + dif;
            TranslatedPosition = TranslatedPosition.NearBy(to, maxDif);
            
            int n = TranslatedPosition.StepCount(to, maxDif);

            
            this.AnimeQueue("scroll").Wait(250).Custom(100, n + 1, (d) =>
                                                                   {
                                                                       if (d.Canceled) return;
                                                                       TranslatedPosition = TranslatedPosition.NearBy(to, maxDif);
                                                                   });
        }

        private void DrawScroll_MouseUp(object sender, MouseEventArgs e)
        {
            this.AnimeCancel("scroll");
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
                button1.Alignment = Align == Orientation.Vertical ? RectAlignment.Top : RectAlignment.Left;
                button2.Alignment = Align == Orientation.Vertical ? RectAlignment.Bottom : RectAlignment.Right;
            }
            finally
            {
                EnableInvalidate();
            }

            TranslatedMaxPosition = ScrollMaxWay();
        }
    }
}

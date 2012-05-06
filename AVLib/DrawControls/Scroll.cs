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
        protected override void InitializeControl()
        {
            base.InitializeControl();
            Behaviours.Add(Core.Behaviours.Progress(), "progress");

            button1 = new DrawButton();
            button1.Size = Size;
            button1.CornerRadius = 1;
            button1.BorderSize = 1;
            button1.Alignment = RectAlignment.Top;
            button1.Transparent = true;
            button1.Flat = true;
            button1.TabStop = false;
            button1.Value["Color"] = Property["Color"];
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
            button2.Gradient = true;
            button2.Value["Direction"] = Property["Direction"];
            this.Add(button2);

            InitProperties();
            InitializePeinters();

            this.Resize += DrawScroll_Resize;
            this.MouseWheel += DrawScroll_MouseWheel;

            button1.MouseDown += (s, e) =>
                                     {
                                         Position--;
                                         button1.AnimeQueue("pos").Wait(250).Custom(100, MaxPosition, (d) => { Position--; });
                                     };
            button1.MouseUp += (s, e) => { button1.AnimeCancel("pos"); };
            button2.MouseDown += (s, e) =>
                                     {
                                         Position++;
                                         button2.AnimeQueue("pos").Wait(250).Custom(100, MaxPosition, (d) => { Position++; });
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

            TranslatedMaxPosition = ScrollMaxWay();
        }
    }
}

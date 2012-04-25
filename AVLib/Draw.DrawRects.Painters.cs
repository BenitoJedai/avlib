using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;

namespace AVLib.Draw.DrawRects.Painters
{
    public enum SimpleDirection
    {
        Horisontal,
        Vertical,
        Diagonal,
        Diagonal2
    }

    public class PainterFillRectGradient : RectPainter
    {
        private SimpleDirection direction;
        private Color color1;
        private Color color2;
        public PainterFillRectGradient(SimpleDirection direction, Color color1, Color color2, string name)
        {
            this.direction = direction;
            this.color1 = color1;
            this.color2 = color2;
            Name = name;
        }
        public SimpleDirection Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                DoChange();
            }
        }
        public Color Color1
        {
            get { return color1; }
            set
            {
                color1 = value;
                DoChange();
            }
        }
        public Color Color2
        {
            get { return color2; }
            set
            {
                color2 = value;
                DoChange();
            }
        }

        public override void Paint(DrawRect rect, Graphics graf)
        {
            Point p1 = direction != SimpleDirection.Diagonal2
                         ? new Point(rect.Rect.X, rect.Rect.Y)
                         : new Point(rect.Rect.Right, rect.Rect.Y);
            Point p2;
            switch (direction)
            {
                case SimpleDirection.Vertical:
                case SimpleDirection.Diagonal2:
                    p2 = new Point(rect.Rect.X, rect.Rect.Bottom);
                    break;
                case SimpleDirection.Horisontal:
                    p2 = new Point(rect.Rect.Right, rect.Rect.Y);
                    break;
                default:
                    p2 = new Point(rect.Rect.Right, rect.Rect.Bottom);
                    break;
            }
            if (p1 != p2)
                graf.FillRectangle(new LinearGradientBrush(p1, p2, color1, color2), rect.Rect);
            else
                graf.FillRectangle(new SolidBrush(color1), rect.Rect);
        }
    }

    public class PainterFillRect : RectPainter
    {
        private Color color;
        public PainterFillRect(Color color, string name)
        {
            this.color = color;
            Name = name;
        }
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                DoChange();
            }
        }

        public override void Paint(DrawRect rect, Graphics graf)
        {
            graf.FillRectangle(new SolidBrush(color), rect.Rect);
        }
    }

    public class PainterDrawRectangle : RectPainter
    {
        private Color color;
        private int width;
        public PainterDrawRectangle(Color color, int width, string name)
        {
            this.color = color;
            this.width = width;
            Name = name;
        }
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                DoChange();
            }
        }
        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                DoChange();
            }
        }

        public override void Paint(DrawRect rect, Graphics graf)
        {
            if (width <= 0) return;
            int h = width / 2;
            int h2 = (2 * h == width) ? 2 * h : 2 * h + 1;
            var drawRect = new Rectangle(rect.Rect.Left + h, rect.Rect.Top + h, rect.Rect.Width - h2, rect.Rect.Height - h2);
            graf.DrawRectangle(new Pen(color, width), drawRect);
        }
    }

    public static class Painters
    {
        public static void FillRect(this RectPainters.PaintLevel paintLevel, Color color, string name)
        {
            if (paintLevel != null)
                paintLevel.Add(new PainterFillRect(color, name));
        }

        public static void FillRect(this RectPainters.PaintLevel paintLevel, Color color)
        {
            paintLevel.FillRect(color, "");
        }

        public static void FillRectGradient(this RectPainters.PaintLevel paintLevel, SimpleDirection direction, Color color1, Color color2, string name)
        {
            if (paintLevel != null)
                paintLevel.Add(new PainterFillRectGradient(direction, color1, color2, name));
        }

        public static void FillRectGradient(this RectPainters.PaintLevel paintLevel, SimpleDirection direction, Color color1, Color color2)
        {
            paintLevel.FillRectGradient(direction, color1, color2, "");
        }

        public static void DrawRectangle(this RectPainters.PaintLevel paintLevel, Color color, int width, string name)
        {
            if (paintLevel != null)
                paintLevel.Add(new PainterDrawRectangle(color, width, name));
        }

        public static void DrawRectangle(this RectPainters.PaintLevel paintLevel, Color color, int width)
        {
            paintLevel.DrawRectangle(color, width, "");
        }

        public static void DrawRectangle(this RectPainters.PaintLevel paintLevel, Color color)
        {
            paintLevel.DrawRectangle(color, 1);
        }
    }
}

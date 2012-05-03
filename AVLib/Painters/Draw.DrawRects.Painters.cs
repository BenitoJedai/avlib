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

    public struct RectSides
    {
        public bool left;
        public bool right;
        public bool top;
        public bool bottom;

        public bool Full
        {
            get { return left && right && top && bottom; }
            set
            {
                left = value;
                right = value;
                top = value;
                bottom = value;
            }
        }
    }

    public static class DrawPath
    {
        public static GraphicsPath RoundCorner(Rectangle rect, int radius, RectSides sides)
        {
            GraphicsPath gfxPath = new GraphicsPath();
            if (sides.left)
            {
                gfxPath.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
                gfxPath.AddLine(rect.X, rect.Y + radius, rect.X + radius, rect.Y);
            }
            if (sides.top)
            {
                gfxPath.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
                gfxPath.AddLine(rect.Right - radius, rect.Y, rect.Right, rect.Y + radius);
            }
            if (sides.right)
            {
                gfxPath.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
                gfxPath.AddLine(rect.Right, rect.Bottom - radius, rect.Right - radius, rect.Bottom);
            }
            if (sides.bottom)
            {
                gfxPath.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
                gfxPath.AddLine(rect.X + radius, rect.Bottom, rect.X, rect.Bottom - radius);
            }
            return gfxPath;
        }

        public static GraphicsPath RoundCorner(Rectangle rect, int radius)
        {
            return RoundCorner(rect, radius, new RectSides() {left = true, right = true, top = true, bottom = true});
        }

        public static GraphicsPath RectSides(Rectangle rect, RectSides sides)
        {
            GraphicsPath gfxPath = new GraphicsPath();
            if(sides.left)
                gfxPath.AddLine(rect.X, rect.Y + rect.Height, rect.X, rect.Y);
            if (sides.top)
                gfxPath.AddLine(rect.X, rect.Y, rect.X + rect.Width, rect.Y);
            if (sides.right)
                gfxPath.AddLine(rect.X + rect.Width, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
            if (sides.bottom)
                gfxPath.AddLine(rect.X + rect.Width, rect.Y + rect.Height, rect.X, rect.Y + rect.Height);
            return gfxPath;
        }
    }

    public static class DrawRectMethods
    {
        public static void DrawRectangle(Rectangle rect, Graphics graf, Color color, int width, int cornerRadius, RectSides sides)
        {
            if (width <= 0) return;
            int h = width / 2;
            int h2 = (2 * h == width) ? 2 * h : 2 * h + 1;
            var drawRect = new Rectangle(rect.Left + h, rect.Top + h, rect.Width - h2, rect.Height - h2);
            if (cornerRadius <= 0 && sides.Full)
            {
                graf.DrawRectangle(new Pen(color, width), drawRect);
            }
            else
            {
                GraphicsPath gfxPath;
                if (cornerRadius <= 0)
                    gfxPath = DrawPath.RectSides(drawRect, sides);
                else
                {
                    gfxPath = DrawPath.RoundCorner(drawRect, cornerRadius, sides);
                }
                graf.DrawPath(new Pen(color, width), gfxPath);
            }
        }

        public static void DrawRectangle(Rectangle rect, Graphics graf, Color color, int width, int cornerRadius)
        {
            DrawRectangle(rect, graf, color, width, cornerRadius, new RectSides(){Full = true});
        }

        public static void DrawRectangle(Rectangle rect, Graphics graf, Color color, int width, RectSides sides)
        {
            DrawRectangle(rect, graf, color, width, 0, sides);
        }

        public static void DrawRectangle(Rectangle rect, Graphics graf, Color color, int width)
        {
            DrawRectangle(rect, graf, color, width, 0, new RectSides(){Full = true});
        }

        public static void FillRectGradient(Rectangle rect, Graphics graf, Color color1, Color color2, SimpleDirection direction, int cornerRadius)
        {
            Point p1;
            switch (direction)
            {
                case SimpleDirection.Vertical:
                    p1 = new Point(rect.X + rect.Width/2, rect.Y);
                    break;
                case SimpleDirection.Diagonal2:
                    p1 = new Point(rect.Right - cornerRadius, rect.Y + cornerRadius);
                    break;
                case SimpleDirection.Diagonal:
                    p1 = new Point(rect.X + cornerRadius, rect.Y + cornerRadius);
                    break;
                default:
                    p1 = new Point(rect.X, rect.Y + rect.Width/2);
                    break;
            }

            Point p2;
            switch (direction)
            {
                case SimpleDirection.Vertical:
                    p2 = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height);
                    break;
                case SimpleDirection.Diagonal2:
                    p2 = new Point(rect.X + cornerRadius, rect.Bottom - cornerRadius);
                    break;
                case SimpleDirection.Diagonal:
                    p2 = new Point(rect.Right - cornerRadius, rect.Bottom - cornerRadius);
                    break;
                default:
                    p2 = new Point(rect.Right, rect.Y + rect.Width/2);
                    break;
            }

            if (cornerRadius <= 0)
            {
                if (p1 != p2)
                    graf.FillRectangle(new LinearGradientBrush(p1, p2, color1, color2), rect);
                else
                    graf.FillRectangle(new SolidBrush(color1), rect);
            }
            else
            {
                GraphicsPath gfxPath = DrawPath.RoundCorner(rect, cornerRadius);
                gfxPath.CloseAllFigures();

                if (p1 != p2)
                    graf.FillPath(new LinearGradientBrush(p1, p2, color1, color2), gfxPath);
                else
                    graf.FillPath(new SolidBrush(color1), gfxPath);
            }
        }

        public static void FillRectGradient(Rectangle rect, Graphics graf, Color color1, Color color2, SimpleDirection direction)
        {
            FillRectGradient(rect, graf, color1, color2, direction, 0);
        }

        public static void FillRect(Rectangle rect, Graphics graf, Color color, int cornerRadius)
        {
            if (cornerRadius <= 0)
                graf.FillRectangle(new SolidBrush(color), rect);
            else
            {
                GraphicsPath gfxPath = DrawPath.RoundCorner(rect, cornerRadius);
                graf.FillPath(new SolidBrush(color), gfxPath);
            }
        }

        public static void FillRect(Rectangle rect, Graphics graf, Color color)
        {
            FillRect(rect, graf, color, 0);
        }
    }

    public class PainterFillRectGradient : RectPainter
    {
        private SimpleDirection direction;
        private Color color1;
        private Color color2;
        private int cornerRadius;
        public PainterFillRectGradient(SimpleDirection direction, Color color1, Color color2, int cornerRadius, string name)
        {
            this.direction = direction;
            this.color1 = color1;
            this.color2 = color2;
            this.cornerRadius = cornerRadius;
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
        public int CornerRadius
        {
            get { return cornerRadius; }
            set
            {
                if (cornerRadius != value)
                {
                    cornerRadius = value;
                    DoChange();
                }
            }
        }

        public override void Paint(DrawRect rect, Graphics graf)
        {
            DrawRectMethods.FillRectGradient(rect.Rect, graf, color1, color2, direction, cornerRadius);
        }
    }

    public class PainterFillRect : RectPainter
    {
        private Color color;
        private int cornerRadius;
        public PainterFillRect(Color color, int cornerRadius, string name)
        {
            this.color = color;
            this.cornerRadius = cornerRadius;
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
        public int CornerRadius
        {
            get { return cornerRadius; }
            set
            {
                if (cornerRadius != value)
                {
                    cornerRadius = value;
                    DoChange();
                }
            }
        }

        public override void Paint(DrawRect rect, Graphics graf)
        {
            DrawRectMethods.FillRect(rect.Rect, graf, color, cornerRadius);
        }
    }

    public class PainterDrawRectangle : RectPainter
    {
        private Color color;
        private int width;
        private int cornerRadius;
        private RectSides sides;
        public PainterDrawRectangle(Color color, int width, RectSides sides, int cornerRadius, string name)
        {
            this.color = color;
            this.width = width;
            this.cornerRadius = cornerRadius;
            this.sides = sides;
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
        public int CornerRadius
        {
            get { return cornerRadius; }
            set
            {
                if(cornerRadius != value)
                {
                    cornerRadius = value;
                    DoChange();
                }
            }
        }

        public override void Paint(DrawRect rect, Graphics graf)
        {
            DrawRectMethods.DrawRectangle(rect.Rect, graf, color, width, cornerRadius, sides);
        }
    }

    public static class Painters
    {
        public static void FillRect(this RectPainters.PaintLevel paintLevel, Color color, int cornerRadius, string name)
        {
            if (paintLevel != null)
                paintLevel.Add(new PainterFillRect(color, cornerRadius, name));
        }

        public static void FillRect(this RectPainters.PaintLevel paintLevel, Color color, string name)
        {
            FillRect(paintLevel, color, 0, name);
        }

        public static void FillRect(this RectPainters.PaintLevel paintLevel, Color color)
        {
            paintLevel.FillRect(color, "");
        }

        public static void FillRectGradient(this RectPainters.PaintLevel paintLevel, SimpleDirection direction, Color color1, Color color2, int cornerRadius, string name)
        {
            if (paintLevel != null)
                paintLevel.Add(new PainterFillRectGradient(direction, color1, color2, cornerRadius, name));
        }

        public static void FillRectGradient(this RectPainters.PaintLevel paintLevel, SimpleDirection direction, Color color1, Color color2, string name)
        {
            FillRectGradient(paintLevel, direction, color1, color2, 0, name);
        }

        public static void FillRectGradient(this RectPainters.PaintLevel paintLevel, SimpleDirection direction, Color color1, Color color2)
        {
            paintLevel.FillRectGradient(direction, color1, color2, "");
        }

        public static void DrawRectangle(this RectPainters.PaintLevel paintLevel, Color color, int width, RectSides sides, int cornerRadius, string name)
        {
            if (paintLevel != null)
                paintLevel.Add(new PainterDrawRectangle(color, width, sides, cornerRadius, name));
        }

        public static void DrawRectangle(this RectPainters.PaintLevel paintLevel, Color color, int width, RectSides sides, string name)
        {
            DrawRectangle(paintLevel, color, width, sides, 0, name);
        }

        public static void DrawRectangle(this RectPainters.PaintLevel paintLevel, Color color, int width, int cornerRadius, string name)
        {
            DrawRectangle(paintLevel, color, width, new RectSides() {Full = true}, cornerRadius, name);
        }

        public static void DrawRectangle(this RectPainters.PaintLevel paintLevel, Color color, int width, string name)
        {
            DrawRectangle(paintLevel, color, width, 0, name);
        }

        public static void DrawRectangle(this RectPainters.PaintLevel paintLevel, Color color, int width)
        {
            paintLevel.DrawRectangle(color, width, "");
        }

        public static void DrawRectangle(this RectPainters.PaintLevel paintLevel, Color color)
        {
            paintLevel.DrawRectangle(color, 1);
        }

        public static void DrawCustom(this RectPainters.PaintLevel paintLevel, PainterPaintHandler paintHandler, string name)
        {
            if (paintLevel != null)
                paintLevel.Add(new CustomPainter(paintHandler, name));
        }

        public static void DrawCustom(this RectPainters.PaintLevel paintLevel, PainterPaintHandler paintHandler)
        {
            if (paintLevel != null)
                paintLevel.Add(new CustomPainter(paintHandler, ""));
        }
    }
}

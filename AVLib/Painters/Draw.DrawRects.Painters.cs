using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using AVLib.Utils;
using VALib.Draw.Controls;

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

    public static class BasePainters
    {
        public static void Rect(IControlPropertiesValue Params, DrawRect rect, Graphics graf)
        {
            DrawRectMethods.DrawRectangle(Params["Rect", rect.Rect].As<Rectangle>(), graf, Params["Color"].AsColor(),
                                          Params["Width", rect.BorderSize].AsInteger(),
                                          Params["CornerRadius"].AsInteger(), Params["Sides", () => { return new RectSides() { Full = true }; }].As<RectSides>());
        }

        public static void FillRect(IControlPropertiesValue Params, DrawRect rect, Graphics graf)
        {
            DrawRectMethods.FillRect(Params["Rect", rect.Rect].As<Rectangle>(), graf, Params["Color"].AsColor(), Params["CornerRadius"].AsInteger());
        }

        public static void FillGradient(IControlPropertiesValue Params, DrawRect rect, Graphics graf)
        {
            DrawRectMethods.FillRectGradient(Params["Rect", rect.Rect].As<Rectangle>(), graf, Params["Color"].AsColor(), Params["Color2"].AsColor(),
                                                 Params["Direction", SimpleDirection.Vertical].As<SimpleDirection>(),
                                                 Params["CornerRadius"].AsInteger());
        }

        public static void Text(IControlPropertiesValue Params, DrawRect rect, Graphics graf)
        {
            var text = Params["Text"].AsString();
            if (text == "") return;
            var drawRect = Params["Rect", rect.Rect].As<Rectangle>();
            var font = Params["Font", () => { return new Font("Arial", 8); }].As<Font>();
            var alignment = Params["Alignment", StringAlignment.Near].As<StringAlignment>();
            var vertAlignment = Params["VertAlignment", StringAlignment.Center].As<StringAlignment>();

            var sz = graf.MeasureString(text, font);
            var dx = (int)(drawRect.Width - sz.Width);
            var dy = (int)(drawRect.Height - sz.Height);

            var offset = Params["Offset", () => { return new Point(0, 0); }].As<Point>();
            var pt = new Point(0, 0);
            switch (alignment)
            {
                case StringAlignment.Near:
                    pt.X = 0;
                    break;
                case StringAlignment.Far:
                    pt.X = dx;
                    break;
                case StringAlignment.Center:
                    pt.X = dx / 2;
                    break;
            }
            switch (vertAlignment)
            {
                case StringAlignment.Near:
                    pt.Y = 0;
                    break;
                case StringAlignment.Far:
                    pt.Y = dy;
                    break;
                case StringAlignment.Center:
                    pt.Y = dy / 2;
                    break;
            }
            pt.Offset(offset);

            graf.DrawString(text, font, new SolidBrush(Params["Color"].AsColor()), drawRect.X + pt.X, rect.Rect.Y + pt.Y);
        }
    }
}

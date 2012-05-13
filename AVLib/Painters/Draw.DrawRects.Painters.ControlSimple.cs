using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using AVLib.Utils;
using VALib.Draw.Controls;

namespace AVLib.Draw.DrawRects.Painters.ControlSimple
{
    public static class ControlSimpleDraw
    {
        public static void ButtonBorder(IControlPropertiesValue Params, DrawRect rect, Graphics graf)
        {
            var drawRect = rect.Rect;

            RectSides darkSides, brightSides;
            if (Params["Down"].AsBoolean())
            {
                brightSides = new RectSides() { right = true, bottom = true };
                darkSides = new RectSides() { left = true, top = true };
            }
            else
            {
                darkSides = new RectSides() { right = true, bottom = true };
                brightSides = new RectSides() { left = true, top = true };
            }
            DrawRectMethods.DrawRectangle(drawRect, graf, Params["Color"].AsColor().DarkColor(20), rect.BorderSize, Params["CornerRadius"].AsInteger(), darkSides);
            DrawRectMethods.DrawRectangle(drawRect, graf, Params["Color"].AsColor().BrightColor(20), rect.BorderSize, Params["CornerRadius"].AsInteger(), brightSides);
            DrawRectMethods.DrawRectangle(drawRect, graf, Params["Color"].AsColor().DarkColor(40), 1, Params["CornerRadius"].AsInteger(), darkSides);
            DrawRectMethods.DrawRectangle(drawRect, graf, Params["Color"].AsColor().DarkColor(10), 1, Params["CornerRadius"].AsInteger(), brightSides);
        }

        public static void Body(IControlPropertiesValue Params, DrawRect rect, Graphics graf)
        {
            if (Params["Gradient"].AsBoolean())
                BasePainters.FillGradient(Params, rect, graf);
            else
                BasePainters.FillRect(Params, rect, graf);
        }

        public static void ScrollArrowUp(IControlPropertiesValue Params, DrawRect rect, Graphics graf)
        {
            var drawRect = Params["Rect", rect.Rect].As<Rectangle>();
            var center = new Point(drawRect.Left + drawRect.Width/2, drawRect.Top + drawRect.Height/2);

            var arrowUpPoints = new Point[]
                                    {
                                        new Point(center.X - 5, center.Y + 2),
                                        new Point(center.X, center.Y - 3),
                                        new Point(center.X + 5, center.Y + 2)
                                    };

            graf.FillPolygon(new LinearGradientBrush(new Point(center.X - 2, center.Y - 5), arrowUpPoints[2],
                                                     Params["Color", SystemColors.Control].AsColor().DarkColor(60),
                                                     Params["Color", SystemColors.Control].AsColor().BrightColor(10)),
                             arrowUpPoints);
        }

        public static void ScrollArrowDown(IControlPropertiesValue Params, DrawRect rect, Graphics graf)
        {
            var drawRect = Params["Rect", rect.Rect].As<Rectangle>();
            var center = new Point(drawRect.Left + drawRect.Width / 2, drawRect.Top + drawRect.Height / 2);

            var arrowUpPoints = new Point[]
                                    {
                                        new Point(center.X - 5, center.Y - 2),
                                        new Point(center.X + 5, center.Y - 2),
                                        new Point(center.X, center.Y + 3)
                                    };

            graf.FillPolygon(new LinearGradientBrush(new Point(center.X - 6, center.Y - 3), arrowUpPoints[2],
                                                     Params["Color", SystemColors.Control].AsColor().DarkColor(60),
                                                     Params["Color", SystemColors.Control].AsColor().BrightColor(10)),
                             arrowUpPoints);
        }

        public static void ScrollArrowLeft(IControlPropertiesValue Params, DrawRect rect, Graphics graf)
        {
            var drawRect = Params["Rect", rect.Rect].As<Rectangle>();
            var center = new Point(drawRect.Left + drawRect.Width / 2, drawRect.Top + drawRect.Height / 2);

            var arrowUpPoints = new Point[]
                                    {
                                        new Point(center.X - 4, center.Y),
                                        new Point(center.X + 1, center.Y - 5),
                                        new Point(center.X + 1, center.Y + 5)
                                    };

            graf.FillPolygon(new LinearGradientBrush(new Point(center.X - 5, center.Y - 1), arrowUpPoints[2],
                                                     Params["Color", SystemColors.Control].AsColor().DarkColor(60),
                                                     Params["Color", SystemColors.Control].AsColor().BrightColor(10)),
                             arrowUpPoints);
        }

        public static void ScrollArrowRight(IControlPropertiesValue Params, DrawRect rect, Graphics graf)
        {
            var drawRect = Params["Rect", rect.Rect].As<Rectangle>();
            var center = new Point(drawRect.Left + drawRect.Width / 2, drawRect.Top + drawRect.Height / 2);

            var arrowUpPoints = new Point[]
                                    {
                                        new Point(center.X - 1, center.Y - 5),
                                        new Point(center.X + 4, center.Y),
                                        new Point(center.X - 1, center.Y + 5)
                                    };

            graf.FillPolygon(new LinearGradientBrush(arrowUpPoints[0], new Point(center.X + 5, center.Y + 1),
                                                     Params["Color", SystemColors.Control].AsColor().DarkColor(60),
                                                     Params["Color", SystemColors.Control].AsColor().BrightColor(10)),
                             arrowUpPoints);
        }

        public static void ScrollButtonVerticalBar(IControlPropertiesValue Params, DrawRect rect, Graphics graf)
        {
            var drawRect = Params["Rect", rect.Rect].As<Rectangle>();
            if (drawRect.Height < 16) return;
            var center = new Point(drawRect.Left + drawRect.Width / 2, drawRect.Top + drawRect.Height / 2);

            int sz = (drawRect.Height < 36) ? (drawRect.Height - 16)/2 : 10;
            var arrowUpPoints = new Point[]
                                    {
                                        new Point(center.X - 2, center.Y - sz),
                                        new Point(center.X, center.Y - sz - 2),
                                        new Point(center.X + 2, center.Y - sz),
                                        new Point(center.X + 2 , center.Y + sz),
                                        new Point(center.X, center.Y + sz + 2),
                                        new Point(center.X - 2, center.Y + sz) 
                                    };
            graf.FillPolygon(new LinearGradientBrush(arrowUpPoints[0], arrowUpPoints[2],
                                                     Params["Color", SystemColors.Control].AsColor().DarkColor(20),
                                                     Params["Color", SystemColors.Control].AsColor().BrightColor(20)),
                             arrowUpPoints);
        }

        public static void ScrollButtonHorisontalBar(IControlPropertiesValue Params, DrawRect rect, Graphics graf)
        {
            var drawRect = Params["Rect", rect.Rect].As<Rectangle>();
            if (drawRect.Width < 16) return;
            var center = new Point(drawRect.Left + drawRect.Width / 2, drawRect.Top + drawRect.Height / 2);

            int sz = (drawRect.Width < 36) ? (drawRect.Width - 16) / 2 : 10;
            var arrowUpPoints = new Point[]
                                    {
                                        new Point(center.X - sz, center.Y + 2),
                                        new Point(center.X - sz - 2, center.Y),
                                        new Point(center.X - sz, center.Y - 3),
                                        new Point(center.X + sz , center.Y - 2),
                                        new Point(center.X + sz + 2, center.Y),
                                        new Point(center.X + sz, center.Y + 2) 
                                    };
            graf.FillPolygon(new LinearGradientBrush(arrowUpPoints[2], arrowUpPoints[0],
                                                     Params["Color", SystemColors.Control].AsColor().DarkColor(20),
                                                     Params["Color", SystemColors.Control].AsColor().BrightColor(20)),
                             arrowUpPoints);
        }
    }
}

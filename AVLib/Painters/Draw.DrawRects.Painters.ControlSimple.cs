using System;
using System.Collections.Generic;
using System.Drawing;
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

        
    }
}

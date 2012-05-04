using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AVLib.Draw.DrawRects;
using AVLib.Utils;

namespace VALib.Draw.Controls.Core
{
    public class BaseProperties : ControlRect
    {
        public Color Color
        {
            get { return Value["Color"].AsColor(); }
            set { Value["Color"] = value; }
        }

        public Color MouseOverColor
        {
            get { return Value["MouseOverColor"].AsColor(); }
            set { Value["MouseOverColor"] = value; }
        }

        public Color CurrentColor
        {
            get { return Value["CurrentColor"].AsColor(); }
            set { Value["CurrentColor"] = value; }
        }

        public Color FocusColor
        {
            get { return Value["FocusColor"].AsColor(); }
            set { Value["FocusColor"] = value; }
        }

        public bool DrawFocus
        {
            get { return Value["DrawFocus"].AsBoolean(); }
            set { Value["DrawFocus"] = value; }
        }

        public bool MouseIsOver
        {
            get { return Value["MouseIsOver"].AsBoolean(); }
            internal set { Value["MouseIsOver"] = value; }
        }

        public bool MouseIsDown
        {
            get { return Value["MouseIsDown"].AsBoolean(); }
            internal set { Value["MouseIsDown"] = value; }
        }

        public bool UseMouseOverColor
        {
            get { return Value["UseMouseOverColor"].AsBoolean(); }
            set { Value["UseMouseOverColor"] = value; }
        }

        public bool UseMouseOverColorAnime
        {
            get { return Value["UseMouseOverColorAnime"].AsBoolean(); }
            set { Value["UseMouseOverColorAnime"] = value; }
        }

        public int MouseOverColorAnimeLeaveTime
        {
            get { return Value["MouseOverColorAnimeLeaveTime"].AsInteger(); }
            set { Value["MouseOverColorAnimeLeaveTime"] = value; }
        }

        public int MouseOverColorAnimeEnterTime
        {
            get { return Value["MouseOverColorAnimeEnterTime"].AsInteger(); }
            set { Value["MouseOverColorAnimeEnterTime"] = value; }
        }

        public bool CaptureMouseClick
        {
            get { return Value["CaptureMouseClick", false].AsBoolean(); }
            set { Value["CaptureMouseClick"] = value; }
        }

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

        public Font Font
        {
            get { return Value["Font"].As<Font>(); }
            set { Value["Font"] = value; }
        }

        public StringAlignment TextAlignment
        {
            get { return Value["TextAlignment", StringAlignment.Near].As<StringAlignment>(); }
            set { Value["TextAlignment"] = value; }
        }

        public StringAlignment TextVertAlignment
        {
            get { return Value["TextVertAlignment", StringAlignment.Center].As<StringAlignment>(); }
            set { Value["TextVertAlignment"] = value; }
        }
    }
}

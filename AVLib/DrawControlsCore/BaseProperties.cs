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
            get { return Property["Color"].AsColor(); }
            set { Property.SetProperty("Color", value); }
        }

        public Color MouseOverColor
        {
            get { return Property["MouseOverColor"].AsColor(); }
            set { Property.SetProperty("MouseOverColor", value); }
        }

        public Color CurrentColor
        {
            get { return Property["CurrentColor"].AsColor(); }
            set { Property.SetProperty("CurrentColor", value); }
        }

        public Color FocusColor
        {
            get { return Property["FocusColor"].AsColor(); }
            set { Property.SetProperty("FocusColor", value); }
        }

        public bool DrawFocus
        {
            get { return Property["DrawFocus"].AsBoolean(); }
            set { Property.SetProperty("DrawFocus", value); }
        }

        public bool MouseIsOver
        {
            get { return Property["MouseIsOver"].AsBoolean(); }
            internal set { Property.SetProperty("MouseIsOver", value); }
        }

        public bool MouseIsDown
        {
            get { return Property["MouseIsDown"].AsBoolean(); }
            internal set { Property.SetProperty("MouseIsDown", value); }
        }

        public bool UseMouseOverColor
        {
            get { return Property["UseMouseOverColor"].AsBoolean(); }
            set { Property.SetProperty("UseMouseOverColor", value); }
        }

        public bool UseMouseOverColorAnime
        {
            get { return Property["UseMouseOverColorAnime"].AsBoolean(); }
            set { Property.SetProperty("UseMouseOverColorAnime", value); }
        }

        public int MouseOverColorAnimeLeaveTime
        {
            get { return Property["MouseOverColorAnimeLeaveTime"].AsInteger(); }
            set { Property.SetProperty("MouseOverColorAnimeLeaveTime", value); }
        }

        public int MouseOverColorAnimeEnterTime
        {
            get { return Property["MouseOverColorAnimeEnterTime"].AsInteger(); }
            set { Property.SetProperty("MouseOverColorAnimeEnterTime", value); }
        }

        public bool CaptureMouseClick
        {
            get { return Property["CaptureMouseClick", false].AsBoolean(); }
            set { Property.SetProperty("CaptureMouseClick", value); }
        }
    }
}

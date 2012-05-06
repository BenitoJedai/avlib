using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AVLib.Animations;
using AVLib.Draw.DrawRects;
using AVLib.Utils;

namespace VALib.Draw.Controls.Core
{
    public class BaseControl : ControlRect
    {

        #region BaseProperties

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

        #endregion

        #region BaseEventReaction

        public event EventHandler Click;

        protected override void InitializeControl()
        {
            base.InitializeControl();
            InitProperties();

            this.MouseEnter += DrawMouseReaction_MouseEnter;
            this.MouseLeave += DrawMouseReaction_MouseLeave;
            this.MouseDown += DrawMouseReaction_MouseDown;
            this.MouseUp += DrawMouseReaction_MouseUp;
        }

        public void InitProperties()
        {
            Property["Color", SystemColors.Control]
                .AnyChanged += () => { if (!UseMouseOverColor || !MouseIsOver) CurrentColor = Color; };

            Property["MouseOverColor", SystemColors.Control.BrightColor(30)]
                .AnyChanged += () => { if (UseMouseOverColor && MouseIsOver) CurrentColor = MouseOverColor; };

            Property["CurrentColor", SystemColors.Control]
                .Changed += () => { Invalidate(); };

            Property["FocusColor", Color.Blue]
                .Changed += () => { if (Focused) Invalidate(); };

            Property["DrawFocus", false]
                .Changed += () => { if (Focused) Invalidate(); };

            Property["MouseIsOver", false]
                .Changed += () => { Invalidate(); };

            Property["UseMouseOverColor", false]
                .Changed += () => { if (MouseIsOver) Invalidate(); };

            MouseOverColorAnimeLeaveTime = 500;
            MouseOverColorAnimeEnterTime = 200;

            Property["CaptureMouseClick", false]
                .Changed += () =>
                {
                    if (!CaptureMouseClick && MouseIsDown)
                    {
                        Capture(this, false);
                        if (!MouseIsOver) MouseIsDown = false;
                    }
                };
        }

        public void ChangeCurrentColor(Color color, int animeTime)
        {
            if (CurrentColor != color)
            {
                if (animeTime > 0)
                {
                    this.AnimeCancel("color");
                    this.Anime("color", animeTime).Change("CurrentColor", color);
                }
                else
                    CurrentColor = color;
            }
        }

        #region Handle mouse events

        private void DrawMouseReaction_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseIsDown = true;
                if (CaptureMouseClick) Capture(this, true);
            }
        }

        private void DrawMouseReaction_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MouseIsDown)
                {
                    MouseIsDown = false;
                    if (CaptureMouseClick) Capture(this, false);
                    if (UseMouseOverColor && !MouseIsOver) ChangeCurrentColor(Color, MouseOverColorAnimeLeaveTime);
                    if (MouseIsOver) DoClick();
                }
            }
        }

        public void DoClick()
        {
            if (Click != null) Click(this, new EventArgs());
        }

        private void DrawMouseReaction_MouseLeave(object sender, EventArgs e)
        {
            MouseIsOver = false;

            if (!CaptureMouseClick && MouseIsDown) MouseIsDown = false;

            if (UseMouseOverColor && !MouseIsDown) ChangeCurrentColor(Color, MouseOverColorAnimeLeaveTime);
        }

        private void DrawMouseReaction_MouseEnter(object sender, EventArgs e)
        {
            MouseIsOver = true;

            if (UseMouseOverColor) ChangeCurrentColor(MouseOverColor, MouseOverColorAnimeEnterTime);
        }

        #endregion

        #endregion

        #region Behaviours

        private BehaviourList m_behaviours;
        public BehaviourList Behaviours
        {
            get
            {
                if (m_behaviours == null) m_behaviours = new BehaviourList(this);
                return m_behaviours;
            }
        }

        #endregion
    }
}

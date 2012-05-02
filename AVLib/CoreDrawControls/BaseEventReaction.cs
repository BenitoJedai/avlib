using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AVLib.Animations;
using AVLib.Draw.DrawRects;
using AVLib.Utils;

namespace VALib.Draw.Controls.Core
{
    public class BaseEventReaction : BaseProperties
    {
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
            Property.Get("Color", SystemColors.Control)
                .Changed += () => { if (!UseMouseOverColor || !MouseIsOver) CurrentColor = Color; };

            Property.Get("MouseOverColor", SystemColors.Control.BrightColor(30))
                .Changed += () => { if (UseMouseOverColor && MouseIsOver) CurrentColor = MouseOverColor; };

            Property.Get("CurrentColor", SystemColors.Control)
                .Changed += () => { Invalidate(); };

            Property.Get("FocusColor", Color.Blue)
                .Changed += () => { if (Focused) Invalidate(); };

            Property.Get("DrawFocus", false)
                .Changed += () => { if (Focused) Invalidate(); };

            Property.Get("MouseIsOver", false)
                .Changed += () => { Invalidate(); };

            Property.Get("UseMouseOverColor", false)
                .Changed += () => { if (MouseIsOver) Invalidate(); };

            Property.SetProperty("MouseOverColorAnimeLeaveTime", 500);
            Property.SetProperty("MouseOverColorAnimeEnterTime", 200);

            Property.Get("CaptureMouseClick", false)
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

        private void DrawMouseReaction_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MouseIsDown)
                {
                    MouseIsDown = false;
                    if (CaptureMouseClick) Capture(this, false);
                    if (UseMouseOverColor && !MouseIsOver) ChangeCurrentColor(Color, MouseOverColorAnimeLeaveTime);
                }
            }
        }

        private void DrawMouseReaction_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseIsDown = true;
                if (CaptureMouseClick) Capture(this, true);
            }
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
    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AVLib.Draw.DrawRects;
using AVLib.Utils;

namespace AVLib.Animations
{
    public enum Speed
    {
        Slow,
        Medium,
        Fast
    }

    public enum SpeedMode
    {
        Normal,
        Inc,
        Dec
    }

    public static class AnimationUtils
    {
        private class SizeThreadParam : AnimationControler.BaseThreadParam
        {
            public int size;
            public SpeedMode speedMode;
            protected override AnimationControler.BaseThreadParam CreateClon()
            {
                return new SizeThreadParam();
            }
            protected override void CloneTo(AnimationControler.BaseThreadParam clon)
            {
                base.CloneTo(clon);
                ((SizeThreadParam) clon).size = size;
                ((SizeThreadParam) clon).speedMode = speedMode;
            }
        }

        private class HiglightThreadParam : AnimationControler.BaseThreadParam
        {
            public int HiglightPercent;
            protected override AnimationControler.BaseThreadParam CreateClon()
            {
                return new HiglightThreadParam();
            }
            protected override void CloneTo(AnimationControler.BaseThreadParam clon)
            {
                base.CloneTo(clon);
                ((HiglightThreadParam) clon).HiglightPercent = HiglightPercent;
            }
        }

        private class ColorThreadParam : AnimationControler.BaseThreadParam
        {
            public Color color;
            public SpeedMode speedMode;
            protected override AnimationControler.BaseThreadParam CreateClon()
            {
                return new ColorThreadParam();
            }
            protected override void CloneTo(AnimationControler.BaseThreadParam clon)
            {
                base.CloneTo(clon);
                ((ColorThreadParam) clon).color = color;
                ((ColorThreadParam) clon).speedMode = speedMode;
            }
        }

        private delegate void SetObjectPropertyDelegate(object ctrl, string path, object value);
        private static void SetPropertyForObject(object ctrl, string path, object value)
        {
            ctrl.SetProperty(path, value);
        }

        private static void SetObjectProperty(object ctrl, string path, object value)
        {
            if (ctrl is Control)
            {
                ((Control)ctrl).Invoke(new SetObjectPropertyDelegate(SetPropertyForObject), ctrl, path, value);
                return;
            }
            if (ctrl is DrawRect)
            {
                ((DrawRect)ctrl).Invoke(new SetObjectPropertyDelegate(SetPropertyForObject), ctrl, path, value);
                return;
            }
            if (ctrl is IRectPainter)
            {
                ((IRectPainter)ctrl).Invoke(new SetObjectPropertyDelegate(SetPropertyForObject), ctrl, path, value);
                return;
            }
            ctrl.SetProperty(path, value);
        }

        private static void SetIntProperty(object sizeThreadParam)
        {
            SizeThreadParam td = (SizeThreadParam)sizeThreadParam;
            try
            {
                int iterations = AnimationControler.GetIterations(td.time);
                int currHeight = (int)td.control.GetProperty(td.PropertyName);
                int changeSize = td.size - currHeight;
                SizeCalculator cacl = new SizeCalculator(changeSize, td.speedMode, iterations);
                StepProcesor procesor = new StepProcesor(iterations, td.time);
                bool up = changeSize > 0;
                int newHeight = currHeight;

                procesor.Start((d) =>
                {
                    newHeight = currHeight + cacl.NextSize;
                    if (up && newHeight > td.size || !up && newHeight < td.size) newHeight = td.size;
                    if (td.animatorState.Canceled)
                    {
                        d.Cancel = true;
                        return;
                    }
                    SetObjectProperty(td.control, td.PropertyName, newHeight);
                });

                if (newHeight != td.size)
                {
                    try
                    {
                        if (td.animatorState.Canceled) return;
                        SetObjectProperty(td.control, td.PropertyName, td.size);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            finally
            {
                td.controlState.AnimatorEnd(td.animatorState);
            }
        }

        private static void SetColorProperty(object colorThreadParam)
        {
            ColorThreadParam td = (ColorThreadParam)colorThreadParam;
            try
            {
                Color controlColor = (Color)td.control.GetProperty(td.PropertyName);
                int iterations = AnimationControler.GetIterations(td.time);
                SizeCalculator cacl = new SizeCalculator(100, td.speedMode, iterations);
                StepProcesor procesor = new StepProcesor(iterations, td.time);

                Color newColor = controlColor;
                procesor.Start((d) =>
                                   {
                                       if (td.animatorState.Canceled)
                                       {
                                           d.Cancel = true;
                                           return;
                                       }
                                       newColor = controlColor.MergeColor(td.color, cacl.NextSize);
                                       SetObjectProperty(td.control, td.PropertyName, newColor);
                                   });

                if (newColor != td.color)
                {
                    try
                    {
                        if (td.animatorState.Canceled) return;
                        SetObjectProperty(td.control, td.PropertyName, td.color);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            finally
            {
                td.controlState.AnimatorEnd(td.animatorState);
            }
        }

        private static void DoHighlight(object higlightThreadParam)
        {
            HiglightThreadParam td = (HiglightThreadParam)higlightThreadParam;
            Color ctrlColor = (Color)td.control.GetProperty("BackColor");
            try
            {
                SetObjectProperty(td.control, "BackColor", ctrlColor.BrightColor(td.HiglightPercent));
            }
            catch (Exception)
            {
            }
            int iteration = AnimationControler.GetIterations(td.time);
            int t = td.time / iteration;

            for (int i = 0; i < iteration; i++)
            {
                if (td.animatorState.Canceled) break;
                Thread.Sleep(t);
            }

            try
            {
                SetObjectProperty(td.control, "BackColor", ctrlColor);
            }
            catch (Exception)
            {
            }

            td.controlState.AnimatorEnd(td.animatorState);
        }

        private static void DoHighlightForecolor(object higlightThreadParam)
        {
            HiglightThreadParam td = (HiglightThreadParam)higlightThreadParam;
            Color ctrlColor = (Color)td.control.GetProperty("ForeColor");
            try
            {
                SetObjectProperty(td.control, "ForeColor", ctrlColor.BrightColor(td.HiglightPercent));
            }
            catch (Exception)
            {
            }
            int iteration = AnimationControler.GetIterations(td.time);
            int t = td.time / iteration;

            for (int i = 0; i < iteration; i++)
            {
                if (td.animatorState.Canceled) break;
                Thread.Sleep(t);
            }

            try
            {
                SetObjectProperty(td.control, "ForeColor", ctrlColor);
            }
            catch (Exception)
            {
            }

            td.controlState.AnimatorEnd(td.animatorState);
        }

        private static void Wait(object baseThreadParam)
        {
            AnimationControler.BaseThreadParam td = (AnimationControler.BaseThreadParam)baseThreadParam;
            StepProcesor procesor = new StepProcesor(AnimationControler.GetIterations(td.time), td.time);

            procesor.Start((d) => { if (td.animatorState.Canceled) d.Cancel = true; });

            td.controlState.AnimatorEnd(td.animatorState);
        }

        internal static AnimationControler.AnimePacket AnimeWaitPacket(int time, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback)
        {
            AnimationControler.BaseThreadParam baseThreadParam = new AnimationControler.BaseThreadParam();
            baseThreadParam.time = time;
            baseThreadParam.QueueLevel = queueLevel;
            baseThreadParam.finalCallback = finalCallback;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = Wait, threadParam = baseThreadParam };
        }

        public static object AnimeWait(Control ctrl, int time, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeWaitPacket(time, queue, queueLevel, queueOwner, finalCallback));
        }

        internal static AnimationControler.AnimePacket AnimeIntPropPacket(string propName, int height, int time, SpeedMode speedMode, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback)
        {
            SizeThreadParam sizeParam = new SizeThreadParam();
            sizeParam.size = height;
            sizeParam.time = time;
            sizeParam.PropertyName = propName;
            sizeParam.speedMode = speedMode;
            sizeParam.QueueLevel = queueLevel;
            sizeParam.finalCallback = finalCallback;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = SetIntProperty, threadParam = sizeParam };
        }

        public static object AnimeIntProp(object ctrl, string propName, int height, int time, SpeedMode speedMode, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeIntPropPacket(propName, height, time, speedMode, queue, queueLevel, queueOwner, finalCallback));
        }

        internal static AnimationControler.AnimePacket AnimeColorPropPacket(string propName, Color color, int time, SpeedMode speedMode, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback)
        {
            ColorThreadParam colorParam = new ColorThreadParam();
            colorParam.color = color;
            colorParam.time = time;
            colorParam.PropertyName = propName;
            colorParam.speedMode = speedMode;
            colorParam.QueueLevel = queueLevel;
            colorParam.finalCallback = finalCallback;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = SetColorProperty, threadParam = colorParam };
        }

        public static object AnimeColorProp(object ctrl, string propName, Color color, int time, SpeedMode speedMode, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeColorPropPacket(propName, color, time, speedMode, queue, queueLevel, queueOwner, finalCallback));
        }

        internal static AnimationControler.AnimePacket AnimeHighlightPacket(int highlightPercent, int time, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback)
        {
            HiglightThreadParam highlightParam = new HiglightThreadParam();
            highlightParam.HiglightPercent = highlightPercent;
            highlightParam.time = time;
            highlightParam.QueueLevel = queueLevel;
            highlightParam.finalCallback = finalCallback;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = DoHighlight, threadParam = highlightParam };
        }

        public static object AnimeHighlight(Control ctrl, int highlightPercent, int time, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeHighlightPacket(highlightPercent, time, queue, queueLevel, queueOwner, finalCallback));
        }

        internal static AnimationControler.AnimePacket AnimeHighlightForecolorPacket(int highlightPercent, int time, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback)
        {
            HiglightThreadParam highlightParam = new HiglightThreadParam();
            highlightParam.HiglightPercent = highlightPercent;
            highlightParam.time = time;
            highlightParam.QueueLevel = queueLevel;
            highlightParam.finalCallback = finalCallback;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = DoHighlightForecolor, threadParam = highlightParam };
        }

        public static object AnimeHighlightForecolor(Control ctrl, int highlightPercent, int time, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeHighlightForecolorPacket(highlightPercent, time, queue, queueLevel, queueOwner, finalCallback));
        }

        public static void AnimeCancel(object ctrl)
        {
            AnimationControler.Cancel(ctrl);
        }

    }
}
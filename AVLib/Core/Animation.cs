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
        private class IntValueThreadParam : AnimationControler.BaseThreadParam
        {
            public int value;
            public SpeedMode speedMode;
            protected override AnimationControler.BaseThreadParam CreateClon()
            {
                return new IntValueThreadParam();
            }
            protected override void CloneTo(AnimationControler.BaseThreadParam clon)
            {
                base.CloneTo(clon);
                ((IntValueThreadParam)clon).value = value;
                ((IntValueThreadParam)clon).speedMode = speedMode;
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
                ((HiglightThreadParam)clon).HiglightPercent = HiglightPercent;
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
                ((ColorThreadParam)clon).color = color;
                ((ColorThreadParam)clon).speedMode = speedMode;
            }
        }

        private class LocationThreadParam : AnimationControler.BaseThreadParam
        {
            public Point location;
            public SpeedMode speedMode;
            protected override AnimationControler.BaseThreadParam CreateClon()
            {
                return new LocationThreadParam();
            }
            protected override void CloneTo(AnimationControler.BaseThreadParam clon)
            {
                base.CloneTo(clon);
                ((LocationThreadParam)clon).location = location;
                ((LocationThreadParam)clon).speedMode = speedMode;
            }
        }

        private class SizeThreadParam : AnimationControler.BaseThreadParam
        {
            public Size size;
            public SpeedMode speedMode;
            protected override AnimationControler.BaseThreadParam CreateClon()
            {
                return new SizeThreadParam();
            }
            protected override void CloneTo(AnimationControler.BaseThreadParam clon)
            {
                base.CloneTo(clon);
                ((SizeThreadParam)clon).size = size;
                ((SizeThreadParam)clon).speedMode = speedMode;
            }
        }

        private class RectThreadParam : AnimationControler.BaseThreadParam
        {
            public Rectangle rect;
            public SpeedMode speedMode;
            protected override AnimationControler.BaseThreadParam CreateClon()
            {
                return new RectThreadParam();
            }
            protected override void CloneTo(AnimationControler.BaseThreadParam clon)
            {
                base.CloneTo(clon);
                ((RectThreadParam)clon).rect = rect;
                ((RectThreadParam)clon).speedMode = speedMode;
            }
        }

        private class RectArrayThreadParam : AnimationControler.BaseThreadParam
        {
            public Rectangle[] rects;
            public object[] objects;
            public SpeedMode speedMode;
            protected override AnimationControler.BaseThreadParam CreateClon()
            {
                return new RectArrayThreadParam();
            }
            protected override void CloneTo(AnimationControler.BaseThreadParam clon)
            {
                base.CloneTo(clon);
                ((RectArrayThreadParam)clon).rects = (Rectangle[])rects.Clone();
                ((RectArrayThreadParam)clon).objects = (object[])objects.Clone();
                ((RectArrayThreadParam)clon).speedMode = speedMode;
            }
        }

        public delegate void CustomAnimeHandler(AnimationControler.AnimatorState animator);
        private class CustomThreadParam : AnimationControler.BaseThreadParam
        {
            public CustomAnimeHandler customMethod;
            public int MaxIteration;
            protected override AnimationControler.BaseThreadParam CreateClon()
            {
                return new CustomThreadParam();
            }
            protected override void CloneTo(AnimationControler.BaseThreadParam clon)
            {
                base.CloneTo(clon);
                ((CustomThreadParam) clon).customMethod = customMethod;
                ((CustomThreadParam) clon).MaxIteration = MaxIteration;
            }
        }

        private delegate void SetObjectPropertyDelegate(object ctrl, string path, object value);
        private static void SetPropertyForObject(object ctrl, string path, object value)
        {
            ctrl.SetProperty(path, value);
        }

        private delegate void SetObjectPropertyArrayDelegate(object[] objects, string path, object[] values);
        private static void SetPropertiesFroObjects(object[] objects, string path, object[] values)
        {
            for (int i = 0; i < objects.Length; i++)
                objects[i].SetProperty(path, values[i]);
        }

        private static void CustomObjectMethod(object ctrl, AnimationControler.AnimatorState state, CustomAnimeHandler method)
        {
            if (ctrl is Control)
            {
                ((Control)ctrl).Invoke(new CustomAnimeHandler(method), state);
                return;
            }
            if (ctrl is DrawRect)
            {
                ((DrawRect)ctrl).Invoke(new CustomAnimeHandler(method), state);
                return;
            }
            method(state);
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
            ctrl.SetProperty(path, value);
        }

        private static void SetObjectsProperties(object ctrl, object[] objects, string path, object[] values)
        {
            if (ctrl is Control)
            {
                ((Control)ctrl).Invoke(new SetObjectPropertyArrayDelegate(SetPropertiesFroObjects), objects, path, values);
                return;
            }
            if (ctrl is DrawRect)
            {
                ((DrawRect)ctrl).Invoke(new SetObjectPropertyArrayDelegate(SetPropertiesFroObjects), objects, path, values);
                return;
            }
            SetPropertiesFroObjects(objects, path, values);
        }

        private static void SetIntProperty(object intThreadParam)
        {
            IntValueThreadParam td = (IntValueThreadParam)intThreadParam;
            try
            {
                int iterations = AnimationControler.GetIterations(td.time);
                int currHeight = (int)td.control.GetProperty(td.PropertyName);
                int changeSize = td.value - currHeight;
                SizeCalculator cacl = new SizeCalculator(changeSize, td.speedMode, iterations);
                StepProcesor procesor = new StepProcesor(iterations, td.time);
                bool up = changeSize > 0;
                int newHeight = currHeight;
                int prevHeight = currHeight;

                procesor.Start((d) =>
                {
                    newHeight = currHeight + cacl.NextSize;
                    if (up && newHeight > td.value || !up && newHeight < td.value) newHeight = td.value;
                    if (td.animatorState.Canceled)
                    {
                        d.Cancel = true;
                        return;
                    }
                    if (newHeight != prevHeight)
                        SetObjectProperty(td.control, td.PropertyName, newHeight);
                    prevHeight = newHeight;
                });

                if (newHeight != td.value)
                {
                    try
                    {
                        if (td.animatorState.Canceled && !td.CompleteIfCancel) return;
                        SetObjectProperty(td.control, td.PropertyName, td.value);
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

        private static void SetPointProperty(object pointThreadParam)
        {
            LocationThreadParam td = (LocationThreadParam)pointThreadParam;
            try
            {
                int iterations = AnimationControler.GetIterations(td.time);
                Point currPos = (Point)td.control.GetProperty(td.PropertyName);
                int changeX = td.location.X - currPos.X;
                int changeY = td.location.Y - currPos.Y;
                SizeCalculator calcX = new SizeCalculator(changeX, td.speedMode, iterations);
                SizeCalculator calcY = new SizeCalculator(changeY, td.speedMode, iterations);
                StepProcesor procesor = new StepProcesor(iterations, td.time);
                Point newPos = currPos;
                Point prevPos = currPos;

                procesor.Start((d) =>
                {
                    newPos = new Point(currPos.X + calcX.NextSize, currPos.Y + calcY.NextSize);
                    if (td.animatorState.Canceled)
                    {
                        d.Cancel = true;
                        return;
                    }
                    if (newPos != prevPos)
                        SetObjectProperty(td.control, td.PropertyName, newPos);
                    prevPos = newPos;
                });

                if (newPos != td.location)
                {
                    try
                    {
                        if (td.animatorState.Canceled && !td.CompleteIfCancel) return;
                        SetObjectProperty(td.control, td.PropertyName, td.location);
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

        private static void SetSizeProperty(object sizeThreadParam)
        {
            SizeThreadParam td = (SizeThreadParam)sizeThreadParam;
            try
            {
                int iterations = AnimationControler.GetIterations(td.time);
                Size currSize = (Size)td.control.GetProperty(td.PropertyName);
                int changeX = td.size.Width - currSize.Width;
                int changeY = td.size.Height - currSize.Height;
                SizeCalculator calcX = new SizeCalculator(changeX, td.speedMode, iterations);
                SizeCalculator calcY = new SizeCalculator(changeY, td.speedMode, iterations);
                StepProcesor procesor = new StepProcesor(iterations, td.time);
                Size newSize = currSize;
                Size prevSize = currSize;

                procesor.Start((d) =>
                {
                    newSize = new Size(currSize.Width + calcX.NextSize, currSize.Height + calcY.NextSize);
                    if (td.animatorState.Canceled)
                    {
                        d.Cancel = true;
                        return;
                    }
                    if (newSize != prevSize)
                        SetObjectProperty(td.control, td.PropertyName, newSize);
                    prevSize = newSize;
                });

                if (newSize != td.size)
                {
                    try
                    {
                        if (td.animatorState.Canceled && !td.CompleteIfCancel) return;
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

        private static void SetRectProperty(object rectThreadParam)
        {
            RectThreadParam td = (RectThreadParam)rectThreadParam;
            try
            {
                int iterations = AnimationControler.GetIterations(td.time);
                Rectangle currRect = (Rectangle)td.control.GetProperty(td.PropertyName);
                int changeX = td.rect.X - currRect.X;
                int changeY = td.rect.Y - currRect.Y;
                int changeW = td.rect.Width - currRect.Width;
                int changeH = td.rect.Height - currRect.Height;
                SizeCalculator calcX = new SizeCalculator(changeX, td.speedMode, iterations);
                SizeCalculator calcY = new SizeCalculator(changeY, td.speedMode, iterations);
                SizeCalculator calcW = new SizeCalculator(changeW, td.speedMode, iterations);
                SizeCalculator calcH = new SizeCalculator(changeH, td.speedMode, iterations);
                StepProcesor procesor = new StepProcesor(iterations, td.time);
                Rectangle newRect = currRect;
                Rectangle prevRect = currRect;

                procesor.Start((d) =>
                                   {
                                       newRect = new Rectangle(currRect.X + calcX.NextSize, currRect.Y + calcY.NextSize, currRect.Width + calcW.NextSize, currRect.Height + calcH.NextSize);
                                       if (td.animatorState.Canceled)
                                       {
                                           d.Cancel = true;
                                           return;
                                       }
                                       if (newRect != prevRect)
                                           SetObjectProperty(td.control, td.PropertyName, newRect);
                                       prevRect = newRect;
                                   });

                if (newRect != td.rect)
                {
                    try
                    {
                        if (td.animatorState.Canceled && !td.CompleteIfCancel) return;
                        SetObjectProperty(td.control, td.PropertyName, td.rect);
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

        private class MultiRectCalcData
        {
            public Rectangle currRect;
            public Rectangle finalRect;
            public int changeX;
            public int changeY;
            public int changeW;
            public int changeH;
            public SizeCalculator calcX;
            public SizeCalculator calcY;
            public SizeCalculator calcW;
            public SizeCalculator calcH;
            public Rectangle newRect;
            public Rectangle prevRect;
            public object obj;
        }

        private static void SetMultiRectProperty(object rectArrayThreadParam)
        {
            RectArrayThreadParam td = (RectArrayThreadParam)rectArrayThreadParam;
            try
            {
                int iterations = AnimationControler.GetIterations(td.time);
                var calcList = new List<MultiRectCalcData>();
                for (int i = 0; i < td.objects.Length; i++)
                {
                    var calc = new MultiRectCalcData();
                    calcList.Add(calc);

                    calc.obj = td.objects[i];
                    calc.currRect = (Rectangle)td.objects[i].GetProperty(td.PropertyName);
                    calc.finalRect = td.rects[i];
                    calc.changeX = calc.finalRect.X - calc.currRect.X;
                    calc.changeY = calc.finalRect.Y - calc.currRect.Y;
                    calc.changeW = calc.finalRect.Width - calc.currRect.Width;
                    calc.changeH = calc.finalRect.Height - calc.currRect.Height;
                    calc.calcX = new SizeCalculator(calc.changeX, td.speedMode, iterations);
                    calc.calcY = new SizeCalculator(calc.changeY, td.speedMode, iterations);
                    calc.calcW = new SizeCalculator(calc.changeW, td.speedMode, iterations);
                    calc.calcH = new SizeCalculator(calc.changeH, td.speedMode, iterations);
                    calc.newRect = calc.currRect;
                    calc.prevRect = calc.currRect;
                }

                StepProcesor procesor = new StepProcesor(iterations, td.time);

                procesor.Start((d) =>
                {
                    List<object> objects = new List<object>();
                    List<object> newRects = new List<object>();
                    for (int i = 0; i < calcList.Count; i++)
                    {
                        var calc = calcList[i];
                        calc.newRect = new Rectangle(calc.currRect.X + calc.calcX.NextSize, calc.currRect.Y + calc.calcY.NextSize, calc.currRect.Width + calc.calcW.NextSize, calc.currRect.Height + calc.calcH.NextSize);
                        if (td.animatorState.Canceled)
                        {
                            d.Cancel = true;
                            return;
                        }
                        if (calc.newRect != calc.prevRect)
                        {
                            objects.Add(calc.obj);
                            newRects.Add(calc.newRect);
                        }
                        //SetObjectProperty(td.control, td.PropertyName, newRect);
                        calc.prevRect = calc.newRect;
                    }
                    if (objects.Count > 0) SetObjectsProperties(td.control, objects.ToArray(), td.PropertyName, newRects.ToArray());
                });

                if (td.animatorState.Canceled && !td.CompleteIfCancel) return;

                foreach (var calc in calcList)
                {
                    List<object> objects = new List<object>();
                    List<object> newRects = new List<object>();
                    if (calc.newRect != calc.finalRect)
                    {
                        objects.Add(calc.obj);
                        newRects.Add(calc.finalRect);
                    }

                    if (objects.Count > 0)
                        try
                        {
                            SetObjectsProperties(td.control, objects.ToArray(), td.PropertyName, newRects.ToArray());
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
                Color prevColor = controlColor;
                procesor.Start((d) =>
                                   {
                                       if (td.animatorState.Canceled)
                                       {
                                           d.Cancel = true;
                                           return;
                                       }
                                       newColor = controlColor.MergeColor(td.color, cacl.NextSize);
                                       if (newColor != prevColor)
                                           SetObjectProperty(td.control, td.PropertyName, newColor);
                                       prevColor = newColor;
                                   });

                if (newColor != td.color)
                {
                    try
                    {
                        if (td.animatorState.Canceled && !td.CompleteIfCancel) return;
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

        private static void Custom(object customThreadParam)
        {
            CustomThreadParam td = (CustomThreadParam) customThreadParam;
            try
            {
                if (td.MaxIteration <= 0) return;
                int i = 0;
                while (!td.animatorState.Canceled)
                {
                    CustomObjectMethod(td.control, td.animatorState, td.customMethod);
                    i++;
                    if (i >= td.MaxIteration && td.MaxIteration > 0) break;
                    Thread.Sleep(td.time);
                }
            }
            finally
            {
                td.controlState.AnimatorEnd(td.animatorState);
            }
        }

        internal static AnimationControler.AnimePacket AnimeCustomPacket(string queueName, int sleepTime, CustomAnimeHandler method, int maxIteration, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback)
        {
            CustomThreadParam baseThreadParam = new CustomThreadParam();
            baseThreadParam.time = sleepTime;
            baseThreadParam.customMethod = method;
            baseThreadParam.MaxIteration = maxIteration;
            baseThreadParam.QueueLevel = queueLevel;
            baseThreadParam.finalCallback = finalCallback;
            baseThreadParam.queueName = queueName;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = Custom, threadParam = baseThreadParam };
        }

        public static object AnimeCustom(Control ctrl, string queueName, int sleepTime, CustomAnimeHandler method, int maxIteration, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeCustomPacket(queueName, sleepTime, method, maxIteration, queue, queueLevel, queueOwner, finalCallback));
        }

        internal static AnimationControler.AnimePacket AnimeWaitPacket(string queueName, int time, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback)
        {
            AnimationControler.BaseThreadParam baseThreadParam = new AnimationControler.BaseThreadParam();
            baseThreadParam.time = time;
            baseThreadParam.QueueLevel = queueLevel;
            baseThreadParam.finalCallback = finalCallback;
            baseThreadParam.queueName = queueName;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = Wait, threadParam = baseThreadParam };
        }

        public static object AnimeWait(Control ctrl, string queueName, int time, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeWaitPacket(queueName, time, queue, queueLevel, queueOwner, finalCallback));
        }

        internal static AnimationControler.AnimePacket AnimeIntPropPacket(string queueName, string propName, int height, int time, SpeedMode speedMode, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback, bool CompleteIfCancel)
        {
            IntValueThreadParam sizeParam = new IntValueThreadParam() { CompleteIfCancel = CompleteIfCancel };
            sizeParam.value = height;
            sizeParam.time = time;
            sizeParam.PropertyName = propName;
            sizeParam.speedMode = speedMode;
            sizeParam.QueueLevel = queueLevel;
            sizeParam.finalCallback = finalCallback;
            sizeParam.queueName = queueName;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = SetIntProperty, threadParam = sizeParam };
        }

        public static object AnimeIntProp(string queueName, object ctrl, string propName, int height, int time, SpeedMode speedMode, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback, bool CompleteIfCancel)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeIntPropPacket(queueName, propName, height, time, speedMode, queue, queueLevel, queueOwner, finalCallback, CompleteIfCancel));
        }

        internal static AnimationControler.AnimePacket AnimePointPropPacket(string queueName, string propName, Point point, int time, SpeedMode speedMode, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback, bool CompleteIfCancel)
        {
            LocationThreadParam sizeParam = new LocationThreadParam() { CompleteIfCancel = CompleteIfCancel };
            sizeParam.location = point;
            sizeParam.time = time;
            sizeParam.PropertyName = propName;
            sizeParam.speedMode = speedMode;
            sizeParam.QueueLevel = queueLevel;
            sizeParam.finalCallback = finalCallback;
            sizeParam.queueName = queueName;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = SetPointProperty, threadParam = sizeParam };
        }

        public static object AnimePointProp(object ctrl, string queueName, string propName, Point point, int time, SpeedMode speedMode, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback, bool CompleteIfCancel)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimePointPropPacket(queueName, propName, point, time, speedMode, queue, queueLevel, queueOwner, finalCallback, CompleteIfCancel));
        }

        internal static AnimationControler.AnimePacket AnimeSizePropPacket(string queueName, string propName, Size size, int time, SpeedMode speedMode, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback, bool CompleteIfCancel)
        {
            SizeThreadParam sizeParam = new SizeThreadParam() { CompleteIfCancel = CompleteIfCancel };
            sizeParam.size = size;
            sizeParam.time = time;
            sizeParam.PropertyName = propName;
            sizeParam.speedMode = speedMode;
            sizeParam.QueueLevel = queueLevel;
            sizeParam.finalCallback = finalCallback;
            sizeParam.queueName = queueName;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = SetSizeProperty, threadParam = sizeParam };
        }

        public static object AnimeSizeProp(object ctrl, string queueName, string propName, Size size, int time, SpeedMode speedMode, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback, bool CompleteIfCancel)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeSizePropPacket(queueName, propName, size, time, speedMode, queue, queueLevel, queueOwner, finalCallback, CompleteIfCancel));
        }

        internal static AnimationControler.AnimePacket AnimeRectPropPacket(string queueName, string propName, Rectangle rect, int time, SpeedMode speedMode, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback, bool CompleteIfCancel)
        {
            RectThreadParam sizeParam = new RectThreadParam() { CompleteIfCancel = CompleteIfCancel };
            sizeParam.rect = rect;
            sizeParam.time = time;
            sizeParam.PropertyName = propName;
            sizeParam.speedMode = speedMode;
            sizeParam.QueueLevel = queueLevel;
            sizeParam.finalCallback = finalCallback;
            sizeParam.queueName = queueName;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = SetRectProperty, threadParam = sizeParam };
        }

        public static object AnimeRectProp(object ctrl, string queueName, string propName, Rectangle rect, int time, SpeedMode speedMode, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback, bool CompleteIfCancel)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeRectPropPacket(queueName, propName, rect, time, speedMode, queue, queueLevel, queueOwner, finalCallback, CompleteIfCancel));
        }

        internal static AnimationControler.AnimePacket AnimeMultiRectPropPacket(string queueName, string propName, object[] objects, Rectangle[] rects, int time, SpeedMode speedMode, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback, bool CompleteIfCancel)
        {
            RectArrayThreadParam multiRectParam = new RectArrayThreadParam() { CompleteIfCancel = CompleteIfCancel };
            multiRectParam.rects = rects;
            multiRectParam.objects = objects;
            multiRectParam.time = time;
            multiRectParam.PropertyName = propName;
            multiRectParam.speedMode = speedMode;
            multiRectParam.QueueLevel = queueLevel;
            multiRectParam.finalCallback = finalCallback;
            multiRectParam.queueName = queueName;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = SetMultiRectProperty, threadParam = multiRectParam };
        }

        public static object AnimeMultiRectProp(object ctrl, string queueName, string propName, object[] objects, Rectangle[] rects, int time, SpeedMode speedMode, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback, bool CompleteIfCancel)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeMultiRectPropPacket(queueName, propName, objects, rects, time, speedMode, queue, queueLevel, queueOwner, finalCallback, CompleteIfCancel));
        }

        internal static AnimationControler.AnimePacket AnimeColorPropPacket(string queueName, string propName, Color color, int time, SpeedMode speedMode, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback, bool CompleteIfCancel)
        {
            ColorThreadParam colorParam = new ColorThreadParam() { CompleteIfCancel = CompleteIfCancel };
            colorParam.color = color;
            colorParam.time = time;
            colorParam.PropertyName = propName;
            colorParam.speedMode = speedMode;
            colorParam.QueueLevel = queueLevel;
            colorParam.finalCallback = finalCallback;
            colorParam.queueName = queueName;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = SetColorProperty, threadParam = colorParam };
        }

        public static object AnimeColorProp(object ctrl, string queueName, string propName, Color color, int time, SpeedMode speedMode, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback, bool CompleteIfCancel)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeColorPropPacket(queueName, propName, color, time, speedMode, queue, queueLevel, queueOwner, finalCallback, CompleteIfCancel));
        }

        internal static AnimationControler.AnimePacket AnimeHighlightPacket(string queueName, int highlightPercent, int time, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback)
        {
            HiglightThreadParam highlightParam = new HiglightThreadParam();
            highlightParam.HiglightPercent = highlightPercent;
            highlightParam.time = time;
            highlightParam.QueueLevel = queueLevel;
            highlightParam.finalCallback = finalCallback;
            highlightParam.queueName = queueName;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = DoHighlight, threadParam = highlightParam };
        }

        public static object AnimeHighlight(Control ctrl, string queueName, int highlightPercent, int time, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeHighlightPacket(queueName, highlightPercent, time, queue, queueLevel, queueOwner, finalCallback));
        }

        internal static AnimationControler.AnimePacket AnimeHighlightForecolorPacket(string queueName, int highlightPercent, int time, bool queue, int queueLevel, object qOwner, AnimationControler.FinalCallback finalCallback)
        {
            HiglightThreadParam highlightParam = new HiglightThreadParam();
            highlightParam.HiglightPercent = highlightPercent;
            highlightParam.time = time;
            highlightParam.QueueLevel = queueLevel;
            highlightParam.finalCallback = finalCallback;
            highlightParam.queueName = queueName;
            return new AnimationControler.AnimePacket() { isQueue = queue && queueLevel >= 0, queueOwner = qOwner, method = DoHighlightForecolor, threadParam = highlightParam };
        }

        public static object AnimeHighlightForecolor(Control ctrl, string queueName, int highlightPercent, int time, bool queue, int queueLevel, object queueOwner, AnimationControler.FinalCallback finalCallback)
        {
            return AnimationControler.ProcessPacket(ctrl, AnimeHighlightForecolorPacket(queueName, highlightPercent, time, queue, queueLevel, queueOwner, finalCallback));
        }

        public static void AnimeCancel(object ctrl, string name)
        {
            AnimationControler.Cancel(ctrl, name, false);
        }

        public static void AnimeForce(object ctrl, string name)
        {
            AnimationControler.Cancel(ctrl, name, true);
        }

    }
}

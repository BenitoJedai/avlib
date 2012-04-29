using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AVLib.Animations;

namespace AVLib.Draw.DrawRects
{
    public static class DrawRectsAnimeExternals
    {
        public class DrawRectQueue : AnimationExternals.ObjectQueue
        {
            public DrawRectQueue(DrawRect ctrl, int queueLevel, object queueOwner, bool isQueue)
                : base(ctrl, queueLevel, queueOwner, isQueue)
            {

            }
        }


        public static DrawRectQueue Anime(this DrawRect ctrl)
        {
            return new DrawRectQueue(ctrl, -1, null, false);
        }

        public static DrawRectQueue Anime(this DrawRect ctrl, bool CompleteIfCancel)
        {
            return new DrawRectQueue(ctrl, -1, null, false) { CompleteIfCancel = CompleteIfCancel };
        }

        public static DrawRectQueue Anime(this DrawRect ctrl, int time)
        {
            return new DrawRectQueue(ctrl, -1, null, false) { time = time };
        }

        public static DrawRectQueue Anime(this DrawRect ctrl, int time, bool CompleteIfCancel)
        {
            return new DrawRectQueue(ctrl, -1, null, false) { time = time, CompleteIfCancel = CompleteIfCancel };
        }

        public static DrawRectQueue Anime(this DrawRect ctrl, int time, SpeedMode speedMode)
        {
            return new DrawRectQueue(ctrl, -1, null, false) { time = time, SpeedMode = speedMode };
        }

        public static DrawRectQueue Anime(this DrawRect ctrl, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new DrawRectQueue(ctrl, -1, null, false) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel };
        }

        public static DrawRectQueue Anime(this DrawRect ctrl, Speed speed)
        {
            return new DrawRectQueue(ctrl, -1, null, false) { Speed = speed };
        }

        public static DrawRectQueue Anime(this DrawRect ctrl, Speed speed, bool CompleteIfCancel)
        {
            return new DrawRectQueue(ctrl, -1, null, false) { Speed = speed, CompleteIfCancel = CompleteIfCancel };
        }

        public static DrawRectQueue Anime(this DrawRect ctrl, Speed speed, SpeedMode speedMode)
        {
            return new DrawRectQueue(ctrl, -1, null, false) { Speed = speed, SpeedMode = speedMode };
        }

        public static DrawRectQueue Anime(this DrawRect ctrl, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new DrawRectQueue(ctrl, -1, null, false) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel };
        }

        public static DrawRectQueue AnimeQueue(this DrawRect ctrl)
        {
            return new DrawRectQueue(ctrl, -1, null, true);
        }

        public static DrawRectQueue AnimeQueue(this DrawRect ctrl, bool CompleteIfCancel)
        {
            return new DrawRectQueue(ctrl, -1, null, true) { CompleteIfCancel = CompleteIfCancel };
        }

        public static DrawRectQueue AnimeQueue(this DrawRect ctrl, int time)
        {
            return new DrawRectQueue(ctrl, -1, null, true) { time = time };
        }

        public static DrawRectQueue AnimeQueue(this DrawRect ctrl, int time, bool CompleteIfCancel)
        {
            return new DrawRectQueue(ctrl, -1, null, true) { time = time, CompleteIfCancel = CompleteIfCancel };
        }

        public static DrawRectQueue AnimeQueue(this DrawRect ctrl, int time, SpeedMode speedMode)
        {
            return new DrawRectQueue(ctrl, -1, null, true) { time = time, SpeedMode = speedMode };
        }

        public static DrawRectQueue AnimeQueue(this DrawRect ctrl, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new DrawRectQueue(ctrl, -1, null, true) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel };
        }

        public static DrawRectQueue AnimeQueue(this DrawRect ctrl, Speed speed)
        {
            return new DrawRectQueue(ctrl, -1, null, true) { Speed = speed };
        }

        public static DrawRectQueue AnimeQueue(this DrawRect ctrl, Speed speed, bool CompleteIfCancel)
        {
            return new DrawRectQueue(ctrl, -1, null, true) { Speed = speed, CompleteIfCancel = CompleteIfCancel };
        }

        public static DrawRectQueue AnimeQueue(this DrawRect ctrl, Speed speed, SpeedMode speedMode)
        {
            return new DrawRectQueue(ctrl, -1, null, true) { Speed = speed, SpeedMode = speedMode };
        }

        public static DrawRectQueue AnimeQueue(this DrawRect ctrl, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new DrawRectQueue(ctrl, -1, null, true) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel };
        }
        





        public static DrawRectQueue Wait(this DrawRectQueue ctrlQ, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static DrawRectQueue Wait(this DrawRectQueue ctrlQ, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static DrawRectQueue Wait(this DrawRectQueue ctrlQ, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static DrawRectQueue Wait(this DrawRectQueue ctrlQ, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }



        #region Height

        public static DrawRectQueue Height(this DrawRectQueue ctrlQ, int height, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Height(this DrawRectQueue ctrlQ, int height, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Height(this DrawRectQueue ctrlQ, int height)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue HeightInc(this DrawRectQueue ctrlQ, int height, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue HeightInc(this DrawRectQueue ctrlQ, int height, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue HeightInc(this DrawRectQueue ctrlQ, int height)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue HeightDec(this DrawRectQueue ctrlQ, int height, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue HeightDec(this DrawRectQueue ctrlQ, int height, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue HeightDec(this DrawRectQueue ctrlQ, int height)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }



        public static DrawRectQueue Height(this DrawRectQueue ctrlQ, int height, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Height(this DrawRectQueue ctrlQ, int height, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Height(this DrawRectQueue ctrlQ, int height, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue HeightInc(this DrawRectQueue ctrlQ, int height, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue HeightInc(this DrawRectQueue ctrlQ, int height, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue HeightInc(this DrawRectQueue ctrlQ, int height, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue HeightDec(this DrawRectQueue ctrlQ, int height, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue HeightDec(this DrawRectQueue ctrlQ, int height, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue HeightDec(this DrawRectQueue ctrlQ, int height, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        #endregion




        #region Width

        public static DrawRectQueue Width(this DrawRectQueue ctrlQ, int width, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Width(this DrawRectQueue ctrlQ, int width, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Width(this DrawRectQueue ctrlQ, int width)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue WidthInc(this DrawRectQueue ctrlQ, int width, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue WidthInc(this DrawRectQueue ctrlQ, int width, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue WidthInc(this DrawRectQueue ctrlQ, int width)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue WidthDec(this DrawRectQueue ctrlQ, int width, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue WidthDec(this DrawRectQueue ctrlQ, int width, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue WidthDec(this DrawRectQueue ctrlQ, int width)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Width(this DrawRectQueue ctrlQ, int width, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Width(this DrawRectQueue ctrlQ, int width, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Width(this DrawRectQueue ctrlQ, int width, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue WidthInc(this DrawRectQueue ctrlQ, int width, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue WidthInc(this DrawRectQueue ctrlQ, int width, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue WidthInc(this DrawRectQueue ctrlQ, int width, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue WidthDec(this DrawRectQueue ctrlQ, int width, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue WidthDec(this DrawRectQueue ctrlQ, int width, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue WidthDec(this DrawRectQueue ctrlQ, int width, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        #endregion








        #region Pos

        public static DrawRectQueue Pos(this DrawRectQueue ctrlQ, Point pos, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Pos(this DrawRectQueue ctrlQ, Point pos, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Pos(this DrawRectQueue ctrlQ, Point pos)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue PosInc(this DrawRectQueue ctrlQ, Point pos, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue PosInc(this DrawRectQueue ctrlQ, Point pos, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue PosInc(this DrawRectQueue ctrlQ, Point pos)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue PosDec(this DrawRectQueue ctrlQ, Point pos, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue PosDec(this DrawRectQueue ctrlQ, Point pos, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue PosDec(this DrawRectQueue ctrlQ, Point pos)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Pos(this DrawRectQueue ctrlQ, Point pos, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Pos(this DrawRectQueue ctrlQ, Point pos, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Pos(this DrawRectQueue ctrlQ, Point pos, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue PosInc(this DrawRectQueue ctrlQ, Point pos, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue PosInc(this DrawRectQueue ctrlQ, Point pos, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue PosInc(this DrawRectQueue ctrlQ, Point pos, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue PosDec(this DrawRectQueue ctrlQ, Point pos, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue PosDec(this DrawRectQueue ctrlQ, Point pos, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue PosDec(this DrawRectQueue ctrlQ, Point pos, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket("Pos", pos, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        #endregion









        #region Size

        public static DrawRectQueue Size(this DrawRectQueue ctrlQ, Size size, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Size(this DrawRectQueue ctrlQ, Size size, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Size(this DrawRectQueue ctrlQ, Size size)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue SizeInc(this DrawRectQueue ctrlQ, Size size, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue SizeInc(this DrawRectQueue ctrlQ, Size size, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue SizeInc(this DrawRectQueue ctrlQ, Size size)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue SizeDec(this DrawRectQueue ctrlQ, Size size, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue SizeDec(this DrawRectQueue ctrlQ, Size size, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue SizeDec(this DrawRectQueue ctrlQ, Size size)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Size(this DrawRectQueue ctrlQ, Size size, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Size(this DrawRectQueue ctrlQ, Size size, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Size(this DrawRectQueue ctrlQ, Size size, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue SizeInc(this DrawRectQueue ctrlQ, Size size, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue SizeInc(this DrawRectQueue ctrlQ, Size size, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue SizeInc(this DrawRectQueue ctrlQ, Size size, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue SizeDec(this DrawRectQueue ctrlQ, Size size, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue SizeDec(this DrawRectQueue ctrlQ, Size size, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue SizeDec(this DrawRectQueue ctrlQ, Size size, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket("Size", size, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        #endregion









        #region Rect

        public static DrawRectQueue Rect(this DrawRectQueue ctrlQ, Rectangle rect, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Rect(this DrawRectQueue ctrlQ, Rectangle rect, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Rect(this DrawRectQueue ctrlQ, Rectangle rect)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue RectInc(this DrawRectQueue ctrlQ, Rectangle rect, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue RectInc(this DrawRectQueue ctrlQ, Rectangle rect, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue RectInc(this DrawRectQueue ctrlQ, Rectangle rect)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue RectDec(this DrawRectQueue ctrlQ, Rectangle rect, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue RectDec(this DrawRectQueue ctrlQ, Rectangle rect, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue RectDec(this DrawRectQueue ctrlQ, Rectangle rect)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Rect(this DrawRectQueue ctrlQ, Rectangle rect, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Rect(this DrawRectQueue ctrlQ, Rectangle rect, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue Rect(this DrawRectQueue ctrlQ, Rectangle rect, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue RectInc(this DrawRectQueue ctrlQ, Rectangle rect, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue RectInc(this DrawRectQueue ctrlQ, Rectangle rect, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue RectInc(this DrawRectQueue ctrlQ, Rectangle rect, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue RectDec(this DrawRectQueue ctrlQ, Rectangle rect, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue RectDec(this DrawRectQueue ctrlQ, Rectangle rect, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue RectDec(this DrawRectQueue ctrlQ, Rectangle rect, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("Rect", rect, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        #endregion







        #region ContainerRect

        public static DrawRectQueue ContainerRect(this DrawRectQueue ctrlQ, Rectangle rect, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRect(this DrawRectQueue ctrlQ, Rectangle rect, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRect(this DrawRectQueue ctrlQ, Rectangle rect)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRectInc(this DrawRectQueue ctrlQ, Rectangle rect, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRectInc(this DrawRectQueue ctrlQ, Rectangle rect, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRectInc(this DrawRectQueue ctrlQ, Rectangle rect)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRectDec(this DrawRectQueue ctrlQ, Rectangle rect, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRectDec(this DrawRectQueue ctrlQ, Rectangle rect, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRectDec(this DrawRectQueue ctrlQ, Rectangle rect)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRect(this DrawRectQueue ctrlQ, Rectangle rect, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRect(this DrawRectQueue ctrlQ, Rectangle rect, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRect(this DrawRectQueue ctrlQ, Rectangle rect, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRectInc(this DrawRectQueue ctrlQ, Rectangle rect, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRectInc(this DrawRectQueue ctrlQ, Rectangle rect, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRectInc(this DrawRectQueue ctrlQ, Rectangle rect, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRectDec(this DrawRectQueue ctrlQ, Rectangle rect, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRectDec(this DrawRectQueue ctrlQ, Rectangle rect, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static DrawRectQueue ContainerRectDec(this DrawRectQueue ctrlQ, Rectangle rect, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket("ContainerRect", rect, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        #endregion
    }
}

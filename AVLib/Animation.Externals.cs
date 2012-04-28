using System;
using System.Drawing;
using System.Windows.Forms;

namespace AVLib.Animations
{
    public static class AnimationExternals
    {
        public class ObjectQueue
        {
            internal object ctrl;
            internal int queueLevel = 0;
            internal object queueOwner = null;
            internal bool isQueue = false;
            internal int time = AnimationControler.GetSpeedTime(Speed.Fast);
            internal Speed speed = Speed.Fast;
            internal SpeedMode speedMode = SpeedMode.Normal;

            public ObjectQueue(object ctrl, int queueLevel, object queueOwner, bool isQueue)
            {
                this.ctrl = ctrl;
                this.queueLevel = queueLevel;
                this.queueOwner = queueOwner;
                this.isQueue = isQueue;
            }

            internal virtual void ProcessPacket(AnimationControler.AnimePacket packet)
            {
                queueOwner = AnimationControler.ProcessPacket(ctrl, packet);
                if (isQueue) queueLevel++;
            }
        }

        public class ControlQueue : ObjectQueue
        {
            public ControlQueue(Control ctrl, int queueLevel, object queueOwner, bool isQueue) : base(ctrl, queueLevel, queueOwner, isQueue)
            {
                
            }
        }

        public static void AnimeCancel(this object ctrl)
        {
            AnimationUtils.AnimeCancel(ctrl);
        }

        public static ObjectQueue Anime(this object ctrl)
        {
            return new ObjectQueue(ctrl, -1, null, false);
        }

        public static ObjectQueue AnimeQueue(this object ctrl)
        {
            return new ObjectQueue(ctrl, -1, null, true);
        }

        public static ControlQueue Anime(this Control ctrl)
        {
            return new ControlQueue(ctrl, -1, null, false);
        }

        public static ControlQueue AnimeQueue(this Control ctrl)
        {
            return new ControlQueue(ctrl, -1, null, true);
        }

        #region Wait

        public static ControlQueue Wait(this ControlQueue ctrlQ, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Wait(this ControlQueue ctrlQ, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }


        public static ControlQueue Wait(this ControlQueue ctrlQ, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue Wait(this ControlQueue ctrlQ, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        #endregion

        #region Height

        public static ControlQueue Height(this ControlQueue ctrlQ, int height, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, time, SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Height(this ControlQueue ctrlQ, int height, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Height(this ControlQueue ctrlQ, int height)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue HeightInc(this ControlQueue ctrlQ, int height, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue HeightInc(this ControlQueue ctrlQ, int height, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue HeightInc(this ControlQueue ctrlQ, int height)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue HeightDec(this ControlQueue ctrlQ, int height, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue HeightDec(this ControlQueue ctrlQ, int height, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue HeightDec(this ControlQueue ctrlQ, int height)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }



        public static ControlQueue Height(this ControlQueue ctrlQ, int height, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, time, SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue Height(this ControlQueue ctrlQ, int height, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue Height(this ControlQueue ctrlQ, int height, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue HeightInc(this ControlQueue ctrlQ, int height, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue HeightInc(this ControlQueue ctrlQ, int height, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue HeightInc(this ControlQueue ctrlQ, int height, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue HeightDec(this ControlQueue ctrlQ, int height, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue HeightDec(this ControlQueue ctrlQ, int height, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue HeightDec(this ControlQueue ctrlQ, int height, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Height", height, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        #endregion

        #region Width

        public static ControlQueue Width(this ControlQueue ctrlQ, int width, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, time, SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Width(this ControlQueue ctrlQ, int width, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Width(this ControlQueue ctrlQ, int width)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue WidthInc(this ControlQueue ctrlQ, int width, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue WidthInc(this ControlQueue ctrlQ, int width, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue WidthInc(this ControlQueue ctrlQ, int width)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue WidthDec(this ControlQueue ctrlQ, int width, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue WidthDec(this ControlQueue ctrlQ, int width, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue WidthDec(this ControlQueue ctrlQ, int width)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }




        public static ControlQueue Width(this ControlQueue ctrlQ, int width, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, time, SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue Width(this ControlQueue ctrlQ, int width, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue Width(this ControlQueue ctrlQ, int width, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue WidthInc(this ControlQueue ctrlQ, int width, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue WidthInc(this ControlQueue ctrlQ, int width, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue WidthInc(this ControlQueue ctrlQ, int width, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue WidthDec(this ControlQueue ctrlQ, int width, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue WidthDec(this ControlQueue ctrlQ, int width, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue WidthDec(this ControlQueue ctrlQ, int width, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket("Width", width, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        #endregion

        #region Change<T>

        private static ObjectQueue UnsuportedType()
        {
            throw new ApplicationException("Unsuported type");
        }

        public static ObjectQueue Change<T>(this ObjectQueue ctrlQ, string propName, T value, int time)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int) (object) value, time, SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, time, SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue Change<T>(this ObjectQueue ctrlQ, string propName, T value, Speed speed)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int) (object) value, AnimationControler.GetSpeedTime(speed), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue Change<T>(this ObjectQueue ctrlQ, string propName, T value)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeInc<T>(this ObjectQueue ctrlQ, string propName, T value, int time)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeInc<T>(this ObjectQueue ctrlQ, string propName, T value, Speed speed)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeInc<T>(this ObjectQueue ctrlQ, string propName, T value)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeDec<T>(this ObjectQueue ctrlQ, string propName, T value, int time)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeDec<T>(this ObjectQueue ctrlQ, string propName, T value, Speed speed)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeDec<T>(this ObjectQueue ctrlQ, string propName, T value)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
                return ctrlQ;
            }
            return UnsuportedType();
        }



        public static ObjectQueue Change<T>(this ObjectQueue ctrlQ, string propName, T value, int time, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, time, SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, time, SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue Change<T>(this ObjectQueue ctrlQ, string propName, T value, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue Change<T>(this ObjectQueue ctrlQ, string propName, T value, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeInc<T>(this ObjectQueue ctrlQ, string propName, T value, int time, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeInc<T>(this ObjectQueue ctrlQ, string propName, T value, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeInc<T>(this ObjectQueue ctrlQ, string propName, T value, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeDec<T>(this ObjectQueue ctrlQ, string propName, T value, int time, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeDec<T>(this ObjectQueue ctrlQ, string propName, T value, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeDec<T>(this ObjectQueue ctrlQ, string propName, T value, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(propName, (int)(object)value, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(propName, (Color)(object)value, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        #endregion

        #region Highlight

        public static ControlQueue Highlight(this ControlQueue ctrlQ, int highlightPercent, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightPacket(highlightPercent, time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Highlight(this ControlQueue ctrlQ, int highlightPercent, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightPacket(highlightPercent, AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Highlight(this ControlQueue ctrlQ, int highlightPercent)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightPacket(highlightPercent, AnimationControler.GetSpeedTime(Speed.Fast), true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }


        public static ControlQueue Highlight(this ControlQueue ctrlQ, int highlightPercent, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightPacket(highlightPercent, time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue Highlight(this ControlQueue ctrlQ, int highlightPercent, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightPacket(highlightPercent, AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue Highlight(this ControlQueue ctrlQ, int highlightPercent, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightPacket(highlightPercent, AnimationControler.GetSpeedTime(Speed.Fast), true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        #endregion

        #region AnimeHighlightForecolor

        public static ControlQueue HighlightForecolor(this ControlQueue ctrlQ, int highlightPercent, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightForecolorPacket(highlightPercent, time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue HighlightForecolor(this ControlQueue ctrlQ, int highlightPercent, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightForecolorPacket(highlightPercent, AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue HighlightForecolor(this ControlQueue ctrlQ, int highlightPercent)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightForecolorPacket(highlightPercent, AnimationControler.GetSpeedTime(Speed.Fast), true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }


        public static ControlQueue HighlightForecolor(this ControlQueue ctrlQ, int highlightPercent, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightForecolorPacket(highlightPercent, time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue HighlightForecolor(this ControlQueue ctrlQ, int highlightPercent, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightForecolorPacket(highlightPercent, AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue HighlightForecolor(this ControlQueue ctrlQ, int highlightPercent, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightForecolorPacket(highlightPercent, AnimationControler.GetSpeedTime(Speed.Fast), true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        #endregion

        #region Color

        public static ControlQueue Color(this ControlQueue ctrlQ, Color color, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, time, SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Color(this ControlQueue ctrlQ, Color color, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Color(this ControlQueue ctrlQ, Color color)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ColorInc(this ControlQueue ctrlQ, Color color, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ColorInc(this ControlQueue ctrlQ, Color color, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ColorInc(this ControlQueue ctrlQ, Color color)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ColorDec(this ControlQueue ctrlQ, Color color, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ColorDec(this ControlQueue ctrlQ, Color color, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ColorDec(this ControlQueue ctrlQ, Color color)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }



        public static ControlQueue Color(this ControlQueue ctrlQ, Color color, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, time, SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue Color(this ControlQueue ctrlQ, Color color, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue Color(this ControlQueue ctrlQ, Color color, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ColorInc(this ControlQueue ctrlQ, Color color, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ColorInc(this ControlQueue ctrlQ, Color color, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ColorInc(this ControlQueue ctrlQ, Color color, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ColorDec(this ControlQueue ctrlQ, Color color, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ColorDec(this ControlQueue ctrlQ, Color color, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ColorDec(this ControlQueue ctrlQ, Color color, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("BackColor", color, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        #endregion

        #region AnimeForeColor

        public static ControlQueue ForeColor(this ControlQueue ctrlQ, Color color, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, time, SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ForeColor(this ControlQueue ctrlQ, Color color, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ForeColor(this ControlQueue ctrlQ, Color color)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ForeColorInc(this ControlQueue ctrlQ, Color color, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ForeColorInc(this ControlQueue ctrlQ, Color color, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ForeColorInc(this ControlQueue ctrlQ, Color color)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ForeColorDec(this ControlQueue ctrlQ, Color color, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ForeColorDec(this ControlQueue ctrlQ, Color color, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue ForeColorDec(this ControlQueue ctrlQ, Color color)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }



        public static ControlQueue ForeColor(this ControlQueue ctrlQ, Color color, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, time, SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ForeColor(this ControlQueue ctrlQ, Color color, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ForeColor(this ControlQueue ctrlQ, Color color, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Normal, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ForeColorInc(this ControlQueue ctrlQ, Color color, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ForeColorInc(this ControlQueue ctrlQ, Color color, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ForeColorInc(this ControlQueue ctrlQ, Color color, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ForeColorDec(this ControlQueue ctrlQ, Color color, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ForeColorDec(this ControlQueue ctrlQ, Color color, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue ForeColorDec(this ControlQueue ctrlQ, Color color, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket("ForeColor", color, AnimationControler.GetSpeedTime(Speed.Fast), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        #endregion
    }
}
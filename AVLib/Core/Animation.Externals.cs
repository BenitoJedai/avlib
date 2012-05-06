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
            internal Speed Speed{set { time = AnimationControler.GetSpeedTime(value); }}
            internal SpeedMode SpeedMode = SpeedMode.Normal;
            internal bool CompleteIfCancel = false;
            internal string QueueName = "";

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
            AnimationUtils.AnimeCancel(ctrl, "");
        }

        public static void AnimeCancelAll(this object ctrl)
        {
            AnimationUtils.AnimeCancel(ctrl, "all");
        }

        public static void AnimeCancel(this object ctrl, string queueName)
        {
            AnimationUtils.AnimeCancel(ctrl, queueName);
        }

        public static void AnimeForce(this object ctrl)
        {
            AnimationUtils.AnimeForce(ctrl, "");
        }

        public static void AnimeForceAll(this object ctrl)
        {
            AnimationUtils.AnimeForce(ctrl, "all");
        }

        public static void AnimeForce(this object ctrl, string queueName)
        {
            AnimationUtils.AnimeForce(ctrl, queueName);
        }





        public static ObjectQueue Anime(this object ctrl)
        {
            return new ObjectQueue(ctrl, -1, null, false);
        }

        public static ObjectQueue Anime(this object ctrl, string queueName)
        {
            return new ObjectQueue(ctrl, -1, null, false){QueueName = queueName};
        }

        public static ObjectQueue Anime(this object ctrl, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, false){CompleteIfCancel = CompleteIfCancel};
        }

        public static ObjectQueue Anime(this object ctrl, string queueName, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, false) { CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ObjectQueue Anime(this object ctrl, int time)
        {
            return new ObjectQueue(ctrl, -1, null, false){time = time};
        }

        public static ObjectQueue Anime(this object ctrl, string queueName, int time)
        {
            return new ObjectQueue(ctrl, -1, null, false) { time = time, QueueName = queueName};
        }

        public static ObjectQueue Anime(this object ctrl, int time, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, false) { time = time, CompleteIfCancel = CompleteIfCancel};
        }

        public static ObjectQueue Anime(this object ctrl, string queueName, int time, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, false) { time = time, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ObjectQueue Anime(this object ctrl, int time, SpeedMode speedMode)
        {
            return new ObjectQueue(ctrl, -1, null, false) { time = time, SpeedMode = speedMode};
        }

        public static ObjectQueue Anime(this object ctrl, string queueName, int time, SpeedMode speedMode)
        {
            return new ObjectQueue(ctrl, -1, null, false) { time = time, SpeedMode = speedMode, QueueName = queueName};
        }

        public static ObjectQueue Anime(this object ctrl, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, false) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static ObjectQueue Anime(this object ctrl, string queueName, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, false) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ObjectQueue Anime(this object ctrl, Speed speed)
        {
            return new ObjectQueue(ctrl, -1, null, false) { Speed = speed };
        }

        public static ObjectQueue Anime(this object ctrl, string queueName, Speed speed)
        {
            return new ObjectQueue(ctrl, -1, null, false) { Speed = speed, QueueName = queueName};
        }

        public static ObjectQueue Anime(this object ctrl, Speed speed, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, false) { Speed = speed, CompleteIfCancel = CompleteIfCancel};
        }

        public static ObjectQueue Anime(this object ctrl, string queueName, Speed speed, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, false) { Speed = speed, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ObjectQueue Anime(this object ctrl, Speed speed, SpeedMode speedMode)
        {
            return new ObjectQueue(ctrl, -1, null, false) { Speed = speed, SpeedMode = speedMode};
        }

        public static ObjectQueue Anime(this object ctrl, string queueName, Speed speed, SpeedMode speedMode)
        {
            return new ObjectQueue(ctrl, -1, null, false) { Speed = speed, SpeedMode = speedMode, QueueName = queueName};
        }

        public static ObjectQueue Anime(this object ctrl, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, false) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static ObjectQueue Anime(this object ctrl, string queueName, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, false) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ObjectQueue AnimeQueue(this object ctrl)
        {
            return new ObjectQueue(ctrl, -1, null, true);
        }

        public static ObjectQueue AnimeQueue(this object ctrl, string queueName)
        {
            return new ObjectQueue(ctrl, -1, null, true){QueueName = queueName};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, true){CompleteIfCancel = CompleteIfCancel};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, string queueName, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, true) { CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, int time)
        {
            return new ObjectQueue(ctrl, -1, null, true){time = time};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, string queueName, int time)
        {
            return new ObjectQueue(ctrl, -1, null, true) { time = time, QueueName = queueName};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, int time, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, true) { time = time, CompleteIfCancel = CompleteIfCancel};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, string queueName, int time, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, true) { time = time, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, int time, SpeedMode speedMode)
        {
            return new ObjectQueue(ctrl, -1, null, true) { time = time, SpeedMode = speedMode};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, string queueName, int time, SpeedMode speedMode)
        {
            return new ObjectQueue(ctrl, -1, null, true) { time = time, SpeedMode = speedMode, QueueName = queueName};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, true) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, string queueName, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, true) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, Speed speed)
        {
            return new ObjectQueue(ctrl, -1, null, true) { Speed = speed};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, string queueName, Speed speed)
        {
            return new ObjectQueue(ctrl, -1, null, true) { Speed = speed, QueueName = queueName};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, Speed speed, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, true) { Speed = speed, CompleteIfCancel = CompleteIfCancel};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, string queueName, Speed speed, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, true) { Speed = speed, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, Speed speed, SpeedMode speedMode)
        {
            return new ObjectQueue(ctrl, -1, null, true) { Speed = speed, SpeedMode = speedMode};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, string queueName, Speed speed, SpeedMode speedMode)
        {
            return new ObjectQueue(ctrl, -1, null, true) { Speed = speed, SpeedMode = speedMode, QueueName = queueName};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, true) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static ObjectQueue AnimeQueue(this object ctrl, string queueName, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ObjectQueue(ctrl, -1, null, true) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }




        public static ControlQueue Anime(this Control ctrl)
        {
            return new ControlQueue(ctrl, -1, null, false);
        }

        public static ControlQueue Anime(this Control ctrl, string queueName)
        {
            return new ControlQueue(ctrl, -1, null, false){QueueName = queueName};
        }

        public static ControlQueue Anime(this Control ctrl, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, false){CompleteIfCancel = CompleteIfCancel};
        }

        public static ControlQueue Anime(this Control ctrl, string queueName, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, false) { CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ControlQueue Anime(this Control ctrl, int time)
        {
            return new ControlQueue(ctrl, -1, null, false){time = time};
        }

        public static ControlQueue Anime(this Control ctrl, string queueName, int time)
        {
            return new ControlQueue(ctrl, -1, null, false) { time = time, QueueName = queueName};
        }

        public static ControlQueue Anime(this Control ctrl, int time, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, false) { time = time, CompleteIfCancel = CompleteIfCancel};
        }

        public static ControlQueue Anime(this Control ctrl, string queueName, int time, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, false) { time = time, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ControlQueue Anime(this Control ctrl, int time, SpeedMode speedMode)
        {
            return new ControlQueue(ctrl, -1, null, false) { time = time, SpeedMode = speedMode};
        }

        public static ControlQueue Anime(this Control ctrl, string queueName, int time, SpeedMode speedMode)
        {
            return new ControlQueue(ctrl, -1, null, false) { time = time, SpeedMode = speedMode, QueueName = queueName};
        }

        public static ControlQueue Anime(this Control ctrl, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, false) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static ControlQueue Anime(this Control ctrl, string queueName, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, false) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ControlQueue Anime(this Control ctrl, Speed speed)
        {
            return new ControlQueue(ctrl, -1, null, false) { Speed = speed};
        }

        public static ControlQueue Anime(this Control ctrl, string queueName, Speed speed)
        {
            return new ControlQueue(ctrl, -1, null, false) { Speed = speed, QueueName = queueName};
        }

        public static ControlQueue Anime(this Control ctrl, Speed speed, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, false) { Speed = speed, CompleteIfCancel = CompleteIfCancel};
        }

        public static ControlQueue Anime(this Control ctrl, string queueName, Speed speed, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, false) { Speed = speed, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ControlQueue Anime(this Control ctrl, Speed speed, SpeedMode speedMode)
        {
            return new ControlQueue(ctrl, -1, null, false) { Speed = speed, SpeedMode = speedMode};
        }

        public static ControlQueue Anime(this Control ctrl, string queueName, Speed speed, SpeedMode speedMode)
        {
            return new ControlQueue(ctrl, -1, null, false) { Speed = speed, SpeedMode = speedMode, QueueName = queueName};
        }

        public static ControlQueue Anime(this Control ctrl, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, false) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static ControlQueue Anime(this Control ctrl, string queueName, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, false) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ControlQueue AnimeQueue(this Control ctrl)
        {
            return new ControlQueue(ctrl, -1, null, true);
        }

        public static ControlQueue AnimeQueue(this Control ctrl, string queueName)
        {
            return new ControlQueue(ctrl, -1, null, true){QueueName = queueName};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, true){CompleteIfCancel = CompleteIfCancel};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, string queueName, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, true) { CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, int time)
        {
            return new ControlQueue(ctrl, -1, null, true){time = time};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, string queueName, int time)
        {
            return new ControlQueue(ctrl, -1, null, true) { time = time, QueueName = queueName};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, int time, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, true) { time = time, CompleteIfCancel = CompleteIfCancel};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, string queueName, int time, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, true) { time = time, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, int time, SpeedMode speedMode)
        {
            return new ControlQueue(ctrl, -1, null, true) { time = time, SpeedMode = speedMode};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, string queueName, int time, SpeedMode speedMode)
        {
            return new ControlQueue(ctrl, -1, null, true) { time = time, SpeedMode = speedMode, QueueName = queueName};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, true) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, string queueName, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, true) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, Speed speed)
        {
            return new ControlQueue(ctrl, -1, null, true) { Speed = speed};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, string queueName, Speed speed)
        {
            return new ControlQueue(ctrl, -1, null, true) { Speed = speed, QueueName = queueName};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, Speed speed, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, true) { Speed = speed, CompleteIfCancel = CompleteIfCancel};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, string queueName, Speed speed, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, true) { Speed = speed, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, Speed speed, SpeedMode speedMode)
        {
            return new ControlQueue(ctrl, -1, null, true) { Speed = speed, SpeedMode = speedMode};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, string queueName, Speed speed, SpeedMode speedMode)
        {
            return new ControlQueue(ctrl, -1, null, true) { Speed = speed, SpeedMode = speedMode, QueueName = queueName};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, true) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static ControlQueue AnimeQueue(this Control ctrl, string queueName, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new ControlQueue(ctrl, -1, null, true) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }








        #region Wait

        public static ControlQueue Wait(this ControlQueue ctrlQ, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(ctrlQ.QueueName, time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Wait(this ControlQueue ctrlQ, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(ctrlQ.QueueName, AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }


        public static ControlQueue Wait(this ControlQueue ctrlQ, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(ctrlQ.QueueName, time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue Wait(this ControlQueue ctrlQ, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(ctrlQ.QueueName, AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }



        public static ObjectQueue Wait(this ObjectQueue ctrlQ, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(ctrlQ.QueueName, time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ObjectQueue Wait(this ObjectQueue ctrlQ, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(ctrlQ.QueueName, AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }


        public static ObjectQueue Wait(this ObjectQueue ctrlQ, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(ctrlQ.QueueName, time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ObjectQueue Wait(this ObjectQueue ctrlQ, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeWaitPacket(ctrlQ.QueueName, AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        #endregion





        #region Custom

        public static ControlQueue Custom(this ControlQueue ctrlQ, int sleepTime, AnimationUtils.CustomAnimeHandler method)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeCustomPacket(ctrlQ.QueueName, sleepTime, method, 0, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Custom(this ControlQueue ctrlQ, int sleepTime, int maxIteration, AnimationUtils.CustomAnimeHandler method)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeCustomPacket(ctrlQ.QueueName, sleepTime, method, maxIteration, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Custom(this ControlQueue ctrlQ, int sleepTime, AnimationUtils.CustomAnimeHandler method, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeCustomPacket(ctrlQ.QueueName, sleepTime, method, 0, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue Custom(this ControlQueue ctrlQ, int sleepTime, int maxIteration, AnimationUtils.CustomAnimeHandler method, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeCustomPacket(ctrlQ.QueueName, sleepTime, method, maxIteration, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ObjectQueue Custom(this ObjectQueue ctrlQ, int sleepTime, AnimationUtils.CustomAnimeHandler method)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeCustomPacket(ctrlQ.QueueName, sleepTime, method, 0, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ObjectQueue Custom(this ObjectQueue ctrlQ, int sleepTime, int maxIteration, AnimationUtils.CustomAnimeHandler method)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeCustomPacket(ctrlQ.QueueName, sleepTime, method, maxIteration, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ObjectQueue Custom(this ObjectQueue ctrlQ, int sleepTime, AnimationUtils.CustomAnimeHandler method, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeCustomPacket(ctrlQ.QueueName, sleepTime, method, 0, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ObjectQueue Custom(this ObjectQueue ctrlQ, int sleepTime, int maxIteration, AnimationUtils.CustomAnimeHandler method, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeCustomPacket(ctrlQ.QueueName, sleepTime, method, maxIteration, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        #endregion



        #region Height

        public static ControlQueue Height(this ControlQueue ctrlQ, int height, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue Height(this ControlQueue ctrlQ, int height, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue Height(this ControlQueue ctrlQ, int height)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue HeightInc(this ControlQueue ctrlQ, int height, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue HeightInc(this ControlQueue ctrlQ, int height, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue HeightInc(this ControlQueue ctrlQ, int height)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue HeightDec(this ControlQueue ctrlQ, int height, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue HeightDec(this ControlQueue ctrlQ, int height, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue HeightDec(this ControlQueue ctrlQ, int height)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }



        public static ControlQueue Height(this ControlQueue ctrlQ, int height, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue Height(this ControlQueue ctrlQ, int height, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue Height(this ControlQueue ctrlQ, int height, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue HeightInc(this ControlQueue ctrlQ, int height, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue HeightInc(this ControlQueue ctrlQ, int height, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue HeightInc(this ControlQueue ctrlQ, int height, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue HeightDec(this ControlQueue ctrlQ, int height, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue HeightDec(this ControlQueue ctrlQ, int height, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue HeightDec(this ControlQueue ctrlQ, int height, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Height", height, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        #endregion

        #region Width

        public static ControlQueue Width(this ControlQueue ctrlQ, int width, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue Width(this ControlQueue ctrlQ, int width, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue Width(this ControlQueue ctrlQ, int width)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue WidthInc(this ControlQueue ctrlQ, int width, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue WidthInc(this ControlQueue ctrlQ, int width, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue WidthInc(this ControlQueue ctrlQ, int width)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue WidthDec(this ControlQueue ctrlQ, int width, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue WidthDec(this ControlQueue ctrlQ, int width, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue WidthDec(this ControlQueue ctrlQ, int width)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }




        public static ControlQueue Width(this ControlQueue ctrlQ, int width, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue Width(this ControlQueue ctrlQ, int width, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue Width(this ControlQueue ctrlQ, int width, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue WidthInc(this ControlQueue ctrlQ, int width, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue WidthInc(this ControlQueue ctrlQ, int width, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue WidthInc(this ControlQueue ctrlQ, int width, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue WidthDec(this ControlQueue ctrlQ, int width, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue WidthDec(this ControlQueue ctrlQ, int width, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue WidthDec(this ControlQueue ctrlQ, int width, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, "Width", width, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
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
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            }
            return UnsuportedType();
        }

        public static ObjectQueue Change<T>(this ObjectQueue ctrlQ, string propName, T value, Speed speed)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue Change<T>(this ObjectQueue ctrlQ, string propName, T value)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeInc<T>(this ObjectQueue ctrlQ, string propName, T value, int time)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeInc<T>(this ObjectQueue ctrlQ, string propName, T value, Speed speed)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeInc<T>(this ObjectQueue ctrlQ, string propName, T value)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeDec<T>(this ObjectQueue ctrlQ, string propName, T value, int time)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeDec<T>(this ObjectQueue ctrlQ, string propName, T value, Speed speed)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeDec<T>(this ObjectQueue ctrlQ, string propName, T value)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }



        public static ObjectQueue Change<T>(this ObjectQueue ctrlQ, string propName, T value, int time, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue Change<T>(this ObjectQueue ctrlQ, string propName, T value, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue Change<T>(this ObjectQueue ctrlQ, string propName, T value, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeInc<T>(this ObjectQueue ctrlQ, string propName, T value, int time, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeInc<T>(this ObjectQueue ctrlQ, string propName, T value, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeInc<T>(this ObjectQueue ctrlQ, string propName, T value, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeDec<T>(this ObjectQueue ctrlQ, string propName, T value, int time, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeDec<T>(this ObjectQueue ctrlQ, string propName, T value, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        public static ObjectQueue ChangeDec<T>(this ObjectQueue ctrlQ, string propName, T value, AnimationControler.FinalCallback finalCallback)
        {
            if (typeof(T) == typeof(int))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeIntPropPacket(ctrlQ.QueueName, propName, (int)(object)value, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Color))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, propName, (Color)(object)value, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Point))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimePointPropPacket(ctrlQ.QueueName, propName, (Point)(object)value, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Size))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeSizePropPacket(ctrlQ.QueueName, propName, (Size)(object)value, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            if (typeof(T) == typeof(Rectangle))
            {
                ctrlQ.ProcessPacket(AnimationUtils.AnimeRectPropPacket(ctrlQ.QueueName, propName, (Rectangle)(object)value, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
                return ctrlQ;
            }
            return UnsuportedType();
        }

        #endregion

        #region Highlight

        public static ControlQueue Highlight(this ControlQueue ctrlQ, int highlightPercent, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightPacket(ctrlQ.QueueName, highlightPercent, time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Highlight(this ControlQueue ctrlQ, int highlightPercent, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightPacket(ctrlQ.QueueName, highlightPercent, AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue Highlight(this ControlQueue ctrlQ, int highlightPercent)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightPacket(ctrlQ.QueueName, highlightPercent, ctrlQ.time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }


        public static ControlQueue Highlight(this ControlQueue ctrlQ, int highlightPercent, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightPacket(ctrlQ.QueueName, highlightPercent, time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue Highlight(this ControlQueue ctrlQ, int highlightPercent, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightPacket(ctrlQ.QueueName, highlightPercent, AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue Highlight(this ControlQueue ctrlQ, int highlightPercent, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightPacket(ctrlQ.QueueName, highlightPercent, ctrlQ.time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        #endregion

        #region AnimeHighlightForecolor

        public static ControlQueue HighlightForecolor(this ControlQueue ctrlQ, int highlightPercent, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightForecolorPacket(ctrlQ.QueueName, highlightPercent, time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue HighlightForecolor(this ControlQueue ctrlQ, int highlightPercent, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightForecolorPacket(ctrlQ.QueueName, highlightPercent, AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }

        public static ControlQueue HighlightForecolor(this ControlQueue ctrlQ, int highlightPercent)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightForecolorPacket(ctrlQ.QueueName, highlightPercent, ctrlQ.time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null));
            return ctrlQ;
        }


        public static ControlQueue HighlightForecolor(this ControlQueue ctrlQ, int highlightPercent, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightForecolorPacket(ctrlQ.QueueName, highlightPercent, time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue HighlightForecolor(this ControlQueue ctrlQ, int highlightPercent, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightForecolorPacket(ctrlQ.QueueName, highlightPercent, AnimationControler.GetSpeedTime(speed), true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        public static ControlQueue HighlightForecolor(this ControlQueue ctrlQ, int highlightPercent, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeHighlightForecolorPacket(ctrlQ.QueueName, highlightPercent, ctrlQ.time, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback));
            return ctrlQ;
        }

        #endregion

        #region Color

        public static ControlQueue Color(this ControlQueue ctrlQ, Color color, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue Color(this ControlQueue ctrlQ, Color color, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue Color(this ControlQueue ctrlQ, Color color)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ColorInc(this ControlQueue ctrlQ, Color color, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ColorInc(this ControlQueue ctrlQ, Color color, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ColorInc(this ControlQueue ctrlQ, Color color)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ColorDec(this ControlQueue ctrlQ, Color color, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ColorDec(this ControlQueue ctrlQ, Color color, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ColorDec(this ControlQueue ctrlQ, Color color)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }



        public static ControlQueue Color(this ControlQueue ctrlQ, Color color, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue Color(this ControlQueue ctrlQ, Color color, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue Color(this ControlQueue ctrlQ, Color color, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ColorInc(this ControlQueue ctrlQ, Color color, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ColorInc(this ControlQueue ctrlQ, Color color, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ColorInc(this ControlQueue ctrlQ, Color color, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ColorDec(this ControlQueue ctrlQ, Color color, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ColorDec(this ControlQueue ctrlQ, Color color, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ColorDec(this ControlQueue ctrlQ, Color color, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "BackColor", color, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        #endregion

        #region AnimeForeColor

        public static ControlQueue ForeColor(this ControlQueue ctrlQ, Color color, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColor(this ControlQueue ctrlQ, Color color, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColor(this ControlQueue ctrlQ, Color color)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColorInc(this ControlQueue ctrlQ, Color color, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColorInc(this ControlQueue ctrlQ, Color color, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColorInc(this ControlQueue ctrlQ, Color color)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColorDec(this ControlQueue ctrlQ, Color color, int time)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColorDec(this ControlQueue ctrlQ, Color color, Speed speed)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColorDec(this ControlQueue ctrlQ, Color color)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, null, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }



        public static ControlQueue ForeColor(this ControlQueue ctrlQ, Color color, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColor(this ControlQueue ctrlQ, Color color, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, AnimationControler.GetSpeedTime(speed), ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColor(this ControlQueue ctrlQ, Color color, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, ctrlQ.time, ctrlQ.SpeedMode, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColorInc(this ControlQueue ctrlQ, Color color, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColorInc(this ControlQueue ctrlQ, Color color, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColorInc(this ControlQueue ctrlQ, Color color, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, ctrlQ.time, SpeedMode.Inc, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColorDec(this ControlQueue ctrlQ, Color color, int time, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColorDec(this ControlQueue ctrlQ, Color color, Speed speed, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, AnimationControler.GetSpeedTime(speed), SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        public static ControlQueue ForeColorDec(this ControlQueue ctrlQ, Color color, AnimationControler.FinalCallback finalCallback)
        {
            ctrlQ.ProcessPacket(AnimationUtils.AnimeColorPropPacket(ctrlQ.QueueName, "ForeColor", color, ctrlQ.time, SpeedMode.Dec, true, ctrlQ.queueLevel, ctrlQ.queueOwner, finalCallback, ctrlQ.CompleteIfCancel));
            return ctrlQ;
        }

        #endregion
    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AVLib.Animations
{
    public class ControlEventQueue : AnimationExternals.ControlQueue
    {
        private AnimeEvent animeEvent;
        public ControlEventQueue(Control ctrl, AnimeEvent animeEvent, int queueLevel, object queueOwner, bool isQueue)
            : base(ctrl, queueLevel, queueOwner, isQueue)
        {
            this.animeEvent = animeEvent;
        }

        private EventQueue eventQueue = null;
        private void InitQueue()
        {
            if (eventQueue != null) return;
            eventQueue = ControlController.GetEventThreads(ctrl, animeEvent).NewQueue();
            if (eventQueue.IsNew) ControlController.SetHandler(ctrl, animeEvent);
        }
        internal override void ProcessPacket(AnimationControler.AnimePacket packet)
        {
            InitQueue();
            eventQueue.Add(packet);
            if (isQueue) queueLevel++;
        }
    }

    public class AnimeCollectorEventQueue : AnimationExternals.ControlQueue
    {
        private AnimeCollector animeCollector;
        private AnimeEvent animeEvent;

        public AnimeCollectorEventQueue(AnimeCollector collector, AnimeEvent animeEvent, int queueLevel, object queueOwner, bool isQueue)
            : base(null, queueLevel, queueOwner, isQueue)
        {
            this.animeEvent = animeEvent;
            this.animeCollector = collector;
        }

        private EventQueue eventQueue = null;
        private void InitQueue()
        {
            if (eventQueue != null) return;
            eventQueue = ControlController.GetEventThreads(animeCollector, animeEvent).NewQueue();
        }
        internal override void ProcessPacket(AnimationControler.AnimePacket packet)
        {
            InitQueue();
            eventQueue.Add(packet);
            if (isQueue) queueLevel++;
        }
    }

    public class AnimeEventSelector
    {
        internal Control ctrl;
        internal bool isQueue;
        internal int time = AnimationControler.GetSpeedTime(Speed.Fast);
        internal Speed Speed { set { time = AnimationControler.GetSpeedTime(Speed.Fast); } }
        internal SpeedMode SpeedMode = SpeedMode.Normal;
        internal bool CompleteIfCancel = false;
        internal string QueueName = "";
        public AnimeEventSelector(Control ctrl, bool isQueue)
        {
            this.ctrl = ctrl;
            this.isQueue = isQueue;
        }
    }

    public class AnimeEventCollector
    {
        internal AnimeCollector collector;
        internal bool isQueue;
        internal int time = AnimationControler.GetSpeedTime(Speed.Fast);
        internal Speed Speed { set { time = AnimationControler.GetSpeedTime(Speed.Fast); } }
        internal SpeedMode SpeedMode = SpeedMode.Normal;
        internal bool CompleteIfCancel = false;
        internal string QueueName = "";
        public AnimeEventCollector(AnimeCollector collector, bool isQueue)
        {
            this.collector = collector;
            this.isQueue = isQueue;
        }
    }

    public static class AnimeControlExternals
    {
        #region Control

        public static AnimeEventSelector AnimeEvent(this Control control)
        {
            return new AnimeEventSelector(control, false);
        }

        public static AnimeEventSelector AnimeEvent(this Control control, string queueName)
        {
            return new AnimeEventSelector(control, false){QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, false){CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, string queueName, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, false) { CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, int time)
        {
            return new AnimeEventSelector(control, false) { time = time };
        }

        public static AnimeEventSelector AnimeEvent(this Control control, string queueName, int time)
        {
            return new AnimeEventSelector(control, false) { time = time, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, int time, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, false) { time = time, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, string queueName, int time, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, false) { time = time, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, int time, SpeedMode speedMode)
        {
            return new AnimeEventSelector(control, false) { time = time, SpeedMode = speedMode };
        }

        public static AnimeEventSelector AnimeEvent(this Control control, string queueName, int time, SpeedMode speedMode)
        {
            return new AnimeEventSelector(control, false) { time = time, SpeedMode = speedMode, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, false) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, string queueName, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, false) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, Speed speed)
        {
            return new AnimeEventSelector(control, false) { Speed = speed };
        }

        public static AnimeEventSelector AnimeEvent(this Control control, string queueName, Speed speed)
        {
            return new AnimeEventSelector(control, false) { Speed = speed, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, Speed speed, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, false) { Speed = speed, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, string queueName, Speed speed, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, false) { Speed = speed, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, Speed speed, SpeedMode speedMode)
        {
            return new AnimeEventSelector(control, false) { Speed = speed, SpeedMode = speedMode };
        }

        public static AnimeEventSelector AnimeEvent(this Control control, string queueName, Speed speed, SpeedMode speedMode)
        {
            return new AnimeEventSelector(control, false) { Speed = speed, SpeedMode = speedMode, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, false) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventSelector AnimeEvent(this Control control, string queueName, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, false) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control)
        {
            return new AnimeEventSelector(control, true);
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, string queueName)
        {
            return new AnimeEventSelector(control, true){QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, true){CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, string queueName, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, true) { CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, int time)
        {
            return new AnimeEventSelector(control, true) { time = time };
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, string queueName, int time)
        {
            return new AnimeEventSelector(control, true) { time = time, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, int time, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, true) { time = time, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, string queueName, int time, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, true) { time = time, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, int time, SpeedMode speedMode)
        {
            return new AnimeEventSelector(control, true) { time = time, SpeedMode = speedMode };
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, string queueName, int time, SpeedMode speedMode)
        {
            return new AnimeEventSelector(control, true) { time = time, SpeedMode = speedMode, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, true) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, string queueName, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, true) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, Speed speed)
        {
            return new AnimeEventSelector(control, true) { Speed = speed };
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, string queueName, Speed speed)
        {
            return new AnimeEventSelector(control, true) { Speed = speed, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, Speed speed, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, true) { Speed = speed, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, string queueName, Speed speed, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, true) { Speed = speed, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, Speed speed, SpeedMode speedMode)
        {
            return new AnimeEventSelector(control, true) { Speed = speed, SpeedMode = speedMode };
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, string queueName, Speed speed, SpeedMode speedMode)
        {
            return new AnimeEventSelector(control, true) { Speed = speed, SpeedMode = speedMode, QueueName = queueName};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, true) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventSelector AnimeEventQueue(this Control control, string queueName, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventSelector(control, true) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }




        public static ControlEventQueue MouseEnter(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.MouseEnter, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName};
        }

        public static ControlEventQueue MouseLeave(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.MouseLeave, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue Click(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.Click, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue MouseDown(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.MouseDonw, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue MouseUp(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.MouseUp, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue DoubleClick(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.DoubleClick, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue DragEnter(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.DragEnter, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue DragLeave(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.DragLeave, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue DragDrop(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.DragDrop, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue Enter(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.Enter, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue Leave(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.Leave, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue GotFocus(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.GotFocus, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue KeyDown(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.KeyDown, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue KeyUp(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.KeyUp, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue LostFocus(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.LostFocus, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue MouseWheel(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.MouseWheel, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue TextChanged(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.TextChanged, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue Visible(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.Visible, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue Enabled(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.Enabled, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        public static ControlEventQueue Disabled(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.Disabled, -1, null, selector.isQueue) { time = selector.time, SpeedMode = selector.SpeedMode, CompleteIfCancel = selector.CompleteIfCancel, QueueName = selector.QueueName };
        }

        #endregion

        #region Collector

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector)
        {
            return new AnimeEventCollector(collector, false);
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, string queueName)
        {
            return new AnimeEventCollector(collector, false){QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, false){CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, string queueName, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, false) { CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, int time)
        {
            return new AnimeEventCollector(collector, false) { time = time };
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, string queueName, int time)
        {
            return new AnimeEventCollector(collector, false) { time = time, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, int time, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, false) { time = time, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, string queueName, int time, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, false) { time = time, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, int time, SpeedMode speedMode)
        {
            return new AnimeEventCollector(collector, false) { time = time, SpeedMode = speedMode };
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, string queueName, int time, SpeedMode speedMode)
        {
            return new AnimeEventCollector(collector, false) { time = time, SpeedMode = speedMode, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, false) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, string queueName, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, false) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, Speed speed)
        {
            return new AnimeEventCollector(collector, false) { Speed = speed };
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, string queueName, Speed speed)
        {
            return new AnimeEventCollector(collector, false) { Speed = speed, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, Speed speed, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, false) { Speed = speed, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, string queueName, Speed speed, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, false) { Speed = speed, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, Speed speed, SpeedMode speedMode)
        {
            return new AnimeEventCollector(collector, false) { Speed = speed, SpeedMode = speedMode };
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, string queueName, Speed speed, SpeedMode speedMode)
        {
            return new AnimeEventCollector(collector, false) { Speed = speed, SpeedMode = speedMode, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, false) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector, string queueName, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, false) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector)
        {
            return new AnimeEventCollector(collector, true);
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, string queueName)
        {
            return new AnimeEventCollector(collector, true){QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, true){CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, string queueName, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, true) { CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, int time)
        {
            return new AnimeEventCollector(collector, true) { time = time };
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, string queueName, int time)
        {
            return new AnimeEventCollector(collector, true) { time = time, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, int time, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, true) { time = time, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, string queueName, int time, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, true) { time = time, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, int time, SpeedMode speedMode)
        {
            return new AnimeEventCollector(collector, true) { time = time, SpeedMode = speedMode };
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, string queueName, int time, SpeedMode speedMode)
        {
            return new AnimeEventCollector(collector, true) { time = time, SpeedMode = speedMode, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, true) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, string queueName, int time, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, true) { time = time, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, Speed speed)
        {
            return new AnimeEventCollector(collector, true) { Speed = speed };
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, string queueName, Speed speed)
        {
            return new AnimeEventCollector(collector, true) { Speed = speed, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, Speed speed, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, true) { Speed = speed, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, string queueName, Speed speed, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, true) { Speed = speed, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, Speed speed, SpeedMode speedMode)
        {
            return new AnimeEventCollector(collector, true) { Speed = speed, SpeedMode = speedMode };
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, string queueName, Speed speed, SpeedMode speedMode)
        {
            return new AnimeEventCollector(collector, true) { Speed = speed, SpeedMode = speedMode, QueueName = queueName};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, true) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel};
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector, string queueName, Speed speed, SpeedMode speedMode, bool CompleteIfCancel)
        {
            return new AnimeEventCollector(collector, true) { Speed = speed, SpeedMode = speedMode, CompleteIfCancel = CompleteIfCancel, QueueName = queueName};
        }




        public static AnimeCollectorEventQueue MouseEnter(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.MouseEnter, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName};
        }

        public static AnimeCollectorEventQueue MouseLeave(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.MouseLeave, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue Click(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.Click, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue MouseDown(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.MouseDonw, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue MouseUp(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.MouseUp, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue DoubleClick(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.DoubleClick, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue DragEnter(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.DragEnter, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue DragLeave(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.DragLeave, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue DragDrop(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.DragDrop, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue Enter(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.Enter, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue Leave(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.Leave, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue GotFocus(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.GotFocus, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue KeyDown(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.KeyDown, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue KeyUp(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.KeyUp, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue LostFocus(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.LostFocus, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue MouseWheel(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.MouseWheel, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue TextChanged(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.TextChanged, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue Visible(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.Visible, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue Enabled(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.Enabled, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        public static AnimeCollectorEventQueue Disabled(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.Disabled, -1, null, collector.isQueue) { time = collector.time, SpeedMode = collector.SpeedMode, CompleteIfCancel = collector.CompleteIfCancel, QueueName = collector.QueueName };
        }

        #endregion
    }
}

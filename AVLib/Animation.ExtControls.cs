using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using AVLib.Animations;

namespace AVLib.Animations
{
    public enum AnimeEvent
    {
        None,
        Click,
        DoubleClick,
        MouseEnter,
        MouseLeave,
        MouseDonw,
        MouseUp,
        DragEnter,
        DragLeave,
        DragDrop,
        Enter,
        Leave,
        GotFocus,
        LostFocus,
        KeyDown,
        KeyUp,
        MouseWheel,
        TextChanged,
        Visible,
        Enabled,
        Disabled
    }

    public class ControlEventQueue : AnimationExternals.ControlQueue
    {
        private AnimeEvent animeEvent;
        public ControlEventQueue(Control ctrl, AnimeEvent animeEvent, int queueLevel, object queueOwner, bool isQueue) : base(ctrl, queueLevel, queueOwner, isQueue)
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

    public class AnimeCollector
    {
        public void ApplyToControl(Control ctrl)
        {
            ControlController.ApplyToControl(ctrl, this);
        }

        public void ApplyToControl(IEnumerable<Control> controls)
        {
            foreach (var ctrl in controls)
                ApplyToControl(ctrl);
        }
    }

    internal class EventQueue : ICloneable
    {
        private bool isNew = false;
        List<AnimationControler.AnimePacket> packetQueue = new List<AnimationControler.AnimePacket>();
        internal List<AnimationControler.AnimePacket> PacketQueue { get { return packetQueue; } }

        public EventQueue(bool isNew)
        {
            this.isNew = isNew;
        }
        public bool IsNew { get { return isNew; } }
        
        public void Add(AnimationControler.AnimePacket packet)
        {
            packetQueue.Add(packet);
        }

        public void Execute(Control control)
        {
            object owner = null;
            for (int i = 0; i < packetQueue.Count; i++)
            {
                var p = packetQueue[i];
                p.queueOwner = owner;
                owner = AnimationControler.ProcessPacket(control, p);
                p.queueOwner = null;
            }
        }

        public object Clone()
        {
            var clon = new EventQueue(isNew);
            for (int i = 0; i < packetQueue.Count; i++)
                clon.Add((AnimationControler.AnimePacket)packetQueue[i].Clone());
            return clon;
        }
    }

    internal class EventQueueThreads : ICloneable
    {
        private List<EventQueue> eventsQueue = new List<EventQueue>();
        internal List<EventQueue> EventsQueue { get { return eventsQueue; } }

        public void Execute(Control control)
        {
            control.AnimeCancel();
            for (int i = 0; i < eventsQueue.Count; i++)
                eventsQueue[i].Execute(control);
        }

        public EventQueue NewQueue()
        {
            EventQueue res = new EventQueue(eventsQueue.Count == 0);
            eventsQueue.Add(res);
            return res;
        }

        public void Add(EventQueue queue)
        {
            eventsQueue.Add(queue);
        }

        public object Clone()
        {
            var clon = new EventQueueThreads();
            for (int i = 0; i < eventsQueue.Count; i++)
                clon.Add((EventQueue) eventsQueue[i].Clone());
            return clon;
        }
    }

    internal class ControllData : ICloneable
    {
        internal bool DisposeAttached = false;

        private Dictionary<AnimeEvent, EventQueueThreads> eventThreads = new Dictionary<AnimeEvent, EventQueueThreads>();
        internal Dictionary<AnimeEvent, EventQueueThreads> EventThreads { get { return eventThreads; } }

        public EventQueueThreads FindEventQueueThreads(AnimeEvent animeEvent)
        {
            if (eventThreads.ContainsKey(animeEvent))
                return eventThreads[animeEvent];
            return null;
        }

        public EventQueueThreads GetEventQueueThreads(AnimeEvent animeEvent)
        {
            var res = FindEventQueueThreads(animeEvent);
            if (res == null)
            {
                res = new EventQueueThreads();
                eventThreads.Add(animeEvent, res);
            }
            return res;
        }

        public object Clone()
        {
            var clon = new ControllData();
            foreach (var pair in EventThreads)
                clon.eventThreads.Add(pair.Key, (EventQueueThreads) pair.Value.Clone());
            return clon;
        }
    }

    internal static class ControlController
    {
        private static Dictionary<object, ControllData> controlsData = new Dictionary<object, ControllData>();
        private static Dictionary<AnimeCollector, ControllData> animeCollectors = new Dictionary<AnimeCollector, ControllData>();

        private static ControllData FindControlData(object control)
        {
            if (controlsData.ContainsKey(control))
                return controlsData[control];
            return null;
        }

        private static ControllData FindCollectorData(AnimeCollector collector)
        {
            if (animeCollectors.ContainsKey(collector))
                return animeCollectors[collector];
            return null;
        }

        public static ControllData GetControlData(object control)
        {
            lock (controlsData)
            {
                var res = FindControlData(control);
                if (res == null)
                {
                    res = new ControllData();
                    controlsData.Add(control, res);
                }
                return res;
            }
        }

        public static ControllData GetCollectorData(AnimeCollector collector)
        {
            lock (animeCollectors)
            {
                var res = FindCollectorData(collector);
                if (res == null)
                {
                    res = new ControllData();
                    animeCollectors.Add(collector, res);
                }
                return res;
            }
        }

        public static EventQueueThreads FindEventThreads(Control control, AnimeEvent animeEvent)
        {
            lock (controlsData)
            {
                var cData = FindControlData(control);
                if (cData != null)
                    return cData.FindEventQueueThreads(animeEvent);
                return null;
            }
        }

        public static EventQueueThreads FindEventThreads(AnimeCollector collector, AnimeEvent animeEvent)
        {
            lock (animeCollectors)
            {
                var cData = FindCollectorData(collector);
                if (cData != null)
                    return cData.FindEventQueueThreads(animeEvent);
                return null;
            }
        }

        public static EventQueueThreads GetEventThreads(object control, AnimeEvent animeEvent)
        {
            lock (controlsData)
            {
                var cd = GetControlData(control);
                if (!cd.DisposeAttached)
                {
                    if (control is IComponent) ((IComponent)control).Disposed += ctrlDisposed;
                    cd.DisposeAttached = true;
                }
                return cd.GetEventQueueThreads(animeEvent);
            }
        }

        public static EventQueueThreads GetEventThreads(AnimeCollector collector, AnimeEvent animeEvent)
        {
            lock (animeCollectors)
            {
                var cd = GetCollectorData(collector);
                return cd.GetEventQueueThreads(animeEvent);
            }
        }

        static void ctrlDisposed(object sender, EventArgs e)
        {
            lock (controlsData)
            {
                if (controlsData.ContainsKey((Control)sender))
                    controlsData.Remove((Control) sender);
            }
        }

        public static void Execute(Control control, AnimeEvent animeEvent)
        {
            lock (controlsData)
            {
                var cData = FindControlData(control);
                if (cData != null)
                {
                    var eThread = cData.FindEventQueueThreads(animeEvent);
                    if (eThread != null) eThread.Execute(control);
                }
            }
        }

        internal static void ApplyToControl(Control control, AnimeCollector collector)
        {
            if (control == null || collector == null) return;
            ControllData dataClone = null;
            lock (animeCollectors)
            {
                var cd = FindCollectorData(collector);
                if (cd != null) dataClone = (ControllData) cd.Clone();
            }
            if (dataClone == null) return;

            foreach (var pair in dataClone.EventThreads)
            {
                foreach (var queue in pair.Value.EventsQueue)
                {
                    var eventQueue = GetEventThreads(control, pair.Key).NewQueue();
                    if (eventQueue.IsNew) SetHandler(control, pair.Key);
                    for (int i = 0; i < queue.PacketQueue.Count; i++)
                    {
                        eventQueue.Add(queue.PacketQueue[i]);
                    }
                } 
            }
        }

        #region Events handlers

        internal static void SetHandler(object control, AnimeEvent animeEvent)
        {
            if (control is Control)
                SetHandler((Control)control, animeEvent);
        }

        internal static void SetHandler(Control control, AnimeEvent animeEvent)
        {
            switch (animeEvent)
            {
                case AnimeEvent.MouseEnter:
                    control.MouseEnter += ctrlMouseEnter;
                    break;
                case AnimeEvent.MouseLeave:
                    control.MouseLeave += ctrlMouseLeave;
                    break;
                case AnimeEvent.Click:
                    control.Click += ctrlClick;
                    break;
                case AnimeEvent.MouseDonw:
                    control.MouseDown += ctrlMouseDown;
                    break;
                case AnimeEvent.MouseUp:
                    control.MouseUp += ctrlMouseUp;
                    break;
                case AnimeEvent.DoubleClick:
                    control.DoubleClick += ctrlDoubleClick;
                    break;
                case AnimeEvent.DragEnter:
                    control.DragEnter += ctrlDragEnter;
                    break;
                case AnimeEvent.DragLeave:
                    control.DragLeave += ctrlDragLeave;
                    break;
                case AnimeEvent.DragDrop:
                    control.DragDrop += ctrlDragDrop;
                    break;
                case AnimeEvent.Enter:
                    control.Enter += ctrlEnter;
                    break;
                case AnimeEvent.Leave:
                    control.Leave += ctrlLeave;
                    break;
                case AnimeEvent.GotFocus:
                    control.GotFocus += ctrlGotFocus;
                    break;
                case AnimeEvent.LostFocus:
                    control.LostFocus += ctrlLostFocus;
                    break;
                case AnimeEvent.KeyDown:
                    control.KeyDown += ctrlKeyDown;
                    break;
                case AnimeEvent.KeyUp:
                    control.KeyUp += ctrlKeyUp;
                    break;
                case AnimeEvent.MouseWheel:
                    control.MouseWheel += ctrlMouseWheel;
                    break;
                case AnimeEvent.TextChanged:
                    control.TextChanged += ctrlTextChanged;
                    break;
                case AnimeEvent.Visible:
                    control.VisibleChanged += ctrlVisibleChanged;
                    break;
                case AnimeEvent.Enabled:
                case AnimeEvent.Disabled:
                    control.EnabledChanged += ctrlEnabledChanged;
                    break;
            }
        }

        private static void ctrlEnabledChanged(object sender, EventArgs e)
        {
            if (((Control) sender).Enabled)
                Execute((Control) sender, AnimeEvent.Enabled);
            else
                Execute((Control) sender, AnimeEvent.Disabled);
        }

        private static void ctrlVisibleChanged(object sender, EventArgs e)
        {
            if (((Control)sender).Visible)
                Execute((Control)sender, AnimeEvent.Visible);
        }

        private static void ctrlTextChanged(object sender, EventArgs e)
        {
            Execute((Control)sender, AnimeEvent.TextChanged);
        }

        private static void ctrlMouseWheel(object sender, MouseEventArgs e)
        {
            Execute((Control)sender, AnimeEvent.MouseWheel);
        }

        private static void ctrlLostFocus(object sender, EventArgs e)
        {
            Execute((Control)sender, AnimeEvent.LostFocus);
        }

        private static void ctrlKeyUp(object sender, KeyEventArgs e)
        {
            Execute((Control)sender, AnimeEvent.KeyUp);
        }

        private static void ctrlKeyDown(object sender, KeyEventArgs e)
        {
            Execute((Control)sender, AnimeEvent.KeyDown);
        }

        private static void ctrlGotFocus(object sender, EventArgs e)
        {
            Execute((Control)sender, AnimeEvent.GotFocus);
        }

        private static void ctrlLeave(object sender, EventArgs e)
        {
            Execute((Control)sender, AnimeEvent.Leave);
        }

        private static void ctrlEnter(object sender, EventArgs e)
        {
            Execute((Control)sender, AnimeEvent.Enter);
        }

        private static void ctrlDragDrop(object sender, DragEventArgs e)
        {
            Execute((Control)sender, AnimeEvent.DragDrop);
        }

        private static void ctrlDragLeave(object sender, EventArgs e)
        {
            Execute((Control)sender, AnimeEvent.DragLeave);
        }

        private static void ctrlDragEnter(object sender, DragEventArgs e)
        {
            Execute((Control)sender, AnimeEvent.DragEnter);
        }

        private static void ctrlDoubleClick(object sender, EventArgs e)
        {
            Execute((Control)sender, AnimeEvent.DoubleClick);
        }

        private static void ctrlMouseUp(object sender, MouseEventArgs e)
        {
            Execute((Control)sender, AnimeEvent.MouseUp);
        }

        private static void ctrlMouseDown(object sender, MouseEventArgs e)
        {
            Execute((Control)sender, AnimeEvent.MouseDonw);
        }

        private static void ctrlMouseEnter(object sender, EventArgs args)
        {
            Execute((Control)sender, AnimeEvent.MouseEnter);
        }

        private static void ctrlMouseLeave(object sender, EventArgs args)
        {
            Execute((Control)sender, AnimeEvent.MouseLeave);
        }

        private static void ctrlClick(object sender, EventArgs args)
        {
            Execute((Control)sender, AnimeEvent.Click);
        }

        #endregion
    }

    public class AnimeEventSelector
    {
        internal Control ctrl;
        internal bool isQueue;
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

        public static AnimeEventSelector AnimeEventQueue(this Control control)
        {
            return new AnimeEventSelector(control, true);
        }

        public static ControlEventQueue MouseEnter(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.MouseEnter, -1, null, selector.isQueue);
        }

        public static ControlEventQueue MouseLeave(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.MouseLeave, -1, null, selector.isQueue);
        }

        public static ControlEventQueue Click(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.Click, -1, null, selector.isQueue);
        }

        public static ControlEventQueue MouseDown(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.MouseDonw, -1, null, selector.isQueue);
        }

        public static ControlEventQueue MouseUp(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.MouseUp, -1, null, selector.isQueue);
        }

        public static ControlEventQueue DoubleClick(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.DoubleClick, -1, null, selector.isQueue);
        }

        public static ControlEventQueue DragEnter(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.DragEnter, -1, null, selector.isQueue);
        }

        public static ControlEventQueue DragLeave(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.DragLeave, -1, null, selector.isQueue);
        }

        public static ControlEventQueue DragDrop(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.DragDrop, -1, null, selector.isQueue);
        }

        public static ControlEventQueue Enter(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.Enter, -1, null, selector.isQueue);
        }

        public static ControlEventQueue Leave(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.Leave, -1, null, selector.isQueue);
        }

        public static ControlEventQueue GotFocus(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.GotFocus, -1, null, selector.isQueue);
        }

        public static ControlEventQueue KeyDown(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.KeyDown, -1, null, selector.isQueue);
        }

        public static ControlEventQueue KeyUp(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.KeyUp, -1, null, selector.isQueue);
        }

        public static ControlEventQueue LostFocus(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.LostFocus, -1, null, selector.isQueue);
        }

        public static ControlEventQueue MouseWheel(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.MouseWheel, -1, null, selector.isQueue);
        }

        public static ControlEventQueue TextChanged(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.TextChanged, -1, null, selector.isQueue);
        }

        public static ControlEventQueue Visible(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.Visible, -1, null, selector.isQueue);
        }

        public static ControlEventQueue Enabled(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.Enabled, -1, null, selector.isQueue);
        }

        public static ControlEventQueue Disabled(this AnimeEventSelector selector)
        {
            return new ControlEventQueue(selector.ctrl, Animations.AnimeEvent.Disabled, -1, null, selector.isQueue);
        }

        #endregion

        #region Collector

        public static AnimeEventCollector AnimeEvent(this AnimeCollector collector)
        {
            return new AnimeEventCollector(collector, false);
        }

        public static AnimeEventCollector AnimeEventQueue(this AnimeCollector collector)
        {
            return new AnimeEventCollector(collector, true);
        }

        public static AnimeCollectorEventQueue MouseEnter(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.MouseEnter, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue MouseLeave(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.MouseLeave, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue Click(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.Click, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue MouseDown(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.MouseDonw, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue MouseUp(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.MouseUp, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue DoubleClick(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.DoubleClick, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue DragEnter(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.DragEnter, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue DragLeave(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.DragLeave, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue DragDrop(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.DragDrop, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue Enter(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.Enter, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue Leave(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.Leave, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue GotFocus(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.GotFocus, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue KeyDown(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.KeyDown, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue KeyUp(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.KeyUp, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue LostFocus(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.LostFocus, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue MouseWheel(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.MouseWheel, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue TextChanged(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.TextChanged, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue Visible(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.Visible, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue Enabled(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.Enabled, -1, null, collector.isQueue);
        }

        public static AnimeCollectorEventQueue Disabled(this AnimeEventCollector collector)
        {
            return new AnimeCollectorEventQueue(collector.collector, Animations.AnimeEvent.Disabled, -1, null, collector.isQueue);
        }

        #endregion
    }
}
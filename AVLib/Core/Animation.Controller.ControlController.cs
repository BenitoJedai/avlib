using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using AVLib.Animations;
using AVLib.Draw.DrawRects;

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

        public void Execute(object control)
        {
            object owner = null;
            for (int i = 0; i < packetQueue.Count; i++)
            {
                var p = packetQueue[i];
                p.queueOwner = owner;
                control.AnimeCancel(p.threadParam.queueName);
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

        public void Execute(object control)
        {
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

    public static class ControlController
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

        internal static ControllData GetControlData(object control)
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

        internal static ControllData GetCollectorData(AnimeCollector collector)
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

        internal static EventQueueThreads FindEventThreads(Control control, AnimeEvent animeEvent)
        {
            lock (controlsData)
            {
                var cData = FindControlData(control);
                if (cData != null)
                    return cData.FindEventQueueThreads(animeEvent);
                return null;
            }
        }

        internal static EventQueueThreads FindEventThreads(AnimeCollector collector, AnimeEvent animeEvent)
        {
            lock (animeCollectors)
            {
                var cData = FindCollectorData(collector);
                if (cData != null)
                    return cData.FindEventQueueThreads(animeEvent);
                return null;
            }
        }

        internal static EventQueueThreads GetEventThreads(object control, AnimeEvent animeEvent)
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

        internal static EventQueueThreads GetEventThreads(AnimeCollector collector, AnimeEvent animeEvent)
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

        public static void RemoveControl(object control)
        {
            lock (controlsData)
            {
                if (controlsData.ContainsKey(control))
                    controlsData.Remove(control);
            }
        }

        public static void Execute(object control, AnimeEvent animeEvent)
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

        internal static void ApplyToControl(object control, AnimeCollector collector)
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
            {
                SetHandler((Control) control, animeEvent);
                return;
            }
            if (control is ControlRect)
            {
                SetHandler((ControlRect) control, animeEvent);
                return;
            }
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

        internal static void SetHandler(ControlRect rect, AnimeEvent animeEvent)
        {
            switch (animeEvent)
            {
                case AnimeEvent.MouseEnter:
                    rect.MouseEnter += ctrlMouseEnter;
                    break;
                case AnimeEvent.MouseLeave:
                    rect.MouseLeave += ctrlMouseLeave;
                    break;
                case AnimeEvent.Click:
                    rect.Click += ctrlClick;
                    break;
                case AnimeEvent.MouseDonw:
                    rect.MouseDown += ctrlMouseDown;
                    break;
                case AnimeEvent.MouseUp:
                    rect.MouseUp += ctrlMouseUp;
                    break;
                case AnimeEvent.DoubleClick:
                    rect.DoubleClick += ctrlDoubleClick;
                    break;
                case AnimeEvent.DragEnter:
                    rect.DragEnter += ctrlDragEnter;
                    break;
                case AnimeEvent.DragLeave:
                    rect.DragLeave += ctrlDragLeave;
                    break;
                case AnimeEvent.DragDrop:
                    rect.DragDrop += ctrlDragDrop;
                    break;
                case AnimeEvent.Enter:
                case AnimeEvent.GotFocus:
                    rect.Enter += ctrlEnter;
                    break;
                case AnimeEvent.Leave:
                case AnimeEvent.LostFocus:
                    rect.Leave += ctrlLeave;
                    break;
                case AnimeEvent.KeyDown:
                    rect.KeyDown += ctrlKeyDown;
                    break;
                case AnimeEvent.KeyUp:
                    rect.KeyUp += ctrlKeyUp;
                    break;
                case AnimeEvent.MouseWheel:
                    rect.MouseWheel += ctrlMouseWheel;
                    break;
                case AnimeEvent.TextChanged:
                    //rect.TextChanged += ctrlTextChanged;
                    break;
                case AnimeEvent.Visible:
                    rect.VisibleChanged += ctrlVisibleChanged;
                    break;
                case AnimeEvent.Enabled:
                case AnimeEvent.Disabled:
                    rect.EnabledChanged += ctrlEnabledChanged;
                    break;
            }
        }

        private static void ctrlEnabledChanged(object sender, EventArgs e)
        {
            if (sender is Control)
            {
                if (((Control) sender).Enabled)
                    Execute((Control) sender, AnimeEvent.Enabled);
                else
                    Execute((Control) sender, AnimeEvent.Disabled);
            }
        }

        private static void ctrlVisibleChanged(object sender, EventArgs e)
        {
            if (sender is Control)
            {
                if (((Control) sender).Visible)
                    Execute(sender, AnimeEvent.Visible);
                return;
            }
            if (sender is ControlRect)
            {
                if (((ControlRect) sender).Visible)
                    Execute(sender, AnimeEvent.Visible);
            }
        }

        private static void ctrlTextChanged(object sender, EventArgs e)
        {
            Execute(sender, AnimeEvent.TextChanged);
        }

        private static void ctrlMouseWheel(object sender, MouseEventArgs e)
        {
            Execute(sender, AnimeEvent.MouseWheel);
        }

        private static void ctrlLostFocus(object sender, EventArgs e)
        {
            Execute(sender, AnimeEvent.LostFocus);
        }

        private static void ctrlKeyUp(object sender, KeyEventArgs e)
        {
            Execute(sender, AnimeEvent.KeyUp);
        }

        private static void ctrlKeyDown(object sender, KeyEventArgs e)
        {
            Execute(sender, AnimeEvent.KeyDown);
        }

        private static void ctrlGotFocus(object sender, EventArgs e)
        {
            Execute(sender, AnimeEvent.GotFocus);
        }

        private static void ctrlLeave(object sender, EventArgs e)
        {
            Execute(sender, AnimeEvent.Leave);
        }

        private static void ctrlEnter(object sender, EventArgs e)
        {
            Execute(sender, AnimeEvent.Enter);
        }

        private static void ctrlDragDrop(object sender, DragEventArgs e)
        {
            Execute(sender, AnimeEvent.DragDrop);
        }

        private static void ctrlDragLeave(object sender, EventArgs e)
        {
            Execute(sender, AnimeEvent.DragLeave);
        }

        private static void ctrlDragEnter(object sender, DragEventArgs e)
        {
            Execute(sender, AnimeEvent.DragEnter);
        }

        private static void ctrlDoubleClick(object sender, EventArgs e)
        {
            Execute(sender, AnimeEvent.DoubleClick);
        }

        private static void ctrlMouseUp(object sender, MouseEventArgs e)
        {
            Execute(sender, AnimeEvent.MouseUp);
        }

        private static void ctrlMouseDown(object sender, MouseEventArgs e)
        {
            Execute(sender, AnimeEvent.MouseDonw);
        }

        private static void ctrlMouseEnter(object sender, EventArgs args)
        {
            Execute(sender, AnimeEvent.MouseEnter);
        }

        private static void ctrlMouseLeave(object sender, EventArgs args)
        {
            Execute(sender, AnimeEvent.MouseLeave);
        }

        private static void ctrlClick(object sender, EventArgs args)
        {
            Execute(sender, AnimeEvent.Click);
        }

        #endregion
    }
}
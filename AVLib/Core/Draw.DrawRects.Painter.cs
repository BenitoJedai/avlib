using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AVLib.Animations;
using AVLib.Utils;
using VALib.Draw.Controls;

namespace AVLib.Draw.DrawRects
{
    public interface IRectPainter
    {
        string Name { get; set; }
        event ChangedHandler Changed;
        void DoChange();
        void Paint(DrawRect rect, Graphics graf);
        bool InvokeRequired { get; }
        object Invoke(Delegate method, params Object[] args);
        object Invoke(Delegate method);
        bool Enabled { get; set; }
    }

    public delegate void RectPaintHandler(IControlPropertiesValue Params, DrawRect rect, Graphics graf);
    public abstract class RectPainter : IRectPainter, IInvokeCompatible
    {
        #region Internal

        internal Control control;

        public bool InvokeRequired
        {
            get { return (control != null && control.InvokeRequired); }
        }

        public object Invoke(Delegate method, params Object[] args)
        {
            if (control != null)
                return control.Invoke(method, args);

            return method.DynamicInvoke(method, args);
        }

        public object Invoke(Delegate method)
        {
            if (control != null)
                return control.Invoke(method);

            return method.DynamicInvoke(method);
        }

        private ControlProperties m_properties = new ControlProperties();
        public IControlProperties Property
        {
            get { return m_properties; }
        }
        public IControlPropertiesValue Value
        {
            get { return m_properties; }
        }

        public event ChangedHandler Changed;
        public void DoChange()
        {
            if (Changed != null) Changed();
        }

        #endregion

        public string Name
        {
            get { return Value["Name"].AsString(); } 
            set { Value["Name"] = value; }
        }

        public bool Enabled
        {
            get { return Value["Enabled"].AsBoolean(); }
            set { Value["Enabled"] = value; }
        }

        public RectPainter()
        {
            Property["Enabled", true].Changed += () => { DoChange(); };
        }

        public abstract void Paint(DrawRect rect, Graphics graf);
    }

    public delegate void PainterPaintHandler(DrawRect rect, Graphics graf);
    public class CustomPainter : RectPainter
    {
        private PainterPaintHandler paintHandler;
        public CustomPainter(PainterPaintHandler paintHandler, string name)
        {
            this.paintHandler = paintHandler;
            this.Name = name;
        }
        public override void Paint(DrawRect rect, Graphics graf)
        {
            if (paintHandler != null) paintHandler(rect, graf);
        }
    }

    public class RectPainters
    {
        internal Control control;

        public event ChangedHandler Changed;

        private int m_disable_change = 0;
        private bool m_need_change = false;

        private void DisableChange()
        {
            m_disable_change++;
        }
        private void EnableChange()
        {
            m_disable_change--;
            if (m_disable_change == 0 && m_need_change)
            {
                m_need_change = false;
                DoChanged();
            }
        }

        private void DoChanged()
        {
            if (Changed != null)
            {
                if (m_disable_change == 0)
                    Changed();
                else
                    m_need_change = true;
            }
        }

        public class PaintLevel
        {
            private RectPainters m_owner;
            private List<IRectPainter> m_handlers = new List<IRectPainter>();
            public PaintLevel(RectPainters owner)
            {
                m_owner = owner;
            }
            public void Paint(DrawRect rect, Graphics graf)
            {
                foreach (var h in m_handlers)
                    h.Paint(rect, graf);
            }
            public void Add(RectPainter painter)
            {
                painter.control = m_owner.control;
                m_handlers.Add(painter);
                painter.Changed += painter_Changed;
                m_owner.DoChanged();
            }
            public void Add(PainterPaintHandler method, string name)
            {
                var painter = new CustomPainter(method, name);
                Add(painter);
            }
            public void Add(PainterPaintHandler method)
            {
                Add(method, "");
            }

            private void painter_Changed()
            {
                m_owner.DoChanged();
            }
            public void Clear()
            {
                foreach (var painter in m_handlers)
                    painter.Changed -= painter_Changed;
                m_handlers.Clear();
            }
            public int Count
            {
                get { return m_handlers.Count; }
            }
            public IRectPainter this[string name]
            {
                get
                {
                    if (string.IsNullOrEmpty(name)) return null;
                    var s = name.ToLower();
                    foreach (var h in m_handlers)
                    {
                        if (h.Name.ToLower() == s)
                            return h;
                    }
                    return null;
                }
            }
        }

        private List<PaintLevel> m_PaintLevels = new List<PaintLevel>();

        public int LevelsCount
        {
            get { return m_PaintLevels.Count; }
            set { ClearFromLevel(Math.Max(0, value)); }
        }

        public void Clear()
        {
            ClearFromLevel(0);
        }

        private void ClearFromLevel(int level)
        {
            DisableChange();
            for (int i = m_PaintLevels.Count - 1; i >= level; i--)
            {
                m_PaintLevels[i].Clear();
                m_PaintLevels.RemoveAt(i);
            }
            EnableChange();
        }

        private void EnsureLevel(int level)
        {
            while (m_PaintLevels.Count < level + 1)
                m_PaintLevels.Add(new PaintLevel(this));
        }

        public void Paint(DrawRect rect, Graphics graf)
        {
            foreach (var level in m_PaintLevels)
                level.Paint(rect, graf);
        }

        public PaintLevel this[int Index]
        {
            get
            {
                EnsureLevel(Index);
                return m_PaintLevels[Index];
            }
        }

        public IRectPainter this[string name]
        {
            get
            {
                IRectPainter res = null;
                foreach (var l in m_PaintLevels)
                {
                    res = l[name];
                    if (res != null) return res;
                }
                return null;
            }
        }
    }
}

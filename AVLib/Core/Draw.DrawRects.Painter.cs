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
    public delegate void RectPaintHandler(IControlPropertiesValue Params, DrawRect rect, Graphics graf);
    public class RectPainter    {

        private ControlProperties m_properties = new ControlProperties();
        public IControlProperties Property
        {
            get { return m_properties; }
        }
        public IControlPropertiesValue Value
        {
            get { return m_properties; }
        }

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

        public bool UseRectProperties
        {
            get { return Value["UseRectProperties"].AsBoolean(); }
            set { Value["UseRectProperties"] = value; }
        }

        public RectPainter()
        {
            Enabled = true;
        }


        private RectPaintHandler m_paintMethod = null;
        public RectPaintHandler PaintMethod
        {
            get { return m_paintMethod; }
            set { m_paintMethod = value; }
        }

        public virtual void Paint(DrawRect rect, Graphics graf)
        {
            if (m_paintMethod != null) m_paintMethod(UseRectProperties ? rect.Value : this.Value, rect, graf);
        }
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
        public class PaintLevel
        {
            private List<RectPainter> m_handlers = new List<RectPainter>();
            
            public void Paint(DrawRect rect, Graphics graf)
            {
                foreach (var h in m_handlers)
                    if (h.Enabled) h.Paint(rect, graf);
            }
            public void Add(RectPainter painter)
            {
                m_handlers.Add(painter);
            }
            public RectPainter Add(RectPaintHandler method, string name)
            {
                var painter = new RectPainter() { PaintMethod = method, Name = name };
                Add(painter);
                return painter;
            }
            public RectPainter Add(RectPaintHandler method)
            {
                return Add(method, "");
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

            public void Clear()
            {
                m_handlers.Clear();
            }
            public int Count
            {
                get { return m_handlers.Count; }
            }
            public RectPainter this[string name]
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
            for (int i = m_PaintLevels.Count - 1; i >= level; i--)
            {
                m_PaintLevels[i].Clear();
                m_PaintLevels.RemoveAt(i);
            }
        }

        private void EnsureLevel(int level)
        {
            while (m_PaintLevels.Count < level + 1)
                m_PaintLevels.Add(new PaintLevel());
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

        public RectPainter this[string name]
        {
            get
            {
                RectPainter res = null;
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

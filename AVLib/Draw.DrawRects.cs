using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AVLib.Draw.DrawRects
{
    public enum RectAlignment
    {
        Absolute,
        Left,
        Right,
        Top,
        Bottom,
        Fill
    }

    public delegate void ChangedHandler();

    public class DrawRect
    {
        private class DrawRectChild
        {
            public Rectangle RectForChild;
            public DrawRect Child;
        }

        private Point m_pos;
        private Size m_size;
        private Rectangle m_rect;
        private RectAlignment m_alignment;
        private Rectangle m_freeRect;
        private Rectangle m_clipRect;
        private DrawRect m_Parent;
        private int m_ParentIndex;
        private List<DrawRectChild> m_childs = null;
        private RectPainters m_painters = new RectPainters();
        private Graphics m_graf;
        private Image m_Image;
        private int m_paint_disabled = 0;
        private bool m_needPaint = false;
        private bool m_inAlign = false;
        private Region m_invalidateRegion = new Region();
        private bool m_needInvalidate = false;
        private bool m_fullValidate = false;
        private int m_BorderSize = 0;
        private int m_InvalidateBorder = 0;
        private Control m_control = null;
        private bool m_Visible = true;
        private Control m_LockControl = null;

        private bool m_UserParentBuffer = false;
        private Bitmap m_ParentBuffer = null;
        private bool m_ParentBufferAvailable = false;
        private Graphics m_ParentBufferGraf = null;
        private object lockObject = new object();

        private object LockObject()
        {
            return (m_Image == null) ? lockObject : (object)m_Image;
        }

        #region Constructors

        public DrawRect(Control control, Point pos, int width, int height)
            : this()
        {
            m_LockControl = control;
            m_painters.control = control;
            m_alignment = RectAlignment.Absolute;
            m_pos = pos;
            m_size = new Size(width, height);
            m_rect = new Rectangle(pos.X, pos.Y, width, height);
            m_freeRect = m_rect;
            m_clipRect = m_rect;
        }

        protected DrawRect()
        {
            m_painters.Changed += new ChangedHandler(m_painters_Changed);
        }


        private delegate void MethodHandler();
        private void m_painters_Changed()
        {
            if (InvokeRequired)
                Invoke(new MethodHandler(Invalidate));
            else
                Invalidate();
        }

        #endregion

        public bool InvokeRequired
        {
            get { return m_LockControl.InvokeRequired; }
        }

        public object Invoke(Delegate method)
        {
            return m_LockControl.Invoke(method);
        }

        public object Invoke(Delegate method, params Object[] args)
        {
            return m_LockControl.Invoke(method, args);
        }

        #region Public

        public delegate void OnPaintHandler(DrawRect rect);
        public delegate void OnValidateHandler(Region rect);
        public event OnPaintHandler OnPaint;
        public event OnValidateHandler OnInvalidate;

        public Point Pos
        {
            get { return m_pos; }
            set
            {
                if (m_pos != value)
                {
                    m_pos = value;
                    DoRectChanged(true);
                }
            }
        }

        public Size Size
        {
            get { return m_size; }
            set
            {
                if (m_size != value)
                {
                    Rectangle old = new Rectangle(new Point(0, 0), m_size);
                    m_size = value;
                    DoRectChanged(true);
                    if (m_Parent == null) InvalidateChildRect(old, this);
                }
            }
        }

        public int BorderSize
        {
            get { return m_BorderSize; }
            set
            {
                if (m_BorderSize != value)
                {
                    m_BorderSize = Math.Max(0, value);
                    DoRectChanged(false);
                    DoInvalidate(Rect);
                }
            }
        }

        public bool UserParentBuffer
        {
            get { return m_UserParentBuffer; }
            set
            {
                m_UserParentBuffer = value;
                DoRectChanged(true);
            }
        }

        public int InvalidateBorder
        {
            get { return m_InvalidateBorder; }
            set { m_InvalidateBorder = value; }
        }

        public RectAlignment Alignment
        {
            get { return m_alignment; }
            set
            {
                if (m_alignment != value)
                {
                    m_alignment = value;
                    DoRectChanged(false);
                }
            }
        }

        public Rectangle Rect
        {
            get { return m_rect; }
            set
            {
                m_pos = value.Location;
                m_size = value.Size;
                DoRectChanged(true);
            }
        }

        public Control Control
        {
            get { return m_control; }
            set
            {
                m_control = value;
                AlignControl();
            }
        }

        public bool Visible
        {
            get { return m_Visible; }
            set
            {
                if (m_Visible != value)
                {
                    m_Visible = value;
                    if (m_control != null) m_control.Visible = m_Visible;
                    DoRectChanged(true);
                    DoInvalidate(m_rect);
                }
            }
        }

        public int Height
        {
            get { return m_size.Height; }
            set { Size = new Size(m_size.Width, value); }
        }

        public int Width
        {
            get { return m_size.Width; }
            set { Size = new Size(value, m_size.Height); }
        }

        public DrawRect Parent
        {
            get { return m_Parent; }
        }

        public object Target;

        public int Count
        {
            get { return m_childs == null ? 0 : m_childs.Count; }
        }

        public DrawRect this[int index]
        {
            get { return m_childs == null ? null : m_childs[index].Child; }
        }

        public Graphics Graf
        {
            get { return m_graf; }
            set
            {
                var needInvalidate = (m_graf == null);
                m_graf = value;
                BeginAlign();
                if (m_childs != null)
                {
                    foreach (var child in m_childs)
                        child.Child.Graf = m_graf;
                }
                if (needInvalidate) DoInvalidate(Rect);
                EndAlign();
            }
        }

        public Image Image
        {
            get { return m_Image; }
            set
            {
                m_Image = value;
                if (m_childs != null)
                {
                    foreach (var child in m_childs)
                        child.Child.Image = m_Image;
                }
            }
        }

        public void Clear()
        {
            m_childs = null;
        }

        public RectPainters Painters
        {
            get { return m_painters; }
        }

        public DrawRect GetChildEt(Point pt)
        {
            if (m_childs == null) return null;
            for (int i = m_childs.Count - 1; i >= 0; i--)
            {
                if (m_childs[i].Child.Visible && m_childs[i].Child.Rect.Contains(pt))
                    return m_childs[i].Child;
            }
            return null;
        }

        private void DoInvalidate(Rectangle rect)
        {
            if (m_Parent != null)
                m_Parent.DoInvalidate(rect);
            else
            {
                if (OnInvalidate != null)
                {
                    if (m_inAlign)
                    {
                        m_needInvalidate = true;
                        m_invalidateRegion.Union(rect);
                    }
                    else
                        OnInvalidate(new Region(rect));
                }
            }
        }

        private void DoInvalidate(Region region)
        {
            if (m_Parent != null)
                m_Parent.DoInvalidate(region);
            else
            {
                if (OnInvalidate != null)
                    OnInvalidate(region);
            }
        }

        private void BeginAlign()
        {
            m_inAlign = true;
        }

        private void EndAlign()
        {
            m_inAlign = false;
            if (m_needInvalidate)
            {
                m_needInvalidate = false;
                DoInvalidate(m_invalidateRegion);
                m_invalidateRegion.MakeEmpty();
            }
        }

        public void Invalidate()
        {
            if (DoPaint()) DoInvalidate(Rect);
        }

        private void RecreateParentBuffer()
        {
            bool doCreate = false;
            if (m_ParentBuffer == null) doCreate = true;
            else
            {
                if (m_ParentBuffer.Width < m_rect.Width || m_ParentBuffer.Height < m_rect.Height) doCreate = true;
                else
                    m_ParentBufferAvailable = true;
            }

            if (doCreate && m_rect.Width > 0 && m_rect.Height > 0)
            {
                m_ParentBuffer = new Bitmap(m_rect.Width, m_rect.Height);
                m_ParentBufferGraf = Graphics.FromImage(m_ParentBuffer);
                m_ParentBufferAvailable = true;
            }
        }

        private void RemBuffer()
        {
            m_ParentBufferAvailable = false;
            if (!m_UserParentBuffer || m_graf == null || m_Image == null) return;
            RecreateParentBuffer();
            if (m_ParentBufferAvailable)
            {
                lock (LockObject())
                {
                    m_ParentBufferGraf.DrawImage(m_Image, new Rectangle(new Point(0, 0), new Size(m_rect.Width, m_rect.Height)), m_rect, GraphicsUnit.Pixel);
                }
            }
        }

        private void PaintParentBuffer()
        {
            if (m_ParentBufferAvailable)
            {
                lock (LockObject())
                {
                    m_graf.DrawImage(m_ParentBuffer, m_rect, new Rectangle(new Point(0, 0), new Size(m_rect.Width, m_rect.Height)), GraphicsUnit.Pixel);
                }
            }
        }

        public void Paint()
        {
            Paint(-1, false);
        }

        private void Paint(int fromIndex, bool skipRaiseParentPaint)
        {
            if (m_graf != null && m_Visible)
            {
                if (fromIndex < 0)
                {
                    m_needPaint = false;
                    if (OnPaint != null) OnPaint(this);

                    lock (LockObject())
                    {
                        var old = m_graf.Clip;
                        m_graf.Clip = new Region(m_clipRect);
                        if (m_UserParentBuffer) PaintParentBuffer();
                        m_painters.Paint(this, m_graf);
                        m_graf.Clip = old;
                    }
                }

                if (m_childs != null)
                {
                    for (int i = Math.Max(0, fromIndex); i < m_childs.Count; i++)
                    {
                        if (fromIndex < 0) m_childs[i].Child.RemBuffer();
                        m_childs[i].Child.DoPaint(true);
                        if (fromIndex >= 0) m_childs[i].Child.DoInvalidate(m_childs[i].Child.Rect);
                    }
                }

                if (fromIndex < 0 && !skipRaiseParentPaint && m_Parent != null)
                {
                    m_Parent.Paint(-1, false);
                }
            }
        }

        public void DisablePainting()
        {
            m_paint_disabled++;
        }

        public void EnablePainting()
        {
            m_paint_disabled--;
            if (m_paint_disabled == 0 && m_needPaint) Paint();
        }

        #endregion

        protected bool DoPaint()
        {
            return DoPaint(false);
        }

        private bool DoPaint(bool skipRaiseParentPaint)
        {
            if (m_paint_disabled > 0 || (m_Parent != null && m_Parent.m_paint_disabled > 0))
            {
                m_needPaint = true;
                return false;
            }

            Paint(-1, skipRaiseParentPaint);
            return true;
        }

        private Rectangle RectWithoutBorder()
        {
            int n = Math.Max(m_InvalidateBorder, m_BorderSize);
            return new Rectangle(m_rect.X + n, m_rect.Y + n, m_rect.Width - 2 * n, m_rect.Height - 2 * n);
        }

        private void DoRectChanged(bool isRect)
        {
            if (m_Parent != null)
                m_Parent.RealignChilds(m_ParentIndex, isRect);
            else
            {
                var old = m_rect;
                m_rect = new Rectangle(m_pos, m_size);
                m_clipRect = m_rect;
                m_freeRect = RectWithoutBorder();
                RealignChilds();
                InvalidateChildRect(old, this);
            }
            DoPaint();
        }

        private void FixNegativeSize(ref Rectangle rect)
        {
            if (rect.Width < 0) rect.Width = 0;
            if (rect.Height < 0) rect.Height = 0;
        }

        private void InvalidateChildRect(Rectangle oldRect, DrawRect rect)
        {
            if (oldRect.Location != rect.Rect.Location)
            {
                DoInvalidate(oldRect);
                DoInvalidate(rect.Rect);
                return;
            }
            if (rect.m_fullValidate)
            {
                DoInvalidate(rect.Rect);
                return;
            }
            if (rect.Rect.Width > oldRect.Width)
            {
                DoInvalidate(new Rectangle(oldRect.Right - rect.m_BorderSize, oldRect.Y, rect.Rect.Width - oldRect.Width + rect.m_BorderSize, rect.Rect.Height));
            }
            else
            {
                if (rect.Rect.Width < oldRect.Width)
                    DoInvalidate(new Rectangle(rect.Rect.Right - rect.m_BorderSize, rect.Rect.Top, oldRect.Width - rect.Width + rect.m_BorderSize, rect.Rect.Height));
            }
            if (rect.Rect.Height > oldRect.Height)
            {
                DoInvalidate(new Rectangle(oldRect.X, oldRect.Bottom - rect.m_BorderSize, rect.Rect.Width, rect.Rect.Height - oldRect.Height + rect.m_BorderSize));
            }
            else
            {
                if (rect.Rect.Height < oldRect.Height)
                    DoInvalidate(new Rectangle(rect.Rect.X, rect.Rect.Bottom - rect.m_BorderSize, rect.Rect.Width, oldRect.Height - rect.Height + rect.m_BorderSize));
            }
        }

        private bool DoAlign(DrawRectChild child)
        {
            child.RectForChild = m_freeRect;
            child.Child.m_clipRect = m_freeRect;
            child.Child.DisablePainting();
            var oldRect = child.Child.Rect;
            if (child.Child.Visible)
            {
                switch (child.Child.m_alignment)
                {
                    case RectAlignment.Absolute:
                        child.Child.m_rect = new Rectangle(m_freeRect.X + child.Child.m_pos.X,
                                                           m_freeRect.Y + child.Child.m_pos.Y, child.Child.Size.Width,
                                                           child.Child.Size.Height);
                        break;
                    case RectAlignment.Left:
                        child.Child.m_rect = new Rectangle(m_freeRect.Location,
                                                           new Size(child.Child.Width, m_freeRect.Height));
                        m_freeRect = new Rectangle(m_freeRect.X + child.Child.m_rect.Width, m_freeRect.Y,
                                                   m_freeRect.Width - child.Child.Width, m_freeRect.Height);
                        FixNegativeSize(ref m_freeRect);
                        break;
                    case RectAlignment.Right:
                        child.Child.m_rect = new Rectangle(m_freeRect.Left + m_freeRect.Width - child.Child.Width,
                                                           m_freeRect.Y, child.Child.Width, m_freeRect.Height);
                        m_freeRect = new Rectangle(m_freeRect.Location,
                                                   new Size(m_freeRect.Width - child.Child.Width, m_freeRect.Height));
                        FixNegativeSize(ref m_freeRect);
                        break;
                    case RectAlignment.Top:
                        child.Child.m_rect = new Rectangle(m_freeRect.Location,
                                                           new Size(m_freeRect.Width, child.Child.Height));
                        m_freeRect = new Rectangle(m_freeRect.X, m_freeRect.Y + child.Child.Height, m_freeRect.Width,
                                                   m_freeRect.Height - child.Child.Height);
                        FixNegativeSize(ref m_freeRect);
                        break;
                    case RectAlignment.Bottom:
                        child.Child.m_rect = new Rectangle(m_freeRect.X,
                                                           m_freeRect.Y + m_freeRect.Height - child.Child.Height,
                                                           m_freeRect.Width, child.Child.Height);
                        m_freeRect = new Rectangle(m_freeRect.Location,
                                                   new Size(m_freeRect.Width, m_freeRect.Height - child.Child.Height));
                        FixNegativeSize(ref m_freeRect);
                        break;
                    case RectAlignment.Fill:
                        child.Child.m_rect = m_freeRect;
                        break;
                }
            }
            child.Child.RealignChilds();
            child.Child.EnablePainting();
            InvalidateChildRect(oldRect, child.Child);
            return oldRect != child.Child.Rect;
        }

        private void AlignControl()
        {
            if (m_control != null)
            {
                m_control.Location = m_rect.Location;
                m_control.Size = m_rect.Size;
            }
        }

        private void RealignChilds()
        {
            AlignControl();
            RealignChilds(0, false);
        }

        private void RealignChilds(int fromIndex, bool repaint)
        {
            BeginAlign();
            try
            {
                DisablePainting();
                if (fromIndex == 0)
                    m_freeRect = RectWithoutBorder();
                else
                    m_freeRect = m_childs[fromIndex].RectForChild;

                bool aligned = false;
                for (int i = fromIndex; i < Count; i++)
                {
                    aligned = DoAlign(m_childs[i]);
                }
                EnablePainting();
                if (aligned)
                    Invalidate();
                else
                    if (repaint) DoPaint();
            }
            finally
            {
                EndAlign();
            }
        }

        protected virtual DrawRect CreateInstance()
        {
            return new DrawRect();
        }

        private void Add(DrawRect rect)
        {
            if (m_childs == null) m_childs = new List<DrawRectChild>();

            DrawRectChild child = new DrawRectChild();
            child.Child = rect;
            rect.m_LockControl = m_LockControl;
            rect.Painters.control = m_LockControl;
            rect.m_Parent = this;
            rect.m_ParentIndex = m_childs.Count;
            rect.Graf = m_graf;
            rect.Image = m_Image;
            m_childs.Add(child);
            DoAlign(child);
        }

        public DrawRect Add(Point pos, int width, int height)
        {
            DrawRect res = CreateInstance();
            res.m_pos = pos;
            res.m_size = new Size(width, height);
            res.m_alignment = RectAlignment.Absolute;
            Add(res);
            return res;
        }

        public DrawRect Add(RectAlignment alignment, int size)
        {
            return Add(alignment, size, false);
        }

        public DrawRect Add(RectAlignment alignment, int size, bool fullValidate)
        {
            DrawRect res = CreateInstance();
            res.m_size = new Size(size, size);
            res.m_alignment = alignment;
            res.m_fullValidate = fullValidate;
            Add(res);
            return res;
        }
    }

    public interface IRectPainter
    {
        string Name { get; set; }
        event ChangedHandler Changed;
        void DoChange();
        void Paint(DrawRect rect, Graphics graf);
        bool InvokeRequired();
        object Invoke(Delegate method, params Object[] args);
        object Invoke(Delegate method);
    }

    public abstract class RectPainter : IRectPainter
    {
        internal Control control;
        public string Name { get; set; }

        public event ChangedHandler Changed;
        public void DoChange()
        {
            if (Changed != null) Changed();
        }

        public abstract void Paint(DrawRect rect, Graphics graf);

        public bool InvokeRequired()
        {
            return (control != null && control.InvokeRequired);
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

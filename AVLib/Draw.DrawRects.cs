﻿using System;
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
        private RectAlignment m_alignment;
        private bool m_Transparent; //todo: make property

        private Rectangle m_rect;
        private Rectangle m_freeRect;
        private Region m_clipReg = new Region();
        private DrawRect m_Parent;
        private int m_ParentIndex;
        private List<DrawRectChild> m_childs = null;
        private RectPainters m_painters = new RectPainters();

        private bool m_fullValidate = false;
        private int m_BorderSize = 0;
        private int m_InvalidateBorder = 0;
        private Control m_control = null;
        private bool m_Visible = true;
        private Control m_LockControl = null;

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
            m_invalidateRegion = new Region();
        }

        public DrawRect()
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

        public Point Pos
        {
            get { return m_pos; }
            set
            {
                if (m_pos != value)
                {
                    m_pos = value;
                    DoRectChanged(false);
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
                    DisableInvalidate();
                    DoRectChanged(true);
                    if (m_Parent == null) InvalidateChildRect(old, this);
                    EnableInvalidate();
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
                    DoRectChanged(true);
                    Invalidate(Rect);
                }
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
                    DoRectChanged(true);
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
                    if (m_Visible || m_alignment != RectAlignment.Absolute) DoRectChanged(true);
                    Invalidate(m_rect);
                }
            }
        }

        public bool Transparent
        {
            get { return m_Transparent; }
            set
            {
                if (m_Transparent != value)
                {
                    m_Transparent = value;
                    Invalidate();
                }
            }
        }

        public bool FullValidate
        {
            get { return m_fullValidate; }
            set
            {
                if (m_fullValidate != value)
                {
                    m_fullValidate = value;
                    Invalidate();
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

        public void Clear()
        {
            m_childs = null;
            Invalidate();
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

        public void Add(DrawRect rect)
        {
            if (m_childs == null) m_childs = new List<DrawRectChild>();

            DrawRectChild child = new DrawRectChild();
            child.Child = rect;
            rect.m_LockControl = m_LockControl;
            rect.Painters.control = m_LockControl;
            rect.m_Parent = this;
            rect.m_ParentIndex = m_childs.Count;
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
            DrawRect res = CreateInstance();
            res.m_size = new Size(size, size);
            res.m_alignment = alignment;
            Add(res);
            return res;
        }

        #endregion

        #region Painting and alligning

        /*=============== Paint and aligning rules ====================
         * public Paint(Graphics gr) should
         *  - get clip region
         *  - from top child to lower child: (create transparent info stack)
         *    - if not transparent: 
         *      - paint it in clip region
         *      - exclude child rect from clip region
         *    - if transparent: 
         *      - remmeber info in stack, inculude child and its region, wich intersects with clip region 
         * - paint self in remaining region
         * - for each info in stack: paint transparent child in their remembered regions
         * 
         * protected InvalidateRegin(oldRect, item) shold:
         *  - return if oldRect = item.Rect
         *  - call Invalidate for freed rect
         *  - if (item.FullValidate*) call Invalidate item.Rect
         *  - else:
         *    - call Invalidate for new added retion
         *    - calc max(item.border_size, item.invalidate_border) region and Invalidate it
         * 
         * On resize:
         *  - if not is TOP rect:
         *    - call parent to realign child
         *  - else
         *    - Disable invalidating (remember invalidate regios mode)
         *    - realign childs *
         *    - call InvalidateRegion(OldRect, this)
         *    - Enable invalidating
         *  
         * Realign childs:
         *  - return if aligning disabled (need force realign after enabling)
         *  - realign with call to InvalidateRegion(oldRect, child) for each
         *  - if child rect changed call realign child for each child
         *  
         * On visible/hide:
         *  - call parent to realign child
         *  
         * On visible property change:
         *  - create update region
         *  - call parent to remove overlaps from this region
         *    - parent shold remove top not transparent childs rect from this region
         *    - parent shold call parent ... up to top for removes all overlaped rects
         *  - If region not empty - invalidate it
        --------------------------------------------------*/

        //--- Paint --------------------------------------

        private struct PaintStackInfo
        {
            public DrawRect child;
            public Region childRegion;
        }
        public void Paint(Graphics gr)
        {
            if (!m_Visible) return;

            Region paintReg = gr.Clip.Clone();
            Stack<PaintStackInfo> transparentInfo = new Stack<PaintStackInfo>();
            if (Count > 0)
            {
                for (int i = m_childs.Count - 1; i >= 0; i--)
                {
                    if (m_childs[i].Child.Visible && paintReg.IsVisible(m_childs[i].Child.Rect))
                    {
                        if (m_childs[i].Child.m_Transparent)
                        {
                            PaintStackInfo stackInfo = new PaintStackInfo();
                            stackInfo.child = m_childs[i].Child;
                            stackInfo.childRegion = paintReg.Clone();
                            stackInfo.childRegion.Intersect(m_childs[i].Child.m_clipReg);
                            transparentInfo.Push(stackInfo);
                        }
                        else
                        {
                            gr.Clip = paintReg.Clone();
                            gr.Clip.Intersect(m_childs[i].Child.m_clipReg);
                            m_childs[i].Child.Paint(gr);
                            paintReg.Exclude(m_childs[i].Child.m_clipReg);
                        }
                    }
                }
            }
            if (paintReg.IsVisible(Rect))
            {
                gr.Clip = paintReg;
                m_painters.Paint(this, gr);
            }
            if (Count > 0)
            {
                while (transparentInfo.Count > 0)
                {
                    var info = transparentInfo.Pop();
                    gr.Clip = info.childRegion;
                    info.child.Paint(gr);
                }
            }
        }

        //------------------------------------------------

        //--- Invalidation logic:-------------------------

        private readonly Region m_invalidateRegion = null; //only for TOPmost rect, child rect should call parent
        private Region InvalidateRegion
        {
            get
            {
                if (m_Parent != null)
                    return m_Parent.InvalidateRegion;
                return m_invalidateRegion;
            }
        }
        private int m_InvalidateDisabled = 0; //only for TOPmost rect, child rect should call parent
        private int InvalidateDisabled
        {
            get
            {
                if (m_Parent != null)
                    return m_Parent.InvalidateDisabled;
                return m_InvalidateDisabled;
            }
            set
            {
                if (m_Parent != null)
                    m_Parent.InvalidateDisabled = value;
                else
                    m_InvalidateDisabled = value;
            }
        }
        public void DisableInvalidate()
        {
            InvalidateDisabled++;
        }
        public void EnableInvalidate()
        {
            InvalidateDisabled--;
            if (InvalidateDisabled <= 0) InvalidateIfNeeded();
        }
        private void InvalidateIfNeeded()
        {
            if (m_Parent != null)
            {
                m_Parent.InvalidateIfNeeded();
                return;
            }

            if (OnInvalidate != null)
            {
                if (InvalidateRegion.IsVisible(Rect))
                {
                    InvalidateRegion.Intersect(Rect);
                    OnInvalidate(InvalidateRegion);
                }
            }
            InvalidateRegion.MakeEmpty();
        }
        public delegate void OnValidateHandler(Region rect);
        public event OnValidateHandler OnInvalidate;
        public void Invalidate()
        {
            InvalidateRegion.Union(Rect);
            if (InvalidateDisabled <= 0) InvalidateIfNeeded();
        }
        public void Invalidate(Rectangle invalidateRect)
        {
            InvalidateRegion.Union(invalidateRect);
            if (InvalidateDisabled <= 0) InvalidateIfNeeded();
        }
        public void Invalidate(Region invalidateRegion)
        {
            InvalidateRegion.Union(invalidateRegion);
            if (InvalidateDisabled <= 0) InvalidateIfNeeded();
        }
        public void PreInvalidate(Rectangle invalidateRect)
        {
            InvalidateRegion.Union(invalidateRect);
        }
        public void PreInvalidate(Region invalidateRegion)
        {
            InvalidateRegion.Union(invalidateRegion);
        }

        //------------------------------------------

        //----- Align logic ------------------------

        private int m_AlignDisabled = 0; //only for TOPmost rect, child rect should call parent
        private int AlignDisabled
        {
            get
            {
                if (m_Parent != null) return m_Parent.AlignDisabled;
                return m_AlignDisabled;
            }
            set
            {
                if (m_Parent != null)
                    m_Parent.AlignDisabled = value;
                else
                    m_AlignDisabled = value;
            }
        }
        public void DisableAlign()
        {
            AlignDisabled++;
        }
        public void EnableAlign()
        {
            AlignDisabled--;
            if (m_Parent == null && AlignDisabled <= 0)
                RealignChilds();
        }

        private void InvalidateChildRect(Rectangle oldRect, DrawRect rect)
        {
            if (oldRect == rect.Rect) return;
            if (oldRect.Location != rect.Rect.Location)
            {
                PreInvalidate(oldRect);
                Invalidate(rect.Rect);
                return;
            }
            int borderSize = Math.Max(rect.m_BorderSize, rect.m_InvalidateBorder);
            if (rect.Rect.Width < oldRect.Width)
                Invalidate(new Rectangle(rect.Rect.Right - borderSize, rect.Rect.Top, oldRect.Width - rect.Width + borderSize, rect.Rect.Height));
            if (rect.Rect.Height < oldRect.Height)
                Invalidate(new Rectangle(rect.Rect.X, rect.Rect.Bottom - borderSize, rect.Rect.Width, oldRect.Height - rect.Height + borderSize));
            if (rect.m_fullValidate)
            {
                Invalidate(rect.Rect);
                return;
            }
            if (rect.Rect.Width > oldRect.Width)
            {
                Invalidate(new Rectangle(oldRect.Right - borderSize, oldRect.Y,
                                           rect.Rect.Width - oldRect.Width + borderSize, rect.Rect.Height));
            }
            if (rect.Rect.Height > oldRect.Height)
            {
                Invalidate(new Rectangle(oldRect.X, oldRect.Bottom - borderSize, rect.Rect.Width,
                                           rect.Rect.Height - oldRect.Height + borderSize));
            }
        }

        private bool DoAlign(DrawRectChild child)
        {
            DisableInvalidate();

            child.RectForChild = m_freeRect;
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
                        break;
                    case RectAlignment.Right:
                        child.Child.m_rect = new Rectangle(m_freeRect.Left + m_freeRect.Width - child.Child.Width,
                                                           m_freeRect.Y, child.Child.Width, m_freeRect.Height);
                        break;
                    case RectAlignment.Top:
                        child.Child.m_rect = new Rectangle(m_freeRect.Location,
                                                           new Size(m_freeRect.Width, child.Child.Height));
                        break;
                    case RectAlignment.Bottom:
                        child.Child.m_rect = new Rectangle(m_freeRect.X,
                                                           m_freeRect.Y + m_freeRect.Height - child.Child.Height,
                                                           m_freeRect.Width, child.Child.Height);
                        break;
                    case RectAlignment.Fill:
                        child.Child.m_rect = m_freeRect;
                        break;
                }
            }

            child.Child.m_clipReg.MakeEmpty();
            child.Child.m_clipReg.Union(child.Child.m_rect);
            child.Child.m_clipReg.Intersect(RectWithoutBorder());

            if (child.Child.Visible)
            {
                switch (child.Child.m_alignment)
                {
                    case RectAlignment.Left:
                        m_freeRect = new Rectangle(m_freeRect.X + child.Child.m_rect.Width, m_freeRect.Y,
                                                   m_freeRect.Width - child.Child.Width, m_freeRect.Height);
                        break;
                    case RectAlignment.Right:
                        m_freeRect = new Rectangle(m_freeRect.Location,
                                                   new Size(m_freeRect.Width - child.Child.Width, m_freeRect.Height));
                        break;
                    case RectAlignment.Top:
                        m_freeRect = new Rectangle(m_freeRect.X, m_freeRect.Y + child.Child.Height, m_freeRect.Width,
                                                   m_freeRect.Height - child.Child.Height);
                        break;
                    case RectAlignment.Bottom:
                        m_freeRect = new Rectangle(m_freeRect.Location,
                                                   new Size(m_freeRect.Width, m_freeRect.Height - child.Child.Height));
                        break;
                }
            }

            child.Child.RealignChilds();
            InvalidateChildRect(oldRect, child.Child);

            EnableInvalidate();
            return oldRect != child.Child.Rect;
        }

        private void AlignControl()
        {
            if (m_control != null)
            {
                m_control.Location = m_rect.Location;
                m_control.Size = m_rect.Size;
                var contrlR = m_clipReg.Clone();
                contrlR.Translate(-m_rect.X, -m_rect.Y);
                m_control.Region = contrlR;
            }
        }

        private void ClipChildControl(Region region, int upTo)
        {
            if (Count > 0)
            {
                for (int i = 0; i < Math.Min(upTo + 1, m_childs.Count); i++)
                    m_childs[i].Child.ClipControl(region);
            }
        }

        private void ClipControl(Region region)
        {
            if (m_control != null && region.IsVisible(m_rect))
            {
                var contrlR = m_control.Region == null ? null : m_control.Region.Clone();
                if (contrlR == null)
                    contrlR = m_clipReg.Clone();
                else
                    contrlR.Translate(m_rect.X, m_rect.Y);

                contrlR.Exclude(region);
                contrlR.Translate(-m_rect.X, -m_rect.Y);
                m_control.Region = contrlR;
            }

            ClipChildControl(region, Count - 1);
        }

        private void RealignChilds()
        {
            AlignControl();
            RealignChilds(0);
        }

        private void RealignChilds(int fromIndex)
        {
            DisableInvalidate();
            if (fromIndex == 0)
                m_freeRect = RectWithoutBorder();
            else
                m_freeRect = m_childs[fromIndex].RectForChild;

            bool aligned = false;
            for (int i = fromIndex; i < Count; i++)
            {
                aligned = DoAlign(m_childs[i]);
                this.ClipChildControl(m_childs[i].Child.m_clipReg, i - 1);
            }
            EnableInvalidate();
        }

        //------------------------------------------

        #endregion

        private Rectangle RectWithoutBorder()
        {
            int n = m_BorderSize;
            return new Rectangle(m_rect.X + n, m_rect.Y + n, m_rect.Width - 2 * n, m_rect.Height - 2 * n);
        }

        private void DoRectChanged(bool isRect)
        {
            if (m_Parent != null)
                m_Parent.RealignChilds(m_ParentIndex);
            else
            {
                var old = m_rect;
                m_rect = new Rectangle(m_pos, m_size);
                m_freeRect = RectWithoutBorder();
                if (isRect) RealignChilds();
                InvalidateChildRect(old, this);
            }
        }

        protected virtual DrawRect CreateInstance()
        {
            return new DrawRect();
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
        bool Enabled { get; set; }
    }

    public abstract class RectPainter : IRectPainter
    {
        private bool m_Enabled;

        internal Control control;
        public string Name { get; set; }
        public bool Enabled
        {
            get { return m_Enabled; }
            set
            {
                if (m_Enabled != value)
                {
                    m_Enabled = value;
                    DoChange();
                }
            }
        }

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

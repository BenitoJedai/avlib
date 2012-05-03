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
    public static class AnimeNames
    {
        public static string Align = "c:align";
        public static string ChildAlign = "c:childalign";
    }

    public enum RectAlignment
    {
        Absolute,
        Left,
        Right,
        Top,
        Bottom,
        Fill,
        Center
    }

    public delegate void ChangedHandler();
    public delegate void ItemChangeHandler(DrawRect rect);

    public class DrawRect : IInvokeCompatible
    {
        public event EventHandler VisibleChanged;
        public event ItemChangeHandler AlignmentChanged;
        public event ItemChangeHandler ItemAdded;
        public event ItemChangeHandler Resize;

        private class DrawRectChild
        {
            public Rectangle RectForChild;
            public DrawRect Child;
        }

        private Point m_pos;
        private Size m_size;
        private RectAlignment m_alignment;
        private bool m_Transparent; //todo: make property

        private Rectangle m_rect_internal;
        private Rectangle m_rect
        {
            get { return m_rect_internal; }
            set
            {
                if (m_rect_internal != value)
                {
                    m_rect_internal = value;
                    if (Resize != null) Resize(this);
                    ChildMoved(this);
                }
            }
        }
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

        private bool m_AnimeAlign = false;
        private bool m_AnimeChildAlign = false;

        private ControlProperties m_properties = new ControlProperties();
        public IControlProperties Property
        {
            get { return m_properties; }
        }
        public IControlPropertiesValue Value
        {
            get { return m_properties; }
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
            m_invalidateRegion = new Region();
        }

        public DrawRect(Point pos, int width, int height)
            : this()
        {
            m_alignment = RectAlignment.Absolute;
            m_pos = pos;
            m_size = new Size(width, height);
            m_rect = new Rectangle(pos.X, pos.Y, width, height);
        }

        public DrawRect(RectAlignment alignment, int size)
        {
            m_alignment = alignment;
            m_size = new Size(size, size);
        }

        public DrawRect()
        {
            m_painters.Changed += new ChangedHandler(m_painters_Changed);
        }

        protected Control LockControl
        {
            get { return m_LockControl; }
            private set
            {
                m_LockControl = value;
                m_painters.control = value;
                if (Count > 0)
                {
                    foreach (var child in m_childs)
                        child.Child.LockControl = value;
                }
            }
        }

        public delegate void NotifyHandler(object sender);
        public event NotifyHandler OnChildMoved;
        public void ChildMoved(object sender)
        {
            if (m_Parent != null)
                m_Parent.ChildMoved(this);
            else
                if (OnChildMoved != null) OnChildMoved(this);
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
            get
            {
                return (m_LockControl == null) ? true : m_LockControl.InvokeRequired;
            }
        }

        public object Invoke(Delegate method)
        {
            if (m_LockControl == null) return null;
            return m_LockControl.Invoke(method);
        }

        public object Invoke(Delegate method, params Object[] args)
        {
            if (m_LockControl == null) return null;
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
                    m_size = value;
                    DoRectChanged(true);
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

        public Rectangle Rect
        {
            get { return m_rect; }
            set
            {
                if (m_rect != value)
                {
                    var old = m_rect;
                    m_rect = value;
                    DisableInvalidate();
                    RealignChilds(false);
                    this.InvalidateChildRect(old, this);
                    EnableInvalidate();
                }
            }
        }

        public Rectangle ContainerRect
        {
            get
            {
                var res = m_rect;
                res.Offset(ContainerOffset);
                return res;
            }
            set
            {
                var of = ContainerOffset;
                value.Offset(-of.X, -of.Y);
                Rect = value;
            }
        }

        public Point ContainerOffset
        {
            get
            {
                var p = new Point(0, 0);
                if (m_Parent != null) p.Offset(-m_Parent.Rect.X - m_Parent.m_BorderSize, -m_Parent.Rect.Y - m_Parent.m_BorderSize); ;
                return p;
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
                    if (AlignmentChanged != null) AlignmentChanged(this);
                    DoRectChanged(true);
                }
            }
        }

        public Control Control
        {
            get { return m_control; }
            set
            {
                if (m_control != null) m_control.Validated -= m_control_TextChanged;
                m_control = value;
                m_control.TextChanged += m_control_TextChanged;
                AlignControl();
            }
        }

        private void m_control_TextChanged(object sender, EventArgs e)
        {
            Invalidate(m_rect);
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
                    if (m_Visible || (m_alignment != RectAlignment.Absolute && m_alignment != RectAlignment.Center)) 
                        DoRectChanged(true);
                    Invalidate(m_rect);

                    if (VisibleChanged != null) VisibleChanged(this, new EventArgs());
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

        public bool AnimeAlign
        {
            get { return m_AnimeAlign; }
            set { m_AnimeAlign = value; }
        }

        public bool AnimeChildAlign
        {
            get { return m_AnimeChildAlign; }
            set { m_AnimeChildAlign = value; }
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
            if (m_childs == null)
            {
                m_childs = new List<DrawRectChild>();
                m_freeRect = RectWithoutBorder();
            }

            DrawRectChild child = new DrawRectChild();
            child.Child = rect;
            rect.LockControl = m_LockControl;
            rect.m_Parent = this;
            rect.m_ParentIndex = m_childs.Count;
            m_childs.Add(child);
            DoAlign(child.Child, CalcAlign(child, ref m_freeRect), false);

            DoItemAdded(rect);
        }

        protected virtual void DoItemAdded(DrawRect item)
        {
            if (ItemAdded != null) ItemAdded(item);
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
                            stackInfo.childRegion.Intersect(m_childs[i].Child.ClipReg);
                            transparentInfo.Push(stackInfo);
                        }
                        else
                        {
                            gr.Clip = paintReg.Clone();
                            gr.Clip.Intersect(m_childs[i].Child.ClipReg);
                            m_childs[i].Child.Paint(gr);
                            paintReg.Exclude(m_childs[i].Child.ClipReg);
                        }
                    }
                }
            }
            if (paintReg.IsVisible(Rect))
            {
                gr.Clip = paintReg;
                if (m_control == null)
                    m_painters.Paint(this, gr);
                else
                    PaintControl(paintReg, gr);
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

        private Bitmap m_controlMap = null;
        private void PrepareControlMap()
        {
            if (m_controlMap == null || m_controlMap.Width < m_control.Width || m_controlMap.Height < m_control.Height)
            {
                m_controlMap = new Bitmap(m_control.Width, m_control.Height);
            }
        }
        private void PaintControl(Region paintReg, Graphics gr)
        {
            var redrowReg = new Region(m_rect);
            RemoveOverlapsFromRegion(redrowReg, false);
            var reg = new Region(m_rect);
            RemoveOverlapsFromRegion(reg, true);
            redrowReg.Exclude(reg);

            if (redrowReg.IsVisible(m_rect))
            {
                PrepareControlMap();
                m_control.DrawToBitmap(m_controlMap, new Rectangle(0, 0, m_control.Width, m_control.Height));
                gr.Clip = redrowReg;
                gr.DrawImage(m_controlMap, m_rect, new Rectangle(0, 0, m_control.Width, m_control.Height), GraphicsUnit.Pixel);
            }

            reg.Translate(-m_rect.X, -m_rect.Y);
            if (m_control.Region == null || !reg.Equals(m_control.Region))
                m_control.Region = reg;
        }

        private Region ClipReg
        {
            get
            {
                m_clipReg.MakeEmpty();
                m_clipReg.Union(m_rect);
                if (m_Parent != null) m_clipReg.Intersect(m_Parent.RectWithoutBorder());
                return m_clipReg;
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

            if (InvalidateRegion == null) return; //not assigned to Parent
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
            Invalidate(Rect);
        }
        public void Invalidate(Rectangle invalidateRect)
        {
            var reg = new Region(invalidateRect);
            Invalidate(reg);
        }
        public void Invalidate(Region invalidateRegion)
        {
            if (InvalidateRegion == null) return;

            RemoveOverlapsFromRegion(invalidateRegion, false);
            InvalidateRegion.Union(invalidateRegion);
            if (InvalidateDisabled <= 0) InvalidateIfNeeded();
        }
        public void PreInvalidate(Rectangle invalidateRect)
        {
            var reg = new Region(invalidateRect);
            PreInvalidate(reg);
        }
        public void PreInvalidate(Region invalidateRegion)
        {
            if (InvalidateRegion == null) return;

            RemoveOverlapsFromRegion(invalidateRegion, false);
            InvalidateRegion.Union(invalidateRegion);
        }
        private void RemoveOverlapsFromRegion(Region region, bool withTransparent)
        {
            if (m_Parent != null)
            {
                region.Intersect(m_Parent.RectWithoutBorder());
                m_Parent.RemoveOverlapsFromRegion(region, m_ParentIndex, withTransparent, false);
            }
        }
        private void RemoveOverlapsFromRegion(Region region, int childIndex, bool withTransparent, bool skipParent)
        {
            if (Count > 0)
            {
                for (int i = childIndex + 1; i < m_childs.Count; i++)
                {
                    if (m_childs[i].Child.Transparent && !withTransparent)
                        m_childs[i].Child.RemoveOverlapsFromRegion(region, -1, withTransparent, true);
                    else
                        region.Exclude(m_childs[i].Child.Rect);
                }
            }
            if (!skipParent) RemoveOverlapsFromRegion(region, withTransparent);
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
            if (m_Parent == null) RealignChilds(false);
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
            int maxHeight = Math.Min(rect.Rect.Height, oldRect.Height);
            int maxWidth = Math.Max(rect.Rect.Width, oldRect.Width);

            if (rect.Rect.Width < oldRect.Width)
                Invalidate(new Rectangle(rect.Rect.Right - borderSize, rect.Rect.Top, oldRect.Width - rect.Width + borderSize, maxHeight));
            if (rect.Rect.Height < oldRect.Height)
                Invalidate(new Rectangle(rect.Rect.X, rect.Rect.Bottom - borderSize, maxWidth, oldRect.Height - rect.Height + borderSize));

            if (rect.m_fullValidate)
            {
                Invalidate(rect.Rect);
                return;
            }
            if (rect.Rect.Width > oldRect.Width)
            {
                Invalidate(new Rectangle(oldRect.Right - borderSize, oldRect.Y, rect.Rect.Width - oldRect.Width + borderSize, maxHeight));
            }
            if (rect.Rect.Height > oldRect.Height)
            {
                Invalidate(new Rectangle(oldRect.X, oldRect.Bottom - borderSize, maxWidth, rect.Rect.Height - oldRect.Height + borderSize));
            }
        }

        private Rectangle CalcAlign(DrawRectChild child, ref Rectangle freeRect)
        {
            child.RectForChild = freeRect;
            var newRect = child.Child.Rect;
            if (child.Child.Visible)
            {
                switch (child.Child.m_alignment)
                {
                    case RectAlignment.Absolute:
                        newRect = new Rectangle(m_rect.X + m_BorderSize + child.Child.m_pos.X,
                                                           m_rect.Y + m_BorderSize + child.Child.m_pos.Y, child.Child.Size.Width,
                                                           child.Child.Size.Height);
                        break;
                    case RectAlignment.Center:
                        newRect = new Rectangle(m_rect.X + (m_rect.Width - child.Child.Width)/2,
                                                m_rect.Y + (m_rect.Height - child.Child.Height)/2, child.Child.Width,
                                                child.Child.Height);
                        break;
                    case RectAlignment.Left:
                        newRect = new Rectangle(freeRect.Location,
                                                           new Size(child.Child.Width, freeRect.Height));
                        freeRect = new Rectangle(freeRect.X + newRect.Width, freeRect.Y,
                                                   freeRect.Width - newRect.Width, freeRect.Height);
                        break;
                    case RectAlignment.Right:
                        newRect = new Rectangle(freeRect.Left + freeRect.Width - child.Child.Width,
                                                           freeRect.Y, child.Child.Width, freeRect.Height);
                        freeRect = new Rectangle(freeRect.Location,
                                                   new Size(freeRect.Width - newRect.Width, freeRect.Height));
                        break;
                    case RectAlignment.Top:
                        newRect = new Rectangle(freeRect.Location,
                                                           new Size(freeRect.Width, child.Child.Height));
                        freeRect = new Rectangle(freeRect.X, freeRect.Y + newRect.Height, freeRect.Width,
                                                   freeRect.Height - newRect.Height);
                        break;
                    case RectAlignment.Bottom:
                        newRect = new Rectangle(freeRect.X,
                                                           freeRect.Y + freeRect.Height - child.Child.Height,
                                                           freeRect.Width, child.Child.Height);
                        freeRect = new Rectangle(freeRect.Location,
                                                   new Size(freeRect.Width, freeRect.Height - newRect.Height));
                        break;
                    case RectAlignment.Fill:
                        newRect = freeRect;
                        break;
                }
            }
            return newRect;
        }

        private bool DoAlign(DrawRect child, Rectangle newRect, bool force)
        {
            if (!force && AlignDisabled > 0) return false;

            DisableInvalidate();

            var oldRect = child.Rect;
            if (!m_AnimeChildAlign && child.AnimeAlign && oldRect != newRect)
            {
                child.AnimeCancel(AnimeNames.Align);
                child.Anime(AnimeNames.Align).RectInc(newRect);
            }
            else
            {
                child.m_rect = newRect;
                child.RealignChilds(force);
                InvalidateChildRect(oldRect, child);
            }

            EnableInvalidate();
            return oldRect != newRect;
        }

        private void AlignControl()
        {
            if (m_control != null)
            {
                m_control.Location = m_rect.Location;
                m_control.Size = m_rect.Size;
            }
        }

        public void Align()
        {
            RealignChilds(true);
        }

        private void RealignChilds(bool force)
        {
            if (!force && AlignDisabled > 0) return;
            AlignControl();
            RealignChilds(0, force);
        }

        private void RealignChilds(int fromIndex, bool force)
        {
            if (!force && AlignDisabled > 0) return;

            DisableInvalidate();

            if (fromIndex == 0)
                m_freeRect = RectWithoutBorder();
            else
                m_freeRect = m_childs[fromIndex].RectForChild;

            var childs = new List<DrawRect>();
            var newRects = new List<Rectangle>();

            for (int i = fromIndex; i < Count; i++)
            {
                var newRect = CalcAlign(m_childs[i], ref m_freeRect);
                if (newRect != m_childs[i].Child.m_rect)
                {
                    newRects.Add(newRect);
                    childs.Add(m_childs[i].Child);
                }
            }

            if (m_AnimeChildAlign)
            {
                foreach (var child in childs)
                    child.AnimeCancel(AnimeNames.Align);
                this.AnimeCancel(AnimeNames.ChildAlign);
                this.Anime(AnimeNames.ChildAlign).MultiRectDec(childs.ToArray(), newRects.ToArray());
            }
            else
            {
                for (int i = 0; i < childs.Count; i++)
                    DoAlign(childs[i], newRects[i], force);
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
                m_Parent.RealignChilds(m_ParentIndex, false);
            else
            {
                var old = m_rect;
                m_rect = new Rectangle(m_pos, m_size);
                m_freeRect = RectWithoutBorder();
                DisableInvalidate();
                if (isRect) RealignChilds(false);
                InvalidateChildRect(old, this);
                EnableInvalidate();
            }
        }

        protected virtual DrawRect CreateInstance()
        {
            return new DrawRect();
        }



        #region Enumerate child


        public class EnumerateChildStatus
        {
            public bool skipChild;
            public bool skipAll;
        }

        public delegate void OnEnumerateRectHandler(DrawRect item, EnumerateChildStatus status);

        public void EnumerateChilds(OnEnumerateRectHandler method)
        {
            EnumerateChilds(method, true);
        }

        public void EnumerateChilds(OnEnumerateRectHandler method, bool forward)
        {
            var status = new EnumerateChildStatus() {skipChild = false, skipAll = false};
            EnumerateChilds(method, forward, status);
        }

        public void EnumerateChilds(OnEnumerateRectHandler method, bool forward, EnumerateChildStatus status)
        {
            if (forward)
            {
                for (int i = 0; i < Count; i++)
                {
                    method(m_childs[i].Child, status);
                    if (status.skipAll) return;
                    if (!status.skipChild) m_childs[i].Child.EnumerateChilds(method, forward, status);
                    if (status.skipAll) return;
                }
            }
            else
            {
                for (int i = Count - 1; i >= 0; i--)
                {
                    method(m_childs[i].Child, status);
                    if (status.skipAll) return;
                    if (!status.skipChild) m_childs[i].Child.EnumerateChilds(method, forward, status);
                    if (status.skipAll) return;
                }
            }
        }

        #endregion
    }

    
}

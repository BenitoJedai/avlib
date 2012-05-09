using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AVLib.Animations;
using AVLib.Utils;

namespace AVLib.Draw.DrawRects
{
    public class ControlRect : DrawRect, IDisposable
    {
        public event MouseEventHandler MouseMove;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseClick;
        public event EventHandler Click;
        public event MouseEventHandler MouseDoubleClick;
        public event EventHandler DoubleClick;
        public event EventHandler MouseHover;
        public event MouseEventHandler MouseWheel;
        public event EventHandler EnabledChanged;
        public event DragEventHandler DragEnter;
        public event EventHandler DragLeave;
        public event DragEventHandler DragDrop;
        public event DragEventHandler DragOver;

        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;


        public event MouseEventHandler PostMouseMove;
        public event MouseEventHandler PostMouseDown;
        public event MouseEventHandler PostMouseUp;
        public event MouseEventHandler PostMouseClick;
        public event EventHandler PostClick;
        public event MouseEventHandler PostMouseDoubleClick;
        public event EventHandler PostDoubleClick;
        public event EventHandler PostMouseHover;
        public event MouseEventHandler PostMouseWheel;
        public event EventHandler PostEnabledChanged;
        public event DragEventHandler PostDragEnter;
        public event EventHandler PostDragLeave;
        public event DragEventHandler PostDragDrop;
        public event DragEventHandler PostDragOver;

        public event KeyEventHandler PostKeyDown;
        public event KeyEventHandler PostKeyUp;


        public event Func<object, EventArgs, bool> OnCanSelect;
        public event EventHandler Enter;
        public event EventHandler Leave;

        private ControlRect m_lastMouseControl = null;
        private ControlRect m_captureControl = null;
        private static readonly Point InvalidPoint = new Point(-1, -1);
        private Point m_lastMousePos = InvalidPoint;

        public Point LastMousePos { get { return m_lastMousePos; } set { m_lastMousePos = value; } }

        public bool Enabled
        {
            get { return Value["Enabled"].AsBoolean(); }
            set { Value["Enabled"] = value; }
        }

        public ControlRect(Control control, Point pos, int width, int height)
            : base(control, pos, width, height)
        {
            InitializeControl();
        }

        public ControlRect(Point pos, int width, int height)
            : base(pos, width, height)
        {
            InitializeControl();
        }

        public ControlRect()
            : base()
        {
            InitializeControl();
        }

        protected virtual void InitializeControl()
        {
            AddValidatedProperty("Enabled", true).Changed += () => { if (EnabledChanged != null) EnabledChanged(this, new EventArgs()); };
        }

        protected void Capture(ControlRect rect, bool value)
        {
            var pctrl = this.ParentControlRect;
            if (pctrl != null)
            {
                pctrl.Capture(rect, value);
                return;
            }

            if (LockControl != null)
            {
                LockControl.Capture = value;

                if (value)
                {
                    m_captureControl = rect;
                }
                else
                {
                    m_captureControl = null;
                    MouseChild(LockControl.PointToClient(Control.MousePosition));
                }
            }
        }

        protected override DrawRect CreateInstance()
        {
            return new ControlRect();
        }

        public ControlRect GetChildControlEt(Point pt)
        {
            var res = GetChildEt(pt);
            while (res != null && !(res is ControlRect))
                res = res.GetChildEt(pt);
            return res == null ? null : res as ControlRect;
        }

        internal ControlRect MouseChild(Point pt)
        {
            ControlRect ch;
            if (m_captureControl != null)
                return m_captureControl;
           
            if (pt.X < 0 || pt.Y < 0)
                ch = null;
            else
                ch = GetChildControlEt(pt);

            if (ch != m_lastMouseControl)
            {
                if (m_lastMouseControl != null) m_lastMouseControl.OnMouseLeave(new EventArgs());
                if (ch != null) ch.OnMouseEnter(new EventArgs());
            }
            m_lastMouseControl = ch;
            m_lastMousePos = pt;
            return ch;
        }

        internal void OnMouseMove(MouseEventArgs e)
        {
            if (!Enabled) return;
            var ch = MouseChild(e.Location);

            if (ch != null)
            {
                ch.OnMouseMove(e);
            }
            else
                if (MouseMove != null) MouseMove(this, e);

            if (PostMouseMove != null) PostMouseMove(this, e);
        }

        internal void OnMouseEnter(EventArgs e)
        {
            if (!Enabled) return;
            if (m_lastMouseControl != null)
            {
                m_lastMouseControl.OnMouseLeave(e);
                m_lastMouseControl = null;
            }
            
            if (MouseEnter != null) MouseEnter(this, e);
        }

        internal void OnMouseLeave(EventArgs e)
        {
            if (!Enabled) return;
            if (m_lastMouseControl != null)
            {
                m_lastMouseControl.OnMouseLeave(e);
                m_lastMouseControl = null;
            }
            
            if (MouseLeave != null) MouseLeave(this, e);
            m_lastMousePos = InvalidPoint;
        }

        internal void OnMouseDown(MouseEventArgs e)
        {
            if (!Enabled) return;
            var ch = MouseChild(e.Location);
            if (ch != null)
                {
                    if (ch != m_focusedControl && ch.CanSelect())
                        Select(ch);
                    ch.OnMouseDown(e);
                }
            else
                if (MouseDown != null) MouseDown(this, e);

            if (PostMouseDown != null) PostMouseDown(this, e);
        }

        internal void OnMouseUp(MouseEventArgs e)
        {
            if (!Enabled) return;
            var ch = MouseChild(e.Location);
            if (ch != null)
            {
                ch.OnMouseUp(e);
            }
            else
                if (MouseUp != null) MouseUp(this, e);

            if (PostMouseUp != null) PostMouseUp(this, e);
        }

        internal void OnMouseClick(MouseEventArgs e)
        {
            if (!Enabled) return;
            var ch = MouseChild(e.Location);
            if (ch != null)
            {
                ch.OnMouseClick(e);
            }
            else
            {
                if (MouseClick != null) MouseClick(this, e);
                if (Click != null) Click(this, e);
            }

            if (PostMouseClick != null) PostMouseClick(this, e);
            if (PostClick != null) PostClick(this, e);
        }

        internal void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (!Enabled) return;
            var ch = MouseChild(e.Location);
            if (ch != null)
            {
                ch.OnMouseDoubleClick(e);
            }
            else
            {
                if (MouseDoubleClick != null) MouseDoubleClick(this, e);
                if (DoubleClick != null) DoubleClick(this, e);
            }

            if (PostMouseDoubleClick != null) PostMouseDoubleClick(this, e);
            if (PostDoubleClick != null) PostDoubleClick(this, e);
        }

        internal void OnMouseHover(EventArgs e)
        {
            if (!Enabled) return;

            if (m_lastMouseControl != null)
            {
                m_lastMouseControl.OnMouseHover(e);
            }
            else
            {
                if (MouseHover != null) MouseHover(this, e);
            }

            if (PostMouseHover != null) PostMouseHover(this, e);
        }

        internal void OnMouseWheel(MouseEventArgs e)
        {
            if (!Enabled) return;
            var ch = MouseChild(e.Location);
            if (ch != null)
            {
                ch.OnMouseWheel(e);
            }
            else
                if (MouseWheel != null) MouseWheel(this, e);

            if (PostMouseWheel != null) PostMouseWheel(this, e);
        }

        internal void OnDragEnter(DragEventArgs drgevent)
        {
            if (!Enabled) return;
            var ch = MouseChild(new Point(drgevent.X, drgevent.Y));
            if (ch != null)
            {
                ch.OnDragEnter(drgevent);
            }
            else
                if (DragEnter != null) DragEnter(this, drgevent);

            if (PostDragEnter != null) PostDragEnter(this, drgevent);
        }

        internal void OnDragLeave(EventArgs e)
        {
            if (!Enabled) return;
            if (m_lastMouseControl != null)
            {
                m_lastMouseControl.OnMouseLeave(e);
                m_lastMouseControl = null;
            }
            else
                if (DragLeave != null) DragLeave(this, e);

            if (PostDragLeave != null) PostDragLeave(this, e);
            m_lastMousePos = InvalidPoint;
        }

        internal void OnDragDrop(DragEventArgs drgevent)
        {
            if (!Enabled) return;
            var ch = MouseChild(new Point(drgevent.X, drgevent.Y));
            if (ch != null)
            {
                ch.OnDragDrop(drgevent);
            }
            else
                if (DragDrop != null) DragDrop(this, drgevent);

            if (PostDragDrop != null) PostDragDrop(this, drgevent);
        }

        internal void OnDragOver(DragEventArgs drgevent)
        {
            if (!Enabled) return;
            var ch = MouseChild(new Point(drgevent.X, drgevent.Y));
            if (ch != null)
            {
                ch.OnDragOver(drgevent);
            }
            else
                if (DragOver != null) DragOver(this, drgevent);

            if (PostDragOver != null) PostDragOver(this, drgevent);
        }

        private bool m_focused = false;
        public bool Focused
        {
            get { return m_focused; }
        }

        internal void OnEnter()
        {
            m_focused = true;
            if (TabStop) Invalidate();
            if (Enter != null) Enter(this, new EventArgs());
        }

        internal void OnLeave()
        {
            m_focused = false;
            if (TabStop) Invalidate();
            if (Leave != null) Leave(this, new EventArgs());
        }
        

        #region Focus

        public int TabIndex { get; set; }
        public bool TabStop { get; set; }
        private ControlRect m_focusedControl = null;
        public ControlRect FocusedControl
        {
            get { return m_focusedControl; }
        }

        protected virtual bool CanSelect()
        {
            if (!Visible || !Enabled || !TabStop) return false;
            if (OnCanSelect != null) return OnCanSelect(this, new EventArgs());
            return true;
        }

        public ControlRect GetNextControl(ControlRect current)
        {
            int tab = current == null ? -1 : current.TabIndex;
            int currTab = int.MaxValue;
            ControlRect currControl = null;
            EnumerateChilds((item, status) =>
                                {
                                    var cr = (item as ControlRect);
                                    if (cr != null && cr.TabStop)
                                    {
                                        if (cr != current && cr.TabIndex > tab && cr.TabIndex < currTab)
                                        {
                                            currTab = cr.TabIndex;
                                            currControl = cr;
                                        }
                                    }
                                });
            if (currControl != null)
            {
                if (currControl.CanSelect()) return currControl;
                return GetNextControl(currControl);
            }
            return null;
        }

        public ControlRect GetPrevControl(ControlRect current)
        {
            int tab = current == null ? int.MaxValue : current.TabIndex;
            int currTab = -1;
            ControlRect currControl = null;
            EnumerateChilds((item, status) =>
                                {
                                    var cr = (item as ControlRect);
                                    if (cr != null && cr.TabStop)
                                    {
                                        if (cr != current && cr.TabIndex < tab && cr.TabIndex > currTab)
                                        {
                                            currTab = cr.TabIndex;
                                            currControl = cr;
                                        }
                                    }
                                });
            if (currControl != null)
            {
                if (currControl.CanSelect()) return currControl;
                return GetPrevControl(currControl);
            }
            return null;
        }

        internal void OnGotFocus(EventArgs e)
        {
            if (m_focusedControl != null)
                m_focusedControl.OnEnter();
            else
            {
                if (Control.ModifierKeys != Keys.Shift)
                    m_focusedControl = GetNextControl(null);
                else
                    m_focusedControl = GetPrevControl(null);

                if (m_focusedControl != null) m_focusedControl.OnEnter();
            }
        }

        internal void OnLostFocus(EventArgs e)
        {
            if (m_focusedControl != null) m_focusedControl.OnLeave();
        }

        public void Select()
        {
            Select(this);
        }

        protected ControlRect ParentControlRect
        {
            get
            {
                var rect = Parent;
                while (rect != null && !(rect is ControlRect)) rect = rect.Parent;
                return rect == null ? null : (ControlRect)rect;
            }
        }

        private void Select(ControlRect control)
        {
            var rect = ParentControlRect;

            if (rect != null)
                ((ControlRect)rect).Select(control);
            else
            {
                if (m_focusedControl != null) m_focusedControl.OnLeave();
                m_focusedControl = control;
                m_focusedControl.OnEnter();
            }
        }

        public void SelectNext()
        {
            var rect = ParentControlRect;
            if (rect != null)
                ((ControlRect)rect).SelectNext();
            else
            {
                if (m_focusedControl != null) m_focusedControl.OnLeave();
                m_focusedControl = GetNextControl(m_focusedControl);
                if (m_focusedControl != null) m_focusedControl.OnEnter();
            }
        }

        public void SelectPrev()
        {
            var rect = ParentControlRect;
            if (rect != null)
                ((ControlRect)rect).SelectPrev();
            else
            {
                if (m_focusedControl != null) m_focusedControl.OnLeave();
                m_focusedControl = GetPrevControl(m_focusedControl);
                if (m_focusedControl != null) m_focusedControl.OnEnter();
            }
        }

        #endregion


        internal void OnKeyDown(KeyEventArgs e)
        {
            if (m_focusedControl != null)
            {
                m_focusedControl.OnKeyDown(e);
            }
            else
                if (KeyDown != null) KeyDown(this, e);

            if (PostKeyDown != null) PostKeyDown(this, e);
        }

        internal void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (Parent == null && e.KeyCode == Keys.Tab)
            {
                if (!e.Shift)
                {
                    SelectNext();
                    if (m_focusedControl != null)
                    {
                        e.IsInputKey = true;
                    }
                }
                else
                {
                    SelectPrev();
                    if (m_focusedControl != null)
                    {
                        e.IsInputKey = true;
                    }
                }
            }
        }

        internal void OnKeyUp(KeyEventArgs e)
        {
            if (m_focusedControl != null)
            {
                m_focusedControl.OnKeyUp(e);
            }
            else
                if (KeyUp != null) KeyUp(this, e);

            if (PostKeyUp != null) PostKeyUp(this, e);
        }

        private void DisposeChild(DrawRect rect)
        {
            if (rect is ControlRect)
                ((ControlRect)rect).Dispose();
            else
            {
                for (int i = 0; i < rect.Count; i++)
                    DisposeChild(rect[i]);
            }
        }

        public void Dispose()
        {
            ControlController.RemoveControl(this);
            for (int i = 0; i < Count; i++)
                DisposeChild(this[i]);
        }
    }
}

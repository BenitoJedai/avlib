using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AVLib.Draw.DrawRects;
using AVLib.Draw.DrawRects.Painters;
using AVLib.Utils;
using VALib.Draw.Controls.Core;

namespace VALib.Draw.Controls
{
    public class Scrollable : BaseControl
    {
        #region Property

        public int ScrollHeight
        {
            get { return Value["ScrollHeight"].AsInteger(); }
            set { Value["ScrollHeight"] = value; }
        }

        public int ScrollWidth
        {
            get { return Value["ScrollWidth"].AsInteger(); }
            set { Value["ScrollWidth"] = value; }
        }

        public int ScrollHorizontalPos
        {
            get { return Value["ScrollHorizontalPos"].AsInteger(); }
            set { Value["ScrollHorizontalPos"] = value; }
        }

        public int ScrollVerticalPos
        {
            get { return Value["ScrollVerticalPos"].AsInteger(); }
            set { Value["ScrollVerticalPos"] = value; }
        }

        public int HidenHeight
        {
            get { return Math.Max(0, ScrollHeight - Container.Rect.Height); }
        }

        public int HidenWidth
        {
            get { return Math.Max(0, ScrollWidth - Container.Rect.Width); }
        }

        public bool VirtualScroll
        {
            get { return Value["VirtualScroll"].AsBoolean(); }
            set { Value["VirtualScroll"] = value; }
        }

        private ControlRect m_container = null;
        public override DrawRect Container
        {
            get { return m_container; }
        }

        private DrawRect m_HorizontalScrollHolder = null;
        public DrawScroll HorizontalScroll
        {
            get { return (DrawScroll) m_HorizontalScrollHolder.Find("HorizontalScroll"); }
        }

        private DrawRect m_VerticalScrollHolder = null;
        public DrawScroll VerticalScroll
        {
            get { return (DrawScroll) m_VerticalScrollHolder.Find("VerticalScroll"); }
        }

        #endregion

        protected override void InitializeControl()
        {
            base.InitializeControl();
            m_container = new ControlRect();
            m_container.Transparent = true;
            m_container.Alignment = RectAlignment.Fill;
            this.Add(m_container);

            m_HorizontalScrollHolder = new DrawRect();
            m_HorizontalScrollHolder.Alignment = RectAlignment.Bottom;
            m_HorizontalScrollHolder.Size = new Size(14, 14);
            m_HorizontalScrollHolder.Transparent = true;
            var hcornerRect = new DrawRect();
            hcornerRect.Name = "cornerRect";
            hcornerRect.Size = new Size(14, 14);
            hcornerRect.Alignment = RectAlignment.Right;
            hcornerRect.Transparent = true;
            hcornerRect.Visible = false;
            var hp = hcornerRect.Painters[0].Add(BasePainters.FillRect, "cornerPaint");
            hp.Value["Color"] = Property["Color"];
            m_HorizontalScrollHolder.Add(hcornerRect);
            m_HorizontalScrollHolder.Visible = false;
            var hscroll = new DrawScroll();
            hscroll.Name = "HorizontalScroll";
            hscroll.Size = new Size(14, 14);
            hscroll.Value["Color"] = Property["Color"];
            hscroll.Align = Orientation.Horizontal;
            hscroll.Alignment = RectAlignment.Fill;
            hscroll.Visible = true;
            hscroll.Value["Position"] = Property["ScrollHorizontalPos"];
            m_HorizontalScrollHolder.Add(hscroll);
            this.Insert(0, m_HorizontalScrollHolder);

            m_VerticalScrollHolder = new DrawRect();
            m_VerticalScrollHolder.Alignment = RectAlignment.Right;
            m_VerticalScrollHolder.Size = new Size(14, 14);
            m_VerticalScrollHolder.Transparent = true;
            var vcornerRect = new DrawRect();
            vcornerRect.Name = "cornerRect";
            vcornerRect.Size = new Size(14, 14);
            vcornerRect.Alignment = RectAlignment.Bottom;
            vcornerRect.Transparent = true;
            vcornerRect.Visible = false;
            var vp = vcornerRect.Painters[0].Add(BasePainters.FillRect, "cornerPaint");
            vp.Value["Color"] = Property["Color"];
            m_VerticalScrollHolder.Add(vcornerRect);
            m_VerticalScrollHolder.Visible = false;
            var vscroll = new DrawScroll();
            vscroll.Name = "VerticalScroll";
            vscroll.Size = new Size(14, 14);
            vscroll.Value["Color"] = Property["Color"];
            vscroll.Align = Orientation.Vertical;
            vscroll.Alignment = RectAlignment.Fill;
            vscroll.Visible = true;
            vscroll.Value["Position"] = Property["ScrollVerticalPos"];
            m_VerticalScrollHolder.Add(vscroll);
            this.Insert(0, m_VerticalScrollHolder);

            InitProperties();

            m_container.Resize += new ItemChangeHandler(Scrollable_Resize);
            m_container.PostMouseWheel += new MouseEventHandler(Scrollable_MouseWheel);
        }

        public void InitProperties()
        {
            Property["ScrollHeight"].AnyChanged += () => { UpdateScrolls(); };
            Property["ScrollWidth"].AnyChanged += () => { UpdateScrolls(); };
            Property["ScrollHorizontalPos"].AnyChanged += () => { ScrollContent(); };
            Property["ScrollVerticalPos"].AnyChanged += () => { ScrollContent(); };
        }

        private void UpdateScrolls()
        {
            if (HidenHeight > 0)
            {
                VerticalScroll.MaxPosition = HidenHeight;
                m_VerticalScrollHolder.Visible = true;
            }
            else
            {
                VerticalScroll.MaxPosition = 0;
                m_VerticalScrollHolder.Visible = false;
            }

            if (HidenWidth > 0)
            {
                HorizontalScroll.MaxPosition = HidenWidth;
                m_HorizontalScrollHolder.Visible = true;
            }
            else
            {
                HorizontalScroll.MaxPosition = 0;
                m_HorizontalScrollHolder.Visible = false;
            }

            if (HidenHeight > 0 && HidenWidth > 0)
            {
                m_HorizontalScrollHolder.Find("cornerRect").Visible = m_HorizontalScrollHolder.ParentIndex < m_VerticalScrollHolder.ParentIndex;
                m_VerticalScrollHolder.Find("cornerRect").Visible = m_VerticalScrollHolder.ParentIndex < m_HorizontalScrollHolder.ParentIndex;
            }
            else
            {
                m_HorizontalScrollHolder.Find("cornerRect").Visible = false;
                m_VerticalScrollHolder.Find("cornerRect").Visible = false;
            }
        }

        private void ScrollContent()
        {
            if (!VirtualScroll) Container.ApperLeftAlignPoint = new Point(-ScrollHorizontalPos, -ScrollVerticalPos);
        }

        private void Scrollable_Resize(DrawRect rect)
        {
            UpdateScrolls();
        }

        private void Scrollable_MouseWheel(object sender, MouseEventArgs e)
        {
            int n = 3*e.Delta/120;
            ScrollVerticalPos -= n;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AVLib.Controls;
using AVLib.Draw.DrawRects;
using AVLib.Draw.DrawRects.Painters;
using AVLib.Utils;
using AVLib.Animations;
using VALib.Draw.Controls;

namespace DrawRectTestApp
{
    public partial class Form1 : Form
    {
        private ControlRect transparentRect;
        private DrawRect controlRect;
        private DrawRect yellowRect;
        private DrawRect backRect;
        private ControlRect mouseRect;
        private DrawRect topRect;
        private DrawButton drowBt;

        public Form1()
        {
            InitializeComponent();
            ThreadPool.SetMinThreads(20, 8);

            button1_Click(null, null);
            button1.Visible = false;
        }

        private DrawRect CreateSimple(Point pos, int width, int height, Color color)
        {
            var cr = new DrawRect(pos, width, height);
            cr.AddValidatedProperty("Color", color);
            cr.AddValidatedProperty("BorderColor", color.DarkColor(50));
            cr.Painters[0].Add(BasePainters.FillRect).Value["Color"] = cr.Property["Color"];
            cr.Painters[0].Add(BasePainters.Rect).Value["Color"] = cr.Property["BorderColor"];
            cr.Alignment = RectAlignment.Absolute;
            cr.BorderSize = 2;
            return cr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rect = TestControl.MainRect;
            rect.BorderSize = 5;
            rect.AnimeChildAlign = true;

            topRect = rect.Add(RectAlignment.Top, 20);
            topRect.AnimeAlign = true;
            topRect.AddValidatedProperty("Color", Color.Red);
            topRect.Painters[0].Add(BasePainters.FillRect).Value["Color"] = topRect.Property["Color"];
            {
                var r2 = topRect.Add(RectAlignment.Right, 30);
                r2.AddValidatedProperty("Color", Color.Indigo);
                r2.Painters[0].Add(BasePainters.FillRect).Value["Color"] = r2.Property["Color"];

                controlRect = topRect.Add(RectAlignment.Right, 60);
                TextBox txt = new TextBox();
                TestControl.Controls.Add(txt);
                controlRect.Control = txt;

                r2 = topRect.Add(RectAlignment.Left, 30);
                r2.AddValidatedProperty("Color", Color.Aqua);
                r2.Painters[0].Add(BasePainters.FillRect).Value["Color"] = r2.Property["Color"];
                r2 = topRect.Add(RectAlignment.Left, 30);
                r2.AddValidatedProperty("Color", Color.Green);
                r2.Painters[0].Add(BasePainters.FillRect).Value["Color"] = r2.Property["Color"];
                yellowRect = topRect.Add(RectAlignment.Left, 30);
                yellowRect.AddValidatedProperty("Color", Color.Yellow);
                yellowRect.Painters[0].Add(BasePainters.FillRect).Value["Color"] = yellowRect.Property["Color"];
                r2 = topRect.Add(RectAlignment.Left, 30);
                r2.AnimeAlign = true;
                r2.AddValidatedProperty("Color", Color.LightSteelBlue);
                r2.Painters[0].Add(BasePainters.FillRect).Value["Color"] = r2.Property["Color"];
            }
            var r = rect.Add(RectAlignment.Bottom, 20);
            r.AddValidatedProperty("Color", Color.Green.BrightColor(40));
            r.AddValidatedProperty("Color2", Color.Green);
            var p = r.Painters[0].Add(BasePainters.FillGradient);
            p.Value["Color"] = r.Property["Color"];
            p.Value["Color2"] = r.Property["Color2"];
            {
                var r2 = r.Add(RectAlignment.Left, 40);
                r2.AddValidatedProperty("Color", Color.Aqua);
                r2.Painters[0].Add(BasePainters.FillRect).Value["Color"] = r2.Property["Color"];
                r2 = r.Add(RectAlignment.Left, 40);
                r2.AddValidatedProperty("Color", Color.Green);
                r2.Painters[0].Add(BasePainters.FillRect).Value["Color"] = r2.Property["Color"];
                r2 = r.Add(RectAlignment.Left, 40);
                r2.AddValidatedProperty("Color", Color.Yellow);
                r2.Painters[0].Add(BasePainters.FillRect).Value["Color"] = r2.Property["Color"];
                r2 = r.Add(RectAlignment.Left, 40);
                r2.AddValidatedProperty("Color", Color.LightSteelBlue);
                r2.Painters[0].Add(BasePainters.FillRect).Value["Color"] = r2.Property["Color"];

                r2 = r.Add(RectAlignment.Right, 40);
                r2.AddValidatedProperty("Color", Color.Indigo);
                r2.Painters[0].Add(BasePainters.FillRect).Value["Color"] = r2.Property["Color"];
            }
            r = rect.Add(RectAlignment.Left, 20);
            r.AddValidatedProperty("Color", Color.Blue);
            r.Painters[0].Add(BasePainters.FillRect).Value["Color"] = r.Property["Color"];
            r = rect.Add(RectAlignment.Right, 20);
            r.AddValidatedProperty("Color", Color.Silver);
            r.AddValidatedProperty("Color2", Color.Gray);
            r.AddValidatedProperty("Direction", SimpleDirection.Horisontal);
            p = r.Painters[0].Add(BasePainters.FillGradient);
            p.Value["Color"] = r.Property["Color"];
            p.Value["Color2"] = r.Property["Color2"];
            p.Value["Direction"] = r.Property["Direction"];

            r = rect.Add(RectAlignment.Right, 20);
            r.AddValidatedProperty("Color", Color.Maroon);
            r.AddValidatedProperty("Color2", Color.Maroon.BrightColor(40));
            r.AddValidatedProperty("Direction", SimpleDirection.Horisontal);
            p = r.Painters[0].Add(BasePainters.FillGradient);
            p.Value["Color"] = r.Property["Color"];
            p.Value["Color2"] = r.Property["Color2"];
            p.Value["Direction"] = r.Property["Direction"];

            backRect = rect.Add(RectAlignment.Fill, 0);
            backRect.BorderSize = 10;
            backRect.AddValidatedProperty("Color", Color.Sienna);
            backRect.AddValidatedProperty("BorderColor", Color.White);
            backRect.Painters[0].Add(BasePainters.FillRect).Value["Color"] = backRect.Property["Color"];
            backRect.Painters[1].Add(BasePainters.Rect).Value["Color"] = backRect.Property["BorderColor"];

            {
                var r2 = backRect.Add(RectAlignment.Fill, 0);
                r2.AddValidatedProperty("Color", Color.Green.BrightColor(60));
                r2.Painters[0].Add(BasePainters.FillRect).Value["Color"] = r2.Property["Color"];

                var locationRect = (ControlRect)r2.Add(new Point(70, 100), 80, 80);
                locationRect.Transparent = true;
                locationRect.BorderSize = 2;
                locationRect.AddValidatedProperty("Color", Color.Red.BrightColor(40).Transparent(40));
                locationRect.AddValidatedProperty("BorderColor", Color.Brown.DarkColor(20));
                locationRect.Painters[0].Add(BasePainters.FillRect).Value["Color"] = locationRect.Property["Color"];
                locationRect.Painters[0].Add(BasePainters.Rect).Value["Color"] = locationRect.Property["BorderColor"]; 
                locationRect.AnimeEvent(true).MouseEnter().Change("ContainerRect", new Rectangle(170, 100, 150, 150));
                locationRect.AnimeEventQueue().MouseLeave().Wait(Speed.Slow).Change("ContainerRect", new Rectangle(70, 100, 80, 80));
            }

            mouseRect = (ControlRect)rect.Add(new Point(80, 10), 60, 60);
            mouseRect.Alignment = RectAlignment.Custom;
            mouseRect.Transparent = true;
            mouseRect.AddValidatedProperty("Color", Color.Blue.Transparent(50));
            mouseRect.Painters[0].Add(BasePainters.FillRect, "r1").Value["Color"] = mouseRect.Property["Color"];
            mouseRect.BorderSize = 4;
            mouseRect.AddValidatedProperty("BorderColor", Color.Navy);
            mouseRect.Painters[0].Add(BasePainters.Rect, "r2").Value["Color"] = mouseRect.Property["BorderColor"];
            mouseRect.AnimeEvent("color").MouseEnter().ChangeDec("Color", Color.Aqua.Transparent(20), Speed.Medium);
            mouseRect.AnimeEvent("size").MouseEnter().ChangeDec("Rect", new Rectangle(80, 10, 180, 180), Speed.Medium);
            mouseRect.AnimeEvent("color").MouseLeave().ChangeInc("Color", Color.Blue.Transparent(50), Speed.Medium);
            mouseRect.AnimeEvent("size").MouseLeave().ChangeDec("Rect", new Rectangle(80, 10, 60, 60), Speed.Medium);
            mouseRect.Painters["r2"].Value["CornerRadius"] = 2;
            mouseRect.Painters["r1"].Value["CornerRadius"] = 2;

            var ctrlRect = rect.Add(new Point(100, 100), 100, 20);
            var txt2 = new TextBox();
            TestControl.Controls.Add(txt2);
            ctrlRect.Control = txt2;

            transparentRect = (ControlRect)rect.Add(new Point(40, 40), 80, 80);
            transparentRect.Alignment = RectAlignment.Custom;
            transparentRect.Transparent = true;
            transparentRect.BorderSize = 4;
            transparentRect.AddValidatedProperty("Color", Color.Green.Transparent(60));
            transparentRect.AddValidatedProperty("BorderColor", Color.Green);
            transparentRect.Painters[0].Add(BasePainters.FillRect, "r1").Value["Color"] = transparentRect.Property["Color"];
            transparentRect.Painters[0].Add(BasePainters.Rect, "r2").Value["Color"] = transparentRect.Property["BorderColor"];
            transparentRect.Painters["r2"].Value["CornerRadius"] = 5;
            transparentRect.Painters["r1"].Value["CornerRadius"] = 5;

            transparentRect.AnimeEvent("color").MouseEnter().Change("Color", Color.Blue.Transparent(80), Speed.Medium);
            transparentRect.AnimeEvent("color").MouseLeave().Change("Color", Color.Green.Transparent(60), Speed.Medium);

            transparentRect.AnimeEvent("size").MouseEnter().ChangeDec("Rect", new Rectangle(40, 40, 160, 160), Speed.Medium);
            transparentRect.AnimeEvent("size").MouseLeave().ChangeDec("Rect", new Rectangle(40, 40, 80, 80), Speed.Medium);
//            transparentRect.MouseEnter += (s, arg) => { s.SetProperty("Size", new Size(160, 160)); };
//            transparentRect.MouseLeave += (s, arg) => { s.SetProperty("Size", new Size(80, 80)); };

            rect.Align();












            //********** tabDrawControl *************************
            tabDrawControl.TabStop = true;
            rect = tabDrawControl.MainRect;

            var cr = new ControlRect(new Point(40, 40), 100, 20);
            rect.Add(cr);
            cr.AddValidatedProperty("Color", Color.Red.BrightColor(40));
            cr.AddValidatedProperty("BorderColor", Color.Maroon);
            cr.Painters[0].Add(BasePainters.FillRect).Value["Color"] = cr.Property["Color"];
            cr.Painters[0].Add(BasePainters.Rect).Value["Color"] = cr.Property["BorderColor"];
            cr.TabIndex = 0;
            cr.TabStop = true;
            cr.BorderSize = 2;
            cr.Enter += (s, arg) => { s.SetProperty("Color", Color.Red.BrightColor(80)); };
            cr.Leave += (s, arg) => { s.SetProperty("Color", Color.Red.BrightColor(40)); };

            cr = new ControlRect(new Point(40, 80), 100, 20);
            cr.Alignment = RectAlignment.Center;
            rect.Add(cr);
            cr.AddValidatedProperty("Color", Color.Green.BrightColor(40));
            cr.AddValidatedProperty("BorderColor", Color.Green);
            cr.Painters[0].Add(BasePainters.FillRect).Value["Color"] = cr.Property["Color"];
            cr.Painters[0].Add(BasePainters.Rect).Value["Color"] = cr.Property["BorderColor"];
            cr.TabIndex = 1;
            cr.TabStop = true;
            cr.BorderSize = 2;
            cr.Enter += (s, arg) => { s.SetProperty("Color", Color.Green.BrightColor(80)); };
            cr.Leave += (s, arg) => { s.SetProperty("Color", Color.Green.BrightColor(40)); };
            

            cr = new ControlRect(new Point(40, 120), 100, 20);
            rect.Add(cr);
            cr.AddValidatedProperty("Color", Color.Blue.BrightColor(40));
            cr.AddValidatedProperty("BorderColor", Color.Navy);
            cr.Painters[0].Add(BasePainters.FillRect).Value["Color"] = cr.Property["Color"];
            cr.Painters[0].Add(BasePainters.Rect).Value["Color"] = cr.Property["BorderColor"];
            cr.TabIndex = 2;
            cr.TabStop = true;
            cr.BorderSize = 2;
            cr.Enter += (s, arg) => { s.SetProperty("Color", Color.Blue.BrightColor(80)); };
            cr.Leave += (s, arg) => { s.SetProperty("Color", Color.Blue.BrightColor(40)); };


            var bt = new DrawButton();
            drowBt = bt;
            bt.Pos = new Point(40, 160);
            bt.Size = new Size(100, 23);
            bt.TabIndex = 3;
            //bt.Color = Color.Aqua;
            bt.MouseOverColor = Color.Blue.BrightColor(70);
            bt.UseMouseOverColor = true;
            bt.BorderSize = 2;
            bt.CornerRadius = 2;
            bt.Gradient = true;
            bt.Transparent = true;
            bt.DrawFocus = true;
            bt.Switch = false;
            bt.Flat = false;
            bt.Text = "Test";
            bt.TextColor = Color.Navy;
            rect.Add(bt);


            var scroll = new DrawScroll();
            scroll.Size = new Size(14, 14);
            scroll.Alignment = RectAlignment.Right;
            scroll.Align = Orientation.Vertical;
            scroll.Color = Color.Blue.BrightColor(75);
            scroll.MaxPosition = 100;
            scroll.Property["Position"].AnyChanged += () => { bt.Text = scroll.Position + " %"; };
            scroll.ScrollButtonAutoSize = true;
            //scroll.BorderSize = 1;
            rect.Add(scroll);


            var crTxt = new TextBox();
            tabDrawControl.Controls.Add(crTxt);
            cr.Control = crTxt;
            crTxt.KeyDown += (s, arg) =>
                                 {
                                     if (arg.KeyCode == Keys.Enter) scroll.Position = int.Parse(crTxt.Text);
                                 };











            //--------- Scrollable
            Scrollable sRect = new Scrollable();
            sRect.Alignment = RectAlignment.Fill;
            ScrollableContainer.MainRect.Add(sRect);
            sRect.ScrollHeight = 400;
            sRect.ScrollWidth = 400;
            sRect.Transparent = true;
            sRect.VirtualScroll = false;

            sRect.Container.Add(CreateSimple(new Point(50, 50), 80, 23, Color.Lime));
            sRect.Container.Add(CreateSimple(new Point(100, 100), 80, 23, Color.Aqua));
            sRect.Container.Add(CreateSimple(new Point(250, 250), 80, 23, Color.Yellow));
            sRect.Container.Add(CreateSimple(new Point(300, 300), 80, 23, Color.Blue.BrightColor(30)));

            var top = CreateSimple(new Point(0, 0), 20, 20, Color.Orange);
            top.Alignment = RectAlignment.Top;
            sRect.Container.Add(top);

            var bottom = CreateSimple(new Point(0, 0), 20, 20, Color.Green);
            bottom.Alignment = RectAlignment.Bottom;
            sRect.Container.Add(bottom);

            var right = CreateSimple(new Point(0, 0), 20, 20, Color.Blue);
            right.Alignment = RectAlignment.Right;
            sRect.Container.Add(right);

            var left = CreateSimple(new Point(0, 0), 20, 20, Color.Silver);
            left.Alignment = RectAlignment.Left;
            sRect.Container.Add(left);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                transparentRect.Value["Color"] = dialog.Color.Transparent(30);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            controlRect.Visible = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            yellowRect.Visible = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            backRect.Visible = checkBox3.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TestControl.Invalidate();
        }

        private void e_topHeight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int n;
                if (int.TryParse(e_topHeight.Text, out n))
                {
                    topRect.Height = n;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (Form1 newForm = new Form1())
            {
                newForm.ShowDialog();
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            this.drowBt.Enabled = checkBox4.Checked;
        }
    }
}

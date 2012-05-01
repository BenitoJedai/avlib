using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        private void button1_Click(object sender, EventArgs e)
        {
            var rect = TestControl.MainRect;
            rect.Painters.Clear();
            rect.Painters[0].FillRect(SystemColors.Control);
            rect.BorderSize = 5;
            rect.AnimeChildAlign = true;

            topRect = rect.Add(RectAlignment.Top, 20);
            topRect.AnimeAlign = true;
            topRect.Painters[0].FillRect(Color.Red);
            {
                var r2 = topRect.Add(RectAlignment.Right, 30);
                r2.Painters[0].FillRect(Color.Indigo);

                controlRect = topRect.Add(RectAlignment.Right, 60);
                TextBox txt = new TextBox();
                TestControl.Controls.Add(txt);
                controlRect.Control = txt;

                r2 = topRect.Add(RectAlignment.Left, 30);
                r2.Painters[0].FillRect(Color.Aqua);
                r2 = topRect.Add(RectAlignment.Left, 30);
                r2.Painters[0].FillRect(Color.Green);
                yellowRect = topRect.Add(RectAlignment.Left, 30);
                yellowRect.Painters[0].FillRect(Color.Yellow);
                r2 = topRect.Add(RectAlignment.Left, 30);
                r2.AnimeAlign = true;
                r2.Painters[0].FillRect(Color.LightSteelBlue);
            }
            var r = rect.Add(RectAlignment.Bottom, 20);
            r.Painters[0].FillRectGradient(SimpleDirection.Vertical, Color.Green.BrightColor(40), Color.Green);
            {
                var r2 = r.Add(RectAlignment.Left, 40);
                r2.Painters[0].FillRect(Color.Aqua);
                r2 = r.Add(RectAlignment.Left, 40);
                r2.Painters[0].FillRect(Color.Green);
                r2 = r.Add(RectAlignment.Left, 40);
                r2.Painters[0].FillRect(Color.Yellow);
                r2 = r.Add(RectAlignment.Left, 40);
                r2.Painters[0].FillRect(Color.LightSteelBlue);

                r2 = r.Add(RectAlignment.Right, 40);
                r2.Painters[0].FillRect(Color.Indigo);
            }
            r = rect.Add(RectAlignment.Left, 20);
            r.Painters[0].FillRect(Color.Blue);
            r = rect.Add(RectAlignment.Right, 20);
            r.Painters[0].FillRectGradient(SimpleDirection.Horisontal, Color.Silver, Color.Gray);
            r = rect.Add(RectAlignment.Right, 20);
            r.Painters[0].FillRectGradient(SimpleDirection.Horisontal, Color.Maroon, Color.Maroon.BrightColor(40));

            backRect = rect.Add(RectAlignment.Fill, 0);
            backRect.BorderSize = 10;
            backRect.Painters[0].FillRect(Color.Sienna);
            backRect.Painters[1].DrawRectangle(Color.White, 4);

            {
                var r2 = backRect.Add(RectAlignment.Fill, 0);
                r2.Painters[0].FillRect(Color.Green.BrightColor(60));

                var locationRect = (ControlRect)r2.Add(new Point(70, 100), 80, 80);
                locationRect.Transparent = true;
                locationRect.BorderSize = 2;
                locationRect.Painters[0].FillRect(Color.Red.BrightColor(40).Transparent(40));
                locationRect.Painters[0].DrawRectangle(Color.Brown.DarkColor(20), 2);
                locationRect.MouseEnter += (s, args) =>
                                               {
                                                   s.AnimeCancel();
                                                   s.Anime(true).Change("ContainerRect", new Rectangle(170, 100, 150, 150));
                                               };
                locationRect.MouseLeave += (s, args) =>
                                               {
                                                   s.AnimeQueue().Wait(Speed.Slow).Change("ContainerRect", new Rectangle(70, 100, 80, 80));
                                               };
            }

            mouseRect = (ControlRect)rect.Add(new Point(80, 10), 60, 60);
            mouseRect.Transparent = true;
            mouseRect.Painters[0].FillRect(Color.Blue.Transparent(50), "r1");
            mouseRect.BorderSize = 3;
            mouseRect.Painters[0].DrawRectangle(Color.Navy, 3);
            mouseRect.MouseEnter += (s, args) =>
                                                {
                                                    ((ControlRect)s).Painters["r1"].AnimeCancel();
                                                    s.AnimeCancel();
                                                    ((ControlRect)s).Painters["r1"].Anime().ChangeDec("Color", Color.Aqua.Transparent(20), Speed.Medium);
                                                    s.Anime().ChangeDec("ContainerRect", new Rectangle(80, 10, 180, 180), Speed.Medium);
                                                };
            mouseRect.MouseLeave += (s, args) =>
                                                {
                                                    ((ControlRect)s).Painters["r1"].AnimeCancel();
                                                    s.AnimeCancel();
                                                    ((ControlRect)s).Painters["r1"].Anime().ChangeInc("Color", Color.Blue.Transparent(50), Speed.Medium);
                                                    s.Anime().ChangeDec("ContainerRect", new Rectangle(80, 10, 60, 60), Speed.Medium);
                                                };

            var ctrlRect = rect.Add(new Point(100, 100), 100, 20);
            var txt2 = new TextBox();
            TestControl.Controls.Add(txt2);
            ctrlRect.Control = txt2;

            transparentRect = (ControlRect)rect.Add(new Point(40, 40), 80, 80);
            transparentRect.Transparent = true;
            transparentRect.BorderSize = 2;
            transparentRect.Painters[0].FillRect(Color.Green.Transparent(60), "transparent");
            transparentRect.Painters[0].DrawRectangle(Color.Green, 2);
            transparentRect.MouseEnter += (s, args) =>
                                        {
                                            ((ControlRect)s).Painters["transparent"].AnimeCancel();
                                            //s.AnimeCancel();
                                            ((ControlRect)s).Painters["transparent"].Anime().Change("Color", Color.Blue.Transparent(80), Speed.Medium);
                                            //s.Anime().ChangeDec("ContainerRect", new Rectangle(40, 40, 160, 160), Speed.Medium);
                                        };
            transparentRect.MouseLeave += (s, args) =>
                                        {
                                            ((ControlRect)s).Painters["transparent"].AnimeCancel();
                                            //s.AnimeCancel();
                                            ((ControlRect)s).Painters["transparent"].Anime().Change("Color", Color.Green.Transparent(60), Speed.Medium);
                                            //s.Anime().ChangeDec("ContainerRect", new Rectangle(40, 40, 80, 80), Speed.Medium);
                                        };
            transparentRect.AnimeEvent(Speed.Medium).MouseEnter().ChangeDec("ContainerRect", new Rectangle(40, 40, 160, 160));
            transparentRect.AnimeEvent(Speed.Medium).MouseLeave().ChangeDec("ContainerRect", new Rectangle(40, 40, 80, 80));

            rect.Align();












            //********** tabDrawControl *************************
            tabDrawControl.TabStop = true;
            rect = tabDrawControl.MainRect;

            var cr = new ControlRect(new Point(40, 40), 100, 20);
            rect.Add(cr);
            cr.Painters[0].FillRect(Color.Red.BrightColor(40), "color");
            cr.Painters[0].DrawRectangle(Color.Maroon, 2);
            cr.TabIndex = 0;
            cr.TabStop = true;
            cr.Enter += (s, arg) =>
                            {
                                ((ControlRect)s).Painters["color"].SetProperty("Color", Color.Red.BrightColor(80));
                            };
            cr.Leave += (s, arg) => { ((ControlRect)s).Painters["color"].SetProperty("Color", Color.Red.BrightColor(40)); };

            cr = new ControlRect(new Point(40, 80), 100, 20);
            cr.Alignment = RectAlignment.Center;
            rect.Add(cr);
            cr.Painters[0].FillRect(Color.Green.BrightColor(40), "color");
            cr.Painters[0].DrawRectangle(Color.Green, 2);
            cr.TabIndex = 1;
            cr.TabStop = true;
            cr.Enter += (s, arg) => { ((ControlRect)s).Painters["color"].SetProperty("Color", Color.Green.BrightColor(80)); };
            cr.Leave += (s, arg) => { ((ControlRect)s).Painters["color"].SetProperty("Color", Color.Green.BrightColor(40)); };

            cr = new ControlRect(new Point(40, 120), 100, 20);
            rect.Add(cr);
            cr.Painters[0].FillRect(Color.Blue.BrightColor(40), "color");
            cr.Painters[0].DrawRectangle(Color.Navy, 2);
            cr.TabIndex = 2;
            cr.TabStop = true;
            cr.Enter += (s, arg) => { ((ControlRect)s).Painters["color"].SetProperty("Color", Color.Blue.BrightColor(80)); };
            cr.Leave += (s, arg) => { ((ControlRect)s).Painters["color"].SetProperty("Color", Color.Blue.BrightColor(40)); };


            var bt = new DrawButton();
            drowBt = bt;
            bt.Pos = new Point(40, 160);
            bt.Size = new Size(100, 23);
            bt.TabIndex = 3;
            //bt.Color = Color.Aqua;
            bt.MouseOverColor = Color.Blue.BrightColor(70);
            bt.BorderSize = 2;
            bt.CornerRadius = 2;
            bt.Gradient = true;
            bt.Transparent = true;
            bt.DrawFocus = true;
            bt.Switch = false;
            bt.Flat = false;
            bt.Text = "Test";
            bt.TextColor = Color.Navy;
            bt.CaptuReDown = true;
            rect.Add(bt);


            var scroll = new DrawScroll();
            scroll.Size = new Size(16, 16);
            scroll.Alignment = RectAlignment.Right;
            scroll.Align = DrawScrollAlign.Vertical;
            scroll.BorderSize = 1;
            rect.Add(scroll);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ((PainterFillRect)transparentRect.Painters["transparent"]).Color = dialog.Color.Transparent(30);
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

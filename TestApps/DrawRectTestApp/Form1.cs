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

namespace DrawRectTestApp
{
    public partial class Form1 : Form
    {
        private ControlRect transparentRect;
        private DrawRect controlRect;
        private DrawRect yellowRect;
        private DrawRect backRect;
        private ControlRect mouseRect;

        public Form1()
        {
            InitializeComponent();
            ThreadPool.SetMinThreads(20, 8);

            button1_Click(null, null);
            button1.Visible = false;

            btEnterLeave.AnimeEventQueue().Click().Wait(Speed.Slow, (d) =>
                                                                   {
                                                                       mouseRect.OnMouseEnter(new EventArgs());
                                                                   }).Wait(Speed.Slow, (d) =>
                                                                                           {
                                                                                               mouseRect.OnMouseLeave(new EventArgs());
                                                                                           });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rect = TestControl.MainRect;
            rect.Painters.Clear();
            rect.Painters[0].FillRect(SystemColors.Control);
            rect.BorderSize = 5;

            var r = rect.Add(RectAlignment.Top, 20);
            r.Painters[0].FillRect(Color.Red);
            {
                var r2 = r.Add(RectAlignment.Right, 30);
                r2.Painters[0].FillRect(Color.Indigo);

                controlRect = r.Add(RectAlignment.Right, 60);
                TextBox txt = new TextBox();
                TestControl.Controls.Add(txt);
                controlRect.Control = txt;

                r2 = r.Add(RectAlignment.Left, 30);
                r2.Painters[0].FillRect(Color.Aqua);
                r2 = r.Add(RectAlignment.Left, 30);
                r2.Painters[0].FillRect(Color.Green);
                yellowRect = r.Add(RectAlignment.Left, 30);
                yellowRect.Painters[0].FillRect(Color.Yellow);
                r2 = r.Add(RectAlignment.Left, 30);
                r2.Painters[0].FillRect(Color.LightSteelBlue);
            }
            r = rect.Add(RectAlignment.Bottom, 20);
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
            }

            mouseRect = (ControlRect)rect.Add(new Point(80, 20), 60, 60);
            mouseRect.Painters[0].FillRect(Color.Blue, "r1");
            mouseRect.BorderSize = 3;
            mouseRect.Painters[0].DrawRectangle(Color.Red.BrightColor(30), 3);
            mouseRect.MouseEnter += (s, args) =>
                                                {
                                                    ((ControlRect)s).Painters["r1"].AnimeCancel();
                                                    s.AnimeCancel();
                                                    ((ControlRect)s).Painters["r1"].Anime().ChangeDec("Color", Color.Aqua, Speed.Medium);
                                                    s.Anime().ChangeDec("Size", new Size(180, 180), Speed.Medium);
                                                };
            mouseRect.MouseLeave += (s, args) =>
                                                {
                                                    ((ControlRect)s).Painters["r1"].AnimeCancel();
                                                    s.AnimeCancel();
                                                    ((ControlRect)s).Painters["r1"].Anime().ChangeInc("Color", Color.Blue, Speed.Medium);
                                                    s.Anime().ChangeDec("Size", new Size(60, 60), Speed.Medium);
                                                };

            transparentRect = (ControlRect)rect.Add(new Point(40, 40), 80, 80);
            transparentRect.Transparent = true;
            transparentRect.BorderSize = 2;
            transparentRect.Painters[0].FillRect(Color.Green.Transparent(40), "transparent");
            transparentRect.Painters[0].DrawRectangle(Color.Yellow, 2);
            transparentRect.MouseEnter += (s, args) =>
                                        {
                                            ((ControlRect)s).Painters["transparent"].AnimeCancel();
                                            s.AnimeCancel();
                                            ((ControlRect)s).Painters["transparent"].Anime().ChangeDec("Color", Color.Yellow.Transparent(40), Speed.Medium);
                                            s.Anime().ChangeDec("Size", new Size(160, 160), Speed.Medium);
                                        };
            transparentRect.MouseLeave += (s, args) =>
                                        {
                                            ((ControlRect)s).Painters["transparent"].AnimeCancel();
                                            s.AnimeCancel();
                                            ((ControlRect)s).Painters["transparent"].Anime().ChangeInc("Color", Color.Green.Transparent(40), Speed.Medium);
                                            s.Anime().ChangeDec("Size", new Size(80, 80), Speed.Medium);
                                        };

            var locationRect = (ControlRect)rect.Add(new Point(100, 150), 80, 80);
            locationRect.Transparent = true;
            locationRect.BorderSize = 2;
            locationRect.Painters[0].FillRect(Color.Red.BrightColor(40).Transparent(40));
            locationRect.Painters[0].DrawRectangle(Color.Navy, 2);
            locationRect.MouseEnter += (s, args) =>
                                           {
                                               s.AnimeCancel();
                                               s.Anime(true).Change("Rect", new Rectangle(200, 150, 150,  150));
                                           };
            locationRect.MouseLeave += (s, args) =>
                                           {
                                               s.AnimeQueue().Wait(Speed.Slow).Change("Rect", new Rectangle(100, 150, 80, 80));
                                           };

            rect.Invalidate();
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
    }
}

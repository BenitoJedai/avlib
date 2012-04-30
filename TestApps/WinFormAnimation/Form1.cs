using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AVLib.Animations;
using AVLib.Utils;

namespace WinFormAnimation
{
    public partial class Form1 : Form
    {
        private Color panelColor;
        private Color widthPColor;

        private AnimeCollector DownUpCollector = new AnimeCollector();

        public Form1()
        {
            InitializeComponent();

            panelColor = Color.FromArgb(192, 192, 255);
            widthPColor = SystemColors.Control;
            ThreadPool.SetMaxThreads(100, 80);
            ThreadPool.QueueUserWorkItem(new WaitCallback((d) => { }), null);

            btnNewForm.MouseEnter += (sender, args) =>
                                         {
                                             ((Control)sender).AnimeCancel();
                                             ((Control)sender).Anime().Color(Color.Aqua);
                                         };
            btnNewForm.MouseLeave += (sender, args) =>
                                         {
                                             ((Control)sender).AnimeCancel();
                                             ((Control)sender).Anime().Color(SystemColors.Control);
                                         };

            autoPanel.AnimeEvent().MouseEnter().Height(100).Width(200).Color(Color.Yellow);
            autoPanel.AnimeEvent().MouseLeave().Height(50).Width(100).Color(SystemColors.Control);
            autoPanel.AnimeEvent().MouseDown().Color(Color.Lime);
            autoPanel.AnimeEvent().MouseUp().Color(Color.Yellow);

            txtEnterLeave.AnimeEvent().Enter().Color(Color.Aqua.BrightColor(20), Speed.Slow);
            txtEnterLeave.AnimeEvent().Leave().Color(Color.White);
            txtEnterLeave.AnimeEventQueue().TextChanged().Color(Color.Lime).Color(Color.Aqua.BrightColor(20));
            txtEnterLeave.AnimeEventQueue().Visible().Color(Color.Red).Color(Color.White);
            txtEnterLeave.AnimeEvent().Enabled().Color(Color.White);
            txtEnterLeave.AnimeEvent().Disabled().Color(Color.Silver, Speed.Slow);
            txtEnterLeave.AnimeEventQueue().Leave().ForeColor(Color.Yellow, Speed.Medium).ForeColor(Color.Blue,Speed.Medium).ForeColor(Color.Black);
            txtEnterLeave.AnimeEventQueue().MouseEnter().Color(Color.Yellow).Color(Color.White);

            chbEnabled.AnimeEventQueue().Click().ForeColor(Color.Yellow, Speed.Medium).ForeColor(Color.Yellow).ForeColor(Color.Black, Speed.Medium);


            DownUpCollector.AnimeEvent().MouseEnter().HeightDec(100, Speed.Medium);
            DownUpCollector.AnimeEventQueue().MouseEnter().Color(Color.Aqua).ColorDec(Color.Ivory, Speed.Medium);
            DownUpCollector.AnimeEvent().MouseLeave().HeightDec(50, Speed.Medium);
            DownUpCollector.AnimeEventQueue().MouseLeave().ColorInc(Color.Aqua, Speed.Medium).Color(panelColor);

            DownUpCollector.ApplyToControl(new Control[] {panel, panel2, panel3, panel4});

            chbEnabled.AnimeEvent().MouseEnter().Wait(1000, (d) =>
                                                               {
                                                                   if (!d.Canceled)
                                                                    Form2.ShowPoppup((Control)d.QueueMethodParam.control);
                                                               });
            chbEnabled.AnimeEvent().MouseLeave().Wait(10);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel.AnimeCancel();
            panel.AnimeQueue().Highlight(10).Height(100).HeightInc(50).Highlight(10);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel.AnimeCancel();
            panel.AnimeQueue().Highlight(10).HeightDec(100, 2000).HeightInc(50, 2000).Highlight(10);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel.AnimeCancel("down-up");
            panel.AnimeQueue("color").ColorDec(Color.Lime, 2000).ColorInc(panelColor, 2000);
            panel.AnimeQueue("down-up").HeightDec(100, 2000).HeightInc(50, 2000);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel.AnimeForceAll();
            panel.AnimeQueue("color").Color(Color.Yellow, Speed.Slow).Color(Color.Red, Speed.Slow).Color(Color.Lime, Speed.Slow).
                Color(Color.Blue, Speed.Slow).Color(Color.Maroon, Speed.Slow).Color(Color.Aqua, Speed.Slow).
                Color(panelColor, Speed.Slow);
        }

        private void widthP_MouseEnter(object sender, EventArgs e)
        {
            ((Panel)sender).AnimeCancel();
            ((Panel)sender).Anime().WidthDec(120, Speed.Medium).ColorDec(Color.Lime, Speed.Medium);
        }

        private void widthP_MouseLeave(object sender, EventArgs e)
        {
            ((Panel)sender).AnimeCancel();
            ((Panel)sender).Anime().WidthInc(60, Speed.Medium).ColorInc(widthPColor, Speed.Medium);
        }

        private void pAllSize_MouseEnter(object sender, EventArgs e)
        {
            pAllSize.AnimeCancel();
            pAllSize.Anime().WidthDec(200, 600).HeightDec(100, 600);
            pAllSize.AnimeQueue().Color(Color.Lime, 300).Color(Color.Red, 300);
        }

        private void pAllSize_MouseLeave(object sender, EventArgs e)
        {
            pAllSize.AnimeCancel();
            pAllSize.Anime().WidthInc(100, 600).HeightInc(50, 600);
            pAllSize.AnimeQueue().Color(Color.Lime, 300).Color(Color.AliceBlue, 300);
        }

        private void pAll2_MouseEnter(object sender, EventArgs e)
        {
            pAll2.AnimeCancel();
            pAll2.AnimeQueue().WidthDec(150, 300).Wait(300).WidthDec(200, 300);
            pAll2.AnimeQueue().HeightDec(75, 300).Wait(300).HeightDec(100, 300);
            pAll2.AnimeQueue().Color(Color.Lime, 400).Wait(100).Color(Color.Aqua, 400);
        }

        private void pAll2_MouseLeave(object sender, EventArgs e)
        {
            pAll2.AnimeCancel();
            pAll2.AnimeQueue().WidthInc(150, 300).Wait(300).WidthInc(100, 300);
            pAll2.AnimeQueue().HeightInc(75, 300).Wait(300).HeightInc(50, 300);
            pAll2.AnimeQueue().Color(Color.Lime, 400).Wait(100).Color(Color.AliceBlue, 400);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(() =>
                                       {
                                           using (Form1 f = new Form1())
                                           {
                                               f.ShowDialog();
                                           }
                                       });
            th.Start();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtEnterLeave.Visible = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            txtEnterLeave.Enabled = chbEnabled.Checked;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            panel.AnimeQueue().HeightDec(100, (d) =>
                {
                    panel2.AnimeQueue().HeightDec(100, (d2) =>
                           {
                               panel3.AnimeQueue().HeightDec(100, (d3) =>
                                      {
                                          panel4.AnimeQueue().HeightDec(100).HeightInc(50);
                                      })
                                   .HeightInc(50);
                           }
                        ).HeightInc(50);
                }).HeightInc(50);
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            (sender as Label).AnimeCancel();
            (sender as Label).AnimeQueue().ForeColor(Color.Yellow, Speed.Medium).ForeColor(Color.Aqua, Speed.Medium).ForeColor(Color.Red).ForeColor(Color.Black, Speed.Medium);
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel.AnimeForceAll();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AVLib.Animations;

namespace WinFormAnimation
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Shown += new EventHandler(Form2_Shown);
            panel1.MouseEnter += (s, a) =>
                                   {
                                       mouseEntered = true;
                                       Trace.WriteLine("Mouse entered: form");
                                   };
            panel1.MouseLeave += (s, a) =>
                                   {
                                       
                                       Trace.WriteLine("Mouse leave: form");
                                       var pt = Cursor.Position;
                                       var rect = new Rectangle(this.PointToScreen(new Point(0, 0)), this.Size);
                                       if (!rect.Contains(pt)) Close();
                                   };
        }

        void Form2_Shown(object sender, EventArgs e)
        {
            this.Anime().HeightDec(initialSize.Height, Speed.Medium).WidthDec(initialSize.Width, Speed.Medium);
            this.Anime().ColorDec(Color.Yellow, Speed.Slow);
            dependControl.MouseLeave += new EventHandler(dependControl_MouseLeave);
        }

        void dependControl_MouseLeave(object sender, EventArgs e)
        {
            Trace.WriteLine("Mouse leave: control");
            this.Anime().Wait(500, (d) =>
                                       {
                                           Trace.WriteLine("Wait after leave: control");
                                           if (!mouseEntered)
                                           {
                                               Trace.WriteLine("Not form entered => close");
                                               Close();
                                           }
                                       });
        }

        private Size initialSize = new Size(0, 0);
        private Control dependControl = null;

        private bool mouseEntered = false;

        public static void ShowPoppup(Control forControl)
        {
            var pt = forControl.PointToScreen(new Point(0, 0));
            pt.Y += forControl.Height;
            Form2 f = new Form2();
            f.initialSize = f.Size;
            f.dependControl = forControl;
            f.Size= new Size(5, 5);
            f.Location = pt;
            f.Show();
        }
    }
}

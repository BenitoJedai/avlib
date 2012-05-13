namespace DrawRectTestApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.e_topHeight = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.TestControl = new AVLib.Controls.DrawControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabDrawControl = new AVLib.Controls.DrawControl();
            this.tabScrollable = new System.Windows.Forms.TabPage();
            this.ScrollableContainer = new AVLib.Controls.DrawControl();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabScrollable.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox4);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.e_topHeight);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.checkBox3);
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(446, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(122, 354);
            this.panel1.TabIndex = 3;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(12, 288);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(101, 17);
            this.checkBox4.TabIndex = 12;
            this.checkBox4.Text = "drowBt Enabled";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(20, 25);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(90, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "New Form";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 236);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Top height:";
            // 
            // e_topHeight
            // 
            this.e_topHeight.Location = new System.Drawing.Point(7, 253);
            this.e_topHeight.Name = "e_topHeight";
            this.e_topHeight.Size = new System.Drawing.Size(100, 20);
            this.e_topHeight.TabIndex = 9;
            this.e_topHeight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.e_topHeight_KeyDown);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(20, 210);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Invalidate";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(20, 168);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(82, 17);
            this.checkBox3.TabIndex = 7;
            this.checkBox3.Text = "back visible";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(20, 145);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(87, 17);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "yellow visible";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(20, 122);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(69, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "txt visible";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(20, 66);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 40);
            this.button2.TabIndex = 4;
            this.button2.Text = "Color for transparent";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(37, 15);
            this.button1.TabIndex = 3;
            this.button1.Text = "Create rects";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabScrollable);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(446, 354);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.TestControl);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(438, 328);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // TestControl
            // 
            this.TestControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TestControl.Location = new System.Drawing.Point(3, 3);
            this.TestControl.Name = "TestControl";
            this.TestControl.Size = new System.Drawing.Size(432, 322);
            this.TestControl.TabIndex = 5;
            this.TestControl.Text = "drawControl1";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabDrawControl);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(438, 328);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabDrawControl
            // 
            this.tabDrawControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDrawControl.Location = new System.Drawing.Point(3, 3);
            this.tabDrawControl.Name = "tabDrawControl";
            this.tabDrawControl.Size = new System.Drawing.Size(432, 322);
            this.tabDrawControl.TabIndex = 0;
            this.tabDrawControl.Text = "drawControl1";
            // 
            // tabScrollable
            // 
            this.tabScrollable.Controls.Add(this.ScrollableContainer);
            this.tabScrollable.Location = new System.Drawing.Point(4, 22);
            this.tabScrollable.Name = "tabScrollable";
            this.tabScrollable.Padding = new System.Windows.Forms.Padding(3);
            this.tabScrollable.Size = new System.Drawing.Size(438, 328);
            this.tabScrollable.TabIndex = 2;
            this.tabScrollable.Text = "tabPage3";
            this.tabScrollable.UseVisualStyleBackColor = true;
            // 
            // ScrollableContainer
            // 
            this.ScrollableContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScrollableContainer.Location = new System.Drawing.Point(3, 3);
            this.ScrollableContainer.Name = "ScrollableContainer";
            this.ScrollableContainer.Size = new System.Drawing.Size(432, 322);
            this.ScrollableContainer.TabIndex = 0;
            this.ScrollableContainer.Text = "drawControl1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 354);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabScrollable.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox e_topHeight;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private AVLib.Controls.DrawControl TestControl;
        private System.Windows.Forms.TabPage tabPage2;
        private AVLib.Controls.DrawControl tabDrawControl;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.TabPage tabScrollable;
        private AVLib.Controls.DrawControl ScrollableContainer;

    }
}


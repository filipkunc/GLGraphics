namespace GLTestApp
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
            this.components = new System.ComponentModel.Container();
            this.glView1 = new GLWrapper.GLView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelGL = new System.Windows.Forms.Label();
            this.labelGDI = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glView1
            // 
            this.glView1.BackColor = System.Drawing.Color.White;
            this.glView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glView1.Location = new System.Drawing.Point(0, 23);
            this.glView1.Margin = new System.Windows.Forms.Padding(0);
            this.glView1.Name = "glView1";
            this.glView1.Size = new System.Drawing.Size(530, 486);
            this.glView1.TabIndex = 0;
            this.glView1.Click += new System.EventHandler(this.glView1_Click);
            this.glView1.PaintCanvas += new System.EventHandler<GLWrapper.CanvasEventArgs>(this.glView1_PaintCanvas);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 23);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(534, 486);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // labelGL
            // 
            this.labelGL.AutoSize = true;
            this.labelGL.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelGL.Location = new System.Drawing.Point(0, 0);
            this.labelGL.Name = "labelGL";
            this.labelGL.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.labelGL.Size = new System.Drawing.Size(47, 23);
            this.labelGL.TabIndex = 2;
            this.labelGL.Text = "OpenGL";
            // 
            // labelGDI
            // 
            this.labelGDI.AutoSize = true;
            this.labelGDI.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelGDI.Location = new System.Drawing.Point(0, 0);
            this.labelGDI.Name = "labelGDI";
            this.labelGDI.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.labelGDI.Size = new System.Drawing.Size(32, 23);
            this.labelGDI.TabIndex = 3;
            this.labelGDI.Text = "GDI+";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel1.Controls.Add(this.labelGDI);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.glView1);
            this.splitContainer1.Panel2.Controls.Add(this.labelGL);
            this.splitContainer1.Size = new System.Drawing.Size(1068, 509);
            this.splitContainer1.SplitterDistance = 534;
            this.splitContainer1.TabIndex = 4;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 509);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GLWrapper.GLView glView1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelGDI;
        private System.Windows.Forms.Label labelGL;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Timer timer1;
    }
}


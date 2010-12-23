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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.glView1 = new GLWrapper.GLView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // glView1
            // 
            this.glView1.BackColor = System.Drawing.Color.White;
            this.glView1.Location = new System.Drawing.Point(90, 78);
            this.glView1.Name = "glView1";
            this.glView1.SharedContextView = null;
            this.glView1.Size = new System.Drawing.Size(182, 121);
            this.glView1.TabIndex = 0;
            this.glView1.ViewOffset = ((System.Drawing.PointF)(resources.GetObject("glView1.ViewOffset")));
            this.glView1.PaintCanvas += new System.EventHandler<GLWrapper.CanvasEventArgs>(this.glView1_PaintCanvas);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(372, 126);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 363);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.glView1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GLWrapper.GLView glView1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}


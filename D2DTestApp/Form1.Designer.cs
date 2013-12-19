namespace D2DTestApp
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
            this.panel1 = new D2DView();
            this.checkBoxD2DEnabled = new System.Windows.Forms.CheckBox();
            this.labelFPS = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(12, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1089, 653);
            this.panel1.TabIndex = 0;
            // 
            // checkBoxD2DEnabled
            // 
            this.checkBoxD2DEnabled.AutoSize = true;
            this.checkBoxD2DEnabled.Location = new System.Drawing.Point(12, 12);
            this.checkBoxD2DEnabled.Name = "checkBoxD2DEnabled";
            this.checkBoxD2DEnabled.Size = new System.Drawing.Size(110, 17);
            this.checkBoxD2DEnabled.TabIndex = 1;
            this.checkBoxD2DEnabled.Text = "Direct2D Enabled";
            this.checkBoxD2DEnabled.UseVisualStyleBackColor = true;
            this.checkBoxD2DEnabled.CheckedChanged += new System.EventHandler(this.checkBoxD2DEnabled_CheckedChanged);
            // 
            // labelFPS
            // 
            this.labelFPS.AutoSize = true;
            this.labelFPS.Location = new System.Drawing.Point(166, 13);
            this.labelFPS.Name = "labelFPS";
            this.labelFPS.Size = new System.Drawing.Size(30, 13);
            this.labelFPS.TabIndex = 3;
            this.labelFPS.Text = "FPS:";
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
            this.ClientSize = new System.Drawing.Size(1113, 700);
            this.Controls.Add(this.labelFPS);
            this.Controls.Add(this.checkBoxD2DEnabled);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private D2DView panel1;
        private System.Windows.Forms.CheckBox checkBoxD2DEnabled;
        private System.Windows.Forms.Label labelFPS;
        private System.Windows.Forms.Timer timer1;
    }
}


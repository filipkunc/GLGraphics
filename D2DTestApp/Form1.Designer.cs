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
            this.labelFPS = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panelGdipDrawLines = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panelGdipForDrawLine = new System.Windows.Forms.PictureBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panelD2DHW = new D2DTestApp.D2DView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.panelD2DSW = new D2DTestApp.D2DView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.panelGL = new GLWrapper.GLView();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.panelGdi32 = new System.Windows.Forms.PictureBox();
            this.numericUpDownStep = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownAmplitude = new System.Windows.Forms.NumericUpDown();
            this.checkBoxAntialiasing = new System.Windows.Forms.CheckBox();
            this.buttonRunAutomaticTest = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.checkBoxDownsample = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelGdipDrawLines)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelGdipForDrawLine)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelGdi32)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAmplitude)).BeginInit();
            this.SuspendLayout();
            // 
            // labelFPS
            // 
            this.labelFPS.AutoSize = true;
            this.labelFPS.Location = new System.Drawing.Point(15, 10);
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
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Location = new System.Drawing.Point(12, 43);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1089, 645);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panelGdipDrawLines);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1081, 619);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "GDI+ DrawLines";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panelGdipDrawLines
            // 
            this.panelGdipDrawLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGdipDrawLines.Location = new System.Drawing.Point(3, 3);
            this.panelGdipDrawLines.Name = "panelGdipDrawLines";
            this.panelGdipDrawLines.Size = new System.Drawing.Size(1075, 613);
            this.panelGdipDrawLines.TabIndex = 0;
            this.panelGdipDrawLines.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panelGdipForDrawLine);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1081, 619);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "GDI+ for() DrawLine";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panelGdipForDrawLine
            // 
            this.panelGdipForDrawLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGdipForDrawLine.Location = new System.Drawing.Point(3, 3);
            this.panelGdipForDrawLine.Name = "panelGdipForDrawLine";
            this.panelGdipForDrawLine.Size = new System.Drawing.Size(1075, 613);
            this.panelGdipForDrawLine.TabIndex = 0;
            this.panelGdipForDrawLine.TabStop = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panelD2DHW);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1081, 619);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Direct2D HW";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panelD2DHW
            // 
            this.panelD2DHW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelD2DHW.Location = new System.Drawing.Point(3, 3);
            this.panelD2DHW.Name = "panelD2DHW";
            this.panelD2DHW.Size = new System.Drawing.Size(1075, 613);
            this.panelD2DHW.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.panelD2DSW);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1081, 619);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Direct2D SW";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // panelD2DSW
            // 
            this.panelD2DSW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelD2DSW.Location = new System.Drawing.Point(3, 3);
            this.panelD2DSW.Name = "panelD2DSW";
            this.panelD2DSW.Size = new System.Drawing.Size(1075, 613);
            this.panelD2DSW.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.panelGL);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1081, 619);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "OpenGL";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // panelGL
            // 
            this.panelGL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGL.Location = new System.Drawing.Point(3, 3);
            this.panelGL.Name = "panelGL";
            this.panelGL.Size = new System.Drawing.Size(1075, 613);
            this.panelGL.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.panelGdi32);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(1081, 619);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "GDI32 MoveToEx, for() LineTo";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // panelGdi32
            // 
            this.panelGdi32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGdi32.Location = new System.Drawing.Point(3, 3);
            this.panelGdi32.Name = "panelGdi32";
            this.panelGdi32.Size = new System.Drawing.Size(1075, 613);
            this.panelGdi32.TabIndex = 0;
            this.panelGdi32.TabStop = false;
            // 
            // numericUpDownStep
            // 
            this.numericUpDownStep.DecimalPlaces = 1;
            this.numericUpDownStep.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownStep.Location = new System.Drawing.Point(227, 10);
            this.numericUpDownStep.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownStep.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownStep.Name = "numericUpDownStep";
            this.numericUpDownStep.Size = new System.Drawing.Size(52, 20);
            this.numericUpDownStep.TabIndex = 5;
            this.numericUpDownStep.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            // 
            // numericUpDownAmplitude
            // 
            this.numericUpDownAmplitude.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownAmplitude.Location = new System.Drawing.Point(307, 10);
            this.numericUpDownAmplitude.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownAmplitude.Name = "numericUpDownAmplitude";
            this.numericUpDownAmplitude.Size = new System.Drawing.Size(52, 20);
            this.numericUpDownAmplitude.TabIndex = 5;
            this.numericUpDownAmplitude.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // checkBoxAntialiasing
            // 
            this.checkBoxAntialiasing.AutoSize = true;
            this.checkBoxAntialiasing.Checked = true;
            this.checkBoxAntialiasing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAntialiasing.Location = new System.Drawing.Point(414, 11);
            this.checkBoxAntialiasing.Name = "checkBoxAntialiasing";
            this.checkBoxAntialiasing.Size = new System.Drawing.Size(79, 17);
            this.checkBoxAntialiasing.TabIndex = 6;
            this.checkBoxAntialiasing.Text = "Antialiasing";
            this.checkBoxAntialiasing.UseVisualStyleBackColor = true;
            // 
            // buttonRunAutomaticTest
            // 
            this.buttonRunAutomaticTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRunAutomaticTest.Location = new System.Drawing.Point(978, 12);
            this.buttonRunAutomaticTest.Name = "buttonRunAutomaticTest";
            this.buttonRunAutomaticTest.Size = new System.Drawing.Size(123, 27);
            this.buttonRunAutomaticTest.TabIndex = 7;
            this.buttonRunAutomaticTest.Text = "Run Automatic Test";
            this.buttonRunAutomaticTest.UseVisualStyleBackColor = true;
            this.buttonRunAutomaticTest.Click += new System.EventHandler(this.buttonRunAutomaticTest_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(892, 15);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(80, 20);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 8;
            this.progressBar1.Visible = false;
            // 
            // checkBoxDownsample
            // 
            this.checkBoxDownsample.AutoSize = true;
            this.checkBoxDownsample.Checked = true;
            this.checkBoxDownsample.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDownsample.Location = new System.Drawing.Point(528, 9);
            this.checkBoxDownsample.Name = "checkBoxDownsample";
            this.checkBoxDownsample.Size = new System.Drawing.Size(87, 17);
            this.checkBoxDownsample.TabIndex = 6;
            this.checkBoxDownsample.Text = "Downsample";
            this.checkBoxDownsample.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 700);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.buttonRunAutomaticTest);
            this.Controls.Add(this.checkBoxDownsample);
            this.Controls.Add(this.checkBoxAntialiasing);
            this.Controls.Add(this.numericUpDownAmplitude);
            this.Controls.Add(this.numericUpDownStep);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.labelFPS);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelGdipDrawLines)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelGdipForDrawLine)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelGdi32)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAmplitude)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFPS;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox panelGdipDrawLines;
        private System.Windows.Forms.PictureBox panelGdipForDrawLine;
        private System.Windows.Forms.TabPage tabPage3;
        private D2DView panelD2DHW;
        private System.Windows.Forms.TabPage tabPage4;
        private D2DView panelD2DSW;
        private System.Windows.Forms.TabPage tabPage5;
        private GLWrapper.GLView panelGL;
        private System.Windows.Forms.NumericUpDown numericUpDownStep;
        private System.Windows.Forms.NumericUpDown numericUpDownAmplitude;
        private System.Windows.Forms.CheckBox checkBoxAntialiasing;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.PictureBox panelGdi32;
        private System.Windows.Forms.Button buttonRunAutomaticTest;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox checkBoxDownsample;
    }
}


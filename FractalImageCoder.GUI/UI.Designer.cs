namespace FractalImageCoder.GUI
{
    partial class UI
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
            this.originalImagePanel = new System.Windows.Forms.Panel();
            this.loadButton = new System.Windows.Forms.Button();
            this.rangeBlockPanel = new System.Windows.Forms.Panel();
            this.domainBlockPanel = new System.Windows.Forms.Panel();
            this.isometryLabel = new System.Windows.Forms.Label();
            this.offsetLabel = new System.Windows.Forms.Label();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.xDLabel = new System.Windows.Forms.Label();
            this.yDLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // originalImagePanel
            // 
            this.originalImagePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.originalImagePanel.Location = new System.Drawing.Point(12, 12);
            this.originalImagePanel.Name = "originalImagePanel";
            this.originalImagePanel.Size = new System.Drawing.Size(512, 512);
            this.originalImagePanel.TabIndex = 0;
            this.originalImagePanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.originalImagePanel_MouseClick);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(12, 559);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 1;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // rangeBlockPanel
            // 
            this.rangeBlockPanel.Location = new System.Drawing.Point(6, 35);
            this.rangeBlockPanel.Name = "rangeBlockPanel";
            this.rangeBlockPanel.Size = new System.Drawing.Size(80, 80);
            this.rangeBlockPanel.TabIndex = 2;
            // 
            // domainBlockPanel
            // 
            this.domainBlockPanel.Location = new System.Drawing.Point(92, 35);
            this.domainBlockPanel.Name = "domainBlockPanel";
            this.domainBlockPanel.Size = new System.Drawing.Size(160, 160);
            this.domainBlockPanel.TabIndex = 3;
            // 
            // isometryLabel
            // 
            this.isometryLabel.AutoSize = true;
            this.isometryLabel.Location = new System.Drawing.Point(3, 151);
            this.isometryLabel.Name = "isometryLabel";
            this.isometryLabel.Size = new System.Drawing.Size(58, 13);
            this.isometryLabel.TabIndex = 4;
            this.isometryLabel.Text = "Isometry = ";
            // 
            // offsetLabel
            // 
            this.offsetLabel.AutoSize = true;
            this.offsetLabel.Location = new System.Drawing.Point(3, 182);
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(47, 13);
            this.offsetLabel.TabIndex = 5;
            this.offsetLabel.Text = "Offset = ";
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Location = new System.Drawing.Point(3, 166);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(46, 13);
            this.scaleLabel.TabIndex = 6;
            this.scaleLabel.Text = "Scale = ";
            // 
            // xDLabel
            // 
            this.xDLabel.AutoSize = true;
            this.xDLabel.Location = new System.Drawing.Point(3, 118);
            this.xDLabel.Name = "xDLabel";
            this.xDLabel.Size = new System.Drawing.Size(32, 13);
            this.xDLabel.TabIndex = 7;
            this.xDLabel.Text = "Xd = ";
            // 
            // yDLabel
            // 
            this.yDLabel.AutoSize = true;
            this.yDLabel.Location = new System.Drawing.Point(3, 135);
            this.yDLabel.Name = "yDLabel";
            this.yDLabel.Size = new System.Drawing.Size(32, 13);
            this.yDLabel.TabIndex = 8;
            this.yDLabel.Text = "Yd = ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.rangeBlockPanel);
            this.groupBox1.Controls.Add(this.yDLabel);
            this.groupBox1.Controls.Add(this.domainBlockPanel);
            this.groupBox1.Controls.Add(this.xDLabel);
            this.groupBox1.Controls.Add(this.isometryLabel);
            this.groupBox1.Controls.Add(this.scaleLabel);
            this.groupBox1.Controls.Add(this.offsetLabel);
            this.groupBox1.Location = new System.Drawing.Point(260, 559);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 203);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Matching Blocks";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Range 80 x 80";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(159, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Domain 160 x 160";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 588);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 10;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 530);
            this.progressBar.Maximum = 4096;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(512, 23);
            this.progressBar.TabIndex = 11;
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 809);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.originalImagePanel);
            this.Name = "UI";
            this.Text = "Fractal Image Coder";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel originalImagePanel;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Panel rangeBlockPanel;
        private System.Windows.Forms.Panel domainBlockPanel;
        private System.Windows.Forms.Label isometryLabel;
        private System.Windows.Forms.Label offsetLabel;
        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.Label xDLabel;
        private System.Windows.Forms.Label yDLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}


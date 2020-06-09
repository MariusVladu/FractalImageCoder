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
            this.loadButton.Location = new System.Drawing.Point(12, 530);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 1;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 688);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.originalImagePanel);
            this.Name = "UI";
            this.Text = "Fractal Image Coder";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel originalImagePanel;
        private System.Windows.Forms.Button loadButton;
    }
}


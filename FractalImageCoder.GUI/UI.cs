using System;
using System.Drawing;
using System.Windows.Forms;

namespace FractalImageCoder.GUI
{
    public partial class UI : Form
    {
        private string originalImagePath;
        private Bitmap originalBitmap;

        private Graphics graphics;
        private Pen pen = new Pen(Color.White);

        private Coder coder = new Coder();

        public UI()
        {
            InitializeComponent();

            graphics = originalImagePanel.CreateGraphics();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Bitmap Image|*.bmp";
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            originalImagePath = fileDialog.FileName;
            originalBitmap = new Bitmap(originalImagePath);

            this.originalImagePanel.BackgroundImage = originalBitmap;
        }

        private void originalImagePanel_MouseClick(object sender, MouseEventArgs e)
        {
            var x1 = e.Location.X;
            var y1 = e.Location.Y;

            var matchingBlocks = coder.GetBestMatchingDomainBlock(x1, y1, originalBitmap);

            xDLabel.Text = "Xd = " + matchingBlocks.Domain.StartX;
            yDLabel.Text = "Yd = " + matchingBlocks.Domain.StartY;
            isometryLabel.Text = "Isometry = " + matchingBlocks.Isometry;
            scaleLabel.Text = "Scale = " + matchingBlocks.Scale;
            offsetLabel.Text = "Offset = " + matchingBlocks.Offset;

            var rangeImage = new Bitmap(80, 80);
            var domainImage = new Bitmap(160, 160);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var intensity = coder.imageMatrix[matchingBlocks.Range.StartX + i, matchingBlocks.Range.StartY + j];
                    for (int t = 0; t < 10; t++)
                    {
                        for (int v = 0; v < 10; v++)
                        {
                            rangeImage.SetPixel(i * 10 + t, j * 10 + v, Color.FromArgb(intensity, intensity, intensity));
                        }
                    }
                }
            }

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    var intensity = coder.imageMatrix[matchingBlocks.Domain.StartX + i, matchingBlocks.Domain.StartY + j];
                    for (int t = 0; t < 10; t++)
                    {
                        for (int v = 0; v < 10; v++)
                        {
                            domainImage.SetPixel(i * 10 + t, j * 10 + v, Color.FromArgb(intensity, intensity, intensity));
                        }
                    }
                }
            }

            rangeBlockPanel.BackgroundImage = rangeImage;
            domainBlockPanel.BackgroundImage = domainImage;

            originalImagePanel.Refresh();

            graphics.DrawRectangle(pen, matchingBlocks.Range.StartX, matchingBlocks.Range.StartY, 8, 8);
            graphics.DrawRectangle(pen, matchingBlocks.Domain.StartX, matchingBlocks.Domain.StartY, 16, 16);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            coder.CodeToFile(originalImagePath, $"{originalImagePath}.f", UpdateProgressBar);
        }

        private void UpdateProgressBar(int value)
        {
            progressBar.Value = value;
        }
    }
}

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractalImageCoder.GUI
{
    public partial class UI : Form
    {
        private string originalImagePath;
        private Bitmap originalBitmap;

        private string fractalEncodedImagePath;

        private Graphics graphics;
        private Pen pen = new Pen(Color.White);

        private Coder coder = new Coder();
        private Decoder decoder;

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

        private async void originalImagePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (originalBitmap == null) return;

            var x = e.Location.X;
            var y = e.Location.Y;

            var matchingBlocks = await Task.Run(() => coder.GetBestMatchingDomainBlock(x, y, originalBitmap));

            var dequantizedScale = RangeDomainRelation.DequantizeScale((int)matchingBlocks.Scale);
            var dequantizedOffset = RangeDomainRelation.DequantizeOffset((int)matchingBlocks.Offset, dequantizedScale);

            xDLabel.Text = "Xd = " + matchingBlocks.Domain.StartX;
            yDLabel.Text = "Yd = " + matchingBlocks.Domain.StartY;
            isometryLabel.Text = "Isometry = " + matchingBlocks.Isometry;
            scaleLabel.Text = "Scale = " + Math.Round(dequantizedScale, 2);
            offsetLabel.Text = "Offset = " + Math.Round(dequantizedOffset, 2);

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

        private async void saveButton_Click(object sender, EventArgs e)
        {
            await Task.Run(() => coder.CodeToFile(originalImagePath, $"{originalImagePath}.f", UpdateProgressBar));
        }

        private void UpdateProgressBar(int value)
        {
            progressBar.BeginInvoke((MethodInvoker)delegate ()
            {
                progressBar.Value = value;
            });
        }

        private void loadEncodedButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Fractal Encoded Image|*.f";
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            fractalEncodedImagePath = fileDialog.FileName;

            loadInitialButton.Enabled = true;
            decodeButton.Enabled = true;
            savedDecodedButton.Enabled = true;
        }

        private void loadInitialButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Bitmap Image|*.bmp";
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            originalImagePath = fileDialog.FileName;
            originalBitmap = new Bitmap(originalImagePath);

            this.originalImagePanel.BackgroundImage = originalBitmap;

            decoder = new Decoder(fractalEncodedImagePath, originalImagePath);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < steps.Value; i++)
                {
                    DecodeImageOneStep();
                }
            });
        }

        private void DecodeImageOneStep()
        {
            var decodedImageMatrix = decoder.Decode(UpdateProgressBar);
            var decodedImage = new Bitmap(decoder.width, decoder.height);

            for (int i = 0; i < decodedImage.Height; i++)
            {
                for (int j = 0; j < decodedImage.Width; j++)
                {
                    var intensity = decodedImageMatrix[i, j];

                    decodedImage.SetPixel(i, j, Color.FromArgb(intensity, intensity, intensity));
                }
            }

            decodedImagePanel.BackgroundImage = decodedImage;
        }
    }
}

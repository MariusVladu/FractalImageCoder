using FractalImageCoder.Entities;
using FractalImageCoder.Mappers;
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
        private string initialImageForDecodingPath;
        private Bitmap initialBitmapForDecoding;
        private Bitmap decodedBitmap;

        private Graphics originalPanelGraphics;
        private Graphics decodedPanelGraphics;
        private Pen pen = new Pen(Color.White);

        private Coder coder = new Coder();
        private Decoder decoder;

        private bool isDecoding = false;
        private bool isEncoding = false;

        public UI()
        {
            InitializeComponent();

            originalPanelGraphics = originalImagePanel.CreateGraphics();
            decodedPanelGraphics = decodedImagePanel.CreateGraphics();
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
            if (originalBitmap == null || isEncoding || isDecoding) return;
            isEncoding = true;

            var x = e.Location.X;
            var y = e.Location.Y;

            var matchingBlock = await Task.Run(() => coder.GetBestMatchingDomainBlock(x, y, originalBitmap));

            originalImagePanel.Refresh();
            ShowMatchingBlocks(matchingBlock, coder.imageMatrix, originalPanelGraphics);
            isEncoding = false;
        }

        private void ShowMatchingBlocks(MatchingBlocks matchingBlock, int[,] imageMatrix, Graphics panelGraphics)
        {
            var dequantizedScale = RangeDomainRelation.DequantizeScale((int)matchingBlock.Scale);
            var dequantizedOffset = RangeDomainRelation.DequantizeOffset((int)matchingBlock.Offset, dequantizedScale);

            xDLabel.Text = "Xd = " + matchingBlock.Domain.StartX;
            yDLabel.Text = "Yd = " + matchingBlock.Domain.StartY;
            isometryLabel.Text = "Isometry = " + matchingBlock.Isometry;
            scaleLabel.Text = "Scale = " + Math.Round(dequantizedScale, 2);
            offsetLabel.Text = "Offset = " + Math.Round(dequantizedOffset, 2);

            var rangeImage = new Bitmap(80, 80);
            var domainImage = new Bitmap(160, 160);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var intensity = imageMatrix[matchingBlock.Range.StartX + i, matchingBlock.Range.StartY + j];
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
                    var intensity = imageMatrix[matchingBlock.Domain.StartX + i, matchingBlock.Domain.StartY + j];
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

            panelGraphics.DrawRectangle(pen, matchingBlock.Range.StartX, matchingBlock.Range.StartY, 8, 8);
            panelGraphics.DrawRectangle(pen, matchingBlock.Domain.StartX, matchingBlock.Domain.StartY, 16, 16);
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (isEncoding || isDecoding) return;
            isEncoding = true;
            originalImagePanel.Enabled = false;

            await Task.Run(() => coder.CodeToFile(originalImagePath, $"{originalImagePath}.f", UpdateProgressBar));

            originalImagePanel.Enabled = true;
            isEncoding = false;
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
        }

        private void loadInitialButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Bitmap Image|*.bmp";
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            initialImageForDecodingPath = fileDialog.FileName;
            initialBitmapForDecoding = new Bitmap(initialImageForDecodingPath);

            this.decodedImagePanel.BackgroundImage = initialBitmapForDecoding;

            decoder = new Decoder(fractalEncodedImagePath, initialImageForDecodingPath);
        }

        private async void decodeButton_Click(object sender, EventArgs e)
        {
            if (isDecoding) return;
            isDecoding = true;

            await Task.Run(() =>
            {
                for (int i = 0; i < steps.Value; i++)
                {
                    DecodeImageOneStep();
                }
            });

            isDecoding = false;

            savedDecodedButton.Enabled = true;
        }

        private void DecodeImageOneStep()
        {
            var decodedImageMatrix = decoder.Decode(UpdateProgressBar);
            decodedBitmap = ImageMapper.GetImageFromPixelMatrix(decodedImageMatrix);

            decodedImagePanel.BackgroundImage = decodedBitmap;
            ComputePSNR();
        }

        private void ComputePSNR()
        {
            if (string.IsNullOrWhiteSpace(originalImagePath) || decodedBitmap == null)
                return;

            var originalMatrix = ImageMapper.GetPixelMatrixFromImage(originalBitmap);
            var decodedMatrix = ImageMapper.GetPixelMatrixFromImage(decodedBitmap);

            var maxOriginalValue = -1;
            long differencesSquared = 0;

            for (int i = 0; i < originalBitmap.Height; i++)
            {
                for (int j = 0; j < originalBitmap.Width; j++)
                {
                    var originalValue = originalMatrix[i, j];
                    var decodedValue = decodedMatrix[i, j];

                    if (originalValue > maxOriginalValue)
                        maxOriginalValue = originalValue;

                    differencesSquared += (originalValue - decodedValue) * (originalValue - decodedValue);
                }
            }

            var psnr = 10 * Math.Log10((255 * 255) / ((double)differencesSquared / (originalBitmap.Width * originalBitmap.Height)));

            UpdatePSNRLabel(psnr);
        }

        private void UpdatePSNRLabel(double psnr)
        {
            PSNRLabel.BeginInvoke((MethodInvoker)delegate ()
            {
                PSNRLabel.Text = "PSNR = " + string.Format("{0:0.0000}", psnr);
            });
        }

        private void savedDecodedButton_Click(object sender, EventArgs e)
        {
            if (isEncoding || isDecoding) return;
            ImageSaver.SaveBitmapToFile(fractalEncodedImagePath, decodedBitmap, $"{fractalEncodedImagePath}.bmp");
        }

        private void decodedImagePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (decodedBitmap == null || decoder == null || isDecoding) return;
            isDecoding = true;

            var x = e.Location.X;
            var y = e.Location.Y;

            var matchingBlock = decoder.GetMatchingBlock(x, y);

            decodedImagePanel.Refresh();
            ShowMatchingBlocks(matchingBlock, decoder.matrix, decodedPanelGraphics);
            isDecoding = false;
        }
    }
}

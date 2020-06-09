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
        private Pen pen = new Pen(Color.Black);

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

            var matchingDomain = coder.GetBestMatchingDomainBlock(x1, y1, originalBitmap);

            graphics.DrawRectangle(pen, x1 / 8 * 8, y1 / 8 * 8, 8, 8);
            graphics.DrawRectangle(pen, matchingDomain.StartX, matchingDomain.StartY, 16, 16);

            var imageMatrix = coder.imageMatrix;
            var image = new Bitmap(512, 512);

            for (int i = 0; i < 62; i++)
            {
                for (int j = 0; j < 62; j++)
                {
                    for (int t = 0; t < 8; t++)
                        for (int v = 0; v < 8; v++)
                        {
                            Isometries.ComputeBlockCoordinatesByIsometry(t, v, 7, out var x, out var y);

                            x = i * 8 + x * 2;
                            y = j * 8 + y * 2;

                            var downsampledValue = (imageMatrix[x, y] + imageMatrix[x, y + 1] + imageMatrix[x + 1, y] + imageMatrix[x + 1, y + 1]) / 4;
                            image.SetPixel(i * 8 + t * 2, j * 8 + v * 2, Color.FromArgb(downsampledValue, downsampledValue, downsampledValue));
                            image.SetPixel(i * 8 + t * 2, j * 8 + v * 2 + 1, Color.FromArgb(downsampledValue, downsampledValue, downsampledValue));
                            image.SetPixel(i * 8 + t * 2 + 1, j * 8 + v * 2, Color.FromArgb(downsampledValue, downsampledValue, downsampledValue));
                            image.SetPixel(i * 8 + t * 2 + 1, j * 8 + v * 2 + 1, Color.FromArgb(downsampledValue, downsampledValue, downsampledValue));
                        }
                }
            }

            //for (int i = 0; i < 63; i++)
            //{
            //    for (int j = 0; j < 63; j++)
            //    {
            //        for (int t = 0; t < 8; t++)
            //            for (int v = 0; v < 8; v++)
            //            {
            //                Isometries.ComputeBlockCoordinatesByIsometry(t, v, 2, out var x, out var y);

            //                x = i * 8 + x;
            //                y = j * 8 + y;

            //                var downsampledValue = imageMatrix[x, y];
            //                image.SetPixel(i * 8 + t, j * 8 + v, Color.FromArgb(downsampledValue, downsampledValue, downsampledValue));
            //            }
            //    }
            //}

            //originalImagePanel.BackgroundImage = image;

            //var imageMatrix = new int[8, 8];
            //for (int i = 0; i < 8; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        //for (int t = 0; t < 64; t++)
            //        //    for (int v = 0; v < 64; v++)
            //        //    {
            //        //        imageMatrix[i, j] += originalBitmap.GetPixel(i * 64 + t, j * 64 + v).R;
            //        //    }
            //        imageMatrix[i, j] += (originalBitmap.GetPixel(i, j).R + originalBitmap.GetPixel(i, j).G + originalBitmap.GetPixel(i, j).B)/3;
            //    }
            //}

            //var image = new Bitmap(512, 512);

            //for (int i = 0; i < 8; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        Isometries.ComputeBlockCoordinatesByIsometry(i, j, 7, out var x, out var y);

            //        for (int t = 0; t < 64; t++)
            //            for (int v = 0; v < 64; v++)
            //        {
            //            image.SetPixel(i*64+t, j*64+v, Color.FromArgb(imageMatrix[x, y], imageMatrix[x, y], imageMatrix[x, y]));

            //        }
            //    }
            //}

            originalImagePanel.BackgroundImage = image;
        }
    }
}

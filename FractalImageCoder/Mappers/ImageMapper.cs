using System.Drawing;

namespace FractalImageCoder.Mappers
{
    public static class ImageMapper
    {
        public static int[,] GetPixelMatrixFromImage(Bitmap image)
        {
            int[,] pixelMatrix = new int[image.Height, image.Width];

            for (int i = 0; i < image.Height; i++)
                for (int j = 0; j < image.Width; j++)
                {
                    Color color = image.GetPixel(i, j);
                    pixelMatrix[i, j] = (color.R + color.G + color.B) / 3;
                }

            return pixelMatrix;
        }

        public static Bitmap GetImageFromPixelMatrix(int[,] matrix)
        {
            var height = matrix.GetLength(0);
            var width = matrix.GetLength(1);

            var bitmap = new Bitmap(width, height);

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    bitmap.SetPixel(i, j, Color.FromArgb(matrix[i, j], matrix[i, j], matrix[i, j]));
                }

            return bitmap;
        }
    }
}

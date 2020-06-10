using BitReaderWriter;
using FractalImageCoder.Mappers;
using System.Drawing;
using System.IO;

namespace FractalImageCoder
{
    public static class ImageSaver
    {
        public static void SaveBitmapToFile(string headerFilePath, Bitmap bitmap, string outputFilePath)
        {
            var imageMatrix = ImageMapper.GetPixelMatrixFromImage(bitmap);

            using (var bitWriter = new BitWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.Write)))
            using (var bitReader = new BitReader(new FileStream(headerFilePath, FileMode.Open, FileAccess.Read)))
            {
                for (int i = 0; i < 1078; i++)
                {
                    bitWriter.WriteNBits(8, bitReader.ReadNBits(8));
                }

                var height = imageMatrix.GetLength(0);
                var width = imageMatrix.GetLength(1);

                for (int i = height - 1; i >= 0; i--)
                    for (int j = 0; j < width; j++)
                        bitWriter.WriteNBits(8, (uint)imageMatrix[j, i]);
            }
        }
    }
}

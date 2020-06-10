using BitReaderWriter;
using FractalImageCoder.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FractalImageCoder
{
    public class Decoder
    {
        public int height;
        public int width;
        public int[,] matrix;
        private List<MatchingBlocks> matchingBlocks;

        private int bitsForCoordinates;

        public Decoder(string encodedFilePath, string initialImagePath)
        {
            InitializeDecoder(encodedFilePath, initialImagePath);
        }

        private void InitializeDecoder(string encodedFilePath, string initialImagePath)
        {
            var initialImage = new Bitmap(initialImagePath);
            InitializeImageMatrix(initialImage);

            matchingBlocks = new List<MatchingBlocks>();

            using (var bitReader = new BitReader(new FileStream(encodedFilePath, FileMode.Open, FileAccess.Read)))
            {
                for (int i = 0; i < 1078; i++)
                    bitReader.ReadNBits(8);

                for (int i = 0; i < height / 8; i++)
                {
                    for (int j = 0; j < width / 8; j++)
                    {
                        var startX = (int)bitReader.ReadNBits(bitsForCoordinates) * 8;
                        var startY = (int)bitReader.ReadNBits(bitsForCoordinates) * 8;
                        var isometry = (int)bitReader.ReadNBits(3);
                        var dequantizedScale = RangeDomainRelation.DequantizeScale((int)bitReader.ReadNBits(5));
                        var dequantizedOffset = RangeDomainRelation.DequantizeOffset((int)bitReader.ReadNBits(7), dequantizedScale);

                        matchingBlocks.Add(new MatchingBlocks
                        {
                            Domain = new Block
                            {
                                StartX = startX,
                                StartY = startY
                            },
                            Range = new Block
                            {
                                StartX = i * 8,
                                StartY = j * 8
                            },
                            Isometry = isometry,
                            Scale = dequantizedScale,
                            Offset = dequantizedOffset
                        });
                    }
                }
            }
        }

        public int[,] Decode(Action<int> onProgress)
        {
            var processedBlocks = 0;
            var computedMatrix = new int[height, width];

            foreach (var matchingBlock in matchingBlocks)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        var domainValue = Isometries.GetDomainValue(i, j, matchingBlock.Isometry, matchingBlock.Domain, matrix);

                        computedMatrix[matchingBlock.Range.StartX + i, matchingBlock.Range.StartY + j] = NormalizeValue(matchingBlock.Scale * domainValue + matchingBlock.Offset);
                    }
                }

                onProgress(++processedBlocks);
            }

            matrix = computedMatrix;

            return computedMatrix;
        }

        private int NormalizeValue(double value)
        {
            if (value < 0)
                return 0;

            if (value > 255)
                return 255;

            return (int)value;
        }

        private void InitializeImageMatrix(Bitmap image)
        {
            height = image.Height;
            width = image.Width;
            bitsForCoordinates = (int)Math.Ceiling(Math.Log(Math.Max(width, height) / 8, 2));

            matrix = new int[height, width];

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    var pixel = image.GetPixel(i, j);
                    matrix[i, j] = (pixel.R + pixel.G + pixel.B) / 3;
                }
            }
        }
    }
}

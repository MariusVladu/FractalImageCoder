using BitReaderWriter;
using BitReaderWriter.Contracts;
using FractalImageCoder.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace FractalImageCoder
{
    public class Coder
    {
        private int height;
        private int width;
        public int[,] imageMatrix;

        private IBitWriter bitWriter;
        private int bitsForCoordinates;

        public void CodeToFile(string inputFilePath, string outputFilePath, Action<int> onProgress)
        {
            InitializeImageMatrix(new Bitmap(inputFilePath));
            InitializeOutputFile(inputFilePath, outputFilePath);

            var ranges = ComputeRanges();
            var domains = ComputeDomains();

            var processedRanges = 0;

            foreach (var range in ranges)
            {
                Block bestDomain = null;
                int bestIsometry = 0;
                int bestScale = 0;
                int bestOffset = 0;
                double minimumError = int.MaxValue;

                foreach (var domain in domains)
                {
                    for (int isometryIndex = 0; isometryIndex < 8; isometryIndex++)
                    {
                        var rdsum = Isometries.GetRDSumByIsometry(range, domain, isometryIndex, imageMatrix);

                        RangeDomainRelation.ComputeRangeDomainRelationParameters(range, domain, rdsum, out var scale, out var offset, out var error);

                        if (error < minimumError)
                        {
                            bestDomain = domain;
                            bestIsometry = isometryIndex;
                            bestScale = scale;
                            bestOffset = offset;
                            minimumError = error;
                        }
                    }
                }

                WriteBestDomainMatchParameters(bestDomain, bestIsometry, bestScale, bestOffset);

                onProgress(++processedRanges);
            }
        }

        public MatchingBlocks GetBestMatchingDomainBlock(int x, int y, Bitmap image)
        {
            InitializeImageMatrix(image);

            var range = ComputeBlock(x / 8 * 8, y / 8 * 8, 8);
            var domain = GetBestMatchingDomainBlockForRange(range, out var isometry, out var scale, out var offset);

            return new MatchingBlocks
            {
                Range = range,
                Domain = domain,
                Isometry = isometry,
                Scale = scale,
                Offset = offset
            };
        }

        public Block GetBestMatchingDomainBlockForRange(Block range, out int bestIsometry, out int bestScale, out int bestOffset)
        {
            Block bestDomain = null;
            bestIsometry = 0;
            bestScale = 0;
            bestOffset = 0;
            double minimumError = int.MaxValue;

            foreach (var domain in ComputeDomains())
            {
                for (int isometryIndex = 0; isometryIndex < 8; isometryIndex++)
                {
                    var rdsum = Isometries.GetRDSumByIsometry(range, domain, isometryIndex, imageMatrix);

                    RangeDomainRelation.ComputeRangeDomainRelationParameters(range, domain, rdsum, out var scale, out var offset, out var error);

                    if (error < minimumError)
                    {
                        bestDomain = domain;
                        bestIsometry = isometryIndex;
                        bestScale = scale;
                        bestOffset = offset;
                        minimumError = error;
                    }
                }
            }

            return bestDomain;
        }

        private void InitializeImageMatrix(Bitmap image)
        {
            height = image.Height;
            width = image.Width;
            bitsForCoordinates = (int)Math.Ceiling(Math.Log(Math.Max(width, height) / 8, 2));

            imageMatrix = new int[height, width];

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    var pixel = image.GetPixel(i, j);
                    imageMatrix[i, j] = (pixel.R + pixel.G + pixel.B) / 3;
                }
            }
        }

        private void InitializeOutputFile(string inputFilePath, string outputFilePath)
        {
            var outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);
            bitWriter = new BitWriter(outputFileStream);

            using (var bitReader = new BitReader(new FileStream(inputFilePath, FileMode.Open, FileAccess.Read)))
            {
                for (int i = 0; i < 1078; i++)
                {
                    bitWriter.WriteNBits(8, bitReader.ReadNBits(8));
                }
            }

            bitWriter.WriteNBits(32, (uint)bitsForCoordinates);
        }

        private void WriteBestDomainMatchParameters(Block bestDomain, int bestIsometry, int bestScale, int bestOffset)
        {
            bitWriter.WriteNBits(bitsForCoordinates, (uint)bestDomain.StartX);
            bitWriter.WriteNBits(bitsForCoordinates, (uint)bestDomain.StartY);
            bitWriter.WriteNBits(3, (uint)bestIsometry);
            bitWriter.WriteNBits(5, (uint)bestScale);
            bitWriter.WriteNBits(7, (uint)bestOffset);
        }

        private List<Block> ComputeRanges()
        {
            var ranges = new List<Block>();

            for (int i = 0; i < height / 8; i++)
                for (int j = 0; j < width / 8; j++)
                    ranges.Add(ComputeBlock(i * 8, j * 8, 8));

            return ranges;
        }

        private List<Block> ComputeDomains()
        {
            var domains = new List<Block>();

            for (int i = 0; i < height / 8 - 1; i++)
                for (int j = 0; j < height / 8 - 1; j++)
                {
                    var sum = 0;
                    var sumOfSquares = 0;

                    int x = i * 8;
                    int y = j * 8;

                    for (int t = 0; t < 16; t += 2)
                        for (int v = 0; v < 16; v += 2)
                        {
                            var downsampledValue = (imageMatrix[x + t, y + v] + imageMatrix[x + t, y + v + 1] + imageMatrix[x + t + 1, y + v] + imageMatrix[x + t + 1, y + v + 1]) / 4;
                            sum += downsampledValue;
                            sumOfSquares += downsampledValue * downsampledValue;
                        }

                    domains.Add(new Block
                    {
                        Size = 16,
                        StartX = x,
                        StartY = y,
                        Sum = sum,
                        SumOfSquares = sumOfSquares
                    });
                }

            return domains;
        }

        private Block ComputeBlock(int x, int y, int blockSize)
        {
            var sum = 0;
            var sumOfSquares = 0;

            for (int i = 0; i < blockSize; i++)
                for (int j = 0; j < blockSize; j++)
                {
                    var value = imageMatrix[x + i, y + j];
                    sum += value;
                    sumOfSquares += value * value;
                }

            return new Block
            {
                Size = blockSize,
                StartX = x,
                StartY = y,
                Sum = sum,
                SumOfSquares = sumOfSquares
            };
        }
    }
}

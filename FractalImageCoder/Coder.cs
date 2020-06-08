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
        private int[,] imageMatrix;

        private IBitWriter bitWriter;
        private int bitsForCoordinates;

        public void CodeToFile(string inputFilePath, string outputFilePath)
        {
            InitializeImageMatrix(inputFilePath);
            InitializeOutputFile(inputFilePath, outputFilePath);

            var ranges = ComputeRanges();
            var domains = ComputeDomains();

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

                        if(error < minimumError)
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
            }
        }

        private void InitializeImageMatrix(string inputFilePath)
        {
            var image = new Bitmap(inputFilePath);

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

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    ranges.Add(ComputeBlock(i, j, 8));

            return ranges;
        }

        private List<Block> ComputeDomains()
        {
            var domains = new List<Block>();

            for (int i = 0; i < height - 1; i++)
                for (int j = 0; j < width - 1; j++)
                    domains.Add(ComputeBlock(i, j, 16));

            return domains;
        }

        private Block ComputeBlock(int x, int y, int blockSize)
        {
            var sum = 0;
            var sumOfSquares = 0;

            for (int k = 0; k < blockSize; k++)
            {
                sum += imageMatrix[x, y];
                sumOfSquares += imageMatrix[x, y] * imageMatrix[x, y];
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

using FractalImageCoder.Entities;

namespace FractalImageCoder
{
    public static class Isometries
    {
        public static int BlockSize = 8;

        public static int GetRDSumByIsometry(Block range, Block domain, int isometryIndex, int[,] imageMatrix)
        {
            var rdSum = 0;

            for (int i = 0; i < range.Size; i++)
                for (int j = 0; j < range.Size; j++)
                {
                    ComputeBlockCoordinatesByIsometry(i, j, isometryIndex, out var blockComputedI, out var blockComputedJ);

                    rdSum += domain.BlockPixels[blockComputedI, blockComputedJ] * imageMatrix[range.StartX + i, range.StartY + j];
                }

            return rdSum;
        }

        public static int GetDomainValue(int i, int j, int isometryIndex, Block domain, int[,] imageMatrix)
        {
            ComputeBlockCoordinatesByIsometry(i, j, isometryIndex, out var blockComputedI, out var blockComputedJ);
            i = domain.StartX + blockComputedI * 2;
            j = domain.StartY + blockComputedJ * 2;

            return (imageMatrix[i, j] + imageMatrix[i, j + 1] + imageMatrix[i + 1, j] + imageMatrix[i + 1, j + 1]) / 4;
        }

        public static void ComputeBlockCoordinatesByIsometry(int i, int j, int isometryIndex, out int blockComputedI, out int blockComputedJ)
        {
            switch (isometryIndex)
            {
                case 1:
                    j = BlockSize - 1 - j;
                    break;
                case 2:
                    i = BlockSize - 1 - i;
                    break;
                case 3:
                    {
                        var temp = i;
                        i = j;
                        j = temp;
                        break;
                    }
                case 4:
                    {
                        var temp = i;
                        i = BlockSize - 1 - j;
                        j = BlockSize - 1 - temp;
                        break;
                    }
                case 5:
                    {
                        var temp = i;
                        i = BlockSize - 1 - j;
                        j = temp;
                        break;
                    }
                case 6:
                    i = BlockSize - 1 - i;
                    j = BlockSize - 1 - j;
                    break;
                case 7:
                    {
                        var temp = i;
                        i = j;
                        j = BlockSize - 1 - temp;
                        break;
                    }
            }

            blockComputedI = i;
            blockComputedJ = j;
        }
    }
}

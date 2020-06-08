using FractalImageCoder.Entities;

namespace FractalImageCoder
{
    public static class Isometries
    {
        public static int GetRDSumByIsometry(Block range, Block domain, int isometryIndex, int[,] imageMatrix)
        {
            var rdSum = 0;

            for (int i = 0; i < range.Size; i++)
                for (int j = 0; j < range.Size; j++)
                {
                    var x = domain.StartX * i;
                    var y = domain.StartY * j;

                    rdSum += GetDomainValue(x, y, isometryIndex, range.Size, imageMatrix) * imageMatrix[range.StartX * i, range.StartY * j];
                }

            return rdSum;
        }

        private static int GetDomainValue(int i, int j, int isometryIndex, int blockSize, int[,] imageMatrix)
        {
            switch (isometryIndex)
            {
                case 1: 
                    j = blockSize - 1 - j; 
                    break;
                case 2:
                    i = blockSize - 1 - i;
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
                        i = blockSize - 1 - j;
                        j = blockSize - 1 - temp;
                        break;
                    }
                case 5:
                    {
                        var temp = i;
                        i = blockSize - 1 - j;
                        j = temp;
                        break;
                    }
                case 6:
                    i = blockSize - 1 - i;
                    j = blockSize - 1 - j;
                    break;
                case 7:
                    {
                        var temp = i;
                        i = j;
                        j = blockSize - 1 - temp;
                        break;
                    }
            }

            return (imageMatrix[i, j] + imageMatrix[i, j + 1] + imageMatrix[i + 1, j] + imageMatrix[i + 1, j + 1]) / 4;
        }
    }
}

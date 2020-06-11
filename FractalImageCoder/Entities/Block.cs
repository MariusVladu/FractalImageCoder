namespace FractalImageCoder.Entities
{
    public class Block
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int Size { get; set; }

        public int Sum { get; set; }
        public int SumOfSquares { get; set; }
        public int[,] BlockPixels { get; set; }
    }
}

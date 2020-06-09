namespace FractalImageCoder.Entities
{
    public class MatchingBlocks
    {
        public Block Range { get; set; }
        public Block Domain { get; set; }
        public int Isometry { get; set; }
        public double Scale { get; set; }
        public double Offset { get; set; }
    }
}

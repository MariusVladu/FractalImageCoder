namespace FractalImageCoder.Entities
{
    public class MatchingBlocks
    {
        public Block Range { get; set; }
        public Block Domain { get; set; }
        public int Isometry { get; set; }
        public int Scale { get; set; }
        public int Offset { get; set; }
    }
}

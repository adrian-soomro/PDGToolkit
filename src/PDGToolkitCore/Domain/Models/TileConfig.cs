namespace PDGToolkitCore.Domain.Models
{
    public class TileConfig
    {
        public TileConfig(int size)
        {
            Size = size;
        }

        public int Size { get; }
    }
}
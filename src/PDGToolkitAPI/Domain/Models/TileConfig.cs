namespace PDGToolkitAPI.Domain.Models
{
    public class TileConfig
    {
        public int Height { get; }
        public int Width { get; }
        
        public TileConfig(int height, int width)
        {
            Height = height;
            Width = width;
        }
    }
}
using System.Collections.Generic;

namespace PDGToolkitCore.Domain.Models
{
    public class Grid
    {
        public int Height { get; }
        public int Width { get; }
        public TileConfig TileConfig { get; }
        public List<Tile> Tiles { get; }

        public Grid(int height, int width, TileConfig tileConfig, List<Tile> tiles)
        {
            Height = height;
            Width = width;
            TileConfig = tileConfig;
            Tiles = tiles;
        }
    }
}
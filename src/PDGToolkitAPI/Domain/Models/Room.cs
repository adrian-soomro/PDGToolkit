using System.Collections.Generic;

namespace PDGToolkitAPI.Domain.Models
{
    public class Room
    {
        public int Width { get; }
        public int Height { get; }
        public Position StartingPosition { get; }
        public List<Tile> Tiles { get; }


        public Room(int width, int height, Position startingPosition, List<Tile> tiles)
        {
            Width = width;
            Height = height;
            StartingPosition = startingPosition;
            Tiles = tiles;
        }
    }
}
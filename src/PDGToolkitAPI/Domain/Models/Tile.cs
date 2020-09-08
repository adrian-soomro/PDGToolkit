namespace PDGToolkitAPI.Domain.Models
{
    public class Tile
    {
        public TileType Type { get; }
        public Position Position { get; }

        public Tile(TileType type, Position position)
        {
            Type = type;
            Position = position;
        }
    }
}
namespace PDGToolkitCore.Domain.Models
{
    public struct Tile
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
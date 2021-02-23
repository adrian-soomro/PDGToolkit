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

        public bool IsEmpty()
        {
            return Type is null && Position.Equals(new Position());
        }

        public bool IsWalkable()
        {
            return Type.Equals(TileType.Floor) || Type.Equals(TileType.Door);
        }
    }
}
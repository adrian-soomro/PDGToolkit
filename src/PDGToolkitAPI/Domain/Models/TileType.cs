namespace PDGToolkitAPI.Domain.Models
{
    public class TileType
    {
        public static readonly TileType Floor = new TileType("Floor");
        public static readonly TileType Wall = new TileType("Wall");
        public static readonly TileType Obstacle = new TileType("Obstacle");
        public string Name { get; }
        
        private TileType(string name)
        {
            Name = name;
        }
    }
}
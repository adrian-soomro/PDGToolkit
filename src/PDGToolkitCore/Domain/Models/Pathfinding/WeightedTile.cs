using System;

namespace PDGToolkitCore.Domain.Models.Pathfinding
{
    public class WeightedTile
    {
        public Position Position { get; set; }
        public TileType Type { get; set; }
        public int CostFromStartToThis { get; set; }
        public int DistanceToTarget { get; private set; }

        public int CostDistance => CostFromStartToThis + DistanceToTarget;

        public WeightedTile Parent { get; set; }

        public WeightedTile() {}
        public WeightedTile(Tile tile)
        {
            Position = tile.Position;
            Type = tile.Type;
        }

        public void SetDistance(Position target)
        {
            DistanceToTarget = Math.Abs(target.X - Position.X) + Math.Abs(target.Y - Position.Y);
        }
    }
}
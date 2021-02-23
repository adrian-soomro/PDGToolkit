using System.Collections.Generic;
using System.Linq;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Application.Extensions
{
    public static class TileExtensions
    {
        /**
         * Given a list of tiles, and a replacement tile,
         * Finds all tiles that share position with the replacement, removes them and
         * Adds the replacement to the list of tiles.
         */
        public static void ReplaceTilesWithOtherTile(this List<Tile> tiles, Tile replacement)
        {
            var allTilesAtThisPosition = tiles.FindAll(t => t.Position.Equals(replacement.Position));

            foreach (var tile in allTilesAtThisPosition)
            {
                tiles.Remove(tile);
            }

            tiles.Add(replacement);
        }

        public static bool HasTwoAdjacentFloorTiles(this Tile tile, List<Tile> allTiles)
        {
            var up = new Position(tile.Position.X, tile.Position.Y + 1);
            var down = new Position(tile.Position.X, tile.Position.Y - 1);
            var left = new Position(tile.Position.X - 1, tile.Position.Y);
            var right = new Position(tile.Position.X + 1, tile.Position.Y);
            var adjacentFloorTiles = allTiles.Where(t => t.Type.Equals(TileType.Floor)).ToList().FindAll(t =>
                t.Position.Equals(up) || t.Position.Equals(down) || t.Position.Equals(left) ||
                t.Position.Equals(right));

            var adjacentFloorPositions = adjacentFloorTiles.Select(a => a.Position).ToList();
            return adjacentFloorPositions.Contains(up) && adjacentFloorPositions.Contains(down) ||
                   adjacentFloorPositions.Contains(right) && adjacentFloorPositions.Contains(left);
        }

        public static bool HasMaxTwoAdjacentWallTiles(this Tile tile, List<Tile> allTiles)
        {
            var up = new Position(tile.Position.X, tile.Position.Y + 1);
            var down = new Position(tile.Position.X, tile.Position.Y - 1);
            var left = new Position(tile.Position.X - 1, tile.Position.Y);
            var right = new Position(tile.Position.X + 1, tile.Position.Y);
            var adjacentWallTiles = allTiles.Where(t => t.Type.Equals(TileType.Wall)).ToList().FindAll(t =>
                t.Position.Equals(up) || t.Position.Equals(down) || t.Position.Equals(left) ||
                t.Position.Equals(right));

            var adjacentWallPositions = adjacentWallTiles.Select(a => a.Position).ToList();

            return adjacentWallPositions.Count == 2 &&
                   (adjacentWallPositions.Contains(up) && adjacentWallPositions.Contains(down) ||
                    adjacentWallPositions.Contains(left) && adjacentWallPositions.Contains(right));
        }

        public static bool IsOutsideBounds(this Tile tile, int minX, int maxX, int minY, int maxY)
        {
            return tile.Position.X <= minX || tile.Position.X >= maxX ||
                      tile.Position.Y <= minY || tile.Position.Y >= maxY;
        }
    }
}
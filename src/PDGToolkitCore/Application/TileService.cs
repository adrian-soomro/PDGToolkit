using System.Collections.Generic;
using System.Linq;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Application
{
    public class TileService : ITileService
    {
        public IEnumerable<Tile> FindTilesBasedOnPositionAndType(List<Tile> tiles, Position position, TileType type)
        {
            return tiles.FindAll(t => t.Position.Equals(position) && t.Type.Equals(type));  
        }
        
        public IEnumerable<Tile> FindTilesBasedOnPositionsAndType(List<Tile> tiles, List<Position> positions, TileType type)
        {
            return tiles.FindAll(t => positions.Contains(t.Position) && t.Type.Equals(type));  
        }
        
        public bool HasTwoAdjacentFloorTiles(List<Tile> allTiles, Position positionOfTileInQuestion)
        {
            var up = new Position(positionOfTileInQuestion.X, positionOfTileInQuestion.Y + 1);
            var down = new Position(positionOfTileInQuestion.X, positionOfTileInQuestion.Y - 1);
            var left = new Position(positionOfTileInQuestion.X - 1, positionOfTileInQuestion.Y);
            var right = new Position(positionOfTileInQuestion.X + 1, positionOfTileInQuestion.Y);
            var adjacentFloorTiles = allTiles.Where(t => t.Type.Equals(TileType.Floor)).ToList().FindAll(t =>
                t.Position.Equals(up) || t.Position.Equals(down) || t.Position.Equals(left) ||
                t.Position.Equals(right));

            var adjacentFloorPositions = adjacentFloorTiles.Select(a => a.Position); 
            return adjacentFloorPositions.Contains(up) && adjacentFloorPositions.Contains(down) || adjacentFloorPositions.Contains(right) && adjacentFloorPositions.Contains(left);
        }

        public Dictionary<Position, int> GetPositionsOfDuplicateTiles(List<Tile> tiles)
        {
            return tiles.GroupBy(t => t.Position)
                        .Where(g => g.Count() > 1)
                        .Select(y => new { Element = y.Key, Count = y.Count()})
                        .ToDictionary(x => x.Element, y => y.Count);
        }

        public List<Tile> RemoveOverlappingDuplicateWalls(List<Tile> allTiles)
        {
            var positionToNumberOfWalls = GetPositionsOfDuplicateTiles(allTiles);
            var duplicatePositions = positionToNumberOfWalls.Keys.ToList();

            var floorTilesWithDuplicateTilesInTheSamePosition = FindTilesBasedOnPositionsAndType(allTiles, duplicatePositions, TileType.Floor);

            var toBeRemoved = new List<Tile>();
            
            foreach (var tile in floorTilesWithDuplicateTilesInTheSamePosition)
            {
                var dupes =  FindTilesBasedOnPositionAndType(allTiles, tile.Position, TileType.Wall);

                toBeRemoved.AddRange(dupes);
            }

            foreach (var tile in toBeRemoved)
            {
                allTiles.Remove(tile);
            }
            return allTiles;
        }

        public List<Tile> RemoveSharedWalls(List<Tile> tiles)
        {
            var positionToNumberOfDuplicates = GetPositionsOfDuplicateTiles(tiles);
            var allDuplicatePositions = positionToNumberOfDuplicates.Keys.ToList();

            var duplicateWalls = FindTilesBasedOnPositionsAndType(tiles, allDuplicatePositions, TileType.Wall);
            var toBeRemoved = new List<Tile>();
            foreach (var tile in duplicateWalls)
            {
                positionToNumberOfDuplicates.TryGetValue(tile.Position, out var numberOfWallTilesInThisPosition);
                if (numberOfWallTilesInThisPosition >= 2 && HasTwoAdjacentFloorTiles(tiles, tile.Position))
                {
                    toBeRemoved.Add(tile);
                }
            }

            foreach (var tile in toBeRemoved)
            {
                tiles.Add(new Tile(TileType.Floor, tile.Position));
                tiles.Remove(tile);
            }

            return tiles;
        }
        
    }
}
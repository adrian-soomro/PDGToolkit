using System;
using System.Collections.Generic;
using System.Linq;
using PDGToolkitCore.Domain;
using PDGToolkitCore.Domain.Models;
using PDGToolkitCore.Domain.Models.Pathfinding;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCore.Application.PathFinding
{
    /**
     * Adaptation of https://dotnetcoretutorials.com/2020/07/25/a-search-pathfinding-algorithm-in-c/
     */
    public class AStarPathFindingService : IPathFindingService
    {
        private readonly Boundary endOfDungeonBoundary;

        public AStarPathFindingService(Settings settings)
        {
            var dungeonWidth = settings.GridSettings.Width / settings.TileSettings.Size;
            var dungeonHeight = settings.GridSettings.Height / settings.TileSettings.Size;
            const int wallThickness = 1;
            endOfDungeonBoundary = new Boundary(0, dungeonWidth - wallThickness, 0, dungeonHeight - wallThickness);
        }

        public IEnumerable<Tile> FindPath(Tile startTile, Tile finishTile, IEnumerable<Tile> tiles)
        {
            var start = new WeightedTile(startTile);
            var finish = new WeightedTile(finishTile);
            var allTiles = tiles.ToList();
            
            start.SetDistance(finish.Position);

            var activeWeightedTiles = new List<WeightedTile>();
            activeWeightedTiles.Add(start);
            var visitedWeightedTiles = new List<WeightedTile>();

            while (activeWeightedTiles.Any())
            {
                var checkTile = activeWeightedTiles.OrderBy(t => t.CostDistance).First();

                if (checkTile.Position.Equals(finish.Position))
                {
                    var lastTile = checkTile;
                    var path = new List<WeightedTile>();
                    while (lastTile != null)
                    {
                        path.Add(lastTile);
                        lastTile = lastTile.Parent;
                    }
                    return path.Select(weightedTile => new Tile(TileType.Floor, weightedTile.Position)).ToList();
                }
                visitedWeightedTiles.Add(checkTile);
                activeWeightedTiles.Remove(checkTile);

                var walkableTiles = GetWalkableTiles(checkTile, finish, allTiles);
                
                foreach (var walkableTile in walkableTiles)
                {
                    if (visitedWeightedTiles.Any(t => t.Position.Equals(walkableTile.Position)))
                        continue;

                    if (activeWeightedTiles.Any(t => t.Position.Equals(walkableTile.Position)))
                    {
                        var existingTile = activeWeightedTiles.First(t => t.Position.Equals(walkableTile.Position));
                        if (existingTile.CostDistance > checkTile.CostDistance)
                        {
                            activeWeightedTiles.Remove(existingTile);
                            activeWeightedTiles.Add(walkableTile);
                        }
                    } else
                    {
                        activeWeightedTiles.Add(walkableTile);
                    }
                }
            }

            Console.Out.WriteLine($"[!] Couldn't find a path!");
            return new List<Tile>();
        }
        
        private List<WeightedTile> GetWalkableTiles(WeightedTile currentTile, WeightedTile targetTile, List<Tile> allTiles)
        {
            var possibleTiles = new List<WeightedTile>
            {
                new WeightedTile { Position = new Position(currentTile.Position.X, currentTile.Position.Y - 1), Parent = currentTile, CostFromStartToThis = currentTile.CostFromStartToThis + 1 },
                new WeightedTile { Position = new Position(currentTile.Position.X, currentTile.Position.Y + 1), Parent = currentTile, CostFromStartToThis = currentTile.CostFromStartToThis + 1 },
                new WeightedTile { Position = new Position(currentTile.Position.X - 1, currentTile.Position.Y), Parent = currentTile, CostFromStartToThis = currentTile.CostFromStartToThis + 1 },
                new WeightedTile { Position = new Position(currentTile.Position.X + 1, currentTile.Position.Y), Parent = currentTile, CostFromStartToThis = currentTile.CostFromStartToThis + 1 },
            };

            possibleTiles.ForEach(tile => tile.SetDistance(targetTile.Position));

            // keep only empty tiles (tile with such position is not in allTiles) or walkableTiles
            return possibleTiles.Where(weightedTile =>
            {
                var tileAtPos = allTiles.FirstOrDefault(tile => tile.Position.Equals(weightedTile.Position));
                return  weightedTile.Position.X < endOfDungeonBoundary.MaxX && 
                        weightedTile.Position.Y < endOfDungeonBoundary.MaxY &&
                        weightedTile.Position.X > endOfDungeonBoundary.MinX &&
                        weightedTile.Position.Y > endOfDungeonBoundary.MinY &&
                        (tileAtPos.IsEmpty() || tileAtPos.IsWalkable());
            }).ToList();
        }
    }
}
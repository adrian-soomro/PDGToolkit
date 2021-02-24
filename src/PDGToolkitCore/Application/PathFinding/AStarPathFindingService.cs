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
        private readonly Random random;
        private Boundary endOfDungeonBoundary;

        public AStarPathFindingService(Random random, Settings settings)
        {
            this.random = random;

            var dungeonWidth = settings.GridSettings.Width / settings.TileSettings.Size;
            var dungeonHeight = settings.GridSettings.Height / settings.TileSettings.Size;
            const int wallThickness = 1;
            endOfDungeonBoundary = new Boundary(0, dungeonWidth - wallThickness, 0, dungeonHeight - wallThickness);
            
        }

        public IEnumerable<Tile> ConstructAllPaths(IEnumerable<Room> rooms)
        {
            var allRooms = rooms.ToList();
            var allTiles = allRooms.SelectMany(r => r.Tiles).ToList();
            var result = new List<Tile>();
           
            var depletedRooms = new List<Room>();

            foreach (var room in allRooms)
            {
                depletedRooms.Add(room);
                var availableRooms = allRooms.FindAll(r => !depletedRooms.Contains(r));
                if (availableRooms.Any())
                {
                    var index = random.Next(availableRooms.Count);
                    var roomToConnectTo = availableRooms.ElementAt(index);

                    var start = GetDoor(room);
                    var finish = GetDoor(roomToConnectTo);
                    result.AddRange(FindPath(start, finish, allTiles));
                }
            }
            
            allTiles.AddRange(result);
            var walls = new List<Tile>();
            
            foreach (var tile in result)
            {
                walls.AddRange(TrySurroundTileWithWallTiles(tile, allTiles));
            }

            walls = walls.Distinct().ToList();
            
            return result.Concat(walls);
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

        private IEnumerable<Tile> TrySurroundTileWithWallTiles(Tile tile, IEnumerable<Tile> tiles)
        {
            var potentialSurroundingTiles = new List<Tile>()
            {
                new Tile(TileType.Wall, new Position(tile.Position.X, tile.Position.Y - 1)),
                new Tile(TileType.Wall, new Position(tile.Position.X, tile.Position.Y + 1)),
                new Tile(TileType.Wall, new Position(tile.Position.X - 1, tile.Position.Y)),
                new Tile(TileType.Wall, new Position(tile.Position.X + 1, tile.Position.Y)),
                
                new Tile(TileType.Wall, new Position(tile.Position.X - 1, tile.Position.Y - 1)),
                new Tile(TileType.Wall, new Position(tile.Position.X - 1, tile.Position.Y + 1)),
                new Tile(TileType.Wall, new Position(tile.Position.X + 1, tile.Position.Y - 1)),
                new Tile(TileType.Wall, new Position(tile.Position.X + 1, tile.Position.Y + 1)),
            };

            var allPositions = tiles.Select(t => t.Position);

            return potentialSurroundingTiles.Where(t => !allPositions.Contains(t.Position));
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
        
        private Tile GetDoor(Room room)
        {
            return room.Tiles.First(t => t.Type.Equals(TileType.Door));
        }
    }
}
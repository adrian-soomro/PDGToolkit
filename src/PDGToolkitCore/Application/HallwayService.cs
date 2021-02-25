using System;
using System.Collections.Generic;
using System.Linq;
using PDGToolkitCore.Application.Extensions;
using PDGToolkitCore.Application.PathFinding;
using PDGToolkitCore.Domain;
using PDGToolkitCore.Domain.Models;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCore.Application
{
    class HallwayService : IHallwayService
    {
        private readonly Random random;
        private readonly Boundary doorPositionBoundary;
        private readonly IPathFindingService pathFindingService;

        public HallwayService(Random random, Settings settings, IPathFindingService pathFindingService)
        {
            this.random = random;
            this.pathFindingService = pathFindingService;

            var dungeonWidth = settings.GridSettings.Width / settings.TileSettings.Size;
            var dungeonHeight = settings.GridSettings.Height / settings.TileSettings.Size;
            const int wallThickness = 1;
            doorPositionBoundary = new Boundary(wallThickness, dungeonWidth - 2 * wallThickness, wallThickness , dungeonHeight - 2 * wallThickness);
        }

        public IEnumerable<Room> CreateDoors(IEnumerable<Room> rooms)
        {
            var allRooms = rooms.ToList();
            
            foreach (var room in allRooms)
            {
                var allTiles = allRooms.SelectMany(r => r.Tiles).ToList();
                AllocateDoors(room, allTiles);
            }

            return allRooms;
        }
        
        public IEnumerable<Tile> CreateHallways(IEnumerable<Room> rooms)
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
                    result.AddRange(pathFindingService.FindPath(start, finish, allTiles));
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
        
        public IEnumerable<Tile> HandleDoorTiles(IEnumerable<Tile> tiles)
        {
            var allTiles = tiles.ToList();
            var allDoorTiles = allTiles.FindAll(t => t.Type.Equals(TileType.Door));
            foreach (var doorTile in allDoorTiles)
            {
                if (doorTile.HasTwoAdjacentFloorTiles(allTiles))
                {
                    allTiles.ReplaceTilesWithOtherTile(doorTile);
                    continue;
                } 
                allTiles.ReplaceTilesWithOtherTile(new Tile(TileType.Wall, doorTile.Position));
            }

            return allTiles;
        }
        
        private Room AllocateDoors(Room room, List<Tile> allTiles)
        {
            var numberOfDoorsForThisRoom = room.NumContainedRooms / 2 + 4;
            
            var allWallTiles  = room.Tiles.FindAll(t => t.Type.Equals(TileType.Wall)).ToList();
            var allDuplicateWallTiles = room.Tiles.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();
            var uniqueWallTiles = allWallTiles.Except(allDuplicateWallTiles).ToList();

            var allRoomTilesWithoutUniqueWalls = room.Tiles.Except(uniqueWallTiles).ToList();

            var allTilesWithoutDupes = allTiles.Distinct().ToList();
            var wallTilesWithDoors = ReplaceWallsWithDoorsWherePossible(numberOfDoorsForThisRoom, uniqueWallTiles, allTilesWithoutDupes);
            
            var allRoomTiles = allRoomTilesWithoutUniqueWalls.Concat(wallTilesWithDoors);
            
            room.Tiles.Clear();
            room.Tiles.AddRange(allRoomTiles);
            
            return room;
        }
        
        private List<Tile> ReplaceWallsWithDoorsWherePossible(int numberOfDoorsToPlace, List<Tile> uniqueWallTiles, List<Tile> allTiles)
        {
            while (numberOfDoorsToPlace != 0)
            {
                var index = random.Next(uniqueWallTiles.Count);
                var randomlyChosenWallTile = uniqueWallTiles.ElementAt(index);

                if (IsTileAValidDoorLocation(randomlyChosenWallTile, allTiles))
                {
                    uniqueWallTiles.Add(new Tile(TileType.Door, new Position(uniqueWallTiles.ElementAt(index).Position.X, uniqueWallTiles.ElementAt(index).Position.Y)));
                    uniqueWallTiles.RemoveAt(index);
                    numberOfDoorsToPlace--;
                }
            }

            return uniqueWallTiles;
        }
        
        private bool IsTileAValidDoorLocation(Tile tile, IEnumerable<Tile> tiles)
        {
            var allTiles = tiles.ToList();
            return tile.HasMaxTwoAdjacentWallTiles(allTiles) && !tile.IsOutsideBounds(doorPositionBoundary.MinX, doorPositionBoundary.MaxX,
                doorPositionBoundary.MinY, doorPositionBoundary.MaxY);;
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
        
        private Tile GetDoor(Room room)
        {
            return room.Tiles.First(t => t.Type.Equals(TileType.Door));
        }
        
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using PDGToolkitCore.Application.Extensions;
using PDGToolkitCore.Domain;
using PDGToolkitCore.Domain.Models;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCore.Application
{
    internal class RoomService : IRoomService
    {
        private readonly Random random;
        private readonly Boundary outsideDungeonBoundary;
        private readonly Boundary doorPositionBoundary;
        
        public RoomService(Random random, Settings settings)
        {
            this.random = random;
            var dungeonWidth = settings.GridSettings.Width / settings.TileSettings.Size;
            var dungeonHeight = settings.GridSettings.Height / settings.TileSettings.Size;
            const int wallThickness = 1;
            outsideDungeonBoundary = new Boundary(0, dungeonWidth - wallThickness, 0, dungeonHeight - wallThickness);
            doorPositionBoundary = new Boundary(1, dungeonWidth - 2 * wallThickness, 1 , dungeonHeight - 2 * wallThickness);
        }

        public bool AreRoomsOverlapping(Room firstRoom, Room secondRoom)
        {
            var sharedTiles = GetOverlappingTiles(firstRoom, secondRoom);
            var allTiles = firstRoom.Tiles.Concat(secondRoom.Tiles).ToList();
            return sharedTiles.Count > 1 &&
                   sharedTiles.Any(sharedTile => sharedTile.HasTwoAdjacentFloorTiles(allTiles));
        }

        public Room MergeRooms(Room r1, Room r2)
        {
            if (r1.Equals(r2)) 
                throw new ArgumentException("Can't merge a room with itself!");
            
            var allTiles = r1.Tiles.Concat(r2.Tiles).ToList();
            allTiles = UncoverFloorTiles(allTiles);
           
            var overlappingWalls = r2.Tiles.FindAll(t =>
            {
                var roomsWithSharedPosition = GetRoomsByPosition(new List<Room> {r1, r2}, t.Position);
                
                return roomsWithSharedPosition.Contains(r1) && t.Type.Equals(TileType.Wall) && t.HasTwoAdjacentFloorTiles(allTiles);
            });

            foreach (var tile in overlappingWalls)
            {
                allTiles.ReplaceTilesWithOtherTile(new Tile(TileType.Floor, tile.Position));
            }
            
            r1.Tiles.Clear();
            r1.Tiles.AddRange(allTiles);
            
            return r1;
        }

        public IEnumerable<Room> GetRoomsByPosition(IEnumerable<Room> rooms, Position position)
        { 
            var foundRooms = new List<Room>(); 
            foreach (var room in rooms.ToList()) 
            {
               var roomTiles = room.Tiles;
               var foundTiles = roomTiles.FindAll(t => t.Position.Equals(position));
               if (foundTiles.Any())
                   foundRooms.Add(room);
            } 
            return foundRooms;
        }

        /**
         * Recursively keep trying to merge rooms given in <param name="rooms">rooms</param>,
         * until no more merging can occur.
         */
        public IEnumerable<Room> MergeAllRooms(IEnumerable<Room> rooms)
        {
            var allRooms = rooms.ToList();
            var result = new HashSet<Room>();
            var depleted = new List<Room>();
            
            // source of inspiration: https://stackoverflow.com/questions/12666300/create-pairs-from-list-values
            var uniqueRoomPairs = allRooms.SelectMany(
                (first, i) => allRooms.Skip(i + 1).Select(second => (first, second))).ToList();
            
            foreach (var (first, second) in uniqueRoomPairs)
            {
                if (depleted.Contains(first) || depleted.Contains(second))
                    continue;
            
                if (AreRoomsOverlapping(first, second))
                {
                    result.Add(MergeRooms(first, second));
                    first.IncrementMergedRoomsCounter();
                    depleted.Add(first);
                    depleted.Add(second);
                    continue;
                }
                result.Add(first);
            }

            if (!depleted.Contains(allRooms.Last()))
                result.Add(allRooms.Last());
            
            if (result.Count != allRooms.Count)
                return MergeAllRooms(result);
            
            return result;
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
        
        /**
         * Trims tiles that are spilling out of bounds <see cref="outsideDungeonBoundary"/>
         *  in all <param name="rooms">rooms</param>.
         */
        public IEnumerable<Room> TrimSpilledRooms(IEnumerable<Room> rooms)
        {
            var allRooms = rooms.ToList();
            foreach (var room in allRooms)
            {
                var allTilesInThisRoom = room.Tiles.ToList();
                var spillingTilePositions =
                    allTilesInThisRoom.FindAll(t => t.Position.X >= outsideDungeonBoundary.MaxX || t.Position.Y >= outsideDungeonBoundary.MaxY)
                        .Select(t => t.Position).ToList();
                var cleanTiles = allTilesInThisRoom.FindAll(t => !spillingTilePositions.Contains(t.Position));
                
                room.Tiles.Clear();
                room.Tiles.AddRange(cleanTiles);
            }

            return allRooms;
        }

        private Room AllocateDoors(Room room, List<Tile> allTiles)
        {
            var numberOfDoorsForThisRoom = room.NumContainedRooms / 2 + 1;
            
            var allWallTiles  = room.Tiles.FindAll(t => t.Type.Equals(TileType.Wall)).ToList();
            var allDuplicateWallTiles = room.Tiles.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();
            var uniqueWallTiles = allWallTiles.Except(allDuplicateWallTiles).ToList();

            var allRoomTilesWithoutUniqueWalls = room.Tiles.Except(uniqueWallTiles).ToList();

            var wallTilesWithDoors = ReplaceWallsWithDoorsWherePossible(numberOfDoorsForThisRoom, uniqueWallTiles, allTiles);
            
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

        /**
         * Removes any tiles that are in the same position as floor tiles.
         */
        private List<Tile> UncoverFloorTiles(List<Tile> tiles)
        {
            var dupePositions = GetPositionsOfDuplicateTiles(tiles).Keys.ToList();
            var tilesWithDupesInTHeirPositions = tiles.FindAll(t => dupePositions.Contains(t.Position)).Where(t => t.Type.Equals(TileType.Floor)).ToList();
            var floorTilesWithDupesInTheirPositions = tilesWithDupesInTHeirPositions.Where(t => t.Type.Equals(TileType.Floor)).ToList();
            foreach (var tile in floorTilesWithDupesInTheirPositions)
            {
                tiles.ReplaceTilesWithOtherTile(new Tile(TileType.Floor, tile.Position));
            }
            return tiles;
        }
        
        public IEnumerable<Tile> UncoverDoorTiles(IEnumerable<Tile> tiles)
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
        
        private Dictionary<Position, int> GetPositionsOfDuplicateTiles(List<Tile> tiles)
        {
            return tiles.GroupBy(t => t.Position)
                .Where(g => g.Count() > 1)
                .Select(y => new { Element = y.Key, Count = y.Count()})
                .ToDictionary(x => x.Element, y => y.Count);
        }
        
        private List<Tile> GetOverlappingTiles(Room r1, Room r2)
        {
            var allTiles = r1.Tiles.Concat(r2.Tiles).ToList();
            
            var positionsOfTilesFromR1 = r1.Tiles.Select(t => t.Position);
            var positionsOfTilesFromR2 = r2.Tiles.Select(t => t.Position);
            
            var overlappingTilePositions = positionsOfTilesFromR1.Intersect(positionsOfTilesFromR2).ToList();
            return allTiles.FindAll(t => overlappingTilePositions.Contains(t.Position)).Distinct().ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using PDGToolkitCore.Application.Extensions;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Application
{
    internal class RoomService : IRoomService
    {
        private Random random;

        public RoomService(Random random)
        {
            this.random = random;
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
                AllocateDoors(room);
            }

            return allRooms;
        }
        
        /**
         * Trims tiles that are spilling out of bounds (their X position is >= <param name="xThreshold">X threshold</param>
         *  or Y position is >= <param name="yThreshold">Y threshold</param>) in all <param name="rooms">rooms</param>.
         */
        public IEnumerable<Room> TrimSpilledRooms(IEnumerable<Room> rooms, int xThreshold, int yThreshold)
        {
            var allRooms = rooms.ToList();
            foreach (var room in allRooms)
            {
                var allTilesInThisRoom = room.Tiles.ToList();
                var spillingTilePositions =
                    allTilesInThisRoom.FindAll(t => t.Position.X >= xThreshold || t.Position.Y >= yThreshold)
                        .Select(t => t.Position).ToList();
                var cleanTiles = allTilesInThisRoom.FindAll(t => !spillingTilePositions.Contains(t.Position));
                
                room.Tiles.Clear();
                room.Tiles.AddRange(cleanTiles);
            }

            return allRooms;
        }

        private Room AllocateDoors(Room room)
        {
            var doorPermits = room.NumContainedRooms / 2 + 1;
            
            var allWallTiles  = room.Tiles.FindAll(t => t.Type.Equals(TileType.Wall)).ToList();
            var allDuplicateWallTiles = room.Tiles.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();
            var uniqueWallTiles = allWallTiles.Except(allDuplicateWallTiles).ToList();

            var allRoomTilesWithoutUniqueWalls = room.Tiles.Except(uniqueWallTiles).ToList();
            
            for (var i = 0; i < doorPermits; i++)
            {
                var index = random.Next(uniqueWallTiles.Count);
                uniqueWallTiles.Add(new Tile(TileType.Door, new Position(uniqueWallTiles.ElementAt(index).Position.X, uniqueWallTiles.ElementAt(index).Position.Y)));
                uniqueWallTiles.RemoveAt(index);
            }
            
            var allRoomTiles = allRoomTilesWithoutUniqueWalls.Concat(uniqueWallTiles);
            
            room.Tiles.Clear();
            room.Tiles.AddRange(allRoomTiles);
            
            return room;
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
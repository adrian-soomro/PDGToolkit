using System;
using System.Collections.Generic;
using System.Linq;
using PDGToolkitCore.Application.Extensions;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Application
{
    internal class RoomService : IRoomService
    {
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

        public IEnumerable<Room> MergeAllRooms(IEnumerable<Room> rooms)
        {
            var result = new HashSet<Room>();
            var allRooms = rooms.ToList();
            var depleted = new List<Room>();
            var uniqueRoomPairs = allRooms.SelectMany((first, i) => allRooms.Skip(i + 1).Select(second => (first, second))).ToList();
            foreach (var (first, second) in uniqueRoomPairs)
            {
                if (depleted.Contains(first) || depleted.Contains(second))
                    continue;
            
                Console.Out.WriteLine($"Comparing room {allRooms.IndexOf(first) +1 }, and {allRooms.IndexOf(second) +1}");
                if (AreRoomsOverlapping(first, second))
                {
                    Console.Out.WriteLine($"Merging room {allRooms.IndexOf(first) +1 }, and {allRooms.IndexOf(second) +1}");
                    result.Add(MergeRooms(first, second));
                    depleted.Add(first);
                    depleted.Add(second);
                    continue;
                }
                result.Add(first);
                Console.Out.WriteLine($"Tried to add room #{allRooms.IndexOf(first) +1} to results, there are {result.Count()} elements in results atm");
                foreach (var room in result)
                {
                    Console.Out.Write($"{room.Id}, ");
                }
                Console.Out.WriteLine("");
            }

            if (!depleted.Contains(allRooms.Last()))
            {
                result.Add(allRooms.Last());
            }

            Console.Out.WriteLine($"End of MergeAllRooms(), started with {rooms.Count()}, ended with {result.Count()}");

            if (result.Count() != rooms.Count())
            {
                return MergeAllRooms(result);
            }

            return result;
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
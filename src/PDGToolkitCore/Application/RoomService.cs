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
            var allRooms = new List<Room>{firstRoom, secondRoom};
            var allTiles = allRooms.SelectMany(r => r.Tiles).ToList();
                
            var allDupePositions = GetPositionsOfDuplicateTiles(allTiles).Keys.ToList();
            foreach (var dupePosition in allDupePositions)
            {
                var numRoomSharingDupePosition = GetRoomsByPosition(allRooms, dupePosition).ToList();
                if (numRoomSharingDupePosition.Count() > 1)
                    return true;
            }

            return false;
        }
        
        public Room MergeRooms(Room r1, Room r2)
        {
            var allTiles = r1.Tiles.Concat(r2.Tiles).ToList();
            allTiles = UncoverFloorTiles(allTiles);

            if (r1.Equals(r2)) 
                throw new ArgumentException("Can't merge a room with itself!");
            
            var overlappingWalls = r2.Tiles.FindAll(t =>
            {
                var sharedPositionsBetweenRooms = GetRoomsByPosition(new List<Room> {r1, r2}, t.Position);
                
                return sharedPositionsBetweenRooms.Contains(r1) && t.Type.Equals(TileType.Wall) && t.HasTwoAdjacentFloorTiles(allTiles);
            });

            foreach (var tile in overlappingWalls)
            {
                allTiles.ReplaceTilesWithOtherTile(new Tile(TileType.Floor, tile.Position));
            }
            
            return new Room(0, 0, new Position(0, 0), allTiles, r1.Id);
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

    }
}
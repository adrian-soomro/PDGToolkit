using System;
using System.Collections.Generic;
using System.Linq;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Application
{
    internal class RoomService : IRoomService
    {
        public IEnumerable<Room> GetAllOverlappingRooms(IEnumerable<Room> rooms)
        {
            var allRooms = rooms.ToList();
            var allTiles = allRooms.SelectMany(r => r.Tiles).ToList();

            var allDupePositions = GetPositionsOfDuplicateTiles(allTiles).Keys.ToList();

            var overlappingRooms = new HashSet<Room>();
            foreach (var dupePosition in allDupePositions)
            {
                var roomsSharingDupePosition = GetRoomsByPosition(allRooms, dupePosition).ToList();
                if (roomsSharingDupePosition.Count() > 1)
                    overlappingRooms.UnionWith(roomsSharingDupePosition);
            }

            return overlappingRooms;
        }

        public bool AreRoomsOverlapping(Room firstRoom, Room secondRoom)
        {
            var allRooms = new List<Room>{firstRoom, secondRoom};
            var allTiles = allRooms.SelectMany(r => r.Tiles).ToList();
                
            var allDupePositions = GetPositionsOfDuplicateTiles(allTiles).Keys.ToList();

            foreach (var dupePosition in allDupePositions)
            {
                var roomsSharingDupePosition = GetRoomsByPosition(allRooms, dupePosition).ToList();
                if (roomsSharingDupePosition.Count() > 1)
                    return true;
            }

            return false;
        }
        
        public Room MergeRooms(IEnumerable<Room> rooms)
        {
            var allRooms = rooms.ToHashSet();
            var id = allRooms.First().Id;
            
            var tiles = allRooms.ToList().SelectMany(r => r.Tiles).ToList();
            return new Room(0,0, new Position(0,0), tiles, id);
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
        
        private Dictionary<Position, int> GetPositionsOfDuplicateTiles(List<Tile> tiles)
        {
            return tiles.GroupBy(t => t.Position)
                .Where(g => g.Count() > 1)
                .Select(y => new { Element = y.Key, Count = y.Count()})
                .ToDictionary(x => x.Element, y => y.Count);
        }
    }
}
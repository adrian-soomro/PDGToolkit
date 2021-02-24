using System.Collections.Generic;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Application
{
    internal interface IRoomService
    {
        public Room MergeRooms(Room r1, Room r2);

        IEnumerable<Room> GetRoomsByPosition(IEnumerable<Room> rooms, Position position);

        bool AreRoomsOverlapping(Room firstRoom, Room secondRoom);

        IEnumerable<Room> MergeAllRooms(IEnumerable<Room> rooms);
        
        IEnumerable<Room> CreateDoors(IEnumerable<Room> rooms);
        
        IEnumerable<Room> TrimSpilledRooms(IEnumerable<Room> rooms);

        public IEnumerable<Tile> UncoverDoorTiles(IEnumerable<Tile> tiles);
    }
}
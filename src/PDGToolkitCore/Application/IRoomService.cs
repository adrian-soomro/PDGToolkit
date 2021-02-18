using System.Collections.Generic;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Application
{
    internal interface IRoomService
    {
        public IEnumerable<Room> GetAllOverlappingRooms(IEnumerable<Room> rooms);
        public Room MergeRooms(IEnumerable<Room> rooms);

        IEnumerable<Room> GetRoomsByPosition(IEnumerable<Room> rooms, Position position);

        bool AreRoomsOverlapping(Room firstRoom, Room secondRoom);
    }
}
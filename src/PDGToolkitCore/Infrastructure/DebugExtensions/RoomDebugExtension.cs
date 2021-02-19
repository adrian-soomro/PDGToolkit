using System;
using System.Collections.Generic;
using System.Linq;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Infrastructure.DebugExtensions
{
    public static class RoomDebugExtension
    {
        public static void ListRooms(this IEnumerable<Room> allRooms)
        {
            var rooms = allRooms.ToList();
            for (var i = 0; i < rooms.Count; i++)
            {
                var tile = rooms[i].Tiles.First();
                Console.Out.WriteLine($"Room #{i+1} => Id: {rooms[i].Id}, around: {tile.Position.X}, {tile.Position.Y}");
            }
        }
    }
}
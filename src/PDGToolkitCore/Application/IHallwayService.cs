using System.Collections.Generic;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Application
{
    internal interface IHallwayService
    {
        IEnumerable<Room> CreateDoors(IEnumerable<Room> rooms);

        IEnumerable<Tile> CreateHallways(IEnumerable<Room> rooms);

        IEnumerable<Tile> HandleDoorTiles(IEnumerable<Tile> tiles);
    }
}
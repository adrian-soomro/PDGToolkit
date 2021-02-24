using System.Collections.Generic;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Application.PathFinding
{
    public interface IPathFindingService
    {
        IEnumerable<Tile> FindPath(Tile start, Tile finish, IEnumerable<Tile> tiles);
        
        IEnumerable<Tile> ConstructAllPaths(IEnumerable<Room> rooms);
    }
}
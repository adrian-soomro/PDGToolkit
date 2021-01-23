using System.Collections.Generic;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Application
{
    public interface ITileService
    {
        IEnumerable<Tile> FindTilesBasedOnPositionAndType(List<Tile> tiles, Position position, TileType type);
        IEnumerable<Tile> FindTilesBasedOnPositionsAndType(List<Tile> tiles, List<Position> positions, TileType type);
        bool HasTwoAdjacentFloorTiles(List<Tile> allTiles, Position positionOfTileInQuestion);
        Dictionary<Position, int> GetPositionsOfDuplicateTiles(List<Tile> tiles);
        List<Tile> RemoveOverlappingDuplicateWalls(List<Tile> allTiles);
        List<Tile> RemoveSharedWalls(List<Tile> tiles);
    }
}
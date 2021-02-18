using System.Collections.Generic;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Application.Extensions
{
    public static class TileExtensions
    {
        /**
         * Given a list of tiles, and a replacement tile,
         * Finds all tiles that share position with the replacement, removes them and
         * Adds the replacement to the list of tiles.
         */
        public static void ReplaceTilesWithOtherTile(this List<Tile> tiles, Tile replacement)
        {
            var allTilesAtThisPosition = tiles.FindAll(t => t.Position.Equals(replacement.Position));

            foreach (var tile in allTilesAtThisPosition)
            {
                tiles.Remove(tile);
            }
            
            tiles.Add(replacement);
        }
    }
}
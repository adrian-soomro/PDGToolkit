using System.Collections.Generic;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.UnitTests
{
    public class TileBuilder
    {
        private List<Tile> allTiles = new List<Tile>();

        public static TileBuilder Create()
        {
            return new TileBuilder();
        }

        public TileBuilder WithHorizontalLineOfFloorsStartingAtPosition(int floorWallWidth, Position position)
        {
            var floorTiles = CreateWallOfTileTypeAtPositionWithWidthWithOrientation(TileType.Floor, position,
                floorWallWidth, Orientation.HORIZONTAL);
            allTiles.AddRange(floorTiles);
            return this;
        }
        
        public TileBuilder WithVerticalLineOfFloorsStartingAtPosition(int floorWallWidth, Position position)
        {
            var floorTiles = CreateWallOfTileTypeAtPositionWithWidthWithOrientation(TileType.Floor, position,
                floorWallWidth, Orientation.VERTICAL);
            allTiles.AddRange(floorTiles);
            return this;
        }
        
        public TileBuilder WithHorizontalWallStartingAtPosition(int wallWidth, Position position)
        {
            var wallTiles = CreateWallOfTileTypeAtPositionWithWidthWithOrientation(TileType.Wall, position, wallWidth,
                Orientation.HORIZONTAL);
            allTiles.AddRange(wallTiles);
            return this;
        }

        public TileBuilder WithVerticalWallStartingAtPosition(int wallWidth, Position position)
        {
            var wallTiles = CreateWallOfTileTypeAtPositionWithWidthWithOrientation(TileType.Wall, position, wallWidth,
                Orientation.VERTICAL);
            allTiles.AddRange(wallTiles);
            return this;
        }

        private List<Tile> CreateWallOfTileTypeAtPositionWithWidthWithOrientation(TileType type, Position position, int width, Orientation orientation)
        {
            var tiles = new List<Tile>();
            for (var i = 0; i < width; i++)
            {
                var x = orientation == Orientation.HORIZONTAL ? position.X + i : position.X;
                var y = orientation == Orientation.VERTICAL ? position.Y + i : position.Y;
                
                tiles.Add(new Tile(type, new Position(x, y)));
            }

            return tiles;
        }

        public List<Tile> Build()
        {
            return allTiles;
        }
    }
    
    internal enum Orientation {HORIZONTAL, VERTICAL}
}
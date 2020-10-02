using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PDGToolkitAPI.Domain.Models;

namespace PDGToolkitAPI.Application
{
    public class RoomBuilder
    {
        private Position StartingPosition { get; }
        private const int IndexOffset = 1;
        private const int WallThickness = 1;
        private int LengthOfX { get; }
        private int LengthOfY { get; }

        public RoomBuilder(Position startingPosition, int lengthOfX, int lengthOfY)
        {
            StartingPosition = startingPosition;
            LengthOfX = lengthOfX;
            LengthOfY = lengthOfY;
        }
        
        public async Task<List<Tile>> CreateOuterWallsAsync()
        {
            return await Task.Run(() =>
            {
                var tiles = new List<Tile>();
                for (var thickness = 0; thickness < WallThickness; thickness++)
                {
                    for (var x = StartingPosition.X; x < StartingPosition.X + LengthOfX; x++)
                    {
                        tiles.Add(new Tile(TileType.Wall, new Position(x, StartingPosition.Y + thickness)));
                        tiles.Add(new Tile(TileType.Wall, new Position(x, StartingPosition.Y + LengthOfY - IndexOffset - thickness)));
                    }
                    
                    for (var y = StartingPosition.Y; y < StartingPosition.Y + LengthOfY; y++)
                    {
                        tiles.Add(new Tile(TileType.Wall, new Position(StartingPosition.X + thickness, y)));
                        tiles.Add(new Tile(TileType.Wall, new Position(StartingPosition.X + LengthOfX - IndexOffset - thickness, y)));
                    }
                }

                return tiles;
            });
        }

        public async Task<List<Tile>> FillInsideTiles(Func<int, Task<List<Tile>>> roomFillingFunction)
        {
            return await roomFillingFunction.Invoke(WallThickness);
        }

        public async Task<List<Tile>> FillInsideTilesWith(TileType type)
        {
            return await Task.Run(() =>
            {
                var tiles = new List<Tile>();
                for (var x = StartingPosition.X + WallThickness;
                    x < StartingPosition.X + LengthOfX - WallThickness;
                    x++)
                {
                    for (var y = StartingPosition.Y + WallThickness;
                        y < StartingPosition.Y + LengthOfY - WallThickness;
                        y++)
                    {
                        tiles.Add(new Tile(type, new Position(x, y)));
                    }
                }

                return tiles;
            });
        }

    }
}
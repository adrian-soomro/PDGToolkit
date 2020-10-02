using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PDGToolkitAPI.Domain.Models;

namespace PDGToolkitAPI.Application
{
    public class RoomBuilder
    {
        public RoomBuilder(Position startingPosition, int lengthOfX, int lengthOfY)
        {
            StartingPosition = startingPosition;
            LengthOfX = lengthOfX;
            LengthOfY = lengthOfY;
        }

        private const int IndexOffset = 1;
        private const int WallThickness = 1;
        private int LengthOfX { get; }
        private int LengthOfY { get; }
        
        private Position StartingPosition { get; }
        
        public async Task<List<Tile>> CreateOuterWallsAsync()
        {
            return await Task.Run(() =>
            {
                var tiles = new List<Tile>();
                for (var thickness = 0; thickness < WallThickness; thickness++)
                {
                    for (var x = StartingPosition.X; x < StartingPosition.X + LengthOfX; x++)
                    {
                        tiles.Add(new Tile(TileType.Wall, new Position(x, thickness)));
                        tiles.Add(new Tile(TileType.Wall, new Position(x, LengthOfY - IndexOffset - thickness)));
                    }
                    
                    for (var y = StartingPosition.Y; y < StartingPosition.Y + LengthOfY; y++)
                    {
                        tiles.Add(new Tile(TileType.Wall, new Position(thickness, y)));
                        tiles.Add(new Tile(TileType.Wall, new Position(LengthOfX - IndexOffset - thickness, y)));
                    }
                }

                return tiles;
            });
        }

        public async Task<List<Tile>> FillInsideTiles(Func<int, List<Tile>> func)
        {
            return await Task.Run(() => func.Invoke(WallThickness));
        }

    }
}
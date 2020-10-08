using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PDGToolkitAPI.Domain.Models;
using PDGToolkitAPI.Infrastructure;

namespace PDGToolkitAPI.Application
{
    public class SmallRoomsGenerator : IGenerator
    {
        private readonly Settings settings;
        private readonly Random random = new Random();
        private readonly RoomBuilder roomBuilder;
        private readonly int miniRoomMaxX;
        private readonly int miniRoomMaxY;
        private const int MinimumRoomSize = 3;

        private int Width { get; }
        private int Height { get; }
        private int oneInXChanceToGenerateARoom = 600;
        
        public SmallRoomsGenerator(Settings settings)
        {
            this.settings = settings;
            Width = settings.GridSettings.Width / settings.TileSettings.Size;
            Height = settings.GridSettings.Height / settings.TileSettings.Size;
            roomBuilder = new RoomBuilder(new Position(0,0),Width, Height);
            miniRoomMaxX = Width / 4;
            miniRoomMaxY = Height / 4;
        }

        public async Task<Grid> GenerateGridAsync()
        {
            var tiles = await roomBuilder.CreateOuterWallsAsync();
            
            tiles.AddRange(await roomBuilder.FillInsideTiles(wallThickness => GenerateSmallRoomsAsync(wallThickness)));
            return new Grid(settings.GridSettings.Height, settings.GridSettings.Width,
                new TileConfig(settings.TileSettings.Size), tiles);
        }

        private async Task<List<Tile>> GenerateSmallRoomsAsync(int wallThickness)
        {
            var tiles = new List<Tile>();
                for (var x = wallThickness; x < Width - wallThickness; x++)
                {
                    for (var y = wallThickness; y < Height - wallThickness; y++)
                    {
                        if (OneIn(oneInXChanceToGenerateARoom))
                        {
                            var smallRoomBuilder = new RoomBuilder(new Position(x, y),
                                SelectWallLength(miniRoomMaxX), SelectWallLength(miniRoomMaxY));
                            var roomWalls = await smallRoomBuilder.CreateOuterWallsAsync();
                            var roomFloor = await smallRoomBuilder.FillInsideTilesWith(TileType.Floor);
                            
                            tiles.AddRange(roomWalls);
                            tiles.AddRange(roomFloor);
                        }
                    }
                }

                return tiles;
        }
        
        private int SelectWallLength(int maxSize)
        {
            return random.Next(MinimumRoomSize, maxSize);;
        }

        private bool OneIn(int chance)
        {
            return random.Next(chance) < 1;
        }
    }
}
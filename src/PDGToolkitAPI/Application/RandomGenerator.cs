using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PDGToolkitAPI.Domain.Models;
using PDGToolkitAPI.Infrastructure;

namespace PDGToolkitAPI.Application
{
    public class RandomGenerator : IGenerator
    {
        private readonly Random random = new Random();
        private readonly Settings settings;
        private readonly RoomBuilder roomBuilder;
        private int Width { get; }
        private int Height { get; }

        public RandomGenerator(Settings settings)
        {
            this.settings = settings;
            Width = settings.GridSettings.Width / settings.TileSettings.Size;
            Height = settings.GridSettings.Height / settings.TileSettings.Size;
            roomBuilder = new RoomBuilder(new Position(0,0), Width, Height);
        }

        public async Task<Grid> GenerateGridAsync()
        {
            var tiles = await roomBuilder.CreateOuterWallsAsync();
            tiles.AddRange(await roomBuilder.FillInsideTiles(wallThickness => CreateFloor(wallThickness)));
            
            return new Grid(settings.GridSettings.Height, settings.GridSettings.Width,
                new TileConfig(settings.TileSettings.Size), tiles);
        }

        private async Task<List<Tile>> CreateFloor(int wallThickness)
        {
            return await Task.Run(() => 
            {
                var tiles = new List<Tile>();
                for (var x = wallThickness; x < Width - wallThickness; x++)
                {
                    for (var y = wallThickness; y < Height - wallThickness; y++)
                    {
                        tiles.Add(new Tile(GenerateRandomTileType(), new Position(x, y)));
                    }
                }
                
                return tiles;
            });
        }

        private TileType GenerateRandomTileType()
        {
            return random.Next(1, 11) > 8 ? TileType.Obstacle : TileType.Floor;
        }
    }
}
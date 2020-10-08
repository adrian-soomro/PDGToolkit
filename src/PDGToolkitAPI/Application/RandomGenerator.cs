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
        private int Width { get; }
        private int Height { get; }
        
        public RandomGenerator(Settings settings)
        {
            this.settings = settings;
            Width = settings.GridSettings.Width / settings.TileSettings.Size;
            Height = settings.GridSettings.Height / settings.TileSettings.Size;
        }

        public async Task<Grid> GenerateGridAsync()
        {
            //TODO: make RoomBuilder async
            var room = RoomBuilder.Create()
                .WithHeight(Height)
                .WithWidth(Width)
                .WithOutsideWalls()
                .WithInsideTiles(wallThickness => CreateFloor(wallThickness))
                .Build();

            return new Grid(settings.GridSettings.Height, settings.GridSettings.Width,
                new TileConfig(settings.TileSettings.Size), room.Tiles);
        }

        private List<Tile> CreateFloor(int wallThickness)
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
        }

        private TileType GenerateRandomTileType()
        {
            return random.Next(1, 11) > 8 ? TileType.Obstacle : TileType.Floor;
        }
    }
}
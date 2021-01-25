using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PDGToolkitCore.Domain.Models;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCore.Application
{
    /**
     * A primitive Generator used to develop and verify generator loading features
     */
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
            var room = await RoomBuilder.Create()
                .WithHeight(Height)
                .WithWidth(Width)
                .WithOutsideWalls()
                .WithInsideTiles(wallThickness => CreateFloor(wallThickness))
                .BuildAsync();

            return new Grid(settings.GridSettings.Height, settings.GridSettings.Width,
                new TileConfig(settings.TileSettings.Size), room.Tiles);
        }

        private async Task<List<Tile>> CreateFloor(int wallThickness)
        {
            return await Task.Run((() =>
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
            }));
        }

        private TileType GenerateRandomTileType()
        {
            return random.Next(1, 11) > 8 ? TileType.Obstacle : TileType.Floor;
        }
    }
}
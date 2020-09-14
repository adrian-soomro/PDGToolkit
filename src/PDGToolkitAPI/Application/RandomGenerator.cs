using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PDGToolkitAPI.Domain.Models;
using PDGToolkitAPI.Infrastructure;

namespace PDGToolkitAPI.Application
{
    public class RandomGenerator : IGenerator
    {
        private readonly Settings settings;
        private readonly Random random = new Random();

        public RandomGenerator(Settings settings)
        {
            this.settings = settings;
        }

        public async Task<Grid> GenerateGridAsync()
        {
            return await Task.Run(() =>
            {
                var tiles = new List<Tile>();
                for (var x = 0; x < settings.GridSettings.Width / settings.TileSettings.Size; x++)
                {
                    for (var y = 0; y < settings.GridSettings.Height / settings.TileSettings.Size; y++)
                    {
                        var tile = new Tile(GenerateRandomTileType(), new Position(x, y));
                        tiles.Add(tile);
                    }
                }

                return new Grid(settings.GridSettings.Height, settings.GridSettings.Width,
                    new TileConfig(settings.TileSettings.Size), tiles);
            });
        }

        private TileType GenerateRandomTileType()
        {
            return random.Next(0, 3) switch
            {
                0 => TileType.Floor,
                1 => TileType.Obstacle,
                2 => TileType.Wall,
                _ => TileType.Floor
            };
        }
    }
}
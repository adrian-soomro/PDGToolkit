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
        private readonly ReferenceFrame referenceFrame;
        
        public RandomGenerator(Settings settings)
        {
            this.settings = settings;
            referenceFrame = new ReferenceFrame(0, 0,
                            settings.GridSettings.Width / settings.TileSettings.Size,
                            settings.GridSettings.Height / settings.TileSettings.Size);
        }

        public async Task<Grid> GenerateGridAsync()
        {
            var tiles = new List<Tile>(await CreateWalls());
            for (var x = referenceFrame.MinX; x < referenceFrame.MaxX; x++)
            {
                for (var y = referenceFrame.MinY; y < referenceFrame.MaxY; y++)
                {
                    var tile = new Tile(GenerateRandomTileType(), new Position(x, y));
                    tiles.Add(tile);
                }
            }
            return new Grid(settings.GridSettings.Height, settings.GridSettings.Width,
                new TileConfig(settings.TileSettings.Size), tiles);
        }
        
        private async Task<List<Tile>> CreateWalls()
        {
            var walls = new List<Tile>();
            walls.AddRange(await CreateTopWall());
            walls.AddRange(await CreateBottomWall());
            walls.AddRange(await CreateLeftWall());
            walls.AddRange(await CreateRightWall());
            return walls;
        }
        
        private async Task<List<Tile>> CreateTopWall()
        {
            return await Task.Run(() =>
            {
                var topTiles = new List<Tile>();
                for (var x = referenceFrame.MinX; x < referenceFrame.MaxX; x++)
                {
                    var tile = new Tile(TileType.Wall, new Position(x, referenceFrame.MinY));
                    topTiles.Add(tile);
                }
                referenceFrame.IncreaseMinY();
                return topTiles;
            });
        }
        
        private async Task<List<Tile>> CreateBottomWall()
        {
            return await Task.Run(() =>
            {
                referenceFrame.DecreaseMaxY();
                var tiles = new List<Tile>();
                for (var x = referenceFrame.MinX; x < referenceFrame.MaxX; x++)
                {
                    var tile = new Tile(TileType.Wall, new Position(x, referenceFrame.MaxY));
                    tiles.Add(tile);
                }
                return tiles;
            });
        }
        
        private async Task<List<Tile>> CreateLeftWall()
        {
            return await Task.Run(() =>
            {
                var tiles = new List<Tile>();
                for (var y = referenceFrame.MinY; y < referenceFrame.MaxY; y++)
                {
                    var tile = new Tile(TileType.Wall, new Position(referenceFrame.MinX, y));
                    tiles.Add(tile);
                }
                referenceFrame.IncreaseMinX();
                return tiles;
            });
        }
        
        private async Task<List<Tile>> CreateRightWall()
        {
            return await Task.Run(() =>
            {
                referenceFrame.DecreaseMaxX();
                var tiles = new List<Tile>();
                for (var y = referenceFrame.MinY; y < referenceFrame.MaxY; y++)
                {
                    var tile = new Tile(TileType.Wall, new Position(referenceFrame.MaxX, y));
                    tiles.Add(tile);
                }
                return tiles;
            });
        }

        private TileType GenerateRandomTileType()
        {
            return random.Next(1, 11) > 8 ? TileType.Obstacle : TileType.Floor;
        }

        private class ReferenceFrame
        {
           public int MinX { get; private set; } 
           public int MinY { get; private set; } 
           public int MaxX { get; private set; } 
           public int MaxY { get; private set; }

           public ReferenceFrame(int minX, int minY, int maxX, int maxY)
           {
               MinX = minX;
               MinY = minY;
               MaxX = maxX;
               MaxY = maxY;
           }
           
           public void IncreaseMinX() => MinX++;
           public void IncreaseMinY() => MinY++;
           public void DecreaseMaxX() => MaxX--;
           public void DecreaseMaxY() => MaxY--;
        }
    }
}
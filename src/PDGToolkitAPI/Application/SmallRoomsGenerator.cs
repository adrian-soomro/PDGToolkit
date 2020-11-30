using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PDGToolkitAPI.Domain.Models;
using PDGToolkitAPI.Infrastructure;

namespace PDGToolkitAPI.Application
{
    public class SmallRoomsGenerator : IGenerator
    {
        private readonly Settings settings;
        private readonly Random random = new Random();
        private const int MinimumRoomSize = 3;
        private const int OneInXChanceToGenerateARoom = 600;

        private int Width { get; }
        private int Height { get; }
        
        public SmallRoomsGenerator(Settings settings)
        {
            this.settings = settings;
            Width = settings.GridSettings.Width / settings.TileSettings.Size;
            Height = settings.GridSettings.Height / settings.TileSettings.Size;
        }

        public async Task<Grid> GenerateGridAsync()
        {
            var room = RoomBuilder.Create()
                .WithHeight(Height)
                .WithWidth(Width)
                .WithInsideTiles(wallThickness => GenerateSmallRooms(wallThickness))
                .WithOutsideWalls()
                .Build();
            
            return new Grid(settings.GridSettings.Height, settings.GridSettings.Width,
                new TileConfig(settings.TileSettings.Size), room.Tiles);
        }
        
        private List<Tile> GenerateSmallRooms(int wallThickness)
        {
            var tiles = new List<Tile>();
                for (var x = wallThickness; x < Width - wallThickness; x++)
                {
                    for (var y = wallThickness; y < Height - wallThickness; y++)
                    {
                        if (OneIn(OneInXChanceToGenerateARoom))
                        {
                            var room = RoomBuilder.Create()
                                .WithWidth(SelectRoomWidth)
                                .WithHeight(SelectRoomHeight)
                                .WithStartingPosition(new Position(x, y))
                                .WithOutsideWalls()
                                .WithInsideTilesOfType(TileType.Floor)
                                .Build();

                            tiles.AddRange(room.Tiles);
                        }
                    }
                }

                return RemoveDuplicateWalls(tiles);
        }

        private List<Tile> RemoveDuplicateWalls(List<Tile> tiles)
        {
            var duplicateElements = tiles.GroupBy(t => t.Position)
                .Where(g => g.Count() > 1)
                .Select(y => new { Element = y.Key})
                .ToList();

            var dupes = duplicateElements.Select(q => q.Element);
            
            var toBeRemoved = tiles.Where(tile => dupes.Contains(tile.Position) && tile.Type == TileType.Wall).ToList();


            foreach (var tileToBeRemoved in toBeRemoved)
            {
                var dupe = tiles.Find(t => t.Position.Equals(tileToBeRemoved.Position) && t.Type.Equals(TileType.Floor));
                if (dupe != null )
                {
                    tiles.Remove(tileToBeRemoved);
                }
            }
  
            return tiles;
        }
        
        // TODO: Refactor magic number to a meaningful variable
        private int SelectRoomWidth => RandomlySelectWallLength(Width / 4);
        private int SelectRoomHeight => RandomlySelectWallLength(Height / 4);
        
        private int RandomlySelectWallLength(int maxLength)
        {
            return random.Next(MinimumRoomSize, maxLength);;
        }

        private bool OneIn(int chance)
        {
            return random.Next(chance) < 1;
        }
    }
}
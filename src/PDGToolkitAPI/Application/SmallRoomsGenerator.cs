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

                tiles = RemoveOverlappingDuplicateWalls(tiles);
                return RemoveSharedWalls(tiles);
        }

        private Dictionary<Position, int> GetPositionsOfDuplicateTiles(List<Tile> tiles)
        {
            return tiles.GroupBy(t => t.Position)
                        .Where(g => g.Count() > 1)
                        .Select(y => new { Element = y.Key, Count = y.Count()})
                        .ToDictionary(x => x.Element, y => y.Count);
        }
        
        private List<Tile> RemoveOverlappingDuplicateWalls(List<Tile> allTiles)
        {
            var positionToNumberOfWalls = GetPositionsOfDuplicateTiles(allTiles);
            var duplicatePositions = positionToNumberOfWalls.Keys.ToList();

            var floorTilesWithDuplicateTilesInTheSamePosition = FindTilesBasedOnPositionsAndType(allTiles, duplicatePositions, TileType.Floor);

            var toBeRemoved = new List<Tile>();
            
            foreach (var tile in floorTilesWithDuplicateTilesInTheSamePosition)
            {
                var dupes =  FindTilesBasedOnPositionAndType(allTiles, tile.Position, TileType.Wall);

                toBeRemoved.AddRange(dupes);
            }

            foreach (var tile in toBeRemoved)
            {
                allTiles.Remove(tile);
            }
            return allTiles;
        }
        
        private List<Tile> RemoveSharedWalls(List<Tile> tiles)
        {
            var positionToNumberOfDuplicates = GetPositionsOfDuplicateTiles(tiles);
            var allDuplicatePositions = positionToNumberOfDuplicates.Keys.ToList();

            var duplicateWalls = FindTilesBasedOnPositionsAndType(tiles, allDuplicatePositions, TileType.Wall);
            var toBeRemoved = new List<Tile>();
            foreach (var tile in duplicateWalls)
            {
                positionToNumberOfDuplicates.TryGetValue(tile.Position, out var numberOfWallTilesInThisPosition);
                if (numberOfWallTilesInThisPosition >= 2 && HasTwoAdjacentFloorTiles(tiles, tile))
                {
                    toBeRemoved.Add(tile);
                }
            }

            foreach (var tile in toBeRemoved)
            {
                tiles.Add(new Tile(TileType.Floor, tile.Position));
                tiles.Remove(tile);
            }

            return tiles;
        }
        
        private List<Tile> FindTilesBasedOnPositionAndType(List<Tile> tiles, Position position, TileType type)
        {
            return tiles.FindAll(t => t.Position.Equals(position) && t.Type.Equals(type));  
        }
        
        private List<Tile> FindTilesBasedOnPositionsAndType(List<Tile> tiles, List<Position> positions, TileType type)
        {
            return tiles.FindAll(t => positions.Contains(t.Position) && t.Type.Equals(type));  
        }
        
        private bool HasTwoAdjacentFloorTiles(List<Tile> allTiles, Tile tileInQuestion)
        {
            var up = new Position(tileInQuestion.Position.X, tileInQuestion.Position.Y + 1);
            var down = new Position(tileInQuestion.Position.X, tileInQuestion.Position.Y - 1);
            var left = new Position(tileInQuestion.Position.X - 1, tileInQuestion.Position.Y);
            var right = new Position(tileInQuestion.Position.X + 1, tileInQuestion.Position.Y);
            var adjacents = allTiles.Where(t => t.Type.Equals(TileType.Floor)).ToList().FindAll(t =>
                t.Position.Equals(up) || t.Position.Equals(down) || t.Position.Equals(left) ||
                t.Position.Equals(right));

            var adjacentFloorPositions = adjacents.Select(a => a.Position); 
            return adjacentFloorPositions.Contains(up) && adjacentFloorPositions.Contains(down) || adjacentFloorPositions.Contains(right) && adjacentFloorPositions.Contains(left);
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
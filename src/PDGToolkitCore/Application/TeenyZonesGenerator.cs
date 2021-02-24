using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PDGToolkitCore.Application.PathFinding;
using PDGToolkitCore.Domain.Models;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCore.Application
{
    internal class TeenyZonesGenerator : IGenerator
    {
        private readonly Settings settings;
        private readonly Random random;
        private readonly IRoomService roomService;
        private readonly IPathFindingService pathFindingService;
        private const int MinimumRoomSize = 4;
        private const int OneInXChanceToGenerateARoom = 600;

        private int Width { get; }
        private int Height { get; }
        
        public TeenyZonesGenerator(Settings settings, IRoomService roomService, Random random, IPathFindingService pathFindingService)
        {
            this.settings = settings;
            this.roomService = roomService;
            this.random = random;
            this.pathFindingService = pathFindingService;
            Width = settings.GridSettings.Width / settings.TileSettings.Size;
            Height = settings.GridSettings.Height / settings.TileSettings.Size;
        }

        /**
         * Generates a Grid asynchronously, using the RoomBuilder class.
         * It does so by generating a room in the size of the dungeon and fills it using <see cref="GenerateRooms"/> function,
         */
        public async Task<Grid> GenerateGridAsync()
        {
            var room = await RoomBuilder.Create()
                .WithHeight(Height)
                .WithWidth(Width)
                .WithInsideTiles(wallThickness => GenerateRooms(wallThickness))
                .WithOutsideWalls()
                .BuildAsync();
          
            return new Grid(settings.GridSettings.Height, settings.GridSettings.Width,
                new TileConfig(settings.TileSettings.Size), room.Tiles);
        }

        /**
         * Select random points in the room using the <see cref="OneInXChanceToGenerateARoom"/> function,
         * then create new, smaller rooms of varying size <see cref="SelectRoomWidth"/> & <see cref="SelectRoomHeight"/>
         * and lastly, use <see cref="ITileService"/> to remove merge overlapping rooms into one bigger, more naturally
         * looking room.
         */
        private async Task<List<Tile>> GenerateRooms(int wallThickness)
        {
            var allRooms = new List<Room>();
            for (var x = wallThickness; x < Width - wallThickness - MinimumRoomSize; x++)
            {
                for (var y = wallThickness; y < Height - wallThickness - MinimumRoomSize; y++)
                {
                    if (OneIn(OneInXChanceToGenerateARoom))
                    {
                        var room = await RoomBuilder.Create()
                            .WithWidth(SelectRoomWidth)
                            .WithHeight(SelectRoomHeight)
                            .WithStartingPosition(new Position(x, y))
                            .WithOutsideWalls()
                            .WithInsideTilesOfType(TileType.Floor)
                            .BuildAsync();

                        allRooms.Add(room);
                    }
                }
            }
            allRooms = roomService.TrimSpilledRooms(allRooms).ToList();
            var mergedRooms = roomService.MergeAllRooms(allRooms).ToList();
            roomService.CreateDoors(mergedRooms);
            var hallways = pathFindingService.ConstructAllPaths(mergedRooms);
            return mergedRooms.SelectMany(r => r.Tiles).Concat(hallways).ToList();
        }
        
        private int MaximumRoomWidth => Width / 4;
        private int MaximumRoomHeight => Height / 4;
        
        private int SelectRoomWidth => RandomlySelectWallLength(MaximumRoomWidth);
        private int SelectRoomHeight => RandomlySelectWallLength(MaximumRoomHeight);
        
        private int RandomlySelectWallLength(int maxLength)
        {
            return random.Next(MinimumRoomSize, maxLength);
        }

        /**
         * Perform a pseudo-random roll, having one in <paramref name="chance"/> to succeed,
         * by generating a number between 0 and <paramref name="chance"/> - 1.
         * <returns> true if roll is 0, false otherwise</returns>
         */
        private bool OneIn(int chance)
        {
            return random.Next(chance) < 1;
        }
    }
}
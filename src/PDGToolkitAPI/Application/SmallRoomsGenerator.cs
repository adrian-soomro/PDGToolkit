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

        private int Width { get; }
        private int Height { get; }
        private int oneInXChance = 600;
        
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
            
            tiles.AddRange(await roomBuilder.FillInsideTiles(wallThickness => Kek(wallThickness)));
            return new Grid(settings.GridSettings.Height, settings.GridSettings.Width,
                new TileConfig(settings.TileSettings.Size), tiles);
        }

        /*
         go across the available space
         and on each tile, theres's a X(5)% chance that this is a random Spot - center of a rectangle
         if so, increment the number of allocated random spots & draw a rectangle with random x ( maxX / 4 ) and y (max Y / 4)   
         meaning draw the walls if possible, don't draw anything outside. 
         drawing walls can be done like so: for i in wallLength, make sure no walls are drawn outside of the great boundary
         */
        private async Task<List<Tile>> Kek(int wallThickness)
        {
            var tiles = new List<Tile>();
                for (var x = wallThickness; x < Width - wallThickness; x++)
                {
                    for (var y = wallThickness; y < Height - wallThickness; y++)
                    {
                        if (random.Next(oneInXChance) < 1)
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
        
         // start counting from 3, as 0 can lead to really small rooms
        private int SelectWallLength(int maxSize)
        {
            return random.Next(3, maxSize + 1);
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Application
{
    /**
     * This builder offers to build <see cref="Room"/> asynchronously.
     * Inspiration on how to tackle asynchronous builder pattern: https://stackoverflow.com/questions/25302178/using-async-tasks-with-the-builder-pattern
     */
    public class RoomBuilder
    {
        private const int IndexOffset = 1;
        private int width;
        private int height;
        private int wallThickness;
        private Position startingPosition;
        private List<Tile> tiles;
        
        /**
         * The private fields below represent tasks that return a List of tiles when completed.
         * By default, they return an empty list, however, when the user calls one of hte builder's methods
         * eg <see cref="WithOutsideWalls"/>, the field now points to a task that's returned by
         * <see cref="CreateOuterWallsAsync"/> method and when <see cref="BuildAsync"/> is called, it awaits the result,
         * appending it to the list of created tiles.
         */
        private Task<List<Tile>> createOutsideWallsTask = Task.FromResult(new List<Tile>());
        private Task<List<Tile>> callRoomFillingFunctionTask = Task.FromResult(new List<Tile>());
        private Task<List<Tile>> fillInsideWithTilesOfTypeTask = Task.FromResult(new List<Tile>());

        private RoomBuilder()
        {
            startingPosition = new Position(0,0);
            tiles = new List<Tile>();
            wallThickness = 1;
        }

        public static RoomBuilder Create()
        {
            return new RoomBuilder();
        }

        public RoomBuilder WithWidth(int width)
        {
            this.width = width;
            return this;
        }
        
        public RoomBuilder WithHeight(int height)
        {
            this.height = height;
            return this;
        }

        public RoomBuilder WithWallThickness(int wallThickness)
        {
            this.wallThickness = wallThickness;
            return this;
        }
        
        public RoomBuilder WithStartingPosition(Position startingPosition)
        {
            this.startingPosition = startingPosition;
            return this;
        }

        public RoomBuilder WithOutsideWalls()
        {
            createOutsideWallsTask = CreateOuterWallsAsync();
            return this;
        }
        
        public RoomBuilder WithInsideTiles(Func<int, Task<List<Tile>>> roomFillingFunction)
        {
            callRoomFillingFunctionTask = roomFillingFunction.Invoke(wallThickness);
            return this;
        }
        
        public RoomBuilder WithInsideTilesOfType(TileType type)
        {
            fillInsideWithTilesOfTypeTask= FillInsideTilesWithAsync(type);
            return this;
        }

        private async Task<List<Tile>> CreateOuterWallsAsync()
        {
            return await Task.Run(() =>
            {
                var outerWalls = new List<Tile>();
                for (var thickness = 0; thickness < wallThickness; thickness++)
                {
                    for (var x = startingPosition.X; x < startingPosition.X + width; x++)
                    {
                        outerWalls.Add(new Tile(TileType.Wall, new Position(x, startingPosition.Y + thickness)));
                        outerWalls.Add(new Tile(TileType.Wall, new Position(x, startingPosition.Y + height - IndexOffset - thickness)));
                    }
                
                    for (var y = startingPosition.Y; y < startingPosition.Y + height; y++)
                    {
                        outerWalls.Add(new Tile(TileType.Wall, new Position(startingPosition.X + thickness, y)));
                        outerWalls.Add(new Tile(TileType.Wall, new Position(startingPosition.X + width - IndexOffset - thickness, y)));
                    }
                }
            
                return outerWalls;
            });
        }

        private async Task<List<Tile>> FillInsideTilesWithAsync(TileType type)
        {
            return await Task.Run(() =>
            {
                var insideTiles = new List<Tile>();
                for (var x = startingPosition.X + wallThickness;
                    x < startingPosition.X + width - wallThickness;
                    x++)
                {
                    for (var y = startingPosition.Y + wallThickness;
                        y < startingPosition.Y + height - wallThickness;
                        y++)
                    {
                        insideTiles.Add(new Tile(type, new Position(x, y)));
                    }
                }

                return insideTiles;
            });
        }

        public async Task<Room> BuildAsync()
        {
            tiles.AddRange(await fillInsideWithTilesOfTypeTask);
            tiles.AddRange(await callRoomFillingFunctionTask);
            tiles.AddRange(await createOutsideWallsTask);
            return new Room(width, height, startingPosition, tiles);
        }
    }
    
}
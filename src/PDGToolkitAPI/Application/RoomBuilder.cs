using System;
using System.Collections.Generic;
using PDGToolkitAPI.Domain.Models;

namespace PDGToolkitAPI.Application
{
    public class RoomBuilder
    {
        private const int IndexOffset = 1;
        private int width;
        private int height;
        private int wallThickness;
        private Position startingPosition;
        private List<Tile> tiles;
        
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
            tiles.AddRange(CreateOuterWalls());
            return this;
        }

        
        public RoomBuilder WithInsideTiles(Func<int, List<Tile>> roomFillingFunction)
        {
            tiles.AddRange(roomFillingFunction.Invoke(wallThickness));
            return this;
        }
        
        public RoomBuilder WithInsideTilesOfType(TileType type)
        {
            tiles.AddRange(FillInsideTilesWith(type));
            return this;
        }

        public Room Build()
        {
            return new Room(width, height, startingPosition, tiles);
        }
        
        private List<Tile> CreateOuterWalls()
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
        }
        

        private List<Tile> FillInsideTilesWith(TileType type)
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
        }

    }
}
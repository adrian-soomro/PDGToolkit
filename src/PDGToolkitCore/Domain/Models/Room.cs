using System;
using System.Collections.Generic;

namespace PDGToolkitCore.Domain.Models
{
    public class Room
    {
        public Guid Id { get; }
        public int Width { get; }
        public int Height { get; }
        public Position StartingPosition { get; }
        public List<Tile> Tiles { get; }


        public Room(int width, int height, Position startingPosition, List<Tile> tiles, Guid id = new Guid())
        {
            if (id.Equals(Guid.Empty))
                id = Guid.NewGuid();
            Id = id;
            Width = width;
            Height = height;
            StartingPosition = startingPosition;
            Tiles = tiles;
        }
    }
}
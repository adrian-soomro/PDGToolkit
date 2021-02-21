using System;
using System.Collections.Generic;

namespace PDGToolkitCore.Domain.Models
{
    public class Room : IEquatable<Room>
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

        public bool Equals(Room other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Room) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
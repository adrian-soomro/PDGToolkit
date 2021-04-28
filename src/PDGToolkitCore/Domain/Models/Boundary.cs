namespace PDGToolkitCore.Domain
{
    /**
     * Represents positional boundaries that need to be respected when placing / manipulating certain tiles
     * by <see cref="RoomService"/>.
     */
    internal class Boundary
    {
        public int MinX { get; }
        public int MaxX { get; }
        public int MinY { get; }
        public int MaxY { get; }

        public Boundary(int minX, int maxX, int minY, int maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }
    }
}
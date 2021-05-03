using System;

namespace PDGToolkitCLI.Validation
{
    internal class DungeonDimensionValidator
    {
        private readonly int tileSize;
        private readonly int size;
        private readonly string property;

        public DungeonDimensionValidator(string property, int size, int tileSize) 
        {
            this.tileSize = tileSize;
            this.size = size;
            this.property = property;
        }
        
        public void Validate() {
            if (!IsSizeDivisibleByTileSize()){
                Console.Error.WriteLine($"It seems that its not possible to divide {size} by {tileSize} so that there's no remainder, please set the value of {property} to be divisible by {tileSize}");
                Environment.Exit(1);
            }
        }

        private bool IsSizeDivisibleByTileSize()
        {
            return size % tileSize == 0;
        }
    }
}
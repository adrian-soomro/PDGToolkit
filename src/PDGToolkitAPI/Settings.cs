using Microsoft.Extensions.Configuration;
using PDGToolkitAPI.Domain.Models;

namespace PDGToolkitAPI
{
    public class Settings
    {
        public GridSettings GridSettings { get; }
        public TileSettings TileSettings { get; }

        public Settings(IConfiguration config)
        {
            GridSettings = config.GetSection("grid").Get<GridSettings>();
            TileSettings = config.GetSection("tiles").Get<TileSettings>();
        }
    }
    
    public class GridSettings
    {
        public int Height { get; set; }
        public int Width { get; set; }
    }
    
    public class TileSettings
    {
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
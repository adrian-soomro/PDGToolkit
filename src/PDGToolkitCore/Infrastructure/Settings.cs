using Microsoft.Extensions.Configuration;

namespace PDGToolkitCore.Infrastructure
{
    public class Settings
    {
        public GridSettings GridSettings { get; }
        public TileSettings TileSettings { get; }
        public string RelativePathToOutput { get; }
        public string Generator { get; }
        public string Serialiser { get; }

        public Settings(IConfiguration config)
        {
            GridSettings = config.GetSection("grid").Get<GridSettings>();
            TileSettings = config.GetSection("tiles").Get<TileSettings>();
            RelativePathToOutput = SetRelativePathToOutput(config);
            Generator = config.GetValue<string>("generator");
            Serialiser = config.GetValue<string>("serialiser");
        }

        private string SetRelativePathToOutput(IConfiguration config)
        {
            var fileName = config.GetValue<string>("outputRelativePath");
            var serialiser = config.GetValue<string>("serialiser");
            var serialiserPrefix = serialiser.Replace("Serialiser", "").ToLower();
            return $"{fileName}.{serialiserPrefix}";
        }
    }
    
    public class GridSettings
    {
        public int Height { get; set; }
        public int Width { get; set; }
    }
    
    public class TileSettings
    {
        public int Size { get; set; }
    }
}
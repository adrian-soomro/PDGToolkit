using System.IO;

namespace PDGToolkitCLI
{
    /*
     * Handles config.json (settings) file that gets copied to the output directory on build,
     * by modifying certain fields of hte config file.
     * This results in modifying certain settings of the toolkit that apply to only one run.
     */
    public static class SettingsHandler
    {
        private static readonly string PathToConfigInOutDirectory =
            Path.Combine(Directory.GetCurrentDirectory(), "config.json");
        
        internal static void EditOutputRelativePathSetting(string relativePath)
        {
            var settings = ReadSettingsObject();
            settings["outputRelativePath"] = relativePath;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(PathToConfigInOutDirectory, output);
        }

        internal static void EditHeightSetting(int height)
        {
            var settings = ReadSettingsObject();
            settings["grid"]["height"] = height.ToString();
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(PathToConfigInOutDirectory, output);
        }
        
        internal static void EditWidthSetting(int width)
        {
            var settings = ReadSettingsObject();
            settings["grid"]["width"] = width.ToString();
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(PathToConfigInOutDirectory, output);
        }

        private static dynamic ReadSettingsObject()
        {
            var pathToConfigInOutDirectory = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
            var jsonAsText = File.ReadAllText(pathToConfigInOutDirectory);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonAsText);
        }
        
        
    }
}
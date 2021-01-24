using System.IO;

namespace PDGToolkitCLI
{
    /*
     * Handles config.json (settings) file that gets copied to the output directory on build,
     * by modifying certain fields of hte config file.
     * This results in modifying certain settings of the toolkit that apply to only one run.
     */
    public class SettingsHandler
    {
        internal static void EditOutputRelativePathSetting(string relativePath)
        {
            var pathToConfigInOutDirectory = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
            var jsonAsText = File.ReadAllText(pathToConfigInOutDirectory);
            dynamic settings = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonAsText);
            settings["outputRelativePath"] = relativePath;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(pathToConfigInOutDirectory, output);
        }
    }
}
using System.IO;

namespace PDGToolkitCLI
{
    /// <summary>
    /// Makes use of the builder pattern to:
    /// Load the config.json file into a dynamic <see cref="settings"/> variable
    /// Set different values to said variable based on called methods
    /// Persist those changes back into config.json 
    /// </summary>
    public class CustomSettingsHandler
    {
        private dynamic settings;
        private readonly string pathToConfigInOutDirectory = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
        
        private CustomSettingsHandler()
        {
            settings = ReadSettingsObject();
        }

        public static CustomSettingsHandler Create()
        {
            return new CustomSettingsHandler();
        }
        
        public CustomSettingsHandler SetWidth(int width)
        {
            settings["grid"]["width"] = width.ToString();
            return this;
        }
        
        public CustomSettingsHandler SetHeight(int height)
        {
            settings["grid"]["height"] = height.ToString();
            return this;
        }

        public CustomSettingsHandler SetRelativePath(string relativePath)
        {
            settings["outputRelativePath"] = relativePath;
            return this;
        }

        public CustomSettingsHandler SetSerialiser(string serialiser)
        {
            settings["serialiser"] = serialiser;
            return this;
        }
        
        public CustomSettingsHandler SetGenerator(string generator)
        {
            settings["generator"] = generator;
            return this;
        }
        
        public void PersistChanges()
        {
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(pathToConfigInOutDirectory, output);
        }
        
        private dynamic ReadSettingsObject()
        {
            var jsonAsText = File.ReadAllText(pathToConfigInOutDirectory);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonAsText);
        }
    }
}
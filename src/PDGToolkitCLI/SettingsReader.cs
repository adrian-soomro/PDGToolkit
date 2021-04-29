using System.IO;
using Microsoft.Extensions.Configuration;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCLI
{
    public static class SettingsReader
    {
        public static Settings GetSettings()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "config.json"));

            return new Settings(configurationBuilder.Build());
        }
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PDGToolkitAPI.Application;
using PDGToolkitAPI.Application.Serialisers;

namespace PDGToolkitAPI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(InitialiseSettings());
            services.AddTransient<IGenerator, RandomGenerator>();
            services.AddTransient<ISerialiser, JsonSerialiser>();
            services.AddTransient<IFileWriter, FileWriter>();
            services.AddSingleton<IRunner, Runner>();
        }

        private static IConfiguration GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("config.json");

            return configurationBuilder.Build();
        }

        private static Settings InitialiseSettings()
        {
            return new Settings(GetConfiguration());
        }
    }
}
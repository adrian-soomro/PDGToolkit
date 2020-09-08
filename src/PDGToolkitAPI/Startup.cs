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
            services.AddSingleton(GetSettings());
            services.AddTransient<IGenerator, RandomGenerator>();
            services.AddTransient<ISerialiser, JsonSerialiser>();
        }

        private static IConfiguration GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("config.json");

            return configurationBuilder.Build();
        }

        private static Settings GetSettings()
        {
            return new Settings(GetConfiguration());
        }
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PDGToolkitAPI.Application;
using PDGToolkitAPI.Application.Serialisers;

namespace PDGToolkitAPI.Infrastructure
{
    public static class Startup
    {
        public static IRunner InitialiseRunner()
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetService<IRunner>();
        }
        
        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton(InitialiseSettings());
            services.AddTransient<IGenerator, RandomGenerator>();
            services.AddTransient<IGenerator, SmallRoomsGenerator>();
            services.AddTransient<ISerialiser, JsonSerialiser>();
            services.AddTransient<IFileWriter, FileWriter>();
            services.AddSingleton<IRunner, Runner>();
            services.AddTransient<ITileService, TileService>();
            
            return services;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PDGToolkitCore.API;
using PDGToolkitCore.API.Serialisers;
using PDGToolkitCore.Application;

namespace PDGToolkitCore.Infrastructure
{
    public static class Startup
    {
        public static IRunner InitialiseRunner(string generatorName)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            var generators = serviceProvider.GetServices<IGenerator>();

            var selectedGenerator = GeneratorLoader.LoadGenerator(generators, generatorName);
            
            return new Runner(selectedGenerator, serviceProvider.GetService<ISerialiser>(),serviceProvider.GetService<IFileWriter>());
        }
        
        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton(InitialiseSettings());
            services.AddTransient<IGenerator, RandomGenerator>();
            services.AddTransient<IGenerator, TeenyZonesGenerator>();
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
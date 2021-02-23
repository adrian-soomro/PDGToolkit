using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PDGToolkitCore.API;
using PDGToolkitCore.API.Serialisers;
using PDGToolkitCore.Application;
using PDGToolkitCore.Application.PathFinding;

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
            var seed = Guid.NewGuid().GetHashCode();
            Console.Out.WriteLine($"The seed used for this run: {seed}");
            var services = new ServiceCollection();
            services.AddSingleton(InitialiseSettings());
            services.AddTransient<IGenerator, RandomGenerator>();
            services.AddTransient<IGenerator, TeenyZonesGenerator>();
            services.AddTransient<ISerialiser, JsonSerialiser>();
            services.AddTransient<IFileWriter, FileWriter>();
            services.AddSingleton<IRunner, Runner>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<IPathFindingService, AStarPathFindingService>();
            services.AddSingleton(new Random(seed));
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
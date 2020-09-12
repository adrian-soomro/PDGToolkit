using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PDGToolkitAPI.Application;
using PDGToolkitAPI.Application.Serialisers;

namespace PDGToolkitAPI
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(services);
            
            var serviceProvider = services.BuildServiceProvider();
            var runner =  serviceProvider.GetService<IRunner>();
            
            await runner.Run();
        }
    }

    public sealed class Runner : IRunner
    {
        private readonly IGenerator generator;
        private readonly ISerialiser serialiser;
        private readonly IFileWriter writer;
        public Runner(IGenerator generator, ISerialiser serialiser, IFileWriter writer)
        {
            this.generator = generator;
            this.serialiser = serialiser;
            this.writer = writer;
        }

        public async Task Run()
        {
            var grid = await generator.GenerateGridAsync();
            var gridAsJson = serialiser.Serialise(grid);
            //await writer.WriteAsync(gridAsJson);
            Console.WriteLine(gridAsJson);
        }
    }

    public interface IRunner
    {
        Task Run();
    }
}
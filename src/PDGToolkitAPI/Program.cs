using System;
using Microsoft.Extensions.DependencyInjection;
using PDGToolkitAPI.Application;
using PDGToolkitAPI.Application.Serialisers;

namespace PDGToolkitAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var generator = serviceProvider.GetService<IGenerator>();


            var seriliaser = serviceProvider.GetService<ISerialiser>();
            var runner = new Runner(generator, seriliaser);
            
            runner.Run();
        }
        
    }

    public sealed class Runner
    {
        private readonly IGenerator generator;
        private readonly ISerialiser serialiser;
        public Runner(IGenerator generator, ISerialiser serialiser)
        {
            this.generator = generator;
            this.serialiser = serialiser;
        }

        public void Run()
        {
            var grid = generator.GenerateGrid();
            var output = serialiser.Serialise(grid);
            Console.WriteLine(output);
        }
    }
}
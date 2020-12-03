using System;
using System.Threading.Tasks;
using PDGToolkitAPI.API;
using PDGToolkitAPI.API.Serialisers;
using PDGToolkitAPI.Application;

namespace PDGToolkitAPI.Infrastructure
{
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
            await writer.WriteAsync(gridAsJson);
        }
    }
}
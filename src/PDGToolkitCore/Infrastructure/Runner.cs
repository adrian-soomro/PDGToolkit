using System;
using System.Threading.Tasks;
using PDGToolkitCore.API;
using PDGToolkitCore.API.Serialisers;
using PDGToolkitCore.Application;

namespace PDGToolkitCore.Infrastructure
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
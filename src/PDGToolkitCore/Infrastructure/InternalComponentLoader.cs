using System.Collections.Generic;
using System.Linq;
using PDGToolkitCore.API.Serialisers;
using PDGToolkitCore.Application;

namespace PDGToolkitCore.Infrastructure
{
    internal static class InternalComponentLoader
    {
        public static IGenerator LoadGenerator(IEnumerable<IGenerator> generators, string generatorName)
        {
            return generators.First(x => x.GetType().Name == generatorName);
        }
        
        public static ISerialiser LoadSerialiser(IEnumerable<ISerialiser> serialisers, string serialiserName)
        {
            return serialisers.First(x => x.GetType().Name == serialiserName);
        }
    }
}
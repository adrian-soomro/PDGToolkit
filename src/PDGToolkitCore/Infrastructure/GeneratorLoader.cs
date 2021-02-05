using System.Collections.Generic;
using System.Linq;
using PDGToolkitCore.Application;

namespace PDGToolkitCore.Infrastructure
{
    internal static class GeneratorLoader
    {
        public static IGenerator LoadGenerator(IEnumerable<IGenerator> generators, string generatorName)
        {
            return generators.First(x => x.GetType().Name == generatorName);
        }
    }
}
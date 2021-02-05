using System;
using System.Collections.Generic;
using System.Linq;
using PDGToolkitCore.Application;

namespace PDGToolkitCore.API
{
    public static class GeneratorService 
    {
        // source: https://garywoodfine.com/get-c-classes-implementing-interface/
        public static List<string> GetAllGenerators()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(IGenerator).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.Name).ToList();
        }
    }
}
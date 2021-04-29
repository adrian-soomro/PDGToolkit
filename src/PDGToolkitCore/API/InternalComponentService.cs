using System;
using System.Collections.Generic;
using System.Linq;
using PDGToolkitCore.API.Serialisers;
using PDGToolkitCore.Application;

namespace PDGToolkitCore.API
{
    /**
     * Enables callers to enquire about the concrete classes that implement certain interfaces
     */
    public static class InternalComponentService 
    {
        // source: https://garywoodfine.com/get-c-classes-implementing-interface/
        public static List<string> GetAllGenerators()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(IGenerator).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.Name).ToList();
        }
        
        public static List<string> GetAllSerialisers()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(ISerialiser).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.Name).ToList();
        }
    }
}
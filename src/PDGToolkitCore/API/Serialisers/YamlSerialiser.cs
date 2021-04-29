using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PDGToolkitCore.API.Serialisers
{
    public class YamlSerialiser : ISerialiser
    {
        public string Serialise<T>(T input)
        {
            var serialiser = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .DisableAliases()
                .Build();
            return serialiser.Serialize(input); 
        }
    }
}
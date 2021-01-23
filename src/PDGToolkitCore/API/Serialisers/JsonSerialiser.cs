using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PDGToolkitCore.API.Serialisers
{
    public class JsonSerialiser : ISerialiser
    {
        public string Serialise<T>(T input)
        {
            var settings = SetupSettings();
            return input is null ? null : JsonConvert.SerializeObject(input, settings);
        }

        private JsonSerializerSettings SetupSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
        }
    }
}
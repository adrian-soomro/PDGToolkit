using Newtonsoft.Json;

namespace PDGToolkitAPI.Application.Serialisers
{
    public class JsonSerialiser : ISerialiser
    {
        public string Serialise<T>(T input)
        {
            return input is null ? null : JsonConvert.SerializeObject(input);
        }
    }
}
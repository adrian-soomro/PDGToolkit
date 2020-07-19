using Microsoft.Extensions.Configuration;

namespace PDGToolkitAPI
{
    public class Settings
    {
        public string Foo { get; }

        public Settings(IConfiguration config)
        {
            Foo = config.GetValue<string>("foo");
        }
    }
}
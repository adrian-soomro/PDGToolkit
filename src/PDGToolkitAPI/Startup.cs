using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PDGToolkitAPI.foo;

namespace PDGToolkitAPI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(GetSettings());
            services.AddTransient<IBar, Bar>();
        }

        private static IConfiguration GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("config.json");

            return configurationBuilder.Build();
        }

        private static Settings GetSettings()
        {
            return new Settings(GetConfiguration());
        }
    }
}
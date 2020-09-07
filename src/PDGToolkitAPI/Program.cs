using Microsoft.Extensions.DependencyInjection;
using PDGToolkitAPI.foo;

namespace PDGToolkitAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var bar = serviceProvider.GetService<IBar>();
            var runner = new Runner(bar);
            
            runner.Run();
        }
        
    }

    public sealed class Runner
    {
        private readonly IBar bar;

        public Runner(IBar bar)
        {
            this.bar = bar;
        }

        public void Run()
        {
           bar.DoSomething();
        }
    }
}
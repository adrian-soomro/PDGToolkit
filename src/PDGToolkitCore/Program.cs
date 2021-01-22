using System.Threading.Tasks;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCore
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            var runner = Startup.InitialiseRunner();
            await runner.Run();
        }
    }
}
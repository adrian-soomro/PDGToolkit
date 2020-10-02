using System.Threading.Tasks;
using PDGToolkitAPI.Infrastructure;

namespace PDGToolkitAPI
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
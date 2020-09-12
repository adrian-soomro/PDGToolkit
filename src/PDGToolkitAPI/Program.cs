using System.Threading.Tasks;
using PDGToolkitAPI.Infrastructure;

namespace PDGToolkitAPI
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var runner = Startup.InitialiseRunner();
            await runner.Run();
        }
    }
}
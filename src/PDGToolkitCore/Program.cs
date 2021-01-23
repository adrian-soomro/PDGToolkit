using System.Linq;
using System.Threading.Tasks;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCore
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            /*
             * TODO CLI component to parse any arguments given and then start Core, passing in those arguments to initialise runner
             * this way different impl. of ISerialiser & IFileWriter can be loaded based on user's input.
             */
            var generatorName = args.First();
            var runner = Startup.InitialiseRunner(generatorName);
            await runner.Run();
        }
    }
}
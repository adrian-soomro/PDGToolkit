using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCLI
{
    class Program
    {
        static void Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);
        
        [Option(Description = "The generator to load")]
        public string Generator { get; } = "TeenyZonesGenerator";
        
        private async Task OnExecuteAsync()
        {
            var runner = Startup.InitialiseRunner(Generator);
            await runner.Run();
        }
    }
}
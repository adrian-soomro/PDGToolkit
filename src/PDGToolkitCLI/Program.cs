using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

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
            var parsedArgs = new [] {Generator};
            await PDGToolkitCore.Program.Main(parsedArgs); 
        }
    }
}
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
        
        [Option("-p|--path", Description = @"Relative path to where the output file should be stored, 
                                                      including the file's name. Relative to the solution root.")]
        public string PathToOutputFile { get; } = "dungeon.json";
        
        private async Task OnExecuteAsync()
        {
            SettingsHandler.EditOutputRelativePathSetting(PathToOutputFile);
            var runner = Startup.InitialiseRunner(Generator);
            await runner.Run();
        }
    }
}
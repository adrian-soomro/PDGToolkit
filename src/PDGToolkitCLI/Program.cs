using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using PDGToolkitCore.API;
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

        [Option("-l|--list", Description = @"Lists all generators in the toolkit.")]
        public bool ListGenerators { get; }
        
        private async Task OnExecuteAsync()
        {
            if (ListGenerators)
            {
                var generators = GeneratorService.GetAllGenerators();
                await ResponseFormatter.RespondWithCollectionAsync("Currently available generators are:", generators);
                Environment.Exit(0);
            }
            
            SettingsHandler.EditOutputRelativePathSetting(PathToOutputFile);
            var runner = Startup.InitialiseRunner(Generator);
            await runner.Run();
        }
    }
}
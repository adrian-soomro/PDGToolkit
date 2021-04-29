using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using PDGToolkitCLI.Validation;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCLI
{
    class Program
    {
        private static readonly Settings InitialSettings = SettingsReader.GetSettings();
        static void Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        [Option(Description = "The generator to load")]
        [IsValidGeneratorName]
        public string Generator { get; } = InitialSettings.Generator;

        [Option(Description = "The serialiser to use")]
        [IsValidSerialiserName]
        public string Serialiser { get; } = InitialSettings.Serialiser; 
        
        [Option("-p|--path", Description = @"Relative path to where the output file should be stored, 
                                                      including the file's name. Relative to the solution root.")]        
        public string PathToOutputFile { get; } = InitialSettings.RelativePathToOutput;
        
        [Option("-w|--width", Description = @"Sets the width of the generated dungeon.")]
        [Range(1, int.MaxValue)]
        public int DungeonWidth { get; } = InitialSettings.GridSettings.Width;

        [Option("--height", Description = @"Sets the height of the generated dungeon.")]
        [Range(1, int.MaxValue)]
        public int DungeonHeight { get; } = InitialSettings.GridSettings.Height;
        
        private async Task OnExecuteAsync()
        {
            CustomSettingsHandler.Create()
                .SetGenerator(Generator)
                .SetRelativePath(PathToOutputFile)
                .SetWidth(DungeonWidth)
                .SetHeight(DungeonHeight)
                .SetSerialiser(Serialiser)
                .PersistChanges();
   
            var runner = Startup.InitialiseRunner();
            await runner.Run();
        }
    }
}
using System.IO;
using System.Threading.Tasks;
using PDGToolkitCore.Infrastructure;

namespace PDGToolkitCore.API
{
    public class FileWriter : IFileWriter
    {
        private readonly Settings settings;

        public FileWriter(Settings settings)
        {
            this.settings = settings;
        }

        public async Task WriteAsync(string input)
        {
            var projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.Parent?.FullName;
            await File.WriteAllTextAsync(Path.Combine(projectRoot ?? Directory.GetCurrentDirectory(), settings.RelativePathToOutput), input);
        }
    }
}
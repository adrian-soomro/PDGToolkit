using System.IO;
using System.Text.RegularExpressions;
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
            await File.WriteAllTextAsync(settings.RelativePathToOutput.ToAbsolutePathFromPathRelativeToSolutionRoot(), input);
        }
    }
}
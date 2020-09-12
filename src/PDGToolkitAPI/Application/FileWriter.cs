using System.IO;
using System.Threading.Tasks;

namespace PDGToolkitAPI.Application
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
            await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), settings.RelativePathToOutput), input);
        }
    }

    public interface IFileWriter
    {
        Task WriteAsync(string input);
    }
}
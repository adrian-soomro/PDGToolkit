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
            await File.WriteAllTextAsync(Path.Combine(GetApplicationRoot(), settings.RelativePathToOutput), input);
        }
        
        /*
         * Modified version of: http://codebuckets.com/2017/10/19/getting-the-root-directory-path-for-net-core-applications/
         */
        private string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            Regex appPathMatcher= new Regex(@"^(.*?)FYP"); //TODO rename FYP -> PDGToolkit (repo name)
            var appRoot= appPathMatcher.Match(exePath).Value;
            return appRoot;
        }
    }
}
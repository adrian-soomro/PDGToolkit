using System.IO;
using System.Text.RegularExpressions;

namespace PDGToolkitCore.Infrastructure
{
    public static class StringExtensions
    {
        /*
         * Modified version of: http://codebuckets.com/2017/10/19/getting-the-root-directory-path-for-net-core-applications/
         */
        public static string ToAbsolutePathFromPathRelativeToSolutionRoot(this string relativeFilePath)
        {
            var exePath = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            Regex appPathMatcher = new Regex(@"^(.*?)PDGToolkit");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return Path.Combine(appRoot, relativeFilePath);
        }
    }
}
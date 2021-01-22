using System.Threading.Tasks;

namespace PDGToolkitCore.API
{
    public interface IFileWriter
    {
        Task WriteAsync(string input);
    }
}
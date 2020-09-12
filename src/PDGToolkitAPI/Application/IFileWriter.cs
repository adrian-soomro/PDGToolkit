using System.Threading.Tasks;

namespace PDGToolkitAPI.Application
{
    public interface IFileWriter
    {
        Task WriteAsync(string input);
    }
}
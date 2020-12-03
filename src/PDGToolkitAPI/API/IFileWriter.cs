using System.Threading.Tasks;

namespace PDGToolkitAPI.API
{
    public interface IFileWriter
    {
        Task WriteAsync(string input);
    }
}
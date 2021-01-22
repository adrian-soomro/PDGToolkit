using System.Threading.Tasks;
using PDGToolkitCore.Domain.Models;

namespace PDGToolkitCore.Application
{
    public interface IGenerator
    {
        Task<Grid> GenerateGridAsync();
    }
}
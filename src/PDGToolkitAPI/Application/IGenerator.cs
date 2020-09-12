using System.Threading.Tasks;
using PDGToolkitAPI.Domain.Models;

namespace PDGToolkitAPI.Application
{
    public interface IGenerator
    {
        Task<Grid> GenerateGridAsync();
    }
}
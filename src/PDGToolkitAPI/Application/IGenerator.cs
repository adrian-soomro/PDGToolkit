using PDGToolkitAPI.Domain.Models;

namespace PDGToolkitAPI.Application
{
    public interface IGenerator
    {
        Grid GenerateGrid();
    }
}
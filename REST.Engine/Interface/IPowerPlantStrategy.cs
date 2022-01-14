using System.Threading.Tasks;
using REST.Engine.Entities;

namespace REST.Engine.Interface
{
    public interface IPowerPlantStrategy
    {
        Task<Combination> ExecuteProcess(PayLoad payLoad);
    }
}
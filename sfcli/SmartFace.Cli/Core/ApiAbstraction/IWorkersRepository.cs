using System.Threading.Tasks;
using SmartFace.Cli.ApiAbstraction.Models;

namespace SmartFace.Cli.ApiAbstraction
{
    public interface IWorkersRepository
    {
        Task<WorkerModel> EnableWorker(long workerId);

        Task<WorkerModel> DisableWorker(long workerId);
    }
}
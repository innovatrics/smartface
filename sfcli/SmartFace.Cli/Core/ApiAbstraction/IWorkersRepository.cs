using System.Threading.Tasks;
using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Core.ApiAbstraction
{
    public interface IWorkersRepository
    {
        Task<WorkerModel> EnableWorker(long workerId);

        Task<WorkerModel> DisableWorker(long workerId);
    }
}
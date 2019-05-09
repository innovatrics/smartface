using System.Threading.Tasks;
using SmartFace.Cli.ApiAbstraction;
using SmartFace.Cli.ApiAbstraction.Models;
using SmartFace.ODataClient.Action;
using SmartFace.ODataClient.Default;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class WorkersRepository : IWorkersRepository
    {
        private Container Container { get; }
        
        public WorkersRepository(Container container)
        {
            Container = container;
        }

        public async Task<WorkerModel> EnableWorker(long workerId)
        {
            var worker = await Container.Workers.EnableWorker(workerId).GetValueAsync();
            return worker.ToDomainModel();
        }

        public async Task<WorkerModel> DisableWorker(long workerId)
        {
            var worker = await Container.Workers.DisableWorker(workerId).GetValueAsync();
            return worker.ToDomainModel();
        }
    }
}
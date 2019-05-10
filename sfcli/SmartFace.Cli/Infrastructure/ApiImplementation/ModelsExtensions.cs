using SmartFace.Cli.ApiAbstraction.Models;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public static class ModelsExtensions
    {
        public static WorkerModel ToDomainModel(this Worker worker)
        {
            if (worker == null)
            {
                return null;
            }
            return new WorkerModel
            {
                Id = worker.Id,
                Enabled = worker.Enabled
            };
        }
    }
}
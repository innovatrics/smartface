using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.ODataClient.SmartFace.Domain.DataAccess.Models.Core;

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
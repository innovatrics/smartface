using SmartFace.Cli.ApiAbstraction.Models;
using SmartFace.Cli.ApiAbstraction.Models.Configs;

namespace SmartFace.Cli.ApiAbstraction
{
    public interface IStreamWorkerConfigRepository
    {
        StreamWorkerConfigModel Get(long streamWorkerId);

        void Set(long streamWorkerId, StreamWorkerConfigModel configModel);
    }
}
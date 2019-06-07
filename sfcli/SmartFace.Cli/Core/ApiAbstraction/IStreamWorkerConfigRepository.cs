using SmartFace.Cli.Core.ApiAbstraction.Models.Configs;

namespace SmartFace.Cli.Core.ApiAbstraction
{
    public interface IStreamWorkerConfigRepository
    {
        StreamWorkerConfigModel Get(long streamWorkerId);

        void Set(long streamWorkerId, StreamWorkerConfigModel configModel);
    }
}
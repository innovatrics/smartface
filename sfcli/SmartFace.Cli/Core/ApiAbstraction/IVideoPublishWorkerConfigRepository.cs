using SmartFace.Cli.ApiAbstraction.Models;
using SmartFace.Cli.ApiAbstraction.Models.Configs;

namespace SmartFace.Cli.ApiAbstraction
{
    public interface IVideoPublishWorkerConfigRepository
    {
        VideoPublishWorkerConfigModel Get(long videoPublishWorkerId);

        void Set(long videoPublishWorkerId, VideoPublishWorkerConfigModel configModel);
    }
}
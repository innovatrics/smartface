using SmartFace.Cli.Core.ApiAbstraction.Models.Configs;

namespace SmartFace.Cli.Core.ApiAbstraction
{
    public interface IVideoPublishWorkerConfigRepository
    {
        VideoPublishWorkerConfigModel Get(long videoPublishWorkerId);

        void Set(long videoPublishWorkerId, VideoPublishWorkerConfigModel configModel);
    }
}
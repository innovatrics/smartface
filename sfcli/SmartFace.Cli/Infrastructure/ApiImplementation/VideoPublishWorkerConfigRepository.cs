using SmartFace.Cli.ApiAbstraction;
using SmartFace.Cli.ApiAbstraction.Models.Configs;
using SmartFace.Cli.Common;
using SmartFace.Cli.Infrastructure.ApiClient.Extensions;
using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.Extensions;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class VideoPublishWorkerConfigRepository : IVideoPublishWorkerConfigRepository
    {
        private Container Container { get; }

        private const string VideoSource = "VideoSource";

        public VideoPublishWorkerConfigRepository(Container container)
        {
            Container = container;
        }

        public VideoPublishWorkerConfigModel Get(long videoWorkerId)
        {
            var videoConfig = Container.Configs.GetConfigValuesEx(Constants.ConfigName_VideoPublishWorkerConfig, $"{videoWorkerId}");
            return new VideoPublishWorkerConfigModel
            {
                VideoSource = (string)videoConfig[VideoSource]
            };
        }

        public void Set(long videoWorkerId, VideoPublishWorkerConfigModel configModel)
        {
            if (configModel.VideoSource != null)
            {
                Container.Configs.SetConfigValueEx(Constants.ConfigName_VideoPublishWorkerConfig, $"{videoWorkerId}", VideoSource, configModel.VideoSource);
            }
        }
    }
}
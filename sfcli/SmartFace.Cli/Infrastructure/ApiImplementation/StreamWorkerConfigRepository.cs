using SmartFace.Cli.ApiAbstraction;
using SmartFace.Cli.ApiAbstraction.Models.Configs;
using SmartFace.Cli.Common;
using SmartFace.Cli.Infrastructure.ApiClient.Extensions;
using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.Extensions;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class StreamWorkerConfigRepository : IStreamWorkerConfigRepository
    {
        private Container Container { get; }

        private const string TrackMaxEyeDistance = "TrackMaxEyeDistance";
        private const string TrackMinEyeDistance = "TrackMinEyeDistance";
        private const string MjpegVisualizerPort = "MJPEGPreviewPort";
        private const string FaceDiscoveryFrequence = "FaceDiscoveryFrequence";

        public StreamWorkerConfigRepository(Container container)
        {
            Container = container;
        }

        public StreamWorkerConfigModel Get(long streamWorkerId)
        {
            var streamConfig = Container.Configs.GetConfigValuesEx(Constants.ConfigName_StreamWorkerConfig, $"{streamWorkerId}");

            return new StreamWorkerConfigModel
            {
                TrackMaxEyeDistance = (long)streamConfig[TrackMaxEyeDistance],
                TrackMinEyeDistance = (long)streamConfig[TrackMinEyeDistance],
                FaceDiscoveryFrequence = (long)streamConfig[FaceDiscoveryFrequence],
                MjpegPreviewPort = (long)streamConfig[MjpegVisualizerPort],
            };
        }

        public void Set(long streamWorkerId, StreamWorkerConfigModel configModel)
        {
            SetStreamWorkerConfig(streamWorkerId, TrackMinEyeDistance, configModel.TrackMinEyeDistance);
            SetStreamWorkerConfig(streamWorkerId, TrackMaxEyeDistance, configModel.TrackMaxEyeDistance);
            SetStreamWorkerConfig(streamWorkerId, FaceDiscoveryFrequence, configModel.FaceDiscoveryFrequence);
            SetStreamWorkerConfig(streamWorkerId, MjpegVisualizerPort, configModel.MjpegPreviewPort);
        }
        private void SetStreamWorkerConfig(long streamWorkerId, string propertyName, long? propertyValue)
        {
            if (propertyValue.HasValue)
            {
                Container.Configs.SetConfigValueEx(
                    Constants.ConfigName_StreamWorkerConfig,
                    $"{streamWorkerId}",
                    propertyName,
                    propertyValue);
            }
        }
    }
}
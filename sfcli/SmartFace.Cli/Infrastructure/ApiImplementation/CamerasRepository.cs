using SmartFace.Cli.Core.ApiAbstraction;
using SmartFace.Cli.Core.ApiAbstraction.Models;
using SmartFace.Cli.Infrastructure.ApiClient.Extensions;
using SmartFace.ODataClient.Default;
using SmartFace.ODataClient.Extensions;
using SmartFace.ODataClient.SmartFace.WebApi.Models.Requests;

namespace SmartFace.Cli.Infrastructure.ApiImplementation
{
    public class CamerasRepository: ICamerasRepository
    {
        private Container Container { get; }
        
        public CamerasRepository(Container container)
        {
            Container = container;
        }
        
        public CameraModel Create(CreateCameraModel createCameraModel)
        {
            var streamSourceData = StreamSourceData.CreateStreamSourceData(createCameraModel.ScopeId, createCameraModel.VideoSource, false);
            var camera = Container.Cameras.Create(streamSourceData);
            return new CameraModel
            {
                StreamId = camera.StreamId.Value
            };
        }
    }
}
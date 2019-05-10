using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.OData.Client;
using SmartFace.Cli.ApiAbstraction.Models;
using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.ApiAbstraction
{
    public interface IApiProvider
    {
        IWorkersRepository Workers { get; }

        IStreamsRepository Streams { get; }

        IScopesRepository Scopes { get; }

        ICamerasRepository Cameras { get; }

        IStreamWorkerConfigRepository StreamWorkerConfigs { get; }
        
        IVideoPublishWorkerConfigRepository VideoPublishWorkerConfigs { get; }

        IFaceHandlerConfigRepository FaceHandlerConfigs { get; }
        
        IIFaceConfigRepository IFaceConfigs { get; }

        void ClearTrackingEntities();
    }

}
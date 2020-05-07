using SmartFace.Cli.Core.ApiAbstraction.Models;

namespace SmartFace.Cli.Core.ApiAbstraction
{
    public interface IApiProvider
    {
        IWorkersRepository Workers { get; }

        IStreamsRepository Streams { get; }

        IScopesRepository Scopes { get; }

        ICamerasRepository Cameras { get; }

        IStreamWorkerConfigRepository StreamWorkerConfigs { get; }
        
        IVideoPublishWorkerConfigRepository VideoPublishWorkerConfigs { get; }

        void ClearTrackingEntities();
    }

}